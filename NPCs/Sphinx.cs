using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs
{
    public class Sphinx : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sphinx");
            //Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BlueSlime];
        }

        public override void SetDefaults()
        {

            npc.width = 816;
            npc.height = 508;
            npc.damage = 12;
            npc.defense = 0;
            npc.lifeMax = 90;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 100f;
            npc.knockBackResist = 0.3f;
            npc.alpha = 0;
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
