using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.NPCs.Components;

public sealed class NPCDeathEffects : NPCComponent
{
    public int GoreAmount { get; set; } = -1;

    public override void HitEffect(NPC npc, NPC.HitInfo hit) {
        if (!Enabled || npc.life > 0 || Main.netMode == NetmodeID.Server) {
            return;
        }

        if (GoreAmount > 0) {
            for (var i = 0; i < GoreAmount; i++) {
                Gore.NewGore(npc.GetSource_Death(), npc.position, npc.velocity, Mod.Find<ModGore>(npc.ModNPC.Name + i).Type);
            }
        }

        if (!npc.townNPC) {
            return;
        }

        var hat = npc.GetPartyHatGore();

        if (hat < 0) {
            return;
        }

        Gore.NewGore(npc.GetSource_Death(), npc.position, npc.velocity, hat);
    }
}
