using EEMod.Items.Weapons.Mage;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Mage
{
    public class CyanoburstTome : EEItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cyanoburst Tome");
        }

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.knockBack = 0;
            Item.rare = ItemRarityID.Green;
            Item.autoReuse = false;
            Item.crit = 4;
            Item.noMelee = true;
            Item.magic = true;
            Item.shoot = ModContent.ProjectileType<CyanoburstTomeProjectile>();
            Item.shootSpeed = 16f;
            Item.mana = 2;
            Item.UseSound = SoundID.DD2_MonkStaffSwing;
            Item.useStyle = ItemUseStyleID.HoldingOut;
        }
    }
}