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
        void PlaceGroundGrass(int i, int j)
        {
            int noOfGrassBlades = (int)(((i + j) % 16) * 0.1f);
            string tex = "Tiles/Foliage/KelpGrassLong";
            string tex3 = "Tiles/Foliage/KelpGrassShort";
            string tex4 = "Tiles/Foliage/KelpGrassStubbed";
            string tex5 = "Tiles/Foliage/KelpGrassLongX";
            string tex6 = "Tiles/Foliage/KelpGrassLongXX";
            string tex2 = "Tiles/Foliage/KelpGrassMedium";
            string Chosen = tex;

            for (int a = 0; a < noOfGrassBlades; a++)
            {
                switch ((i + j + a * 7) % 6)
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
                    case 4:
                        Chosen = tex5;
                        break;
                    case 5:
                        Chosen = tex6;
                        break;
                }
                float pos = i * 16 + (i + j * a + a * 7) % 16;
                if ((i + j * a * 2) % 2 != 0)
                    ModContent.GetInstance<EEMod>().TVH.AddElement(new Leaf(new Vector2(pos, j * 16), Chosen, 0f, Color.Lerp(Color.LightGreen, Color.Green, ((i + j + a * 3) % 4) / 4f), false));
                else
                {
                    ModContent.GetInstance<EEMod>().TVH.AddElement(new Leaf(new Vector2(pos - EEMod.instance.GetTexture(Chosen).Width, j * 16), Chosen, 0f, Color.Lerp(Color.LightGreen, Color.Green, ((i + j + a * 3) % 4) / 4f), true));
                }
            }
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (!Main.tileSolid[Framing.GetTileSafely(i, j - 1).type] || !Framing.GetTileSafely(i, j - 1).active() && Framing.GetTileSafely(i, j).slope() == 0 && !Framing.GetTileSafely(i, j).halfBrick())
            {
                PlaceGroundGrass(i, j);
            }
            return true;
        }
    }
}