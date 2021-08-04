using Microsoft.Xna.Framework;
using System;

namespace EEMod.Items.Weapons.Melee.Warhammers
{
    public class BarrierGreatswordProj : WarHammer
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Barrier Greatsword");
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 52;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.alpha = 0;

            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.melee = true;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.damage = 20;
            Projectile.knockBack = 4.5f;
        }
        public override float rotationalCoverage => MathHelper.Pi;
        public override float RotationalOffset => MathHelper.PiOver2;
        public override float dirtSmashIntensity => 12;
        public override int shakeLength => 20;
        public override int AoE => base.AoE;
        public override bool canCrash => true;
        public override float damageIncreaseOverTime => 0.05f;
        public override float weight => 2;
    }
}