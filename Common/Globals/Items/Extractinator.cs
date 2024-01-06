using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Globals.Items;

public sealed class Extractinator : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
        return entity.type == ItemID.Extractinator || entity.type == ItemID.ChlorophyteExtractinator;
    }

    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
        foreach (var tooltip in tooltips) {
            if (tooltip.Name == "Tooltip0" && tooltip.Mod == "Terraria") {
                tooltip.Text = Language.GetTextValue($"Mods.{nameof(EndlessEscapade)}.Tooltips.Extractinator");
            }
        }
    }
}
