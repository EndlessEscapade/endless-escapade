using EEMod.Extensions;
using EEMod.Items.Placeables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Tiles
{
    public class ThermalMossTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlendAll[Type] = true;

            Main.tileLighted[Type] = true;
            Main.tileBlockLight[Type] = true;

            AddMapEntry(new Color(48, 115, 135));

            dustType = DustID.Rain;
            drop = ModContent.ItemType<LightGemsand>();
            soundStyle = 1;
            mineResist = 1f;
            minPick = 0;
        }
        void PlaceGroundGrass(int i, int j)
        {
            int noOfGrassBlades = (int)(((i + j) % 16) * 0.2f);
            const string tex = "Tiles/Foliage/KelpGrassStubbedMoss2";
            string Chosen = tex;

            for (int a = 0; a < noOfGrassBlades; a++)
            {
                float pos = i * 16 + (i + j * a + a * 7) % 16;
                if ((i + j * a * 2) % 2 != 0)
                    ModContent.GetInstance<EEMod>().TVH.AddElement(new Leaf(new Vector2(pos, j * 16), Chosen, 0f, Color.Lerp(Color.Yellow, Color.LightYellow, ((i + j + a * 3) % 4) / 4f), false, true, true));
                else
                {
                    ModContent.GetInstance<EEMod>().TVH.AddElement(new Leaf(new Vector2(pos - ModContent.GetInstance<EEMod>().GetTexture(Chosen).Width, j * 16), Chosen, 0f, Color.Lerp(Color.Yellow, Color.LightYellow, ((i + j + a * 3) % 4) / 4f), true, true, true));
                }
            }
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (!Main.tileSolid[Framing.GetTileSafely(i, j - 1).type] || !Framing.GetTileSafely(i, j - 1).active() && Framing.GetTileSafely(i, j).slope() == 0 && !Framing.GetTileSafely(i, j).halfBrick() && Main.GameUpdateCount % 500 == 0)
            {
                PlaceGroundGrass(i, j);
            }
            return true;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Color color = Color.White*Math.Abs((float)Math.Sin(Main.GameUpdateCount/200f));
            int frameX = Framing.GetTileSafely(i, j).frameX;
            int frameY = Framing.GetTileSafely(i, j).frameY;
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if(Main.rand.Next(4) == 1 && !Framing.GetTileSafely(i,j-1).active() )
            {
                Color chosen = Color.Lerp(Color.Crimson, Color.White, Main.rand.NextFloat(1f));
                //EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.03f));
                //EEMod.MainParticles.SpawnParticles(new Vector2(i*16,j*16), -Vector2.UnitY, 4, chosen, new SlowDown(0.98f), new RotateTexture(Main.rand.NextFloat(-0.03f, 0.03f)), new SetMask(ModContent.GetInstance<EEMod>().GetTexture("Masks/RadialGradient")), new AfterImageTrail(1f), new RotateVelocity(Main.rand.NextFloat(-0.03f,0.03f)), new SetLighting(chosen.ToVector3(),0.1f));
            }
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Vector2 position = new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero;
            Texture2D texture = ModContent.GetInstance<EEMod>().GetTexture("Tiles/ThermalMossTileGlow");
            Rectangle rect = new Rectangle(frameX, frameY, 16, 16);
            //Main.spriteBatch.Draw(texture, position, rect, Lighting.GetColor(i,j), 0f, default, 1f, SpriteEffects.None, 0f);
            //Main.spriteBatch.Draw(texture, position, rect, color, 0f, default, 1f, SpriteEffects.None, 0f);
        }
    }
}
