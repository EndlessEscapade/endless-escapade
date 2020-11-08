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
        }

        public override void SetDefaults()
        {
            projectile.width = 54;
            projectile.height = 60;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.scale = 1f;

            projectile.melee = true;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.damage = 20;
            projectile.knockBack = 3.5f;
        }

        public override void AI()
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
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Main.spriteBatch.Draw(mod.GetTexture("Projectiles/Melee/LythenWarhammerProjectileGlow"), new Rectangle((int)(projectile.Center.X - Main.screenPosition.X), (int)(projectile.Center.Y - Main.screenPosition.Y), 54, 60), Main.projectileTexture[projectile.type].Bounds, Color.White, projectile.rotation, new Vector2(27, 30), SpriteEffects.None, 0);
        }
    }
}