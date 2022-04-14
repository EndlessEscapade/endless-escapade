using EEMod.Buffs.Buffs;
using EEMod.Items.Fish;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Consumables
{
    public class VexingPotion : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vexing Potion");
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
            player.AddBuff(ModContent.BuffType<Vex>(), 60 * 60 * 7);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<Barracuda>(), 1).AddIngredient(ItemID.BottledWater, 1).AddIngredient(ItemID.Deathweed, 1).AddIngredient(ItemID.Fireblossom, 1).AddTile(TileID.Bottles).Register();
        }
    }
}