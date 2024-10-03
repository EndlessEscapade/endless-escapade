using EndlessEscapade.Content.Items.Placeables;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Armor.Tropical;

[AutoloadEquip(EquipType.Body)]
public class TropicalWoodChestplate : ModItem
{
    public override void SetDefaults() {
        Item.defense = 3;

        Item.width = 24;
        Item.height = 22;
    }

    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient<TropicalWood>(30)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
