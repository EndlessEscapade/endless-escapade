using EEMod.Extensions;
using EEMod.Prim;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.NPCs.Goblins.Shaman;
using EEMod.NPCs.Goblins.Berserker;

namespace EEMod.NPCs.Goblins.Bard
{
    public class CymbalBard : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Goblin Bard");
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            NPC.HitSound = SoundID.NPCHit40;
            NPC.DeathSound = SoundID.NPCDeath42;

            NPC.alpha = 0;

            NPC.lifeMax = 550;
            NPC.defense = 10;

            NPC.width = 34;
            NPC.height = 44;

            NPC.friendly = false;

            NPC.damage = 20;

            NPC.knockBackResist = 0.9f;
        }

        public int buffedNPC = -1;
        public override void AI()
        {
            NPC.TargetClosest();

            Player player = Main.player[NPC.target];

            NPC.velocity += new Vector2(Vector2.Normalize(player.Center - NPC.Center).X * 0.2f, 0);

            NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -1.5f, 1.5f);

            if (NPC.velocity.X > 0.01f || NPC.velocity.X < -0.01f)
            {
                NPC.spriteDirection = (NPC.velocity.X < 0) ? -1 : 1;
            }

            if (buffedNPC != -1 && (Main.npc[buffedNPC] == null || Main.npc[buffedNPC].active == false))
            {
                buffedNPC = -1;
            }

            if (buffedNPC != -1 && Vector2.DistanceSquared(Main.npc[buffedNPC].Center, NPC.Center) > 16 * 16 * 16 * 16)
            {
                Main.npc[buffedNPC].GetGlobalNPC<GoblinBardGlobal>().hasBardBuff = false;

                buffedNPC = -1;
            }

            if (buffedNPC == -1)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i] != null && Main.npc[i].active && Vector2.DistanceSquared(Main.npc[i].Center, NPC.Center) <= 16 * 16 * 16 * 16 &&
                        !Main.npc[i].GetGlobalNPC<GoblinBardGlobal>().hasBardBuff &&
                        (Main.npc[i].type == ModContent.NPCType<GoblinShaman>() ||
                         Main.npc[i].type == ModContent.NPCType<Berserker.GoblinBerserker>()))
                    {
                        buffedNPC = i;

                        Main.npc[buffedNPC].defense = (int)(Main.npc[buffedNPC].defense * 1.2f);

                        Main.npc[buffedNPC].GetGlobalNPC<GoblinBardGlobal>().hasBardBuff = true;

                        SoundEngine.PlaySound(SoundID.Item144, NPC.Center);

                        break;
                    }
                }
            }
        }

        public override void PostAI()
        {
            if (NPC.position == NPC.oldPosition)
            {
                NPC.velocity += new Vector2(0, -4f);
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (buffedNPC != -1 && Main.npc[buffedNPC] != null && Main.npc[buffedNPC].active) 
            {
                Texture2D tex = ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Bard/CymbalBardNote").Value;
                Texture2D mask = ModContent.Request<Texture2D>("EEMod/Textures/RadialGradient").Value;

                Helpers.DrawAdditive(mask, Main.npc[buffedNPC].Top + new Vector2(0, -20 + (float)(Math.Sin(Main.GameUpdateCount / 40f) * 4f)) - Main.screenPosition, Color.Violet, 0.33f);

                spriteBatch.Draw(tex, Main.npc[buffedNPC].Top + new Vector2(0, -20 + (float)(Math.Sin(Main.GameUpdateCount / 40f) * 4f)) - Main.screenPosition, null, Color.White, 0f, tex.TextureCenter(), 1f, SpriteEffects.None, 0f);
            }
        }
    }

    public class PercussionBard : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Goblin Bard");
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            NPC.HitSound = SoundID.NPCHit40;
            NPC.DeathSound = SoundID.NPCDeath42;

            NPC.alpha = 0;

            NPC.lifeMax = 550;
            NPC.defense = 10;

            NPC.width = 32;
            NPC.height = 44;

            NPC.friendly = false;

            NPC.damage = 20;

            NPC.knockBackResist = 0.9f;
        }

        public int buffedNPC = -1;
        public override void AI()
        {
            NPC.TargetClosest();

            Player player = Main.player[NPC.target];

            NPC.velocity += new Vector2(Vector2.Normalize(player.Center - NPC.Center).X * 0.2f, 0);

            NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -1.5f, 1.5f);

            if (NPC.velocity.X > 0.01f || NPC.velocity.X < -0.01f)
            {
                NPC.spriteDirection = (NPC.velocity.X < 0) ? -1 : 1;
            }

            if (buffedNPC != -1 && (Main.npc[buffedNPC] == null || Main.npc[buffedNPC].active == false))
            {
                buffedNPC = -1;
            }

            if (buffedNPC != -1 && Vector2.DistanceSquared(Main.npc[buffedNPC].Center, NPC.Center) > 16 * 16 * 16 * 16)
            {
                Main.npc[buffedNPC].GetGlobalNPC<GoblinBardGlobal>().hasBardBuff = false;

                buffedNPC = -1;
            }

            if (buffedNPC == -1)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i] != null && Main.npc[i].active && Vector2.DistanceSquared(Main.npc[i].Center, NPC.Center) <= 16 * 16 * 16 * 16 &&
                        !Main.npc[i].GetGlobalNPC<GoblinBardGlobal>().hasBardBuff &&
                        (Main.npc[i].type == ModContent.NPCType<GoblinShaman>() ||
                         Main.npc[i].type == ModContent.NPCType<Berserker.GoblinBerserker>()))
                    {
                        buffedNPC = i;

                        Main.npc[buffedNPC].GetGlobalNPC<GoblinBardGlobal>().hasBardBuff = true;

                        Main.npc[buffedNPC].defense = (int)(Main.npc[buffedNPC].defense * 1.2f);

                        SoundEngine.PlaySound(SoundID.Item142, NPC.Center);

                        break;
                    }
                }
            }
        }

        public override void PostAI()
        {
            if (NPC.position == NPC.oldPosition)
            {
                NPC.velocity += new Vector2(0, -4f);
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (buffedNPC != -1 && Main.npc[buffedNPC] != null && Main.npc[buffedNPC].active)
            {
                Texture2D tex = ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Bard/PercussionBardNote").Value;
                Texture2D mask = ModContent.Request<Texture2D>("EEMod/Textures/RadialGradient").Value;

                Helpers.DrawAdditive(mask, Main.npc[buffedNPC].Top + new Vector2(0, -20 + (float)(Math.Sin(Main.GameUpdateCount / 40f) * 4f)) - Main.screenPosition, Color.Violet, 0.33f);

                spriteBatch.Draw(tex, Main.npc[buffedNPC].Top + new Vector2(0, -20 + (float)(Math.Sin(Main.GameUpdateCount / 40f) * 4f)) - Main.screenPosition, null, Color.White, 0f, tex.TextureCenter(), 1f, SpriteEffects.None, 0f);
            }
        }
    }

    public class PanfluteBard : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Goblin Bard");
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            NPC.HitSound = SoundID.NPCHit40;
            NPC.DeathSound = SoundID.NPCDeath42;

            NPC.alpha = 0;

            NPC.lifeMax = 550;
            NPC.defense = 10;

            NPC.width = 28;
            NPC.height = 44;

            NPC.friendly = false;

            NPC.damage = 20;

            NPC.knockBackResist = 0.9f;
        }

        public int buffedNPC = -1;
        public override void AI()
        {
            NPC.TargetClosest();

            Player player = Main.player[NPC.target];

            NPC.velocity += new Vector2(Vector2.Normalize(player.Center - NPC.Center).X * 0.2f, 0);

            NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -1.5f, 1.5f);

            if (NPC.velocity.X > 0.01f || NPC.velocity.X < -0.01f)
            {
                NPC.spriteDirection = (NPC.velocity.X < 0) ? -1 : 1;
            }

            if (buffedNPC != -1 && (Main.npc[buffedNPC] == null || Main.npc[buffedNPC].active == false))
            {
                buffedNPC = -1;
            }

            if (buffedNPC != -1 && Vector2.DistanceSquared(Main.npc[buffedNPC].Center, NPC.Center) > 16 * 16 * 16 * 16)
            {
                Main.npc[buffedNPC].GetGlobalNPC<GoblinBardGlobal>().hasBardBuff = false;

                buffedNPC = -1;
            }

            if (buffedNPC == -1)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i] != null && Main.npc[i].active && Vector2.DistanceSquared(Main.npc[i].Center, NPC.Center) <= 16 * 16 * 16 * 16 &&
                        !Main.npc[i].GetGlobalNPC<GoblinBardGlobal>().hasBardBuff &&
                        (Main.npc[i].type == ModContent.NPCType<GoblinShaman>() ||
                         Main.npc[i].type == ModContent.NPCType<Berserker.GoblinBerserker>()))
                    {
                        buffedNPC = i;

                        Main.npc[buffedNPC].GetGlobalNPC<GoblinBardGlobal>().hasBardBuff = true;

                        Main.npc[buffedNPC].defense = (int)(Main.npc[buffedNPC].defense * 1.2f);

                        SoundEngine.PlaySound(SoundID.DSTFemaleHurt, NPC.Center);

                        break;
                    }
                }
            }
        }

        public override void PostAI()
        {
            if (NPC.position == NPC.oldPosition)
            {
                NPC.velocity += new Vector2(0, -4f);
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (buffedNPC != -1 && Main.npc[buffedNPC] != null && Main.npc[buffedNPC].active)
            {
                Texture2D tex = ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Bard/PanfluteBardNote").Value;
                Texture2D mask = ModContent.Request<Texture2D>("EEMod/Textures/RadialGradient").Value;

                Helpers.DrawAdditive(mask, Main.npc[buffedNPC].Top + new Vector2(0, -20 + (float)(Math.Sin(Main.GameUpdateCount / 40f) * 4f)) - Main.screenPosition, Color.Violet, 0.33f);

                spriteBatch.Draw(tex, Main.npc[buffedNPC].Top + new Vector2(0, -20 + (float)(Math.Sin(Main.GameUpdateCount / 40f) * 4f)) - Main.screenPosition, null, Color.White, 0f, tex.TextureCenter(), 1f, SpriteEffects.None, 0f);
            }
        }
    }

    public class GoblinBardGlobal : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public bool hasBardBuff = false;
    }
}