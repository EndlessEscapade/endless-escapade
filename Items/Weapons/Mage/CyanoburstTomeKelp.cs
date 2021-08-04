using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Mage
{
    public class CyanoburstTomeKelp : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cyanoburst Plankton");
            Main.projFrames[Projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 58;
            Projectile.timeLeft = 420;
            Projectile.ignoreWater = true;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.damage = 100;
        }

        private NPC chosenTarget;

        public void DrawBehind()
        {
            Main.spriteBatch.Draw(Main.projectileTexture[Projectile.type], Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), Color.White, Projectile.rotation, new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height).Size() / 2, 1, SpriteEffects.None, 0);
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
                chosenTarget.Center = Projectile.Center + new Vector2(0, -16);
            }
            Dust.NewDustPerfect(Projectile.Center + new Vector2(Main.rand.Next(-100, 100), Main.rand.Next(-100, 100)), DustID.GreenBlood);
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 8 && Projectile.frame < 7)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
        }
    }
}