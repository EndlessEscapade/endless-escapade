using EEMod.Tiles.Furniture.Paintings;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Placeables.Furniture.Paintings
{
    public class MysteriousJar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mysterious Jar");
            Tooltip.SetDefault("^ < >");
        }

        public override void SetDefaults()
        {
            //item.useStyle = 1;
            //item.useTurn = true;
            //item.useAnimation = 15;
            //item.useTime = 10;
            //item.autoReuse = true;
            //item.maxStack = 99;
            //item.consumable = true;
            //item.createTile = 285 + type - 2174;
            //item.width = 12;
            //item.height = 12;
            item.rare = ItemRarityID.Cyan;

            item.CloneDefaults(ItemID.WoodenCrate);
            item.createTile = ModContent.TileType<MysteriousJarTile>();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.GreenThread, 3);
            recipe.AddIngredient(ItemID.Bottle, 1);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}