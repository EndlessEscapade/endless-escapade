using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace EEMod.Projectiles
{
    public class DesertArrowProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Desert Arrow");
        }

        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 22;
            projectile.friendly = true;
            projectile.thrown = true;
            projectile.penetrate = 2;
            projectile.aiStyle = 2;
            projectile.timeLeft = 9999;
            aiType = 510;
        }

        public bool visible = true;
        public override void AI()
        {
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 0.8f+(float)(Math.PI/4);
            if (Main.rand.Next(2) == 0)
            {
                //int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 123, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-1f, 1f), 6, new Color(255, 217, 184, 255), projectile.scale * 0.5f);
            }
            if (projectile.spriteDirection == -1)
            {
                projectile.rotation -= 1.57f;
            }
            for (var i = 0; i < 2; i++)
            {
                int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 0, 0, 0, 6, default, projectile.scale*0.7f);
                Main.dust[num].noGravity = true;
                Main.dust[num].noLight = false;
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
          for (var i = 0; i < 10; i++)
          {
              int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 0, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f), 6, default, projectile.scale);
              Main.dust[num].noGravity = false;
              Main.dust[num].velocity *= 2.5f;
              Main.dust[num].noLight = false;
          }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
          projectile.velocity.X *= Main.rand.NextFloat(-.1f,-.4f);
          projectile.velocity.Y *= Main.rand.NextFloat(-.8f,-.5f);
          projectile.velocity.Y += Main.rand.NextFloat(-1,-1.5f);
        }
    }
}
