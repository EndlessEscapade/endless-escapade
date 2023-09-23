
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Players
{
    public class ScreenShakePlayer : ModPlayer
    {
        public float screenShake;

        public override void ModifyScreenPosition() {
            if(screenShake > 0.1f) {
                Main.screenPosition += new Vector2(Main.rand.NextFloat(screenShake), Main.rand.NextFloat(screenShake));
                screenShake *= 0.9f;

            }
        }
    }
}
