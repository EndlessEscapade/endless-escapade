using EEMod.Items.Placeables.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Tools.Hydrite
{
    public class HydriteHamaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydrite Hamaxe");
        }

        public override void SetDefaults()
        {
            item.axe = 13;
            item.hammer = 50;
            item.useTime = 38;
            item.useAnimation = 38;
            item.width = 20;
            item.height = 20;
            item.rare = ItemRarityID.Orange;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.value = Item.sellPrice(0, 0, 48);
            item.damage = 12;
            item.melee = true;
            item.autoReuse = true;
            item.UseSound = SoundID.Item1;
            item.knockBack = 3f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<HydriteBar>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}