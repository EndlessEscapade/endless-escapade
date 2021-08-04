using EEMod.Items.Placeables.Ores;
using EEMod.Items.Weapons.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.Items.Weapons.Melee.Warhammers
{
    public class ShroomSmasher : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shroom Smasher");
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.useStyle = ItemUseStyleID.HoldingOut;
            Item.useAnimation = 60;
            Item.useTime = 60;
            Item.shootSpeed = 16f;
            Item.knockBack = 6.5f;
            Item.width = 32;
            Item.height = 32;
            Item.scale = 1f;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(silver: 10);

            Item.melee = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = false;

            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<HydrofluoricWarhammerProj>();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 0)
            {
                type = ModContent.ProjectileType<HydrofluoricWarhammerProj>();
                speedX = 0;
                speedY = 0;
                Item.shoot = ModContent.ProjectileType<HydrofluoricWarhammerProj>();
            }
            if (player.altFunctionUse == 2)
            {
                type = ModContent.ProjectileType<HydrofluoricWarhammerProjAlt>();
                Item.shoot = ModContent.ProjectileType<HydrofluoricWarhammerProjAlt>();
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
    }
}