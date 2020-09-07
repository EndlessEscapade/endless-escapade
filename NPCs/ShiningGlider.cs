/*using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Materials;

namespace EEMod.NPCs
{
    public class ShiningGlider : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shining Glider");
            Main.npcFrameCount[npc.type] = 5;
        }

        public override void SetDefaults()
        {
            npc.width = 48;
            npc.height = 40;
            npc.value = 0;
            npc.npcSlots = 1;
            npc.aiStyle = 14;
            npc.lifeMax = Main.expertMode ? 70 : 140;
            npc.defense = Main.expertMode ? 6 : 6;
            npc.damage = Main.expertMode ? 22 : 36;
            npc.HitSound = SoundID.Item27;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.knockBackResist = 0.3f;
            npc.noGravity = true;
            npc.noTileCollide = false;
            isGliding = false;
            npc.value = 100f;
        }

        //public override void HitEffect(int hitDirection, double damage)
        //{
        //    bool isDead = npc.life <= 0;
        //    for (int m = 0; m < (isDead ? 25 : 5); m++)
        //    {
        //    }
        //}

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Underground.Chance * 0.1f;
        }

        float shootAI = 0;
        public bool isGliding = false;
        public override void AI()
        {
            npc.rotation = npc.velocity.X / 16f;
            npc.ai[0]++;
            Player player = Main.player[npc.target];
            bool playerActive = player != null && player.active && !player.dead;
            if (Main.netMode != NetmodeID.MultiplayerClient && playerActive)
            {
                if (npc.ai[0] >= 350 && npc.ai[0] <= 400)
                {
                    npc.velocity *= 0.95f;
                }

                if (npc.ai[0] >= 400 && npc.ai[0] <= 540)
                {
                    isGliding = true;
                    npc.velocity.X *= 0.99f;
                    npc.velocity.Y *= 0.99f;
                    npc.rotation = npc.velocity.X / 16f;
                    if (npc.ai[0] >= 410 && npc.ai[0] <= 420)
                    {
                        // LookInDirectionP(look);
                        npc.velocity = Helpers.MoveTowardsPlayer(30f, npc.velocity.X, npc.velocity.Y, player, npc.Center, npc.direction);
                    }
                }
                if (npc.ai[0] == 550)
                {
                    npc.ai[0] = 0;
                    isGliding = false;
                }
                npc.netUpdate = true;
            }
        }

        public override void FindFrame(int frameHeight) //Frame counter
        {
            npc.spriteDirection = -1;
            if (npc.velocity.X > 0)
            {
                npc.spriteDirection = 1;
            }

            if (!isGliding)
            {
                if (npc.frameCounter++ > 4)
                {
                    npc.frameCounter = 0;
                    npc.frame.Y = npc.frame.Y + frameHeight;
                }
                if (npc.frame.Y >= frameHeight * 4)
                {
                    npc.frame.Y = 0;
                    return;
                }
            }
            if (isGliding)
            {
                npc.frame.Y = frameHeight * 4;
            }
        }

        //private void LookInDirectionP(Vector2 look) // unused
        //{
        //    float angle = 0.5f * (float)Math.PI;
        //    if (look.X != 0f)
        //    {
        //        angle = (float)Math.Atan(look.Y / look.X);
        //    }
        //    else if (look.Y < 0f)
        //    {
        //        angle += (float)Math.PI;
        //    }
        //    if (look.X < 0f)
        //    {
        //        angle += (float)Math.PI;
        //    }
        //    npc.rotation = angle;
        //}

        public override void NPCLoot()
        {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<QuartzGem>(), Main.rand.Next(2, 3));
        }
    }
}*/