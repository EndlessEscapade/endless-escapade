using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Items.Placeables;

namespace EEMod.Tiles.Walls
{
    public class EEGlobalWall : GlobalWall
    {
        public override void ModifyLight(int i, int j, int type, ref float r, ref float g, ref float b)
        {
            if (Main.ActiveWorldFileData.Name == KeyID.Sea)
            {
                if (Main.dayTime)
                {
                    if (Main.time <= 1000)
                    {
                        r += 0.0005f;
                        g += 0.0005f;
                        b += 0.0005f;
                    }
                    if (Main.time >= 53000)
                    {
                        r -= 0.0005f;
                        g -= 0.0005f;
                        b -= 0.0005f;
                    }
                    if (Main.time > 1000 && Main.time < 53000)
                    {
                        r = 0.5f;
                        g = 0.5f;
                        b = 0.5f;
                    }
                }
                else
                {
                    r = 0;
                    g = 0;
                    b = 0;
                }
                //r = MathHelper.Clamp(r, 0, 1);
                //g = MathHelper.Clamp(g, 0, 1);
                //b = MathHelper.Clamp(b, 0, 1);
            }
        }
    }
}
