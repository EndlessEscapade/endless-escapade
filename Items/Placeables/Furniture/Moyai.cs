using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Tiles.Furniture;

namespace EEMod.Items.Placeables.Furniture
{
    public class Moyai : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moyai");
        }

        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.maxStack = 1;
            item.consumable = true;
            item.width = 12;
            item.height = 12;
            item.rare = ItemRarityID.White;

            item.createTile = ModContent.TileType<MoyaiTile>();
        }
    }
}