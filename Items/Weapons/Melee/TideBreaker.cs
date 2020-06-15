using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Placeables.Ores;

namespace EEMod.Items.Weapons.Melee
{
    public class TideBreaker : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tide Breaker");
        }

        public override void SetDefaults()
        {
            item.rare = ItemRarityID.LightRed;
            item.melee = true;
            item.autoReuse = true;
            item.useStyle = ItemUseStyleID.Stabbing;
            item.width = 20;
            item.height = 20;
            item.useTime = 16;
            item.useAnimation = 16;
            item.knockBack = 3f;
            item.damage = 68;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<HydriteBar>(), 14);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}