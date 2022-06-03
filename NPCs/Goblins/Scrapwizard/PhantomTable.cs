using EEMod.Extensions;
using EEMod.Prim;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.Goblins.Scrapwizard
{
    public class PhantomTable : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phantom Table");
        }

        public override void SetDefaults()
        {
            Projectile.width = 160;
            Projectile.height = 32;

            Projectile.alpha = 0;

            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.scale = 1f;

            Projectile.aiStyle = -1;

            Projectile.tileCollide = false;

            Projectile.damage = 0;

            Projectile.timeLeft = 1000000;
        }

        public Vector2 desiredCenter;
        public Vector2 initCenter;
        public Vector2 oldCenter;

        public Vector2 falseVelocity;

        public int dyingTicks = 0;

        public bool colliding;

        public Vector2 offsetPos;
        public Vector2 offsetVel;


        public int slamTicks;

        public override void AI()
        {
            //Reffed from Spirit w/ permission

            if (dyingTicks <= 0)
            {
                if (Projectile.ai[0] <= 40)
                {
                    initCenter = Projectile.Center;
                }
                else if (Projectile.ai[0] <= 80 && Projectile.ai[0] > 40)
                {
                    Projectile.Center = Vector2.SmoothStep(initCenter, desiredCenter, (Projectile.ai[0] - 40) / 40f);
                }
                else
                {
                    if (slamTicks > 120)
                    {
                        Projectile.Center = desiredCenter + offsetPos;

                        //shake
                    }
                    else if (slamTicks > 60)
                    {
                        Projectile.Center += new Vector2(0, (140 - slamTicks) / 15f);

                        //slam
                    }
                    else if (slamTicks > 0)
                    {
                        Projectile.Center = Vector2.SmoothStep(Projectile.Center, desiredCenter, (60 - slamTicks) / 60f);
                    }
                    else
                    {
                        Projectile.Center = desiredCenter + offsetPos;
                    }

                    offsetPos += offsetVel;

                    offsetVel.Y *= 0.85f;

                    Projectile.rotation *= 0.95f;

                    slamTicks--;
                }
            }
            else
            {
                if (dyingTicks <= 40)
                {
                    Projectile.Center = Vector2.SmoothStep(desiredCenter, initCenter, dyingTicks / 40f);
                }
                else
                {
                    Projectile.Kill();
                }

                dyingTicks++;
            }

            Projectile.ai[0]++;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/PhantomTableBloom").Value, Projectile.Center - Main.screenPosition, Color.Pink, 1f, Projectile.rotation);

            return true;
        }
    }
}