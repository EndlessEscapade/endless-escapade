
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace Prophecy.Projectiles
{
    public class Siren : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Siren");
        }

        public override void SetDefaults()
        {
            projectile.width = 80;
            projectile.height = 48;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.scale = 1f;
        }
        public override void AI()
        {

        }
    }
}
