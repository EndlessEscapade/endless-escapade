using EEMod.Buffs.Buffs;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Summons
{
    public class ARProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aqut");
            //  Main.projFrames[projectile.type] = 2;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            projectile.localNPCHitCooldown = 1;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }

        private int delay;

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.CloneDefaults(533); // ID for Deadly Sphere proj
            aiType = 533;
            projectile.width = 34;
            projectile.height = 26;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.timeLeft = 18000;
            projectile.minion = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.minionSlots = 1;
            projectile.friendly = true;
        }

        public override void AI()
        {
            bool areYouHere = projectile.type == ModContent.ProjectileType<ARProj>();
            Player player = Main.player[projectile.owner];
            player.AddBuff(ModContent.BuffType<ARBuff>(), 3600);
            float extra = 1;
            float projWidth = projectile.width;
            float variation = 0.01f;
            projectile.velocity += new Vector2(Main.rand.NextFloat(-variation, variation), Main.rand.NextFloat(-variation, variation));
            projWidth *= 2f;

            int maxprojectiles = Main.projectile.Length - 1;
            for (int j = 0; j < maxprojectiles; j++)
            {
                Projectile p = Main.projectile[j];
                if (j != projectile.whoAmI && p.active && p.owner == projectile.owner && p.type == projectile.type && Math.Abs(projectile.position.X - p.position.X) + Math.Abs(projectile.position.Y - p.position.Y) < projWidth)
                {
                    if (projectile.position.X < p.position.X)
                    {
                        projectile.velocity.X += projectile.velocity.X - extra;
                    }
                    else
                    {
                        projectile.velocity.X += projectile.velocity.X + extra;
                    }
                    if (projectile.position.Y < p.position.Y)
                    {
                        projectile.velocity.Y += projectile.velocity.Y - extra;
                    }
                    else
                    {
                        projectile.velocity.Y += projectile.velocity.Y + extra;
                    }
                }
            }
            Vector2 projOrMinionPos = projectile.position;
            float minDist = 3000f;
            bool zombieAboutToDie = false;
            projectile.tileCollide = false;
            NPC ownerMinionAttackTargetNPC = projectile.OwnerMinionAttackTargetNPC;
            if (ownerMinionAttackTargetNPC != null && ownerMinionAttackTargetNPC.CanBeChasedBy(this, false))
            {
                float disFromBitch = Vector2.Distance(ownerMinionAttackTargetNPC.Center, projectile.Center);
                if (((Vector2.DistanceSquared(projectile.Center, projOrMinionPos) > disFromBitch * disFromBitch && disFromBitch < minDist) || !zombieAboutToDie) && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, ownerMinionAttackTargetNPC.position, ownerMinionAttackTargetNPC.width, ownerMinionAttackTargetNPC.height))
                {
                    minDist = disFromBitch;
                    projOrMinionPos = ownerMinionAttackTargetNPC.Center;
                    zombieAboutToDie = true;
                }
            }
            if (!zombieAboutToDie)
            {
                for (int l = 0; l < 200; l++)
                {
                    NPC nPC2 = Main.npc[l];
                    if (nPC2.CanBeChasedBy(this, false))
                    {
                        float distFromTarget = Vector2.Distance(nPC2.Center, projectile.Center);
                        if (((Vector2.DistanceSquared(projectile.Center, projOrMinionPos) > distFromTarget * distFromTarget && distFromTarget < minDist) || !zombieAboutToDie) && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, nPC2.position, nPC2.width, nPC2.height))
                        {
                            minDist = distFromTarget;
                            projOrMinionPos = nPC2.Center;
                            zombieAboutToDie = true;
                        }
                    }
                }
            }
            int minDisFromPlayer = 1000;
            if (zombieAboutToDie)
            {
                minDisFromPlayer = 2000;
            }
            float disFromPlayerSQ = Vector2.DistanceSquared(player.Center, projectile.Center);
            if (disFromPlayerSQ > minDisFromPlayer * minDisFromPlayer)
            {
                projectile.ai[0] = 1f;
                projectile.netUpdate = true;
            }
            if (projectile.ai[0] == 1f)
            {
                projectile.tileCollide = false;
            }
            if (zombieAboutToDie && projectile.ai[0] == 0f)
            {
                int a = -1;
                int b = 0;

                for (var c = 0; c < 200; c++)
                {
                    // Vector2.Distance(Main.npc[c].Center, projectile.Center) < 300f
                    if (Main.npc[c].lifeMax >= b && Main.npc[c].lifeMax > 1 && !Main.npc[c].townNPC && !Main.npc[c].dontTakeDamage && Main.npc[c].active && Main.npc[c].WithinRange(projectile.Center, 300))
                    {
                        b = Main.npc[c].lifeMax;
                        a = c;
                    }
                }
                if (a >= 0)
                {
                    delay++;
                    if (delay < 14)
                    {
                        projectile.velocity = Helpers.MoveTowardsNPC(65f, projectile.velocity.X, projectile.velocity.Y, Main.npc[a], projectile.Center, projectile.direction);
                    }
                    else
                    {
                        projectile.velocity *= 0.96f;
                    }

                    if (delay == 20)
                    {
                        delay = 0;
                    }
                }
                else
                {
                    projectile.velocity *= 0.94f;
                    zombieAboutToDie = false;
                    projectile.ai[0] = 1f;
                }
            }
            else
            {
                if (!Collision.CanHitLine(projectile.Center, 1, 1, Main.player[projectile.owner].Center, 1, 1))
                {
                    projectile.ai[0] = 1f;
                }
                float bitchGetAwayFromMe = 6f;
                if (projectile.ai[0] == 1f)
                {
                    bitchGetAwayFromMe = 15f;
                }
                Vector2 center2 = projectile.Center;
                projectile.ai[1] = 3600f;
                projectile.netUpdate = true;
                Vector2 distFromPlayerVec = player.Center - center2;
                int bitchGetAwayFromMeX = 1;
                for (int m = 0; m < projectile.whoAmI; m++)
                {
                    Projectile p = Main.projectile[m];
                    if (p.active && p.owner == projectile.owner && p.type == projectile.type)
                    {
                        bitchGetAwayFromMeX++;
                    }
                }
                distFromPlayerVec.X -= 10 * Main.player[projectile.owner].direction;
                distFromPlayerVec.X -= bitchGetAwayFromMeX * 40 * Main.player[projectile.owner].direction;
                distFromPlayerVec.Y -= 10f;
                float distFromPlayerMag = distFromPlayerVec.Length();
                if (distFromPlayerMag > 200f && bitchGetAwayFromMe < 9f)
                {
                    bitchGetAwayFromMe = 9f;
                }
                bitchGetAwayFromMe = (int)(bitchGetAwayFromMe * 0.75);
                if (distFromPlayerMag < 400f && projectile.ai[0] == 1f && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                {
                    projectile.ai[0] = 0f;
                    projectile.netUpdate = true;
                }
                if (distFromPlayerMag > 2000f)
                {
                    Player ownerplayer = Main.player[projectile.owner];
                    projectile.position.X = ownerplayer.position.X;
                    projectile.position.Y = ownerplayer.Center.Y - projectile.width / 2;
                }
                if (distFromPlayerMag > 10f)
                {
                    distFromPlayerVec.Normalize();
                    if (distFromPlayerMag < 50f)
                    {
                        bitchGetAwayFromMe /= 2f;
                    }
                    distFromPlayerVec *= bitchGetAwayFromMe;
                    projectile.velocity = (projectile.velocity * 20f + distFromPlayerVec) / 21f;
                }
                else
                {
                    projectile.direction = Main.player[projectile.owner].direction;
                    projectile.velocity *= 0.9f;
                }
            }

            if (projectile.ai[1] > 0f)
            {
                projectile.ai[1] += 1f;
                if (Main.rand.NextBool(3))
                {
                    projectile.ai[1] += 1f;
                }
            }
            if (projectile.ai[1] > Main.rand.Next(180, 900))
            {
                projectile.ai[1] = 0f;
                projectile.netUpdate = true;
            }
            if (projectile.ai[0] == 0f)
            {
                projectile.frame = 1;

                if (zombieAboutToDie)
                {
                    if ((projOrMinionPos - projectile.Center).X > 0f)
                    {
                        projectile.spriteDirection = projectile.direction = -1;
                    }
                    else if ((projOrMinionPos - projectile.Center).X < 0f)
                    {
                        projectile.spriteDirection = projectile.direction = 1;
                    }
                    if (projectile.ai[1] == 0f)
                    {
                        projectile.ai[1] += 1f;
                        if (Main.myPlayer == projectile.owner)
                        {
                            projectile.netUpdate = true;
                        }
                    }
                }
            }
            somethingIDontNeedToSync += 0.05f;
            projectile.velocity.X += (float)Math.Sin(somethingIDontNeedToSync) / 20f;
            projectile.velocity.Y += (float)Math.Cos(somethingIDontNeedToSync) / 20f;

            if (projectile.velocity.X > 0)
            {
                projectile.spriteDirection = -1;
            }
            else
            {
                projectile.spriteDirection = 1;
            }
        }

        private float somethingIDontNeedToSync;

        public override bool PreAI()
        {
            bool areYouHere = projectile.type == ModContent.ProjectileType<ARProj>();
            Player player = Main.player[projectile.owner];
            EEPlayer modPlayer = player.GetModPlayer<EEPlayer>();

            if (player.dead)
            {
                modPlayer.quartzCrystal = false;
            }
            if (modPlayer.quartzCrystal)
            {
                projectile.timeLeft = 2;
            }

            return true;
        }
    }
}