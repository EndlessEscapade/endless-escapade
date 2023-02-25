using System.Diagnostics.CodeAnalysis;
using EndlessEscapade.Common.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Audio.Ambience;

[Autoload(Side = ModSide.Client)]
public abstract class AmbienceTrack : ModType
{
    protected SoundStyle LoopSoundStyle { get; set; }
    protected SlotId LoopSoundSlot { get; set; }

    protected float Volume { get; set; }
    protected float VolumeFadeRate { get; set; } = 0.005f;

    protected abstract bool Condition { get; }

    protected abstract void Initialize();

    protected sealed override void Register() {
        Initialize();

        ModTypeLookup<AmbienceTrack>.Register(this);
    }

    public sealed override bool IsLoadingEnabled(Mod mod) {
        return SoundEngine.IsAudioSupported && ClientSideConfig.Instance.ToggleAmbience;
    }

    public void Update() {
        UpdateTargetVolume();
        UpdateLoop();
    }

    private void UpdateLoop() {
        ActiveSound? activeSound = null;
        bool isPlaying = TryGetActiveSound(out activeSound, false);

        if (!isPlaying && Condition) {
            isPlaying = TryGetActiveSound(out activeSound, true);
        }
        else if (isPlaying) {
            ref float realVolume = ref activeSound.Volume;

            if (realVolume <= 0f) {
                activeSound.Stop();
                return;
            }

            realVolume = Volume;
        }
    }

    private void UpdateTargetVolume() {
        if (Condition) {
            Volume += VolumeFadeRate;
        }
        else if (!Condition) {
            Volume -= VolumeFadeRate;
        }

        Volume = MathHelper.Clamp(Volume, 0f, Main.ambientVolume);
    }

    private bool TryReplaySound() {
        if (LoopSoundSlot.IsValid && SoundEngine.TryGetActiveSound(LoopSoundSlot, out ActiveSound? activeSound)) {
            SoundEffectInstance instance = activeSound.Sound;

            if (instance != null && instance.State != SoundState.Stopped) {
                activeSound.Stop();
            }
        }

        LoopSoundSlot = SoundEngine.PlaySound(LoopSoundStyle);

        return LoopSoundSlot.IsValid;
    }

    private bool TryGetActiveSound([NotNullWhen(true)] out ActiveSound? result, bool tryPlaying) {
        if (!LoopSoundSlot.IsValid || (!SoundEngine.TryGetActiveSound(LoopSoundSlot, out result) && tryPlaying)) {
            TryReplaySound();

            return SoundEngine.TryGetActiveSound(LoopSoundSlot, out result);
        }

        return result != null;
    }
}