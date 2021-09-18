using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Mage
{
    public class DalantiniumFang : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dalantinium Fang");
        }

        public override void SetDefaults()
        {
            //projectile.CloneDefaults(ProjectileID.PineNeedleFriendly); // CloneDefaults overrides some values
            Projectile.width = 8;
            Projectile.height = 12;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            // Projectile.hostile = false;
            Projectile.friendly = true;
            //aiType = ProjectileID.PineNeedleFriendly;
            Projectile.penetrate = 3;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Dust.NewDust(Projectile.Center, 0, 0, DustID.Blood, 0, 0, 0, Color.Gray, 1);

            return true;
        }

        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] >= 20)
            {
                Projectile.ai[1]++;
                if (Projectile.velocity.Y <= 24)
                {
                    Projectile.velocity.Y += 1.75f;
                }
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + (Projectile.ai[1] / 24 * Projectile.velocity.Y);
        }

        /*public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (projectile.ai[0] >= 120)
                AfterImage.DrawAfterimage(spriteBatch, Main.npcTexture[projectile.type], 0, projectile, 1.5f, 1f, 3, false, 0f, 0f, new Color(drawColor.R, drawColor.G, drawColor.B, 150));
            return true;
        }*/
    }
}