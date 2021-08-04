using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Tiles.Furniture;

namespace EEMod.Items.Materials
{
    public class Kelpberries : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kelpberries");
            Tooltip.SetDefault("Super bright!");
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 59; // influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 28;
            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(0, 0, 0, 0);
            Item.useStyle = ItemUseStyleID.SwingThrow;
            Item.useAnimation = 20;
            Item.useTime = 15;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.maxStack = 999;
            Item.placeStyle = 10;
            Item.createTile = ModContent.TileType<KelpberryPlaced>();
        }
    }
}