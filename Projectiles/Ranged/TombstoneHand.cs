using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace EEMod.Projectiles.Ranged
{
    public class TombstoneHand : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tombstone Hand");
            Main.projFrames[projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 58;
            projectile.timeLeft = 420;
            projectile.ignoreWater = true;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.alpha = 255;
            projectile.damage = 100;
        }
        NPC chosenTarget;
        public void DrawBehind()
        {
            Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, new Rectangle(0, projectile.height * projectile.frame, projectile.width, projectile.height), Color.White, projectile.rotation, new Rectangle(0, projectile.height * projectile.frame, projectile.width, projectile.height).Size() / 2, 1, SpriteEffects.None, 0);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!target.boss && chosenTarget == null)
            {
                chosenTarget = target;
            }
        }
        public override void AI()
        {
            if (chosenTarget != null)
            {
                chosenTarget.Center = projectile.Center + new Vector2(0, -16);
            }
            Dust.NewDustPerfect(projectile.Center + new Vector2(Main.rand.Next(-100, 100), Main.rand.Next(-100, 100)), DustID.Blood);
            projectile.frameCounter++;
            if (projectile.frameCounter > 8 && projectile.frame < 7)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
        }
    }
}
