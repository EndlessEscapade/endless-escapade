using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Placeables.Ores;
using EEMod.Projectiles.Melee;
using EEMod.Projectiles.Runes;
using Microsoft.Xna.Framework;

namespace EEMod.Items.Weapons.Ranger
{
    public class TideWeaver : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Tide Weaver");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Tsunami);
            item.damage = 40;
            item.useAnimation = 18;
            item.useTime = 18;
            item.knockBack = 6.5f;
            item.width = 32;
            item.height = 32;
            item.rare = ItemRarityID.Purple;
            item.value = Item.sellPrice(silver: 10);

            item.ranged = true;
            item.noMelee = true; // Important because the spear is actually a projectile instead of an item. This prevents the melee hitbox of this item.
        }
    }
}
