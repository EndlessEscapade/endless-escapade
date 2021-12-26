using EEMod.Tiles.Furniture;
using EEMod.Tiles.Furniture.Shipyard;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Placeables.Furniture
{
    public class WoodenShipsWheel : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wooden Ship's Wheel");
            Tooltip.SetDefault("Perfect for steering a boat");
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.maxStack = 1;
            Item.consumable = true;
            Item.width = 12;
            Item.height = 12;
            Item.rare = ItemRarityID.White;

            Item.createTile = ModContent.TileType<WoodenShipsWheelTile>();
        }
    }
}