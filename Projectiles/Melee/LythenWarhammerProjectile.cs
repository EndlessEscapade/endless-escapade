using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace EEMod.Projectiles.Melee
{
    public class LythenWarhammerProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lythen Warhammer");
            Main.projFrames[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = 48;
            projectile.height = 48;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.scale = 1f;

            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.damage = 20;
            projectile.knockBack = 3.5f;
        }
        double radians = 0;
        int flickerTime = 0;
        int chargeTime = 90;
        //ai[0] = charge
        //ai[1] = Whether or not thrown
        int height = 60;
        int width = 54;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.4f));
            if (projectile.ai[1] == 0)
            {
                projectile.scale = MathHelper.Clamp(projectile.ai[0] / 10, 0, 1);
                if (player.direction == 1)
                {
                    radians += (double)((projectile.ai[0] + 10) / 300);
                }
                else
                {
                    radians -= (double)((projectile.ai[0] + 10) / 300);
                }
                if (radians > 6.28)
                {
                    radians -= 6.28;
                }
                if (radians < -6.28)
                {
                    radians += 6.28;
                }
                player.itemAnimation -= (int)((projectile.ai[0] + 50) / 10);

                while (player.itemAnimation < 3)
                {
                    player.itemAnimation += 320;
                }
                player.itemTime = player.itemAnimation;
                projectile.velocity = Vector2.Zero;
                projectile.position = player.position;

                if (projectile.ai[0] < chargeTime)
                {
                    projectile.ai[0]++;
                    if (projectile.ai[0] == chargeTime)
                    {
                        Main.PlaySound(SoundID.NPCDeath7, projectile.Center);
                    }
                }
                Vector2 direction = Main.MouseWorld - player.position;
                direction.Normalize();
                double throwingAngle = direction.ToRotation() + 3.14;
                if (player.direction != 1)
                {
                    throwingAngle -= 6.28;
                }
                if (!player.channel && Math.Abs(radians - throwingAngle) < 1)
                {
                    projectile.ai[1] = 1;
                    player.itemTime = 2;
                    player.itemAnimation = 2;

                    projectile.tileCollide = true;
                    projectile.penetrate = 1;
                    projectile.timeLeft = 60;
                    projectile.position = player.position;

                    direction *= 20;
                    projectile.velocity = direction;
                }
                projectile.position.Y = player.Center.Y - (int)(Math.Sin(radians * 0.96) * 40) - (projectile.height / 2);
                projectile.position.X = player.Center.X - (int)(Math.Cos(radians * 0.96) * 40) - (projectile.width / 2);
            }
            else
            {
                if (projectile.ai[0] < chargeTime)
                {
                    projectile.active = false;
                }
                else 
                {
                    EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.4f));
                    EEMod.Particles.Get("Main").SpawnParticles(projectile.Center + (projectile.velocity * 5), new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)) * 2, 2, Color.Cyan, new SlowDown(0.99f), new ZigzagMotion(10, 1.5f), new AfterImageTrail(0.5f));
                    projectile.rotation = projectile.velocity.ToRotation() + 0.78f;
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            if (projectile.ai[1] == 0)
            {
                Color color = lightColor;
                Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center - Main.screenPosition, new Rectangle(0, 0, width, height), color, (float)radians + 3.9f, new Vector2(0, height), projectile.scale, SpriteEffects.None, 0);
                if (projectile.ai[0] >= chargeTime && projectile.ai[1] == 0 && flickerTime < 16)
                {
                    flickerTime++;
                    color = Color.White;
                    float flickerTime2 = (float)(flickerTime / 20f);
                    float alpha = 1.5f - (((flickerTime2 * flickerTime2) / 2) + (2f * flickerTime2));
                    if (alpha < 0)
                    {
                        alpha = 0;
                    }
                    Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center - Main.screenPosition, new Rectangle(0, height, width, height), color * alpha, (float)radians + 3.9f, new Vector2(0, height), projectile.scale, SpriteEffects.None, 1);
                }
                return false;
            }
            return true;
        }

       /* public override void AI()
        {
            Player owner = Main.player[projectile.owner];
            EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.4f));
            if (projectile.ai[0] < 80)
            {
                if (owner.controlUseItem)
                {
                    projectile.ai[0]++;
                    if (owner.velocity.Y > -4)
                    {
                        owner.velocity.Y -= 0.5f;
                    }
                }
                projectile.Center = owner.Center;
            }
            if (projectile.ai[0] >= 40 && projectile.ai[0] <= 80)
            {
                Dust dust = Dust.NewDustPerfect(owner.position + new Vector2(10 + (owner.direction * 3), 13), DustID.Electric, newColor: Color.Cyan);
                dust.velocity = Vector2.Zero;
                dust.noGravity = true;
            }
            if (projectile.ai[0] == 80)
            {
                if (!owner.controlUseItem)
                {
                    projectile.velocity = Vector2.Normalize(projectile.Center - Main.MouseWorld) * -12;
                    projectile.ai[0]++;
                }
                else
                {
                    owner.Center = projectile.Center;
                    owner.velocity.X = 0;
                    owner.velocity.Y = 0;
                }
            }
            else
            {
                projectile.velocity *= 1.02f;
                EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.4f));
                EEMod.Particles.Get("Main").SpawnParticles(projectile.Center, new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)) * 2, 2, Color.Cyan, new SlowDown(0.99f), new ZigzagMotion(10, 1.5f), new AfterImageTrail(0.5f));
            }
            projectile.rotation += projectile.ai[0] / 80;
        }*/

    }
}