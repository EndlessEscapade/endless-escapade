using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Tropical;

[AutoloadEquip(EquipType.Legs)]
public class TropicalWoodBoots : ModItem
{
    public override void SetDefaults() {
        Item.defense = 2;

        Item.width = 18;
        Item.height = 12;
    }

    public override void AddRecipes() {
        var recipe = CreateRecipe();
        recipe.AddIngredient<TropicalWood>(25);
        recipe.AddTile(TileID.WorkBenches);
        recipe.Register();
    }
}
