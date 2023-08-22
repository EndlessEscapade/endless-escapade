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
    private static readonly Action<SoundEffectInstance, float> lowPassAction;
    private static readonly FieldInfo cueHandleInstanceField = typeof(Cue).GetField("handle", ReflectionUtils.PrivateInstanceFlags)!;

    static LowPassSystem() {
        var type = typeof(SoundEffectInstance);
        var method = type.GetMethod("INTERNAL_applyLowPassFilter", ReflectionUtils.PrivateInstanceFlags);

        lowPassAction = method.CreateDelegate<Action<SoundEffectInstance, float>>();
    }

    public override bool IsLoadingEnabled(Mod mod) {
        var config = ModContent.GetInstance<AudioConfig>();

        return SoundEngine.IsAudioSupported && config.EnableLowPassFiltering;
    }

    public static void ApplyParameters(SoundEffectInstance instance, AudioParameters parameters) {
        var intensity = 1f - parameters.LowPass * 0.9f;

        lowPassAction.Invoke(instance, intensity);
    }

    internal static unsafe void ApplyParameters(Cue cue, AudioParameters parameters) {
        nint handle = (nint)cueHandleInstanceField.GetValue(cue)!;

        FACTCue* cuePtr = (FACTCue*)handle;

        FAudio.FAudioFilterParameters filterParameters = new() {
            Frequency = 1f - parameters.LowPass * 0.9f,
            OneOverQ = 1,
            Type = FAudio.FAudioFilterType.FAudioLowPassFilter
        };
        if (cuePtr->simpleWave != null && cuePtr->simpleWave->voice != null) {
            FAudioVoice* voice = cuePtr->simpleWave->voice;
            //FAudio.FAudioVoice_GetVoiceDetails((nint)voice, out FAudio.FAudioVoiceDetails details);
            FAudio.FAudioVoice_SetFilterParameters((nint)voice, ref filterParameters, 0u);
        }
        else if (cuePtr->playingSound != null) {
            FACTSound* factSound = cuePtr->playingSound->sound;
            int count = factSound->trackCount;
            for (int i = 0; i < count; i++) {
                ref var sound = ref cuePtr->playingSound;
                ref var tracks = ref sound->tracks[i];
                ref var wave1 = ref tracks.activeWave;
                FACTWave* wave2 = wave1.wave;
                if (wave2 == null) // seems like there's a data race where this may ocasionally be null
                    continue; 
                FAudioVoice* voice = wave2->voice;
                Marshal.ThrowExceptionForHR((int)FAudio.FAudioVoice_SetFilterParameters((nint)voice, ref filterParameters, 0u));
            }
        }
        //if (cuePtr->simpleWave != null && cuePtr->simpleWave->voice != null) {
        //    FAudioVoice* voice = cuePtr->simpleWave->voice;

        //    FAudio.FAudioVoice_SetFilterParameters((nint)voice, ref filterParameters, 0);
        //}
        //else if (cuePtr->playingSound != null && cuePtr->playingSound->sound != null) {
        //    FACTSound* factsound = cuePtr->playingSound->sound;
        //    int count = factsound->trackCount;
        //    for (int i = 0; i < count; i++) {
        //        ref var sound = ref cuePtr->playingSound;
        //        ref var tracks = ref sound->tracks[i];
        //        ref var wave1 = ref tracks.activeWave;
        //        FACTWave* wave2 = wave1.wave;
        //        FAudioVoice* voice = wave2->voice;
        //        Marshal.ThrowExceptionForHR((int)FAudio.FAudioVoice_SetFilterParameters((nint)voice, ref filterParameters, 0u));
        //    }
        //}
    }

}
