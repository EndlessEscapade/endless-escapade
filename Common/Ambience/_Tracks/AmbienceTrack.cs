using Newtonsoft.Json;
using Terraria.Audio;

namespace EndlessEscapade.Common.Ambience;

public struct AmbienceTrack
{
    [JsonRequired]
    public SoundStyle Style;
}
