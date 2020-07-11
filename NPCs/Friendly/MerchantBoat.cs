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
            npc.noGravity = true;
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
            switch (Main.rand.Next(6))
            {
                case 0:
                    shop.item[nextSlot].SetDefaults(ItemID.EnchantedSword);
                    nextSlot++;
                    break;
                case 1:
                    if (NPC.downedQueenBee)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.BeeKeeper);
                        nextSlot++;
                    }
                    break;
                case 2:
                    shop.item[nextSlot].SetDefaults(ItemID.Starfury);
                    nextSlot++;
                    break;
                case 3:
                    if (NPC.downedPlantBoss)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.Seedler);
                        nextSlot++;
                    }
                    break;
                case 4:
                    if (NPC.downedHalloweenKing)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.TheHorsemansBlade);
                        nextSlot++;
                    }
                    break;
                case 5:
                    if (NPC.downedMartians)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.InfluxWaver);
                        nextSlot++;
                    }
                    break;
                case 6:
                    if (Main.rand.Next(10) == 0)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.Arkhalis);
                        nextSlot++;
                    }
                    break;
            }
            switch (Main.rand.Next(3))
            {
                case 0:
                    if (NPC.downedPlantBoss)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.WispinaBottle);
                        nextSlot++;
                    }
                    break;
                case 1:
                    if (NPC.downedBoss2)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.DD2PetGhost);
                        nextSlot++;
                    }
                    break;
                case 2:
                    shop.item[nextSlot].SetDefaults(ItemID.MagicLantern);
                    nextSlot++;
                    break;
            }
            switch (Main.rand.Next(19))
            {
                case 0:
                    shop.item[nextSlot].SetDefaults(ItemID.AmberMosquito);
                    nextSlot++;
                    break;
                case 1:
                    if (NPC.downedChristmasIceQueen)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.BabyGrinchMischiefWhistle);
                        nextSlot++;
                    }
                    break;
                case 2:
                    if (NPC.downedQueenBee)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.Nectar);
                        nextSlot++;
                    }
                    break;
                case 3:
                    shop.item[nextSlot].SetDefaults(ItemID.Fish);
                    nextSlot++;
                    break;
                case 4:
                    if (Main.hardMode)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.ToySled);
                        nextSlot++;
                    }
                    break;
                case 5:
                    if (Main.hardMode)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.StrangeGlowingMushroom);
                        nextSlot++;
                    }
                    break;
                case 6:
                    shop.item[nextSlot].SetDefaults(ItemID.Carrot);
                    nextSlot++;
                    break;
                case 7:
                    if (NPC.downedHalloweenTree)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.CursedSapling);
                        nextSlot++;
                    }
                    break;
                case 8:
                    if (NPC.downedPlantBoss)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.EyeSpring);
                        nextSlot++;
                    }
                    break;
                case 9:
                    if (NPC.downedBoss2)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.DD2PetDragon);
                        nextSlot++;
                    }
                    break;
                case 10:
                    if (NPC.downedGolemBoss)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.LizardEgg);
                        nextSlot++;
                    }
                    break;
                case 11:
                    shop.item[nextSlot].SetDefaults(ItemID.TartarSauce);
                    break;
                case 12:
                    if (Main.hardMode)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.ParrotCracker);
                        nextSlot++;
                    }
                    break;
                case 13:
                    if (NPC.downedBoss2)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.DD2PetGato);
                        nextSlot++;
                    }
                    break;
                case 14:
                    if (NPC.downedPlantBoss)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.Seedling);
                        nextSlot++;
                    }
                    break;
                case 15:
                    if (NPC.downedPlantBoss)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.SpiderEgg);
                        nextSlot++;
                    }
                    break;
                case 16:
                    shop.item[nextSlot].SetDefaults(ItemID.MagicalPumpkinSeed);
                    nextSlot++;
                    break;
                case 17:
                    shop.item[nextSlot].SetDefaults(ItemID.Seaweed);
                    nextSlot++;
                    break;
                case 18:
                    shop.item[nextSlot].SetDefaults(ItemID.ZephyrFish);
                    nextSlot++;
                    break;
            }
            if (Main.rand.Next(10) == 0)
            {
                shop.item[nextSlot].SetDefaults(ItemID.SlimeStaff);
                nextSlot++;
            }
            if (Main.rand.Next(25) == 0 && NPC.downedBoss3)
            {
                shop.item[nextSlot].SetDefaults(ItemID.BoneKey);
                nextSlot++;
            }
            if (Main.rand.Next(5) == 0 && NPC.downedBoss3)
            {
                shop.item[nextSlot].SetDefaults(ItemID.LavaCharm);
                nextSlot++;
            }
            if (Main.rand.Next(3) == 0 && NPC.downedBoss1)
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
            if (Main.LocalPlayer.talkNPC)
            {

            }
        }
    }
}
