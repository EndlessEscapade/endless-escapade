using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace EEMod.UI.Elements
{
    internal class DragableUIPanelBackgroundTexture : UIPanel
    {
        public Texture2D _backgroundTexture = null;
        private readonly Color[] color = new Color[7];

        public DragableUIPanelBackgroundTexture(string Texture)
        {
            _backgroundTexture = ModContent.GetTexture(Texture);
        }

        public string[] StringOfTextures = {
            "EEMod/Projectiles/Runes/DesertRune",
            "EEMod/Projectiles/Runes/DepocaditaRune",
            "EEMod/Projectiles/Runes/BubblingWatersRune",
            "EEMod/Projectiles/Runes/FeralWrathRune",
            "EEMod/Projectiles/Runes/IgnisRune",
            "EEMod/Projectiles/Runes/PermafrostRune",
            "EEMod/Projectiles/Runes/CycloneStormRune",
            "EEMod/Projectiles/Runes/RunePlacement",
            "EEMod/Projectiles/Runes/RunePlacement"
            };

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Main.LocalPlayer.GetModPlayer<EEPlayer>().hasGottenRuneBefore.Length; i++)
            {
                if (Main.LocalPlayer.GetModPlayer<EEPlayer>().hasGottenRuneBefore[i] == 1)
                {
                    if (_backgroundTexture == ModContent.GetTexture(StringOfTextures[i]))
                    {
                        if (color[i].R < 255)
                        {
                            color[i].R++;
                        }

                        if (color[i].G < 255)
                        {
                            color[i].G++;
                        }

                        if (color[i].B < 255)
                        {
                            color[i].B++;
                        }
                    }
                }
                else
                {
                    color[i] = Color.Black;
                }
            }
            CalculatedStyle dimensions = GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
            for (int i = 0; i < 7; i++)
            {
                if (_backgroundTexture == ModContent.GetTexture(StringOfTextures[i]))
                {
                    spriteBatch.Draw(_backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), color[i]);
                }
            }
        }
    }
}