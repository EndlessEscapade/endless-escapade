using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

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

        private int progress;

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            progress += 11;
            double deg = progress;
            double rad = deg * (Math.PI / 180);
            projectile.velocity.X -= (float)Math.Cos(rad) * projectile.ai[0];
            projectile.velocity.Y += (float)Math.Cos(rad) * projectile.ai[1];
        }
    }
}