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

    public class BubbleClass : GraphicObject
    {

    }

    public class SeagullsClass : GraphicObject
    {
        public override void Draw(Texture2D tex, int noOfFrames, int PerFrame)
        {
            EEPlayer modPlayer2 = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            int FHeight2 = tex.Height / noOfFrames;
            int frameY2 = frameCounter / PerFrame % noOfFrames;
            Rectangle rect2 = new Rectangle(0, FHeight2 * frameY2, tex.Width, FHeight2);
            Color drawColour2 = Lighting.GetColor((int)((Position.X + Main.screenPosition.X) / 16f), (int)((Position.Y + Main.screenPosition.Y) / 16f)) * modPlayer2.seamapLightColor;
            drawColour2.A = 255;
            Main.spriteBatch.Draw(tex, Position.ForDraw() - new Vector2(Main.LocalPlayer.Center.X * paralax, 0), rect2, drawColour2 * (1 - (modPlayer2.cutSceneTriggerTimer / 180f)), MathHelper.Pi, rect2.Size() / 2, scale, SpriteEffects.None, 0f);
            EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            int FHeight = tex.Height / noOfFrames;
            int frameY = frameCounter / PerFrame % noOfFrames;
            Rectangle rect = new Rectangle(0, FHeight * frameY + 2, tex.Width, FHeight - 2);
            Color drawColour = Color.Black * (1 - (modPlayer.cutSceneTriggerTimer / 180f)) * MathHelper.Clamp((0.5f * Lighting.GetColor((int)(Position.X / 16f), (int)(Position.Y / 16f)).A), 0.1f, 0.5f);
            Main.spriteBatch.Draw(tex, Position.ForDraw() + new Vector2(0, 80), rect, drawColour, MathHelper.Pi, rect.Size() / 2, scale, SpriteEffects.None, 0f);
        }
    }
}