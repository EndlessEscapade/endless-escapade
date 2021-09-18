using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Mage
{
    public class InkCloud : EEProjectile
    {
        public override string Texture => "Terraria/Projectile_" + ProjectileID.ToxicCloud;
        public static Color OverrideColor = Color.Black;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("InkCloud");
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.ToxicCloud);
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 228;
            // Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.velocity *= 0.98f;
            Projectile.ai[0]++;
            if (Projectile.ai[0] >= 120)
            {
                Projectile.alpha += 2;
            }
        }
    }
}