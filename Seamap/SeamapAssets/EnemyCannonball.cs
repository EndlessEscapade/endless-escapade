using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Seamap.SeamapContent;

namespace EEMod.Seamap.SeamapAssets
{
    public class EnemyCannonball : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cannonball");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.magic = true;
        }

        private int killTimer = 180;
        private bool sinking;

        public override void AI()
        {
            if (!sinking)
            {
                projectile.velocity *= 0.995f;
                projectile.rotation = projectile.velocity.ToRotation();
                killTimer--;
                if (Vector2.DistanceSquared(SeamapPlayerShip.localship.position + Main.screenPosition, projectile.Center) < (20 * 20))
                {
                    sinking = true;
                    projectile.Kill();
                    Main.PlaySound(SoundID.NPCHit4);
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
            projectile.velocity.X = 0;
            projectile.velocity.Y = 0.3f;
            projectile.alpha += 8;
            sinkTimer--;
            if (sinkTimer <= 0)
            {
                projectile.Kill();
            }
        }
    }
}
