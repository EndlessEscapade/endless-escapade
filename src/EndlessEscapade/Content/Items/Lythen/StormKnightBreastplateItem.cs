namespace EndlessEscapade.Content.Items.Lythen;

[AutoloadEquip(EquipType.Body)]
public class StormKnightBreastplateItem : ModItem
{
    public override void SetDefaults() {
        base.SetDefaults();

        Item.width = 34;
        Item.height = 20;
    }

    public override void AddRecipes() {
        base.AddRecipes();

        CreateRecipe()
            .AddIngredient<LythenBarItem>(5)
            .AddTile(TileID.Anvils)
            .Register();
    }

    public override bool IsArmorSet(Item head, Item body, Item legs) {
        return body.type == ModContent.ItemType<StormKnightBreastplateItem>() && legs.type == ModContent.ItemType<StormKnightLeggingsItem>();
    }
}
