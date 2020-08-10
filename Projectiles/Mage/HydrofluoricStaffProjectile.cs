using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.Projectiles.Mage
{
    public class HydrofluoricStaffProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydrofluoric Staff Bolt");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.penetrate = 2;
        }

        int progress;
        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            progress += 11;
            double deg = progress;
            double rad = deg * (Math.PI / 180);
            projectile.velocity.X -= (float)Math.Cos(rad) * (projectile.ai[0]);
            projectile.velocity.Y += (float)Math.Cos(rad) * (projectile.ai[1]);
            for (var i = 0; i < 5; i++)
            {
                int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 44, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-1f, 1f), 6, new Color(0, 255, 0, 255), 1);
                Main.dust[num].noGravity = true;
                Main.dust[num].velocity *= 1.2f;
                Main.dust[num].noLight = false;
            }
        }
    }
}
