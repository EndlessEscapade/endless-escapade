using EEMod.Items.Weapons.Classes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Combo
{
    public class TestComboWeaponProjectile : ComboWeapon
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Test Combo Weapon");
        }
        public void Attack1()
        {
            Projectile.Center = projOwner.Center;
            Projectile.rotation += 0.1f;
            if (isFinished)
                Projectile.Kill();
        }
        public void Attack2()
        {
            Projectile.Center = projOwner.Center;
            Projectile.rotation -= 0.1f;
            if (isFinished)
                Projectile.Kill();
        }
        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 50;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.alpha = 0;

            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            // Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.damage = 20;
            Projectile.knockBack = 4.5f;
            AddCombo(0, Attack1);
            AddCombo(1, Attack2);
        }
    }
}