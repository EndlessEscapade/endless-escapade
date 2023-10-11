using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;

namespace EndlessEscapade.Common.Systems.Ambience;

public abstract class AmbienceLoop : AmbienceTrack
{
    private float volume;

    public float Volume {
        get => volume;
        private set => volume = MathHelper.Clamp(value, 0f, Main.ambientVolume);
    }

    internal sealed override void Update() {
        UpdateVolume();
        UpdateLoop();
    }

    private void UpdateVolume() {
        var active = Active(Main.LocalPlayer);

        if (active) {
            Volume += 0.025f;
            return;
        }

        Volume -= 0.025f;
    }

    private void UpdateLoop() {
        var active = Active(Main.LocalPlayer);
        var exists = SoundEngine.TryGetActiveSound(SoundSlot, out var sound);

        if (active && (!exists || sound == null)) {
            SoundSlot = SoundEngine.PlaySound(Style);

            if (SoundEngine.TryGetActiveSound(SoundSlot, out sound)) {
                sound.Volume = 0f;
            }
        }

        if (exists && sound != null) {
            if (!active && Volume <= 0f) {
                sound.Stop();
                SoundSlot = SlotId.Invalid;
                return;
            }

            sound.Volume = Volume;
        }
    }
}
