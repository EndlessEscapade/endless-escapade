/*using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Food
{
    public class Cheese : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cheese");
            Tooltip.SetDefault("'Enjoy it while you can'");
            ItemID.Sets.SortingPriorityMaterials[item.type] = 100;
        }

        public override void SetDefaults()
        {
            item.width = 50;
            item.height = 34;
            item.maxStack = 999;
            item.useAnimation = 12;
            item.useTime = 12;
            item.consumable = true;
            item.value = Item.buyPrice(0, 1, 0, 0);
            item.rare = ItemRarityID.Blue;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.UseSound = SoundID.Item2;
        }

        public override bool UseItem(Player player)
        {
            player.AddBuff(BuffID.WellFed, 90000);
            return true;
        }
    }
}*/