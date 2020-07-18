using Terraria;
using Terraria.ModLoader;
using EEMod.Tiles.Ores;
using Terraria.ID;

namespace EEMod.Items.Placeables.Ores
{
    public class DalantiniumOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dalantinium Ore");
        }

        public override void SetDefaults()
        {
            item.width = 10;
            item.height = 10;
            item.rare = ItemRarityID.Orange;
            item.value = Item.sellPrice(0, 0, 3);
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useAnimation = 20;
            item.useTime = 15;
            item.consumable = true;
            item.material = true;
            item.maxStack = 999;
            item.placeStyle = 10;
            item.createTile = ModContent.TileType<DalantiniumOreTile>();
        }
    }
}