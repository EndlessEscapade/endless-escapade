/*
 * Implementation taken and inspired by
 * https://github.com/Mirsario/TerrariaOverhaul/tree/dev/Core/AudioEffects
 */

using System;
using System.Collections.Generic;
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
    private static readonly ImmutableArray<SoundStyle> ignoredSoundStyles = ImmutableArray.Create(SoundID.MenuClose,
        SoundID.MenuOpen,
        SoundID.MenuTick,
        SoundID.Chat,
        SoundID.Grab);

    private static readonly List<AudioModifier> modifiers = new();

    private static FieldInfo trackedSoundsField;
    private static AudioParameters parameters;

    public override void OnModLoad() {
        if (!SoundEngine.IsAudioSupported) {
            Mod.Logger.Warn($"Disabled '{nameof(AudioSystem)}'! {nameof(SoundEngine)}.{nameof(SoundEngine.IsAudioSupported)} returned false.");
            return;
        }

        trackedSoundsField = typeof(SoundPlayer).GetField("_trackedSounds", BindingFlags.Instance | BindingFlags.NonPublic);

        if (trackedSoundsField == null) {
            Mod.Logger.Warn($"Disabled '{nameof(AudioSystem)}'! Missing internal Terraria members.");
            return;
        }

        On_SoundEngine.PlaySound_refSoundStyle_Nullable1_SoundUpdateCallback += SoundEnginePlayHook;
    }

    public override void PostUpdateEverything() {
        UpdateParameters();
        UpdateSounds();
    }

    public static void AddModifier(int time, string identifier, AudioModifier.ModifierDelegate function) {
        var existingIndex = modifiers.FindIndex(modifier => modifier.Id == identifier);

        if (existingIndex < 0) {
            modifiers.Add(new AudioModifier(identifier, time, function));
            return;
        }

        var modifier = modifiers[existingIndex];

        modifier.TimeLeft = Math.Max(modifier.TimeLeft, time);
        modifier.TimeMax = Math.Max(modifier.TimeMax, time);
        modifier.Modifier = function;

        modifiers[existingIndex] = modifier;
    }

    private static void ApplyParameters(SoundEffectInstance instance, in AudioParameters parameters) {
        if (instance?.IsDisposed == true) {
            return;
        }

        var filters = ModContent.GetContent<IAudioFilter>();

        foreach (var filter in filters) {
            filter.Apply(instance, in parameters);
        }
    }

    private static void UpdateParameters() {
        var newParameters = new AudioParameters();

        for (var i = 0; i < modifiers.Count; i++) {
            var modifier = modifiers[i];
            var intensity = modifier.TimeLeft / (float)modifier.TimeMax;

            modifier.Modifier(intensity, ref newParameters);

            if (--modifier.TimeLeft <= 0) {
                modifiers.RemoveAt(i--);
                continue;
            }

            modifiers[i] = modifier;
        }

        parameters = newParameters;
    }

    private static void UpdateSounds() {
        var trackedSounds = (SlotVector<ActiveSound>)trackedSoundsField.GetValue(SoundEngine.SoundPlayer)!;

        foreach (var item in trackedSounds) {
            var sound = item.Value;
            var instance = sound.Sound;

            if (ignoredSoundStyles.Contains(sound.Style) || instance?.IsDisposed == true) {
                continue;
            }

            ApplyParameters(instance, parameters);
        }
    }

    private static SlotId SoundEnginePlayHook(On_SoundEngine.orig_PlaySound_refSoundStyle_Nullable1_SoundUpdateCallback orig, ref SoundStyle style, Vector2? position, SoundUpdateCallback callback) {
        var slot = orig(ref style, position, callback);

        if (ignoredSoundStyles.Contains(style) || !SoundEngine.TryGetActiveSound(slot, out var result) || result.Sound?.IsDisposed == false) {
            return slot;
        }

        ApplyParameters(result.Sound, parameters);

        return slot;
    }
}
