using EEMod.Extensions;
using EEMod.Items.Placeables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Tiles.EmptyTileArrays;
namespace EEMod.Tiles
{
    public class KelpLeafTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = false;
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;

            AddMapEntry(new Color(40, 200, 0));

            dustType = 154;
            drop = ModContent.ItemType<VolcanicAsh>();
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
                    ModContent.GetInstance<EEMod>().TVH.AddElement(new Leaf(new Vector2(pos - ModContent.GetInstance<EEMod>().GetTexture(Chosen).Width, j * 16), Chosen, 0f, Color.Lerp(Color.LightGreen, Color.Green, ((i + j + a * 3) % 4) / 4f), true));
                }
            }
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            return true;
        }
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
        {
            offsetY = 100000;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (!Main.tileSolid[Framing.GetTileSafely(i, j - 1).type] || !Framing.GetTileSafely(i, j - 1).active() && Framing.GetTileSafely(i, j).slope() == 0 && !Framing.GetTileSafely(i, j).halfBrick() && Main.GameUpdateCount % 10 == 0)
            {
                PlaceGroundGrass(i, j);
            }
            Color color = Lighting.GetColor(i, j);
            int frameX = Main.tile[i, j].frameX;
            int frameY = Main.tile[i, j].frameY;
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Vector2 position = new Vector2(i * 16, j * 16).ForDraw() + zero;
            Texture2D texture = ModContent.GetInstance<EEMod>().GetTexture("Tiles/KelpLeafTile");
            Rectangle rect = new Rectangle(frameX, frameY, 16, 16);
            Point tp = Main.LocalPlayer.position.ToTileCoordinates();
            if (tp.Y > j - 4 && tp.Y < j && tp.X > i - 1 && tp.X < i + 1)
            {
                float scaleY = 1f;
                Main.spriteBatch.Draw(texture, position + new Vector2(0, 16 * (1 - scaleY)), rect, color, 0f, default, new Vector2(1f, scaleY), SpriteEffects.None, 0f);
            }
            else if (tp.Y > j - 4 && tp.Y < j && tp.X > i - 2 && tp.X < i + 2)
            {
                float scaleY = 1.2f;
                Main.spriteBatch.Draw(texture, position + new Vector2(0, 16 * (1 - scaleY)), rect, color, 0f, default, new Vector2(1f, scaleY), SpriteEffects.None, 0f);
            }
            else if(!Framing.GetTileSafely(i,j - 1).active() || !Main.tileSolid[Framing.GetTileSafely(i, j - 1).type])
            {
                float scaleY = 1.35f;
                Main.spriteBatch.Draw(texture, position + new Vector2(0, 16 * (1 - scaleY)), rect, color, 0f, default, new Vector2(1f, scaleY), SpriteEffects.None, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(texture, position, rect, color, 0f, default, new Vector2(1f, 1f), SpriteEffects.None, 0f);
            }
        }
    }
}