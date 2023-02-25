using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace EndlessEscapade.Common.Audio.Ambience.Tracks;

public class OceanAmbience : AmbienceTrack
{
    protected override bool Condition => Main.LocalPlayer.HasItem(ItemID.Meowmere);

    protected override void Initialize() {
        LoopSoundStyle = new SoundStyle($"{nameof(EndlessEscapade)}/Assets/Sounds/Ambience/Loops/Waves", SoundType.Ambient) {
            PlayOnlyIfFocused = true,
            IsLooped = true
        };
    }
}