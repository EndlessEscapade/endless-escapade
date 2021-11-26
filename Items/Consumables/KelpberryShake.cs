using EEMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Consumables
{
    public class KelpberryShake : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kelpberry Shake");
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 100;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 32;
            Item.maxStack = 99;
            Item.useAnimation = 12;
            Item.useTime = 12;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.UseSound = SoundID.Item2;
        }

        public override bool? UseItem(Player player)
        {
            player.AddBuff(BuffID.Shine, 60 * 300);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<Kelpberries>(), 5).AddTile(TileID.Bottles).Register();
        }
    }
}