using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items
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
            item.value = Item.buyPrice(0, 1, 0, 0);
            item.rare = ItemRarityID.White;
            item.useAnimation = 12;
            item.useTime = 12;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.consumable = true;
            item.UseSound = SoundID.Item2;
        }
        public override bool UseItem(Player player)
        {
            player.AddBuff(BuffID.WellFed, 90000);
            return true;
        }
    }
}