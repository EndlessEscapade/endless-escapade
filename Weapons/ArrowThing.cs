using Terraria.ID;
using Terraria.ModLoader;

namespace Prophecy.Weapons
{
    public class ArrowThing : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Desert Arrow");
            Tooltip.SetDefault("Yes, this is a ranged weapon.");
        }
        public override void AddRecipes()
        {
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(ItemID.HardenedSand, 4);
                recipe.AddIngredient(ItemID.Sandstone, 7);
                recipe.AddIngredient(ItemID.Diamond, 1);
                recipe.AddIngredient(mod.ItemType("MummifiedRag"), 3);
                recipe.AddTile(TileID.Anvils);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 22;
            item.damage = 16;
            item.thrown = true;
            item.noMelee = true;
            item.consumable = false;
            item.noUseGraphic = true;
            item.useAnimation = 14;
            item.useStyle = 1;
            item.useTime = 14;
            item.knockBack = 0f;
            item.UseSound = SoundID.Item18;
            item.autoReuse = true;
            item.maxStack = 1;
            item.value = 500;
            item.rare = 3;
            item.shoot = mod.ProjectileType("ArrowThingProj");
            item.shootSpeed = 11f;
        }
    }
}
