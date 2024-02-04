using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

    private static List<ActiveSound> sounds = new();
    private static List<AudioModifier> modifiers = new();

    private static AudioParameters parameters;

    public override void Load() {
        if (!SoundEngine.IsAudioSupported) {
            Mod.Logger.Warn($"{nameof(AudioSystem)} was disabled: {nameof(SoundEngine)}.{nameof(SoundEngine.IsAudioSupported)} was false.");
            return;
        }

        On_SoundPlayer.Play_Inner += PlayInnerHook;
    }

    public override void Unload() {
        sounds?.Clear();
        sounds = null;
        
        modifiers?.Clear();
        modifiers = null;
    }

    public static void AddModifier(string context, int duration, AudioModifier.ModifierCallback callback) {
        var index = modifiers.FindIndex(modifier => modifier.Context == context);

        if (index == -1) {
            modifiers.Add(new AudioModifier(context, duration, callback));
            return;
        }

        var modifier = modifiers[index];

        modifier.TimeLeft = Math.Max(modifier.TimeLeft, duration);
        modifier.TimeMax = Math.Max(modifier.TimeMax, duration);
        modifier.Callback = callback;

        modifiers[index] = modifier;
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

        for (var i = 0; i < modifiers.Count; i++) {
            var modifier = modifiers[i];

            if (modifier.TimeLeft-- <= 0) {
                modifiers.RemoveAt(i--);
                continue;
            }

            modifier.Callback(ref newParameters, modifier.TimeLeft / (float)modifier.TimeMax);

            modifiers[i] = modifier;
        }

        parameters = newParameters;
    }

    private static void UpdateSounds() {
        for (var i = 0; i < sounds.Count; i++) {
            var sound = sounds[i];

            if (!sound.IsPlaying) {
                sounds.RemoveAt(i--);
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
            sounds.Add(sound);
        }

        return slot;
    }
}
