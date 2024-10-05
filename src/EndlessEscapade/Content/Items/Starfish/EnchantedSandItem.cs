using EndlessEscapade.Content.Items.Shoals;
using Terraria.Enums;

namespace EndlessEscapade.Content.Items.Starfish;

public class EnchantedSandItem : ModItem
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
            .AddIngredient<CoralsandItem>(4)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
