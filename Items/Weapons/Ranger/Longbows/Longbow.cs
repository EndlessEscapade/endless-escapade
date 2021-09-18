using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using EEMod.Prim;

namespace EEMod.Items.Weapons.Ranger.Longbows
{
    public abstract class Longbow : EEProjectile
    {
        public virtual float speedOfArrow => 2;
        public virtual int newProj => ModContent.ProjectileType<CoralArrowProjectileLongbow>();
        public virtual float minGrav => 2;
        public virtual float ropeThickness => 32f;
        public virtual bool showDots => true;
        public virtual int projCount => 1;
        public virtual float projSpread => 0;
        protected float progression => projOwner.itemAnimation / (float)projOwner.itemAnimationMax;
        protected Player projOwner => Main.player[Projectile.owner];

        public virtual List<int> exclude => new List<int> { };
        public float xDis;
        private readonly float Max = 100;
        private bool vanillaFlag;

        public override void AI()
        {
            Projectile.direction = projOwner.direction;
            projOwner.heldProj = Projectile.whoAmI;
            Projectile.rotation = (Main.MouseWorld - projOwner.Center).ToRotation();
            Projectile.position.X = projOwner.Center.X - Projectile.width / 2;
            Projectile.position.Y = projOwner.Center.Y - Projectile.height / 2;
            float speed = speedOfArrow;
            projOwner.bodyFrame.Y = 56 * (6 + (int)(gravAccel - minGrav));
            if (!projOwner.controlUseItem)
            {
                Projectile.Kill();

                Vector2 comedy = Vector2.Normalize(Main.MouseWorld - projOwner.Center);
                for (float i = 0; i < projCount; i++)
                {
                    Projectile projectile2 = Projectile.NewProjectileDirect(new Terraria.DataStructures.ProjectileSource_ProjectileParent(Projectile), projOwner.Center, comedy.RotatedBy(-(projCount / 2) + i) / Max * speed, newProj, 10, 10f, Main.myPlayer);
                }
            }
            if (Math.Abs(gravAccel - minGrav) < 0.3f && !vanillaFlag)
            {
                for (int i = 0; i < 360; i += 10)
                {
                    float xdist = (int)(Math.Sin(i * (Math.PI / 180)) * 15);
                    float ydist = (int)(Math.Cos(i * (Math.PI / 180)) * 15);
                    Vector2 offset = new Vector2(xdist, ydist);
                    Dust dust = Dust.NewDustPerfect(Projectile.Center + offset, 113, offset * 0.5f);
                    dust.noGravity = true;
                    dust.velocity *= 0.94f;
                    // dust.noLight = false;
                }
                vanillaFlag = true;
                Projectile.ai[1] = 1;
            }
        }

        private float gravAccel = 4;
        private int yeet;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (showDots)
            {
                yeet++;
                if (yeet > 100)
                {
                    yeet = -100;
                }
                float grav = 0;
                gravAccel += (minGrav - gravAccel) / ropeThickness;
                for (int i = 0; i < Max; i++)
                {
                    float diff = 1 - (Math.Abs(i - yeet) / 50f);
                    if (diff < 0)
                    {
                        diff = 0;
                    }

                    grav += gravAccel;
                    Vector2 intendedPath = projOwner.Center + (Main.MouseWorld - projOwner.Center + new Vector2(0, grav)) * (i / Max);
                    spriteBatch.Draw(Terraria.GameContent.TextureAssets.MagicPixel.Value, intendedPath - Main.screenPosition, new Rectangle(0, 0, 2, 2), Color.White * (1 - (i / Max)) * diff, 0f, new Vector2(2, 2) / 2, 1, SpriteEffects.None, 0f);
                }
            }
            return true;
        }
    }
}