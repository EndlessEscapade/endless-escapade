using EEMod.Autoloading;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using EEMod.Extensions;
using System.Linq;
using System;
using EEMod.Effects;
using EEMod.Projectiles.Mage;

namespace EEMod
{
    public class Verlet
    {
        float gravity = 0.5f;
        float bounce = 0.9f;
        float AR = 0.99f;
        int fluff = 1;
        public List<Stick> stickPoints = new List<Stick>();
        public static List<Point> points = new List<Point>();
        public int CreateVerletPoint(Vector2 pos, bool isStatic = false)
        {
            points.Add(new Point(pos, pos - new Vector2(Main.rand.Next(-10,10), Main.rand.Next(-10, 10)), isStatic));
            return points.Count - 1;
        }
        public void ClearPoints()
        {
            points.Clear();
            stickPoints.Clear();
        }
        public int[] CreateVerletSquare(Vector2 pos, int size)
        {
            int a = CreateVerletPoint(pos + new Vector2(-size/2, -size / 2));
            int b = CreateVerletPoint(pos + new Vector2(size / 2,-size / 2));
            int c = CreateVerletPoint(pos + new Vector2(size / 2, size / 2));
            int d = CreateVerletPoint(pos + new Vector2(-size / 2, size / 2));
            BindPoints(a, b);
            BindPoints(b, c);
            BindPoints(c, d);
            BindPoints(d, a);
            BindPoints(a, c);
            return new int[]{a,b,c,d};
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

            BindPoints(first, c,true,Color.Yellow);
            BindPoints(a, i, true, Color.Yellow);
            BindPoints(a, k, true, Color.Yellow);
            BindPoints(j, h, true, Color.Yellow);
            BindPoints(f, d, true, Color.Yellow);
            return new int[] { first,a,b,c,d,e,f,g,h,i,j,k};
        }
        public void BindPoints(int a, int b, bool isVisible = true, Color color = default)
        {
            try
            {
                stickPoints.Add(new Stick(a, b, isVisible,color));
            }
            catch
            {
                Main.NewText("dont be dumb");
            }
        }
        public void Update()
        {
            UpdatePoints();
            ConstrainToWorld();
            for (int i = 0; i < 5; i++)
            {
                UpdateSticks();
                ConstrainPoints();
            }
            UpdateStickCollision();
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
 
        public Point(Vector2 point,Vector2 oldPoint,Vector2 vel)
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
          vel = new Vector2(0,0);
          isStatic = false;
        }
        public Point(Vector2 point,Vector2 oldPoint, bool isStatic)
        {
          this.point = point;
          this.oldPoint = oldPoint;
          this.vel = new Vector2(0,0);
          this.isStatic = isStatic;
        }
        public Point(Vector2 point,Vector2 oldPoint)
        {
          this.point = point;
          this.oldPoint = oldPoint;
          vel = new Vector2(0,0);
          isStatic = false;
        }
      }
        public class Stick
        {
            public Vector2 p1;
            public Vector2 p2;
            public Vector2 oldP1;
            public Vector2 oldP2;
            public Vector2 vel1;
            public Vector2 vel2;
            public float Length;
            public bool[] isStatic;
            public int a;
            public int b;
            public bool isVisible;
            public Color color;
            public Stick(int a, int b, bool isVisible = true, Color color = default)
            {
                this.a = a;
                this.b = b;
                isStatic = new bool[2];
                p1 = points[a].point;
                p2 = points[b].point;
                oldP1 = points[a].oldPoint;
                oldP2 = points[b].oldPoint;
                vel1 = points[a].vel;
                vel2 = points[b].vel;
                float disX = points[b].point.X - points[a].point.X;
                float disY = points[b].point.Y - points[a].point.Y;
                Length = (float)Math.Sqrt(disX * disX + disY * disY);
                isStatic[0] = points[a].isStatic;
                isStatic[1] = points[b].isStatic;
                if(color == default)
                {
                    this.color = Color.DarkRed;
                }
                else
                {
                    this.color = color;
                }
                this.isVisible = isVisible;
            }
        }
        void UpdateSticks()
        {
            for (int i = 0; i < stickPoints.Count; i++)
            {
                Stick stick = stickPoints[i];
                Point p1 = points[stick.a];
                Point p2 = points[stick.b];
                float dx = p2.point.X - p1.point.X;
                float dy = p2.point.Y - p1.point.Y;
                float currentLength = (float)Math.Sqrt(dx * dx + dy * dy);
                float deltaLength = currentLength - stick.Length;
                float perc = (deltaLength / currentLength) * 0.5f;
                float offsetX = perc * dx;
                float offsetY = perc * dy;
                if (!stickPoints[i].isStatic[0])
                {
                    points[stick.a].point.X += offsetX;
                    points[stick.a].point.Y += offsetY;
                }
                if (!stickPoints[i].isStatic[1])
                {
                    points[stick.b].point.X -= offsetX;
                    points[stick.b].point.Y -= offsetY;
                }
            }
        } 
        void UpdateStickCollision()
        {
            for (int i = 0; i < stickPoints.Count; i++)
            {
                Stick stick = stickPoints[i];
                int max = 0;
                    while (!Collision.CanHit(points[stick.a].point, 1, 1, points[stick.b].point, 1, 1))
                    {
                    max++;
                        Vector2 grad = Vector2.Normalize(points[stick.a].point - points[stick.b].point);
                        Vector2 normal = grad.RotatedBy(Math.PI/2f);
                        points[stick.a].point -= normal;
                        points[stick.b].point -= normal;
                    if (max > 20)
                        break;
                    }
            }
        }
        void UpdatePoints()
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (!points[i].isStatic)
                {
                    points[i].vel.X = (points[i].point.X - points[i].oldPoint.X) * AR;
                    points[i].vel.Y = (points[i].point.Y - points[i].oldPoint.Y) * AR;
                    points[i].oldPoint.X = points[i].point.X;
                    points[i].oldPoint.Y = points[i].point.Y;
                    points[i].point.X += points[i].vel.X;
                    points[i].point.Y += points[i].vel.Y;
                    points[i].point.Y += gravity;
                }
            }
        }
        void RenderPoints()
        {

            for (int i = 0; i < points.Count; i++)
            {
                if (i == 0)
                {
                    Main.spriteBatch.Draw(Main.magicPixel, points[i].point.ForDraw(), new Rectangle(0, 0, 20, 20), Color.AliceBlue, 0f, new Rectangle(0, 0, 20, 20).Size(), 1f, SpriteEffects.None, 0f);
                }
                else
                {

                }
            }
        }
        void RenderSticks()
        {
            for (int i = 0; i < stickPoints.Count; i++)
            {
                if (stickPoints[i].isVisible)
                {
                    Vector2 p1 = points[stickPoints[i].a].point;
                    Vector2 p2 = points[stickPoints[i].b].point;
                    float Dist = Vector2.Distance(p1, p2);
                    for (float j = 0; j < 1; j += 1 / Dist)
                    {
                        Vector2 Lerped = p1 + j * (p2 - p1);
                        Main.spriteBatch.Draw(Main.magicPixel, Lerped - Main.screenPosition, new Rectangle(0, 0, 1, 1), stickPoints[i].color, 0f, new Rectangle(0, 0, 1, 1).Size(), 1f, SpriteEffects.None, 0f);
                    }
                }
            }
        }
        int[] GetContactPoints(Vector2 point)
        {
            int[] points = new int[4];
            Vector2 tileP = point / 16;
            points[0] = (int)point.X / 16;
            points[1] = (int)point.X / 16;
            points[2] = (int)point.Y / 16;
            points[3] = (int)point.Y / 16;
            while (!Framing.GetTileSafely(points[0], (int)tileP.Y).active())
            {
                points[0]++;
                if(points[0] - (int)tileP.X > 10)
                {
                    points[0] = -1;
                    break;
                }
            }
            while (!Framing.GetTileSafely(points[1], (int)tileP.Y).active())
            {
                points[1]--;
                if (points[1] - (int)tileP.X < -10)
                {
                    points[1] = -1;
                    break;
                }
            }
            while (!Framing.GetTileSafely((int)tileP.X, points[2]).active())
            {
                points[2]++;
                if (points[2] - (int)tileP.Y > 10)
                {
                    points[2] = -1;
                    break;
                }
            }
            while (!Framing.GetTileSafely((int)tileP.X, points[3]).active())
            {
                points[3]--;
                if (points[3] - (int)tileP.Y < -10)
                {
                    points[3] = -1;
                    break;
                }
            }
            for(int i = 0; i < points.Length; i++)
            {
                if (points[i] != -1)
                {
                    if (i < 2)
                    {
                        if (!Main.tileSolid[Framing.GetTileSafely(points[i], (int)tileP.Y).type])
                        {
                            points[i] = -1;
                        }
                    }
                    else
                    {
                        if (!Main.tileSolid[Framing.GetTileSafely((int)tileP.X, points[i]).type])
                        {
                            points[i] = -1;
                        }
                    }
                }
                if(points[i] != -1)
                points[i] *= 16;
            }
            return points;
        }
        void ConstrainPoints()
        {
            for (int i = 0; i < points.Count; i++)
            {
                points[i].vel.X = (points[i].point.X - points[i].oldPoint.X) * AR;
                points[i].vel.Y = (points[i].point.Y - points[i].oldPoint.Y) * AR;
                int[] ContactPoints = GetContactPoints(points[i].point);
                if (points[i].point.Y > ContactPoints[2] - fluff && ContactPoints[2] != -1)
                {
                    points[i].oldPoint.Y = ContactPoints[2] - fluff + points[i].vel.Y * bounce;
                    points[i].point.Y = ContactPoints[2] - fluff;
                }

                ContactPoints = GetContactPoints(points[i].point);
                if (points[i].point.Y < ContactPoints[3] + fluff && ContactPoints[3] != -1)
                {
                    points[i].oldPoint.Y = ContactPoints[3] + fluff + points[i].vel.Y * bounce;
                    points[i].point.Y = ContactPoints[3] + fluff;
                }
                ContactPoints = GetContactPoints(points[i].point);
                if (points[i].point.X > ContactPoints[0] - fluff && ContactPoints[0] != -1)
                    {
                        points[i].oldPoint.X = ContactPoints[0] - fluff + points[i].vel.X * bounce;
                        points[i].point.X = ContactPoints[0] - fluff;
                        
                    }
                ContactPoints = GetContactPoints(points[i].point);
                if (points[i].point.X < ContactPoints[1] + fluff && ContactPoints[1] != -1)
                    {
                        points[i].oldPoint.X = ContactPoints[1] + fluff + points[i].vel.X * bounce;
                        points[i].point.X = ContactPoints[1] + fluff;
                    }
 
 
 
            }
        }
        void ConstrainToWorld()
        {
            for (int i = 0; i < points.Count; i++)
            {
                points[i].vel.X = (points[i].point.X - points[i].oldPoint.X) * AR;
                points[i].vel.Y = (points[i].point.Y - points[i].oldPoint.Y) * AR;

                if (points[i].point.Y > Main.maxTilesY*16)
                {
                    points[i].oldPoint.Y = Main.maxTilesY * 16 + points[i].vel.Y * bounce;
                    points[i].point.Y = Main.maxTilesY * 16;
                }
                if (points[i].point.Y < 0)
                {
                    points[i].oldPoint.Y = points[i].vel.Y * bounce;
                    points[i].point.Y = 0;
                }
                if (points[i].point.X > Main.maxTilesX * 16)
                {
                    points[i].oldPoint.X = Main.maxTilesX * 16 + points[i].vel.X * bounce;
                    points[i].point.X = Main.maxTilesX * 16;
                }
                if (points[i].point.X < 0)
                {
                    points[i].oldPoint.X = points[i].vel.X * bounce;
                    points[i].point.X = 0;
                }

            }
        }
    }
}
