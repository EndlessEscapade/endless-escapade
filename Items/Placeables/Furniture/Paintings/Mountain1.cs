using EEMod.Tiles.Furniture.Paintings;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Placeables.Furniture.Paintings
{
    public class Mountain1 : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mountain");
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.SwingThrow;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.maxStack = 99;
            Item.consumable = true;
            Item.width = 12;
            Item.height = 12;

            Item.createTile = ModContent.TileType<Mountain1Tile>();
        }
    }
}