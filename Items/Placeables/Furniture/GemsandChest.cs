using EEMod.Tiles.Furniture;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Placeables.Furniture
{
    public class GemsandChest : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gemsand Chest");
        }


        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 28;
            item.value = 500;

            item.maxStack = 99;
            item.rare = ItemRarityID.Green;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 10;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

            item.createTile = ModContent.TileType<GemsandChestTile>();
        }
    }
}
