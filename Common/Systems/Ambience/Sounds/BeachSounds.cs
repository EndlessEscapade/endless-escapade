using EndlessEscapade.Content.Biomes;
using Terraria;
using Terraria.Audio;

namespace EndlessEscapade.Common.Systems.Ambience.Sounds;

public sealed class BeachSounds : AmbienceSound
{
    protected override void Initialize() {
        Style = new SoundStyle($"{nameof(EndlessEscapade)}/Assets/Sounds/Ambience/Beach/Beach", 3, SoundType.Ambient) { PitchVariance = 0.25f };
    }

    protected override bool Active(Player player) {
        return player.ZoneBeach || player.InModBiome<ShipyardBiome>();
    }
}
