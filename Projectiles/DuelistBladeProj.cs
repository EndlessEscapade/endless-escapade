using Terraria;
using Terraria.ID;

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
            Projectile.width = 128;
            Projectile.height = 128;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.alpha = 0;

            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            //Projectile.DamageType = DamageClass.Melee;
            // Projectile.tileCollide = false;
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
    }
}