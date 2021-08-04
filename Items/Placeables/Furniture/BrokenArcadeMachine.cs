using EEMod.Tiles.Furniture;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Placeables.Furniture
{
    public class BrokenArcadeMachine : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Broken Arcade Machine");
            Tooltip.SetDefault("Maybe you can repair it");
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

            Item.createTile = ModContent.TileType<BrokenArcadeMachineTile>();
        }
    }
}