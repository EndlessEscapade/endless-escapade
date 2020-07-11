using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Items.Placeables.Paintings;
using Terraria.Localization;

namespace EEMod.NPCs.Friendly
{
    public class MerchantBoat : ModNPC
    {
        public override void SetDefaults()
        {
            npc.townNPC = true;
            npc.width = 32;
            npc.height = 32;
            npc.friendly = true;
            npc.lifeMax = 2000;
            npc.aiStyle = 0;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
                shop = true;
        }

        public override string GetChat()
        {
            switch (Main.rand.Next(4))
            {
                case 0:
                    return "Sometimes I feel like I'm different from everyone else here.";
                case 1:
                    return "What's your favorite color? My favorite colors are white and black.";
                case 2:
                    return "ae";
                case 3:
                    return "I sell wares from places that don't even exist!";
                default:
                    return "What? I don't have any arms or legs? Oh, don't be ridiculous!";
            }
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            switch (Main.rand.Next(2))
            {
                case 0:
                    shop.item[nextSlot].SetDefaults(ModContent.ItemType<OSPainting>());
                    break;
                case 1:
                    shop.item[nextSlot].SetDefaults(ModContent.ItemType<Moon>());
                    break;
            }
            nextSlot++;
            if (Main.rand.Next(3) == 0)
            {
                switch (Main.rand.Next(23))
                {
                    case 0:
                        shop.item[nextSlot].SetDefaults(ItemID.AmberMosquito);
                        break;
                    case 1:
                        shop.item[nextSlot].SetDefaults(ItemID.GoodieBag);
                        break;
                }
            }
            nextSlot++;
            if (Main.rand.Next(20) == 0)
            {
                shop.item[nextSlot].SetDefaults(ItemID.SlimeStaff);
                nextSlot++;
            }
            if (Main.rand.Next(50) == 0 && NPC.downedBoss3)
            {
                shop.item[nextSlot].SetDefaults(ItemID.BoneKey);
                nextSlot++;
            }
            if (Main.rand.Next(10) == 0 && NPC.downedBoss3)
            {
                shop.item[nextSlot].SetDefaults(ItemID.LavaCharm);
                nextSlot++;
            }
            if (Main.rand.Next(5) == 0 && NPC.downedBoss1)
            {
                switch (Main.rand.Next(3))
                {
                    case 0:
                        shop.item[nextSlot].SetDefaults(ItemID.Sextant);
                        break;
                    case 1:
                        shop.item[nextSlot].SetDefaults(ItemID.FishermansGuide);
                        break;
                    case 2:
                        shop.item[nextSlot].SetDefaults(ItemID.WeatherRadio);
                        break;
                }
                nextSlot++;
            }
        }

        public override void AI()
        {
            
        }
    }
}
