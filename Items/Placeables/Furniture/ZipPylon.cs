using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Placeables.Furniture
{
    public class ZipPylon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zip-Pylon");
        }

        public override void SetDefaults()
        {
            item.width = 10;
            item.height = 10;
            item.rare = ItemRarityID.White;
            item.value = Item.sellPrice(0, 0, 0, 0);
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useAnimation = 15;
            item.useTime = 7;
            item.consumable = true;
            item.autoReuse = true;
            item.maxStack = 999;
            item.placeStyle = 10;
            item.createTile = ModContent.TileType<Tiles.Furniture.ZipPylon>();
        }
    }
}