using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles
{
    public class Torch : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grad");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 14;
            Projectile.alpha = 0;
            Projectile.timeLeft = 1000000;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.damage = 20;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 1;
            Projectile.alpha = 0;
        }

        public override void AI()
        {
            Projectile.rotation = (Main.player[Projectile.owner].Center - (new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition)).ToRotation() + MathHelper.Pi;

            Projectile.Center = Main.player[Projectile.owner].Center + Vector2.UnitX.RotatedBy(Projectile.rotation) * 20f;

            if (!Main.player[Projectile.owner].controlUseItem) Projectile.Kill();
        }
    }
}