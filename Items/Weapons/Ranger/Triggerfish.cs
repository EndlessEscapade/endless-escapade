using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using EEMod.Projectiles.Mage;

namespace EEMod.Items.Weapons.Ranger
{
    public class Triggerfish : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Triggerfish");
        }

        public override void SetDefaults()
        {
            item.damage = 40;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useAnimation = 24;
            item.useTime = 24;
            item.shootSpeed = 20f;
            item.knockBack = 6.5f;
            item.width = 32;
            item.height = 32;
            item.rare = ItemRarityID.Green;
            item.value = Item.sellPrice(silver: 10);
            //item.useAmmo = AmmoID.Bullet;
            item.shoot = ModContent.ProjectileType<CyanoburstTomeProjectile>();

            item.ranged = true;
            item.noMelee = true; // Important because the spear is actually a projectile instead of an item. This prevents the melee hitbox of this item.
            item.autoReuse = true; // Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()

            item.UseSound = SoundID.Item11;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }
    }
}
