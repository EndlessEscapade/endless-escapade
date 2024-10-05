using Terraria.Audio;

namespace EndlessEscapade.Common.Ambience;

public interface IAmbienceSound : ILoadable
{
    SoundStyle Sound { get; }

    int Chance { get; }

    string[] Signals { get; }

    void ILoadable.Load(Mod mod) { }

    void ILoadable.Unload() { }
}
