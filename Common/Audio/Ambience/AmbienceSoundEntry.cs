using Terraria.Audio;

namespace EndlessEscapade.Common.Ambience;

public readonly record struct AmbienceSoundEntry(string Path, int PlaybackChance)
{
    public readonly SoundStyle SoundStyle = new(Path, SoundType.Ambient) {
        PlayOnlyIfFocused = true
    };
}