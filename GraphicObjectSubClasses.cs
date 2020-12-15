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
        public override void Draw(Texture2D tex, int noOfFrames, int PerFrame)
        {
            base.Draw(tex, noOfFrames, PerFrame);
            EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            int FHeight = tex.Height / noOfFrames;
            int frameY = frameCounter / PerFrame % noOfFrames;
            Rectangle rect = new Rectangle(0, FHeight * frameY + 2, tex.Width, FHeight - 2);
            Color drawColour = Color.Black * (1 - (modPlayer.cutSceneTriggerTimer / 180f)) * MathHelper.Clamp((0.5f * Lighting.GetColor((int)(Position.X / 16f), (int)(Position.Y / 16f)).A), 0.1f, 0.5f);
            Main.spriteBatch.Draw(tex, Position.ForDraw() + new Vector2(0, 80) + Main.screenPosition, rect, drawColour, MathHelper.Pi, rect.Size() / 2, scale, SpriteEffects.None, 0f);
        }
    }
}