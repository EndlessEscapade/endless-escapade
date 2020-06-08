using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Placeables.Ores;

namespace EEMod.Items.Tools.Dalantinium
{
    public class DalantiniumHamaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dalantinium Hamaxe");
            Tooltip.SetDefault("STOP! Hammer time");
        }

        public override void SetDefaults()
        {
            item.axe = 13;
            item.hammer = 50;
            item.useTime = 38;
            item.useAnimation = 38;
            item.width = 20;
            item.height = 20;
            item.rare = ItemRarityID.Green;
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
            recipe.AddIngredient(ModContent.ItemType<DalantiniumBar>(), 14);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}