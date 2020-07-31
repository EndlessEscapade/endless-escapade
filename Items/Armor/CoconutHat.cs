using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Materials.Fruit;

namespace EEMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class CoconutHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coconut Hat");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 0, 30);
            item.rare = ItemRarityID.Orange;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Coconut>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}