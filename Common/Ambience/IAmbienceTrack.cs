using ReLogic.Utilities;
using Terraria.Audio;

namespace EndlessEscapade.Common.Ambience;

public interface IAmbienceTrack
{
    SlotId SlotId { get; }

    SoundStyle Style { get; }

    float Volume { get; }

    string[] Flags { get; }
}
