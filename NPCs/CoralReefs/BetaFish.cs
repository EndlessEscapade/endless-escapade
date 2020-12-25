using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs
{
    public class BetaFish : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Beta");
            //bannerItem = ModContent.ItemType<Items.Banners.LunaJellyBanner>();
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;

            npc.friendly = true;

            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;

            npc.lifeMax = 5;

            npc.width = 34;
            npc.height = 24;

            npc.noGravity = true;

            npc.lavaImmune = false;
            npc.noTileCollide = false;
        }
    }
}