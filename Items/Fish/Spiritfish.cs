using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Projectiles.Ranged;

namespace EEMod.Items.Fish
{
    public class Spiritfish : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spiritfish");
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
            description = "Legends tell that there's some sort of old pirate captain who lives in the reefs, and that he has some pet fish! I wanna see what the look like!";
            catchLocation = "Caught in the Coral Reefs";
        }
    }
}