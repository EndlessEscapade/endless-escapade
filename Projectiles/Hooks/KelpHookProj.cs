using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System;

namespace EEMod.Projectiles.Hooks
{
    public class KelpHookProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kelp Hook");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 10;
            projectile.alpha = 0;
            projectile.timeLeft = 60000;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.scale = 1f;
        }

        private bool hooked;

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            hooked = true;
            projectile.velocity = Vector2.Zero;
            return false;
        }

        private int tileRange = 16;
        public override void AI()
        {
            if (hooked)
            {
                Player player = Main.player[projectile.owner];

                projectile.rotation = Vector2.Normalize(player.Center - projectile.Center).ToRotation() - MathHelper.PiOver2;

                if (Vector2.Distance(player.Center, projectile.Center) > 16 * tileRange)
                {
                    player.velocity += Vector2.Normalize(projectile.Center - player.Center) * Helpers.Clamp((float)Math.Pow(1.2, (Vector2.Distance(player.Center, projectile.Center) - (16 * tileRange)) / 4f), 0f, 3f);
                }
                if (Vector2.Distance(player.Center, projectile.Center) > 16 * tileRange * 2f)
                {
                    projectile.Kill();
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (hooked)
            {
                Player player = Main.player[projectile.owner];

                Vector2 vec = projectile.Center - player.Center;

                Texture2D vine = mod.GetTexture("Projectiles/Hooks/KelpHookVine");

                float n = vec.Length();

                for (float k = 0; k < n; k += 16)
                {
                    Rectangle rect = new Rectangle(0, 0, 16, 16);

                    spriteBatch.Draw(vine, projectile.Center + (-Vector2.Normalize(vec) * k) - Main.screenPosition, rect, Color.White, vec.ToRotation() - MathHelper.PiOver2, rect.Size() / 2f, 1f, SpriteEffects.None, 0f);
                }
            }

            return true;
        }
    }
}