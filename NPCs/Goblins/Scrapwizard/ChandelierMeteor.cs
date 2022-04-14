using EEMod.Extensions;
using EEMod.Prim;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.Goblins.Scrapwizard
{
    public class ChandelierMeteor : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hex Bolt");
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;

            Projectile.alpha = 0;

            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.scale = 1f;

            Projectile.aiStyle = -1;

            Projectile.tileCollide = true;

            Projectile.damage = 20;

            Projectile.timeLeft = 600;
        }

        public override void AI()
        {

        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //explode into flames or whatever

            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ShadowEmberTemp>(), 0, 0);

            Projectile.Kill();

            return true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(Main.graphics.GraphicsDevice.Viewport.Width / 2, Main.graphics.GraphicsDevice.Viewport.Height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(Main.GameViewMatrix.Zoom.X, Main.GameViewMatrix.Zoom.Y, 1f);

            Matrix projection = Matrix.CreateOrthographic(Main.graphics.GraphicsDevice.Viewport.Width, Main.graphics.GraphicsDevice.Viewport.Height, 0, 1000);

            Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, default, default, default, Main.GameViewMatrix.ZoomMatrix);

            EEMod.LightningShader.Parameters["maskTexture"].SetValue(EEMod.Instance.Assets.Request<Texture2D>("NPCs/Goblins/Scrapwizard/ScrapwizardHexBolt").Value);

            EEMod.LightningShader.Parameters["newColor"].SetValue(new Vector4(Color.Violet.R, Color.Violet.G, Color.Violet.B, Color.Violet.A) / 255f);

            EEMod.LightningShader.Parameters["transformMatrix"].SetValue(view * projection);

            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/ScrapwizardHexBolt").Value, Projectile.Center - Main.screenPosition, null, Color.Violet, 0f, new Vector2(12, 12), 1f, SpriteEffects.None, 0f);

            Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, default, default, default, Main.GameViewMatrix.ZoomMatrix);

            return false;
        }
    }
}