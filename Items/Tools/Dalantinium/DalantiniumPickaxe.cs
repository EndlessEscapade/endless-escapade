using EEMod.Items.Placeables.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Tools.Dalantinium
{
    public class DalantiniumPickaxe : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dalantinium Pickaxe");
        }

        public override void SetDefaults()
        {
            Item.melee = true;
            Item.pick = 70;
            Item.useTime = 19;
            Item.useAnimation = 19;
            Item.damage = 7;
            Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.SwingThrow;
            Item.width = 16;
            Item.height = 16;
            Item.value = Item.sellPrice(0, 0, 21);
            Item.knockBack = 2f;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;
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