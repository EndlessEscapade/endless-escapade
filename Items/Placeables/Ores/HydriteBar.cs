using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Tiles.Ores;

namespace EEMod.Items.Placeables.Ores
{
    public class HydriteBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydrite Bar");
            ItemID.Sets.SortingPriorityMaterials[item.type] = 59; // influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 99;
            item.value = Item.buyPrice(0, 0, 26, 0);
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTurn = true;
            item.material = true;
            item.rare = ItemRarityID.Pink;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.createTile = ModContent.TileType<HydriteBarTile>();
            item.placeStyle = 0;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<HydriteOre>(), 4);
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}