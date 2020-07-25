using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.Projectiles.Ranged
{
    public class UnripeCoconutProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Unripe Coconut");
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.ranged = true;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
        }
    }
}
