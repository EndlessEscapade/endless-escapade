using EEMod.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Tiles.Furniture;

namespace EEMod.Items.Placeables.Furniture
{
    public class SimpleSail : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Simple Sail");
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(0, 0, 0, 0);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 20;
            Item.useTime = 15;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.maxStack = 999;
            Item.placeStyle = 10;
            Item.useTurn = true;
            Item.createTile = ModContent.TileType<SimpleSailTile>();
        }
    }
}