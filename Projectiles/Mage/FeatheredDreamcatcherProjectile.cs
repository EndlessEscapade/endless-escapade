using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using System;
using Microsoft.Xna.Framework;

namespace EEMod.Projectiles.Mage
{
    public class FeatheredDreamcatcherProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dreamcatcher Feather");
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

        private int dropTimer = 10;
        private bool firstFrame = true;
        public override void AI()
        {
            if (firstFrame)
            {
                Vector2 closestNPCPos = new Vector2(0, 0);
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    if (Vector2.DistanceSquared(Main.LocalPlayer.Center, Main.npc[i].Center) <= Vector2.DistanceSquared(Main.LocalPlayer.Center, closestNPCPos))
                    {
                        closestNPCPos = Main.npc[i].Center;
                    }
                }
                if(closestNPCPos == new Vector2(0, 0) || Main.npc.Length == 0)
                {
                    projectile.Kill();
                }
                projectile.Center = new Vector2(closestNPCPos.X, closestNPCPos.Y - 80);
                firstFrame = false;
            }
            Dust.NewDust(projectile.Center, 0, 0, 127);
            if (dropTimer > 0)
                dropTimer--;
            else
                projectile.velocity.Y = 32;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
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