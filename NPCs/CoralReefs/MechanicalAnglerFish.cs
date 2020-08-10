using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs
{
    public class MechanicalAnglerFish : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mechanical Angler Fish");
            Main.npcFrameCount[npc.type] = 3;
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

        public override void AI()
        {
            base.AI();
        }
    }
}