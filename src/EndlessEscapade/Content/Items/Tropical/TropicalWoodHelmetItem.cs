namespace EndlessEscapade.Content.Items.Tropical;

[AutoloadEquip(EquipType.Head)]
public class TropicalWoodHelmetItem : ModItem
{
    public override void SetStaticDefaults() {
        base.SetStaticDefaults();

        ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
    }

    public override void SetDefaults() {
        base.SetDefaults();

        Item.defense = 2;

        Item.width = 20;
        Item.height = 16;
    }

    public override void AddRecipes() {
        base.AddRecipes();

        CreateRecipe()
            .AddIngredient<TropicalWoodItem>(20)
            .AddTile(TileID.WorkBenches)
            .Register();
    }

    public override void UpdateArmorSet(Player player) {
        base.UpdateArmorSet(player);

        player.setBonus = "+1 defense";
        player.statDefense++;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs) {
        return body.type == ModContent.ItemType<TropicalWoodChestplateItem>() && legs.type == ModContent.ItemType<TropicalWoodBootsItem>();
    }
}
