using System;
using System.Collections.Generic;
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
    private static readonly SoundStyle[] IgnoredSoundStyles = {
        SoundID.MenuClose,
        SoundID.MenuOpen,
        SoundID.MenuTick,
        SoundID.Chat,
        SoundID.Grab
    };

    private static readonly List<ActiveSound> Sounds = new();
    private static readonly List<AudioModifier> Modifiers = new();

    private static AudioParameters parameters;

    public override void Load() {
        if (!SoundEngine.IsAudioSupported) {
            Mod.Logger.Warn($"{nameof(AudioSystem)} was disabled: {nameof(SoundEngine)}.{nameof(SoundEngine.IsAudioSupported)} was false.");
            return;
        }

        On_SoundPlayer.Play_Inner += PlayInnerHook;
    }

    public static void AddModifier(string context, int duration, AudioModifier.ModifierCallback callback) {
        var index = Modifiers.FindIndex(modifier => modifier.Context == context);

        if (index == -1) {
            Modifiers.Add(new AudioModifier(context, duration, callback));
            return;
        }

        var modifier = Modifiers[index];

        modifier.TimeLeft = Math.Max(modifier.TimeLeft, duration);
        modifier.TimeMax = Math.Max(modifier.TimeMax, duration);
        modifier.Callback = callback;

        Modifiers[index] = modifier;
    }

    public override void PostUpdateEverything() {
        UpdateModifiers();
        UpdateSounds();
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

    private static void UpdateModifiers() {
        var newParameters = new AudioParameters();

        for (var i = 0; i < Modifiers.Count; i++) {
            var modifier = Modifiers[i];

            if (modifier.TimeLeft-- <= 0) {
                Modifiers.RemoveAt(i--);
                continue;
            }

            modifier.Callback(ref newParameters, modifier.TimeLeft / (float)modifier.TimeMax);

            Modifiers[i] = modifier;
        }

        parameters = newParameters;
    }

    private static void UpdateSounds() {
        for (var i = 0; i < Sounds.Count; i++) {
            var sound = Sounds[i];

            if (!sound.IsPlaying) {
                Sounds.RemoveAt(i--);
                continue;
            }

            ApplyParameters(sound.Sound, parameters);
        }
    }

    private static SlotId PlayInnerHook(
        On_SoundPlayer.orig_Play_Inner orig,
        SoundPlayer self,
        ref SoundStyle style,
        Vector2? position,
        SoundUpdateCallback updateCallback) {
        var slot = orig(self, ref style, position, updateCallback);

        var hasIgnoredStyle = false;

        foreach (var ignoredStyle in IgnoredSoundStyles) {
            if (style == ignoredStyle) {
                hasIgnoredStyle = true;
                break;
            }
        }

        if (SoundEngine.TryGetActiveSound(slot, out var sound) && sound.Sound?.IsDisposed == false && !hasIgnoredStyle) {
            Sounds.Add(sound);
        }

        return slot;
    }
}
