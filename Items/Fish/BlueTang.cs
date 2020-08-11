using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Projectiles.Ranged;

namespace EEMod.Items.Fish
{
    public class BlueTang : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blue Tang");
            ItemID.Sets.SortingPriorityMaterials[item.type] = 59; // influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 1;
            item.value = Item.buyPrice(0, 0, 18, 0);
            item.rare = ItemRarityID.Green;
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