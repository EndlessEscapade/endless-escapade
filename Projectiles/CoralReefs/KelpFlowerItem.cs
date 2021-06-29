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
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.alpha = 0;
            projectile.scale = 0.5f;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 1000000000;
        }

        public override bool? CanCutTiles() => false;

        private bool funi;
        private bool fadingOut;
        private Player targetPlayer;

        public override void AI()
        {
            if(projectile.scale < 1f) projectile.scale += 0.075f;

            projectile.ai[1]++;
            
            if (projectile.ai[1] < 50)
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
                    projectile.velocity.Y += (float)(Math.Sin(projectile.ai[1] / 60f * 3.14f) * 0.05f);
                }

                if (!fadingOut)
                {
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        if (Main.player[i].active)
                        {
                            Player player = Main.player[i];

                            Item chungu = new Item();
                            chungu.SetDefaults((int)projectile.ai[0]);

                            if ((new Rectangle((int)projectile.Center.X - chungu.width / 2, (int)projectile.Center.Y - chungu.height / 2, chungu.width, chungu.height)).Intersects(player.Hitbox))
                            {
                                fadingOut = true;

                                targetPlayer = player;

                                break;
                            }
                        }
                    }
                }
            }

            if(fadingOut)
            {
                projectile.alpha += 16;
                projectile.scale -= 1 / 16f;

                projectile.velocity = Vector2.Normalize(targetPlayer.Center - projectile.Center) * 2;
            }

            if(projectile.alpha >= 255)
            {
                targetPlayer.QuickSpawnItem((int)projectile.ai[0]);

                projectile.Kill();
            }
        }

        private float alpha = 0;
        private float alpha2 = 0;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = Main.itemTexture[(int)projectile.ai[0]];
            Vector2 pos = (projectile.Center - Main.screenPosition);

            alpha += 0.06f;

            alpha = MathHelper.Clamp(alpha, 0f, 1f);

            Helpers.DrawAdditiveFunky(ModContent.GetInstance<EEMod>().GetTexture("Textures/RadialGradientWide"), projectile.Center.ForDraw(), Color.Lerp(Color.Goldenrod, Color.Gold, 0.5f) * alpha * ((255 - projectile.alpha) / 255f), 0.9f * projectile.scale, 0.8f);

            if (projectile.ai[1] >= 60)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

                Color outlineColor = Color.Lerp(Color.Goldenrod, Color.Gold, 0.5f);

                EEMod.White.CurrentTechnique.Passes[0].Apply();
                EEMod.White.Parameters["alpha"].SetValue(MathHelper.Clamp(alpha, 0f, 1f) * ((255 - projectile.alpha) / 255f));
                EEMod.White.Parameters["color"].SetValue((new Vector3(outlineColor.R, outlineColor.G, outlineColor.B) / 255f) * MathHelper.Clamp(alpha, 0f, 1f) * ((255 - projectile.alpha) / 255f));

                alpha2 += 0.03f;

                alpha2 = MathHelper.Clamp(alpha2, 0f, 1f);

                for (int i = 0; i < 4; i++)
                {
                    Vector2 offsetPositon = Vector2.UnitY.RotatedBy(MathHelper.PiOver2 * i) * (2 * (alpha2 / 1) * ((255 - projectile.alpha) / 255f));
                    spriteBatch.Draw(tex, pos + offsetPositon, null, Color.White * alpha2 * ((255 - projectile.alpha) / 255f), projectile.rotation, tex.Size() * 0.5f, projectile.scale, SpriteEffects.None, 0f);
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

            if ((new Rectangle((int)projectile.Center.X - chungu.width / 2, (int)projectile.Center.Y - chungu.height / 2, chungu.width, chungu.height)).Contains(Main.MouseWorld.ToPoint()))
            {
                Utils.DrawBorderString(Main.spriteBatch, chungu.Name, Main.MouseWorld.ForDraw() + new Vector2(16, 16), Color.Lerp(Color.Goldenrod, Color.LightYellow, (float)Math.Sin(Main.GameUpdateCount / 30f)));
            }
        }
    }
}