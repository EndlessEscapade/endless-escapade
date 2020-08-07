using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace EEMod.Projectiles.Mage
{
    public class InkFlaskProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ink Flask");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 600;
        }

        public override void AI()
        {
            projectile.velocity.Y = projectile.velocity.Y + 0.25f;
            if (projectile.velocity.Y > 16f)
            {
                projectile.velocity.Y = 16f;
            }
            projectile.rotation = projectile.velocity.ToRotation();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Kill(0);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                Projectile.NewProjectile(projectile.position, new Vector2(Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 5)), ModContent.ProjectileType<InkCloud>(), 1, 280);
            }
            Main.PlaySound(SoundID.Item1);
        }
    }
}
