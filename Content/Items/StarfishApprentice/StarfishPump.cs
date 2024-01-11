using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Enums;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Content.Items.StarfishApprentice;

public class StarfishPump : ModItem
{
    public override void SetDefaults() {
        Item.noMelee = true;
        
        Item.DamageType = DamageClass.Melee;
        Item.SetWeaponValues(25, 4f);
        
        Item.width = 76;
        Item.height = 28;

        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.useStyle = ItemUseStyleID.Shoot;

        Item.rare = ItemRarityID.Blue;
        Item.SetShopValues(ItemRarityColor.Blue1, Item.sellPrice());
    }

    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient<EnchantedSand>(4)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
