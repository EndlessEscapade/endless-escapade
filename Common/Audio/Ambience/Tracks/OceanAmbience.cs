using Terraria;

namespace EndlessEscapade.Common.Ambience.Tracks;

public class ShipyardAmbience : AmbienceTrack
{
    public override bool Active => Main.LocalPlayer.ZoneBeach && Main.LocalPlayer.position.X / 16f < Main.maxTilesX / 2f;

    public override string Path { get; } = $"{nameof(EndlessEscapade)}/Assets/Sounds/Ambience/Loops/Waves";
}