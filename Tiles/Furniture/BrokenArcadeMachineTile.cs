using EEMod.Items.Placeables.Furniture;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;

namespace EEMod.Tiles.Furniture
{
    public class BrokenArcadeMachineTile : EETile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Broken Arcade Machine");
            AddMapEntry(new Color(255, 168, 28), name);
            DustType = DustID.Silver;
            DisableSmartCursor = true;
        }

        float HeartBeat;
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            int TileFrameX = Framing.GetTileSafely(i, j).TileFrameX;
            int TileFrameY = Framing.GetTileSafely(i, j).TileFrameY;
            if (TileFrameX == 0 && TileFrameY == 0)
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

                Texture2D tex = EEMod.Instance.Assets.Request<Texture2D>("Tiles/Furniture/Coral/AquamarineLamp1Glow").Value;
                Texture2D mask = EEMod.Instance.Assets.Request<Texture2D>("Textures/SmoothFadeOut").Value;
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