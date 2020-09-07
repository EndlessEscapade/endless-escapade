using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles.CoralReefs
{
    public class Fish : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fish");
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.alpha = 0;
            projectile.scale = 1f;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        private float detectDist = 160f;
        private float rotSpeed = 1f;
        private float closeDist = 16f;
        private float moveSpeed = 7f;

        public override void AI()
        {
            List<Projectile> closeProjectiles = new List<Projectile>();
            for (int i = 0; i < Main.projectile.Length - 1; i++)
            {
                if (Vector2.DistanceSquared(projectile.position, Main.projectile[i].position) <= detectDist * detectDist && projectile.type == ModContent.ProjectileType<Fish>() && projectile != Main.projectile[i])
                {
                    Projectile closeProj = Main.projectile[i];
                    //Flock distancing
                    if (Vector2.DistanceSquared(projectile.position, closeProj.position) <= closeDist * closeDist)
                    {
                        projectile.velocity -= projectile.velocity * 2;
                    }
                    //Flock rotation
                    if (projectile.rotation <= closeProj.rotation)
                        projectile.rotation += rotSpeed;
                    else //if (projectile.rotation > closeProj.rotation)
                        projectile.rotation -= rotSpeed;
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
                projectile.velocity = Vector2.Normalize(projectile.position - (averageLocation / closeProjectiles.Count)) * moveSpeed;
            }
            if (projectile.velocity.X >= 0)
                projectile.spriteDirection = 1;
            else
                projectile.spriteDirection = -1;
        }
    }
}