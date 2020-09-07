using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Armor
{
    public class CorrosiveBubble : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corrosive Bubble");
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.friendly = true;
            projectile.timeLeft = 60;
            projectile.penetrate = 1;
            projectile.ranged = true;
            projectile.damage = 20;
            projectile.knockBack = 0;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(3))
                Dust.NewDust(projectile.Center, 0, 0, DustID.ToxicBubble);
        }
    }
}