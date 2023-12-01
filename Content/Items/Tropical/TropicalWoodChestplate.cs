using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Tropical;

[AutoloadEquip(EquipType.Body)]
public class TropicalWoodChestplate : ModItem
{
    public override void SetDefaults() {
        Item.defense = 3;

        Item.width = 24;
        Item.height = 22;
    }

    public override void AddRecipes() {
        var recipe = CreateRecipe();
        recipe.AddIngredient<TropicalWood>(30);
        recipe.AddTile(TileID.WorkBenches);
        recipe.Register();
    }
}
