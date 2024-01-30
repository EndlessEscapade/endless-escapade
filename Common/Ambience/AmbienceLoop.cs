using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ReLogic.Utilities;
using Terraria.Audio;

namespace EndlessEscapade.Common.Ambience;

public struct AmbienceLoop : IAmbienceTrack
{
    private float volume = 0f;

    public float Volume {
        get => volume;
        set => volume = MathHelper.Clamp(value, 0f, 1f);
    }
    
    public SlotId SlotId { get; set; } = SlotId.Invalid;

    [JsonRequired]
    public SoundStyle Style { get; set; } = default;

    [JsonRequired]
    public string[] Flags { get; set; } = Array.Empty<string>();

    public AmbienceLoop() { }
}
