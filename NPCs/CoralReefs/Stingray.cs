using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs
{
    public class Stingray : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stingray");
            Main.npcFrameCount[NPC.type] = 3;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter == 7)
            {
                NPC.frame.Y = NPC.frame.Y + frameHeight;
                NPC.frameCounter = 0;
            }
            if (NPC.frame.Y >= frameHeight * 3)
            {
                NPC.frame.Y = 0;
                return;
            }
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = 16;

            NPC.lifeMax = 50;
            NPC.damage = 13;
            NPC.defense = 3;

            NPC.width = 52;
            NPC.height = 18;
            NPC.noGravity = false;
            NPC.knockBackResist = 0f;

            NPC.lavaImmune = false;
            banner = NPC.type;
            //bannerItem = ModContent.ItemType<Items.Banners.ClamBanner>();
            NPC.value = Item.sellPrice(0, 0, 0, 75);
        }
    }
}