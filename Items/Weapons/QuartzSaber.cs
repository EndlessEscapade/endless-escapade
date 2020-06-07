using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace Prophecy.Weapons
{
    public class QuartzSaber : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Quartz Saber");
        }

        public override void SetDefaults()
        {
            item.damage = 25;
            item.melee = true;
            item.width = 68;
            item.height = 80;
            item.useTime = 13;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 5;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.crit = 6;
        }
        public override void AddRecipes()
        {
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(ItemID.PlatinumBroadsword, 1);
                recipe.AddIngredient(mod.ItemType("QuartzGem"), 7);
                recipe.AddTile(TileID.Anvils);
                recipe.SetResult(this);
                recipe.AddRecipe();
                recipe = new ModRecipe(mod);
                recipe.AddIngredient(ItemID.GoldBroadsword, 1);
                recipe.AddIngredient(mod.ItemType("QuartzGem"), 7);
                recipe.AddTile(TileID.Anvils);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }
    }
}
