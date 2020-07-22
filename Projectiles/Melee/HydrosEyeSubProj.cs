using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Melee
{
    public class HydrosEyeSubProj : ModProjectile
    {

        public static short customGlowMask = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydros Eye");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 200;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.extraUpdates = 1;
            projectile.damage = 10;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }
        public override void AI()
        {
            projectile.velocity = projectile.velocity.RotatedBy(Math.PI / 180) * 0.99f;
            projectile.ai[0]++;
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Main.spriteBatch.Draw(ModContent.GetTexture("EEMod/Projectiles/Particles"), projectile.Center - Main.screenPosition, new Rectangle(0,0,16,16), new Color(4,2,18f, 0), projectile.rotation, new Vector2(8), 0.5f, SpriteEffects.None, 0);
            return false;
        }
        public override void Kill(int timeLeft)
        {
            for (var a = 0; a < 2; a++)
            {
                int index = Dust.NewDust(projectile.Center, 22, 22, 16, 0, 0);
                Main.dust[index].noGravity = true;
            }
        }
    }
}
