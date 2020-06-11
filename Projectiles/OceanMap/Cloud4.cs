using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace EEMod.Projectiles.OceanMap
{
    public class Cloud4 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cloud");
        }

        public override void SetDefaults()
        {
            projectile.width = 100;
            projectile.height = 48;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.scale = 1.2f;
        }
        public override void AI()
        {
          projectile.scale = projectile.ai[0];
          projectile.alpha = (int)projectile.ai[1];
          projectile.position.X--;
        }
    }
}
