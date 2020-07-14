using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
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
        int sinControl;
        public List<ParticlesClass> Particles = new List<ParticlesClass>();
        public override void AI()
        {
            projectile.timeLeft = 900;
            projectile.Center = Main.player[(int)projectile.ai[1]].Center;
            sinControl++;
            if (sinControl % 40 == 0 && EEModConfigClient.Instance.ParticleEffects)
            {
                Vector2 particlesPos = new Vector2(Main.rand.Next(2000), Main.screenHeight + 200) + Main.screenPosition;
                ParticlesClass particle = new ParticlesClass();
                particle.scale = Main.rand.NextFloat(0.2f, 0.5f);
                particle.alpha = Main.rand.NextFloat(100, 180);
                particle.Position = particlesPos;
                Particles.Add(particle);
            }
            if (!EEModConfigClient.Instance.ParticleEffects)
            {
                projectile.Kill();
            }
        }

        public float flash;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            flash += 0.01f;

            for (int i = 0; i < Particles.Count; i++)
            {
                Particles[i].flash++;
                Particles[i].Position += new Vector2((float)Math.Sin(Particles[i].flash / (Particles[i].alpha / 13)) / (Particles[i].alpha / 2), -2f * Particles[i].scale);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
                Main.spriteBatch.Draw(ModContent.GetTexture("EEMod/Projectiles/Particles"), Particles[i].Position - Main.screenPosition, null, Color.White * Math.Abs((float)(Math.Sin(Particles[i].flash / (Particles[i].alpha / 3f)))), Particles[i].flash / 10f, new Vector2(0), Particles[i].scale, SpriteEffects.None, 0);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin();
            }

            return false;
        }
    }
}
