using EndlessEscapade.Content.Projectiles.Starfish;
using Terraria.Enums;

namespace EndlessEscapade.Content.Items.Starfish;

public class SpinnerFishItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.consumable = true;
        Item.noMelee = true;
        Item.channel = true;
        Item.noUseGraphic = true;

        Item.maxStack = Item.CommonMaxStack;

        Item.DamageType = DamageClass.Melee;
        Item.SetWeaponValues(9, 1f);

        Item.width = 26;
        Item.height = 26;

        Item.shoot = ModContent.ProjectileType<SpinnerFishProjectile>();
        Item.shootSpeed = 10f;

        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.UseSound = SoundID.Item1;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.SetShopValues(ItemRarityColor.Blue1, Item.sellPrice());
    }

    public override void AddRecipes() {
        base.AddRecipes();

        CreateRecipe(50)
            .AddIngredient<EnchantedSandItem>()
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
