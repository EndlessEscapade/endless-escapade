using EEMod.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Placeables
{
    public class KelpLeaf : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kelpleaf Block");
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(0, 0, 0, 0);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTurn = true;
            Item.useTime = 7;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.maxStack = 999;

            Item.createTile = ModContent.TileType<KelpLeafTile>();
        }
    }
}