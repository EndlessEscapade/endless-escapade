using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Mage
{
    public class FeatheredDreamcatcherProjectile : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dreamcatcher Feather");
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

        private int dropTimer = 10;
        private bool firstFrame = true;

        public override void AI()
        {
            if (firstFrame)
            {
                Vector2 closestNPCPos = Vector2.Zero;
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    if (Vector2.DistanceSquared(Main.LocalPlayer.Center, Main.npc[i].Center) <= Vector2.DistanceSquared(Main.LocalPlayer.Center, closestNPCPos) && Main.npc[i].active)
                    {
                        closestNPCPos = Main.npc[i].Center;
                    }
                }
                if (closestNPCPos == Vector2.Zero || Main.npc.Length == 0)
                {
                    Projectile.Kill();
                }
                Projectile.Center = new Vector2(closestNPCPos.X, closestNPCPos.Y - 80);
                firstFrame = false;
            }
            Dust.NewDust(Projectile.Center, 0, 0, DustID.Flare);
            if (dropTimer > 0)
            {
                dropTimer--;
            }
            else
            {
                Projectile.velocity.Y = 32;
            }
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

        public override void Kill(int timeLeft)
        {
        }
    }
}