using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs
{
    public class AquamarineSwordfish : ModNPC
    {
        private bool attacking;

        private int dashes;

        public override void SetStaticDefaults() => DisplayName.SetDefault("Swordfish");

        public override void SetDefaults()
        {
            npc.noGravity = true;

            // Needs stats
            npc.lifeMax = 50;
            npc.damage = 10;
            npc.defense = 2;

            npc.width = npc.height = 32;

            npc.knockBackResist = 0.2f;

            npc.aiStyle = -1;
            aiType = -1;

            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;

            npc.direction = Main.rand.NextBool() ? -1 : 1;
        }

        public override void FindFrame(int frameHeight) => npc.spriteDirection = npc.direction;

        public override void AI()
        {
            npc.TargetClosest(false);

            Player player = Main.player[npc.target];

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
            npc.spriteDirection = npc.direction;

            npc.velocity.X += npc.direction * 0.025f;
            npc.velocity.X = MathHelper.Clamp(npc.velocity.X, -2f, 2f);

            if (npc.collideX) 
            {
                npc.velocity = Vector2.Zero;
                npc.direction = -npc.direction;
            }

            npc.ai[0]++;

            float sine = (float)Math.Sin(npc.ai[0] / 40f);
            npc.velocity.Y = sine;

            float addon = npc.spriteDirection == -1 ? MathHelper.Pi : 0f;
            npc.rotation = npc.velocity.ToRotation() + addon;

            const int cooldown = 180;
            const float attackRange = 16f * 16f;

            if (Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height) && npc.ai[0] > cooldown && player.Distance(npc.Center) < attackRange)
            {
                npc.ai[0] = 0;
                attacking = true;
            }
        }

        private void Attack(Player player)
        {
            npc.ai[0]++;

            if (npc.ai[0] % 60 == 0)
            {
                int maxDashes = Main.rand.Next(1, 4);

                if (dashes > maxDashes)
                {
                    dashes = 0;
                    npc.ai[0] = 0;

                    npc.direction = Main.rand.NextBool() ? -1 : 1;

                    attacking = false;
                    return;
                }

                dashes++;
                npc.velocity = npc.DirectionTo(player.Center) * 16f;
            }

            float addon = npc.spriteDirection == -1 ? MathHelper.Pi : 0f;
            npc.rotation = npc.velocity.ToRotation() + addon;

            npc.velocity *= 0.98f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Main.npcTexture[npc.type];

            Vector2 drawPosition = npc.Center.ForDraw() + new Vector2(0f, npc.gfxOffY);

            SpriteEffects effects = npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            spriteBatch.Draw(texture, drawPosition, npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2f, npc.scale, effects, 0f);

            return false;
        }
    }
}
