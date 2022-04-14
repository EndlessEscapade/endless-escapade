using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Placeables.Furniture
{
    public class ZipPylon : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zip-Pylon");
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(0, 0, 0, 0);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = 7;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.maxStack = 999;
            Item.placeStyle = 10;
            Item.createTile = ModContent.TileType<Tiles.Furniture.ZipPylon>();
        }
    }
}