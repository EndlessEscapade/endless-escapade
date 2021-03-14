using EEMod.Items.Weapons.Mage;
using EEMod.Items.Weapons.Melee;
using EEMod.Items.Weapons.Ranger;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace EEMod.Items.TreasureBags
{
    public class TalosBag : ModItem
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
            /*switch (Main.rand.Next(4))
            {
                case 0:
                    Main.LocalPlayer.QuickSpawnItem(ItemType<FeatheredDreamcatcher>(), 1);
                    break;

                case 1:
                    Main.LocalPlayer.QuickSpawnItem(ItemType<FeatheredChakram>(), 1);
                    break;

                case 2:
                    Main.LocalPlayer.QuickSpawnItem(ItemType<EggPistol>(), 1);
                    break;

                case 3:
                    //Main.LocalPlayer.QuickSpawnItem(ItemType<WaterDragonsScale>(), 1);
                    break;
            }*/
        }

        public override bool CanRightClick()
        {
            return true;
        }
    }
}