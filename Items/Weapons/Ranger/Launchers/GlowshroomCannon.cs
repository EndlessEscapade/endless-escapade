using EEMod.Items.Placeables.Ores;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Ranger.Launchers
{
    public class GlowshroomCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glowshroom Cannon");
        }

        public override void SetDefaults()
        {
            item.melee = false;
            item.noMelee = true;
            item.autoReuse = true;
            item.value = Item.sellPrice(0, 0, 18);
            item.damage = 15;
            item.useTime = 26;
            item.useAnimation = 26;
            item.width = 56;
            item.height = 22;
            item.rare = ItemRarityID.Green;
            item.knockBack = 5f;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shootSpeed = 17f;
            item.UseSound = SoundID.Item11;
            item.useAmmo = AmmoID.Bullet;
            item.shoot = 10;
            item.ranged = true;
            item.crit = 3;
        }

        /*public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            type = ModContent.ProjectileType<GlowshroomCannonProj>();
            return true;
        }*/

        public override Vector2? HoldoutOffset()
        {
            return Vector2.Zero;
        }
    }
}