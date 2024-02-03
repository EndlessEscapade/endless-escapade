using EndlessEscapade.Content.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Armor.Tropical;

[AutoloadEquip(EquipType.Head)]
public class TropicalWoodHelmet : ModItem
{
    public override void SetStaticDefaults() {
        ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
    }

    public override void SetDefaults() {
        Item.defense = 2;

        Item.width = 20;
        Item.height = 16;
    }

    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient<TropicalWood>(20)
            .AddTile(TileID.WorkBenches)
            .Register();
    }

    public override void UpdateArmorSet(Player player) {
        player.setBonus = "+1 defense";
        player.statDefense++;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs) {
        return body.type == ModContent.ItemType<TropicalWoodChestplate>() && legs.type == ModContent.ItemType<TropicalWoodBoots>();
    }
}
