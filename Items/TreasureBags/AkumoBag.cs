using EEMod.Items.Weapons.Mage;
using EEMod.Items.Weapons.Melee.Boomerangs;
using EEMod.Items.Weapons.Ranger;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using EEMod.Items.Weapons.Ranger.Launchers;

namespace EEMod.Items.TreasureBags
{
    public class AkumoBag : EEItem
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
            switch (Main.rand.Next(4))
            {
                case 0:
                    Main.LocalPlayer.QuickSpawnItem(ItemType<FeatheredDreamcatcher>(), 1);
                    break;

                case 1:
                    Main.LocalPlayer.QuickSpawnItem(new Terraria.DataStructures.EntitySource_ItemUse(player, Item), ItemType<FeatheredChakram>(), 1);
                    break;

                case 2:
                    Main.LocalPlayer.QuickSpawnItem(ItemType<EggPistol>(), 1);
                    break;

                case 3:
                    //Main.LocalPlayer.QuickSpawnItem(ItemType<WaterDragonsScale>(), 1);
                    break;
            }
        }

        public override bool CanRightClick()
        {
            return true;
        }
    }
}