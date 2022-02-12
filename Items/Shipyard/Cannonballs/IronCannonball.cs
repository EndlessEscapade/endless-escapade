using EEMod.Seamap.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Shipyard.Cannonballs
{
    public class IronCannonball : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Iron Cannonball");
            Tooltip.SetDefault("Use this to make your boat shoot cannonballs.");
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 59;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.value = Item.buyPrice(0, 0, 18, 0);
            Item.rare = ItemRarityID.Green;
            Item.GetGlobalItem<EEGlobalItem>().Tag = (int)ItemTags.Cannonball;
        }
    }
}