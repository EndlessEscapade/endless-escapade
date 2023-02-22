using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Ambience;

public abstract class AmbienceTrack : ModType
{
    protected readonly List<AmbienceSoundEntry> soundEntries = new();

    // TODO: Change to use multiple SoundStyle s (Layered ambience).
    private SoundEffectInstance soundInstance;

    public abstract string Path { get; }
    public abstract bool Active { get; }

    // TODO: Change to universal client config.
    public virtual float FadeIn { get; } = 0.005f;
    public virtual float FadeOut { get; } = 0.005f;

    public virtual void Setup() {
        soundInstance = ModContent.Request<SoundEffect>(Path, AssetRequestMode.ImmediateLoad).Value.CreateInstance();
        soundInstance.IsLooped = true;
        soundInstance.Volume = 0f;
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

    private void PlayMainLoop() {
        if (soundInstance == null) {
            return;
        }
        
        if (Active) {
            soundInstance.Volume += FadeIn;
        }
        else if (!Active) {
            soundInstance.Volume -= FadeOut;
        }

        soundInstance.Volume = MathHelper.Clamp(soundInstance.Volume, 0f, Main.ambientVolume);

        if (Main.hasFocus && !Main.gameMenu) {
            if (soundInstance.State == SoundState.Stopped) {
                soundInstance.Play();
            }
            else if (soundInstance.State == SoundState.Paused) {
                soundInstance.Resume();
            }
        }
        else {
            soundInstance.Pause();
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