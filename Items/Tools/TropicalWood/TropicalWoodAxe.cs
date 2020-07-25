using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Placeables.Ores;

namespace EEMod.Items.Tools.TropicalWood
{
    public class TropicalWoodAxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tropical Wood Axe");
        }

        public override void SetDefaults()
        {
            item.axe = 11;
            item.useTime = 20;
            item.useAnimation = 20;
            item.width = 20;
            item.height = 20;
            item.rare = ItemRarityID.Green;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.value = Item.sellPrice(0, 0, 18);
            item.damage = 8;
            item.melee = true;
            item.autoReuse = true;
            item.UseSound = SoundID.Item1;
            item.knockBack = 2f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<LythenBar>(), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}