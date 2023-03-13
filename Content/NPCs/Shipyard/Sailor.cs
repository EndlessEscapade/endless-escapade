using System.Collections.Generic;
using EndlessEscapade.Common.Structures;
using EndlessEscapade.Content.Items;
using EndlessEscapade.Utilities.Extensions;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace EndlessEscapade.Content.NPCs.Shipyard;

[AutoloadHead]
public class Sailor : ModNPC
{
    private static int lastShipDialogue;
    
    public override void SetStaticDefaults() {
        NPCID.Sets.ExtraFramesCount[Type] = 9;
        NPCID.Sets.AttackFrameCount[Type] = 4;
        NPCID.Sets.DangerDetectRange[Type] = 700;
        NPCID.Sets.AttackType[Type] = 0;
        NPCID.Sets.AttackTime[Type] = 90;
        NPCID.Sets.AttackAverageChance[Type] = 30;
        NPCID.Sets.HatOffsetY[Type] = 4;

        NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new(0) {
            Velocity = 1f
        };

        NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

        NPC.Happiness.SetNPCAffection(NPCID.Pirate, AffectionLevel.Hate);
        NPC.Happiness.SetNPCAffection(NPCID.Angler, AffectionLevel.Love);
        NPC.Happiness.SetNPCAffection(NPCID.DyeTrader, AffectionLevel.Like);
        NPC.Happiness.SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Dislike);

        NPC.Happiness.SetBiomeAffection<OceanBiome>(AffectionLevel.Love);
        NPC.Happiness.SetBiomeAffection<DesertBiome>(AffectionLevel.Like);
        NPC.Happiness.SetBiomeAffection<UndergroundBiome>(AffectionLevel.Hate);

        Main.npcFrameCount[Type] = 25;
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
        bestiaryEntry.Info.AddRange(
            new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
                new FlavorTextBestiaryInfoElement(Mod.GetTextValue("Bestiary.Sailor"))
            }
        );
    }

    public override void SetDefaults() {
        NPC.townNPC = true;
        NPC.friendly = true;
        NPC.width = 30;
        NPC.height = 50;
        NPC.damage = 10;
        NPC.defense = 15;
        NPC.lifeMax = 500;
        NPC.knockBackResist = 0.5f;
        AnimationType = NPCID.Guide;
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;
        NPC.aiStyle = NPCAIStyleID.Passive;
    }
    
    public override bool PreAI() {
        bool moreThanOne = NPC.CountNPCS(Type) > 1;

        if (moreThanOne) {
            NPC.active = false;
        }

        return moreThanOne;
    }

    public override void SetupShop(Chest shop, ref int nextSlot) {
        shop.item[nextSlot].SetDefaults(ModContent.ItemType<FishermansLog>());
        shop.item[nextSlot].value = Item.buyPrice(gold: 1);
        nextSlot++;
    }

    public override void SetChatButtons(ref string button, ref string button2) {
        button = Language.GetTextValue("LegacyInterface.28");
        button2 = Mod.GetTextValue("TownNPCButton.Sailor.SailingButton");
    }

    public override void OnChatButtonClicked(bool firstButton, ref bool shop) {
        if (firstButton) {
            shop = true;
            return;
        }

        Player player = Main.LocalPlayer;

        bool hasMaterials = player.HasItemStack(ItemID.Wood, 150) && player.HasItemStack(ItemID.Silk, 20) && player.HasItemStack(ItemID.GoldCoin, 5);

        if (hasMaterials) {
            if (!SailboatSystem.HasFixedBoat) {
                Main.npcChatText = Mod.GetTextValue("TownNPCDialogue.Sailor.ShipFixedDialogue");
                
                SailboatSystem.FixBrokenBoat();
                return;
            }

            // Change to wood steering wheel, once it's added.
            Main.npcChatText = Mod.GetTextValue($"TownNPCDialogue.Sailor.ShipCommonDialogue{lastShipDialogue}", ModContent.ItemType<FishermansLog>());
        }
        else {
            Main.npcChatText = Mod.GetTextValue($"TownNPCDialogue.Sailor.ShipRepairDialogue{lastShipDialogue}");
        }

        lastShipDialogue = lastShipDialogue == 0 ? 1 : 0;
    }

    public override string GetChat() {
        var chat = new WeightedRandom<string>();

        if (!NPC.AnyNPCs(NPCID.Angler)) {
            chat.AddLocalizationRange(Mod, "TownNPCDialogue.Sailor.AnglerDialogue", 3);
            return chat;
        }

        if (Main.raining) {
            chat.AddLocalizationRange(Mod, "TownNPCDialogue.Sailor.RainDialogue", 2);
        }

        if (Main.dayTime) {
            chat.AddLocalizationRange(Mod, "TownNPCDialogue.Sailor.DayDialogue", 3);
            return chat;
        }
        
        chat.AddLocalizationRange(Mod, "TownNPCDialogue.Sailor.NightDialogue", 3);

        if (Main.moonType == (int)MoonPhase.Empty) {
            chat.Add(Mod.GetTextValue("TownNPCDialogue.Sailor.NewMoonDialogue"));
        }

        return chat.Get();
    }

    public override List<string> SetNPCNameList() {
        return new List<string> {
            "Skipper"
        };
    }

    public override void TownNPCAttackStrength(ref int damage, ref float knockback) {
        damage = 20;
        knockback = 4f;
    }

    public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown) {
        cooldown = 30;
        randExtraCooldown = 30;
    }

    public override void TownNPCAttackProj(ref int projType, ref int attackDelay) {
        projType = ProjectileID.Anchor;
        attackDelay = 1;
    }

    public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset) {
        multiplier = 12f;
        randomOffset = 2f;
    }

    public override bool CanTownNPCSpawn(int numTownNPCs, int money) {
        return true;
    }

    public override bool CanGoToStatue(bool toKingStatue) {
        return true;
    }
}