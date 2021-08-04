using EEMod.Extensions;
using EEMod.Items.Placeables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Tiles.EmptyTileArrays;
using System;

namespace EEMod.Tiles
{
    public class KelpLeafTile : EETile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = false;
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;

            Main.tileLighted[Type] = true;
            Main.tileBlockLight[Type] = true;

            AddMapEntry(new Color(40, 200, 0));

            dustType = DustID.Rain;
            soundStyle = 1;
            mineResist = 1f;
            minPick = 0;
        }
        public override void RandomUpdate(int i, int j)
        {
            if (!Main.tileSolid[Framing.GetTileSafely(i, j - 1).type] || !Framing.GetTileSafely(i, j - 1).active() && Framing.GetTileSafely(i, j).slope() == 0 && !Framing.GetTileSafely(i, j).halfBrick())
                PlaceGroundGrass(i, j);
        }
        void PlaceGroundGrass(int i, int j)
        {
            int noOfGrassBlades = (int)(((i + j) % 16) * 0.2f);
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
            Point tp = Main.LocalPlayer.position.ToTileCoordinates();
            if (tp.Y > j - 4 && tp.Y < j && tp.X > i - 1 && tp.X < i + 1)
            {
                Color chosen = Color.Lerp(Color.Yellow, Color.LightGoldenrodYellow, Main.rand.NextFloat(1f));
                if (Math.Abs(Main.LocalPlayer.velocity.X) > 0.1f)
                {
                    EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.6f));
                    EEMod.MainParticles.SpawnParticles(Main.LocalPlayer.Center + new Vector2(0, Main.LocalPlayer.height / 2),
                        -Main.LocalPlayer.velocity / 5f + new Vector2(0,-0.5f), null, 20, 2f,
                        chosen,
                        new SlowDown(0.97f), new RotateVelocity(Main.rand.NextFloat(-.04f, .04f)), new SetMask(Helpers.RadialMask, 0.15f), new AfterImageTrail(0.96f));
                }
                if(Main.LocalPlayer.velocity.Y > 3f)
                {
                    EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.6f));
                    EEMod.MainParticles.SpawnParticles(Main.LocalPlayer.Center + new Vector2(0, Main.LocalPlayer.height / 2),
                        -Main.LocalPlayer.velocity / 5f + new Vector2(0, -0.5f), null, 20, 2f,
                        chosen,
                        new SlowDown(0.97f), new RotateVelocity(Main.rand.NextFloat(-.05f, .05f)), new SetMask(Helpers.RadialMask, 0.15f), new Spew(6.14f,1f,Vector2.One,0.9f)
                        , new AfterImageTrail(0.96f));
                }
            }
            return true;
        }
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
        {
            offsetY = 100000;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            
            Color color = Lighting.GetColor(i, j);
            int frameX = Framing.GetTileSafely(i, j).frameX;
            int frameY = Framing.GetTileSafely(i, j).frameY;
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