using EEMod.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using EEMod.Items.Placeables.Walls;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Extensions;
using System;

namespace EEMod.Tiles.Walls
{
    public class KelpForestLeafyWall : ModWall
    {
        public override void SetDefaults()
        {
            AddMapEntry(new Color(67, 47, 155));

            Main.wallHouse[Type] = true;
            dustType = 154;
            drop = ModContent.ItemType<MagmastoneSlabWall>();
            soundStyle = 1;
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
         /*   Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Vector2 pos = new Vector2(i * 16, j * 16);
            for (int a = 0; a < 4; a++)
            { 
                for (int b = 0; b < 4; b++)
                {
                    float sine = (float)Math.Sin(((a + b) / 16f) * 6.28f);
                    float funi = (float)Math.Sin(Main.GameUpdateCount / (float)45f + sine*4 + i + j) *(0.8f + sine*0.3f);
                    float randParam = (i * j + a * b * 9)%7 + funi;
                    float randParam2 = (i * j + a * b * 11) % 7 + funi;
                    Vector2 positionAdd = new Vector2(a * 4 + randParam*3 - 8 + sine, b * 4 + randParam2*3 - 8 + sine);
                    float rotation = 0.5f * funi + i + j;
                    Texture2D tex = ModContent.GetInstance<EEMod>().GetTexture("Particles/ForegroundParticles/Leaf");
                    if(a*a*b*b % 7 + i *j % 8 < 4)
                        tex = ModContent.GetInstance<EEMod>().GetTexture("Particles/ForegroundParticles/Leaf2");
                    Helpers.Draw(tex, pos.ForDraw() + zero + positionAdd, Lighting.GetColor(i, j).MultiplyRGB(new Color(funi*0.3f + 1, funi * 0.3f + 1,funi * 0.3f + 1)),1 + funi*0.05f, default, rotation);
                }
            }*/
            return false;
        }
    }
}