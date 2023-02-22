using System;
using Terraria;
using Terraria.Audio;

namespace EndlessEscapade.Common.Ambience.Tracks;

public class ShipyardAmbience : AmbienceTrack
{
    public override bool Active => Main.LocalPlayer.ZoneBeach && Main.LocalPlayer.position.X / 16f < Main.maxTilesX / 2f;

    public override string Path { get; } = $"{nameof(EndlessEscapade)}/Assets/Sounds/Ambience/Loops/Waves";

    public override void Setup() {
        base.Setup();

        AddAmbienceSound($"{nameof(EndlessEscapade)}/Assets/Sounds/Ambience/Dolphins", 1000);
        
        AddAmbienceSound($"{nameof(EndlessEscapade)}/Assets/Sounds/Ambience/Seagulls1", 1000);
        AddAmbienceSound($"{nameof(EndlessEscapade)}/Assets/Sounds/Ambience/Seagulls1", 1000);
    }
}