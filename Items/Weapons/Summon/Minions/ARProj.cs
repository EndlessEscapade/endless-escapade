using EEMod.Buffs.Buffs;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Summon.Minions
{
    public class ARProj : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aqut");
            //  Main.projFrames[projectile.type] = 2;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            //ProjectileID.Sets.CountsAsHoming[Projectile.type] = true;
            Projectile.localNPCHitCooldown = 1;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        private int delay;

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.CloneDefaults(533); // ID for Deadly Sphere proj
            AIType = 533;
            Projectile.width = 34;
            Projectile.height = 26;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 18000;
            Projectile.minion = true;
            // Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 1;
            Projectile.friendly = true;
        }

        public override void AI()
        {
            bool areYouHere = Projectile.type == ModContent.ProjectileType<ARProj>();
            Player player = Main.player[Projectile.owner];
            player.AddBuff(ModContent.BuffType<ARBuff>(), 3600);
            float extra = 1;
            float projWidth = Projectile.width;
            float variation = 0.01f;
            Projectile.velocity += new Vector2(Main.rand.NextFloat(-variation, variation), Main.rand.NextFloat(-variation, variation));
            projWidth *= 2f;

            int maxprojectiles = Main.projectile.Length - 1;
            for (int j = 0; j < maxprojectiles; j++)
            {
                Projectile p = Main.projectile[j];
                if (j != Projectile.whoAmI && p.active && p.owner == Projectile.owner && p.type == Projectile.type && Math.Abs(Projectile.position.X - p.position.X) + Math.Abs(Projectile.position.Y - p.position.Y) < projWidth)
                {
                    if (Projectile.position.X < p.position.X)
                    {
                        Projectile.velocity.X += Projectile.velocity.X - extra;
                    }
                    else
                    {
                        Projectile.velocity.X += Projectile.velocity.X + extra;
                    }
                    if (Projectile.position.Y < p.position.Y)
                    {
                        Projectile.velocity.Y += Projectile.velocity.Y - extra;
                    }
                    else
                    {
                        Projectile.velocity.Y += Projectile.velocity.Y + extra;
                    }
                }
            }
            Vector2 projOrMinionPos = Projectile.position;
            float minDist = 3000f;
            bool zombieAboutToDie = false;
            // Projectile.tileCollide = false;
            NPC ownerMinionAttackTargetNPC = Projectile.OwnerMinionAttackTargetNPC;
            if (ownerMinionAttackTargetNPC != null && ownerMinionAttackTargetNPC.CanBeChasedBy(this, false))
            {
                float disFromBitch = Vector2.Distance(ownerMinionAttackTargetNPC.Center, Projectile.Center);
                if (((Vector2.DistanceSquared(Projectile.Center, projOrMinionPos) > disFromBitch * disFromBitch && disFromBitch < minDist) || !zombieAboutToDie) && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, ownerMinionAttackTargetNPC.position, ownerMinionAttackTargetNPC.width, ownerMinionAttackTargetNPC.height))
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
                        float distFromTarget = Vector2.Distance(nPC2.Center, Projectile.Center);
                        if (((Vector2.DistanceSquared(Projectile.Center, projOrMinionPos) > distFromTarget * distFromTarget && distFromTarget < minDist) || !zombieAboutToDie) && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, nPC2.position, nPC2.width, nPC2.height))
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
            float disFromPlayerSQ = Vector2.DistanceSquared(player.Center, Projectile.Center);
            if (disFromPlayerSQ > minDisFromPlayer * minDisFromPlayer)
            {
                Projectile.ai[0] = 1f;
                Projectile.netUpdate = true;
            }
            if (Projectile.ai[0] == 1f)
            {
                // Projectile.tileCollide = false;
            }
            if (zombieAboutToDie && Projectile.ai[0] == 0f)
            {
                int a = -1;
                int b = 0;

                for (var c = 0; c < 200; c++)
                {
                    // Vector2.Distance(Main.npc[c].Center, projectile.Center) < 300f
                    if (Main.npc[c].lifeMax >= b && Main.npc[c].lifeMax > 1 && !Main.npc[c].townNPC && !Main.npc[c].dontTakeDamage && Main.npc[c].active && Main.npc[c].WithinRange(Projectile.Center, 300))
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
                        Projectile.velocity = Helpers.MoveTowardsNPC(65f, Projectile.velocity.X, Projectile.velocity.Y, Main.npc[a], Projectile.Center, Projectile.direction);
                    }
                    else
                    {
                        Projectile.velocity *= 0.96f;
                    }

                    if (delay == 20)
                    {
                        delay = 0;
                    }
                }
                else
                {
                    Projectile.velocity *= 0.94f;
                    zombieAboutToDie = false;
                    Projectile.ai[0] = 1f;
                }
            }
            else
            {
                if (!Collision.CanHitLine(Projectile.Center, 1, 1, Main.player[Projectile.owner].Center, 1, 1))
                {
                    Projectile.ai[0] = 1f;
                }
                float bitchGetAwayFromMe = 6f;
                if (Projectile.ai[0] == 1f)
                {
                    bitchGetAwayFromMe = 15f;
                }
                Vector2 center2 = Projectile.Center;
                Projectile.ai[1] = 3600f;
                Projectile.netUpdate = true;
                Vector2 distFromPlayerVec = player.Center - center2;
                int bitchGetAwayFromMeX = 1;
                for (int m = 0; m < Projectile.whoAmI; m++)
                {
                    Projectile p = Main.projectile[m];
                    if (p.active && p.owner == Projectile.owner && p.type == Projectile.type)
                    {
                        bitchGetAwayFromMeX++;
                    }
                }
                distFromPlayerVec.X -= 10 * Main.player[Projectile.owner].direction;
                distFromPlayerVec.X -= bitchGetAwayFromMeX * 40 * Main.player[Projectile.owner].direction;
                distFromPlayerVec.Y -= 10f;
                float distFromPlayerMag = distFromPlayerVec.Length();
                if (distFromPlayerMag > 200f && bitchGetAwayFromMe < 9f)
                {
                    bitchGetAwayFromMe = 9f;
                }
                bitchGetAwayFromMe = (int)(bitchGetAwayFromMe * 0.75);
                if (distFromPlayerMag < 400f && Projectile.ai[0] == 1f && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                {
                    Projectile.ai[0] = 0f;
                    Projectile.netUpdate = true;
                }
                if (distFromPlayerMag > 2000f)
                {
                    Player ownerplayer = Main.player[Projectile.owner];
                    Projectile.position.X = ownerplayer.position.X;
                    Projectile.position.Y = ownerplayer.Center.Y - Projectile.width / 2;
                }
                if (distFromPlayerMag > 10f)
                {
                    distFromPlayerVec.Normalize();
                    if (distFromPlayerMag < 50f)
                    {
                        bitchGetAwayFromMe /= 2f;
                    }
                    distFromPlayerVec *= bitchGetAwayFromMe;
                    Projectile.velocity = (Projectile.velocity * 20f + distFromPlayerVec) / 21f;
                }
                else
                {
                    Projectile.direction = Main.player[Projectile.owner].direction;
                    Projectile.velocity *= 0.9f;
                }
            }

            if (Projectile.ai[1] > 0f)
            {
                Projectile.ai[1] += 1f;
                if (Main.rand.NextBool(3))
                {
                    Projectile.ai[1] += 1f;
                }
            }
            if (Projectile.ai[1] > Main.rand.Next(180, 900))
            {
                Projectile.ai[1] = 0f;
                Projectile.netUpdate = true;
            }
            if (Projectile.ai[0] == 0f)
            {
                Projectile.frame = 1;

                if (zombieAboutToDie)
                {
                    if ((projOrMinionPos - Projectile.Center).X > 0f)
                    {
                        Projectile.spriteDirection = Projectile.direction = -1;
                    }
                    else if ((projOrMinionPos - Projectile.Center).X < 0f)
                    {
                        Projectile.spriteDirection = Projectile.direction = 1;
                    }
                    if (Projectile.ai[1] == 0f)
                    {
                        Projectile.ai[1] += 1f;
                        if (Main.myPlayer == Projectile.owner)
                        {
                            Projectile.netUpdate = true;
                        }
                    }
                }
            }
            somethingIDontNeedToSync += 0.05f;
            Projectile.velocity.X += (float)Math.Sin(somethingIDontNeedToSync) / 20f;
            Projectile.velocity.Y += (float)Math.Cos(somethingIDontNeedToSync) / 20f;

            if (Projectile.velocity.X > 0)
            {
                Projectile.spriteDirection = -1;
            }
            else
            {
                Projectile.spriteDirection = 1;
            }
        }

        private float somethingIDontNeedToSync;

        public override bool PreAI()
        {
            bool areYouHere = Projectile.type == ModContent.ProjectileType<ARProj>();
            Player player = Main.player[Projectile.owner];
            EEPlayer modPlayer = player.GetModPlayer<EEPlayer>();

            if (player.dead)
            {
                // modPlayer.quartzCrystal = false;
            }

            return true;
        }
    }
}