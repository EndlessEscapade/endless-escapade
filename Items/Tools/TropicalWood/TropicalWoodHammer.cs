using EEMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Tools.TropicalWood
{
    public class TropicalWoodHammer : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tropical Wood Hammer");
        }

        public override void SetDefaults()
        {
            Item.hammer = 48;
            Item.useTime = 34;
            Item.useAnimation = 34;
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Green;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 0, 18);
            Item.damage = 9;
            Item.DamageType = DamageClass.Melee;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;
            Item.knockBack = 2f;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<TropicalWoodItem>(), 8).AddTile(TileID.Anvils).Register();
        }
    }
}