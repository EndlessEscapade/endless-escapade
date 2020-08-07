using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace EEMod.Projectiles.Mage
{
    public class InkCloud : ModProjectile
    {
        public override string Texture => "Terraria/Projectile_" + ProjectileID.ToxicCloud;
        public static Color OverrideColor = Color.Black;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("InkCloud");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.ToxicCloud);
            projectile.friendly = true;
            projectile.magic = true;
            projectile.timeLeft = 228;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            projectile.velocity *= 0.98f;
            projectile.ai[0]++;
            if(projectile.ai[0] >= 120)
                projectile.alpha+=2;
        }
    }
}
