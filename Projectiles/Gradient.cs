using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles
{
    public class Gradient : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grad");
        }

        public override void SetDefaults()
        {
            Projectile.width = 200;
            Projectile.height = 100;
            Projectile.alpha = 0;
            Projectile.timeLeft = 600;
            Projectile.penetrate = -1;
            // Projectile.hostile = false;
            // Projectile.friendly = false;
            // Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale *= 1;
        }

        public override void AI()
        {
            float brightness = 1;
            Projectile.timeLeft = 100;
            Projectile.Center = Main.player[Projectile.owner].Center + new Vector2(36, 0).RotatedBy(Projectile.rotation);
            Projectile.rotation = (Main.player[Projectile.owner].Center - (new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition)).ToRotation() + MathHelper.Pi;
            for (int i = 0; i < 10; i++)
            {
                Lighting.AddLight(Projectile.Center + new Vector2(180 - (i * 20), 0).RotatedBy(Projectile.rotation), new Vector3(Projectile.ai[0] * brightness, Projectile.ai[0] * brightness, Projectile.ai[0] * brightness));
            }
        }

        public void pixelPlacmentHours()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Projectiles/Gradient").Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 200, 100), Color.White * 0.5f * Projectile.ai[0], Projectile.rotation, new Vector2(0, 50), 1, SpriteEffects.None, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}