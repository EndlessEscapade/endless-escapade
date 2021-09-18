using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Weapons.Mage;

namespace EEMod.Items.Weapons.Ranger.Guns
{
    public class Triggerfish : EEItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Triggerfish");
        }

        public override void SetDefaults()
        {
            Item.damage = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 24;
            Item.useTime = 24;
            Item.shootSpeed = 20f;
            Item.knockBack = 6.5f;
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(silver: 10);
            //item.useAmmo = AmmoID.Bullet;
            Item.shoot = ModContent.ProjectileType<CyanoburstTomeProjectile>();

            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true; // Important because the spear is actually a projectile instead of an item. This prevents the melee hitbox of this item.
            Item.autoReuse = true; // Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()

            Item.UseSound = SoundID.Item11;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }
    }
}