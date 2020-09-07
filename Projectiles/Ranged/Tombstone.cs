using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Ranged
{
    public class Tombstone : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tombstone");
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.ranged = true;
        }

        public override void AI()
        {
            projectile.rotation++;
            if (projectile.velocity.Y < 16)
            {
                projectile.velocity.Y *= 1.003f;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
                Dust.NewDustPerfect(projectile.Center + new Vector2(Main.rand.Next(-32, 32), Main.rand.Next(-32, 32)), DustID.Stone);
            Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<TombstoneHand>(), projectile.damage, 0, Main.myPlayer);
        }
    }
}