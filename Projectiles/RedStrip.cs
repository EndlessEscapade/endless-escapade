using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace EEMod.Projectiles
{
    public class RedStrip : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("RedStrip");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 8;
            projectile.alpha = 0;
            projectile.timeLeft = 600;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale *= 0.7f;
        }

        private void LookToPlayer()
        {
            Player player = Main.player[projectile.owner];
            Vector2 look = Main.player[projectile.owner].Center - projectile.Center;
            LookInDirectionP(look);
        }
        private void LookInDirectionP(Vector2 look)
        {
            float angle = 0.5f * (float)Math.PI;
            if (look.X != 0f)
            {
                angle = (float)Math.Atan(look.Y / look.X);
            }
            else if (look.Y < 0f)
            {
                angle += (float)Math.PI;
            }
            if (look.X < 0f)
            {
                angle += (float)Math.PI;
            }
            projectile.rotation = angle;
        }
        public override void AI()
        {
            projectile.rotation = new Vector2(projectile.ai[0], projectile.ai[1]).ToRotation();
        }

    }
}
