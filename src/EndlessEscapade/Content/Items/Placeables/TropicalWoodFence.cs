using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Placeables;

public class TropicalWoodFence : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableWall((ushort)ModContent.WallType<Walls.TropicalWoodFence>());

        Item.width = 32;
        Item.height = 32;
    }

    public override void AddRecipes() {
        CreateRecipe(4)
            .AddIngredient<TropicalWood>()
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
