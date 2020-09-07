using EEMod.Projectiles.Melee;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Melee
{
    public class HydrosEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydros Eye");
            Tooltip.SetDefault("Shoots bolts of water when enemies are near.");
        }

        public override void SetDefaults()
        {
            item.rare = ItemRarityID.Green;
            item.melee = true;
            item.width = 20;
            item.height = 20;
            item.useTime = 24;
            item.useAnimation = 24;
            item.knockBack = 3f;
            item.damage = 16;
            item.useStyle = ItemUseStyleID.HoldingOut;

            item.melee = true;
            item.channel = true;
            item.noMelee = true;
            item.noUseGraphic = true;

            item.UseSound = SoundID.Item1;
            item.shoot = ModContent.ProjectileType<HydrosEyeProjectile>();
        }
    }
}