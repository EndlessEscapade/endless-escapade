using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Items.Materials;
using EEMod.Items.Weapons.Mage;
using EEMod.Items.Weapons.Melee;
using EEMod.Items.Weapons.Ranger;
using System;
using EEMod.NPCs.Bosses.Hydros;
using EEMod.Projectiles.CoralReefs;
using EEMod.Items.Weapons.Summon;
using static Terraria.ModLoader.ModContent;
using EEMod.Items.TreasureBags;
using EEMod.Items.Accessories;

namespace EEMod.Items.TreasureBags
{
    public class AkumoBag : ModItem
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
            if (randVal == 0)
            {
                Main.LocalPlayer.QuickSpawnItem(ItemType<FeatheredDreamcatcher>(), 1);
            }
            else if (randVal == 1)
            {
                Main.LocalPlayer.QuickSpawnItem(ItemType<FeatheredChakram>(), 1);
            }
            else if (randVal == 2)
            {
                Main.LocalPlayer.QuickSpawnItem(ItemType<EggPistol>(), 1);
            }
            else if (randVal == 3)
            {
                //Main.LocalPlayer.QuickSpawnItem(ItemType<Hydroshot>(), 1);
            }
            //Main.LocalPlayer.QuickSpawnItem(ItemType<WaterDragonsScale>(), 1);
        }

        public override bool CanRightClick()
        {
            return true;
        }
    }
}