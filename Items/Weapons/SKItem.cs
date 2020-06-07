using Terraria.ID;
using Terraria.ModLoader;

namespace Prophecy.Weapons
{
    public class SKItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sandvek Kunai");
            Tooltip.SetDefault("Kunai sticks to an enemy and deals 5 damage per second. Stacks up to 3.");
        }

        public override void SetDefaults()
        {
            item.shoot = mod.ProjectileType("SandvekKunai");
            item.shootSpeed = 8f;
            item.damage = 50;
            item.knockBack = 5f;
            item.ranged = true;
            item.useStyle = 1;
            item.UseSound = SoundID.Item1;
            item.useAnimation = 60;
            item.useTime = 60;
            item.width = 80;
            item.height = 80;
            item.maxStack = 1;
            item.consumable = false;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.value = 10000;
            item.rare = 3;
        }

       
        public override void AddRecipes()
        {
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(ItemID.HardenedSand, 8);
                recipe.AddIngredient(ItemID.Sandstone, 3);
                recipe.AddIngredient(ItemID.Diamond, 1);
                recipe.AddIngredient(mod.ItemType("MummifiedRag"), 2);
                recipe.AddTile(TileID.Anvils);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }
    }
}
