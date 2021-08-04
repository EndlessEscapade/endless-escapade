using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Summon.Minions
{
    public class AkumoMinionProjectile : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Akumo Feather");
        }

        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 18;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 120;
            Projectile.rotation = (float)(Math.PI / 2);
            Projectile.penetrate = 1;
            Projectile.hostile = false;
            Projectile.friendly = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
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
                Dust.NewDust(Projectile.Center, 0, 0, DustID.Flare);
            }
            Projectile.Kill();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            AfterImage.DrawAfterimage(spriteBatch, Main.projectileTexture[Projectile.type], 0, Projectile, 1.5f, 1f, 3, false, 0f, 0f, new Color(lightColor.R, lightColor.G, lightColor.B, 150));
            return true;
        }
    }
}