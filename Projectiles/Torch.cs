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
            Projectile.width = 46;
            Projectile.height = 16;
            Projectile.alpha = 0;
            Projectile.timeLeft = 1;
            Projectile.penetrate = -1;
            // Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            // Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale *= 1;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            Projectile.rotation = (Main.player[Projectile.owner].Center - (new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition)).ToRotation() + MathHelper.Pi;
        }
    }
}