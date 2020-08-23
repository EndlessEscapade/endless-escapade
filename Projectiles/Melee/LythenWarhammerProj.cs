using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;
//using SharpDX.D3DCompiler;

namespace EEMod.Projectiles.Melee
{
    public class LythenWarhammerProj : WarHammer
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Trident Of The Depths");
        }

        public override void SetDefaults()
        {
            projectile.width = 50;
            projectile.height = 52;
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
        }
        public override float rotationalCoverage => (float)Math.PI;
        public override float RotationalOffset => (float)Math.PI / 2f;
        public override float dirtSmashIntensity => 12;
        public override int shakeLength => 20;
        public override int AoE => base.AoE;
        public override bool canCrash => true;
        public override float damageIncreaseOverTime => 0.05f;
        public override float weight => 2;
    }
}
