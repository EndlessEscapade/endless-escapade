using EEMod.Tiles.Furniture;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Placeables.Furniture
{
    public class AirHockeyTable : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Air Hockey Table");
            Tooltip.SetDefault("Fun for the whole family!");
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.SwingThrow;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.maxStack = 1;
            Item.consumable = true;
            Item.width = 12;
            Item.height = 12;
            Item.rare = ItemRarityID.White;

            Item.createTile = ModContent.TileType<AirHockeyTableTile>();
        }
    }
}