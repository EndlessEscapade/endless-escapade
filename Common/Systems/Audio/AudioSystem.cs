using System.Collections.Immutable;
using System.Reflection;
using EndlessEscapade.Common.Systems.Audio.Filters;
using EndlessEscapade.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;

using FAudioINTERNAL;
using System.Runtime.InteropServices;
using System.Threading;

namespace EndlessEscapade.Common.Systems.Audio;

[Autoload(Side = ModSide.Client)]
public class AudioSystem : ModSystem
{
    private static readonly FieldInfo trackedSoundsField = typeof(SoundPlayer).GetField("_trackedSounds", ReflectionUtils.PrivateInstanceFlags);

    public static readonly ImmutableArray<SoundStyle> IgnoredSounds = ImmutableArray.Create(
        SoundID.MenuClose,
        SoundID.MenuOpen,
        SoundID.MenuTick,
        SoundID.Chat,
        SoundID.Grab
    );

    public static AudioParameters SoundParameters { get; private set; }
    public static AudioParameters MusicParameters { get; private set; }

    public static void SetParameters(AudioParameters sound, AudioParameters music) {
        SoundParameters = sound;
        MusicParameters = music;
    }

    public static void ResetParameters() {
        SoundParameters = default;
        MusicParameters = default;
    }

    public override bool IsLoadingEnabled(Mod mod) {
        return SoundEngine.IsAudioSupported;
    }

    public override void PostUpdateEverything() {
        MusicParameters = new AudioParameters() {
            LowPass = 1f
        };
    }

    public override void Load() {
        On_SoundEngine.PlaySound_refSoundStyle_Nullable1_SoundUpdateCallback += SoundEnginePlayHook;
        On_SoundEngine.Update += SoundEngineUpdateHook;

        IL_LegacyAudioSystem.UpdateCommonTrack += IL_ApplyFrecuencyAfterStarting;

        //Console.WriteLine($"XNA visualization: {FAudio.XNA_VisualizationEnabled()}");
        //FAudio.FACTCue_GetProperties()
    }

    public override void PostUpdateDusts() {
        base.PostUpdateDusts();
    }

    private unsafe void IL_ApplyFrecuencyAfterStarting(MonoMod.Cil.ILContext il) {
        ILCursor c = new(il);
        c.TryGotoNext(i => i.MatchCallvirt<IAudioTrack>("Play"));
        c.Emit(OpCodes.Dup);
        c.Index++;
        c.EmitDelegate((IAudioTrack track) => {
            Console.WriteLine(track.GetType());

            // smaller frequency = more high pitched part
            // 
            // 1 <------------- 0 - value
            //         sound
            // low <--------- high  
            FAudio.FAudioFilterParameters parameters = new() {
                Type = FAudio.FAudioFilterType.FAudioHighPassFilter,
                Frequency = 0.3f, // <------------------ frequency
                OneOverQ = 1
            };
            if (track is CueAudioTrack audiotrack) {
                ReflectionUtils.GetField(audiotrack, "_cue", out Cue cue);
                ReflectionUtils.GetField(cue, "handle", out nint handle);

                Thread.Sleep(100); // looks like there's a data race 

                FACTCue* cuePtr = (FACTCue*)(void*)handle;

                // debug print variable names
                /*char** names = cuePtr->parentBank->parentEngine->variableNames;
                int varCount = cuePtr->parentBank->parentEngine->variableCount;
                float* variableValue = cuePtr->variableValues;
                for (int i = 0; i < varCount; i++) {
                    string variableName = new string((sbyte*)names[i]);
                    Console.WriteLine($"{variableName}: {variableValue[i]} -- {FAudio.FACTCue_GetVariableIndex((nint)cuePtr, variableName)} - {i}");
                }
                Console.WriteLine("----------------------------");*/

                if (cuePtr->simpleWave != null) {
                    FAudioVoice* voice = cuePtr->simpleWave->voice;
                    //FAudio.FAudioVoice_GetVoiceDetails((nint)voice, out FAudio.FAudioVoiceDetails details);

                    FAudio.FAudioVoice_SetFilterParameters((nint)voice, ref parameters, 0u);
                }
                else if (cuePtr->playingSound != null) {
                    FACTSound* factSound = cuePtr->playingSound->sound;
                    int count = factSound->trackCount;
                    for (int i = 0; i < count; i++) {
                        ref var sound = ref cuePtr->playingSound;
                        ref var tracks = ref sound->tracks[i];
                        ref var wave1 = ref tracks.activeWave;
                        FACTWave* wave2 = wave1.wave;
                        FAudioVoice* voice = wave2->voice;

                        Marshal.ThrowExceptionForHR((int)FAudio.FAudioVoice_SetFilterParameters((nint)voice, ref parameters, 0u));

                    }
                }


            }

            return;
        });
    }



    private static SlotId SoundEnginePlayHook(
        On_SoundEngine.orig_PlaySound_refSoundStyle_Nullable1_SoundUpdateCallback orig,
        ref SoundStyle style,
        Vector2? position,
        SoundUpdateCallback updatecallback
    ) {
        var slot = orig(ref style, position, updatecallback);

        if (IgnoredSounds.Contains(style) || !SoundEngine.TryGetActiveSound(slot, out ActiveSound result) || result.Sound.IsDisposed) {
            return slot;
        }

        LowPassSystem.ApplyEffects(result.Sound, SoundParameters);

        return slot;
    }

    private static void SoundEngineUpdateHook(On_SoundEngine.orig_Update orig) {
        orig();

        var value = (SlotVector<ActiveSound>)trackedSoundsField.GetValue(SoundEngine.SoundPlayer);

        foreach (var item in value) {
            var sound = item.Value;
            var instance = sound.Sound;

            if (IgnoredSounds.Contains(sound.Style) || !sound.IsPlaying || instance.IsDisposed) {
                continue;
            }

            LowPassSystem.ApplyEffects(instance, SoundParameters);
        }
    }
}
