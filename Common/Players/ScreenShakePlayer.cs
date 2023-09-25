using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Players;

public class ScreenShakePlayer : ModPlayer
{
    public float ScreenShake { get; set; }

    public override void ModifyScreenPosition() {
        if (ScreenShake > 0.1f) {
            Main.screenPosition += new Vector2(Main.rand.NextFloat(ScreenShake), Main.rand.NextFloat(ScreenShake));
            ScreenShake *= 0.9f;
        }
    }
}
