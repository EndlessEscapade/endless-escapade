using EEMod.Items.Placeables.Ores;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Ranger.Launchers
{
    public class DalantiniumBohiya : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dalantinium Bo-Hiya");
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAmmo = AmmoID.Bullet;
            Item.shoot = ModContent.ProjectileType<DalantiniumSpike>();
            Item.shootSpeed = 16f;
            Item.rare = ItemRarityID.Orange;
            Item.width = 20;
            Item.height = 20;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 20;
            Item.useTime = 1;
            Item.useAnimation = 1;
            Item.value = Item.buyPrice(0, 0, 30, 0);
            Item.autoReuse = true;
            Item.knockBack = 6f;
            Item.UseSound = SoundID.Item11;
            Item.crit = 1;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }

        int chargeTime = 120;
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (chargeTime <= 0)
            {
                chargeTime = 120;
                Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_ItemUse(player, Item), position, new Vector2(speedX, speedY), ModContent.ProjectileType<DalantiniumSpike>(), damage, knockBack);
            }
            return false;
        }

        public override void HoldItem(Player player)
        {
            if (player.controlUseItem) if (chargeTime > 0) chargeTime--;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<DalantiniumBar>(), 7).AddTile(TileID.Anvils).Register();
        }
    }
}