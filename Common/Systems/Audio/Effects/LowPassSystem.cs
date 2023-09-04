using System;
using System.Reflection;
using System.Runtime.InteropServices;
using EndlessEscapade.Common.Configs;
using EndlessEscapade.Utilities;
using FAudioINTERNAL;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.Audio.Effects;

[Autoload(Side = ModSide.Client)]
public class LowPassSystem : ModSystem
{
    private static readonly Action<SoundEffectInstance, float> lowPassAction = typeof(SoundEffectInstance)
        .GetMethod("INTERNAL_applyLowPassFilter", ReflectionUtils.PrivateInstanceFlags)
        .CreateDelegate<Action<SoundEffectInstance, float>>();

    private static readonly FieldInfo cueHandleInstanceField = typeof(Cue).GetField("handle", ReflectionUtils.PrivateInstanceFlags);

    public static bool Enabled { get; private set; }

    public override void Load() {
        Enabled = false;

        if (!SoundEngine.IsAudioSupported) {
            Mod.Logger.Error("Audio effects were disabled: Sound engine does not support audio.");
            return;
        }

        if (lowPassAction == null || cueHandleInstanceField == null) {
            Mod.Logger.Error("Audio effects were disabled: Could not find internal Terraria/FNA objects.");
            return;
        }

        Enabled = true;
    }

    public static void ApplyParameters(SoundEffectInstance instance, AudioParameters parameters) {
        if (!Enabled || !ModContent.GetInstance<AudioConfig>().EnableLowPassFiltering) {
            return;
        }

        lowPassAction.Invoke(instance, 1f - parameters.LowPass * 0.99f);
    }

    public static unsafe void ApplyParameters(Cue cue, AudioParameters parameters) {
        if (!Enabled || !ModContent.GetInstance<AudioConfig>().EnableLowPassFiltering) {
            return;
        }

        var handle = (nint)cueHandleInstanceField.GetValue(cue);
        var cuePtr = (FACTCue*)handle;

        var filterParameters = new FAudio.FAudioFilterParameters {
            Frequency = 1f - parameters.LowPass * 0.99f,
            OneOverQ = 1,
            Type = FAudio.FAudioFilterType.FAudioLowPassFilter
        };

        if (cuePtr->simpleWave != null && cuePtr->simpleWave->voice != null) {
            var voice = cuePtr->simpleWave->voice;

            FAudio.FAudioVoice_SetFilterParameters((nint)voice, ref filterParameters, 0u);
        }
        else if (cuePtr->playingSound != null) {
            var factSound = cuePtr->playingSound->sound;
            var count = factSound->trackCount;

            for (var i = 0; i < count; i++) {
                ref var sound = ref cuePtr->playingSound;
                ref var tracks = ref sound->tracks[i];
                ref var wave1 = ref tracks.activeWave;

                var wave2 = wave1.wave;

                if (wave2 == null) {
                    continue;
                }

                var voice = wave2->voice;

                Marshal.ThrowExceptionForHR((int)FAudio.FAudioVoice_SetFilterParameters((nint)voice, ref filterParameters, 0u));
            }
        }
    }
}
