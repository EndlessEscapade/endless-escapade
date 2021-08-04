using EEMod.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Placeables
{
    public class LightGemsand : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Light Gemsand");
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(0, 0, 0, 0);
            Item.useStyle = ItemUseStyleID.SwingThrow;
            Item.useAnimation = 20;
            Item.useTime = 15;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.maxStack = 999;
            Item.placeStyle = 10;
            Item.createTile = ModContent.TileType<LightGemsandTile>();
        }
    }
}