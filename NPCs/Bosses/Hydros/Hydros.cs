using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Items.Weapons.Melee;
using EEMod.Items.Weapons.Mage;
using EEMod.Items.Weapons.Ranger;
using EEMod.Items.Materials;
using Terraria.Audio;
using EEMod.EEWorld;
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

        private int frameNumber = 0;
        private int frameSpeed = 4;
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter >= frameSpeed)
            {
                npc.frameCounter = 0;
                frameNumber++;
                if (frameNumber >= 8)
                {
                    frameNumber = 0;
                }
                npc.frame.Y = frameNumber * 120;
            }
        }


        public override void SetDefaults()
        {
            npc.aiStyle = 0;

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
        }

        public override void NPCLoot()
        {
            int randVal = Main.rand.Next(3);
            if (randVal == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<HydrosEye>(), 1);
            }
            else if(randVal == 1)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<CyanoburstTome>(), 1);
            }
            else
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<Hydroshot>(), 1);
            }
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<HydrosScales>(), Main.rand.Next(28, 56));
            EEWorld.EEWorld.downedHydros = true;
        }

        private int attackTimer = 180;
        private int dashCooldown = 30;
        private bool canAttack;
        private bool usingLazer;

        public float[] ai = new float[NPC.maxAI];


        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient) // 1
            {
                //Idle
                npc.TargetClosest();
                Player target = Main.player[npc.target];

                if (target.Center.X > npc.Center.X)
                {
                    npc.spriteDirection = 1;
                }
                else
                {
                    npc.spriteDirection = -1;
                }

                if (!target.wet)
                {
                    npc.damage = 9999;
                    npc.defense = 9999;
                }
                else
                {
                    npc.defense = 12;
                    npc.damage = 20;
                }

                if (npc.Center.Y < target.Center.Y && usingLazer == false)
                {
                    npc.position.Y += 2;
                }
                if (npc.Center.Y > target.Center.Y && usingLazer == false)
                {
                    npc.position.Y -= 2;
                }

                if (npc.Center.X < target.Center.X && usingLazer == false)
                {
                    npc.position.X += 2;
                }
                if (npc.Center.X > target.Center.X && usingLazer == false)
                {
                    npc.position.X -= 2;
                }

                //Idle Dash attacks
                if (npc.ai[0] == 0 && canAttack == true)
                {
                    usingLazer = false;
                    // Vector2.Distance(npc.Center, target.Center) < 5000
                    if (dashCooldown > 0 && npc.WithinRange(target.Center, 5000))
                    {
                        frameSpeed = 8;
                        if (npc.Center.X > target.Center.X)
                        {
                            npc.position.X -= 13;
                        }
                        if (npc.Center.X < target.Center.X)
                        {
                            npc.position.X += 13;
                        }
                        if (npc.Center.X > target.Center.X - 7 && npc.Center.X < target.Center.X + 7)
                        {
                            canAttack = false;
                        }

                        dashCooldown--;
                        npc.netUpdate = true;
                    }
                    if (dashCooldown <= 0)
                    {
                        dashCooldown = 60;
                        frameSpeed = 4;
                        canAttack = false;
                    }
                }


                //Attack 1
                if (npc.ai[0] == 1 && canAttack == true)
                {
                    frameSpeed = 4;
                    usingLazer = true;
                    int projectile = Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y + 16), new Vector2(1, 0), ProjectileType<HydrosBeam>(), 24, 0f, Main.myPlayer, 0, npc.whoAmI);
                    Vector2 direction = Main.projectile[projectile].DirectionTo(Main.player[npc.target].Center);
                    Main.projectile[projectile].velocity = Main.projectile[projectile].velocity.RotatedBy(direction.ToRotation() - Main.projectile[projectile].rotation);
                    canAttack = false;
                    npc.netUpdate = true;
                }



                //Attack 2
                if (npc.ai[0] == 2 && canAttack == true)
                {
                    usingLazer = false;
                    frameSpeed = 4;
                    int maxMinions = Main.rand.Next(1, 3);
                    for (int i = 0; i < maxMinions; i++)
                    {
                        int newnpc = NPC.NewNPC((int)npc.Center.X + Main.rand.Next(-60, 61), (int)npc.Center.Y + Main.rand.Next(-40, 41), NPCType<HydrosMinion>(), 20, 0, Main.myPlayer);
                    }
                    canAttack = false;
                    npc.netUpdate = true;
                }

                attackTimer--;
                if(attackTimer <= 0)
                {
                    npc.ai[0] = Main.rand.Next(3);
                    attackTimer = 120;
                    canAttack = true;
                    frameSpeed = 4;
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
