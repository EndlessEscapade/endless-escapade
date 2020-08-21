using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.Placeables.Ores;
using EEMod.Projectiles.Melee;
using EEMod.Projectiles.Runes;
using EEMod.Tiles;
using EEMod.Projectiles;
using Microsoft.Xna.Framework;

namespace EEMod.Items.Weapons.Melee
{
    public class TridentOfTheDepths : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Trident of the Depths");
        }

        public override void SetDefaults()
        {
            item.damage = 40;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useAnimation = 18;
            item.useTime = 24;
            item.shootSpeed = 0f;
            item.knockBack = 6.5f;
            item.width = 32;
            item.height = 32;
            item.scale = 1f;
            item.rare = ItemRarityID.Purple;
            item.value = Item.sellPrice(silver: 10);

            item.melee = true;
            item.noMelee = true; // Important because the spear is actually a projectile instead of an item. This prevents the melee hitbox of this item.
            item.noUseGraphic = true; // Important, it's kind of wired if people see two spears at one time. This prevents the melee animation of this item.
            item.autoReuse = true; // Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()

            item.UseSound = SoundID.Item1;
            item.shoot = ModContent.ProjectileType<TridentOfTheDepthsProjectile>();
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[item.shoot] < 1;
        }

        public override bool UseItem(Player player)
        {
            item.shootSpeed = 4f;
            item.shoot = ModContent.ProjectileType<TridentOfTheDepthsProjectile>();
            return player.ownedProjectileCounts[item.shoot] < 1;
        }

        public override bool AltFunctionUse(Player player)
        {
            item.shootSpeed = 0f;
            item.shoot = ModContent.ProjectileType<TridentOfTheDepthsAltProjectile>();
            return player.ownedProjectileCounts[item.shoot] < 1;
        }
    }
}
