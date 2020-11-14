using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.VerletIntegration
{
    public class Verlet
    {
        private readonly float _gravity = 0.4f;
        private readonly float _bounce = 0.9f;
        private readonly float _AR = 0.999f;
        private readonly int _fluff = 1;
        int RENDERDISTANCE => 2000;
        public static List<Stick> stickPoints = new List<Stick>();
        public static List<Point> Points = new List<Point>();

        public int CreateVerletPoint(Vector2 pos, bool isStatic = false)
        {
            Points.Add(new Point(pos, pos - new Vector2(Main.rand.Next(-10, 10), Main.rand.Next(-10, 10)), isStatic));

            return Points.Count - 1;
        }

        public void ClearPoints()
        {
            VerletHelpers.EndPointChains.Clear();
            Points.Clear();
            stickPoints.Clear();
        }

        public int[] CreateVerletSquare(Vector2 pos, int size)
        {
            int a = CreateVerletPoint(pos + new Vector2(-size / 2, -size / 2));
            int b = CreateVerletPoint(pos + new Vector2(size / 2, -size / 2));
            int c = CreateVerletPoint(pos + new Vector2(size / 2, size / 2));
            int d = CreateVerletPoint(pos + new Vector2(-size / 2, size / 2));

            BindPoints(a, b);
            BindPoints(b, c);
            BindPoints(c, d);
            BindPoints(d, a);
            BindPoints(a, c);

            return new int[] { a, b, c, d };
        }

        public int[] CreateStickMan(Vector2 pos)
        {
            int first = CreateVerletPoint(pos);
            int b = CreateVerletPoint(pos + new Vector2(0, 40));
            int c = CreateVerletPoint(pos + new Vector2(0, 60));
            int d = CreateVerletPoint(pos + new Vector2(20, 90));
            int e = CreateVerletPoint(pos + new Vector2(-20, 90));
            int f = CreateVerletPoint(pos + new Vector2(-25, 120));
            int g = CreateVerletPoint(pos + new Vector2(25, 120));
            int h = CreateVerletPoint(pos + new Vector2(-20, 40));
            int i = CreateVerletPoint(pos + new Vector2(-30, 60));
            int j = CreateVerletPoint(pos + new Vector2(20, 40));
            int k = CreateVerletPoint(pos + new Vector2(30, 60));
            int a = CreateVerletPoint(pos + new Vector2(0, 20));

            BindPoints(first, a);
            BindPoints(a, b);
            BindPoints(b, c);
            BindPoints(c, d);
            BindPoints(d, g);

            BindPoints(c, e);
            BindPoints(e, f);
            BindPoints(a, h);
            BindPoints(h, i);
            BindPoints(a, j);
            BindPoints(j, k);

            BindPoints(first, c, true, Color.Yellow);
            BindPoints(a, i, true, Color.Yellow);
            BindPoints(a, k, true, Color.Yellow);
            BindPoints(j, h, true, Color.Yellow);
            BindPoints(f, d, true, Color.Yellow);

            return new int[] { first, a, b, c, d, e, f, g, h, i, j, k };
        }

        public void BindPoints(int a, int b, bool isVisible = true, Color color = default, Texture2D tex = null, Texture2D glowmask = null, Texture2D LightMap = null)
        {
            try
            {
                stickPoints.Add(new Stick(a, b, isVisible, color, tex, glowmask, LightMap));
            }
            catch
            {
                Main.NewText("Don't be dumb. smh");
            }
        }
        public void Update()
        {
            UpdatePoints();
            ConstrainToWorld();

            for (int i = 0; i < 5; i++)
            {
                UpdateSticks();
                //ConstrainPoints();
            }

            //UpdateStickCollision();
        }

        public void GlobalRenderPoints()
        {
            RenderPoints();
            RenderSticks();
        }

        public class Point
        {
            public Vector2 point;
            public Vector2 oldPoint;
            public Vector2 vel;
            public bool isStatic;

            public Point(Vector2 point, Vector2 oldPoint, Vector2 vel)
            {
                this.point = point;
                this.oldPoint = oldPoint;
                this.vel = vel;
                isStatic = false;
            }

            public Point(Vector2 point)
            {
                this.point = point;
                oldPoint = point;
                vel = Vector2.Zero;
                isStatic = false;
            }

            public Point(Vector2 point, Vector2 oldPoint, bool isStatic)
            {
                this.point = point;
                this.oldPoint = oldPoint;
                this.isStatic = isStatic;
                vel = Vector2.Zero;
            }

            public Point(Vector2 point, Vector2 oldPoint)
            {
                this.point = point;
                this.oldPoint = oldPoint;
                vel = Vector2.Zero;
                isStatic = false;
            }
        }

        public class Stick
        {
            public Color color;
            public Texture2D tex;
            public Vector2 p1;
            public Vector2 p2;
            public Vector2 oldP1;
            public Vector2 oldP2;
            public Vector2 vel1;
            public Vector2 vel2;
            public bool[] isStatic;
            public float Length;
            public int a;
            public int b;
            public bool isVisible;
            public Texture2D glowmask;
            public Texture2D LightMap;

            public Stick(int a, int b, bool isVisible = true, Color color = default, Texture2D tex = null, Texture2D glowmask = null, Texture2D LightMap = null)
            {
                this.a = a;
                this.b = b;
                isStatic = new bool[2];
                p1 = Points[a].point;
                p2 = Points[b].point;
                oldP1 = Points[a].oldPoint;
                oldP2 = Points[b].oldPoint;
                vel1 = Points[a].vel;
                vel2 = Points[b].vel;

                float disX = Points[b].point.X - Points[a].point.X;
                float disY = Points[b].point.Y - Points[a].point.Y;

                Length = (float)Math.Sqrt(disX * disX + disY * disY);
                isStatic[0] = Points[a].isStatic;
                isStatic[1] = Points[b].isStatic;
                this.tex = tex;
                if (color == default)
                {
                    this.color = Color.DarkRed;
                }
                else
                {
                    this.color = color;
                }

                this.isVisible = isVisible;
                this.glowmask = glowmask;
                this.LightMap = LightMap;
            }
        }

        private void UpdateSticks()
        {
            for (int i = 0; i < stickPoints.Count; i++)
            {
                Stick stick = stickPoints[i];
                if ((Main.LocalPlayer.Center - (Points[stick.a].point + Points[stick.b].point) / 2f).LengthSquared() < RENDERDISTANCE * RENDERDISTANCE)
                {
                    Point p1 = Points[stick.a];
                    Point p2 = Points[stick.b];
                    float dx = p2.point.X - p1.point.X;
                    float dy = p2.point.Y - p1.point.Y;
                    float currentLength = (float)Math.Sqrt(dx * dx + dy * dy);
                    float deltaLength = currentLength - stick.Length;
                    float perc = deltaLength / currentLength * 0.5f;
                    float offsetX = perc * dx;
                    float offsetY = perc * dy;

                    if (!stickPoints[i].isStatic[0])
                    {
                        Points[stick.a].point.X += offsetX;
                        Points[stick.a].point.Y += offsetY;
                    }

                    if (!stickPoints[i].isStatic[1])
                    {
                        Points[stick.b].point.X -= offsetX;
                        Points[stick.b].point.Y -= offsetY;
                    }
                }
            }
        }

        private void UpdateStickCollision()
        {
            for (int i = 0; i < stickPoints.Count; i++)
            {

                Stick stick = stickPoints[i];
                int max = 0;

                while (!Collision.CanHit(Points[stick.a].point, 1, 1, Points[stick.b].point, 1, 1))
                {
                    max++;
                    Vector2 grad = Vector2.Normalize(Points[stick.a].point - Points[stick.b].point);
                    Vector2 normal = grad.RotatedBy(Math.PI / 2f);
                    if (!stickPoints[i].isStatic[0])
                        Points[stick.a].point -= normal;
                    if (!stickPoints[i].isStatic[1])
                        Points[stick.b].point -= normal;

                    if (max > 10)
                    {
                        break;
                    }
                }
            }
        }

        private void UpdatePoints()
        {
            for (int i = 0; i < Points.Count; i++)
            {
                if (!Points[i].isStatic && (Main.LocalPlayer.Center - Points[i].point).LengthSquared() < RENDERDISTANCE * RENDERDISTANCE)
                {
                    Points[i].vel.X = (Points[i].point.X - Points[i].oldPoint.X) * _AR;
                    Points[i].vel.Y = (Points[i].point.Y - Points[i].oldPoint.Y) * _AR;
                    Points[i].oldPoint.X = Points[i].point.X;
                    Points[i].oldPoint.Y = Points[i].point.Y;
                    Points[i].point.X += Points[i].vel.X;
                    Points[i].point.Y += Points[i].vel.Y;
                    Points[i].point.Y += _gravity;
                }
            }
        }

        private void RenderPoints()
        {
            for (int i = 0; i < Points.Count; i++)
            {
                if (i == 0)
                {
                    // Main.spriteBatch.Draw(Main.magicPixel, Points[i].point.ForDraw(), new Rectangle(0, 0, 20, 20), Color.AliceBlue, 0f, new Vector2(20, 20), 1f, SpriteEffects.None, 0f);
                }
                else
                {
                    //TODO: Do something.
                }
            }
        }

        private void RenderSticks()
        {
            for (int i = 0; i < stickPoints.Count; i++)
            {
                if ((Main.LocalPlayer.Center - (Points[stickPoints[i].a].point + Points[stickPoints[i].b].point) / 2f).LengthSquared() < RENDERDISTANCE * RENDERDISTANCE)
                {
                    if (stickPoints[i].isVisible)
                    {
                        Vector2 p1 = Points[stickPoints[i].a].point;
                        Vector2 p2 = Points[stickPoints[i].b].point;
                        float Dist = Vector2.Distance(p1, p2);
                        if (stickPoints[i].tex == null)
                        {
                            for (float j = 0; j < 1; j += 1 / Dist)
                            {
                                Vector2 Lerped = p1 + j * (p2 - p1);

                                Main.spriteBatch.Draw(Main.magicPixel, Lerped - Main.screenPosition, new Rectangle(0, 0, 1, 1), stickPoints[i].color, 0f, new Vector2(1, 1), 1f, SpriteEffects.None, 0f);
                            }
                        }
                        else
                        {
                            Vector2 mid = p1 * 0.5f + p2 * 0.5f;
                            if (stickPoints[i].LightMap != null)
                            {
                                Helpers.DrawAdditive(stickPoints[i].LightMap, mid.ForDraw(), Color.Yellow*0.6f, 1.2f, (p1 - p2).ToRotation());
                            }

                            Main.spriteBatch.Draw(stickPoints[i].tex, mid.ForDraw(), stickPoints[i].tex.Bounds, Lighting.GetColor((int)mid.X / 16, (int)mid.Y / 16), (p1 - p2).ToRotation(), stickPoints[i].tex.Bounds.Size() / 2, 1f, SpriteEffects.None, 0f);
                            if (stickPoints[i].glowmask != null)
                            {
                                Main.spriteBatch.Draw(stickPoints[i].glowmask, mid.ForDraw(), stickPoints[i].glowmask.Bounds, Color.White, (p1 - p2).ToRotation(), stickPoints[i].glowmask.Bounds.Size() / 2, 1f, SpriteEffects.None, 0f);
                                EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.003f));
                                EEMod.Particles.Get("Main").SpawnParticles(mid, new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f)), ModContent.GetInstance<EEMod>().GetTexture("Particles/Cross"), 30, 2, Color.Lerp(Color.Goldenrod, Color.Yellow, Main.rand.NextFloat(0f, 1f)), new SlowDown(0.98f), new RotateVelocity(Main.rand.NextFloat(-.01f, .01f)), new RotateTexture(0.02f), new AfterImageTrail(0.7f));
                            }
                        }
                    }
                }
            }
        }

        private int[] GetContactPoints(Vector2 point)
        {
            int[] Points = new int[4];
            Vector2 tileP = point / 16;

            Points[0] = (int)point.X / 16;
            Points[1] = (int)point.X / 16;
            Points[2] = (int)point.Y / 16;
            Points[3] = (int)point.Y / 16;

            while (!Framing.GetTileSafely(Points[0], (int)tileP.Y).active())
            {
                Points[0]++;

                if (Points[0] - (int)tileP.X > 10)
                {
                    Points[0] = -1;

                    break;
                }
            }

            while (!Framing.GetTileSafely(Points[1], (int)tileP.Y).active())
            {
                Points[1]--;

                if (Points[1] - (int)tileP.X < -10)
                {
                    Points[1] = -1;

                    break;
                }
            }

            while (!Framing.GetTileSafely((int)tileP.X, Points[2]).active())
            {
                Points[2]++;

                if (Points[2] - (int)tileP.Y > 10)
                {
                    Points[2] = -1;

                    break;
                }
            }

            while (!Framing.GetTileSafely((int)tileP.X, Points[3]).active())
            {
                Points[3]--;

                if (Points[3] - (int)tileP.Y < -10)
                {
                    Points[3] = -1;

                    break;
                }
            }

            for (int i = 0; i < Points.Length; i++)
            {
                if (Points[i] != -1)
                {
                    if (i < 2)
                    {
                        if (!Main.tileSolid[Framing.GetTileSafely(Points[i], (int)tileP.Y).type])
                        {
                            Points[i] = -1;
                        }
                    }
                    else
                    {
                        if (!Main.tileSolid[Framing.GetTileSafely((int)tileP.X, Points[i]).type])
                        {
                            Points[i] = -1;
                        }
                    }
                }

                if (Points[i] != -1)
                {
                    Points[i] *= 16;
                }
            }

            return Points;
        }

        private void ConstrainPoints()
        {
            for (int i = 0; i < Points.Count; i++)
            {
                if (!Points[i].isStatic)
                {
                    Points[i].vel.X = (Points[i].point.X - Points[i].oldPoint.X) * _AR;
                    Points[i].vel.Y = (Points[i].point.Y - Points[i].oldPoint.Y) * _AR;

                    int[] ContactPoints = GetContactPoints(Points[i].point);

                    if (Points[i].point.Y > ContactPoints[2] - _fluff && ContactPoints[2] != -1)
                    {
                        Points[i].oldPoint.Y = ContactPoints[2] - _fluff + Points[i].vel.Y * _bounce;
                        Points[i].point.Y = ContactPoints[2] - _fluff;
                    }

                    ContactPoints = GetContactPoints(Points[i].point);

                    if (Points[i].point.Y < ContactPoints[3] + _fluff && ContactPoints[3] != -1)
                    {
                        Points[i].oldPoint.Y = ContactPoints[3] + _fluff + Points[i].vel.Y * _bounce;
                        Points[i].point.Y = ContactPoints[3] + _fluff;
                    }
                    ContactPoints = GetContactPoints(Points[i].point);

                    if (Points[i].point.X > ContactPoints[0] - _fluff && ContactPoints[0] != -1)
                    {
                        Points[i].oldPoint.X = ContactPoints[0] - _fluff + Points[i].vel.X * _bounce;
                        Points[i].point.X = ContactPoints[0] - _fluff;
                    }
                    ContactPoints = GetContactPoints(Points[i].point);

                    if (Points[i].point.X < ContactPoints[1] + _fluff && ContactPoints[1] != -1)
                    {
                        Points[i].oldPoint.X = ContactPoints[1] + _fluff + Points[i].vel.X * _bounce;
                        Points[i].point.X = ContactPoints[1] + _fluff;
                    }
                }
            }
        }

        private void ConstrainToWorld()
        {
            for (int i = 0; i < Points.Count; i++)
            {
                if (!Points[i].isStatic)
                {
                    Points[i].vel.X = (Points[i].point.X - Points[i].oldPoint.X) * _AR;
                    Points[i].vel.Y = (Points[i].point.Y - Points[i].oldPoint.Y) * _AR;

                    if (Points[i].point.Y > Main.maxTilesY * 16)
                    {
                        Points[i].oldPoint.Y = Main.maxTilesY * 16 + Points[i].vel.Y * _bounce;
                        Points[i].point.Y = Main.maxTilesY * 16;
                    }

                    if (Points[i].point.Y < 0)
                    {
                        Points[i].oldPoint.Y = Points[i].vel.Y * _bounce;
                        Points[i].point.Y = 0;
                    }

                    if (Points[i].point.X > Main.maxTilesX * 16)
                    {
                        Points[i].oldPoint.X = Main.maxTilesX * 16 + Points[i].vel.X * _bounce;
                        Points[i].point.X = Main.maxTilesX * 16;
                    }

                    if (Points[i].point.X < 0)
                    {
                        Points[i].oldPoint.X = Points[i].vel.X * _bounce;
                        Points[i].point.X = 0;
                    }
                }
            }
        }
    }
}
