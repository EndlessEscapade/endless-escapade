using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Mage
{
    public class CyanoburstTomeProjectile : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cyanoburst Plankton");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.timeLeft = 420;
            Projectile.ignoreWater = true;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
        }

        private Vector2 firstVel;

        public override void AI()
        {
            if (firstVel == default)
            {
                firstVel = Vector2.Normalize(Projectile.velocity) * 2;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
            Projectile.ai[0] += 11;
            double deg = Projectile.ai[0];
            double rad = deg * (Math.PI / 180);
            Projectile.velocity.X -= (float)Math.Cos(rad) * firstVel.X;
            Projectile.velocity.Y += (float)Math.Cos(rad) * firstVel.Y;
            for (var i = 0; i < 3; i++)
            {
                int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GreenBlood, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-1f, 1f), 6, new Color(0, 255, 0, 255), 1);
                Main.dust[num].noGravity = true;
                Main.dust[num].velocity *= 1.2f;
                Main.dust[num].noLight = false;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 360; i += 5)
            {
                float xdist = (int)(Math.Sin(i * (Math.PI / 180)) * 15);
                float ydist = (int)(Math.Cos(i * (Math.PI / 180)) * 15);
                Vector2 offset = new Vector2(xdist, ydist);
                Dust dust = Dust.NewDustPerfect(Projectile.Center + offset, DustID.GreenBlood, offset * 0.5f);
                dust.noGravity = true;
                dust.noLight = false;
            }
            Projectile.NewProjectile(Projectile.Center, Vector2.Zero, ModContent.ProjectileType<CyanoburstTomeKelp>(), 10, 10f, Main.myPlayer);
        }
    }
}