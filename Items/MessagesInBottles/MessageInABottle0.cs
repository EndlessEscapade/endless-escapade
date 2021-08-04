using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.MessagesInBottles
{
    public class MessageInABottle : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Message in a Bottle");
            Tooltip.SetDefault("A Captain boasted he was the best seafarer of the seven seas\nHe was overcome by a storm on one of his voyages\nI was there too, and thankfully he let me in\nI took shelter with the Captian, and in return told him about...\nThe rest of the paper is torn.");
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 59; // influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 0, 18, 0);
            Item.rare = ItemRarityID.White;
        }
    }
}