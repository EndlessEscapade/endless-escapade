using EndlessEscapade.Content.Biomes;
using Terraria;
using Terraria.Audio;

namespace EndlessEscapade.Common.Systems.Audio.Ambience.Tracks;

public class BeachTrack : AmbienceTrack
{
    protected override void Initialize() {
        Loop = new SoundStyle($"{nameof(EndlessEscapade)}/Assets/Sounds/Ambience/Beach/Waves") { IsLooped = true };
        Sounds = new SoundStyle($"{nameof(EndlessEscapade)}/Assets/Sounds/Ambience/Beach/BeachDefault", 3, SoundType.Ambient) { PlayOnlyIfFocused = true };
    }

    protected override bool IsActive(Player player) {
        return player.ZoneBeach || player.InModBiome<ShipyardBiome>();
    }
}
