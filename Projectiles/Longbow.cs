using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Projectiles.Ranged;

namespace EEMod.Projectiles
{
    public abstract class Longbow : ModProjectile
    {
        public virtual float speedOfArrow => 2;
        public virtual int newProj => ModContent.ProjectileType<CoralArrowProjectile>();
        public virtual float minGrav => 2;
        public virtual float ropeThickness => 32f;
        protected float progression => (projOwner.itemAnimation / (float)projOwner.itemAnimationMax);
        protected Player projOwner => Main.player[projectile.owner];


        public virtual List<int> exclude => new List<int> {};
        public float xDis;
        float Max = 100;
        bool vanillaFlag;
        public override void AI()
        {
            projectile.direction = projOwner.direction;
            projOwner.heldProj = projectile.whoAmI;
            projectile.rotation = (Main.MouseWorld - projOwner.Center).ToRotation();
            projectile.position.X = projOwner.Center.X - projectile.width / 2;
            projectile.position.Y = projOwner.Center.Y - projectile.height / 2;
            float speed = speedOfArrow;
            projOwner.bodyFrame.Y = 56*(6 + (int)(gravAccel - minGrav));
            if (!projOwner.controlUseItem)
            {
                projectile.Kill();
                Projectile.NewProjectile(projOwner.Center, ((Main.MouseWorld - projOwner.Center) / Max) * speed, newProj, 10, 10f, Main.myPlayer, (gravAccel / Max) * 2 * speed * speed, projectile.ai[1]);
            }
            if(Math.Abs(gravAccel - minGrav) < 0.3f && !vanillaFlag)
            {
                for (int i = 0; i < 360; i += 10)
                {
                    float xdist = (int)(Math.Sin(i * (Math.PI / 180)) * 15);
                    float ydist = (int)(Math.Cos(i * (Math.PI / 180)) * 15);
                    Vector2 offset = new Vector2(xdist, ydist);
                    Dust dust = Dust.NewDustPerfect(projectile.Center + offset, 113, offset * 0.5f);
                    dust.noGravity = true;
                    dust.velocity *= 0.94f;
                    dust.noLight = false;
                }
                vanillaFlag = true;
                projectile.ai[1] = 1;
            }
        }
        float gravAccel = 4;
        int yeet;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            yeet++;
            if(yeet > 100)
            {
                yeet = -100;
            }
            float grav = 0;
            gravAccel += (minGrav - gravAccel) / ropeThickness;
            for (int i = 0; i < Max; i++)
            {
                float diff = 1 - (Math.Abs(i - yeet)/50f);
                if (diff < 0)
                    diff = 0;
                grav += gravAccel;
                Vector2 intendedPath = projOwner.Center + (Main.MouseWorld - projOwner.Center + new Vector2(0, grav)) * (i / Max);
                spriteBatch.Draw(Main.magicPixel, intendedPath - Main.screenPosition, new Rectangle(0, 0, 2, 2), Color.White * (1 - (i / Max)) * (diff),0f, new Rectangle(0, 0, 2, 2).Size() / 2,1, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}
