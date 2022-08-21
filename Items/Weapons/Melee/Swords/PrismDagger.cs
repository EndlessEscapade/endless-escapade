using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Melee.Swords
{
    public class PrismDagger : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Prism Dagger");
        }

        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 48;
            Projectile.aiStyle = -1;
            Projectile.penetrate = 1;
            Projectile.scale = 1f;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            // Projectile.friendly = false;
            Projectile.damage = 20;
            Projectile.knockBack = 4.5f;
            Projectile.alpha = 100;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        private bool launched = false;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[1] == 0)
            {
                EEMod.Particles.AppendSpawnModule("Main", new SpawnRandomly(1f));
                for (int i = 0; i < 5; i++)
                    EEMod.MainParticles.SpawnParticles(Projectile.Center, default, 3, Main.hslToRgb((Projectile.ai[0] / 16.96f) + 0.46f, 1f, 0.7f), new Spew(6.14f, 1f, Vector2.One, 0.95f), new RotateVelocity(Main.rand.NextFloat(-0.03f, 0.03f)), new AfterImageTrail(1.5f));
            }
            Projectile.ai[1] = (Main.GameUpdateCount / 60f * 6.28f) + Projectile.ai[0];
            if (!Projectile.friendly)
            {

                Vector2 circle = new Vector2(40 * (float)Math.Sin((double)Projectile.ai[1]), 20 * (float)Math.Cos((double)Projectile.ai[1]) + 50);
                Vector2 mouseToPlayer = Main.MouseWorld - player.Center;
                circle = circle.RotatedBy(mouseToPlayer.ToRotation() + 1.57);
                Vector2 posToBe = player.Center + circle - new Vector2(24, 24);
                Vector2 direction = posToBe - Projectile.position;
                float speed = (float)Math.Sqrt(direction.Length()) / 2;
                direction.Normalize();
                direction *= speed;
                Projectile.velocity = direction;
                Vector2 direction2 = Main.MouseWorld - (Projectile.position);
                direction2.Normalize();
                Projectile.rotation = direction2.ToRotation() + 0.78f;
            }
            else
            {
                if (!launched)
                {
                    Vector2 direction2 = Main.MouseWorld - (Projectile.position);
                    direction2.Normalize();
                    direction2 *= 20;
                    Projectile.velocity = direction2;
                    Projectile.rotation = direction2.ToRotation() + 0.78f;
                    Projectile.timeLeft = 300;
                    launched = true;
                }
                if (Projectile.timeLeft <= 285)
                {
                    Projectile.tileCollide = true;
                }
            }
        }
        private float alpha;
        public override bool PreDraw(ref Color lightColor)
        {
            alpha += 0.05f;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            Color shadeColor = Main.hslToRgb((Projectile.ai[0] / 16.96f) + 0.46f, 1f, 0.7f);
            EEMod.PrismShader.Parameters["alpha"].SetValue(alpha * 2 % 6);
            EEMod.PrismShader.Parameters["shineSpeed"].SetValue(0.7f);
            EEMod.PrismShader.Parameters["tentacle"].SetValue(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/PrismDaggerLightMap").Value);
            EEMod.PrismShader.Parameters["lightColour"].SetValue(Color.White.ToVector3());
            EEMod.PrismShader.Parameters["prismColor"].SetValue(shadeColor.ToVector3());
            EEMod.PrismShader.Parameters["shaderLerp"].SetValue(1f);
            EEMod.PrismShader.CurrentTechnique.Passes[0].Apply();
            Vector2 drawOrigin = new Vector2(Projectile.width / 2, Projectile.height / 2);
            Vector2 drawPos = Projectile.position - Main.screenPosition;
            shadeColor.A = 150;
            Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos + drawOrigin, null, shadeColor, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
    }
}