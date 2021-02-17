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

            projectile.rotation = Vector2.Normalize(player.Center - projectile.Center).ToRotation();
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

            var list = Main.projectile.Where(x => x.Hitbox.Intersects(projectile.Hitbox));
            foreach (var proj in list)
            {
                if (proj.type == ModContent.ProjectileType<SceptorPrism>() && proj.active && proj.ai[1] == 0)
                {
                    for (float i = -0.6f; i <= 0.6f; i += 0.3f)
                    {
                        int proj2 = Projectile.NewProjectile(proj.Center - (Vector2.UnitY.RotatedBy((double)i + projectile.rotation) * 60), 5 * Vector2.UnitY.RotatedBy((double)i + projectile.rotation), ModContent.ProjectileType<ShimmerShotProj1>(), projectile.damage, projectile.knockBack, projectile.owner, 0, 1);
                        EEMod.primitives.CreateTrail(new SceptorPrimTrailTwo(Main.projectile[proj2]));
                    }
                    projectile.timeLeft = 6;
                }
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