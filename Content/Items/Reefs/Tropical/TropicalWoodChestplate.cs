using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Reefs.Tropical;

[AutoloadEquip(EquipType.Body)]
public class TropicalWoodChestplate : ModItem
{
    public override void SetDefaults() {
        Item.defense = 3;

        Item.width = 16;
        Item.height = 16;
    }

    public override void AddRecipes() {
        var recipe = CreateRecipe();
        recipe.AddIngredient<TropicalWood>(30);
        recipe.AddTile(TileID.WorkBenches);
        recipe.Register();
    }
}