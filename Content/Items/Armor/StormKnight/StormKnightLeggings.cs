using EndlessEscapade.Content.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Armor.StormKnight;

[AutoloadEquip(EquipType.Legs)]
public class StormKnightLeggings : ModItem
{
    public override void SetDefaults() {
        Item.width = 22;
        Item.height = 14;
    }

    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient<LythenBar>(4)
            .AddTile(TileID.Anvils)
            .Register();
    }
}
