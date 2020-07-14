
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace EEMod.Projectiles.CoralReefs
{
    public class CBPetrude : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubble");
        }

        public override void SetDefaults()
        {
            projectile.width = 100;
            projectile.height = 48;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.scale = projectile.ai[0];
            projectile.alpha = (int)projectile.ai[1];
            projectile.tileCollide = false;
            projectile.timeLeft = 80;
        }
        int sinControl;
        public override void AI()
        {
            projectile.alpha++;
            if (projectile.alpha > 255)
            {
                projectile.alpha = 255;
            }
            projectile.velocity *= .97f;
            projectile.scale = projectile.ai[0];
            projectile.alpha = (int)projectile.ai[1];
            if (sinControl == 0)
            {
                projectile.velocity.Y -= 8;
            }
            sinControl++;
            if (projectile.ai[1] < 140)
                projectile.velocity.X += (float)Math.Sin(sinControl / (projectile.ai[1] / 13)) / (projectile.ai[1] / 10);
            else if (projectile.ai[1] < 160)
                projectile.position.X += (float)Math.Sin(sinControl / (projectile.ai[1] / 13)) / (projectile.ai[1] / 4);
            else
                projectile.velocity.X -= (float)Math.Sin(sinControl / (projectile.ai[1] / 13)) / (projectile.ai[1] / 10);
        }
        public override void Kill(int timeLeft)
        {
            for (var a = 0; a < 5; a++)
            {
                Vector2 vector = new Vector2(0, 10).RotatedBy(((Math.PI * 0.4) * a), default);
                int index = Dust.NewDust(projectile.Center, 22, 22, DustID.BlueCrystalShard, vector.X, vector.Y, 0, Color.Blue, 1f);
                Main.dust[index].velocity *= .4f;
                Main.dust[index].noGravity = true;
                Main.dust[index].noLight = true;
            }
        }
    }
}
