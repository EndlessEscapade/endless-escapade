using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.Audio;

namespace EndlessEscapade.Utilities;

public static class AudioUtils
{
    public static void UpdateSoundLoop(ref SlotId slot, in SoundStyle style, float volume, Vector2? position = null) {
        var exists = SoundEngine.TryGetActiveSound(slot, out var sound);

        if (volume > 0f && (!exists || sound == null)) {
            slot = SoundEngine.PlaySound(in style, position);

            if (!SoundEngine.TryGetActiveSound(slot, out sound)) {
                return;
            }

            sound.Volume = 0f;
        }
        else if (exists && sound != null) {
            if (volume <= 0f) {
                sound.Stop();
                slot = SlotId.Invalid;
                return;
            }

            sound.Volume = volume;
        }
    }
}
