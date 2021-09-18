using EEMod.Tiles.Furniture;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Tiles.Furniture.Chests;

namespace EEMod.Items.Placeables.Furniture
{
    public class CoralChest : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coral Chest");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 28;
            Item.value = 500;

            Item.maxStack = 99;
            Item.rare = ItemRarityID.Green;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.useAnimation = 15;

            Item.useTurn = true;
            Item.autoReuse = true;
            Item.consumable = true;

            Item.createTile = ModContent.TileType<CoralChestTile>();
        }
    }
}