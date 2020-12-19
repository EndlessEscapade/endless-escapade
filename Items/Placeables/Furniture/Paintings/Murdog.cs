using EEMod.Tiles.Furniture.Paintings;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Placeables.Furniture.Paintings
{
    public class Murdog : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Portrait of a Dog");
        }

        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.maxStack = 99;
            item.consumable = true;
            item.width = 12;
            item.height = 12;
            item.rare = ItemRarityID.Cyan;

            item.createTile = ModContent.TileType<MurdogTile>();
        }
    }
}