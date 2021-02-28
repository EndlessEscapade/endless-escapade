using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using System;
using EEMod.Items.Placeables.Furniture;

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
            // DisplayName.SetDefault("Example Person");
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
            return "Skipper";
        }

        public override string GetChat()
        {
            int angler = NPC.FindFirstNPC(NPCID.Angler);
            if (angler >= 0)
            {
                return "I'm overjoyed to hear that " + Main.npc[angler].GivenName + " is safe! I thought I lost him when that storm hit...";
            }
            else
            {
                switch (Main.rand.Next(4))
                {
                    case 0:
                        return "";
                    case 1:
                        return "My son... lost to the waves, so long ago...";
                    case 2:
                        return "I wonder if my son's alive...";
                    case 3:
                        return "The sound of the ocean is really soothing. You should stay here for a while, enjoy the sound of the tides against the Shipyard.";
                    default:
                        return "You say you want to sail the seas? I'd give you my boat if I had the materials to repair it... and maybe some money too.";
                }
            }
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
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

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
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
        }
    }
}