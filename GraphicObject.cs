using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;

namespace EEMod
{
    public class GraphicObject
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public float flash;
        public float scale;
        public float alpha;
        public float rotation;
        public int frameCounter;
        public float paralax;
        public static void LazyAppend<T>(ref List<T> sea) where T : GraphicObject, new()
        {
            Vector2 Pos = new Vector2(Main.screenPosition.X + Main.rand.Next(Main.screenWidth), Main.screenPosition.Y + 1200);
            T objects = new T
            {
                scale = Main.rand.NextFloat(0.5f, 1f),
                alpha = Main.rand.NextFloat(.2f, .8f),
                Position = Pos,
                flash = Main.rand.NextFloat(0, 100),
                Velocity = new Vector2(Main.rand.NextFloat(0.5f, 1), 0)
            };
            sea.Add(objects);
        }

        public static void LazyAppendSides<T>(ref List<T> sea) where T : GraphicObject, new()
        {
            Vector2 Pos = new Vector2(Main.screenPosition.X + 1200, Main.screenPosition.Y + Main.rand.Next(Main.screenHeight));
            T objects = new T
            {
                scale = Main.rand.NextFloat(0.5f, 1f),
                alpha = Main.rand.NextFloat(.2f, .8f),
                Position = Pos,
                flash = Main.rand.NextFloat(0, 100),
                Velocity = new Vector2(Main.rand.NextFloat(0.5f, 1), 0)
            };
            sea.Add(objects);
        }

        public static void LazyAppendInBoids<T>(ref List<T> sea, int maxBoidCount) where T : GraphicObject, new()
        {
            Vector2 Pos = new Vector2(Main.rand.Next(Main.screenWidth), 1200);
            List<Vector2> PosBuffer = new List<Vector2>();
            for (int i = 0; i < maxBoidCount; i++)
            {
                Pos += new Vector2(Main.rand.Next(-20, 20), Main.rand.Next(-20, 20));
                T objects = new T
                {
                    scale = Main.rand.NextFloat(0.5f, 1f),
                    alpha = Main.rand.NextFloat(.2f, .8f),
                    Position = Pos,
                    flash = Main.rand.NextFloat(0, 100),
                    Velocity = new Vector2(Main.rand.NextFloat(0.5f, 1), 0)
                };
                int boidCheck = 0;
                for (int j = 0; j < PosBuffer.Count; j++)
                {
                    if (Vector2.DistanceSquared(Pos, PosBuffer[j]) < 5 * 5)
                    {
                        boidCheck++;
                    }
                }
                if (boidCheck == 0)
                {
                    PosBuffer.Add(Pos);
                    sea.Add(objects);
                }
            }
        }

        public virtual void Draw(Texture2D tex, int noOfFrames, int PerFrame)
        {
            EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            int FHeight = tex.Height / noOfFrames;
            int frameY = frameCounter / PerFrame % noOfFrames;
            Rectangle rect = new Rectangle(0, FHeight * frameY, tex.Width, FHeight);
            Color drawColour = Lighting.GetColor((int)((Position.X + Main.screenPosition.X) / 16f), (int)((Position.Y + Main.screenPosition.Y) / 16f)) * modPlayer.seamapLightColor;
            drawColour.A = 255;
            Main.spriteBatch.Draw(tex, Position.ForDraw() + Main.screenPosition - new Vector2(Main.LocalPlayer.Center.X * paralax, 0), rect, drawColour * (1 - (modPlayer.cutSceneTriggerTimer / 180f)), MathHelper.Pi, rect.Size() / 2, scale, SpriteEffects.None, 0f);
        }
    }
}