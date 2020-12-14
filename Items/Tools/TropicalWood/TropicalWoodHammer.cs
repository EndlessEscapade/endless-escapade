using EEMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Tools.TropicalWood
{
    public class TropicalWoodHammer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tropical Wood Hammer");
        }

        public override void SetDefaults()
        {
            item.hammer = 48;
            item.useTime = 34;
            item.useAnimation = 34;
            item.width = 20;
            item.height = 20;
            item.rare = ItemRarityID.Green;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.value = Item.sellPrice(0, 0, 18);
            item.damage = 9;
            item.melee = true;
            item.autoReuse = true;
            item.UseSound = SoundID.Item1;
            item.knockBack = 2f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<TropicalWoodItem>(), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}