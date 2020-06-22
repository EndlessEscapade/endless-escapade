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

        public Texture2D _backgroundTexture = null;
        Color color = new Color(0, 0, 0);
        public DragableUIPanel(string Texture)
        {
            _backgroundTexture = ModContent.GetTexture(Texture);
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (Main.LocalPlayer.GetModPlayer<EEPlayer>().hasGottenRuneBefore[0] == 1)
            {
                if (_backgroundTexture == ModContent.GetTexture("EEMod/NPCs/Sphinx"))
                {
                    if (color.R < 255)
                        color.R++;
                    if (color.R < 255)
                        color.G++;
                    if (color.B < 255)
                        color.B++;
                }
            }
            else
            {
                color = Color.Black;
            }
            CalculatedStyle dimensions = GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
            spriteBatch.Draw(_backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), color);
        }

        


    }
}
