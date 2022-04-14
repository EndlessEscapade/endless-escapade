using EEMod.Extensions;
using EEMod.Prim;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace EEMod.NPCs.Goblins.Berserker
{
    public class GoblinBerserker : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Goblin Berserker");
            //Main.npcFrameCount[npc.type] = 6;
        }

        /*public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter == 6)
            {
                npc.frame.Y = npc.frame.Y + frameHeight;
                npc.frameCounter = 0;
            }
            if (npc.frame.Y >= frameHeight * 6)
            {
                npc.frame.Y = 0;
                return;
            }
        }*/

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            NPC.HitSound = SoundID.NPCHit40;
            NPC.DeathSound = SoundID.NPCDeath42;

            NPC.alpha = 0;

            NPC.lifeMax = 550;
            NPC.defense = 10;

            NPC.width = 44;
            NPC.height = 56;

            NPC.friendly = false;

            NPC.damage = 20;

            NPC.knockBackResist = 0.9f;

            NPC.noGravity = false;
        }

        public bool aggro;
        public override void AI()
        {
            NPC.TargetClosest();

            Player player = Main.player[NPC.target];

            if (Vector2.DistanceSquared(player.Center, NPC.Center) <= 16 * 16 * 24 * 24 || NPC.life < NPC.lifeMax)
            {
                aggro = true;
            }

            if (aggro)
            {
                if (NPC.ai[0] == 0)
                {
                    NPC.ai[1] = 1;
                    NPC.ai[0] = 1;
                }

                if (NPC.ai[0] == 2)
                {
                    if (Main.projectile[(int)NPC.ai[2]].active == false)
                    {
                        NPC.ai[1] = 2;
                        NPC.ai[0] = 1;
                    }
                }

                NPC.velocity += new Vector2(Vector2.Normalize(player.Center - NPC.Center).X * 0.2f, 0);

                NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -3f, 3f);

                if (NPC.velocity.X > 0.01f || NPC.velocity.X < -0.01f)
                {
                    NPC.spriteDirection = (NPC.velocity.X < 0) ? 1 : -1;
                }

                if (NPC.ai[1] == 1)
                {
                    NPC.ai[1] = 0;
                    NPC.ai[0] = 2;

                    NPC.ai[2] = Projectile.NewProjectile(new EntitySource_Parent(NPC), NPC.Center, Vector2.Normalize(player.Center - NPC.Center) * 8f, ModContent.ProjectileType<BerserkerAxe>(), 20, 3f, default, ai0: 0, ai1: NPC.whoAmI);
                }
                else if (NPC.ai[1] == 2)
                {
                    NPC.ai[2]--;

                    if (NPC.ai[2] <= 0) NPC.ai[1] = 1;
                }
            }

            oldVel = NPC.velocity;
        }

        Vector2 oldVel;

        public override void PostAI()
        {
            if(NPC.position == NPC.oldPosition && aggro)
            {
                NPC.velocity += new Vector2(0, -8f);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            //if (NPC.ai[1] == 2)
            //    return false;
            //else
            //    return true;

            return true;
        }
    }

    public class BerserkerAxe : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Berserker Axe");
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 56;

            Projectile.alpha = 0;

            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.scale = 1f;

            Projectile.aiStyle = -1;

            Projectile.tileCollide = false;

            Projectile.damage = 20;

            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
            Projectile.rotation += (Projectile.velocity.X < 0) ? -0.5f : 0.5f;

            Projectile.ai[0]++;
            if(Projectile.ai[0] > 50)
            {
                Projectile.velocity += Vector2.Lerp(Projectile.velocity, Vector2.Normalize(Main.npc[(int)Projectile.ai[1]].Center - Projectile.Center) * 8f, (Projectile.ai[0] - 60) / 90f);

                if(Projectile.velocity.LengthSquared() >= 64f)
                {
                    Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 8f;
                }
            }

            if(Vector2.DistanceSquared(Projectile.Center, Main.npc[(int)Projectile.ai[1]].Center) <= 16f * 16f && Projectile.ai[0] > 50)
            {
                Main.npc[(int)Projectile.ai[1]].ai[1] = 1;
                Main.npc[(int)Projectile.ai[1]].ai[2] = 120;

                Projectile.Kill();
            }
        }
    }
}