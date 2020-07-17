using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Tiles;

namespace EEMod.Items.Placeables
{
    public class DarkGemsand : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dark Gemsand");
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
            item.createTile = ModContent.TileType<DarkGemsandTile>();
        }
    }
}