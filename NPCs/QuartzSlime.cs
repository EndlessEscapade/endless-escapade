using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs
{
    public class QuartzSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Quartz Slime");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BlueSlime];
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedBoss1 == false)
            {
                return SpawnCondition.Underground.Chance * 0.5f;
            }
            else
            {
                return SpawnCondition.Underground.Chance * 0.1f;
            }
        }
        public override void SetDefaults()
        {

            npc.width = 50;
            npc.height = 50;
            npc.damage = 12;
            npc.defense = 0;
            npc.lifeMax = 90;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 100f;
            npc.knockBackResist = 0.3f;
            npc.aiStyle = 1;
            npc.alpha = 60;
            aiType = NPCID.BlueSlime;
            animationType = NPCID.BlueSlime;
        }
        public override void FindFrame(int frameHeight)
        {

        }
        public override void NPCLoot()
        {
            if (Main.rand.Next(0) == 0)
            {
                 // this is still pretty useless to do
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("QuartzGem"), Main.rand.Next(1,3));
            }

        }
    }
}
