using EEMod.Items.Placeables.Ores;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
#pragma warning disable ChangeMagicNumberToID // Change magic numbers into appropriate ID values
            item.shoot = 10;
#pragma warning restore ChangeMagicNumberToID // Change magic numbers into appropriate ID values
            item.shootSpeed = 36f;
            item.rare = ItemRarityID.Orange;
            item.width = 20;
            item.height = 20;
            item.noMelee = true;
            item.ranged = true;
            item.damage = 20;
            item.useTime = 36;
            item.useAnimation = 36;
            item.value = Item.buyPrice(0, 0, 30, 0);
            item.autoReuse = false;
            item.knockBack = 6f;
            item.UseSound = SoundID.Item11;
            item.crit = 1;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int numberProjectiles = 3 + Main.rand.Next(2);
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(22));

                Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
            }
            player.velocity += -Vector2.Normalize(Main.MouseWorld - player.Center) * 8;
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<DalantiniumBar>(), 7);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddIngredient(ItemID.Boomstick, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}