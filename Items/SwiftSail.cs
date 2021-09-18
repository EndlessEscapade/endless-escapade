using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items
{
    public class SwiftSail : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swift Sail");
            Tooltip.SetDefault("Use this to make your boat sail faster.");
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 59; // influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.value = Item.buyPrice(0, 0, 18, 0);
            Item.rare = ItemRarityID.Green;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.UseSound = SoundID.Item1;
            Item.consumable = true;
            Item.useTime = 15;
            Item.useAnimation = 15;
        }

        public override void OnConsumeItem(Player player)
        {
            EEPlayer modPlayer = player.GetModPlayer<EEPlayer>();
            modPlayer.boatSpeed = 3;
            Main.NewText(modPlayer.boatSpeed);
        }

        public override bool? UseItem(Player player)
        {
            EEPlayer modPlayer = player.GetModPlayer<EEPlayer>();
            modPlayer.boatSpeed = 3;
            if (modPlayer.boatSpeed != 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}