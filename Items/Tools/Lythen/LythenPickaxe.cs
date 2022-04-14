using EEMod.Items.Placeables.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Tools.Lythen
{
    public class LythenPickaxe : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lythen Pickaxe");
        }

        public override void SetDefaults()
        {
            Item.pick = 42;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Green;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 0, 18);
            Item.damage = 8;
            Item.DamageType = DamageClass.Melee;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;
            Item.knockBack = 2f;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<LythenBar>(), 8).AddTile(TileID.Anvils).Register();
        }
    }
}