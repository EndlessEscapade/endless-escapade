using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles
{
    public abstract class Rapier : ModProjectile
    {
        int stealyourmom = 0;
        bool isClicking;
        public float timeForSwing;
        Vector2 firstClickPos = new Vector2();
        Vector2 midPoint = new Vector2();
        Vector2 lastClickPos = new Vector2();
        List<Vector2> positionBuffer = new List<Vector2>();
        float X(float t,
          float x0, float x1, float x2, float x3)
        {
            return (float)(
              x0 * Math.Pow((1 - t), 3) +
              x1 * 3 * t * Math.Pow((1 - t), 2) +
              x2 * 3 * Math.Pow(t, 2) * (1 - t) +
              x3 * Math.Pow(t, 3)
              );
        }
        float Y(float t,
          float y0, float y1, float y2, float y3)
        {
            return (float)(
              y0 * Math.Pow((1 - t), 3) +
              y1 * 3 * t * Math.Pow((1 - t), 2) +
              y2 * 3 * Math.Pow(t, 2) * (1 - t) +
              y3 * Math.Pow(t, 3)
              );
        }
     
        protected Player projOwner => Main.player[projectile.owner];

        public virtual List<int> exclude => new List<int> { };
        public float xDis;

        public override void AI()
        {
            projectile.direction = projOwner.direction;
            projOwner.heldProj = projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;
            projectile.position.X = projOwner.Center.X - projectile.width / 2;
            projectile.position.Y = projOwner.Center.Y - projectile.height / 2;

            if (projOwner.itemAnimation <= 1 && timeForSwing > 0.999f)
            {
                Main.NewText("f");
                projectile.Kill();
            }
            if (Main.LocalPlayer.controlUseItem)
            {
                flag = false;
                timeForSwing = 0;
                if (Main.LocalPlayer.controlUseItem)
                {
                    positionBuffer.Add(new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y));
                    stealyourmom++;
                    if (stealyourmom == 5)
                    {
                        midPoint = new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y);
                    }
                }
                if (Main.LocalPlayer.controlUseItem && !isClicking)
                {


                    firstClickPos = new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y);
                    isClicking = true;
                }
                else
                {
                    midPoint = positionBuffer[positionBuffer.Count / 2];
                    lastClickPos = new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y);
                }
            }
            else
            {
                positionBuffer.Clear();
                stealyourmom = 0;
                if (timeForSwing < 1f)
                    timeForSwing += (1 - timeForSwing) / 16f;
                isClicking = false;

                Vector2 mClickToDraw = midPoint.ForDraw();
                Vector2 fClickToDraw = firstClickPos.ForDraw();
                Vector2 lClickToDraw = lastClickPos.ForDraw();

                float x = X(timeForSwing, fClickToDraw.X, mClickToDraw.X, mClickToDraw.X, lClickToDraw.X);
                float y = Y(timeForSwing, fClickToDraw.Y, mClickToDraw.Y, mClickToDraw.Y, lClickToDraw.Y);
                float distX = x - ppostodraw.X;
                float distY = y - ppostodraw.Y;

                rotation = new Vector2(distX, distY).ToRotation();
            }
            //Vector2 Norm = Vector2.Normalize(Main.MouseWorld - projOwner.Center); //unused
        }
        bool flag;
        float rotations;
        float rotation;
        Vector2 ppostodraw => projOwner.Center.ForDraw();
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float timeForSwingSin = (float)Math.Sin(timeForSwing * 3.14f) + 0.2f;

            
            if (!Main.LocalPlayer.controlUseItem)
            {
                if (!flag)
                {
                    flag = true;
                    EEMod.primitives.CreateTrail(new Prim.RapierPrimTrail(projectile, lastClickPos, midPoint, firstClickPos));
                }


                //Helpers.DrawLine(fClickToDraw, lClickToDraw);
                //Helpers.DrawLine(fClickToDraw, mClickToDraw);
                //Helpers.DrawLine(mClickToDraw, lClickToDraw);
                //Helpers.DrawBezier(Main.magicPixel, Color.White* timeForSwingSin, lastClickPos,firstClickPos, midPoint, 1f, new Rectangle(0, 0, 1, 1));
                //Helpers.Draw(Main.magicPixel, fClickToDraw, Color.White, 1f, new Rectangle(0,0,3,3));
                //Helpers.Draw(Main.magicPixel, mClickToDraw, Color.White, 1f, new Rectangle(0, 0, 3, 3));
                //Helpers.Draw(Main.magicPixel, lClickToDraw, Color.White, 1f, new Rectangle(0, 0, 3, 3));
            }
            Main.NewText(rotations);
            rotations += (rotation - rotations) / 12f;
            float length = 30;
            Helpers.DrawLine(ppostodraw, ppostodraw + new Vector2(1, 0).RotatedBy(rotations) * length);
            return false;
        }
    }
}