using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.TreasureBags
{
    public class OmenBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            ItemID.Sets.SortingPriorityMaterials[item.type] = 100;
        }

        public override void SetDefaults()
        {
            item.width = 50;
            item.height = 34;
            item.maxStack = 999;
            item.value = Item.buyPrice(0, 1, 0, 0);
            item.rare = ItemRarityID.Expert;
            item.consumable = true;
            item.UseSound = SoundID.Item2;
        }

        public override void RightClick(Player player)
        {
            int randVal = Main.rand.Next(4);
            switch (randVal)
            {
                case 0:
                    //Main.LocalPlayer.QuickSpawnItem(ItemType<Shocktopus>(), 1);
                    break;
                case 1:
                    //Main.LocalPlayer.QuickSpawnItem(ItemType<KOKEN>(), 1);
                    break;
                case 2:
                    //Main.LocalPlayer.QuickSpawnItem(ItemType<EggPistol>(), 1);
                    break;
                case 3:
                    //Main.LocalPlayer.QuickSpawnItem(ItemType<Hydroshot>(), 1);
                    break;
            }
            //Main.LocalPlayer.QuickSpawnItem(ItemType<WaterDragonsScale>(), 1);
        }

        public override bool CanRightClick()
        {
            return true;
        }
    }
}