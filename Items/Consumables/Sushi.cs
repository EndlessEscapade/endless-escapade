using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Materials;

namespace EEMod.Items.Consumables
{
    public class Sushi : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sushi");
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 100;
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 34;
            Item.maxStack = 999;
            Item.useAnimation = 12;
            Item.useTime = 12;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.UseSound = SoundID.Item2;
        }

        public override bool? UseItem(Player player)
        {
            player.AddBuff(BuffID.WellFed, 60 * 300);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Bass, 1).AddIngredient(ModContent.ItemType<Kelp>(), 10).AddTile(TileID.CookingPots).Register();
        }
    }
}