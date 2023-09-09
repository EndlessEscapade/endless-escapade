using System.Collections.Generic;
using EndlessEscapade.Content.Biomes;
using Terraria;
using Terraria.Audio;

namespace EndlessEscapade.Common.Systems.Audio.Ambience.Tracks;

public class BeachTrack : AmbienceTrack
{
    private static readonly SoundStyle loop = new SoundStyle($"{nameof(EndlessEscapade)}/Assets/Sounds/Ambience/Beach/Waves", SoundType.Ambient) { IsLooped = true };

    private static readonly SoundStyle commonSounds = new SoundStyle($"{nameof(EndlessEscapade)}/Assets/Sounds/Ambience/Beach/Common", 3, SoundType.Ambient);
    
    protected override void Initialize() {
        Loops = new List<AmbienceSoundData>() { new AmbienceSoundData(loop) };

        Sounds = new List<AmbienceSoundData>() { new AmbienceSoundData(commonSounds) };
    }

    protected override bool IsActive(Player player) {
        return !player.wet && (player.ZoneBeach || player.InModBiome<ShipyardBiome>());
    }
}
