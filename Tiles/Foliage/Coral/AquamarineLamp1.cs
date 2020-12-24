using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;
using System;

namespace EEMod.Tiles.Foliage.Coral
{
    public class AquamarineLamp1 : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.Direction = TileObjectDirection.None;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.RandomStyleRange = 1;
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(120, 85, 60));
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {

        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.9f;
            g = 0.9f;
            b = 0.9f;
        }

        float HeartBeat;
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            int frameX = Main.tile[i, j].frameX;
            int frameY = Main.tile[i, j].frameY;

            if (frameX == 18 && frameY == 18)
            {
                EEMod.Particles.Get("Main").SetSpawningModules(new SpawnPeriodically(8,true));
                Vector2 part = new Vector2((i + 1) * 16 - 18, (j * 16) - 18 - 4) + new Vector2(0, 2 * (float)Math.Sin(Main.GameUpdateCount / 10f) - 4);
                EEMod.Particles.Get("Main").SpawnParticles(part, null, 2, Color.White, new CircularMotionSinSpinC(15, 15, 0.1f, part), new AfterImageTrail(1), new SetMask(Helpers.RadialMask));
                float timeBetween = 70;
                float bigTimeBetween = 200;
                if (Main.GameUpdateCount % 200 < timeBetween)
                {
                    HeartBeat = Math.Abs((float)Math.Sin((Main.GameUpdateCount % bigTimeBetween) * (6.28f / timeBetween))) * (1 - (Main.GameUpdateCount % bigTimeBetween) / (timeBetween * 1.5f));
                }
                else
                {
                    HeartBeat = 0;
                }

                Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
                if (Main.drawToScreen)
                {
                    zero = Vector2.Zero;
                }
                Texture2D tex = EEMod.instance.GetTexture("Tiles/Foliage/Coral/AquamarineLamp1Glow");
                Texture2D mask = EEMod.instance.GetTexture("Masks/SmoothFadeOut");
                Vector2 position = new Vector2(i * 16 - (int)Main.screenPosition.X, (j - 1) * 16 - (int)Main.screenPosition.Y) + zero;

                Lighting.AddLight(new Vector2(i * 16, (j ) * 16) + new Vector2(0, 2 * (float)Math.Sin(Main.GameUpdateCount / 10f) - 4), Color.Lerp(new Color(78, 125, 224), new Color(107, 2, 81), Math.Abs((float)Math.Sin(Main.GameUpdateCount / 100f + i*j))).ToVector3());
                float sineAdd = (float)Math.Sin(Main.GameUpdateCount/20f) + 2.5f;
                    Main.spriteBatch.Draw(mask, new Vector2((i + 1) * 16 - (int)Main.screenPosition.X - 25 - 18, (j * 16) - (int)Main.screenPosition.Y - 30 - 18) + zero + new Vector2(0, 2 * (float)Math.Sin(Main.GameUpdateCount / 10f) - 4), null, new Color(sineAdd, sineAdd, sineAdd, 0)*0.2f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                //mask
                //Helpers.DrawAdditive(tex, position + new Vector2(15, 10) + new Vector2(0, 2 * (float)Math.Sin(Main.GameUpdateCount / 10f) - 4), Color.White * 0.25f * HeartBeat, 1.5f);

                //diamond
                Main.spriteBatch.Draw(tex, position + new Vector2(2 - 18, 2 * (float)Math.Sin(Main.GameUpdateCount / 10f) - 4 - 18), tex.Bounds, Lighting.GetColor(i, j), 0f, default, 1f, SpriteEffects.None, 1f);
                Main.spriteBatch.Draw(tex, position + new Vector2(2-18, 2 * (float)Math.Sin(Main.GameUpdateCount / 10f) - 4 - 18), tex.Bounds, Color.White * HeartBeat, 0f, default, 1f, SpriteEffects.None, 1f);
            }
        }

    }
}
