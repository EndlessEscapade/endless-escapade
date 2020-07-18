using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Tiles.Ores;

namespace EEMod.Items.Placeables.Ores
{
    public class HydroFluoride : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydrofluoride");
        }

        public override void SetDefaults()
        {
            item.width = 10;
            item.height = 10;
            item.rare = ItemRarityID.Lime;
            item.value = Item.sellPrice(0, 0, 5);
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useAnimation = 20;
            item.useTime = 15;
            item.consumable = true;
            item.material = true;
            item.maxStack = 999;
            item.placeStyle = 10;
            item.createTile = ModContent.TileType<HydroFluorideOreTile>();
        }
    }
}