using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Lythen;

public class StormGauntlet : ModItem
{
    public override void SetDefaults() {
        Item.DamageType = DamageClass.Melee;
        Item.damage = 20;
        Item.knockBack = 6.5f;
        
        Item.channel = true;
        Item.noMelee = true;
        Item.noUseGraphic = true;
        
        Item.useTime = 5;
        Item.useAnimation = 5;
        Item.useStyle = ItemUseStyleID.HoldUp;
    }

    public override void AddRecipes() {
        var recipe = CreateRecipe();
        recipe.AddIngredient<LythenBar>(12);
        recipe.AddTile(TileID.Anvils);
        recipe.Register();
    }

    public override bool CanUseItem(Player player) {
        return player.ownedProjectileCounts[Item.shoot] == 0;
    }
}
