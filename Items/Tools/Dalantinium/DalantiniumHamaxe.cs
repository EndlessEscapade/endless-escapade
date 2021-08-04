using EEMod.Items.Placeables.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Tools.Dalantinium
{
    public class DalantiniumHamaxe : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dalantinium Hamaxe");
        }

        public override void SetDefaults()
        {
            Item.axe = 13;
            Item.hammer = 50;
            Item.useTime = 38;
            Item.useAnimation = 38;
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.SwingThrow;
            Item.value = Item.sellPrice(0, 0, 48);
            Item.damage = 12;
            Item.melee = true;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;
            Item.knockBack = 3f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<DalantiniumBar>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}