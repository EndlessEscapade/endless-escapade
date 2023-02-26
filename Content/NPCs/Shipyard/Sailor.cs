using System.Collections.Generic;
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

        NPC.Happiness.SetBiomeAffection<OceanBiome>(AffectionLevel.Love);
        NPC.Happiness.SetNPCAffection(NPCID.Pirate, AffectionLevel.Hate);
        NPC.Happiness.SetNPCAffection(NPCID.Angler, AffectionLevel.Love);

        Main.npcFrameCount[Type] = 25;
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
        bestiaryEntry.Info.AddRange(
            new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
                new FlavorTextBestiaryInfoElement($"Mods.{nameof(EndlessEscapade)}.Bestiary.Sailor")
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

    public override bool CanTownNPCSpawn(int numTownNPCs, int money) {
        return true;
    }

    public override bool PreAI() {
        if (NPC.CountNPCS(Type) > 1) {
            NPC.active = false;
            return false;
        }

        return true;
    }

    public override string GetChat() {
        WeightedRandom<string> chat = new();

        if (!NPC.AnyNPCs(NPCID.Angler)) {
            chat.Add(Language.GetTextValue($"Mods.{nameof(EndlessEscapade)}.Dialogue.Sailor.AnglerDialogue1"));
            chat.Add(Language.GetTextValue($"Mods.{nameof(EndlessEscapade)}.Dialogue.Sailor.AnglerDialogue2"));
            chat.Add(Language.GetTextValue($"Mods.{nameof(EndlessEscapade)}.Dialogue.Sailor.AnglerDialogue3"));
            return chat;
        }

        if (Main.raining) {
            chat.Add(Language.GetTextValue($"Mods.{nameof(EndlessEscapade)}.Dialogue.Sailor.RainDialogue1"));
            chat.Add(Language.GetTextValue($"Mods.{nameof(EndlessEscapade)}.Dialogue.Sailor.RainDialogue2"));
        }

        if (Main.dayTime) {
            chat.Add(Language.GetTextValue($"Mods.{nameof(EndlessEscapade)}.Dialogue.Sailor.DayDialogue1"));
            chat.Add(Language.GetTextValue($"Mods.{nameof(EndlessEscapade)}.Dialogue.Sailor.DayDialogue2"));
            chat.Add(Language.GetTextValue($"Mods.{nameof(EndlessEscapade)}.Dialogue.Sailor.DayDialogue3"));
            return chat;
        }

        chat.Add(Language.GetTextValue($"Mods.{nameof(EndlessEscapade)}.Dialogue.Sailor.NightDialogue1"));
        chat.Add(Language.GetTextValue($"Mods.{nameof(EndlessEscapade)}.Dialogue.Sailor.NightDialogue2"));
        chat.Add(Language.GetTextValue($"Mods.{nameof(EndlessEscapade)}.Dialogue.Sailor.NightDialogue3"));

        if (Main.moonType == (int)MoonPhase.Empty) {
            chat.Add(Language.GetTextValue($"Mods.{nameof(EndlessEscapade)}.Dialogue.Sailor.NewMoonDialogue"));
        }

        return chat.ToString();
    }

    public override List<string> SetNPCNameList() {
        return new List<string> {
            "Skipper"
        };
    }

    public override bool CanGoToStatue(bool toKingStatue) {
        return true;
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
}