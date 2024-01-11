using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Lythen;

[AutoloadEquip(EquipType.Body)]
public class StormKnightBreastplate : ModItem
{
    public override void SetDefaults() {
        Item.width = 34;
        Item.height = 20;
    }
    
    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient<LythenBar>(5)
            .AddTile(TileID.Anvils)
            .Register();
    }

    public override bool IsArmorSet(Item head, Item body, Item legs) {
        return body.type == ModContent.ItemType<StormKnightBreastplate>() && legs.type == ModContent.ItemType<StormKnightLeggings>();
    }
}
