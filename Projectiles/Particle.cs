using EEMod.Config;
using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles
{
    public class Particle : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("ParticleHandler");
        }

        public override void SetDefaults()
        {
            projectile.width = 0;
            projectile.height = 0;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.scale = 1.2f;
            projectile.tileCollide = false;
            projectile.light = 0;
            projectile.timeLeft = 900;
        }

        private int sinControl;
        public List<ParticlesClass> Particles = new List<ParticlesClass>();
        public List<LeafClass> Leaves = new List<LeafClass>();

        public override void AI()
        {
            projectile.timeLeft = 900;
            projectile.Center = Main.player[(int)projectile.ai[1]].Center;
            sinControl++;
            if (sinControl % 20 == 0 && EEModConfigClient.Instance.ParticleEffects)
            {
                Vector2 LeafPos = new Vector2(0, Main.rand.Next(2000)) + Main.screenPosition;
                LeafClass leaf = new LeafClass
                {
                    scale = Main.rand.NextFloat(0.5f, 1f),
                    alpha = Main.rand.NextFloat(20, 100),
                    Position = LeafPos
                };
                if (Leaves.Count < 255)
                {
                    Leaves.Add(leaf);
                }
                else
                {
                    Leaves.RemoveAt(0);
                }
            }
            if (sinControl % 40 == 0 && EEModConfigClient.Instance.ParticleEffects)
            {
                Vector2 particlesPos = new Vector2(Main.rand.Next(2000), Main.screenHeight + 200) + Main.screenPosition;
                ParticlesClass particle = new ParticlesClass
                {
                    scale = Main.rand.NextFloat(0.2f, 0.5f),
                    alpha = Main.rand.NextFloat(100, 180),
                    Position = particlesPos
                };
                if (Particles.Count < 255)
                {
                    Particles.Add(particle);
                }
                else
                {
                    Particles.RemoveAt(0);
                }
            }
            if (!EEModConfigClient.Instance.ParticleEffects)
            {
                projectile.Kill();
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            for (int i = 0; i < Particles.Count; i++)
            {
                Particles[i].flash++;
                Particles[i].Position += new Vector2((float)Math.Sin(Particles[i].flash / (Particles[i].alpha / 13)) / (Particles[i].alpha / 2), -2f * Particles[i].scale);

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
                spriteBatch.Draw(ModContent.GetTexture("EEMod/Projectiles/Particles"), Particles[i].Position - Main.screenPosition, null, Color.White * Math.Abs((float)Math.Sin(Particles[i].flash / (Particles[i].alpha / 3f))), Particles[i].flash / 10f, Vector2.Zero, Particles[i].scale, SpriteEffects.None, 0);
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            }
            for (int i = 0; i < Particles.Count; i++)
            {
                Leaves[i].flash++;
                Leaves[i].Velocity = new Vector2(3f * Leaves[i].scale, (float)Math.Sin(Leaves[i].flash * 0.5f / Leaves[i].alpha));
                Leaves[i].rotation += Leaves[i].Velocity.X / 50f;
                Leaves[i].Position += Leaves[i].Velocity;
                spriteBatch.Draw(TextureCache.Leaf, Leaves[i].Position - Main.screenPosition, null, Color.White, Leaves[i].Velocity.ToRotation() + Leaves[i].rotation, Vector2.Zero, Leaves[i].scale, SpriteEffects.None, 0);
            }
            return false;
        }

        public Texture2D GetScreenTex()
        {
            RenderTarget2D buffer = new RenderTarget2D(Main.instance.GraphicsDevice, 1980, 1017, false, Main.instance.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
            Main.graphics.GraphicsDevice.SetRenderTarget(Main.screenTarget);
            Color[] texdata = new Color[buffer.Width * buffer.Height];
            buffer.GetData(texdata);
            Texture2D screenTex = new Texture2D(Main.graphics.GraphicsDevice, buffer.Width, buffer.Height);
            screenTex.SetData(texdata);
            Main.spriteBatch.Draw(screenTex, Main.LocalPlayer.Center.ForDraw(), new Rectangle(0, 0, 1980, 1017), Color.White, 0f, new Rectangle(0, 0, 1980, 1017).Size() / 2, 1, SpriteEffects.None, 0);
            Main.instance.GraphicsDevice.SetRenderTarget(null);

            return buffer;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Main.spriteBatch.Draw(GetScreenTex(), Main.LocalPlayer.Center.ForDraw(), new Rectangle(0, 0, 1980, 1017), Color.White, 0f, new Rectangle(0, 0, 1980, 1017).Size() / 2, 1, SpriteEffects.FlipVertically, 0);
        }
    }
}