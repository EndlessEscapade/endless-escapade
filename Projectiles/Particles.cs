using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles
{
    public class Particles : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubble");
        }

        public override void SetDefaults()
        {
            Projectile.width = 0;
            Projectile.height = 0;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.scale = 1.2f;
            Projectile.tileCollide = false;
            Projectile.light = 0;
            Projectile.timeLeft = 900;
        }

        private int sinControl;

        public override void AI()
        {
            Projectile.scale = Projectile.ai[0];
            Projectile.alpha = (int)Projectile.ai[1];
            Projectile.position.Y -= 1.5f;
            sinControl++;
            if (Projectile.ai[1] < 140)
            {
                Projectile.velocity.X += (float)Math.Sin(sinControl / (Projectile.ai[1] / 13)) / (Projectile.ai[1] / 2);
            }
            else if (Projectile.ai[1] < 160)
            {
                Projectile.position.X += (float)Math.Sin(sinControl / (Projectile.ai[1] / 13)) / (Projectile.ai[1] / 4);
            }
            else
            {
                Projectile.position.X -= (float)Math.Sin(sinControl / (Projectile.ai[1] / 13)) / Projectile.ai[1];
            }
        }

        public float flash;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            flash += 0.01f;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            Main.spriteBatch.Draw(ModContent.GetTexture("EEMod/Projectiles/Particles"), Projectile.Center - Main.screenPosition, null, lightColor * Math.Abs((float)Math.Sin(flash)) * 2, Projectile.rotation + flash, new Vector2(87), Projectile.ai[0], SpriteEffects.None, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}