namespace EndlessEscapade.Common.Ambience.Tracks;

public class ShipyardAmbience : AmbienceTrack
{
    public override bool Active { get; } = true;

    public override string Path { get; } = $"{nameof(EndlessEscapade)}/Assets/Sounds/Ambience/SentinelPrime";
}