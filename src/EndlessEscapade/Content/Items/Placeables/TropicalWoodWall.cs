﻿using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Placeables;

public class TropicalWoodWall : ModItem
{
    public override void SetDefaults() {
        Item.DefaultToPlaceableWall((ushort)ModContent.WallType<Walls.TropicalWoodWall>());

        Item.width = 24;
        Item.height = 24;
    }

    public override void AddRecipes() {
        CreateRecipe(4)
            .AddIngredient<TropicalWood>()
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
