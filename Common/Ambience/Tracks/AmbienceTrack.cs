using Newtonsoft.Json;
using Terraria.Audio;

namespace EndlessEscapade.Common.Ambience.Tracks;

public struct AmbienceTrack
{
    [JsonRequired]
    public SoundStyle Style;

    [JsonRequired]
    public string[] Flags;
}
