using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Projectiles
{
    public class QuartzDaggerProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Quartz Dagger");
        }

        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 22;
            projectile.friendly = true;
            projectile.thrown = true;
            projectile.penetrate = 1;
            projectile.aiStyle = 2;
            projectile.timeLeft = 9999;
            aiType = 510;
        }

        public override void AI()
        {
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 0.8f;
            if (Main.rand.Next(2) == 0)
            {
                //int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 123, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-1f, 1f), 6, new Color(255, 217, 184, 255), projectile.scale * 0.5f);
            }
            if (projectile.spriteDirection == -1)
            {
                projectile.rotation -= 1.57f;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture2D = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(texture2D, projectile.Center - Main.screenPosition, null, projectile.GetAlpha(lightColor), projectile.rotation, texture2D.Size() / 2f, projectile.scale, 0, 0f);
            return false;
        }

        public override void Kill(int timeLeft)
        {

            for (int i = 0; i < 3; i++)
            {
                int projHolder = Main.rand.Next(0, 1);
                float speedX = -projectile.velocity.X * Main.rand.NextFloat(-.4f, .8f) + Main.rand.NextFloat(-2f, 2f);
                float speedY = -projectile.velocity.Y * Main.rand.Next(34, 37) * 0.01f + Main.rand.NextFloat(-12f, 12.1f) * 0.4f;
                if (projHolder == 0)
                    Projectile.NewProjectile(projectile.Center.X + speedX, projectile.Center.Y + speedY, speedX, speedY, ModContent.ProjectileType<Shard2>(), (int)(projectile.damage * 0.4), 0f, projectile.owner, 0f, 0f);
                if (projHolder == 1)
                    Projectile.NewProjectile(projectile.Center.X + speedX, projectile.Center.Y + speedY, speedX, speedY, ModContent.ProjectileType<Shard1>(), (int)(projectile.damage * 0.3), 0f, projectile.owner, 0f, 0f);
                Main.PlaySound(SoundID.Item27, projectile.position);
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

        }
    }
}
