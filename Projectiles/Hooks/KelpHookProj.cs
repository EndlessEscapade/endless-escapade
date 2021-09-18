using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System;

namespace EEMod.Projectiles.Hooks
{
    public class KelpHookProj : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kelp Hook");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 10;
            Projectile.alpha = 0;
            Projectile.timeLeft = 60000;
            Projectile.penetrate = -1;
            // Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.scale = 1f;
        }

        private bool hooked;

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            hooked = true;
            Projectile.velocity = Vector2.Zero;
            return false;
        }

        private int tileRange = 16;
        public override void AI()
        {
            if (hooked)
            {
                Player player = Main.player[Projectile.owner];

                Projectile.rotation = Vector2.Normalize(player.Center - Projectile.Center).ToRotation() - MathHelper.PiOver2;

                if (Vector2.Distance(player.Center, Projectile.Center) > 16 * tileRange)
                {
                    player.velocity += Vector2.Normalize(Projectile.Center - player.Center) * Helpers.Clamp((float)Math.Pow(1.2, (Vector2.Distance(player.Center, Projectile.Center) - (16 * tileRange)) / 4f), 0f, 3f);
                }
                if (Vector2.Distance(player.Center, Projectile.Center) > 16 * tileRange * 2f)
                {
                    Projectile.Kill();
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            Vector2 vec = Projectile.Center - player.Center;

            Texture2D vine = Mod.Assets.Request<Texture2D>("Projectiles/Hooks/KelpHookVine").Value;

            float n = vec.Length();

            for (float k = 0; k < n; k += 16)
            {
                Rectangle rect = new Rectangle(0, 0, 16, 16);

                spriteBatch.Draw(vine, Projectile.Center + (-Vector2.Normalize(vec) * k) - Main.screenPosition, rect, Color.White, vec.ToRotation() - MathHelper.PiOver2, rect.Size() / 2f, 1f, SpriteEffects.None, 0f);
            }

            return true;
        }
    }
}