using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.UI
{
    internal class DragableUIPanel : UIPanel
    {
        bool dragging;
        float lastX, lastY;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (dragging)
            {
                Top.Pixels = Helpers.Clamp(Top.Pixels - (lastY - Main.mouseY), 0, Parent.Height.Pixels - Height.Pixels);
                Left.Pixels = Helpers.Clamp(Left.Pixels - (lastX - Main.mouseX), 0, Parent.Width.Pixels - Width.Pixels);
                lastX = Main.mouseX;
                lastY = Main.mouseY;
                Recalculate();
            }
        }

        public override void MouseDown(UIMouseEvent evt)
        {
            base.MouseDown(evt);
            var d = evt.MousePosition;
            lastX = d.X;
            lastY = d.Y;
            dragging = true;
        }

        public override void MouseUp(UIMouseEvent evt)
        {
            base.MouseUp(evt);
            dragging = false;
            Recalculate();
        }
    }
}
