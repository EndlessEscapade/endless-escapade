using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Mage
{
    public class DalantiniumFang : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dalantinium Fang");
        }

        public override void SetDefaults()
        {
            //projectile.CloneDefaults(ProjectileID.PineNeedleFriendly); // CloneDefaults overrides some values
            projectile.width = 8;
            projectile.height = 12;
            projectile.timeLeft = 600;
            projectile.ignoreWater = true;
            projectile.hostile = false;
            projectile.friendly = true;
            //aiType = ProjectileID.PineNeedleFriendly;
            projectile.penetrate = 3;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Dust.NewDust(projectile.Center, 0, 0, DustID.Blood, 0, 0, 0, Color.Gray, 1);

            return true;
        }

        public override void AI()
        {
            projectile.ai[0]++;
            if (projectile.ai[0] >= 20)
            {
                projectile.ai[1]++;
                if (projectile.velocity.Y <= 24)
                {
                    projectile.velocity.Y += 1.75f;
                }
            }

            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2 + (projectile.ai[1] / 24 * projectile.velocity.Y);
        }

        /*public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (projectile.ai[0] >= 120)
                AfterImage.DrawAfterimage(spriteBatch, Main.npcTexture[projectile.type], 0, projectile, 1.5f, 1f, 3, false, 0f, 0f, new Color(drawColor.R, drawColor.G, drawColor.B, 150));
            return true;
        }*/
    }
}