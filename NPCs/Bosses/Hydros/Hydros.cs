using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Items.Materials;
using EEMod.Items.Weapons.Mage;
using EEMod.Items.Weapons.Melee;
using EEMod.Items.Weapons.Ranger;
using System;
using EEMod.NPCs.Bosses.Hydros;
using EEMod.Projectiles.CoralReefs;
using EEMod.Items.Weapons.Summon;
using static Terraria.ModLoader.ModContent;

namespace EEMod.NPCs.Bosses.Hydros
{
    [AutoloadBossHead]
    public class Hydros : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 8;
        }

        public override void FindFrame(int frameHeight) //Frame counter
        {
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            if (npc.frameCounter++ > 4)
            {
                npc.frameCounter = 0;
                npc.frame.Y = npc.frame.Y + frameHeight;
            }
            if (npc.frame.Y >= frameHeight * 7)
            {
                npc.frame.Y = 0;
                return;
            }
        }


        public override void SetDefaults()
        {
            npc.aiStyle = -1;

            npc.lifeMax = 1600;
            npc.defense = 12;
            npc.damage = 20;
            npc.knockBackResist = 0;

            npc.value = Item.buyPrice(0, 3, 5, 0);

            npc.HitSound = new LegacySoundStyle(3, 1, Terraria.Audio.SoundType.Sound);
            npc.DeathSound = new LegacySoundStyle(4, 1, Terraria.Audio.SoundType.Sound);

            npc.width = 226;
            npc.height = 120;

            npc.boss = true;
            npc.noGravity = true;

            npc.noTileCollide = true;
        }

        public override void NPCLoot()
        {
            int randVal = Main.rand.Next(5);
            if (randVal == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<HydrosEye>(), 1);
            }
            else if (randVal == 1)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<CyanoburstTome>(), 1);
            }
            else if (randVal == 2)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Fishtol>(), 1);
            }
            else if (randVal == 3)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Hydroshot>(), 1);
            }
            else if (randVal == 4)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<EnchantedCoral>(), 1);
            }
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<HydrosScales>(), Main.rand.Next(28, 56));
            EEWorld.EEWorld.downedHydros = true;
        }


        public float[] ai = new float[NPC.maxAI];
        private void Move(Player player, float sped, float TR, Vector2 addon)
        {
            Vector2 moveTo = player.Center + addon;
            float speed = sped;
            Vector2 move = moveTo - npc.Center;
            float magnitude = move.Length(); // (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
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

        public void SpawnProjectileNearPlayerOnTile(int dist)
        {
            int distFromPlayer = dist;
            int playerTileX = (int)Main.player[npc.target].position.X / 16;
            int playerTileY = (int)Main.player[npc.target].position.Y / 16;
            int tileX = (int)npc.position.X / 16;
            int tileY = (int)npc.position.Y / 16;
            int teleportCheckCount = 0;
            bool hasTeleportPoint = false;
            //player is too far away, don't teleport.
            if (Vector2.Distance(npc.Center, Main.player[npc.target].Center) > 2000f)
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
                    if ((tpY < playerTileY - 4 || tpY > playerTileY + 4 || tpTileX < playerTileX - 4 || tpTileX > playerTileX + 4) && (tpY < tileY - 1 || tpY > tileY + 1 || tpTileX < tileX - 1 || tpTileX > tileX + 1) && (Main.tile[tpTileX, tpY].nactive()))
                    {
                        if ((Main.tileSolid[Main.tile[tpTileX, tpY].type]) && !Collision.SolidTiles(tpTileX - 1, tpTileX + 1, tpY - 4, tpY - 1))
                        {
                            Projectile.NewProjectile(tpTileX * 16, tpY * 16, 0, 0, ProjectileType<Geyser>(), 1, 0f,Main.myPlayer,.3f,140);
                            hasTeleportPoint = true;
                            npc.netUpdate = true;
                            break;
                        }
                    }
                }
            }
        }
        int prepare;
        int timeForAttack;
        public bool flaginOut;
        public float dist1;
        Vector2[] potentialMinionArray = new Vector2[3];
        public override void AI()
        {
            int phaseChange = 400;
            int speed = 6;
            int TR = 40;
            npc.ai[0]++;
            
            if (Main.netMode != NetmodeID.MultiplayerClient) // 1
            if (Main.netMode != NetmodeID.MultiplayerClient) // 1
            {
                //Idle
                npc.TargetClosest();
                Player target = Main.player[npc.target];
                if (npc.ai[1] != 2)
                {
                    npc.rotation = npc.velocity.X / 32f;
                    if (target.Center.X > npc.Center.X)
                    {
                        npc.spriteDirection = 1;
                    }
                    else
                    {
                        npc.spriteDirection = -1;
                    }
                }

                if (npc.ai[1] == 0)
                Move(target, speed, TR, Vector2.Zero);
               
                if (npc.ai[0] % 400 == 0)
                {
                    for(int i = 0; i<5; i++)
                    npc.ai[1] = Main.rand.Next(0,4);
                    prepare = 0;
                    npc.rotation = 0;
                    flaginOut = true;
                    npc.ai[2] = 0;
                    dist1 = 200;
                    for(int i = 0; i<3;i++)
                    {
                        potentialMinionArray[i] = npc.Center + new Vector2(Main.rand.Next(-500,500), Main.rand.Next(-500, 500)); 
                    }
                        npc.netUpdate = true;
                }
                switch(npc.ai[1])
                {
                    case 0:
                        break;
                    case 1:
                        {
                            Move(target, speed, TR, Vector2.Zero);
                            for (int i = 0; i < 10; i++)
                            {
                                if (npc.ai[0] % 200 == 0)
                                SpawnProjectileNearPlayerOnTile(30);
                            }
                            npc.velocity *= 0.98f;
                            break;
                        }
                    case 2:
                        {
                            float timeToDash = 320;
                            if (npc.ai[0] % 400 <= timeToDash - 50)
                            {
                                npc.rotation = npc.velocity.X / 32f;
                                if (target.Center.X > npc.Center.X)
                                {
                                    npc.spriteDirection = 1;
                                }
                                else
                                {
                                    npc.spriteDirection = -1;
                                }
                                Move(target, speed, 9, new Vector2(400, 0));
                            }
                            else if (npc.ai[0] % 400 >= timeToDash && npc.ai[0] % 400 < phaseChange - 20)
                            {
                                npc.rotation = (target.position.X - npc.position.X) / 500f;
                                if(npc.rotation > 1.6f) { npc.rotation = 1.6f; }
                                //npc.rotation = npc.velocity.X / 16f;
                                npc.velocity.Y = ((float)Math.Sin((6 * ((npc.ai[0] % phaseChange) - timeToDash) / (phaseChange - timeToDash)) + 1.57f)) * 10;
                                int speedOfDash = 25;
                                npc.velocity.X = -(speedOfDash - ((npc.ai[0] % phaseChange) - timeToDash) / (float)((phaseChange - timeToDash) / speedOfDash));
                            }
                            else if(npc.ai[0] % 400 < phaseChange - 60)
                            {
                                npc.rotation = -(prepare / 190f);
                                prepare += 4;
                                Move(target, 19, 9, new Vector2(400 + prepare, -prepare/2));
                                npc.velocity *= 0.99f;
                            }
                            else
                            {
                               npc.rotation -= npc.rotation/16f;
                            }
                            break;
                        }
                    case 3:
                        {
                            npc.velocity *= .99f;
                            Move(target, speed, TR, Vector2.Zero);
                                dist1 -= 1;
                            
                            if (npc.ai[0] % 400 < 200)
                            {
                                for (int j = 0; j < 3; j++)
                                {
                                    for (int i = 0; i < 10; i++)
                                    {
                                        double deg = (double)npc.ai[2] + (i * 36); //The degrees, you can multiply projectile.ai[1] to make it orbit faster, may be choppy depending on the value
                                        double rad = deg * (Math.PI / 180) * 0.7f; //Convert degrees to radians
                                        if (dist1 - (i * 10) > 0)
                                        {
                                            int num7 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 113, 0f, 0f, 0, Color.AliceBlue, .7f);
                                            Main.dust[num7].position.X = potentialMinionArray[j].X - (int)(Math.Cos(rad) * (dist1 - (i * 10)));
                                            Main.dust[num7].position.Y = potentialMinionArray[j].Y - (int)(Math.Sin(rad) * (dist1 - (i * 10)));
                                            Main.dust[num7].noGravity = true;
                                        }
                                        npc.ai[2] += 1;
                                    }
                                }
                            }
                            else
                            {
                                if (npc.ai[0] % 400 == 200)
                                {
                                    for (int j = 0; j < 3; j++)
                                    {
                                        for (var a = 0; a < 50; a++)
                                        {
                                            Vector2 vector = new Vector2(0, 20).RotatedBy(((Math.PI * 0.04) * a), default);
                                            int index = Dust.NewDust(potentialMinionArray[j], 22, 22, 113, vector.X, vector.Y, 0, Color.AliceBlue, .7f);
                                            Main.dust[index].velocity *= .5f;
                                            Main.dust[index].noGravity = true;
                                        }
                                        NPC.NewNPC((int)potentialMinionArray[j].X, (int)potentialMinionArray[j].Y, NPCType<HydrosMinion>());
                                        npc.netUpdate = true;
                                    }
                                }
                            }
                            break;
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
