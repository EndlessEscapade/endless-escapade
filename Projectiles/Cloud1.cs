
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace Prophecy.Projectiles
{
    public class Cloud1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cloud");
        }

        public override void SetDefaults()
        {
            projectile.width = 214;
            projectile.height = 113;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            
        }
        public override void AI()
        {
            projectile.scale = projectile.ai[0];
            projectile.alpha = (int)projectile.ai[1];
            projectile.position.X--;
        }
    }
}
