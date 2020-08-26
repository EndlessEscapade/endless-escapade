using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs
{
    public class CAHT : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("CAHT");
        }

        public override void SetDefaults()
        {
            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;

            npc.lifeMax = 38;
            npc.defense = 2;

            npc.buffImmune[BuffID.Confused] = true;

            npc.width = 26;
            npc.height = 26;

            npc.noGravity = true;

            npc.lavaImmune = false;
            npc.noTileCollide = false;
            //bannerItem = ModContent.ItemType<Items.Banners.ToxicPufferBanner>();
        }
    }
}