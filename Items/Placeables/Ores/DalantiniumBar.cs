using EEMod.Tiles.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Placeables.Ores
{
    public class DalantiniumBar : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dalantinium Bar");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.placeStyle = 0;
            Item.material = true;
            Item.consumable = true;
            Item.maxStack = 99;
            Item.useTime = 15;
            Item.useAnimation = 20;
            Item.value = Item.sellPrice(0, 0, 18);
            Item.createTile = ModContent.TileType<DalantiniumBarTile>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<DalantiniumOre>(), 3).AddTile(TileID.Furnaces).Register();
        }
    }
}