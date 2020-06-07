using Terraria.ID;
using Terraria.ModLoader;

namespace Prophecy.Tools
{
    public class QuartzHammer : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 10;
            item.melee = true;
            item.width = 40;
            item.height = 30;
            item.useTime = 18;
            item.useAnimation = 29;
            item.hammer = 59;
            item.useStyle = 1;
            item.knockBack = 1;
            // item.value = Item.sellPrice(0, 0, 10, 0);
            item.rare = 5;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.useTurn = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Quartz Hammer");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "QuartzGem", 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
