using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Projectiles.Ranged
{
    public class LythenHandgunProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lythen Coral");
        }

        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
            projectile.alpha = 0;
            projectile.timeLeft = 600;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.scale *= 1f;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 360; i += 10)
            {
                float xdist = (int)(Math.Sin(i * (Math.PI / 180)) * Math.Cos(i / 10) * 20);
                float ydist = (int)(Math.Cos(i * (Math.PI / 180)) * Math.Sin(i / 10) * 20);
                Vector2 offset = new Vector2(xdist, ydist).RotatedBy(projectile.rotation);
                Dust dust = Dust.NewDustPerfect(projectile.Center + offset, 111, offset * 0.5f);
                dust.noGravity = true;
                dust.velocity *= 0.94f;
                dust.noLight = false;
                dust.fadeIn = 1f;
            }
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            projectile.ai[0]++;

            //brainstorming ideas is fun fun fun fun fun :D

            projectile.ai[1]++;

            float radius = 48 + (12 * (float)Math.Sin(projectile.ai[1] / 10));
            Vector2 position = projectile.Center + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / projectile.ai[1] % 32)) * radius;
            Dust dust = Dust.NewDustPerfect(position, 111);
            dust.velocity = Vector2.Zero;
            dust.noGravity = true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().GetTexture("Masks/Extra_49"), projectile.Center - Main.screenPosition, null, new Color(97, 215, 248, 0), 0f, new Vector2(50, 50), 0.25f * (float)Math.Sin(projectile.ai[0] / 10) + 0.75f, SpriteEffects.None, 0f);
            return base.PreDraw(spriteBatch, lightColor);
        }
    }
}