using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Tiles
{
    public class Bridge : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tile Experimentation");
        }
        private static float X(float t,
   float x0, float x1, float x2, float x3)
        {
            return (float)(
                x0 * Math.Pow((1 - t), 3) +
                x1 * 3 * t * Math.Pow((1 - t), 2) +
                x2 * 3 * Math.Pow(t, 2) * (1 - t) +
                x3 * Math.Pow(t, 3)
            );
        }
        private static float Y(float t,
            float y0, float y1, float y2, float y3)
        {
            return (float)(
                 y0 * Math.Pow((1 - t), 3) +
                 y1 * 3 * t * Math.Pow((1 - t), 2) +
                 y2 * 3 * Math.Pow(t, 2) * (1 - t) +
                 y3 * Math.Pow(t, 3)
             );
        }
        public override void SetDefaults()
        {
            projectile.width = 2;
            projectile.height = 2;
            projectile.alpha = 0;
            projectile.timeLeft = 10000;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale *= 1f;
        }

        Vector2 endPoints = Main.LocalPlayer.Center - new Vector2(200, 200);
        Vector2 startingPos = Main.LocalPlayer.Center;
        Vector2 c1 = Main.LocalPlayer.Center - new Vector2(120, 100);
        Vector2 c2 = Main.LocalPlayer.Center - new Vector2(120, 100);

        float dipX = (Main.LocalPlayer.Center - new Vector2(120, 210)).X;
        float dipY = (Main.LocalPlayer.Center - new Vector2(120, 210)).Y;
        float accelY;
        float accelX;
        float accel2 = 1;
        float accel3;
        float amplitude;
        float maxSpeedY = 10;
        float maxSpeedX = 10;
        float firstPosX = (Main.LocalPlayer.Center).X;
        float secondPosX = (Main.LocalPlayer.Center - new Vector2(200, 200)).X;
        float firstPosY = (Main.LocalPlayer.Center).Y;
        float secondPosY = (Main.LocalPlayer.Center - new Vector2(200, 200)).Y;
        public static float checkForLowest;
        float trueControlPoint;
        public override void AI()
        {
            trueControlPoint = (firstPosX + secondPosX) / 2;
            if (secondPosY > firstPosY)
                checkForLowest = secondPosY;
            else
                checkForLowest = firstPosY;
            if (Main.LocalPlayer.controlUp)
            {
                firstPosX = Main.mouseX + Main.screenPosition.X;
                firstPosY = Main.mouseY + Main.screenPosition.Y;
                accelX = 0;
                accelY = 0;
                accel2 = 1;
                amplitude = dipY;
                if (dipY >= secondPosY)
                    maxSpeedY = (dipY - secondPosY) / 20;
                else
                    maxSpeedY = (secondPosY - dipY) / 20;
                if (dipX >= trueControlPoint)
                    maxSpeedX = (dipX - (trueControlPoint)) / 20;
                else
                    maxSpeedX = ((trueControlPoint) - dipX) / 20;
            }
            if (Main.LocalPlayer.controlUseItem)
            {
                dipX = Main.mouseX + Main.screenPosition.X;
                dipY = Main.mouseY + Main.screenPosition.Y;
                accelX = 0;
                accelY = 0;
                accel2 = 1;
                amplitude = dipY;
                if (dipY >= checkForLowest)
                    maxSpeedY = (dipY - checkForLowest) / 20;
                else
                    maxSpeedY = (checkForLowest - dipY) / 20;
                if (dipX >= checkForLowest)
                    maxSpeedX = (dipX - checkForLowest) / 20;
                else
                    maxSpeedX = (checkForLowest - dipX) / 20;
                
            }
            secondPosX = endPoints.X;
            secondPosY = endPoints.Y;
            if (dipX >= trueControlPoint)
            {
                maxSpeedX *= 0.994f;
                accel3 = (dipX - trueControlPoint) / 100f;
                if (accel3 > 1)
                    accel3 = 1;
                accelX -= accel2 * accel3;
                dipX += accelX;
                if (accelX < -maxSpeedX)
                    accelX = -maxSpeedX;
            }
            if (dipX <= trueControlPoint)
            {
                maxSpeedX *= 0.994f;
                accel3 = (trueControlPoint - dipX) / 100f;
                if (accel3 > 1)
                    accel3 = 1;

                accelX += accel2 * accel3;
                dipX += accelX;
                if (accelX < -maxSpeedX)
                    accelX = -maxSpeedX;
            }

            //other
            if (dipY >= checkForLowest)
            {
                maxSpeedY *= 0.989f;
                accel3 = (dipY - checkForLowest) / 100f;
                if (accel3 > 1)
                    accel3 = 1;

                dipY += accelY;
                accelY -= accel2 * accel3;
                if (accelY < -maxSpeedY)
                    accelY = -maxSpeedY;
            }
            else
            {
                maxSpeedY *= 0.989f;
                accel3 = (checkForLowest - dipY) / 100f;
                if (accel3 > 1)
                    accel3 = 1;
                dipY += accelY;
                accelY += accel2 * accel3;
                if (accelY > maxSpeedY)
                    accelY = maxSpeedY;
            }
            projectile.Center = new Vector2(X(projectile.ai[1], firstPosX, dipX, dipX, endPoints.X), Y(projectile.ai[1], firstPosY, dipY, dipY, endPoints.Y));
            projectile.ai[0] += 0.1f;
            projectile.velocity.Y += (float)Math.Sin(projectile.ai[0]) * 0.1f;
            Rectangle upperPortion = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, 3);
            Rectangle upperPortionWholeEntityCheck = new Rectangle((int)projectile.position.X, (int)projectile.position.Y - 10, projectile.width, 13);
            Rectangle lowerPortion = new Rectangle((int)projectile.position.X, (int)projectile.position.Y + projectile.height - 2, projectile.width, 2);
            Rectangle playerHitBoxFeet = new Rectangle((int)Main.LocalPlayer.position.X, (int)Main.LocalPlayer.position.Y + Main.LocalPlayer.height-(int)(Main.LocalPlayer.velocity.Y/2) -10, Main.LocalPlayer.width, (int)Math.Round(projectile.velocity.Y) + (int)(Main.LocalPlayer.velocity.Y / 2) + 10);

            if (playerHitBoxFeet.Intersects(upperPortion) && Main.LocalPlayer.velocity.Y >= 0)
            {
                Main.LocalPlayer.velocity.Y = 0;
                Main.LocalPlayer.position.Y = projectile.position.Y - Main.LocalPlayer.height + 1;
                trueControlPoint = Main.LocalPlayer.Center.X;
            }
            if(playerHitBoxFeet.Intersects(upperPortionWholeEntityCheck) && Main.LocalPlayer.velocity.Y >= 0)
            {
                Main.LocalPlayer.bodyFrameCounter += (double)Math.Abs(Main.LocalPlayer.velocity.X) * 1.5;
                Main.LocalPlayer.bodyFrame.Y = Main.LocalPlayer.legFrame.Y;
                Main.LocalPlayer.legFrameCounter += (double)Math.Abs(Main.LocalPlayer.velocity.X) * 1.3;
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
        }

    }
}
