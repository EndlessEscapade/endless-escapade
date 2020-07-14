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
            npc.spriteDirection = 1;
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
                    return "Hey! There's my little moneymake--erm, intrepid treasure hunter!";
                case 1:
                    return "Linebe... who's that?";
                case 2:
                    return "Beed.. who's that?";
                case 3:
                    return "I've heard rumors of a mysterious wand that can control the weather... ever seen it?";
                default:
                    return "Tetra... who's that?";
            }
        }

        private bool onSpawn = true;
        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            if (onSpawn)
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
                switch (Main.rand.Next(3))
                {
                    case 0:
                        switch (Main.rand.Next(5))
                        {
                            case 0:
                                shop.item[nextSlot].SetDefaults(ItemID.PaintingAcorns);
                                break;
                            case 1:
                                shop.item[nextSlot].SetDefaults(ItemID.PaintingSnowfellas);
                                break;
                            case 2:
                                shop.item[nextSlot].SetDefaults(ItemID.PaintingCursedSaint);
                                break;
                            case 3:
                                shop.item[nextSlot].SetDefaults(ItemID.PaintingColdSnap);
                                break;
                            case 4:
                                shop.item[nextSlot].SetDefaults(ItemID.PaintingTheSeason);
                                break;
                        }
                        break;
                    case 1:
                        switch (Main.rand.Next(5))
                        {
                            case 0:
                                shop.item[nextSlot].SetDefaults(ItemID.GuidePicasso);
                                break;
                            case 1:
                                shop.item[nextSlot].SetDefaults(ItemID.Discover);
                                break;
                            case 2:
                                shop.item[nextSlot].SetDefaults(ItemID.TerrarianGothic);
                                break;
                            case 3:
                                shop.item[nextSlot].SetDefaults(ItemID.Waldo);
                                break;
                            case 4:
                                shop.item[nextSlot].SetDefaults(ItemID.Land);
                                break;
                        }
                        break;
                    case 2:
                        switch (Main.rand.Next(5))
                        {
                            case 0:
                                shop.item[nextSlot].SetDefaults(ItemID.BitterHarvest);
                                break;
                            case 1:
                                shop.item[nextSlot].SetDefaults(ItemID.BloodMoonCountess);
                                break;
                            case 2:
                                shop.item[nextSlot].SetDefaults(ItemID.JackingSkeletron);
                                break;
                            case 3:
                                shop.item[nextSlot].SetDefaults(ItemID.HallowsEve);
                                break;
                            case 4:
                                shop.item[nextSlot].SetDefaults(ItemID.MorbidCuriosity);
                                break;
                        }
                        break;
                }
                nextSlot++;
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
                if (Main.rand.Next(50) == 0 && NPC.downedBoss3)
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
                switch (Main.rand.Next(4))
                {
                    case 0:
                        shop.item[nextSlot].SetDefaults(ItemID.Pho);
                        break;
                    case 1:
                        shop.item[nextSlot].SetDefaults(ItemID.PadThai);
                        break;
                    case 2:
                        shop.item[nextSlot].SetDefaults(ItemID.CookedShrimp);
                        break;
                    case 3:
                        shop.item[nextSlot].SetDefaults(ItemID.Sashimi);
                        break;
                }
                nextSlot++;
                switch (Main.rand.Next(3))
                {
                    case 0:
                        shop.item[nextSlot].SetDefaults(ItemID.BlueDynastyShingles);
                        break;
                    case 1:
                        shop.item[nextSlot].SetDefaults(ItemID.RedDynastyShingles);
                        break;
                    case 2:
                        shop.item[nextSlot].SetDefaults(ItemID.DynastyWood);
                        break;
                }
                nextSlot++;
                onSpawn = false;
            }
        }

        public override void AI()
        {
            if (Main.LocalPlayer.talkNPC != -1)
            {

            }
            else
            {
                npc.position.X += 0.3f;
            }
        }
    }
}
