using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.EEWorld;
using EEMod.NPCs.Bosses.Archon;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace EEMod.NPCs.Bosses.Gallagar
{
    //	[AutoloadBossHead]
    public class Gallagar : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gallagar");
            Main.npcFrameCount[npc.type] = 6;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Filters.Scene.Deactivate("EEMod:Boom");
            }
        }

        public override void SetDefaults()
        {
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.friendly = false;
            npc.buffImmune[BuffID.Confused] = true;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.Venom] = true;

            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            music = MusicID.Boss2;
            musicPriority = MusicPriority.BossMedium;

            npc.width = 106;
            npc.height = 108;
            npc.damage = 42;
            npc.defense = 11;
            npc.lifeMax = 2400;
            npc.aiStyle = 0;
            npc.value = Item.buyPrice(0, 6, 0, 0);

            npc.knockBackResist = 0f;
            npc.npcSlots = 24f;

            // bossBag = ModContent.ItemType<GallagarBag>();
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 1.80 * bossLifeScale);
            npc.defense = 15;
            npc.damage = 90;
        }

        private bool firstTick = true;
        private int clossness;
        private bool leftOrRight = false;
        public override void AI()
        {

            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            if (firstTick)
            {
                clossness = 800;
                npc.position = player.position + new Vector2(800, 0);
                npc.alpha = 255;
                handL = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y - 128, ModContent.NPCType<GallagarLHand>(), npc.whoAmI + 1, npc.whoAmI, Main.myPlayer);
                handR = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y - 128, ModContent.NPCType<GallagarRHand>(), npc.whoAmI + 2, npc.whoAmI, Main.myPlayer);
                firstTick = false;
            }
            else
            {
                npc.ai[0]++;
                npc.alpha -= 2;
                if (npc.ai[1] != 4 && npc.ai[1] != 3)
                    Move(player, 17, 2, new Vector2(clossness, 0));
                if (npc.ai[0] % 400 == 0)
                {
                    int leftOrRightint = leftOrRight ? -1 : 1;
                    clossness -= 200 * leftOrRightint;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        if (clossness != 200)
                        {
                            for (int i = 0; i < 20; i++)
                                npc.ai[1] = Main.rand.Next(1, 4);
                        }
                        npc.netUpdate = true;
                    }
                    if (clossness == 0)
                        npc.ai[1] = 4;
                }
                if (npc.ai[1] == 1)
                {
                    if (npc.ai[0] % 80 == 0)
                    {
                        int randDisX = Main.rand.Next(0, 800) * (leftOrRight ? 1 : -1);
                        int randDisY = Main.rand.Next(-400, 400);
                        float disX = (float)(Main.player[npc.target].position.X + (double)Main.player[npc.target].width * 0.5 - (npc.position.X + randDisX)) + (float)Main.rand.Next(-50, 51);
                        float disY = (float)(Main.player[npc.target].position.Y + (double)Main.player[npc.target].height * 0.5 - (npc.position.Y + randDisY)) + (float)Main.rand.Next(-50, 51);
                        float dist = (float)Math.Sqrt((double)disX * (double)disX + (double)disY * (double)disY);
                        float speed = 3 / dist;
                        float speedX = disX * speed;
                        float speedY = disY * speed;
                        for (var a = 1; a < 17; a++)
                        {
                            float rotFactor = (0.52359877f) + Main.rand.NextFloat(-0.07f, 0.07f);
                            Vector2 vector = new Vector2(speedX, speedY).RotatedBy((rotFactor * a), default);
                            int index = Projectile.NewProjectile((npc.Center.X + randDisX), (npc.Center.Y + randDisY), vector.X, vector.Y, ModContent.ProjectileType<HadesFireball>(), 1, 0f, Main.myPlayer, 0f, 0f);
                            Main.projectile[index].damage = npc.damage / 2;
                        }
                    }
                }
                if (npc.ai[1] == 2)
                {
                    if (npc.ai[0] % 400 == 30)
                    {
                        Projectile.NewProjectile(npc.Center + new Vector2(0, -100), Vector2.Zero, ModContent.ProjectileType<Portal>(), 150, 0f, Main.myPlayer, npc.whoAmI);
                    }
                }
                if (npc.ai[1] == 3)
                {
                    Move(player, 10, 7, new Vector2(clossness, 0));
                }
                if (npc.ai[1] == 4)
                {
                    npc.ai[2]++;
                    if (npc.ai[2] < 60)
                        Spaz();
                    else
                        npc.velocity = new Vector2(leftOrRight ? 100 - (npc.ai[2]) : -100 + (npc.ai[2]), (float)Math.Sin(npc.ai[2] / 180));
                    if (npc.ai[2] == 100)
                    {
                        npc.ai[2] = 0;
                        npc.ai[0] = 0;
                        for (int i = 0; i < 20; i++)
                            npc.ai[1] = Main.rand.Next(1, 4);
                        leftOrRight = !leftOrRight;
                        clossness = leftOrRight ? -800 : 800;
                    }
                }
            }
            npc.ai[3]++;
        }
        public float opac = 0;
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = TextureCache.Gallagar;
            if (npc.ai[1] == 4)
            {
                AfterImage.DrawAfterimage(spriteBatch, texture, 0, npc, 1.5f, 1f, 3, false, 0f, 0f, new Color(drawColor.R, drawColor.G, drawColor.B, 150));
            }
            string key = "";
            key = "EEMod:Boom";
            opac += 0.1f;
            if (opac < 0)
                opac = 0;
            Filters.Scene["EEMod:Boom"].GetShader().UseOpacity(opac);
            if (Main.netMode != NetmodeID.Server && !Filters.Scene[key].IsActive())
            {
                Filters.Scene.Activate("EEMod:Boom", npc.Center).GetShader().UseColor(0, 0, 0).UseOpacity(opac);
            }

            return;
        }
        private void Spaz()
        {
            npc.velocity = new Vector2(Main.rand.NextFloat(-5, 5), (Main.rand.NextFloat(-5, 5)));
        }
        private void Move(Player player, float sped, float TR, Vector2 addon)
        {
            Vector2 moveTo = player.Center + addon;
            float speed = sped;
            Vector2 move = moveTo - npc.Center;
            float magnitude = move.Length(); //(float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
            float turnResistance = TR;

            move = (npc.velocity * turnResistance + move) / (turnResistance + 1f);
            magnitude = move.Length();
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
            npc.velocity = move;
        }

        public int handL = 0;
        public int handR = 0;

        public override void NPCLoot()
        {
            if (Main.expertMode)
                npc.DropBossBags();
        }

        public override bool CheckDead()
        {
            return EEWorld.EEWorld.downedGallagar = true;
        }

        public override void FindFrame(int frameHeight) //Frame counter
        {
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            if (npc.Center.X - player.Center.X < 0)
            {
                npc.spriteDirection = -1;
            }
            else
            {
                npc.spriteDirection = 1;
            }
            if (npc.frameCounter++ > 4)
            {
                npc.frameCounter = 0;
                npc.frame.Y = npc.frame.Y + frameHeight;
            }
            if (npc.frame.Y >= frameHeight * 6)
            {
                npc.frame.Y = 0;
                return;
            }
        }
    }
}
