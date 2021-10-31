using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace EEMod.NPCs.CoralReefs
{
    public class Swordfish : EENPC
    {
        private bool attacking;

        private int dashes;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swordfish");
            Main.npcCatchable[NPC.type] = true;
            Main.npcFrameCount[NPC.type] = 1;
            //bannerItem = ModContent.ItemType<Items.Banners.LunaJellyBanner>();
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            // NPC.friendly = false;

            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;

            NPC.lifeMax = 76;

            NPC.width = 80;
            NPC.height = 20;

            NPC.noGravity = true;

            // NPC.lavaImmune = false;
            // NPC.noTileCollide = false;
        }

        public override void AI()
        {
            NPC.TargetClosest(false);

            Player player = Main.player[NPC.target];

            if (attacking)
            {
                Attack(player);
            }
            else
            {
                Idle(player);
            }
        }

        private void Idle(Player player)
        {
            NPC.spriteDirection = NPC.direction;

            NPC.velocity.X += NPC.direction * 0.025f;
            NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -2f, 2f);

            if (NPC.collideX)
            {
                NPC.velocity = Vector2.Zero;
                NPC.direction = -NPC.direction;
            }

            NPC.ai[0]++;

            float sine = (float)Math.Sin(NPC.ai[0] / 40f);
            NPC.velocity.Y = sine;

            float addon = NPC.spriteDirection == -1 ? MathHelper.Pi : 0f;
            NPC.rotation = NPC.velocity.ToRotation() + addon;

            const int cooldown = 180;
            const float attackRange = 16f * 16f;

            if (Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height) && NPC.ai[0] > cooldown && player.Distance(NPC.Center) < attackRange)
            {
                NPC.ai[0] = 0;
                attacking = true;
            }
        }

        private void Attack(Player player)
        {
            NPC.ai[0]++;

            if (NPC.ai[0] % 60 == 0)
            {
                int maxDashes = Main.rand.Next(1, 4);

                if (dashes > maxDashes)
                {
                    dashes = 0;
                    NPC.ai[0] = 0;

                    NPC.direction = Main.rand.NextBool() ? -1 : 1;

                    attacking = false;
                    return;
                }

                dashes++;
                NPC.velocity = NPC.DirectionTo(player.Center) * 16f;
            }

            float addon = NPC.spriteDirection == -1 ? MathHelper.Pi : 0f;
            NPC.rotation = NPC.velocity.ToRotation() + addon;

            NPC.velocity *= 0.98f;
        }
    }
}