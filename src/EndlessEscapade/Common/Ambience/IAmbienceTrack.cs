using ReLogic.Utilities;
using Terraria.Audio;

namespace EndlessEscapade.Common.Ambience;

public interface IAmbienceTrack : ILoadable
{
	SoundStyle Sound { get; }

    string[] Signals { get; }

    float StepIn { get; }

    float StepOut { get; }

    float Volume { get; set; }

    SlotId Slot { get; set; }

    void ILoadable.Load(Mod mod) { }

    void ILoadable.Unload() { }
}
