using ReLogic.Utilities;
using Terraria.Audio;

namespace EndlessEscapade.Common.Ambience;

public interface IAmbienceTrack : ILoadable
{
	/// <summary>
	///     The sound style used by this ambience track.
	/// </summary>
	SoundStyle Sound { get; }

	/// <summary>
	///     The signals required for this ambience track to be played.
	/// </summary>
    string[] Signals { get; }

	/// <summary>
	///		The step value used for performing volume fade-ins.
	/// </summary>
    float StepIn { get; }

	/// <summary>
	///		The step value used for performing volume fade-outs.
	/// </summary>
    float StepOut { get; }

    /// <summary>
    ///		The current volume of this ambience track.
    /// </summary>
    float Volume { get; set; }

    /// <summary>
    ///		The sound slot that points to the sound instance of this ambience track.
    /// </summary>
    SlotId Slot { get; set; }

    void ILoadable.Load(Mod mod) { }

    void ILoadable.Unload() { }
}
