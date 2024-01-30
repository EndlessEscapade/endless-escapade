using Terraria.Audio;

namespace EndlessEscapade.Common.Ambience;

public interface IAmbienceTrack
{
    SoundStyle Style { get; }
    
    float Volume { get; }
    
    string[] Flags { get; }
}
