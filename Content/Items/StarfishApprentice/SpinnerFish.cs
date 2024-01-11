using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria;
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
        Item.SetWeaponValues(9, 1f);

        Item.width = 26;
        Item.height = 26;

        Item.shoot = ModContent.ProjectileType<Projectiles.StarfishApprentice.SpinnerFish>();
        Item.shootSpeed = 10f;

        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.UseSound = SoundID.Item1;
        Item.SetShopValues(ItemRarityColor.Blue1, Item.sellPrice());
    }

    public override void AddRecipes() {
        CreateRecipe(50)
            .AddIngredient<EnchantedSand>()
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
