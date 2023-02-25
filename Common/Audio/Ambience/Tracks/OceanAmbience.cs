using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace EndlessEscapade.Common.Ambience.Tracks;

public class OceanAmbience : AmbienceTrack
{
    public override bool Condition => Main.LocalPlayer.HasItem(ItemID.Meowmere);

    public override void Initialize() {
        LoopSoundStyle = new SoundStyle($"{nameof(EndlessEscapade)}/Assets/Sounds/Ambience/Loops/Waves", SoundType.Ambient) {
            PlayOnlyIfFocused = true,
            IsLooped = true
        };
    }
}