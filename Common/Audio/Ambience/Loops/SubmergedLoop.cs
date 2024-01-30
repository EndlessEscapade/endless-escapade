using EndlessEscapade.Utilities.Extensions;
using Terraria;
using Terraria.Audio;

namespace EndlessEscapade.Common.Ambience.Loops;

public sealed class SubmergedLoop : AmbienceLoop
{
    protected override void Initialize() {
        Style = new SoundStyle($"{nameof(EndlessEscapade)}/Assets/Sounds/Ambience/WaterSubmergedLoop", SoundType.Ambient) {
            IsLooped = true
        };
    }

    protected override bool Active(Player player) {
        return player.IsDrowning();
    }
}
