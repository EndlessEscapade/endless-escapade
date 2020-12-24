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
            if (frameX == 0 && frameY == 0)
            {
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

                Texture2D tex = EEMod.instance.GetTexture("Tiles/Furniture/Coral/AquamarineLamp1Glow");
                Texture2D mask = EEMod.instance.GetTexture("Masks/SmoothFadeOut");
                Vector2 position = new Vector2(i * 16 - (int)Main.screenPosition.X, (j - 1) * 16 - (int)Main.screenPosition.Y) + zero;

                Lighting.AddLight(position, new Vector3(1, 1, 1) * 4f);

                //mask
                //Helpers.DrawAdditive(tex, position + new Vector2(15, 10) + new Vector2(0, 2 * (float)Math.Sin(Main.GameUpdateCount / 10f) - 4), Color.White * 0.25f * HeartBeat, 1.5f);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
                Main.spriteBatch.Draw(mask, position, mask.Bounds, Color.White, 0f, mask.TextureCenter(), 1f, SpriteEffects.None, 0f);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
                
                //diamond
                Main.spriteBatch.Draw(tex, position + new Vector2(0, 2 * (float)Math.Sin(Main.GameUpdateCount / 10f) - 4), tex.Bounds, Lighting.GetColor((int)position.X / 16, (int)position.Y / 16), 0f, default, 1f, SpriteEffects.None, 1f);
                Main.spriteBatch.Draw(tex, position + new Vector2(0, 2 * (float)Math.Sin(Main.GameUpdateCount / 10f) - 4), tex.Bounds, Color.White * HeartBeat, 0f, default, 1f, SpriteEffects.None, 1f);
            }
        }
    }
}