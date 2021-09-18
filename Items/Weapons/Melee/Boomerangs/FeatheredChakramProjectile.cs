using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Melee.Boomerangs
{
    public class FeatheredChakramProjectile : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Feathered Chakram");
        }

        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 60;
            Projectile.aiStyle = 3;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            // Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.extraUpdates = 2;
            // Projectile.tileCollide = false;
        }

        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Flare);
            //Main.dust[dust].velocity = -projectile.velocity;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 180);
        }
    }
}