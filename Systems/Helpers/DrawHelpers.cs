using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ObjectData;

namespace EEMod
{
    public static partial class Helpers
    {
        public static void Draw(Texture2D tex, Vector2 position, Color colour, float scale, Rectangle frame = default)
        {
            Main.spriteBatch.Draw(tex, position, frame == default ? tex.Bounds : frame, colour, 0f, frame == default ? tex.TextureCenter() : frame.Center(), scale, SpriteEffects.None, 0f);
        }
        public static void Draw(Texture2D tex, Vector2 position, Color colour, float scale, Rectangle frame = default, float rotation = 0f)
        {
            Main.spriteBatch.Draw(tex, position, frame == default ? tex.Bounds : frame, colour, rotation, frame == default ? tex.TextureCenter() : frame.Center(), scale, SpriteEffects.None, 0f);
        }
        public static Texture2D RadialMask => ModContent.GetInstance<EEMod>().GetTexture("Masks/RadialGradient");
        public static void DrawAdditive(Texture2D tex, Vector2 position, Color colour, float scale)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            Main.spriteBatch.Draw(tex, position, tex.Bounds, colour, 0f, tex.TextureCenter(), scale, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public static void DrawAdditiveFunkyRadial(Vector2 position, Color colour, float scale, float intensity, float offset = 0)
        {
            Texture2D tex = ModContent.GetInstance<EEMod>().GetTexture("Masks/RadialGradient");
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            EEMod.RadialField.Parameters["pos"].SetValue(new Vector2((float)Math.Sin(Main.GameUpdateCount / 60f + offset), (float)Math.Cos(Main.GameUpdateCount / 60f - offset) * 0.1f));
            EEMod.RadialField.Parameters["progress"].SetValue(Main.GameUpdateCount / 60f);
            EEMod.RadialField.Parameters["alpha"].SetValue(intensity);
            EEMod.RadialField.Parameters["noiseTexture"].SetValue(ModContent.GetInstance<EEMod>().GetTexture("Noise/noise"));
            EEMod.RadialField.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.Draw(tex, position, tex.Bounds, colour, 0f, tex.TextureCenter(), scale, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

        }
        public static void DrawAdditiveFunky(Texture2D tex, Vector2 position, Color colour, float scale, float intensity, float offset = 0)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            EEMod.RadialField.Parameters["pos"].SetValue(new Vector2((float)Math.Sin(Main.GameUpdateCount / 60f + offset), (float)Math.Cos(Main.GameUpdateCount / 60f - offset) * 0.1f));
            EEMod.RadialField.Parameters["progress"].SetValue(Main.GameUpdateCount / 60f);
            EEMod.RadialField.Parameters["alpha"].SetValue(intensity);
            EEMod.RadialField.Parameters["noiseTexture"].SetValue(ModContent.GetInstance<EEMod>().GetTexture("Noise/noise"));
            EEMod.RadialField.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.Draw(tex, position, tex.Bounds, colour, 0f, tex.TextureCenter(), scale, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

        }
        public static void DrawAdditiveFunkyNoBatch(Texture2D tex, Vector2 position, Color colour, float scale, float intensity, float offset = 0)
        {
            EEMod.RadialField.Parameters["pos"].SetValue(new Vector2((float)Math.Sin(Main.GameUpdateCount / 60f + offset), (float)Math.Cos(Main.GameUpdateCount / 60f - offset) * 0.1f));
            EEMod.RadialField.Parameters["progress"].SetValue(Main.GameUpdateCount / 60f);
            EEMod.RadialField.Parameters["alpha"].SetValue(intensity);
            EEMod.RadialField.Parameters["noiseTexture"].SetValue(ModContent.GetInstance<EEMod>().GetTexture("Noise/noise"));
            EEMod.RadialField.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.Draw(tex, position, tex.Bounds, colour, 0f, tex.TextureCenter(), scale, SpriteEffects.None, 0f);
        }
        public static void DrawAdditive(Texture2D tex, Vector2 position, Color colour, float scale, float rotation)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            Main.spriteBatch.Draw(tex, position, tex.Bounds, colour, rotation, tex.TextureCenter(), scale, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public static Texture2D playerTexture => ModContent.GetInstance<EEMod>().playerDrawData;
        public static Vector2 TextureCenter(this Texture2D texture) => new Vector2(texture.Width / 2, texture.Height / 2);
        public static Vector2 Size(this Texture2D texture) => new Vector2(texture.Width, texture.Height);

        public static void DrawTileGlowmask(Texture2D texture, int i, int j)
        {
            int frameX = Framing.GetTileSafely(i, j).frameX;
            int frameY = Framing.GetTileSafely(i, j).frameY;

            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }

            Vector2 position = new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero;
            Rectangle rect = new Rectangle(frameX, frameY, 16, 16);

            Main.spriteBatch.Draw(texture, position, rect, Color.White, 0f, default, 1f, SpriteEffects.None, 0f);
        }

        public static void DrawTileGlowmask(Texture2D texture, int i, int j, Color color)
        {
            int frameX = Framing.GetTileSafely(i, j).frameX;
            int frameY = Framing.GetTileSafely(i, j).frameY;

            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }

            Vector2 position = new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero;
            Rectangle rect = new Rectangle(frameX, frameY, 16, 16);

            Main.spriteBatch.Draw(texture, position, rect, color, 0f, default, 1f, SpriteEffects.None, 0f);
        }

        public static Vector2 GetTileDrawZero ()
        {
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }

            return zero;
        }
    }
}
