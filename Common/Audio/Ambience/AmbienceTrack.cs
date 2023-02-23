using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Content;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Ambience;

public abstract class AmbienceTrack : ModType
{
    protected readonly List<AmbienceSoundEntry> soundEntries = new();

    private SoundStyle soundStyle;
    // TODO: Change to use multiple SoundStyle s (Layered ambience).
    private SoundEffectInstance soundInstance;

    public abstract string Path { get; }
    public abstract bool Active { get; }

    // TODO: Change to universal client config.
    public virtual float FadeIn { get; } = 0.005f;
    public virtual float FadeOut { get; } = 0.005f;

    SlotId soundSlot;
    public virtual void Setup() {
        soundStyle = new(Path, SoundType.Ambient) {
            PlayOnlyIfFocused = true,
            IsLooped = true
        };
        soundSlot = SoundEngine.PlaySound(null);
    }

    public virtual void Update() {
        PlayMainLoop();
        PlayAmbienceSounds();
    }

    protected sealed override void Register() {
        ModTypeLookup<AmbienceTrack>.Register(this);
    }

    protected void AddAmbienceSound(string path, int playbackChance) {
        AmbienceSoundEntry entry = new(path, playbackChance);
        soundEntries.Add(entry);
    }

    bool TryGetActiveSound(out ActiveSound result, bool tryPlaying = false) {
        if (!soundSlot.IsValid || !SoundEngine.TryGetActiveSound(soundSlot, out result) && tryPlaying) {
            TryReplay();
            return SoundEngine.TryGetActiveSound(soundSlot, out result);
        }
        return result != null;
    }
    bool TryReplay() {
        if (soundSlot.IsValid && SoundEngine.TryGetActiveSound(soundSlot, out ActiveSound activeSound)) { // theres an existing sound instance
            SoundEffectInstance s = activeSound.Sound;
            if (s != null && s.State != SoundState.Stopped) {
                activeSound.Stop();
            }
        }
        soundSlot = SoundEngine.PlaySound(in soundStyle);
        Main.NewText("played new sound");
        return soundSlot.IsValid;
    }
    float volumeScale;
    private void PlayMainLoop() {

        if (Active) {
            volumeScale += FadeIn;
        }
        else if (!Active) {
            volumeScale -= FadeOut;
        }

        volumeScale = MathHelper.Clamp(volumeScale, 0f, 1f);

        float minVolume = 0;
        bool valid = TryGetActiveSound(out ActiveSound activeSound, tryPlaying: false);
        if (volumeScale <= minVolume) {
            // try to pause, nothing to hear
            if (valid) {
                activeSound.Volume *= 0;
                if (activeSound.IsPlaying)
                    activeSound.Pause();
            }
            return;
        }
        else { // can still play
            if (Main.hasFocus && !Main.gameMenu) {
                if (!valid || !activeSound.IsPlaying && activeSound.Sound?.State is null or SoundState.Stopped) { // sound finished 
                    valid = TryGetActiveSound(out activeSound, tryPlaying: true);
                }
                else if (activeSound.Sound.State == SoundState.Paused) { // not finished but just paused
                    activeSound.Resume();
                }
            }
            else {
                if (activeSound.IsPlaying) {
                    activeSound.Pause();
                }
            }

            if (valid && activeSound.IsPlaying) {
                activeSound.Update();
                activeSound.Sound.Volume *= volumeScale;
            }
        }
    }

    private void PlayAmbienceSounds() {
        if (!Active) {
            return;
        }

        foreach (AmbienceSoundEntry entry in soundEntries) {
            if (!Main.rand.NextBool(entry.PlaybackChance)) {
                continue;
            }

            SoundEngine.PlaySound(entry.SoundStyle);
        }
    }
}