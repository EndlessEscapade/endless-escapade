using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace EEMod
{
    partial class Helpers
    {
        public static void Draw(Texture2D tex,Vector2 position, Color colour,float scale)
        {
            Main.spriteBatch.Draw(tex, position,tex.Bounds, colour, 0f,tex.Bounds.Size()/2,scale,SpriteEffects.None,0f);
        }
    }
}