using EEMod.Items.Placeables.Ores;
using EEMod.Projectiles.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.Items.Weapons.Melee
{
    public class HydrofluoricWarhammer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydrofluoric Warhammer");
        }

        public override void SetDefaults()
        {
            item.damage = 20;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useAnimation = 60;
            item.useTime = 60;
            item.shootSpeed = 16f;
            item.knockBack = 6.5f;
            item.width = 32;
            item.height = 32;
            item.scale = 1f;
            item.rare = ItemRarityID.Purple;
            item.value = Item.sellPrice(silver: 10);

            item.melee = true;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.autoReuse = false;

            item.UseSound = SoundID.Item1;
            item.shoot = ModContent.ProjectileType<HydrofluoricWarhammerProj>();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 0)
            {
                type = ModContent.ProjectileType<HydrofluoricWarhammerProj>();
                speedX = 0;
                speedY = 0;
                item.shoot = ModContent.ProjectileType<HydrofluoricWarhammerProj>();
            }
            if (player.altFunctionUse == 2)
            {
                type = ModContent.ProjectileType<HydrofluoricWarhammerProjAlt>();
                item.shoot = ModContent.ProjectileType<HydrofluoricWarhammerProjAlt>();
            }
            Projectile projectile = Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
            return false;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<HydrofluoricWarhammerProj>()] <= 0 || player.ownedProjectileCounts[ModContent.ProjectileType<HydrofluoricWarhammerProjAlt>()] <= 0;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<HydrofluoricBar>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
