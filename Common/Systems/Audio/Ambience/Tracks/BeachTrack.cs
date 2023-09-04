namespace EndlessEscapade.Common.Systems.Audio.Ambience.Tracks;

public class BeachTrack : AmbienceTrack
{
    protected override void Initialize() {
        RegisterSound("Assets/Sounds/Ambience/Beach/Dolphins", 100);
        RegisterSound("Assets/Sounds/Ambience/Beach/Seagulls", 100, 2);
    }
}
