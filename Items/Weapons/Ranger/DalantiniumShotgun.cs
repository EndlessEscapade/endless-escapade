using EEMod.Items.Placeables.Ores;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Projectiles.Ranged;

namespace EEMod.Items.Weapons.Ranger
{
    public class DalantiniumShotgun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dalantinium Shotgun");
        }

        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useAmmo = AmmoID.Bullet;
            item.shoot = ModContent.ProjectileType<DalantiniumSpike>();
            item.shootSpeed = 16f;
            item.rare = ItemRarityID.Orange;
            item.width = 20;
            item.height = 20;
            item.noMelee = true;
            item.ranged = true;
            item.damage = 20;
            item.useTime = 1;
            item.useAnimation = 1;
            item.value = Item.buyPrice(0, 0, 30, 0);
            item.autoReuse = true;
            item.knockBack = 6f;
            item.UseSound = SoundID.Item11;
            item.crit = 1;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }

        int chargeTime = 120;
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if(chargeTime <= 0)
            {
                chargeTime = 120;
                Projectile.NewProjectile(position, new Vector2(speedX, speedY), ModContent.ProjectileType<DalantiniumSpike>(), damage, knockBack);
            }
            return false;
        }

        public override void HoldItem(Player player)
        {
            if(player.controlUseItem) if(chargeTime > 0) chargeTime--;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<DalantiniumBar>(), 7);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}