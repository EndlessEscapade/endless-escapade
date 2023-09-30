using System.Collections.Generic;
using EndlessEscapade.Content.Tiles.Shipyard;
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

    public override void ModifyTooltips(List<TooltipLine> tooltips) {
        var nameIndex = tooltips.FindIndex(x => x.Name == "ItemName" && x.Mod == "Terraria");
        var attachmentLine = new TooltipLine(Mod, "EndlessEscapade:AttachmentItem", "Boat Attachment") {
            OverrideColor = Color.Goldenrod
        };

        if (nameIndex == -1) {
            tooltips.Add(attachmentLine);
            return;
        }

        tooltips.Insert(nameIndex + 1, attachmentLine);
    }
}
