/*
 * Inspiration taken from
 * https://github.com/Mirsario/TerrariaOverhaul
 */

using System;
using System.Collections.Immutable;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Utilities;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Audio;

[Autoload(Side = ModSide.Client)]
public sealed class AudioSystem : ModSystem
{
    private static readonly FieldInfo trackedSoundsField = typeof(SoundPlayer).GetField("_trackedSounds", BindingFlags.Instance | BindingFlags.NonPublic);
    
    public static readonly ImmutableArray<SoundStyle> IgnoredSounds = ImmutableArray.Create(SoundID.MenuClose,
        SoundID.MenuOpen,
        SoundID.MenuTick,
        SoundID.Chat,
        SoundID.Grab);

    public static AudioModifiers Modifiers { get; private set; }

    public override void OnModLoad() {
        if (trackedSoundsField == null) {
            throw new MissingFieldException(nameof(SoundPlayer), "_trackedSounds");
        }

        On_SoundEngine.PlaySound_refSoundStyle_Nullable1_SoundUpdateCallback += SoundEnginePlayHook;
    }

    public override void PostUpdateEverything() {
        UpdateSounds();

        Modifiers = new AudioModifiers();
    }

    public static void SetParameters(in AudioModifiers audio) {
        Modifiers = audio;
    }

    public static void ApplyParameters(SoundEffectInstance instance, in AudioModifiers parameters) {
        if (instance?.IsDisposed == true) {
            return;
        }

        var filters = ModContent.GetContent<IAudioFilter>();

        foreach (var filter in filters) {
            filter.Apply(instance, in parameters);
        }
    }

    private static void UpdateSounds() {
        var trackedSounds = (SlotVector<ActiveSound>)trackedSoundsField.GetValue(SoundEngine.SoundPlayer)!;

        foreach (var item in trackedSounds) {
            var sound = item.Value;
            var instance = sound.Sound;

            if (IgnoredSounds.Contains(sound.Style) || instance?.IsDisposed == true) {
                continue;
            }

            ApplyParameters(instance, Modifiers);
        }
    }

    private static SlotId SoundEnginePlayHook(On_SoundEngine.orig_PlaySound_refSoundStyle_Nullable1_SoundUpdateCallback orig, ref SoundStyle style, Vector2? position, SoundUpdateCallback callback) {
        var slot = orig(ref style, position, callback);

        if (IgnoredSounds.Contains(style) || !SoundEngine.TryGetActiveSound(slot, out var result) || result.Sound?.IsDisposed == false) {
            return slot;
        }

        ApplyParameters(result.Sound, Modifiers);

        return slot;
    }
}
