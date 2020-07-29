using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Projectiles.Melee;

namespace EEMod.Items.Weapons.Melee
{
    public class FeatheredChakram : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Feathered Chakram");
        }

        public override void SetDefaults()
        {
            item.damage = 116;
            item.melee = true;
            item.noMelee = true;
            item.width = 44;
            item.height = 60;
            item.useTime = 15;
            item.useAnimation = 15;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 8;
            item.value = Item.buyPrice(0, 5, 0, 0);
            item.rare = ItemRarityID.Yellow;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<FeatheredChakramProjectileAlt>();
            item.shootSpeed = 16f;
            item.noUseGraphic = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(Main.LocalPlayer.Center, Vector2.Zero, ModContent.ProjectileType<FeatheredChakramProjectileAlt>(), item.damage, item.knockBack, ai0: MathHelper.Pi);
            Projectile.NewProjectile(Main.LocalPlayer.Center, Vector2.Zero, ModContent.ProjectileType<FeatheredChakramProjectileAlt>(), item.damage, item.knockBack, ai0: 0);
            return true;
        }
    }
}