using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using System;
using EEMod.Extensions;

namespace EEMod.Projectiles.CoralReefs
{
    public class KelpFlowerItem : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("???");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            // Projectile.friendly = false;
            // Projectile.hostile = false;
            Projectile.alpha = 0;
            Projectile.scale = 0.5f;
            // Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1000000000;
        }

        public override bool? CanCutTiles() => false;

        private bool funi;
        private bool fadingOut;
        private Player targetPlayer;

        public override void AI()
        {
            if(Projectile.scale < 1f) Projectile.scale += 0.075f;

            Projectile.ai[1]++;
            
            if (Projectile.ai[1] < 50)
            {
                Projectile.velocity.Y += 0.08f;
            }
            else
            {
                if (!funi)
                {
                    Projectile.velocity.Y *= 0.92f;
            
                    if (Projectile.velocity.Y <= 0.05f)
                    {
                        Projectile.velocity.Y = 0;
                        funi = true;
                    }
                }
                else
                {
                    Projectile.velocity.Y += (float)(Math.Sin(Projectile.ai[1] / 60f * 3.14f) * 0.05f);
                }

                if (!fadingOut)
                {
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        if (Main.player[i].active)
                        {
                            Player player = Main.player[i];

                            Item chungu = new Item();
                            chungu.SetDefaults((int)Projectile.ai[0]);

                            if ((new Rectangle((int)Projectile.Center.X - chungu.width / 2, (int)Projectile.Center.Y - chungu.height / 2, chungu.width, chungu.height)).Intersects(player.Hitbox))
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
                Projectile.alpha += 16;
                Projectile.scale -= 0.0625f;

                Projectile.velocity = Vector2.Normalize(targetPlayer.Center - Projectile.Center) * 3;


            }

            if(Projectile.alpha >= 255)
            {
                targetPlayer.QuickSpawnItem((int)Projectile.ai[0]);

                Projectile.Kill();
            }
        }

        private float alpha = 0;
        private float alpha2 = 0;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Item[(int)Projectile.ai[0]].Value;
            Vector2 pos = (Projectile.Center - Main.screenPosition);

            alpha += 0.06f;

            Color outlineColor = Color.Lerp(Color.Goldenrod, Color.Gold, 0.5f);

            Helpers.DrawAdditiveFunky(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/RadialGradientWide").Value, Projectile.Center.ForDraw(), outlineColor * MathHelper.Clamp(alpha, 0f, 1f) * ((255 - Projectile.alpha) / 255f), 0.9f * Projectile.scale, 0.8f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            EEMod.White.CurrentTechnique.Passes[0].Apply();
            EEMod.White.Parameters["alpha"].SetValue(MathHelper.Clamp(alpha, 0f, 1f) * ((255 - Projectile.alpha) / 255f));
            EEMod.White.Parameters["color"].SetValue((new Vector3(outlineColor.R, outlineColor.G, outlineColor.B) / 255f) * MathHelper.Clamp(alpha, 0f, 1f) * ((255 - Projectile.alpha) / 255f));

            alpha2 += 0.03f;

            alpha2 = MathHelper.Clamp(alpha2, 0f, 1f);

            for (int i = 0; i < 4; i++)
            {
                Vector2 offsetPositon = Vector2.UnitY.RotatedBy(MathHelper.PiOver2 * i) * (2 * (alpha2 / 1) * ((255 - Projectile.alpha) / 255f));
                Main.spriteBatch.Draw(tex, pos + offsetPositon, null, Color.White * alpha2 * ((255 - Projectile.alpha) / 255f), Projectile.rotation, tex.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0f);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            Main.spriteBatch.Draw(tex, pos, tex.Bounds, Color.White, 0f, tex.Bounds.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);

            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Item chungu = new Item();
            chungu.SetDefaults((int)Projectile.ai[0]);

            if ((new Rectangle((int)Projectile.Center.X - chungu.width / 2, (int)Projectile.Center.Y - chungu.height / 2, chungu.width, chungu.height)).Contains(Main.MouseWorld.ToPoint()))
            {
                Utils.DrawBorderString(Main.spriteBatch, chungu.Name, Main.MouseWorld.ForDraw() + new Vector2(16, 16), Color.Lerp(Color.Goldenrod, Color.LightYellow, (float)Math.Sin(Main.GameUpdateCount / 30f)));
            }
        }
    }
}