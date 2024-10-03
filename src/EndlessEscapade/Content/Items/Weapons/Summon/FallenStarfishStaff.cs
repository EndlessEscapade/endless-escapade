using EndlessEscapade.Content.Items.Materials;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.Weapons.Summon;

public class FallenStarfishStaff : ModItem
{
    public override void SetDefaults() {
        Item.DamageType = DamageClass.Summon;

        Item.SetWeaponValues(10, 2f);
        Item.SetShopValues(ItemRarityColor.Blue1, Item.sellPrice());
    }

    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient<EnchantedSand>(4)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
