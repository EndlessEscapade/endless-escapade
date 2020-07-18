using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.Projectiles.Ranged
{
    public class CoralArrowProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coral Arrow");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.penetrate = 1;
            projectile.ignoreWater = false;
            projectile.tileCollide = true;
            projectile.extraUpdates = 1;
            projectile.aiStyle = 1;
            projectile.arrow = true;
        }

        public override void AI()
        {
            projectile.ai[0]--;
            if(projectile.ai[0] <= 0)
            {
                Projectile.NewProjectile(projectile.position, new Vector2(0, -5), ModContent.ProjectileType<WaterDragonsBubble>(), 12, 0);
                projectile.ai[0] = 20;
            }
        }
    }
}
