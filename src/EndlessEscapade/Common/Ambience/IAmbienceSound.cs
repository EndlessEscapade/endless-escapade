using Terraria.Audio;

namespace EndlessEscapade.Common.Ambience;

public interface IAmbienceSound : ILoadable
{
    /// <summary>
    ///     The sound style used by this ambience sound.
    /// </summary>
    SoundStyle Sound { get; }

    /// <summary>
    ///     The signals required for this ambience sound to be played.
    /// </summary>
    string[] Signals { get; }

    /// <summary>
    ///     The chance of this ambience sound playing.
    /// </summary>
    int Chance { get; }

    void ILoadable.Load(Mod mod) { }

    void ILoadable.Unload() { }
}
