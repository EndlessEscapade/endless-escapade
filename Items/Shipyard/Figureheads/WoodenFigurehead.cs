using EEMod.Seamap.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Shipyard.Figureheads
{
    public class WoodenFigurehead : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wooden Figurehead");
            Tooltip.SetDefault("");
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 59;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 0, 18, 0);
            Item.rare = ItemRarityID.Green;
            Item.consumable = false;
            Item.GetGlobalItem<EEGlobalItem>().Tag = (int)ItemTags.Figurehead;

        }
    }
}