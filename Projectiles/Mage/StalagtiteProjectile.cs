using Terraria.ModLoader;
using Terraria;
using System;
using Microsoft.Xna.Framework;

namespace InteritosMod.Projectiles.Mage
{
    public class StalagtiteProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stalagtite");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 20;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.tileCollide = true;
        }
    }
}
