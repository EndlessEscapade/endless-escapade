using EndlessEscapade.Content.Projectiles.Starfish;
using Terraria.Enums;

namespace EndlessEscapade.Content.Items.Starfish;

public class StarCatcherItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.fishingPole = 15;

        Item.width = 52;
        Item.height = 44;

        Item.useTime = 8;
        Item.useAnimation = 8;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.shoot = ModContent.ProjectileType<StarCatcherBobberProjectile>();
        Item.shootSpeed = 10f;

        Item.UseSound = SoundID.Item1;
        Item.SetShopValues(ItemRarityColor.Blue1, Item.sellPrice());
    }

    public override void ModifyFishingLine(Projectile bobber, ref Vector2 lineOriginOffset, ref Color lineColor) {
        base.ModifyFishingLine(bobber, ref lineOriginOffset, ref lineColor);

        lineOriginOffset = new Vector2(46, -36);
    }

    public override void HoldItem(Player player) {
        base.HoldItem(player);

        player.accFishingLine = true;
    }

    public override void AddRecipes() {
        base.AddRecipes();

        CreateRecipe()
            .AddIngredient<EnchantedSandItem>(3)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
