﻿using EndlessEscapade.Content.Items.Placeables;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Tools;

public class TropicalWoodHammer : ModItem
{
    public override void SetDefaults() {
        Item.DamageType = DamageClass.Melee;
        Item.damage = 8;
        Item.knockBack = 5.5f;

        Item.useTime = 18;
        Item.useAnimation = 30;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.width = 40;
        Item.height = 42;

        Item.hammer = 45;

        Item.UseSound = SoundID.Item1;
    }

    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient<TropicalWood>(10)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
