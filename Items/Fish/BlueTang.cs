using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Fish
{
    public class BlueTang : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blue Tang");
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 59; // influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 0, 18, 0);
            Item.rare = ItemRarityID.Green;
        }

        public override bool IsQuestFish()
        {
            return true;
        }

        public override bool IsAnglerQuestAvailable()
        {
            return true;
        }

        public override void AnglerQuestChat(ref string description, ref string catchLocation)
        {
            description = "I've heard of some beautiful fish that make their homes in the reefs, and I'd like to get my hands one one!";
            catchLocation = "Caught in the Coral Reefs";
        }
    }
}