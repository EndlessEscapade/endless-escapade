using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using EEMod.Prim;
using EEMod.Projectiles.Ranged;

namespace EEMod.Projectiles.Mage
{
    public class SceptorPrism : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aquamarine Prism");
        }

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 38;
            projectile.timeLeft = 1200;
            projectile.ignoreWater = true;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.penetrate = -1;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            projectile.rotation = Vector2.Normalize(player.Center - projectile.Center).ToRotation() - MathHelper.PiOver2;
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

            if (projectile.timeLeft > 32)
            {
                var list = Main.projectile.Where(x => Vector2.Distance(projectile.Center, x.Center) <= 24);
                foreach (var proj in list)
                {
                    if (proj.type == ModContent.ProjectileType<SceptorLaser>() && proj.active && proj.ai[0] == 0)
                    {
                        for (float i = -0.6f; i <= 0.6f; i += 0.4f)
                        {
                            Projectile proj2 = Projectile.NewProjectileDirect(proj.Center - (Vector2.UnitY.RotatedBy((i + Math.PI) + projectile.rotation) * 60), 3 * Vector2.UnitY.RotatedBy((i + Math.PI) + projectile.rotation), ModContent.ProjectileType<ShimmerShotProj1>(), projectile.damage, projectile.knockBack, projectile.owner, 0, 1);
                            EEMod.primitives.CreateTrail(new SpirePrimTrail(proj2, Color.Lerp(Color.Cyan, Color.Magenta, i / ((i + 0.6f) / 1.2f)), 40));
                        }
                        proj.Kill();
                        projectile.timeLeft = 32;
                    }
                }
            }
            else
            {
                projectile.alpha += 8;
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
            EEMod.PrismShader.Parameters["tentacle"].SetValue(ModContent.GetInstance<EEMod>().GetTexture("ShaderAssets/PrismLightMap"));
            EEMod.PrismShader.Parameters["lightColour"].SetValue(drawColor.ToVector3() * (1 / (1 + projectile.alpha)));
            EEMod.PrismShader.Parameters["prismColor"].SetValue(shadeColor.ToVector3() * (1 / (1 + projectile.alpha)));
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