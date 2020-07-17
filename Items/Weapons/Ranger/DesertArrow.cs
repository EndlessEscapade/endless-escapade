/*using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Materials;
using EEMod.Projectiles.Ranged;

namespace EEMod.Items.Weapons.Ranger
{
    public class DesertArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Desert Arrow");
            Tooltip.SetDefault("Yes, this is a ranged weapon.");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 22;
            item.damage = 16;
            item.ranged = true;
            item.noMelee = true;
            item.consumable = false;
            item.noUseGraphic = true;
            item.useAnimation = 14;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 14;
            item.knockBack = 0f;
            item.UseSound = SoundID.Item18;
            item.autoReuse = true;
            item.maxStack = 1;
            item.value = 500;
            item.rare = ItemRarityID.Orange;
            item.shoot = ModContent.ProjectileType<DesertArrowProjectile>();
            item.shootSpeed = 11f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HardenedSand, 4);
            recipe.AddIngredient(ItemID.Sandstone, 7);
            recipe.AddIngredient(ItemID.Diamond, 1);
            recipe.AddIngredient(ModContent.ItemType<MummifiedRag>(), 3);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}*/
