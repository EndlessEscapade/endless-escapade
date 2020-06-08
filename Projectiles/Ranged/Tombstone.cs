using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.Projectiles.Ranged
{
    public class Tombstone : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tombstone");
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
            projectile.rotation++;
            if(projectile.velocity.Y < 16)
            {
                projectile.velocity.Y *= 1.003f;
            }
        }
    }
}
