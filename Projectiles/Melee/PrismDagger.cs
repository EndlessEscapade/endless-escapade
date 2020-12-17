using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Melee
{
    public class PrismDagger : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Prism Dagger");
        }

        public override void SetDefaults()
        {
            projectile.width = 46;
            projectile.height = 48;
            projectile.aiStyle = -1;
            projectile.penetrate = 1;
            projectile.scale = 1f;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.damage = 20;
            projectile.knockBack = 4.5f;
            projectile.alpha = 100;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }
        private bool launched = false;

        public override void AI()
        {

            Player player = Main.player[projectile.owner];
            if (projectile.ai[1] == 0)
            {
                EEMod.Particles.AppendSpawnModule("Main", new SpawnRandomly(1f));
                for (int i = 0; i < 5; i++)
                    EEMod.Particles.Get("Main").SpawnParticles(projectile.Center, null, 3, Main.hslToRgb((projectile.ai[0] / 16.96f) + 0.46f, 1f, 0.7f), new Spew(6.14f, 1f, Vector2.One, 0.95f), new RotateVelocity(Main.rand.NextFloat(-0.03f, 0.03f)), new AfterImageTrail(1.5f));
            }
            projectile.ai[1] = (Main.GameUpdateCount / 60f * 6.28f) + projectile.ai[0];
            if (!projectile.friendly)
            {

                Vector2 circle = new Vector2(40 * (float)Math.Sin((double)projectile.ai[1]), 20 * (float)Math.Cos((double)projectile.ai[1]) + 50);
                Vector2 mouseToPlayer = Main.MouseWorld - player.Center;
                circle = circle.RotatedBy(mouseToPlayer.ToRotation() + 1.57);
                Vector2 posToBe = player.Center + circle - new Vector2(24, 24);
                Vector2 direction = posToBe - projectile.position;
                float speed = (float)Math.Sqrt(direction.Length()) / 2;
                direction.Normalize();
                direction *= speed;
                projectile.velocity = direction;
                Vector2 direction2 = Main.MouseWorld - (projectile.position);
                direction2.Normalize();
                projectile.rotation = direction2.ToRotation() + 0.78f;
            }
            else
            {
                if (!launched)
                {
                    Vector2 direction2 = Main.MouseWorld - (projectile.position);
                    direction2.Normalize();
                    direction2 *= 20;
                    projectile.velocity = direction2;
                    projectile.rotation = direction2.ToRotation() + 0.78f;
                    projectile.timeLeft = 300;
                    launched = true;
                }
                if (projectile.timeLeft <= 285)
                {
                    projectile.tileCollide = true;
                }
            }
        }
        private float alpha;
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            alpha += 0.05f;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            Color shadeColor = Main.hslToRgb((projectile.ai[0] / 16.96f) + 0.46f, 1f, 0.7f);
            EEMod.PrismShader.Parameters["alpha"].SetValue(alpha * 2 % 6);
            EEMod.PrismShader.Parameters["shineSpeed"].SetValue(0.7f);
            EEMod.PrismShader.Parameters["tentacle"].SetValue(EEMod.instance.GetTexture("ShaderAssets/PrismDaggerLightMap"));
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