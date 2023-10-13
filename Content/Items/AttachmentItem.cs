using System.Collections.Generic;
using EndlessEscapade.Utilities.Extensions;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items;

public abstract class AttachmentItem : ModItem
{
    public override void SetDefaults() {
        Item.width = 20;
        Item.height = 20;

        Item.rare = ItemRarityID.White;
    }

    public sealed override void ModifyTooltips(List<TooltipLine> tooltips) {
        var index = tooltips.FindIndex(x => x.Name == "ItemName" && x.Mod == "Terraria");
        var line = new TooltipLine(Mod, $"{nameof(EndlessEscapade)}:AttachmentItem", Mod.GetLocalizationValue("Common.AttachmentInfo")) { OverrideColor = Color.Goldenrod };

        if (index == -1) {
            tooltips.Add(line);
            return;
        }

        tooltips.Insert(index + 1, line);
    }
}
