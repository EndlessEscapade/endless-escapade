using EndlessEscapade.Content.Items.Materials;
using EndlessEscapade.Content.Projectiles.Typeless;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Tools;

public class StarCatcher : ModItem
{
    public override void SetDefaults() {
        Item.fishingPole = 15;

        Item.width = 52;
        Item.height = 44;

        Item.useTime = 8;
        Item.useAnimation = 8;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.shoot = ModContent.ProjectileType<StarCatcherBobber>();
        Item.shootSpeed = 10f;
        
        Item.UseSound = SoundID.Item1;
        Item.SetShopValues(ItemRarityColor.Blue1, Item.sellPrice());
    }

    public override void HoldItem(Player player) {
        player.accFishingLine = true;
    }

    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient<EnchantedSand>(3)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
