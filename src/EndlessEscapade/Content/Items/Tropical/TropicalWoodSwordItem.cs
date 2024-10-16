﻿namespace EndlessEscapade.Content.Items.Tropical;

public class TropicalWoodSwordItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.DamageType = DamageClass.Melee;
        Item.damage = 15;
        Item.knockBack = 4.5f;

        Item.useTime = 16;
        Item.useAnimation = 16;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.width = 34;
        Item.height = 34;

        Item.UseSound = SoundID.Item1;
    }

    public override void AddRecipes() {
        base.AddRecipes();

        CreateRecipe()
            .AddIngredient<TropicalWoodItem>(8)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
