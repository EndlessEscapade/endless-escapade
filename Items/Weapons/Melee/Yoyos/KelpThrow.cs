using EEMod.Items.Weapons.Melee;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Melee.Yoyos
{
    public class KelpThrow : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kelp Throw");
            Tooltip.SetDefault("Releases damaging baubles when close to enemes");
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Green;
            Item.melee = true;
            Item.width = 44;
            Item.height = 50;

            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.knockBack = 3f;
            Item.damage = 16;
            Item.useStyle = ItemUseStyleID.HoldingOut;

            Item.melee = true;
            Item.channel = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<KelpThrowProjectile>();
        }
    }
}