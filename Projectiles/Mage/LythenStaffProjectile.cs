using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Projectiles.Mage
{
    public class LythenStaffProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lythen Shell");
        }

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 16;
            projectile.alpha = 0;
            projectile.timeLeft = 600;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.scale *= 1f;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }
        public override void Kill(int timeLeft)
        {
            Vector2 origin = projectile.Center;
            float radius = 8;
            int numLocations = 180;
            for (int i = 0; i < numLocations; i++)
            {
                Vector2 position = origin + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / numLocations * i)) * radius;
                //'position' will be a point on a circle around 'origin'.  If you're using this to spawn dust, use Dust.NewDustPerfect
                Dust dust = Dust.NewDustPerfect(position, 111);
                dust.noGravity = true;
                dust.velocity = Vector2.Normalize(dust.position - projectile.Center) * 4;
                dust.noLight = false;
                dust.fadeIn = 1f;
            }
        }
        private NPC HomeOnTarget()
        {
            const bool homingCanAimAtWetEnemies = true;
            const float homingMaximumRangeInPixels = 2000;

            int selectedTarget = -1;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC target = Main.npc[i];
                if (target.active && (!target.wet || homingCanAimAtWetEnemies) && target.type != NPCID.TargetDummy)
                {
                    float distance = projectile.Distance(target.Center);
                    if (distance <= homingMaximumRangeInPixels &&
                        (
                            selectedTarget == -1 || //there is no selected target
                            projectile.Distance(Main.npc[selectedTarget].Center) > distance)
                    )
                        selectedTarget = i;
                }
            }
            if (selectedTarget == -1)
                return null;
            return Main.npc[selectedTarget];
        }
        NPC npc;
        public Vector2[] positionOfOthers = new Vector2[2];
        public override void AI()
        {
            npc = HomeOnTarget();
            projectile.rotation = projectile.velocity.ToRotation();
            Vector2 origin = projectile.Center;
            float radius = 4;
            int numLocations = 30;
            Vector2 position = origin + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / numLocations * projectile.ai[0])) * radius;
            projectile.ai[0]++;
            Dust dust = Dust.NewDustPerfect(position, 111);
            dust.noGravity = true;
            dust.velocity = Vector2.Normalize(dust.position - projectile.Center) * 2;
            dust.noLight = false;
            dust.fadeIn = 1f;
            projectile.velocity *= 0.98f;
            radius = 16;
            numLocations = 120;
            for (int i = 0; i < projectile.ai[1]; i++)
            {
                if (i % 10 == 0)
                {
                    position = origin + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / numLocations * i)) * radius;
                    dust = Dust.NewDustPerfect(position, 111);
                    dust.noGravity = true;
                    dust.velocity = projectile.velocity;
                    dust.noLight = false;
                    dust.fadeIn = 1f;
                }
            }
            int holder = 0;
            positionOfOthers = new Vector2[2];
            for (int i = 0; i < Main.projectile.Length - 1; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<LythenStaffProjectile>() && i != projectile.whoAmI)
                {
                    if (Vector2.DistanceSquared(Main.projectile[i].Center, projectile.Center) < 2000 * 2000)
                    {
                        holder++;
                        for (int j = 0; j < 2; j++)
                        {
                            if(positionOfOthers[j] == Vector2.Zero)
                            {
                                positionOfOthers[j] = Main.projectile[i].Center;
                                break;
                            }
                        }
                        if (projectile.ai[0] % 50 == 0)
                        {
                            for (float j = 0; j <= 1; j += 0.04f)
                            {
                                Vector2 Lerped = projectile.Center + (Main.projectile[i].Center - projectile.Center) * j;
                                Dust dust2 = Dust.NewDustPerfect(Lerped, 111, Vector2.Zero);
                                dust2.noGravity = true;
                                dust2.noLight = false;
                                dust2.fadeIn = 1f;
                            }
                        }
                    }
                }
            }
            
            if (holder == 2)
            {
                if (npc != null)
                {
                    if(Helpers.IsInside(projectile.Center, positionOfOthers[0], positionOfOthers[1], npc.Center))
                    {
                        npc.AddBuff(BuffID.Electrified, 10);
                    }
                }
            }
            if (projectile.ai[1] < 120)
                projectile.ai[1]++;
            else if (projectile.ai[1] == 120)
            {
                projectile.damage = projectile.damage * 2;
                projectile.ai[1] = 121;
            }
        }
    }
}
