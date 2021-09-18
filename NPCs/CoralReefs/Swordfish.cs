using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs
{
    public class Swordfish : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swordfish");
            Main.npcCatchable[NPC.type] = true;
            Main.npcFrameCount[NPC.type] = 1;
            //bannerItem = ModContent.ItemType<Items.Banners.LunaJellyBanner>();
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = 16;

            // NPC.friendly = false;

            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;

            NPC.lifeMax = 76;

            NPC.width = 80;
            NPC.height = 20;

            NPC.noGravity = true;

            // NPC.lavaImmune = false;
            // NPC.noTileCollide = false;
        }
    }
}