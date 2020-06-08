using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using EEMod.Tiles.Ores;
using EEMod.Items.Materials;

namespace EEMod.Items.Placeables.Ores
{
    public class LythenBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lythen Bar");
            ItemID.Sets.SortingPriorityMaterials[item.type] = 59; // influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 99;
            item.value = Item.buyPrice(0, 0, 18, 0);
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTurn = true;
            item.rare = ItemRarityID.Green;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.consumable = true;
            item.material = true;
            item.placeStyle = 0;
            item.createTile = ModContent.TileType<LythenBarTile>();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<LythenOre>(), 3);
            recipe.AddIngredient(ModContent.ItemType<HydrosScales>(), 1);
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}