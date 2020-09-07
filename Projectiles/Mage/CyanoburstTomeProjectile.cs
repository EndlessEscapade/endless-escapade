using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Mage
{
    public class CyanoburstTomeProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cyanoburst Plankton");
        }

        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.timeLeft = 420;
            projectile.ignoreWater = true;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.penetrate = -1;
        }

        private Vector2 firstVel;

        public override void AI()
        {
            if (firstVel == default)
            {
                firstVel = Vector2.Normalize(projectile.velocity) * 2;
            }
            projectile.rotation = projectile.velocity.ToRotation() - (float)Math.PI / 2;
            projectile.ai[0] += 11;
            double deg = projectile.ai[0];
            double rad = deg * (Math.PI / 180);
            projectile.velocity.X -= (float)Math.Cos(rad) * (firstVel.X);
            projectile.velocity.Y += (float)Math.Cos(rad) * (firstVel.Y);
            for (var i = 0; i < 3; i++)
            {
                int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.GreenBlood, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-1f, 1f), 6, new Color(0, 255, 0, 255), 1);
                Main.dust[num].noGravity = true;
                Main.dust[num].velocity *= 1.2f;
                Main.dust[num].noLight = false;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 360; i += 5)
            {
                float xdist = (int)(Math.Sin(i * (Math.PI / 180)) * 15);
                float ydist = (int)(Math.Cos(i * (Math.PI / 180)) * 15);
                Vector2 offset = new Vector2(xdist, ydist);
                Dust dust = Dust.NewDustPerfect(projectile.Center + offset, DustID.GreenBlood, offset * 0.5f);
                dust.noGravity = true;
                dust.noLight = false;
            }
            Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<CyanoburstTomeKelp>(), 10, 10f, Main.myPlayer);
        }
    }
}