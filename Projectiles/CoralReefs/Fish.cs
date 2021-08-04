using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles.CoralReefs
{
    public class Fish : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fish");
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.alpha = 0;
            Projectile.scale = 1f;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        private readonly float detectDist = 160f;
        private readonly float rotSpeed = 1f;
        private readonly float closeDist = 16f;
        private readonly float moveSpeed = 7f;

        public override void AI()
        {
            List<Projectile> closeProjectiles = new List<Projectile>();
            for (int i = 0; i < Main.projectile.Length - 1; i++)
            {
                if (Vector2.DistanceSquared(Projectile.position, Main.projectile[i].position) <= detectDist * detectDist && Projectile.type == ModContent.ProjectileType<Fish>() && Projectile != Main.projectile[i])
                {
                    Projectile closeProj = Main.projectile[i];
                    //Flock distancing
                    if (Vector2.DistanceSquared(Projectile.position, closeProj.position) <= closeDist * closeDist)
                    {
                        Projectile.velocity -= Projectile.velocity * 2;
                    }
                    //Flock rotation
                    if (Projectile.rotation <= closeProj.rotation)
                    {
                        Projectile.rotation += rotSpeed;
                    }
                    else //if (projectile.rotation > closeProj.rotation)
                    {
                        Projectile.rotation -= rotSpeed;
                    }

                    closeProjectiles.Add(closeProj);
                }
            }
            if (closeProjectiles?.Count > 0)
            {
                //Flock movement
                Vector2 averageLocation = Vector2.Zero;
                for (int i = 0; i < closeProjectiles.Count; i++)
                {
                    averageLocation += Main.projectile[i].position;
                }
                Projectile.velocity = Vector2.Normalize(Projectile.position - (averageLocation / closeProjectiles.Count)) * moveSpeed;
            }
            if (Projectile.velocity.X >= 0)
            {
                Projectile.spriteDirection = 1;
            }
            else
            {
                Projectile.spriteDirection = -1;
            }
        }
    }
}