using EEMod.Items.Placeables.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Tools.Lythen
{
    public class LythenAxe : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lythen Axe");
        }

        public override void SetDefaults()
        {
            Item.axe = 11;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Green;
            Item.useStyle = ItemUseStyleID.SwingThrow;
            Item.value = Item.sellPrice(0, 0, 18);
            Item.damage = 8;
            Item.melee = true;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item1;
            Item.knockBack = 2f;
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