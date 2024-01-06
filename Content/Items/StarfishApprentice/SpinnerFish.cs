using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.StarfishApprentice;

public class SpinnerFish : ModItem
{
    public override void SetDefaults() {
        Item.consumable = true;
        Item.noMelee = true;
        Item.channel = true;
        Item.noUseGraphic = true;

        Item.maxStack = Item.CommonMaxStack;
        
        Item.DamageType = DamageClass.Ranged;

        Item.damage = 9;
        Item.knockBack = 1f;

        Item.width = 26;
        Item.height = 26;

        Item.shoot = ModContent.ProjectileType<Projectiles.StarfishApprentice.SpinnerFish>();
        Item.shootSpeed = 10f;

        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.rare = ItemRarityID.Blue;
        Item.UseSound = SoundID.Item1;
    }
    
    public override void AddRecipes() {
        var recipe = CreateRecipe(50);
        recipe.AddIngredient<EnchantedSand>(1);
        recipe.AddTile(TileID.WorkBenches);
        recipe.Register();
    }
}
