using EEMod.Extensions;
using EEMod.Items.Placeables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Tiles
{
    public class LightGemsandTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;

            AddMapEntry(new Color(88, 179, 179));

            dustType = 154;
            drop = ModContent.ItemType<LightGemsand>();
            soundStyle = 1;
            mineResist = 1f;
            minPick = 0;
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Color drawColour = Lighting.GetColor(i, j);
            if (!Main.tileSolid[Framing.GetTileSafely(i, j - 1).type] || !Framing.GetTileSafely(i, j - 1).active() && Framing.GetTileSafely(i, j).slope() == 0)
            {
                int noOfGrassBlades = (int)(((i + j) % 16) * 0.5f);
                Texture2D tex = EEMod.instance.GetTexture("Tiles/KelpGrassLong");
                Texture2D tex2 = EEMod.instance.GetTexture("Tiles/KelpGrassMedium");
                Texture2D tex3 = EEMod.instance.GetTexture("Tiles/KelpGrassShort");
                Texture2D tex4 = EEMod.instance.GetTexture("Tiles/KelpGrassStubbed");
                Texture2D Chosen = tex;
                for (int a = 0; a < noOfGrassBlades; a++)
                {
                    float sprout = (float)Math.Sin(Main.time / 60f + i + j + a) * 0.2f;

                    bool cond = ((i + j * a * 3) % 2 == 0);
                    switch ((i + j + a * 3) % 4)
                    {
                        case 0:
                            Chosen = tex;
                            break;
                        case 1:
                            Chosen = tex2;
                            break;
                        case 2:
                            Chosen = tex3;
                            break;
                        case 3:
                            Chosen = tex4;
                            break;
                    }
                    Vector2 position = new Vector2((i + 12) * 16 + (i + j * a + a * 4) % 16, (j + 12) * 16 + 2).ForDraw() - new Vector2((cond ? 0 : Chosen.Width), 0);
                    spriteBatch.Draw(Chosen, position, Chosen.Bounds, drawColour.MultiplyRGB(Color.Lerp(Color.LightGreen, Color.Green, ((i + j + a * 3) % 4)/4f)), sprout, new Vector2(0, Chosen.Height), 1f, cond ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);


                }
            }
            return true;
        }
    }
}