using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles
{
    public class Gradient : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grad");
        }

        public override void SetDefaults()
        {
            projectile.width = 200;
            projectile.height = 100;
            projectile.alpha = 0;
            projectile.timeLeft = 600;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale *= 1;
        }

        public override void AI()
        {
            float brightness = 1;
            projectile.timeLeft = 100;
            projectile.Center = Main.player[projectile.owner].Center + new Vector2(36, 0).RotatedBy(projectile.rotation);
            projectile.rotation = (Main.player[projectile.owner].Center - (new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition)).ToRotation() + (float)Math.PI;
            for (int i = 0; i < 10; i++)
            {
                Lighting.AddLight(projectile.Center + new Vector2(180 - (i * 20), 0).RotatedBy(projectile.rotation), new Vector3(projectile.ai[0] * brightness, projectile.ai[0] * brightness, projectile.ai[0] * brightness));
            }
        }

        public void pixelPlacmentHours()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            Main.spriteBatch.Draw(ModContent.GetTexture("EEMod/Projectiles/Gradient"), projectile.Center - Main.screenPosition, new Rectangle(0, 0, 200, 100), Color.White * 0.5f * projectile.ai[0], projectile.rotation, new Vector2(0, 50), 1, SpriteEffects.None, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
    }
}