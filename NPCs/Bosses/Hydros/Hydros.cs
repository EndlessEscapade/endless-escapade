using EEMod.Items.Materials;
using EEMod.Items.TreasureBags;
using EEMod.Items.Weapons.Mage;
using EEMod.Items.Weapons.Summon.Minions;
using EEMod.Items.Weapons.Melee;
using EEMod.Items.Weapons.Ranger.Guns;
using EEMod.Items.Weapons.Melee.Yoyos;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace EEMod.NPCs.Bosses.Hydros
{
    [AutoloadBossHead]
    public class Hydros : EENPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 8;
        }

        public override void FindFrame(int frameHeight) //Frame counter
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];
            if (NPC.frameCounter++ > 4)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y = NPC.frame.Y + frameHeight;
            }
            if (NPC.frame.Y >= frameHeight * 7)
            {
                NPC.frame.Y = 0;
                return;
            }
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            NPC.lifeMax = 1600;
            NPC.defense = 12;
            NPC.damage = 20;
            NPC.knockBackResist = 0;

            NPC.value = Item.buyPrice(0, 3, 5, 0);

            NPC.HitSound = new LegacySoundStyle(3, 1, Terraria.Audio.SoundType.Sound);
            NPC.DeathSound = new LegacySoundStyle(4, 1, Terraria.Audio.SoundType.Sound);
            BossBag = ItemType<HydrosBag>();
            NPC.width = 226;
            NPC.height = 120;

            NPC.boss = true;
            NPC.noGravity = true;

            NPC.noTileCollide = true;
        }

        public override void OnKill()
        {
            if (!Main.expertMode)
            {
                int randVal = Main.rand.Next(5);
                switch (randVal)
                {
                    case 1:
                        Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemType<CyanoburstTome>(), 1);
                        break;

                    case 2:
                        Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemType<Triggerfish>(), 1);
                        break;

                    case 3:
                        //Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Hydroshot>(), 1);
                        break;

                    case 4:
                        Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemType<EnchantedCoral>(), 1);
                        break;
                }
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemType<HydrosScales>(), Main.rand.Next(28, 56));
                EEWorld.EEWorld.downedHydros = true;
            }
            else
            {
                NPC.DropBossBags();
            }
        }

        public float[] ai = new float[NPC.maxAI];

        private void Move(Player player, float sped, float TR, Vector2 addon)
        {
            Vector2 moveTo = player.Center + addon;
            float speed = sped;
            Vector2 move = moveTo - NPC.Center;
            float magnitude = move.Length(); // (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
            float turnResistance = TR;

            move = (NPC.velocity * turnResistance + move) / (turnResistance + 1f);
            magnitude = move.Length();
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
            NPC.velocity = move;
        }

        public void SpawnProjectileNearPlayerOnTile(int dist)
        {
            int distFromPlayer = dist;
            int playerTileX = (int)Main.player[NPC.target].position.X / 16;
            int playerTileY = (int)Main.player[NPC.target].position.Y / 16;
            int tileX = (int)NPC.position.X / 16;
            int tileY = (int)NPC.position.Y / 16;
            int teleportCheckCount = 0;
            bool hasTeleportPoint = false;
            //player is too far away, don't teleport.
            if (Vector2.DistanceSquared(NPC.Center, Main.player[NPC.target].Center) > (2000f * 2000f))
            {
                teleportCheckCount = 100;
                hasTeleportPoint = true;
            }
            while (!hasTeleportPoint && teleportCheckCount < 100)
            {
                teleportCheckCount++;
                int tpTileX = Main.rand.Next(playerTileX - distFromPlayer, playerTileX + distFromPlayer);
                int tpTileY = Main.rand.Next(playerTileY - distFromPlayer, playerTileY + distFromPlayer);
                for (int tpY = tpTileY; tpY < playerTileY + distFromPlayer; tpY++)
                {
                    if ((tpY < playerTileY - 4 || tpY > playerTileY + 4 || tpTileX < playerTileX - 4 || tpTileX > playerTileX + 4) && (tpY < tileY - 1 || tpY > tileY + 1 || tpTileX < tileX - 1 || tpTileX > tileX + 1) && !Framing.GetTileSafely(tpTileX, tpY).IsActive)
                    {
                        if (Main.tileSolid[Framing.GetTileSafely(tpTileX, tpY).type] && !Collision.SolidTiles(tpTileX - 1, tpTileX + 1, tpY - 4, tpY - 1))
                        {
                            Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_NPC(NPC), tpTileX * 16, tpY * 16, 0, 0, ProjectileType<Geyser>(), 1, 0f, Main.myPlayer, .3f, 140);
                            hasTeleportPoint = true;
                            NPC.netUpdate = true;
                            break;
                        }
                    }
                }
            }
        }

        private int prepare;
        public bool flaginOut;
        public float dist1;
        private readonly Vector2[] potentialMinionArray = new Vector2[3];

        public override void AI()
        {
            int phaseChange = 400;
            int speed = 6;
            int TR = 40;
            NPC.ai[0]++;

            if (Main.netMode != NetmodeID.MultiplayerClient) // 1
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) // 1
                {
                    //Idle
                    NPC.TargetClosest();
                    Player target = Main.player[NPC.target];
                    if (NPC.ai[1] != 2)
                    {
                        NPC.rotation = NPC.velocity.X / 32f;
                        if (target.Center.X > NPC.Center.X)
                        {
                            NPC.spriteDirection = 1;
                        }
                        else
                        {
                            NPC.spriteDirection = -1;
                        }
                    }

                    if (NPC.ai[1] == 0)
                    {
                        Move(target, speed, TR, Vector2.Zero);
                    }

                    if (NPC.ai[0] % 400 == 0)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            NPC.ai[1] = Main.rand.Next(0, 4);
                        }

                        prepare = 0;
                        NPC.rotation = 0;
                        flaginOut = true;
                        NPC.ai[2] = 0;
                        dist1 = 200;
                        for (int i = 0; i < 3; i++)
                        {
                            potentialMinionArray[i] = NPC.Center + new Vector2(Main.rand.Next(-500, 500), Main.rand.Next(-500, 500));
                        }
                        NPC.netUpdate = true;
                    }
                    switch (NPC.ai[1])
                    {
                        case 0:
                            break;

                        case 1:
                        {
                            Move(target, speed, TR, Vector2.Zero);
                            for (int i = 0; i < 10; i++)
                            {
                                if (NPC.ai[0] % 200 == 0)
                                {
                                    SpawnProjectileNearPlayerOnTile(30);
                                }
                            }
                            NPC.velocity *= 0.98f;
                            break;
                        }
                        case 2:
                        {
                            float timeToDash = 320;
                            if (NPC.ai[0] % 400 <= timeToDash - 50)
                            {
                                NPC.rotation = NPC.velocity.X / 32f;
                                if (target.Center.X > NPC.Center.X)
                                {
                                    NPC.spriteDirection = 1;
                                }
                                else
                                {
                                    NPC.spriteDirection = -1;
                                }
                                Move(target, speed, 9, new Vector2(400, 0));
                            }
                            else if (NPC.ai[0] % 400 >= timeToDash && NPC.ai[0] % 400 < phaseChange - 20)
                            {
                                NPC.rotation = (target.position.X - NPC.position.X) / 500f;
                                if (NPC.rotation > 1.6f) { NPC.rotation = 1.6f; }
                                //npc.rotation = npc.velocity.X / 16f;
                                NPC.velocity.Y = ((float)Math.Sin((6 * ((NPC.ai[0] % phaseChange) - timeToDash) / (phaseChange - timeToDash)) + 1.57f)) * 10;
                                int speedOfDash = 25;
                                NPC.velocity.X = -(speedOfDash - ((NPC.ai[0] % phaseChange) - timeToDash) / (float)((phaseChange - timeToDash) / speedOfDash));
                            }
                            else if (NPC.ai[0] % 400 < phaseChange - 60)
                            {
                                NPC.rotation = -(prepare / 190f);
                                prepare += 4;
                                Move(target, 19, 9, new Vector2(400 + prepare, -prepare / 2));
                                NPC.velocity *= 0.99f;
                            }
                            else
                            {
                                NPC.rotation -= NPC.rotation / 16f;
                            }
                            break;
                        }
                        case 3:
                        {
                            NPC.velocity *= .99f;
                            Move(target, speed, TR, Vector2.Zero);
                            dist1 -= 1;

                            if (NPC.ai[0] % 400 < 200)
                            {
                                for (int j = 0; j < 3; j++)
                                {
                                    for (int i = 0; i < 10; i++)
                                    {
                                        double deg = (double)NPC.ai[2] + (i * 36); //The degrees, you can multiply projectile.ai[1] to make it orbit faster, may be choppy depending on the value
                                        double rad = deg * (Math.PI / 180) * 0.7f; //Convert degrees to radians
                                        if (dist1 - (i * 10) > 0)
                                        {
                                            int num7 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Clentaminator_Blue, 0f, 0f, 0, Color.AliceBlue, .7f);
                                            Main.dust[num7].position.X = potentialMinionArray[j].X - (int)(Math.Cos(rad) * (dist1 - (i * 10)));
                                            Main.dust[num7].position.Y = potentialMinionArray[j].Y - (int)(Math.Sin(rad) * (dist1 - (i * 10)));
                                            Main.dust[num7].noGravity = true;
                                        }
                                        NPC.ai[2] += 1;
                                    }
                                }
                            }
                            else
                            {
                                if (NPC.ai[0] % 400 == 200)
                                {
                                    for (int j = 0; j < 3; j++)
                                    {
                                        for (var a = 0; a < 50; a++)
                                        {
                                            Vector2 vector = new Vector2(0, 20).RotatedBy(Math.PI * 0.04 * a, default);
                                            int index = Dust.NewDust(potentialMinionArray[j], 22, 22, DustID.Clentaminator_Blue, vector.X, vector.Y, 0, Color.AliceBlue, .7f);
                                            Main.dust[index].velocity *= .5f;
                                            Main.dust[index].noGravity = true;
                                        }
                                        NPC.NewNPC((int)potentialMinionArray[j].X, (int)potentialMinionArray[j].Y, NPCType<HydrosMinion>());
                                        NPC.netUpdate = true;
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }

        /*public override bool CheckDead()
        {
            int goreIndex = Gore.NewGore(new Vector2(npc.position.X + (float)(npc.width / 2) - 24f, npc.position.Y + (float)(npc.height / 2) - 24f), default(Vector2), mod.GetGoreSlot("Gores/HydrosGore"), 1f);
            Main.gore[goreIndex].scale = 1.5f;
            Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;

            return true;
        }*/
    }
}