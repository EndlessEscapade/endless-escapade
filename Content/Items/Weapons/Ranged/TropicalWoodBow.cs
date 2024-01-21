using EndlessEscapade.Content.Items.Placeables;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Weapons.Ranged;

public class TropicalWoodBow : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToBow(27, 7f);

        Item.width = 24;
        Item.height = 32;
    }

    public override Vector2? HoldoutOffset() {
        return new Vector2(2f, 0f);
    }

    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient<TropicalWood>(10)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
