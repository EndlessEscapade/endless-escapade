using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace EEMod.NPCs.CoralReefs
{
    public class StripedBass : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Striped Bass");
            Main.npcFrameCount[npc.type] = 2;
        }

        private int frameNumber = 0;
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter == 6)
            {
                npc.frame.Y = npc.frame.Y + frameHeight;
                npc.frameCounter = 0;
            }
            if (npc.frame.Y >= frameHeight * 2)
            {
                npc.frame.Y = 0;
                return;
            }
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;

            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;

            npc.lifeMax = 550;
            npc.defense = 10;

            npc.width = 58;
            npc.height = 28;

            npc.noGravity = true;

            npc.buffImmune[BuffID.Confused] = true;

            npc.lavaImmune = false;
            npc.noTileCollide = false;
            //bannerItem = ModContent.ItemType<Items.Banners.GiantSquidBanner>();
        }

        public override void AI()
        {
            
        }
    }
}