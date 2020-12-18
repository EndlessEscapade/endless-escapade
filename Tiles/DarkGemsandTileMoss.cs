using EEMod.Extensions;
using EEMod.Items.Placeables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Tiles
{
    public class DarkGemsandTileMoss : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;

            AddMapEntry(new Color(67, 47, 155));

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
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Color color = Color.White*Math.Abs((float)Math.Sin(Main.GameUpdateCount/200f));
            int frameX = Main.tile[i, j].frameX;
            int frameY = Main.tile[i, j].frameY;
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if(Main.rand.Next(4) == 1 && !Framing.GetTileSafely(i,j-1).active())
            {
                Color chosen = Color.Lerp(Color.Yellow, Color.LightYellow, Main.rand.NextFloat(1f));
                EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.03f));
                EEMod.Particles.Get("Main").SpawnParticles(new Vector2(i*16,j*16), -Vector2.UnitY,3, chosen, new SlowDown(0.98f), new RotateTexture(Main.rand.NextFloat(-0.03f, 0.03f)), new SetMask(EEMod.instance.GetTexture("Masks/RadialGradient")), new AfterImageTrail(1f), new RotateVelocity(Main.rand.NextFloat(-0.02f,0.02f)), new SetLighting(chosen.ToVector3(),0.1f));
            }
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Vector2 position = new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero;
            Texture2D texture = EEMod.instance.GetTexture("Tiles/LightGemsandTileMossGlow");
            Rectangle rect = new Rectangle(frameX, frameY, 16, 16);
            Main.spriteBatch.Draw(texture, position, rect, Lighting.GetColor(i,j), 0f, default, 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, position, rect, color, 0f, default, 1f, SpriteEffects.None, 0f);
        }
    }
}
