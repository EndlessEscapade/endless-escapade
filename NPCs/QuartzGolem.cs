using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Items.Materials;

namespace EEMod.NPCs
{
    public class QuartzGolem : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Quartz Golem");
            Main.npcFrameCount[npc.type] = 8;
        }

        public bool flaginOut;
        public override void SetDefaults()
        {
            npc.alpha = 0;
            npc.aiStyle = -1;
            npc.lifeMax = Main.expertMode ? 150 : 150;    //this is the npc health
            npc.damage = Main.expertMode ? 15 : 25;  //this is the npc damage
            npc.defense = 6;         //this is the npc defense
            npc.knockBackResist = 0f;
            npc.width = 28; //this is where you put the npc sprite width.     important
            npc.height = 47; //this is where you put the npc sprite height.   important
            npc.boss = false;
            npc.lavaImmune = true;       //this make the npc immune to lava
            npc.noGravity = true;           //this make the npc float
            npc.noTileCollide = true;        //this make the npc go thru walls
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.behindTiles = false;
            flaginOut = false;
            npc.value = Item.buyPrice(0, 0, 10, 0);
            npc.npcSlots = 1f;
            npc.netAlways = true;
            //npc.music = (this.music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Dionysus"));
        }
        public int Timer;
        public float dist;
        public float dist1;

        public override void AI()
        {
            npc.ai[2]++;
            if (npc.ai[2] == 200)
                npc.ai[2] = 0;
            npc.TargetClosest(false);
            npc.spriteDirection = -1;
            if (npc.velocity.X > 0)
                npc.spriteDirection = 1;
            Player player = Main.player[npc.target];
            Vector2 moveTo = player.Center;
            float speed = 1f;
            Vector2 move = moveTo - npc.Center;
            float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
            float turnResistance = 10f;
            move = (npc.velocity * turnResistance + move) / (turnResistance + 1f);
            magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
            if (magnitude > speed)
            {
                move *= speed / magnitude;
            }
            npc.velocity = move;
            Vector2 moveFor = moveTo - npc.Center;
            dist = (float)Math.Sqrt(moveFor.X * moveFor.X + moveFor.Y * moveFor.Y);
            if (dist < 200 && npc.ai[2] == 100)
            {
                flaginOut = true;
            }
            if (dist < 200)
                npc.defense = 20;
            else
                npc.defense = 6;
            if (flaginOut)
            {
                npc.dontTakeDamage = true;
                if (dist1 < -100)
                    dist1 = -100;
                if (dist1 > 100)
                    dist1 = 100;
                npc.velocity *= 0.8f;
                npc.ai[0]++;
                dist1 -= 2;
                if (npc.ai[0] >= 0 && npc.ai[0] <= 50)
                {
                    npc.alpha += 5;
                }

                if (npc.ai[0] >= 50 && npc.ai[0] <= 100)
                {
                    npc.alpha -= 5;
                }

                if (npc.ai[0] == 100)
                {
                    dist1 = 100;
                    npc.ai[0] = 0;
                    flaginOut = false;
                }
                for (int i = 0; i < 5; i++)
                {
                    double deg = (double)npc.ai[1] + (i * 72); //The degrees, you can multiply projectile.ai[1] to make it orbit faster, may be choppy depending on the value
                    double rad = deg * (Math.PI / 180); //Convert degrees to radians

                    int num7 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 123, 0f, 0f, 0, new Color(255, 217, 184, 255), 0.5f);
                    Main.dust[num7].position.X = npc.Center.X - (int)(Math.Cos(rad) * dist1);
                    Main.dust[num7].position.Y = npc.Center.Y - (int)(Math.Sin(rad) * dist1);

                    npc.ai[1] += 1;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        if (Main.rand.NextBool(2))
                        {
                            Main.dust[num7].scale *= 0.5f;
                        }
                        npc.netUpdate = true;
                    }
                }
            }
            else
            {
                npc.dontTakeDamage = false;
                dist1 = 100;
                npc.alpha -= 5;
                if (npc.alpha < 0)
                    npc.alpha = 0;
                npc.ai[0] = 0;
            }
        }

        public int animDir = 1;
        public override void FindFrame(int frameHeight) //Frame counter
        {
            npc.spriteDirection = -1;
            if (npc.velocity.X > 0)
                npc.spriteDirection = 1;
            if (dist > 150)
            {
                if (npc.frameCounter++ > 4)
                {
                    npc.frameCounter = 0;
                    npc.frame.Y = npc.frame.Y + frameHeight;
                }
                if (npc.frame.Y >= frameHeight * 3)
                {
                    npc.frame.Y = 0;
                    return;
                }
            }
            else
            {
                if (npc.frameCounter++ > 4)
                {
                    npc.frameCounter = 0;
                    npc.frame.Y = npc.frame.Y + frameHeight * animDir;
                }
                if (npc.frame.Y >= frameHeight * 7)
                {
                    animDir = -1;
                    return;
                }
                if (npc.frame.Y <= frameHeight * 4)
                {
                    animDir = 1;
                    return;
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedBoss1 == false)
            {
                return SpawnCondition.Underground.Chance * 0.1f;
            }
            else
            {
                return SpawnCondition.Underground.Chance * 0.1f;
            }
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1f;
            return null;
        }

        public override void NPCLoot()
        {
            // this is still pretty useless to do
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<QuartzicLifeFragment>(), Main.rand.Next(1));
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<QuartzGem>(), Main.rand.Next(2, 4));
        }
    }
}
