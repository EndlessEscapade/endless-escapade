using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using System;
using EEMod.Items.Placeables.Furniture;
using System.Collections.Generic;
using Terraria.Utilities;

namespace EEMod.NPCs.Friendly
{
    [AutoloadHead]
    public class Sailor : ModNPC
    {
        public override string Texture => "EEMod/NPCs/Friendly/Sailor";

        public override bool Autoload(ref string name)
        {
            name = "Sailor";
            return mod.Properties.Autoload;
        }

        public override void SetStaticDefaults()
        {
            // DisplayName automatically assigned from .lang files, but the commented line below is the normal approach.
            DisplayName.SetDefault("Sailor");
            //Main.npcFrameCount[npc.type] = 25;
            //NPCID.Sets.ExtraFramesCount[npc.type] = 9;
            //NPCID.Sets.AttackFrameCount[npc.type] = 4;
            NPCID.Sets.DangerDetectRange[npc.type] = 700;
            NPCID.Sets.AttackType[npc.type] = 0;
            NPCID.Sets.AttackTime[npc.type] = 90;
            NPCID.Sets.AttackAverageChance[npc.type] = 30;
            NPCID.Sets.HatOffsetY[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.townNPC = true;
            npc.friendly = true;
            npc.width = 18;
            npc.height = 40;
            npc.aiStyle = 7;
            npc.damage = 10;
            npc.defense = 15;
            npc.lifeMax = 250;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0.5f;
            //animationType = NPCID.Guide;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            for (int k = 0; k < 255; k++)
            {
                Player player = Main.player[k];
                if (!player.active)
                {
                    continue;
                }
            }
            return false;
        }

        public override string TownNPCName()
        {
            switch (WorldGen.genRand.Next(8))
            {
                case 0:
                    return "James";
                case 1:
                    return "Peter";
                case 2:
                    return "Francis";
                case 3:
                    return "John";
                case 4:
                    return "Ferdinand";
                case 5:
                    return "Herman";
                case 6:
                    return "Christopher";
                case 7:
                    return "Jack";
                default:
                    return "Popeye";
            }
        }

        public override string GetChat()
        {
            WeightedRandom<string> chat = new WeightedRandom<string>();

            int steampunker = NPC.FindFirstNPC(NPCID.Steampunker);
            if (steampunker >= 0)
            {
                chat.Add("Me and " + Main.npc[steampunker].GivenName + " have a lot in common - we both love ships!");
            }

            int pirate = NPC.FindFirstNPC(NPCID.Pirate);
            if (pirate >= 0)
            {
                chat.Add(Main.npc[pirate].GivenName  + "? Oh, he's an old rival.");
            }

            if(Main.dayTime)
            {
                chat.Add("I always love stepping out on the pier at the crack of dawn.");
                chat.Add("The ocean's so enticing today, don't ya think?");
                chat.Add("The sharks seem excited today.");
            }
            else
            {
                chat.Add("The ocean waves are always so calm at nighttime.");
                chat.Add("The moon looks so beautiful on the water.");
                chat.Add("I love the glow of the jellies.");

                if(Main.moonType == 4)
                    chat.Add("The new moon is a sign that the jellyfish over the Coral Reefs are on the move.");
            }

            if (Main.raining)
            {
                chat.Add("I hope this rain doesn't mean a hurricane's coming!");
                chat.Add("I lost my rain slicker in a windy day a few years ago. Wish I had another one.");
            }

            return chat;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
            button2 = "Ship";
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop)
        {
            if (firstButton)
            {
                shop = true;
            }
            else
            {
                Main.playerInventory = true;
                // remove the chat window...
                Main.npcChatText = $"The old ship sitting broken in the Shipyard used to be my old vessel. I'm past my days of sailing, but you look like you want to see the seven seas. If you brought me [c/E4A214:{"150 Wood"}] [i:{ItemID.Wood}] and [c/E4A214:{"20 Silk"}] [i:{ItemID.Silk}] along with a solid payment of [c/E4A214:{"5 gold coins"}], I'd get her fixed right up for you.";
            }
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<WoodenShipsWheel>());
            nextSlot++;
        }

        public override void NPCLoot()
        {
            //Item.NewItem(npc.getRect(), ModContent.ItemType<>());

            //Sailor's Hat?
        }

        // Make this Town NPC teleport to the King and/or Queen statue when triggered.
        public override bool CanGoToStatue(bool toKingStatue)
        {
            return true;
        }

        /*public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 20;
            knockback = 4f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 30;
            randExtraCooldown = 30;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ProjectileID.Anchor;
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 2f;
        }*/
    }
}