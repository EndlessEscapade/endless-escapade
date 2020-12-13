using EEMod.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Tiles.Furniture;

namespace EEMod.Items.Placeables.Furniture
{
    public class SimpleSail : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Simple Sail");
        }

        public override void SetDefaults()
        {
            item.width = 10;
            item.height = 10;
            item.rare = ItemRarityID.White;
            item.value = Item.sellPrice(0, 0, 0, 0);
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useAnimation = 20;
            item.useTime = 15;
            item.consumable = true;
            item.autoReuse = true;
            item.maxStack = 999;
            item.placeStyle = 10;
            item.useTurn = true;
            item.createTile = ModContent.TileType<SimpleSailTile>();
        }
    }
}