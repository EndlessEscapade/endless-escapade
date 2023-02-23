using System.Collections.Generic;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace EndlessEscapade.Content.NPCs.Shipyard;

[AutoloadHead]
public class SailorNPC : ModNPC
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
            Velocity = 1f, 
            Direction = 1
        };

        NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

        NPC.Happiness.SetBiomeAffection<OceanBiome>(AffectionLevel.Love);
        NPC.Happiness.SetNPCAffection(NPCID.Pirate, AffectionLevel.Hate);
        NPC.Happiness.SetNPCAffection(NPCID.Angler, AffectionLevel.Love);

        Main.npcFrameCount[Type] = 25;
    }

    public override void SetDefaults() {
        NPC.townNPC = true;
        NPC.friendly = true;
        NPC.width = 30;
        NPC.height = 50;
        NPC.damage = 10;
        NPC.defense = 15;
        NPC.lifeMax = 250;
        NPC.knockBackResist = 0.5f;
        AnimationType = NPCID.Guide;
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;
        NPC.aiStyle = NPCAIStyleID.Passive;
    }

    public override bool CanTownNPCSpawn(int numTownNPCs, int money) {
        for (int i = 0; i < Main.maxPlayers; i++) {
            Player player = Main.player[i];

            if (!player.active) {
                continue;
            }
        }

        return false;
    }

    public override bool PreAI() {
        if (NPC.CountNPCS(Type) > 1) {
            NPC.active = false;
            return false;
        }

        return true;
    }

    public override string GetChat() {
        WeightedRandom<string> randomChat = new();

        // TODO: Localization entries
        if (NPC.FindFirstNPC(NPCID.Angler) <= -1) {
            randomChat.Add("Have you seen my son anywhere? He went out on a fishing trip recently and hasn't come back.");
            randomChat.Add("My son knows the waves well, so I hope he's alright...");
            randomChat.Add("Have you seen my son? He wears a beige cap and a vest.");
            return randomChat;
        }

        if (Main.raining) {
            randomChat.Add("I hope this rain doesn't mean a hurricane's coming!");
            randomChat.Add("I lost my rain slicker in a windy day a few years ago. Wish I had another one.");
        }

        if (Main.dayTime) {
            randomChat.Add("I always love stepping out on the pier at the crack of dawn.");
            randomChat.Add("The ocean's so enticing today, don't ya think?");
            randomChat.Add("The sharks seem excited today.");
            return randomChat;
        }

        randomChat.Add("The ocean waves are always so calm at nighttime.");
        randomChat.Add("The moon looks so beautiful on the water.");
        randomChat.Add("I love the glow of the jellies.");

        if (Main.moonType == (int)MoonPhase.Empty) {
            randomChat.Add("The new moon is a sign that the jellyfish over the Coral Reefs are on the move.");
        }

        return randomChat;
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