using EndlessEscapade.Content.Biomes;
using Terraria;
using Terraria.Audio;

namespace EndlessEscapade.Common.Audio.Ambience.Sounds;

public sealed class DolphinSounds : AmbienceSound
{
    protected override void Initialize() {
        Style = new SoundStyle($"{nameof(EndlessEscapade)}/Assets/Sounds/Ambience/Beach/BeachDolphins", SoundType.Ambient) {
            PitchVariance = 0.25f
        };
    }

    protected override bool Active(Player player) {
        return player.ZoneBeach || player.InModBiome<Shipyard>();
    }
}
