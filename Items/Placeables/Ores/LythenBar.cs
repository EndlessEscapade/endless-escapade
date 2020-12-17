using EEMod.Items.Materials;
using EEMod.Tiles.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace EEMod.Items.Placeables.Ores
{
    public class LythenBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lythen Bar");
            ItemID.Sets.SortingPriorityMaterials[item.type] = 59; // influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 7));
        }

        public override void SetDefaults()
        {
            item.width = 36;
            item.height = 40;
            item.maxStack = 999;
            item.value = Item.buyPrice(0, 0, 18, 0);
            item.rare = ItemRarityID.Green;
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

        public override void PostUpdate()
        {

        }
    }
}