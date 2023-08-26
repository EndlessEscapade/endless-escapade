using System;
using System.Reflection;
using System.Runtime.InteropServices;
using EndlessEscapade.Common.Config;
using EndlessEscapade.Utilities;
using FAudioINTERNAL;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Systems.Audio.Filters;

[Autoload(Side = ModSide.Client)]
public class LowPassSystem : ModSystem
{
    private static readonly Action<SoundEffectInstance, float> lowPassAction
        = typeof(SoundEffectInstance).GetMethod("INTERNAL_applyLowPassFilter", ReflectionUtils.PrivateInstanceFlags).CreateDelegate<Action<SoundEffectInstance, float>>();

    private static readonly FieldInfo cueHandleInstanceField = typeof(Cue).GetField("handle", ReflectionUtils.PrivateInstanceFlags);

    public override bool IsLoadingEnabled(Mod mod) {
        var config = ModContent.GetInstance<AudioConfig>();

        return SoundEngine.IsAudioSupported && config.EnableLowPassFiltering;
    }

    public static void ApplyParameters(SoundEffectInstance instance, AudioParameters parameters) {
        if (!ModContent.GetInstance<AudioConfig>().EnableLowPassFiltering)
            return;
        var intensity = 1f - parameters.LowPass * 0.9f;

        lowPassAction.Invoke(instance, intensity);
    }

    public static unsafe void ApplyParameters(Cue cue, AudioParameters parameters) {
        if (!ModContent.GetInstance<AudioConfig>().EnableLowPassFiltering)
            return;
        var handle = (nint)cueHandleInstanceField.GetValue(cue)!;
        var cuePtr = (FACTCue*)handle;

        var filterParameters = new FAudio.FAudioFilterParameters { Frequency = 1f - parameters.LowPass * 0.9f, OneOverQ = 1, Type = FAudio.FAudioFilterType.FAudioLowPassFilter };

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
