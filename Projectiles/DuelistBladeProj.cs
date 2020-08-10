using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace EEMod.Projectiles
{
    public class DuelistBladeProj : Blade
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Duelist Blade");
        }

        public override void SetDefaults()
        {
            projectile.width = 128;
            projectile.height = 128;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.scale = 1f;
            projectile.alpha = 0;

            projectile.hide = true;
            projectile.ownerHitCheck = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.damage = 20;
            projectile.knockBack = 4.5f;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (var i = 0; i < 4; i++)
            {
                // int num = Dust.NewDust(target.Center, 2, 2, 182, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f), 6, Color.Red, 1);
                // Main.dust[num].noGravity = false;
            }
        }

    }
}
