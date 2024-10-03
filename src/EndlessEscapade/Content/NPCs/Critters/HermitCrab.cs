using EndlessEscapade.Common.NPCs;
using EndlessEscapade.Common.NPCs.Components;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.NPCs.Critters;

public class HermitCrab : ModNPC
{
    public override void SetStaticDefaults() {
        Main.npcFrameCount[Type] = 4;

        NPCID.Sets.CountsAsCritter[Type] = true;
        NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
    }

    public override void SetDefaults() {
        NPC.lifeMax = 5;
        NPC.defense = 5;

        NPC.width = 30;
        NPC.height = 20;

        NPC.aiStyle = NPCAIStyleID.Passive;

        NPC.TryEnableComponent<NPCDeathEffects>(c => c.GoreAmount = 3);
    }

    public override void FindFrame(int frameHeight) {
        NPC.spriteDirection = NPC.direction;

        if ((NPC.velocity.X == 0f && NPC.frame.Y == 0) || NPC.frameCounter++ < 5) {
            return;
        }

        NPC.frame.Y += frameHeight;
        NPC.frameCounter = 0;

        if (NPC.frame.Y < Main.npcFrameCount[Type] * frameHeight) {
            return;
        }

        NPC.frame.Y = 0;
    }

    public override void HitEffect(NPC.HitInfo hit) {
        var amount = NPC.life > 0 ? 5 : 10;

        for (var i = 0; i < amount; i++) {
            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood);
        }
    }
}
