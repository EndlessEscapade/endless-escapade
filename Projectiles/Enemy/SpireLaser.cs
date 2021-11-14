using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Prim;
using EEMod.Tiles;

using Terraria.Audio;

namespace EEMod.Projectiles.Enemy
{
    public class SpireLaser : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aquamarine Laser");
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.timeLeft = 1200;
            Projectile.ignoreWater = true;
            Projectile.hostile = true;
            // Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 12;
            Projectile.hide = true;
            Projectile.tileCollide = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Bounce(Projectile.ModProjectile, oldVelocity);
            Projectile.ai[0]++;
            return false;
        }

        public void Bounce(ModProjectile modProj, Vector2 oldVelocity, float bouncyness = 1.5f)
        {
            Projectile projectile = modProj.Projectile;
            if (projectile.velocity.X != oldVelocity.X)
            {
                projectile.velocity.X = -oldVelocity.X * bouncyness;
            }

            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.velocity.Y = -oldVelocity.Y * bouncyness;
            }
            SoundEngine.PlaySound(SoundID.DD2_WitherBeastDeath, projectile.Center);
        }

        public override void AI()
        {
            if (Projectile.ai[1] < 5)
            {
                if (Projectile.ai[0] >= 3)
                {
                    Projectile.Kill();
                }

                if (Framing.GetTileSafely((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16)).type == ModContent.TileType<EmptyTile>())
                {
                    Bounce(Projectile.ModProjectile, Projectile.oldVelocity);
                    Projectile.ai[0]++;
                }
            }
            else
            {
                if (Framing.GetTileSafely((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16)).type == ModContent.TileType<EmptyTile>())
                {
                    Projectile.Kill();
                }
            }
        }
    }
}