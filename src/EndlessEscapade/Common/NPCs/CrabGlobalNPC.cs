using EndlessEscapade.Content.Items.Beach;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.NPCs;

public sealed class CrabGlobalNPC : GlobalNPC
{
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
        return entity.type == NPCID.Crab;
    }

    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
        base.ModifyNPCLoot(npc, npcLoot);

        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CrabPincersItem>(), 20));
    }
}
