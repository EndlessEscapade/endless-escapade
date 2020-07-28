using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;

namespace EEMod.Projectiles.Mage
{
    public class CyanoburstTomeProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cyanoburst Plankton");
        }

        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.timeLeft = 420;
            projectile.ignoreWater = true;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.penetrate = -1;
        }
        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
        }
        public override void Kill(int timeLeft)
        {   for (int i = 0; i < 360; i += 5)
                {
                    float xdist = (int)(Math.Sin(i * (Math.PI / 180)) * 15);
                    float ydist = (int)(Math.Cos(i * (Math.PI / 180)) * 15);
                    Vector2 offset = new Vector2(xdist, ydist);
                    Dust dust = Dust.NewDustPerfect(projectile.Center + offset, DustID.GreenBlood, offset * 0.5f);
                    dust.noGravity = true;
                    dust.noLight = false;
                }
            Projectile.NewProjectile(projectile.Center, Vector2.Zero,ModContent.ProjectileType<CyanoburstTomeKelp>(),10,10f,Main.myPlayer);
        }
    }
}
