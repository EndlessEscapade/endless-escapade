using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;

namespace EndlessEscapade.Common.Systems.Ambience.Loops;

public sealed class SubmergedLoop : AmbienceLoop
{
    protected override void Initialize() {
        Style = new SoundStyle($"{nameof(EndlessEscapade)}/Assets/Sounds/Ambience/Water/Submerged", SoundType.Ambient) { IsLooped = true };
    }

    protected override bool Active(Player player) {
        var headPosition = player.Center - new Vector2(0f, 16f);

        return Collision.WetCollision(headPosition, 10, 10);
    }
}
