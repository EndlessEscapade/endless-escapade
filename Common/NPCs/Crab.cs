using EndlessEscapade.Content.Items.Weapons.Summon;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.NPCs;

public sealed class Crab : GlobalNPC
{
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
        return entity.type == NPCID.Crab;
    }

    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CrabPincers>(), 20));
    }
}
