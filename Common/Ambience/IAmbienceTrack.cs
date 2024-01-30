using Terraria.Audio;

namespace EndlessEscapade.Common.Ambience;

public interface IAmbienceTrack
{
    float Volume { get; }

    SoundStyle Style { get; }
    
    string[] Flags { get; }
}
