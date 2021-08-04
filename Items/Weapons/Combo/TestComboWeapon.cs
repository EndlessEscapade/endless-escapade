using EEMod.Items.Weapons.Classes;
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
            Item.damage = 20;
            Item.useStyle = ItemUseStyleID.HoldingOut;
            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.shootSpeed = 12f;
            Item.knockBack = 6.5f;
            Item.width = 32;
            Item.height = 32;
            Item.scale = 1f;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(silver: 10);

            Item.melee = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;

            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<TestComboWeaponProjectile>();
        }
    }
}