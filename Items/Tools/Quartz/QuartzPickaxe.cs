using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Tools.Quartz
{
    public class QuartzPickaxe : ModItem
    {
        public override void SetDefaults()
        {

            item.damage = 15;
            item.melee = true;
            item.width = 64;
            item.height = 64;
            item.useAnimation = 25;
            item.useTime = 10;
            item.pick = 59;
            item.useStyle = 1;
            item.knockBack = 1;
            item.value = Terraria.Item.sellPrice(0, 1, 8, 0);
            item.rare = 5;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.useTurn = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Quartz Pickaxe");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod, "QuartzGem", 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
