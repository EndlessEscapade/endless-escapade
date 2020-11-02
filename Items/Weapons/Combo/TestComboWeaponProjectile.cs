using EEMod.Projectiles;
using EEMod.Projectiles.Mage;
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
            projectile.Center = projOwner.Center;
            projectile.rotation += 0.1f;
            if (isFinished)
                projectile.Kill();
        }
        public void Attack2()
        {
            projectile.Center = projOwner.Center;
            projectile.rotation -= 0.1f;
            if (isFinished)
                projectile.Kill();
        }
        public override void SetDefaults()
        {
            projectile.width = 42;
            projectile.height = 50;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.scale = 1f;
            projectile.alpha = 0;

            projectile.hide = true;
            projectile.ownerHitCheck = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.damage = 20;
            projectile.knockBack = 4.5f;
            AddCombo(0, Attack1);
            AddCombo(1, Attack2);
        }
    }
}