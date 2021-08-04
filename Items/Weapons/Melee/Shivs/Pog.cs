using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace EEMod.Items.Weapons.Melee.Shivs
{
    public class Pog : Rapier
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dalantinium Dagger");
        }

        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.alpha = 255;

            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.melee = true;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.damage = 20;
            Projectile.knockBack = 4.5f;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (var i = 0; i < 4; i++)
            {
                // int num = Dust.NewDust(target.Center, 2, 2, 182, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f), 6, Color.Red, 1);
                // Main.dust[num].noGravity = false;
            }
        }

        public override List<int> exclude => new List<int> { 0, 1, 3, 6, 2, 5 };

    }
}
