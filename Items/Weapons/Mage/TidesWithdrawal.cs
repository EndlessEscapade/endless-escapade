using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Items.Placeables.Ores;
using EEMod.Projectiles.Mage;

namespace EEMod.Items.Weapons.Mage
{
    public class TidesWithdrawal : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tide's Withdrawal");
            Item.staff[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.damage = 12;
            item.width = 32;
            item.height = 32;
            item.useTime = 9;
            item.useAnimation = 9;
            item.knockBack = 1;
            item.rare = ItemRarityID.Green;
            item.autoReuse = true;
            item.noMelee = true;
            item.magic = true;
            item.shoot = ModContent.ProjectileType<TidesWithdrawalProjectile>();
            item.shootSpeed = 22f;
            item.mana = 8;
            item.UseSound = SoundID.Item1;
            item.useStyle = ItemUseStyleID.HoldingOut;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, -4);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position += new Vector2(100, 100);
            Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
            float ceilingLimit = target.Y;
            if (ceilingLimit > player.Center.Y - 200f)
            {
                ceilingLimit = player.Center.Y - 200f;
            }
            for (int i = 0; i < 2; i++)
            {
                position = player.Center + new Vector2(-(float)Main.rand.Next(401) * player.direction, -600f);
                position.Y -= 100 * i;
                Vector2 heading = target - position;
                if (heading.Y < 0f)
                {
                    heading.Y *= -1f;
                }
                if (heading.Y < 20f)
                {
                    heading.Y = 20f;
                }
                heading.Normalize();
                heading *= new Vector2(speedX, speedY).Length();
                speedX = heading.X;
                speedY = heading.Y + Main.rand.Next(-40, 41) * 0.02f;
                Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage * 2, knockBack, player.whoAmI, 0f, ceilingLimit);
            }
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<HydriteBar>(), 11);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
