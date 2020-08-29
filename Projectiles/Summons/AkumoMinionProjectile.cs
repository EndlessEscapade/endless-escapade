using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using System;
using Microsoft.Xna.Framework;

namespace EEMod.Projectiles.Summons
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
            Dust.NewDust(projectile.Center, 0, 0, 127);
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
                Dust.NewDust(projectile.Center, 0, 0, 127);
            }
            projectile.Kill();
        }

        public override void Kill(int timeLeft)
        {

        }
    }
}