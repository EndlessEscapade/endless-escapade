using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using InteritosMod.Projectiles.Mage;
using InteritosMod.Items.Placeables.Ores;

namespace InteritosMod.Items.Weapons.Mage
{
    public class HydrofluoricalStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydrofluorical Staff");
            Item.staff[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.melee = false;
            item.magic = true;
            item.width = 20;
            item.height = 20;
            item.mana = 15;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useTime = 20;
            item.useAnimation = 20;
            item.value = Item.sellPrice(0, 0, 30);
            item.rare = ItemRarityID.Yellow;
            item.shoot = ProjectileID.RuneBlast;
            item.shootSpeed = 15f;
            item.damage = 48;
            item.knockBack = 3;
            item.autoReuse = true;
        }

        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X,position.Y,speedX, speedY, ModContent.ProjectileType<HydrofluoricStaffProjectile>(), damage, 1, Main.myPlayer, Vector2.Normalize(new Vector2(speedX, speedY)).Y, Vector2.Normalize(new Vector2(speedX, speedY)).X);
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<HydrofluoricStaffProjectile>(), damage, 1, Main.myPlayer, -Vector2.Normalize(new Vector2(speedX, speedY)).Y, -Vector2.Normalize(new Vector2(speedX, speedY)).X);
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<HydroFluoricBar>(), 16);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}