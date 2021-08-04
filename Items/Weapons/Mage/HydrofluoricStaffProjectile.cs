using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Mage
{
    public class HydrofluoricStaffProjectile : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydrofluoric Staff Bolt");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.magic = true;
            Projectile.penetrate = 2;
        }

        private int progress;

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            progress += 11;
            double deg = progress;
            double rad = deg * (Math.PI / 180);
            Projectile.velocity.X -= (float)Math.Cos(rad) * Projectile.ai[0];
            Projectile.velocity.Y += (float)Math.Cos(rad) * Projectile.ai[1];
        }
    }
}