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
            projectile.timeLeft = 1000000000;
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
                    projectile.velocity.Y *= 0.92f;
            
                    if (projectile.velocity.Y <= 0.05f)
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

        private float alpha = 0;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = Main.itemTexture[(int)projectile.ai[0]];
            Vector2 pos = (projectile.Center - Main.screenPosition);

            if (projectile.ai[1] >= 60)
            {
                alpha += 0.03f;

                alpha = MathHelper.Clamp(alpha, 0f, 1f);

                Helpers.DrawAdditiveFunky(ModContent.GetInstance<EEMod>().GetTexture("Textures/RadialGradientWide"), projectile.Center.ForDraw(), Color.Gold * alpha, 0.9f, 0.8f);

                Vector2 position = new Vector2(projectile.width / 2, projectile.height - tex.Height * 0.5f + 2f);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

                EEMod.White.CurrentTechnique.Passes[0].Apply();
                EEMod.White.Parameters["alpha"].SetValue(MathHelper.Clamp(alpha, 0f, 1f));
                EEMod.White.Parameters["color"].SetValue((new Vector3(Color.Gold.R, Color.Gold.G, Color.Gold.B) / 255f) * MathHelper.Clamp(alpha, 0f, 1f));

                for (int i = 0; i < 4; i++)
                {
                    Vector2 offsetPositon = Vector2.UnitY.RotatedBy(MathHelper.PiOver2 * i) * (2 * (alpha / 1));
                    spriteBatch.Draw(tex, pos + offsetPositon, null, Color.White * alpha, projectile.rotation, tex.Size() * 0.5f, projectile.scale, SpriteEffects.None, 0f);
                }

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            }

            Main.spriteBatch.Draw(tex, pos, tex.Bounds, Color.White, 0f, tex.Bounds.Size() / 2f, projectile.scale, SpriteEffects.None, 0f);

            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Item chungu = new Item();
            chungu.SetDefaults((int)projectile.ai[0]);

            if ((new Rectangle((int)projectile.position.X + (projectile.width - chungu.width), (int)projectile.position.Y + (projectile.height - chungu.height), chungu.width, chungu.height)).Contains(Main.MouseWorld.ToPoint()))
            {
                Utils.DrawBorderString(Main.spriteBatch, chungu.Name, Main.MouseWorld.ForDraw() + new Vector2(16, 16), Color.Lerp(Color.Goldenrod, Color.LightYellow, (float)Math.Sin(Main.GameUpdateCount / 30f)));
            }
        }
    }
}