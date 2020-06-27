using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Placeables.Ores;
using EEMod.Projectiles.Melee;
using EEMod.Projectiles.Runes;
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
            item.damage = 40;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useAnimation = 18;
            item.useTime = 24;
            item.shootSpeed = 0;
            item.knockBack = 6.5f;
            item.width = 32;
            item.height = 32;
            item.scale = 1f;
            item.rare = ItemRarityID.Pink;
            item.value = Item.sellPrice(silver: 10);
            item.useAmmo = AmmoID.Arrow;

            item.ranged = true;
            item.noMelee = true; // Important because the spear is actually a projectile instead of an item. This prevents the melee hitbox of this item.
            item.autoReuse = true; // Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()

            item.UseSound = SoundID.Item1;
        }
    }
}
