using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Mage
{
    public class LythenStaffProjectile : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lythen Shell");
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 16;
            Projectile.alpha = 0;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            // Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.scale *= 1f;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void Kill(int timeLeft)
        {
            Vector2 origin = Projectile.Center;
            float radius = 8;
            int numLocations = 180;
            for (int i = 0; i < numLocations; i++)
            {
                Vector2 position = origin + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / numLocations * i)) * radius;
                //'position' will be a point on a circle around 'origin'.  If you're using this to spawn dust, use Dust.NewDustPerfect
                Dust dust = Dust.NewDustPerfect(position, 111);
                dust.noGravity = true;
                dust.velocity = Vector2.Normalize(dust.position - Projectile.Center) * 4;
                // dust.noLight = false;
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
                    float distanceSQ = Projectile.DistanceSQ(target.Center);
                    if (distanceSQ <= homingMaximumRangeInPixels * homingMaximumRangeInPixels && (
                        selectedTarget == -1 || //there is no selected target
                        Projectile.DistanceSQ(Main.npc[selectedTarget].Center) > distanceSQ)
                    )
                    {
                        selectedTarget = i;
                    }
                }
            }
            if (selectedTarget == -1)
            {
                return null;
            }

            return Main.npc[selectedTarget];
        }

        private NPC npc;
        public Vector2[] positionOfOthers = new Vector2[2];

        public override void AI()
        {
            npc = HomeOnTarget();
            Projectile.rotation = Projectile.velocity.ToRotation();
            Vector2 origin = Projectile.Center;
            float radius = 4;
            int numLocations = 30;
            Vector2 position = origin + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / numLocations * Projectile.ai[0])) * radius;
            Projectile.ai[0]++;
            Dust dust = Dust.NewDustPerfect(position, 111);
            dust.noGravity = true;
            dust.velocity = Vector2.Normalize(dust.position - Projectile.Center) * 2;
            // dust.noLight = false;
            dust.fadeIn = 1f;
            Projectile.velocity *= 0.95f;
            radius = 16;
            numLocations = 120;
            for (int i = 0; i < Projectile.ai[1]; i++)
            {
                if (i % 10 == 0)
                {
                    position = origin + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / numLocations * i)) * radius;
                    dust = Dust.NewDustPerfect(position, 111);
                    dust.noGravity = true;
                    dust.velocity = Projectile.velocity;
                    // dust.noLight = false;
                    dust.fadeIn = 1f;
                }
            }
            int holder = 0;
            positionOfOthers = new Vector2[2];
            for (int i = 0; i < Main.projectile.Length - 1; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<LythenStaffProjectile>() && i != Projectile.whoAmI)
                {
                    if (Vector2.DistanceSquared(Main.projectile[i].Center, Projectile.Center) < 2000 * 2000)
                    {
                        holder++;
                        for (int j = 0; j < 2; j++)
                        {
                            if (positionOfOthers[j] == Vector2.Zero)
                            {
                                positionOfOthers[j] = Main.projectile[i].Center;
                                break;
                            }
                        }
                        if (Projectile.ai[0] % 50 == 0)
                        {
                            for (float j = 0; j <= 1; j += 0.04f)
                            {
                                Vector2 Lerped = Projectile.Center + (Main.projectile[i].Center - Projectile.Center) * j;
                                Dust dust2 = Dust.NewDustPerfect(Lerped, 111, Vector2.Zero);
                                dust2.noGravity = true;
                                // dust2.noLight = false;
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
                    if (Helpers.IsInside(Projectile.Center, positionOfOthers[0], positionOfOthers[1], npc.Center))
                    {
                        npc.AddBuff(BuffID.Frostburn, 10);
                    }
                }
            }
            if (Projectile.ai[1] < 120)
            {
                Projectile.ai[1]++;
            }
            else if (Projectile.ai[1] == 120)
            {
                Projectile.damage = Projectile.damage * 2;
                Projectile.ai[1] = 121;
            }
        }
    }
}