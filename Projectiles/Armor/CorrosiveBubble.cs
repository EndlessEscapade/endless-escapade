using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Armor
{
    public class CorrosiveBubble : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corrosive Bubble");
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.timeLeft = 60;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.damage = 20;
            Projectile.knockBack = 0;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(Projectile.Center, 0, 0, DustID.ToxicBubble);
            }
        }
    }
}