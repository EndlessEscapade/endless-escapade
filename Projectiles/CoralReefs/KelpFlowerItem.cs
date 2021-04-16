using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using System;
using EEMod.Extensions;

namespace EEMod.Projectiles.CoralReefs
{
    public class KelpFlowerItem : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("???");
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.alpha = 0;
            projectile.scale = 1f;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
        }

        private bool funi;
        public override void AI()
        {
            projectile.ai[1]++;
            
            if (projectile.ai[1] < 60)
            {
                projectile.velocity.Y += 0.08f;
            }
            else
            {
                if (!funi)
                {
                    projectile.velocity.Y *= 0.95f;
            
                    if (projectile.velocity.Y <= 0.03f)
                    {
                        projectile.velocity.Y = 0;
                        funi = true;
                    }
                }
                else
                {
                    projectile.velocity.Y = (float)(Math.Sin(Main.GameUpdateCount / 20f) * 0.4f);
                }
            
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    if (Main.player[i].active)
                    {
                        Player player = Main.player[i];
                        if (Math.Abs(Vector2.Distance(player.Center, projectile.Center)) <= 64)
                        {
                            player.QuickSpawnItem((int)projectile.ai[0]);

                            Main.NewText("Dead");
                            projectile.Kill();
                            break;
                        }
                    }
                }
            }
        }

        private float alpha;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = Main.itemTexture[(int)projectile.ai[0]];
            Vector2 pos = (projectile.Center - Main.screenPosition);

            if (projectile.ai[1] >= 60)
            {
                alpha += 0.03f;

                Helpers.DrawAdditive(mod.GetTexture("Projectiles/Nice"), pos, Color.Gold * (Helpers.Clamp(alpha / 2f, 0f, 0.7f) + (float)(Math.Sin(Main.GameUpdateCount / 30f) / 20f)), 0.5f + (float)(Math.Sin(Main.GameUpdateCount / 40f) / 30f), (alpha / 3f));

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

                EEMod.White.CurrentTechnique.Passes[0].Apply();
                EEMod.White.Parameters["alpha"].SetValue(((float)Math.Sin(alpha) + 1) * 0.5f);
                EEMod.White.Parameters["color"].SetValue(new Vector3(1, 1, 0));
                Main.spriteBatch.Draw(tex, pos, tex.Bounds, Color.White, projectile.rotation, tex.Bounds.Size() / 2f, projectile.scale * 1.05f, SpriteEffects.None, 0);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            }

            Main.spriteBatch.Draw(tex, pos, tex.Bounds, Color.White, 0f, tex.Bounds.Size() / 2f, projectile.scale, SpriteEffects.None, 0f);

            return false;
        }
    }
}