using EEMod.Tiles.Furniture.Paintings;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Placeables.Furniture.Paintings
{
    public class Cat : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cat");
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.maxStack = 99;
            Item.consumable = true;
            Item.width = 12;
            Item.height = 12;

            Item.createTile = ModContent.TileType<CatTile>();
        }
    }
}