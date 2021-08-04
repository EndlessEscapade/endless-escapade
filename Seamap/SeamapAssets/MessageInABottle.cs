using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace EEMod.Seamap.SeamapAssets
{
    public class MessageInABottle : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Message in a Bottle");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 18;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.scale = 1f;
        }

        public bool sinking;

        public override void AI()
        {
            if (!sinking)
            {
                Projectile.velocity = new Vector2(0.5f, 0);
            }
            else
            {
                Sink();
            }
        }

        private int sinkTimer = 32;

        public void Sink()
        {
            Projectile.velocity.X = 0;
            Projectile.velocity.Y = 0.5f;
            Projectile.alpha += 8;
            sinkTimer--;
            if (sinkTimer <= 0)
            {
                Projectile.Kill();
            }
        }
    }
}
