using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.UI
{
    internal class DragableUIPanel : UIPanel
    {

        Texture2D _backgroundTexture = ModContent.GetTexture("EEMod/NPCs/Sphinx");
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            
            CalculatedStyle dimensions = GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
            spriteBatch.Draw(_backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), Color.Black);
        }

    }
}
