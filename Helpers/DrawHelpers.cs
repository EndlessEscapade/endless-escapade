using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace EEMod
{
    public static partial class Helpers
    {
        public static void Draw(Texture2D tex, Vector2 position, Color colour, float scale)
        {
            Main.spriteBatch.Draw(tex, position, tex.Bounds, colour, 0f, tex.TextureCenter(), scale, SpriteEffects.None, 0f);
        }
        public static void DrawAdditive(Texture2D tex, Vector2 position, Color colour, float scale)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            Main.spriteBatch.Draw(tex, position, tex.Bounds, colour, 0f, tex.TextureCenter(), scale, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

        }
        public static Vector2 TextureCenter(this Texture2D texture) => new Vector2(texture.Width / 2, texture.Height / 2);
        public static Vector2 Size(this Texture2D texture) => new Vector2(texture.Width, texture.Height);
    }
}