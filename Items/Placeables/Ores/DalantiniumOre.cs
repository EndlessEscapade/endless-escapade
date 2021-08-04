using EEMod.Tiles.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Placeables.Ores
{
    public class DalantiniumOre : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dalantinium Ore");
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 0, 3);
            Item.useStyle = ItemUseStyleID.SwingThrow;
            Item.useAnimation = 20;
            Item.useTime = 15;
            Item.consumable = true;
            Item.material = true;
            Item.maxStack = 999;
            Item.placeStyle = 10;
            Item.createTile = ModContent.TileType<DalantiniumOreTile>();
        }
    }
}