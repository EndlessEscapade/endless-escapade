using System;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Projectiles
{
    public class Torch : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grad");
        }

        public override void SetDefaults()
        {
            projectile.width = 46;
            projectile.height = 16;
            projectile.alpha = 0;
            projectile.timeLeft = 1;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale *= 1;
            projectile.alpha = 255;
        }

        public override void AI()
        {
            projectile.rotation = (Main.player[projectile.owner].Center - (new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition)).ToRotation() + (float)Math.PI;
        }
    }
}
