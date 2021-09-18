using Terraria;
using Terraria.ModLoader;

namespace EEMod.Seamap.SeamapAssets
{
    public class FriendlyLuminiteCannonball : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Luminite Cannonball");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
        }

        private int killTimer = 180;
        private bool sinking;

        public override void AI()
        {
            if (!sinking)
            {
                Projectile.velocity *= 0.995f;
                Projectile.rotation = Projectile.velocity.ToRotation();
                killTimer--;
            }
            if (killTimer <= 0)
            {
                Sink();
                sinking = true;
            }
        }

        private int sinkTimer = 32;

        private void Sink()
        {
            Projectile.velocity.X = 0;
            Projectile.velocity.Y = 0.3f;
            Projectile.alpha += 8;
            sinkTimer--;
            if (sinkTimer <= 0)
            {
                Projectile.Kill();
            }
        }
    }
}
