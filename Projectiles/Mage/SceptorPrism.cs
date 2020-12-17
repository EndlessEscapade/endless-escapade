using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Mage
{
    public class SceptorPrism : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sceptor Prism");
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.timeLeft = 1200;
            projectile.ignoreWater = true;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.penetrate = -1;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            projectile.rotation = 3.14f;
            Vector2 posToBe = new Vector2(projectile.ai[0], projectile.ai[1]);
            Vector2 direction = posToBe - projectile.position;
            float speed = (float)Math.Sqrt(direction.Length()) / 2;
            if (speed > 0.1f)
            {
                direction.Normalize();
                direction *= speed;
                projectile.velocity = direction;
            }
            else
            {
                projectile.velocity = Vector2.Zero;
            }
        }
        private float alpha;
        float colorcounter = 0;
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            alpha += 0.05f;
            colorcounter += 0.05f;
            if (colorcounter > 6.28f)
            {
                colorcounter = 0;
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            Color shadeColor = Main.hslToRgb((colorcounter / 16.96f) + 0.46f, 1f, 0.7f);
            EEMod.PrismShader.Parameters["alpha"].SetValue(alpha * 2 % 6);
            EEMod.PrismShader.Parameters["shineSpeed"].SetValue(0.7f);
            EEMod.PrismShader.Parameters["tentacle"].SetValue(EEMod.instance.GetTexture("ShaderAssets/PrismLightMap"));
            EEMod.PrismShader.Parameters["lightColour"].SetValue(drawColor.ToVector3());
            EEMod.PrismShader.Parameters["prismColor"].SetValue(shadeColor.ToVector3());
            EEMod.PrismShader.Parameters["shaderLerp"].SetValue(1f);
            EEMod.PrismShader.CurrentTechnique.Passes[0].Apply();
            Vector2 drawOrigin = new Vector2(projectile.width / 2, projectile.height / 2);
            Vector2 drawPos = projectile.position - Main.screenPosition;
            shadeColor.A = 150;
            spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos + drawOrigin, null, shadeColor, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}