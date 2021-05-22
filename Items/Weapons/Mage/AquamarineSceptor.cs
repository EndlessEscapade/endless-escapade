using EEMod.Extensions;
using EEMod.Items.Placeables.Ores;
using EEMod.Items.Weapons.Mage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Prim;

namespace EEMod.Items.Weapons.Mage
{
    public class AquamarineSceptor : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aquamarine Sceptre");
            Item.staff[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.damage = 57;
            item.magic = true;
            item.noMelee = true;
            item.knockBack = 1f;
            item.value = Item.sellPrice(0, 0, 21);
            item.mana = 15;
            item.useTime = 45;
            item.useAnimation = 45;
            item.rare = ItemRarityID.Orange;
            item.width = 20;
            item.height = 20;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<SceptorLaser>();
            item.shootSpeed = 6f;
            item.UseSound = SoundID.Item115;
        }

        public override Vector2? HoldoutOffset()
        {
            return Vector2.Zero;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            //Helpers.TexToDust("thonk",Main.MouseWorld,5,1,100);
            if (player.altFunctionUse == 0)
            {
                Vector2 comedy = Vector2.Normalize(Main.MouseWorld - player.Center) * 2;
                Projectile projectile2 = Projectile.NewProjectileDirect(player.Center, comedy, ModContent.ProjectileType<SceptorLaser>(), 10, 10f, Main.myPlayer);

                EEMod.primitives.CreateTrail(new SpirePrimTrail(projectile2, Color.Lerp(Color.Cyan, Color.Magenta, Main.rand.NextFloat(0, 1)), 40));
            }
            if (player.altFunctionUse == 2)
            {
                type = ModContent.ProjectileType<SceptorPrism>();
                if (player.ownedProjectileCounts[ModContent.ProjectileType<SceptorPrism>()] < 1)
                {
                    position = Main.MouseWorld;
                    speedX = 0;
                    speedY = 0;
                    Projectile projectile = Projectile.NewProjectileDirect(player.Center, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI, position.X, position.Y);
                }
            }

            return false;

        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
    }
}
