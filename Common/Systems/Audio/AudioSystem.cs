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

namespace EndlessEscapade.Common.Systems.Audio;

[Autoload(Side = ModSide.Client)]
public class AudioSystem : ModSystem
{
    // TODO: Handle missing.
    private static readonly FieldInfo cueInstanceField = typeof(CueAudioTrack).GetField("_cue", ReflectionUtils.PrivateInstanceFlags)!;
    private static readonly FieldInfo trackedSoundsField = typeof(SoundPlayer).GetField("_trackedSounds", ReflectionUtils.PrivateInstanceFlags)!;
    private static readonly FieldInfo soundInstanceField = typeof(ASoundEffectBasedAudioTrack).GetField("_soundEffectInstance", ReflectionUtils.PrivateInstanceFlags)!;

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
        SoundParameters = new AudioParameters();
        MusicParameters = new AudioParameters();
    }

    public static void ApplyParameters(SoundEffectInstance instance, AudioParameters parameters) {
        LowPassSystem.ApplyParameters(instance, parameters);
    }

    public static void ApplyParameters(Cue instance, AudioParameters parameters) {
        LowPassSystem.ApplyParameters(instance, parameters);
    }

    public override bool IsLoadingEnabled(Mod mod) {
        return SoundEngine.IsAudioSupported;
    }

    public override void Load() {
        if (trackedSoundsField == null || soundInstanceField == null || cueInstanceField == null) {
            Mod.Logger.Error("Audio effects were disabled: Could not find internal Terraria/FNA objects.");
            return;
        }

        On_SoundEngine.PlaySound_refSoundStyle_Nullable1_SoundUpdateCallback += SoundEnginePlayHook;
        On_ASoundEffectBasedAudioTrack.Play += AudioTrackPlayHook;
        On_CueAudioTrack.Play += CueAudioTrackPlayHook;
    }

    public override void PostUpdateEverything() {
        UpdateSounds();
        UpdateMusic();
        
        ResetParameters();
    }

    private static void UpdateSounds() {
        var value = (SlotVector<ActiveSound>)trackedSoundsField.GetValue(SoundEngine.SoundPlayer)!;

        foreach (var item in value) {
            var sound = item.Value;
            var instance = sound.Sound;

            if (IgnoredSounds.Contains(sound.Style) || !sound.IsPlaying || instance?.IsDisposed is not false) {
                continue;
            }

            ApplyParameters(instance, SoundParameters);
        }
    }

    private static void UpdateMusic() {
        if (Main.audioSystem is not LegacyAudioSystem system) {
            return;
        }

        for (var i = 0; i < system.AudioTracks.Length; i++) {
            var track = system.AudioTracks[i];
            
            if (track is CueAudioTrack cueTrack) {
                var cue = (Cue)cueInstanceField.GetValue(cueTrack);
                
                ApplyParameters(cue, MusicParameters);
            }

            if (track is ASoundEffectBasedAudioTrack audioTrack) {
                var instance = (DynamicSoundEffectInstance)soundInstanceField.GetValue(audioTrack);

                ApplyParameters(instance, MusicParameters);
            }
        }
    }

    private static SlotId SoundEnginePlayHook(On_SoundEngine.orig_PlaySound_refSoundStyle_Nullable1_SoundUpdateCallback orig, ref SoundStyle style, Vector2? position, SoundUpdateCallback callback) {
        var slot = orig(ref style, position, callback);

        if (IgnoredSounds.Contains(style) || !SoundEngine.TryGetActiveSound(slot, out var result) || result.Sound?.IsDisposed is not false) {
            return slot;
        }

        ApplyParameters(result.Sound, SoundParameters);

        return slot;
    }

    private static void AudioTrackPlayHook(On_ASoundEffectBasedAudioTrack.orig_Play orig, ASoundEffectBasedAudioTrack self) {
        orig(self);

        var instance = (DynamicSoundEffectInstance)soundInstanceField.GetValue(self);

        ApplyParameters(instance, MusicParameters);
    }
    
    private static void CueAudioTrackPlayHook(On_CueAudioTrack.orig_Play orig, CueAudioTrack self) {
        orig(self);

        var cue = (Cue)cueInstanceField.GetValue(self);

        ApplyParameters(cue, MusicParameters);
    }
}
