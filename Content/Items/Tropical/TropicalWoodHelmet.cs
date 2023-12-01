using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Tropical;

[AutoloadEquip(EquipType.Head)]
public class TropicalWoodHelmet : ModItem
{
    public override void SetStaticDefaults() {
        ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
    }

    public override void SetDefaults() {
        Item.defense = 2;

        Item.width = 20;
        Item.height = 16;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs) {
        return body.type == ModContent.ItemType<TropicalWoodChestplate>() && legs.type == ModContent.ItemType<TropicalWoodBoots>();
    }

    public override void UpdateArmorSet(Player player) {
        player.setBonus = "+1 defense";
        player.statDefense++;
    }

    public override void AddRecipes() {
        var recipe = CreateRecipe();
        recipe.AddIngredient<TropicalWood>(20);
        recipe.AddTile(TileID.WorkBenches);
        recipe.Register();
    }
}
