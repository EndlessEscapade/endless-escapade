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
using EEMod.Items.Weapons.Ranger.Longbows;

namespace EEMod.Items.Weapons.Mage
{
    public class SceptorPrism : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aquamarine Prism");
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 38;
            Projectile.timeLeft = 1200;
            Projectile.ignoreWater = true;
            // Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.rotation = Vector2.Normalize(player.Center - Projectile.Center).ToRotation() - MathHelper.PiOver2;
            Vector2 posToBe = new Vector2(Projectile.ai[0], Projectile.ai[1]);
            Vector2 direction = posToBe - Projectile.position;
            float speed = (float)Math.Sqrt(direction.Length()) / 2;
            if (speed > 0.1f)
            {
                direction.Normalize();
                direction *= speed;
                Projectile.velocity = direction;
            }
            else
            {
                Projectile.velocity = Vector2.Zero;
            }

            if (Projectile.timeLeft > 32)
            {
                var list = Main.projectile.Where(x => Vector2.Distance(Projectile.Center, x.Center) <= 24);
                foreach (var proj in list)
                {
                    if (proj.type == ModContent.ProjectileType<SceptorLaser>() && proj.active && proj.ai[0] == 0)
                    {
                        for (float i = -0.6f; i <= 0.6f; i += 0.4f)
                        {
                            Projectile proj2 = Projectile.NewProjectileDirect(new Terraria.DataStructures.ProjectileSource_ProjectileParent(Projectile), proj.Center - (Vector2.UnitY.RotatedBy((i + Math.PI) + Projectile.rotation) * 60), 3 * Vector2.UnitY.RotatedBy((i + Math.PI) + Projectile.rotation), ModContent.ProjectileType<ShimmerShotProj1>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 1);
                            PrimtiveSystem.primitives.CreateTrail(new SpirePrimTrail(proj2, Color.Lerp(Color.Cyan, Color.Magenta, i / ((i + 0.6f) / 1.2f)), 40));
                        }
                        proj.Kill();
                        Projectile.timeLeft = 32;
                    }
                }
            }
            else
            {
                Projectile.alpha += 8;
            }
        }

        private float alpha;
        float colorcounter = 0;
        public override bool PreDraw(ref Color lightColor)
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
            EEMod.PrismShader.Parameters["tentacle"].SetValue(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/PrismLightMap").Value);
            EEMod.PrismShader.Parameters["lightColour"].SetValue(Color.White.ToVector3() * (1 / (1 + Projectile.alpha)));
            EEMod.PrismShader.Parameters["prismColor"].SetValue(shadeColor.ToVector3() * (1 / (1 + Projectile.alpha)));
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