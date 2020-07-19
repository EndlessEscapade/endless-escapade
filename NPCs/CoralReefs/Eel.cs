using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs
{
    public class Eel : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eel");
            Main.npcFrameCount[npc.type] = 3;
        }

        // private int frameNumber = 0;
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter == 6)
            {
                npc.frame.Y = npc.frame.Y + frameHeight;
                npc.frameCounter = 0;
            }
            if (npc.frame.Y >= frameHeight * 3)
            {
                npc.frame.Y = 0;
                return;
            }
        }

        public override void SetDefaults()
        {
            npc.aiStyle = 16;

            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;

            npc.alpha = 20;

            npc.lifeMax = 550;
            npc.defense = 10;

            npc.width = 34;
            npc.height = 134;

            npc.noGravity = true;

            npc.buffImmune[BuffID.Confused] = true;

            npc.lavaImmune = false;
            npc.noTileCollide = false;
            //bannerItem = ModContent.ItemType<Items.Banners.GiantSquidBanner>();
        }
    }
}