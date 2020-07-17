/*using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Items.Materials;
using EEMod.Projectiles.Ranged;

namespace EEMod.Items.Weapons.Ranger
{
    public class QuartzShotBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Quartz Shotbow");
            Tooltip.SetDefault("Splits arrows into two quartz arrows that gain speed and power the longer they travel");
        }

        public override void SetDefaults()
        {
            item.damage = 12;
            item.knockBack = 0.5f;
            item.width = 36;
            item.height = 60;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = ItemUseStyleID.HoldingOut; // Bow Use Style
            item.noMelee = true; // Doesn't deal damage if an enemy touches at melee range.
            item.value = Item.sellPrice(0, 1, 0, 0); // Another way to handle value of item.
            item.rare = ItemRarityID.Pink;
            item.useTurn = false;
            item.autoReuse = true;
            item.UseSound = SoundID.Item5;
            item.useAmmo = AmmoID.Arrow; // The ammo used with this weapon
            item.shoot = ModContent.ProjectileType<QuartzArrow>();
            item.shootSpeed = 1f;
            item.ranged = true; // For Ranged Weapon
            item.crit = 5;
        }

        public override bool ConsumeAmmo(Player player)
        {
            return !(player.itemAnimation < item.useAnimation - 2);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float spread = 22f * 0.0174f;
            float baseSpeed = (float)Math.Sqrt((speedX * speedX) + (speedY * speedY));
            double startAngle = Math.Atan2(speedX, speedY) - .1d;
            double deltaAngle = spread / 6f;
            if (player.itemAnimation != 1)
            {
                Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<QuartzArrow>(), damage, knockBack, player.whoAmI, 0.0f, 0.0f);
            }

            double offsetAngle = startAngle + deltaAngle;
            Projectile.NewProjectile(position.X, position.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), ModContent.ProjectileType<QuartzArrow>(), damage, knockBack, Main.myPlayer);

            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.PlatinumBow, 1);
            recipe.AddIngredient(ModContent.ItemType<QuartzGem>(), 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.GoldBow, 1);
            recipe.AddIngredient(ModContent.ItemType<QuartzGem>(), 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}*/
