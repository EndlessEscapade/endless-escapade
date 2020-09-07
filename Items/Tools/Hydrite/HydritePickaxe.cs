using EEMod.Items.Placeables.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Tools.Hydrite
{
    public class HydritePickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydrite Pickaxe");
        }

        public override void SetDefaults()
        {
            item.melee = true;
            item.pick = 70;
            item.useTime = 19;
            item.useAnimation = 19;
            item.damage = 7;
            item.rare = ItemRarityID.Orange;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.width = 16;
            item.height = 16;
            item.value = Item.sellPrice(0, 0, 21);
            item.knockBack = 2f;
            item.autoReuse = true;
            item.UseSound = SoundID.Item1;
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