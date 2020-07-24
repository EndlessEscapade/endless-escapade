using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Projectiles.Mage;

namespace EEMod.Items.Weapons.Mage
{
    public class AbyssalSceptre : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abyssal Sceptre");
            Item.staff[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.damage = 8;
            item.width = 32;
            item.height = 32;
            item.useTime = 30;
            item.useAnimation = 30;
            item.knockBack = 0;
            item.rare = ItemRarityID.Green;
            item.autoReuse = true;
            item.crit = 4;
            item.noMelee = true;
            item.magic = true;
            item.shoot = ModContent.ProjectileType<AbyssalSceptreProjectile>();
            item.shootSpeed = 16f;
            item.mana = 2;
            item.UseSound = SoundID.Item1;
            item.useStyle = ItemUseStyleID.HoldingOut;
        }
    }
}
