using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items
{
    public class SwiftSail : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swift Sail");
            Tooltip.SetDefault("Use this to make your boat sail faster.");
            ItemID.Sets.SortingPriorityMaterials[item.type] = 59; // influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 999;
            item.value = Item.buyPrice(0, 0, 18, 0);
            item.rare = ItemRarityID.Green;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.UseSound = SoundID.Item1;
            item.consumable = true;
            item.useTime = 15;
            item.useAnimation = 15;
        }

        public override void OnConsumeItem(Player player)
        {
            EEPlayer modPlayer = player.GetModPlayer<EEPlayer>();
            if (modPlayer.boatSpeed != 2)
            {
                modPlayer.boatSpeed = 2;
            }
        }

        public override bool UseItem(Player player)
        {
            return true;
        }
    }
}