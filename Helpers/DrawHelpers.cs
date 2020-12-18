using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace EEMod
{
    public static partial class Helpers
    {
        public static void Draw(Texture2D tex, Vector2 position, Color colour, float scale, Rectangle frame = default)
        {
            Main.spriteBatch.Draw(tex, position, frame == default ? tex.Bounds : frame, colour, 0f, frame == default ? tex.TextureCenter() : frame.Center(), scale, SpriteEffects.None, 0f);
        }
        public static void DrawAdditive(Texture2D tex, Vector2 position, Color colour, float scale)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);


            Main.spriteBatch.Draw(tex, position, tex.Bounds, colour, 0f, tex.TextureCenter(), scale, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

        }
        public static void DrawAdditiveFunky(Texture2D tex, Vector2 position, Color colour, float scale, float intensity, float offset = 0)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            EEMod.RadialField.Parameters["pos"].SetValue(new Vector2((float)Math.Sin(Main.GameUpdateCount / 60f + offset), (float)Math.Cos(Main.GameUpdateCount / 60f - offset) * 0.1f));
            EEMod.RadialField.Parameters["progress"].SetValue(Main.GameUpdateCount / 60f);
            EEMod.RadialField.Parameters["alpha"].SetValue(intensity);
            EEMod.RadialField.Parameters["noiseTexture"].SetValue(EEMod.instance.GetTexture("Noise/noise"));
            EEMod.RadialField.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.Draw(tex, position, tex.Bounds, colour, 0f, tex.TextureCenter(), scale, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

        }
        public static void DrawAdditive(Texture2D tex, Vector2 position, Color colour, float scale, float rotation)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            Main.spriteBatch.Draw(tex, position, tex.Bounds, colour, rotation, tex.TextureCenter(), scale, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

        }
        public static Vector2 TextureCenter(this Texture2D texture) => new Vector2(texture.Width / 2, texture.Height / 2);
        public static Vector2 Size(this Texture2D texture) => new Vector2(texture.Width, texture.Height);
    }
}
