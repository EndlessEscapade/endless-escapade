using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Projectiles.Ranged;

namespace EEMod.Items.Fish
{
    public class Ironfin : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ironfin");
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
            description = "I heard that the once-clear waters of the reefs have been tainted by oil... Well, more fish for me! Now go!";
            catchLocation = "Caught in the Coral Reefs";
        }
    }
}