using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Items.Weapons.Mage;

namespace EEMod.NPCs.CoralReefs
{
    public class Clam : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // Calamity be like
            DisplayName.SetDefault("Clam");
            Main.npcFrameCount[npc.type] = 2;
        }
        public override void SetDefaults()
        {
            npc.aiStyle = -1;

            npc.lifeMax = 50;
            npc.damage = 13;
            npc.defense = 3;

            npc.width = 400;
            npc.width = 400;

            npc.knockBackResist = 0f;

            npc.npcSlots = 1f;
            npc.buffImmune[BuffID.Confused] = true;
            npc.lavaImmune = false;

            npc.value = Item.sellPrice(0, 0, 0, 75);
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.22f);
        }

        /* public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.);
        } */

        private const int State_Asleep = 0;
        private const int State_Notice = 1;
        private const int State_Attack = 2;
        private const int State_Fall = 3;

        public float AI_State { get => npc.ai[0]; set => npc.ai[0] = value; }
        public float AI_Timer { get => npc.ai[1]; set => npc.ai[1] = value; }
        public float AI_ClamTime { get => npc.ai[2]; set => npc.ai[2] = value; }

        public override void AI()
        {
            Player player = Main.player[npc.target];
            if (AI_State == State_Asleep)
            {
                npc.TargetClosest(true);

                if (npc.HasValidTarget && player.WithinRange(npc.Center, 256f))
                {
                    AI_State = State_Notice;
                    AI_Timer = 0;
                }
            }
            else if (AI_State == State_Notice)
            {
                if (player.WithinRange(npc.Center, 256f))
                {
                    AI_Timer++;
                    if (AI_Timer >= 20)
                    {
                        AI_State = State_Attack;
                        AI_Timer = 0;
                    }
                }
                else
                {
                    npc.TargetClosest(true);
                    if (!npc.HasValidTarget || !player.WithinRange(npc.Center, 256f))
                    {
                        AI_State = State_Asleep;
                        AI_Timer = 0;
                    }
                }
            }
            else if (AI_State == State_Attack)
            {
                AI_Timer += 1;
                if (AI_Timer == 1 && npc.velocity.Y == 0)
                {
                    npc.velocity = new Vector2(npc.direction * 2, -10f);
                }
                else if (AI_Timer > 40)
                {
                    AI_State = State_Fall;
                    AI_Timer = 0;
                }
            }
            else if (AI_State == State_Fall)
            {
                if (npc.velocity.Y == 0)
                {
                    npc.velocity.X = 0;
                    AI_State = State_Asleep;
                    AI_Timer = 0;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (AI_State == State_Asleep)
            {
                npc.frame.Y = 0;
            }
            else
            {
                npc.frame.Y = frameHeight;
                npc.spriteDirection = npc.direction;
            }
        }
    }
}
