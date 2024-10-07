using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Utilities;
using Terraria.Audio;

namespace EndlessEscapade.Core.Audio;

[Autoload(Side = ModSide.Client)]
public sealed class AudioSystem : ModSystem
{
    private static readonly SoundStyle[] IgnoredSounds = [
        SoundID.MenuClose,
        SoundID.MenuOpen,
        SoundID.MenuTick,
        SoundID.Chat,
        SoundID.Grab
    ];

    private static readonly List<ActiveSound> Sounds = [];
    private static readonly List<AudioModifier> Modifiers = [];

    private static AudioParameters parameters;

    public override void Load() {
        base.Load();

        On_SoundPlayer.Play_Inner += PlayInnerHook;
    }

    public static void AddModifier(string identifier, int duration, AudioModifier.ModifierCallback callback) {
        var index = Modifiers.FindIndex(modifier => modifier.Identifier == identifier);

        if (index == -1) {
            Modifiers.Add(new AudioModifier(identifier, duration, callback));
            return;
        }

        var modifier = Modifiers[index];

        modifier.TimeLeft = Math.Max(modifier.TimeLeft, duration);
        modifier.TimeMax = Math.Max(modifier.TimeMax, duration);
        modifier.Callback = callback;

        Modifiers[index] = modifier;
    }

    public override void PostUpdateEverything() {
        base.PostUpdateEverything();

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

            modifier.TimeLeft--;

            if (modifier.TimeLeft <= 0) {
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
                Sounds.RemoveAt(i);

                i--;
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
        SoundUpdateCallback updateCallback
    ) {
        var slot = orig(self, ref style, position, updateCallback);

        var isSoundIgnored = false;

        foreach (var ignoredStyle in IgnoredSounds) {
            if (style == ignoredStyle) {
                isSoundIgnored = true;
                break;
            }
        }

        var isSoundActive = SoundEngine.TryGetActiveSound(slot, out var sound);
        var isSoundDisposed = sound?.Sound?.IsDisposed == true;

        if (isSoundActive && isSoundActive && !isSoundDisposed) {
            Sounds.Add(sound);
        }

        return slot;
    }
}
