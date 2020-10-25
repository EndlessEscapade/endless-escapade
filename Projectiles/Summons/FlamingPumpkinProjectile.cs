using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using EEMod.Buffs.Buffs;

namespace EEMod.Projectiles.Summons
{
    public class FlamingPumpkinProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flaming Pumpkin");
            Main.projFrames[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 18;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.timeLeft = 720;
            projectile.minion = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.minionSlots = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ai[0] = 60;
        }

        public override void AI()
        {
            projectile.ai[0]++;

            if(projectile.ai[0] < 600)
            {
                Vector2 targetPos = Vector2.Zero;

                for (int i = 0; i < Main.npc.Length - 1; i++)
                    if (Vector2.DistanceSquared(Main.npc[i].Center, projectile.Center) < Vector2.DistanceSquared(targetPos, projectile.Center))
                        targetPos = Main.npc[i].Center;

                if(projectile.velocity.Y >= -0.01f && projectile.velocity.Y <= 0.01f && targetPos != Vector2.Zero && projectile.ai[0] >= 60)
                {
                    projectile.velocity.Y -= 6;
                    projectile.ai[0] = 0;
                }

                projectile.velocity.X += Vector2.Normalize(targetPos - projectile.Center).X / 20;
                MathHelper.Clamp(projectile.velocity.X, -6, 6);

                projectile.rotation = projectile.velocity.X / 24f;
                projectile.velocity.Y += 0.2f;
            }
            if (projectile.ai[0] >= 600 && projectile.ai[0] < 720)
            {
                projectile.velocity.Y += 0.2f;

                projectile.velocity.Y *= 0.95f;

                if(projectile.velocity.X <= 0.01f && projectile.velocity.X >= -0.01f && projectile.velocity.Y <= 0.01f && projectile.velocity.Y >= -0.01f)
                {

                }
            }
        }

        public override void Kill(int timeLeft)
        {
            
        }
    }
}