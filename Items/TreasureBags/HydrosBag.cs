using EEMod.Items.Accessories;
using EEMod.Items.Materials;
using EEMod.Items.Weapons.Mage;
using EEMod.Items.Weapons.Melee;
using EEMod.Items.Weapons.Ranger;
using EEMod.Items.Weapons.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace EEMod.Items.TreasureBags
{
    public class HydrosBag : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 100;
        }

        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 34;
            Item.maxStack = 999;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Expert;
            Item.consumable = true;
            Item.UseSound = SoundID.Item2;
        }

        public override void RightClick(Player player)
        {
            /*switch (Main.rand.Next(5))
            {
                case 0:
                    Main.LocalPlayer.QuickSpawnItem(ItemType<HydrosEye>(), 1);
                    break;

                case 1:
                    Main.LocalPlayer.QuickSpawnItem(ItemType<CyanoburstTome>(), 1);
                    break;

                case 2:
                    Main.LocalPlayer.QuickSpawnItem(ItemType<Triggerfish>(), 1);
                    break;

                case 3:
                    Main.LocalPlayer.QuickSpawnItem(ItemType<Hydroshot>(), 1);
                    break;

                case 4:
                    Main.LocalPlayer.QuickSpawnItem(ItemType<EnchantedCoral>(), 1);
                    break;
            }*/
            Main.LocalPlayer.QuickSpawnItem(ItemType<HydrosScales>(), Main.rand.Next(42, 68));
            Main.LocalPlayer.QuickSpawnItem(ItemType<WaterDragonsScale>(), 1);
        }

        public override bool CanRightClick()
        {
            return true;
        }
    }
}