using EEMod.Items.Placeables.Ores;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Ranger.Longbows
{
    public class ShimmerShot : EEItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Shimmer Shot");
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 2;
            Item.useTime = 24;
            Item.shootSpeed = 4;
            Item.knockBack = 6.5f;
            Item.width = 32;
            Item.height = 32;
            Item.scale = 1f;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(silver: 10);

            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            // Item.autoReuse = false;
            Item.ammo = AmmoID.Arrow;

            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<ShimmerShotProj>();
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
    }
}
