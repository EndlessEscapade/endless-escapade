using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;
using System;
using Terraria.ID;

namespace EEMod.Tiles.Foliage.BulboBall
{
    public class BulboBall : EETile
    {
        public override void SetDefaults()
        {
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.Width = 8;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.Direction = TileObjectDirection.None;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Bulbo Ball");
            AddMapEntry(new Color(20, 60, 20), name);
            disableSmartCursor = true;
            dustType = DustID.Dirt;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {

        }

        /*public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.9f;
            g = 0.7f;
            b = 0.1f;
        }*/

        public float ballHeight;
        public bool peaked;
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (Main.tile[i, j].frameX == 0 && Main.tile[i, j].frameY == 0)
            {
                Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
                if (Main.drawToScreen)
                {
                    zero = Vector2.Zero;
                }

                if(!Main.dayTime)
                {
                    if (ballHeight < 256) ballHeight += (((ballHeight / 2f) + 128) / 256);

                    if (ballHeight >= 256) peaked = true;
                }
                else
                {
                    peaked = false;

                    if (ballHeight > 0) ballHeight -= (((ballHeight / 2f) + 128) / 256);
                }

                if (ballHeight <= 0)
                {
                    //Texture2D mask = ModContent.GetTexture("EEMod/Textures/RadialGradient");

                    /*spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

                    spriteBatch.Draw(mask, new Vector2(i * 16, j * 16) + new Vector2(64, 32) + zero - Main.screenPosition, null, Color.Gold, 0f, mask.TextureCenter(), 2f, SpriteEffects.None, 0f);

                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);*/

                    //Helpers.DrawAdditive(ModContent.GetTexture("EEMod/Textures/RadialGradient"), new Vector2(i * 16, j * 16) + new Vector2(64, 32) + zero - Main.screenPosition, Color.Gold, 2f, 0f);

                    Lighting.AddLight(new Vector2(i * 16, j * 16) + new Vector2(64, 16), Color.Gold.ToVector3() * 0.75f * 0.5f);

                    spriteBatch.Draw(ModContent.GetTexture("EEMod/Tiles/Foliage/BulboBall/GoldBulboBall"), new Vector2(i * 16, j * 16) + new Vector2(0, -32) + zero - Main.screenPosition, new Rectangle(0, 0, 128, 64), Lighting.GetColor(i, j), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
                else
                {
                    if (peaked) ballHeight += (float)Math.Sin((Main.GameUpdateCount / 80f) + (j - i)) * 0.5f;

                    Texture2D vineTex = ModContent.GetTexture("EEMod/Textures/BigVineVert");
                    Texture2D vineTexGlow = ModContent.GetTexture("EEMod/Textures/BigVineVertGlow");
                    Vector2 bezierOrig = new Vector2(i * 16, j * 16) + zero;

                    Vector2 bulboBallPos = new Vector2(i * 16, j * 16) + new Vector2(64, 32 - ballHeight);
                    Vector2 bulboBallPosLight = (new Vector2(i * 16, j * 16) + new Vector2(64, 32 - ballHeight)) / 16f;

                    Lighting.AddLight(new Vector2(i * 16, j * 16) + new Vector2(64, 32 - ballHeight), Color.Gold.ToVector3() * ((MathHelper.Clamp(ballHeight, 0, 128) + 128) / 256f) * 0.75f);

                    Helpers.DrawBezier(vineTex, Lighting.GetColor((int)bulboBallPosLight.X, (int)bulboBallPosLight.Y), bezierOrig + new Vector2(16, 32), bezierOrig + new Vector2(48, - ballHeight), bezierOrig + new Vector2(32, -16), 1f, vineTex.Bounds);
                    Helpers.DrawBezier(vineTex, Lighting.GetColor((int)bulboBallPosLight.X, (int)bulboBallPosLight.Y), bezierOrig + new Vector2(112, 32), bezierOrig + new Vector2(80, - ballHeight), bezierOrig + new Vector2(96, -16), 1f, vineTex.Bounds);

                    Helpers.DrawBezier(vineTexGlow, Color.White * (ballHeight / 256f), bezierOrig + new Vector2(16, 32), bezierOrig + new Vector2(48, - ballHeight), bezierOrig + new Vector2(32, -16), 1f, vineTex.Bounds);
                    Helpers.DrawBezier(vineTexGlow, Color.White * (ballHeight / 256f), bezierOrig + new Vector2(112, 32), bezierOrig + new Vector2(80, - ballHeight), bezierOrig + new Vector2(96, -16), 1f, vineTex.Bounds);

                    spriteBatch.Draw(ModContent.GetTexture("EEMod/Tiles/Foliage/BulboBall/GoldBulboBall"), bulboBallPos + zero - Main.screenPosition, new Rectangle(0, 0, 128, 128), Lighting.GetColor((int)bulboBallPosLight.X, (int)bulboBallPosLight.Y), (float)(Math.Sin((Main.GameUpdateCount / 65f) + i) / 9f) * (ballHeight / 256f), new Vector2(64, 64), 1f, SpriteEffects.None, 0f);
                }
            }

            return true;
        }
    }
}