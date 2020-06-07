using Terraria.ID;
using Terraria.ModLoader;

namespace Prophecy.Tools
{
    public class QuartzAxe : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 40;

            item.useStyle = 1;
            item.useTurn = true;
            item.useAnimation = 20;
            item.useTime = 20;
            item.autoReuse = true;
            item.width = 24;
            item.height = 28;
            item.damage = 7;
            item.axe = 12;
            item.UseSound = SoundID.Item1;
            item.knockBack = 2.5f;
            item.value = 100;
            item.melee = true;
            item.autoReuse = true;
            item.useTurn = true;
            item.rare = 5;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Quartz Axe");
        }

        public override void AddRecipes()  //How to craft item item
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod, "QuartzGem", 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
