using EndlessEscapade.Content.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Armor.StormKnight;

[AutoloadEquip(EquipType.Head)]
public class StormKnightCrest : ModItem
{
    // TODO: Glowmask.
    public override void SetDefaults() {
        Item.width = 26;
        Item.height = 28;
    }

    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient<LythenBar>(3)
            .AddTile(TileID.Anvils)
            .Register();
    }
}
