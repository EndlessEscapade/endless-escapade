/*
 * Inspiration taken from
 * https://github.com/Mirsario/TerrariaOverhaul
 */

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ReLogic.Utilities;
using Terraria.Audio;

namespace EndlessEscapade.Common.Ambience;

public sealed class AmbienceTrack
{
    private float volume;

    public SlotId Slot;

    [JsonRequired]
    public SoundStyle Style;

    [JsonRequired]
    public string[] Flags;

    public float Volume {
        get => volume;
        set => volume = MathHelper.Clamp(value, 0f, 1f);
    }
}
