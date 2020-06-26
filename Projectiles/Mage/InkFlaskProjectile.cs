using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace EEMod.Projectiles.Mage
{
    public class InkFlaskProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("InkFlaskProjectile");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 300;
        }

        public override void AI()
        {
            projectile.velocity.Y = projectile.velocity.Y + 0.25f;
            if (projectile.velocity.Y > 16f)
            {
                projectile.velocity.Y = 16f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Kill(0);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            for(int i = 0; i < 5; i++)
            {
                Projectile.NewProjectile(projectile.position, new Vector2(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2)), ModContent.ProjectileType<InkCloud>(), 1, 280);
            }
        }
    }
}
