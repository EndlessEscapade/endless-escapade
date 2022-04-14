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
            Item.DamageType = DamageClass.Melee;
            Item.pick = 70;
            Item.useTime = 19;
            Item.useAnimation = 19;
            Item.damage = 7;
            Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.width = 16;
            Item.height = 16;
            Item.value = Item.sellPrice(0, 0, 21);
            Item.knockBack = 2f;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<DalantiniumBar>(), 12).AddTile(TileID.Anvils).Register();
        }
    }
}