using System.Collections.Generic;
using EndlessEscapade.Common.Dialogues;
using EndlessEscapade.Common.NPCs;
using EndlessEscapade.Common.NPCs.Components;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace EndlessEscapade.Content.NPCs.Town;

[AutoloadHead]
public class Sailor : ModNPC
{
    public override void SetStaticDefaults() {
        Main.npcFrameCount[Type] = 25;

        NPCID.Sets.ExtraFramesCount[Type] = 9;
        NPCID.Sets.AttackFrameCount[Type] = 4;
        NPCID.Sets.DangerDetectRange[Type] = 700;
        NPCID.Sets.AttackType[Type] = 0;
        NPCID.Sets.AttackTime[Type] = 90;
        NPCID.Sets.AttackAverageChance[Type] = 30;
        NPCID.Sets.HatOffsetY[Type] = 4;
        NPCID.Sets.ShimmerTownTransform[Type] = false;

        NPCID.Sets.NPCBestiaryDrawOffset.Add(
            Type,
            new NPCID.Sets.NPCBestiaryDrawModifiers {
                Velocity = 1f
            }
        );

        NPC.Happiness.SetNPCAffection(NPCID.Angler, AffectionLevel.Love)
            .SetNPCAffection(NPCID.DyeTrader, AffectionLevel.Like)
            .SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Dislike)
            .SetNPCAffection(NPCID.Pirate, AffectionLevel.Hate);

        NPC.Happiness.SetBiomeAffection<OceanBiome>(AffectionLevel.Love)
            .SetBiomeAffection<DesertBiome>(AffectionLevel.Like)
            .SetBiomeAffection<UndergroundBiome>(AffectionLevel.Hate);
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
        bestiaryEntry.Info.AddRange(
            new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
                new FlavorTextBestiaryInfoElement("Mods.EndlessEscapade.Bestiary.Sailor")
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
        NPC.knockBackResist = 0.8f;

        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;
        NPC.aiStyle = NPCAIStyleID.Passive;

        AnimationType = NPCID.Guide;

        NPC.TryEnableComponent<NPCDeathEffects>(c => c.GoreAmount = 4);
    }

    public override void AI() {
        if (NPC.CountNPCS(Type) <= 1) {
            return;
        }

        NPC.active = false;
    }

    public override void HitEffect(NPC.HitInfo hit) {
        var amount = NPC.life > 0 ? 5 : 20;

        for (var i = 0; i < amount; i++) {
            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood);
        }
    }

    public override string GetChat() {
        var chat = new WeightedRandom<string>();

        // TODO: Maybe AddRange and GetLocalizedValueRange extensions for convenience.
        if (!NPC.AnyNPCs(NPCID.Angler)) {
            chat.Add(this.GetLocalizedValue("Chat.AnglerDialogue0"));
            chat.Add(this.GetLocalizedValue("Chat.AnglerDialogue1"));
            chat.Add(this.GetLocalizedValue("Chat.AnglerDialogue2"));

            return chat.Get();
        }

        if (Main.dayTime) {
            chat.Add(this.GetLocalizedValue("Chat.DayDialogue0"));
            chat.Add(this.GetLocalizedValue("Chat.DayDialogue1"));
            chat.Add(this.GetLocalizedValue("Chat.DayDialogue2"));
        }
        else {
            if (Main.moonType == (int)MoonPhase.Empty) {
                chat.Add(this.GetLocalizedValue("Chat.NewMoonDialogue"));
            }

            chat.Add(this.GetLocalizedValue("Chat.NightDialogue0"));
            chat.Add(this.GetLocalizedValue("Chat.NightDialogue1"));
            chat.Add(this.GetLocalizedValue("Chat.NightDialogue2"));
        }

        if (Main.raining) {
            chat.Add(this.GetLocalizedValue("Chat.RainDialogue0"));
            chat.Add(this.GetLocalizedValue("Chat.RainDialogue1"));
        }

        return chat.Get();
    }

    public override void SetChatButtons(ref string button, ref string button2) {
        button = Language.GetTextValue("LegacyInterface.28");
    }

    public override List<string> SetNPCNameList() {
        return new List<string> {
            "Skipper"
        };
    }

    public override void OnChatButtonClicked(bool firstButton, ref string shopName) {
        if (!firstButton) {
            return;
        }

        shopName = "Shop";
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

    public override bool CanTownNPCSpawn(int numTownNPCs) {
        return true;
    }

    public override bool CanGoToStatue(bool toKingStatue) {
        return true;
    }
}
