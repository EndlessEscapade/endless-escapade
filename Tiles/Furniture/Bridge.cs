using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Tiles.Furniture
{
    public class Bridge : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tile Experimentation");
        }

        private static float X(float t,
   float x0, float x1, float x2, float x3)
        {
            return (float)(
                x0 * Math.Pow(1 - t, 3) +
                x1 * 3 * t * Math.Pow(1 - t, 2) +
                x2 * 3 * Math.Pow(t, 2) * (1 - t) +
                x3 * Math.Pow(t, 3)
            );
        }

        private static float Y(float t,
            float y0, float y1, float y2, float y3)
        {
            return (float)(
                 y0 * Math.Pow(1 - t, 3) +
                 y1 * 3 * t * Math.Pow(1 - t, 2) +
                 y2 * 3 * Math.Pow(t, 2) * (1 - t) +
                 y3 * Math.Pow(t, 3)
             );
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (isSupport)
            {
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Tiles/Furniture/BridgeSupport").Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 16, 16), lightColor, Projectile.rotation, new Vector2(16, 16) / 2, 1, SpriteEffects.None, 0);
                return false;
            }
            return true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.alpha = 0;
            Projectile.timeLeft = 10000;
            Projectile.penetrate = -1;
            // Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            // Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale *= 1f;
        }

        public Vector2 endPoints = Vector2.Zero;
        public Vector2 c1 = Vector2.Zero;

        public float dipX;
        public float dipY;
        public float accelY;
        public float accelX;
        public float accel2 = 1;
        public float accel3;
        public float amplitude;
        public float maxSpeedY = 10;
        public float maxSpeedX = 10;
        public float firstPosX = 0;
        public float secondPosX = 0;
        public float firstPosY = 0;
        public float secondPosY = 0;
        public static float checkForLowest;
        //public static float trueControlPoint = (Main.LocalPlayer.Center.X + (Main.LocalPlayer.Center - new Vector2(200, 200)).X) / 2;
        public float chainsPerUse;
        public float rotDis;
        public bool isSupport;

        public override void AI()
        {
            dipX = c1.X;
            dipY = c1.Y + 90;

            Rectangle upperPortion = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y + 13, Projectile.width, 3);
            Rectangle upperPortionWholeEntityCheck = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y - 10 + 13, Projectile.width, 13);
            Rectangle lowerPortion = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y + Projectile.height - 2, Projectile.width, 2);
            Rectangle playerHitBoxFeet = new Rectangle((int)Main.LocalPlayer.position.X, (int)Main.LocalPlayer.position.Y + Main.LocalPlayer.height - (int)(Main.LocalPlayer.velocity.Y / 2) - 10, Main.LocalPlayer.width, (int)Math.Round(Projectile.velocity.Y) + (int)(Main.LocalPlayer.velocity.Y / 2) + 10);
            
            if (playerHitBoxFeet.Intersects(upperPortion) && Main.LocalPlayer.velocity.Y >= 0)
            {
                Main.LocalPlayer.velocity.Y = 0;
                Main.LocalPlayer.position.Y = Projectile.position.Y - Main.LocalPlayer.height + 16;
            }
            if (playerHitBoxFeet.Intersects(upperPortionWholeEntityCheck) && Main.LocalPlayer.velocity.Y >= 0)
            {
                Main.LocalPlayer.bodyFrameCounter += Math.Abs(Main.LocalPlayer.velocity.X) * 1.5;
                Main.LocalPlayer.bodyFrame.Y = Main.LocalPlayer.legFrame.Y;
                Main.LocalPlayer.legFrameCounter += Math.Abs(Main.LocalPlayer.velocity.X) * 1.3;
                while (Main.LocalPlayer.legFrameCounter > 8.0)
                {
                    Main.LocalPlayer.legFrameCounter -= 8.0;
                    Main.LocalPlayer.legFrame.Y += Main.LocalPlayer.legFrame.Height;
                }
                if (Main.LocalPlayer.legFrame.Y < Main.LocalPlayer.legFrame.Height * 7)
                {
                    Main.LocalPlayer.legFrame.Y = Main.LocalPlayer.legFrame.Height * 19;
                }
                else if (Main.LocalPlayer.legFrame.Y > Main.LocalPlayer.legFrame.Height * 19)
                {
                    Main.LocalPlayer.legFrame.Y = Main.LocalPlayer.legFrame.Height * 7;
                }
            }
            if (secondPosY > firstPosY)
            {
                checkForLowest = secondPosY;
            }
            else
            {
                checkForLowest = firstPosY;
            }

            secondPosX = endPoints.X;
            secondPosY = endPoints.Y;

            /*if (dipX >= trueControlPoint)
            {
                maxSpeedX *= 0.994f;
                accel3 = (dipX - trueControlPoint) / 100f;
                if (accel3 > 1)
                {
                    accel3 = 1;
                }

                accelX -= accel2 * accel3;
                dipX += accelX;
                if (accelX < -maxSpeedX)
                {
                    accelX = -maxSpeedX;
                }
            }
            if (dipX <= trueControlPoint)
            {
                maxSpeedX *= 0.994f;
                accel3 = (trueControlPoint - dipX) / 100f;
                if (accel3 > 1)
                {
                    accel3 = 1;
                }

                accelX += accel2 * accel3;
                dipX += accelX;
                if (accelX < -maxSpeedX)
                {
                    accelX = -maxSpeedX;
                }
            }*/

            //other

            if (dipY >= checkForLowest)
            {
                maxSpeedY *= 0.989f;
                accel3 = (dipY - checkForLowest) / 100f;
                if (accel3 > 1)
                {
                    accel3 = 1;
                }

                dipY += accelY;
                accelY -= accel2 * accel3;
                if (accelY < -maxSpeedY)
                {
                    accelY = -maxSpeedY;
                }
            }
            else
            {
                maxSpeedY *= 0.989f;
                accel3 = (checkForLowest - dipY) / 100f;
                if (accel3 > 1)
                {
                    accel3 = 1;
                }

                dipY += accelY;
                accelY += accel2 * accel3;
                if (accelY > maxSpeedY)
                {
                    accelY = maxSpeedY;
                }
            }

            Projectile.Center = new Vector2(X(Projectile.ai[1], firstPosX, dipX, dipX, endPoints.X), Y(Projectile.ai[1], firstPosY, dipY, dipY, endPoints.Y));
            
            Vector2 distBetween = new Vector2(X(Projectile.ai[1], firstPosX, dipX, dipX, endPoints.X) -
                    X(Projectile.ai[1] - chainsPerUse, firstPosX, dipX, dipX, endPoints.X),
                    Y(Projectile.ai[1], firstPosY, dipY, dipY, endPoints.Y) -
                    Y(Projectile.ai[1] - chainsPerUse, firstPosY, dipY, dipY, endPoints.Y));
            
            float projTrueRotation = distBetween.ToRotation() + rotDis;
            
            Projectile.rotation = projTrueRotation;
            Projectile.ai[0] += 0.1f;
            Projectile.velocity.Y += (float)Math.Sin(Projectile.ai[0]) * 0.03f;
        }
    }
}