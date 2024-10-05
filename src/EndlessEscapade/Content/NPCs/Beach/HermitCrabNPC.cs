namespace EndlessEscapade.Content.NPCs.Beach;

public class HermitCrabNPC : ModNPC
{
    public override void SetStaticDefaults() {
        base.SetStaticDefaults();

        Main.npcFrameCount[Type] = 4;

        NPCID.Sets.CountsAsCritter[Type] = true;
        NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
    }

    public override void SetDefaults() {
        base.SetDefaults();

        NPC.lifeMax = 5;
        NPC.defense = 5;

        NPC.width = 30;
        NPC.height = 20;

        NPC.aiStyle = NPCAIStyleID.Passive;
    }

    public override void FindFrame(int frameHeight) {
        base.FindFrame(frameHeight);

        NPC.spriteDirection = NPC.direction;

        NPC.frameCounter++;

        if ((NPC.velocity.X == 0f && NPC.frame.Y == 0) || NPC.frameCounter < 5f) {
            return;
        }

        NPC.frame.Y += frameHeight;
        NPC.frameCounter = 0f;

        if (NPC.frame.Y < Main.npcFrameCount[Type] * frameHeight) {
            return;
        }

        NPC.frame.Y = 0;
    }

    public override void HitEffect(NPC.HitInfo hit) {
        base.HitEffect(hit);

        var amount = NPC.life > 0 ? 5 : 10;

        for (var i = 0; i < amount; i++) {
            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood);
        }
    }
}
