using EEMod.Extensions;
using EEMod.NPCs.Goblins.Bard;
using EEMod.NPCs.Goblins.Berserker;
using EEMod.NPCs.Goblins.Scrapgunner;
using EEMod.NPCs.Goblins.Shaman;
using EEMod.Prim;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.Goblins.Watchman
{
    public class GoblinWatchman : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Goblin Watchman");
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            NPC.HitSound = SoundID.NPCHit40;
            NPC.DeathSound = SoundID.NPCDeath42;

            NPC.alpha = 0;

            NPC.lifeMax = 550;
            NPC.defense = 10;

            NPC.width = 26;
            NPC.height = 44;

            NPC.friendly = false;

            NPC.damage = 20;

            NPC.knockBackResist = 0.9f;
        }

        public override void AI()
        {
            NPC.TargetClosest();
        }

        public bool alertedFriends;

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Color defaultDrawColor = Lighting.GetColor((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f));

            NPC.spriteDirection = Main.LocalPlayer.Center.X < NPC.Center.X ? 1 : -1;

            if (Vector2.DistanceSquared(NPC.Center, Main.player[NPC.target].Center) <= 16 * 16 * 40 * 40)
            {
                Texture2D WatchmanAlert = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("NPCs/Goblins/Watchman/WatchmanAlert").Value;

                Main.spriteBatch.Draw(WatchmanAlert, NPC.Center + new Vector2(0, -32) - Main.screenPosition, null, defaultDrawColor, 0f, WatchmanAlert.TextureCenter(), 1f, SpriteEffects.None, 0f);

                if (!alertedFriends)
                {
                    alertedFriends = true;
                    SoundEngine.PlaySound(SoundID.DD2_GoblinScream, NPC.Center);

                    for(int i = 0; i < Main.maxNPCs; i++)
                    {
                        if (Vector2.DistanceSquared(NPC.Center, Main.npc[i].Center) <= 16 * 16 * 36 * 36)
                        {
                            if (Main.npc[i].type == ModContent.NPCType<GoblinShaman>())
                            {
                                (Main.npc[i].ModNPC as GoblinShaman).aggro = true;
                            }
                            if (Main.npc[i].type == ModContent.NPCType<CymbalBard>())
                            {
                                (Main.npc[i].ModNPC as CymbalBard).aggro = true;
                            }
                            if (Main.npc[i].type == ModContent.NPCType<PercussionBard>())
                            {
                                (Main.npc[i].ModNPC as PercussionBard).aggro = true;
                            }
                            if (Main.npc[i].type == ModContent.NPCType<PanfluteBard>())
                            {
                                (Main.npc[i].ModNPC as PanfluteBard).aggro = true;
                            }
                            if (Main.npc[i].type == ModContent.NPCType<GoblinBerserker>())
                            {
                                (Main.npc[i].ModNPC as GoblinBerserker).aggro = true;
                            }
                            if (Main.npc[i].type == ModContent.NPCType<GoblinScrapgunner>())
                            {
                                (Main.npc[i].ModNPC as GoblinScrapgunner).aggro = true;
                            }
                        }
                    }
                }

                return;
            } 
            if (Vector2.DistanceSquared(NPC.Center, Main.player[NPC.target].Center) <= 16 * 16 * 48 * 48 && !alertedFriends)
            {
                Texture2D WatchmanQuestion = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("NPCs/Goblins/Watchman/WatchmanQuestion").Value;

                Main.spriteBatch.Draw(WatchmanQuestion, NPC.Center + new Vector2(0, -32) - Main.screenPosition, null, defaultDrawColor, 0f, WatchmanQuestion.TextureCenter(), 1f, SpriteEffects.None, 0f);
            }
        }
    }
}