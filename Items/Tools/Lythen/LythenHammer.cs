using EEMod.Items.Placeables.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Tools.Lythen
{
    public class LythenHammer : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lythen Hammer");
        }

        public override void SetDefaults()
        {
            Item.hammer = 48;
            Item.useTime = 34;
            Item.useAnimation = 34;
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Green;
            Item.useStyle = ItemUseStyleID.SwingThrow;
            Item.value = Item.sellPrice(0, 0, 18);
            Item.damage = 9;
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