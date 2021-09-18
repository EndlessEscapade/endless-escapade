using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace EEMod.NPCs.CoralReefs
{
    public class OlvinicSnail : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Olvinic Snail");
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return (spawnInfo.water ? 0 : 0.05f);
        }

        private int frameNumber = 0;
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter == 6)
            {
                NPC.frame.Y = NPC.frame.Y + frameHeight;
                NPC.frameCounter = 0;
            }
            if (NPC.frame.Y >= frameHeight * 2)
            {
                NPC.frame.Y = 0;
                return;
            }
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;

            NPC.lifeMax = 550;
            NPC.defense = 10;

            NPC.width = 58;
            NPC.height = 28;

            NPC.aiStyle = 67;

            NPC.noGravity = true;

            NPC.buffImmune[BuffID.Confused] = true;

            // NPC.lavaImmune = false;
            // NPC.noTileCollide = false;
            //bannerItem = ModContent.ItemType<Items.Banners.GiantSquidBanner>();
        }

        public override void AI()
        {
            
        }
    }
}