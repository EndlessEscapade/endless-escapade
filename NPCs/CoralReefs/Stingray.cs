using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.CoralReefs
{
    public class Stingray : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stingray");
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter == 7)
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

            npc.lifeMax = 50;
            npc.damage = 13;
            npc.defense = 3;

            npc.width = 52;
            npc.height = 18;
            npc.noGravity = false;
            npc.knockBackResist = 0f;

            npc.lavaImmune = false;
            banner = npc.type;
            //bannerItem = ModContent.ItemType<Items.Banners.ClamBanner>();
            npc.value = Item.sellPrice(0, 0, 0, 75);
        }
    }
}