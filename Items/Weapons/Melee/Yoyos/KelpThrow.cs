using EEMod.Items.Weapons.Melee;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Melee.Yoyos
{
    public class KelpThrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kelp Throw");
            Tooltip.SetDefault("Releases damaging baubles when close to enemes");
        }

        public override void SetDefaults()
        {
            item.rare = ItemRarityID.Green;
            item.melee = true;
            item.width = 44;
            item.height = 50;

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
            item.shoot = ModContent.ProjectileType<KelpThrowProjectile>();
        }
    }
}