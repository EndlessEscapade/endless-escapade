using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Projectiles.Mage;
using Microsoft.Xna.Framework;

namespace EEMod.Items.Weapons.Mage
{
    public class Stalagtite : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stalagtite");
            Item.staff[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.autoReuse = true;
            item.noMelee = true;
            item.magic = true;

            item.mana = 12;
            item.useTime = 12;
            item.useAnimation = 12;
            item.rare = ItemRarityID.Lime;
            item.value = Item.sellPrice(0, 0, 80);
            item.width = 40;
            item.height = 40;
            item.useStyle = ItemUseStyleID.HoldingOut;

            item.shoot = ModContent.ProjectileType<StalagtiteProjectile>();
            item.shootSpeed = 9f;
            item.knockBack = 3.5f;

            item.UseSound = SoundID.Item88;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 target = Main.MouseWorld;
            float ceilingLimit = Math.Min(target.Y, player.Center.Y - 200);
            for (int i = 0; i < 2; i++)
            {
                position = player.Center + new Vector2(-Main.rand.NextFloat(401) * player.direction, -(600f + (100 * i)));
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
                speedY = heading.Y + Main.rand.NextFloat(-40 / 50f, 41 / 50f);
                Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage * 2, knockBack, player.whoAmI, 0f, ceilingLimit);
            }
            return false;
        }
    }
}