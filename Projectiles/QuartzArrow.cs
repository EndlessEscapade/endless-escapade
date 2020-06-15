using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.Projectiles
{
    public class QuartzArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Quartz Arrow");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 600;
            projectile.ignoreWater = false;
            projectile.tileCollide = true;
            projectile.extraUpdates = 1;
            projectile.aiStyle = 0;
            projectile.arrow = true;
        }

        public override void AI()           //this make that the projectile will face the corect way
        {
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
            projectile.velocity.X *= 1.01f;
            projectile.velocity.Y *= 1.01f;
            projectile.damage = (int)Math.Sqrt(projectile.velocity.X * projectile.velocity.X + projectile.velocity.Y * projectile.velocity.Y) * 2 + 6;
            if (projectile.damage > 24)
            {
                projectile.damage = 24;
            }

            if (projectile.damage == 24)
            {
                for (var i = 0; i < 5; i++)
                {
                    int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 123, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-1f, 1f), 6, new Color(255, 217, 184, 255), projectile.scale * 0.5f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].velocity *= 2.5f;
                    Main.dust[num].noLight = false;
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //target.AddBuff(BuffID.Chilled, 100);
        }

        public override void Kill(int timeleft)
        {
            //Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 50);
            Main.PlaySound(SoundID.Item27, projectile.position);
            for (var i = 0; i < 20; i++)
            {
                int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 123, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-1f, 1f), 6, new Color(255, 217, 184, 255), projectile.scale * 0.5f);
                Main.dust[num].noGravity = true;
                Main.dust[num].velocity *= 2.5f;
                Main.dust[num].noLight = false;
            }
        }
    }
}
