using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Projectiles.Mage;
using EEMod.Items.Placeables.Ores;

namespace EEMod.Items.Weapons.Mage
{
    public class HydrofluoricStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydrofluoric Staff");
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
            item.rare = ItemRarityID.Lime;
            item.shoot = ProjectileID.RuneBlast;
            item.UseSound = SoundID.DD2_MonkStaffSwing;
            item.shootSpeed = 2f;
            item.damage = 48;
            item.knockBack = 3;
            item.autoReuse = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            speedX *= 0.2f;
            speedY *= 0.2f;
            Vector2 vector = Vector2.Normalize(new Vector2(speedX, speedY));
            position += vector * 60;
            vector *= 3;
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<HydrofluoricStaffProjectile>(), damage, 1, Main.myPlayer, vector.Y, vector.X);
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<HydrofluoricStaffProjectile>(), damage, 1, Main.myPlayer, -vector.Y, -vector.X);
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<HydrofluoricBar>(), 16);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}