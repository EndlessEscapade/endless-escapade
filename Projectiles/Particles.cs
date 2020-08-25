using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace EEMod.Projectiles
{
    public class Particles : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubble");
        }

        public override void SetDefaults()
        {
            projectile.width = 0;
            projectile.height = 0;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.scale = 1.2f;
            projectile.tileCollide = false;
            projectile.light = 0;
            projectile.timeLeft = 900;
        }
        int sinControl;
        public override void AI()
        {
            projectile.scale = projectile.ai[0];
            projectile.alpha = (int)projectile.ai[1];
            projectile.position.Y -= 1.5f;
            sinControl++;
            if (projectile.ai[1] < 140)
                projectile.velocity.X += (float)Math.Sin(sinControl / (projectile.ai[1] / 13)) / (projectile.ai[1] / 2);
            else if (projectile.ai[1] < 160)
                projectile.position.X += (float)Math.Sin(sinControl / (projectile.ai[1] / 13)) / (projectile.ai[1] / 4);
            else
                projectile.position.X -= (float)Math.Sin(sinControl / (projectile.ai[1] / 13)) / (projectile.ai[1]);
        }

        public float flash;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            flash += 0.01f;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            Main.spriteBatch.Draw(ModContent.GetTexture("EEMod/Projectiles/Particles"), projectile.Center - Main.screenPosition, null, lightColor * Math.Abs((float)Math.Sin(flash)) * 2, projectile.rotation + flash, new Vector2(87), projectile.ai[0], SpriteEffects.None, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin();
            return false;
        }
    }
}
