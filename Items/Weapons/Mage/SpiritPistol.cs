using EEMod.Items.Weapons.Mage;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Mage
{
    public class SpiritPistol : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spirit Pistol");
        }

        public override void SetDefaults()
        {
            item.damage = 10;
            item.width = 32;
            item.height = 32;
            item.useTime = 30;
            item.useAnimation = 30;
            item.knockBack = 0;
            item.rare = ItemRarityID.Green;
            item.autoReuse = false;
            item.crit = 4;
            item.noMelee = true;
            item.magic = true;
            item.shoot = ModContent.ProjectileType<SpiritPistolProjectileMain>();
            item.shootSpeed = 16f;
            item.mana = 2;
            item.UseSound = SoundID.Item1;
            item.useStyle = ItemUseStyleID.HoldingOut;
        }
    }
}