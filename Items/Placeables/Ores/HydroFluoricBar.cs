using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Tiles.Ores;

namespace EEMod.Items.Placeables.Ores
{
    public class HydrofluoricBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydrofluoric Bar");
            ItemID.Sets.SortingPriorityMaterials[item.type] = 59; // influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 99;
            item.value = Item.buyPrice(0, 0, 32, 0);
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTurn = true;
            item.rare = ItemRarityID.Lime;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.consumable = true;
            item.material = true;
            item.placeStyle = 0;
            item.createTile = ModContent.TileType<HydrofluoricBarTile>();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<HydrofluoricOre>(), 4);
            recipe.AddTile(TileID.AdamantiteForge);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}