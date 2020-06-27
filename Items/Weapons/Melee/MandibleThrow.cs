using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Projectiles.Melee;

namespace EEMod.Items.Weapons.Melee
{
    public class MandibleThrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mandible Throw");
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
            item.shoot = ModContent.ProjectileType<MandibleThrowProjectile>();
        }
    }
}