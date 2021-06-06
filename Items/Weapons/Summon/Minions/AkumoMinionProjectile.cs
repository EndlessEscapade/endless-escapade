using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Summon.Minions
{
    public class AkumoMinionProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Akumo Feather");
        }

        public override void SetDefaults()
        {
            projectile.width = 42;
            projectile.height = 18;
            projectile.tileCollide = false;
            projectile.timeLeft = 120;
            projectile.rotation = (float)(Math.PI / 2);
            projectile.penetrate = 1;
            projectile.hostile = false;
            projectile.friendly = true;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 180);
            KillVisible();
        }

        private void KillVisible()
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(projectile.Center, 0, 0, DustID.Flare);
            }
            projectile.Kill();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            AfterImage.DrawAfterimage(spriteBatch, Main.projectileTexture[projectile.type], 0, projectile, 1.5f, 1f, 3, false, 0f, 0f, new Color(lightColor.R, lightColor.G, lightColor.B, 150));
            return true;
        }
    }
}