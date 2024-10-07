using Terraria.Enums;

namespace EndlessEscapade.Content.Items.Starfish;

public class FallenStarfishStaffItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.DamageType = DamageClass.Summon;

        Item.SetWeaponValues(10, 2f);
        Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice());
    }

    public override void AddRecipes() {
        base.AddRecipes();

        CreateRecipe()
            .AddIngredient<EnchantedSandItem>(4)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
