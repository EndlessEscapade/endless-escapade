using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Materials;

public class LythenBar : ModItem
{
    public override void SetStaticDefaults() {
        Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 7));
    }

    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.LythenBar>());

        Item.width = 30;
        Item.height = 24;
    }

    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient<LythenOre>(3)
            .AddTile(TileID.Furnaces)
            .Register();
    }
}
