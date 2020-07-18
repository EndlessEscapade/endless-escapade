using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Projectiles.Ranged;

namespace EEMod.Items
{
    public class CoralArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coral Arrow");
            ItemID.Sets.SortingPriorityMaterials[item.type] = 59; // influences the inventory sort order. 59 is PlatinumBar, higher is more valuable.
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 999;
            item.value = Item.buyPrice(0, 0, 18, 0);
            item.rare = ItemRarityID.Green;
            item.consumable = true;
            item.ranged = true;
            item.damage = 12;
            item.shoot = ModContent.ProjectileType<CoralArrowProjectile>();
            item.ammo = AmmoID.Arrow;
        }
    }
}