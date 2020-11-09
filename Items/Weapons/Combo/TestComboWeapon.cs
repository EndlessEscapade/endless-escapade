using EEMod.Projectiles;
using EEMod.Projectiles.Mage;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Combo
{
    public class TestComboWeapon : ComboWeaponItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scythe");
        }

        public override void ComboChangeBehaviour()
        {
            if (CurrentCombo == 0)
            {
                CurrentCombo = 1;
                return;
            }
            if (CurrentCombo == 1)
            {
                CurrentCombo = 0;
            }
        }
        public override int NumberOfCombinations => 2;
        public override int ComboProjectile => ModContent.ProjectileType<TestComboWeaponProjectile>();
        public override void SetDefaults()
        {
            item.damage = 20;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useAnimation = 40;
            item.useTime = 40;
            item.shootSpeed = 12f;
            item.knockBack = 6.5f;
            item.width = 32;
            item.height = 32;
            item.scale = 1f;
            item.rare = ItemRarityID.Purple;
            item.value = Item.sellPrice(silver: 10);

            item.melee = true;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.autoReuse = true;

            item.UseSound = SoundID.Item1;
            item.shoot = ModContent.ProjectileType<TestComboWeaponProjectile>();
        }
    }
}