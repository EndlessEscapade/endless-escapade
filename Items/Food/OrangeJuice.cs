using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Materials.Fruit;

namespace EEMod.Items.Food
{
    public class OrangeJuice : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orange Juice");
            ItemID.Sets.SortingPriorityMaterials[item.type] = 100;
        }

        public override void SetDefaults()
        {
            item.width = 50;
            item.height = 34;
            item.maxStack = 999;
            item.value = Item.buyPrice(0, 1, 0, 0);
            item.rare = ItemRarityID.Blue;
            item.useAnimation = 12;
            item.useTime = 12;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.consumable = true;
            item.UseSound = SoundID.Item2;
        }
        public override bool UseItem(Player player)
        {
            player.AddBuff(BuffID.WellFed, 60 * 300);
            player.AddBuff(BuffID.Swiftness, 60 * 60);
            player.AddBuff(BuffID.Regeneration, 60 * 30);
            return true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Orange>(), 10);
            recipe.AddTile(TileID.Kegs);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}