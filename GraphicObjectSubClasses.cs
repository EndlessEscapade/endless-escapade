using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace EEMod
{
    public class ParticlesClass : GraphicObject
    {
    }

    public class LeafClass : GraphicObject
    {
    }

    public class BubbleClass : GraphicObject
    {
    }

    public class SeagullsClass : GraphicObject
    {
        public void DrawShadow(Texture2D tex, int noOfFrames, int PerFrame)
        {
            EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            int FHeight = tex.Height / noOfFrames;
            int frameY = (frameCounter / PerFrame) % noOfFrames;
            Rectangle rect = new Rectangle(0, FHeight * frameY, tex.Width, FHeight);
            Main.spriteBatch.Draw(tex, Position.ForDraw() + new Vector2(0, 80) + Main.screenPosition, rect, Color.Black * (1 - (modPlayer.cutSceneTriggerTimer / 180f)) * 0.5f, (float)Math.PI, rect.Size() / 2, scale, SpriteEffects.None, 0f);
        }
    }
}