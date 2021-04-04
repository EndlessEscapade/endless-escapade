using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs
{
    public class Swordfish : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swordfish");
            Main.npcCatchable[npc.type] = true;
            Main.npcFrameCount[npc.type] = 1;
            //bannerItem = ModContent.ItemType<Items.Banners.LunaJellyBanner>();
        }

        public override void SetDefaults()
        {
            npc.aiStyle = 16;

            npc.friendly = false;

            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;

            npc.lifeMax = 76;

            npc.width = 80;
            npc.height = 20;

            npc.noGravity = true;

            npc.lavaImmune = false;
            npc.noTileCollide = false;
        }
    }
}