using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Seamap.SeamapContent;
using Terraria.Audio;

namespace EEMod.Seamap.SeamapAssets
{
    public class EnemyCannonball : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cannonball");
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
                if (Vector2.DistanceSquared(SeamapPlayerShip.localship.position + Main.screenPosition, Projectile.Center) < (20 * 20))
                {
                    sinking = true;
                    Projectile.Kill();
                    SoundEngine.PlaySound(SoundID.NPCHit4);
                }
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
