using EEMod.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Placeables
{
    public class RustyPlating : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rusty Plating");
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(0, 0, 0, 0);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 20;
            Item.useTime = 15;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.maxStack = 999;
            Item.placeStyle = 10;
            Item.createTile = ModContent.TileType<RustyPlatingTile>();
        }
    }
}