using System.Collections.Immutable;
using System.Reflection;
using EndlessEscapade.Common.Systems.Audio.Filters;
using EndlessEscapade.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Utilities;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.Audio;

[Autoload(Side = ModSide.Client)]
public sealed class SoundSystem : ModSystem
{
    private static readonly FieldInfo trackedSoundsField = typeof(SoundPlayer).GetField("_trackedSounds", ReflectionUtils.PrivateInstanceFlags)!;

    public static readonly ImmutableArray<SoundStyle> IgnoredSounds = ImmutableArray.Create(
        SoundID.MenuClose,
        SoundID.MenuOpen,
        SoundID.MenuTick,
        SoundID.Chat,
        SoundID.Grab
    );

    public static bool Enabled { get; private set; }

    public static SoundModifiers SoundParameters { get; private set; }

    public override void OnModLoad() {
        Enabled = SoundEngine.IsAudioSupported;

        if (!Enabled) {
            return;
        }

        On_SoundEngine.PlaySound_refSoundStyle_Nullable1_SoundUpdateCallback += SoundEnginePlayHook;
    }

    public override void PostUpdateEverything() {
        UpdateSounds();
        ResetParameters();
    }

    internal static void SetParameters(in SoundModifiers sound) {
        SoundParameters = sound;
    }

    internal static void ResetParameters() {
        SoundParameters = new SoundModifiers();
    }

    internal static void ApplyParameters(SoundEffectInstance instance, in SoundModifiers parameters) {
        if (!Enabled || instance?.IsDisposed == true) {
            return;
        }

        LowPassSystem.ApplyParameters(instance, parameters);
    }

    private static void UpdateSounds() {
        var value = (SlotVector<ActiveSound>)trackedSoundsField.GetValue(SoundEngine.SoundPlayer)!;

        foreach (var item in value) {
            var sound = item.Value;
            var instance = sound.Sound;

            if (IgnoredSounds.Contains(sound.Style) || instance?.IsDisposed == true) {
                continue;
            }

            ApplyParameters(instance, SoundParameters);
        }
    }

    private static SlotId SoundEnginePlayHook(On_SoundEngine.orig_PlaySound_refSoundStyle_Nullable1_SoundUpdateCallback orig, ref SoundStyle style, Vector2? position, SoundUpdateCallback callback) {
        var slot = orig(ref style, position, callback);

        if (IgnoredSounds.Contains(style) || !SoundEngine.TryGetActiveSound(slot, out var result) || result.Sound?.IsDisposed == false) {
            return slot;
        }

        ApplyParameters(result.Sound, SoundParameters);

        return slot;
    }
}
