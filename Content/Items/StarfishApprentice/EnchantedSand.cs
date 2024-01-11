using EndlessEscapade.Content.Items.Shoals;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.StarfishApprentice;

public class EnchantedSand : ModItem
{
    public override void SetDefaults() {
        Item.maxStack = Item.CommonMaxStack;

        Item.width = 24;
        Item.height = 18;

        Item.SetShopValues(ItemRarityColor.Blue1, Item.sellPrice(copper: 50));
    }

    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient(ItemID.FallenStar)
            .AddIngredient<Coralsand>(4)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
