using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Extensions;
using EEMod.Prim;
using System;

namespace EEMod.NPCs.Glowshroom
{
    public class MushroomMage : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mushroom Mage");
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            NPC.friendly = true;

            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;

            //npc.alpha = 127;

            NPC.lifeMax = 5;

            NPC.width = 46;
            NPC.height = 40;

            NPC.noGravity = true;
        }

        public override void AI()
        {
            NPC.ai[0]++;

            if(NPC.ai[0] % 60 == 0)
            {
                Projectile.NewProjectile(new ProjectileSource_NPC(NPC), NPC.Center, new Vector2(0, -4), ModContent.ProjectileType<MushroomBall>(), 0, 0f);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            for (int i = 0; i < balls.Length; i++) 
            {
                MushBall ball = balls[i];

                ball.i = i;
                ball.Update();

                if(ball.inFront == false)
                {
                    Texture2D tex = ModContent.Request<Texture2D>("EEMod/Textures/PermafrostRuneLightMap").Value;

                    Main.spriteBatch.Draw(tex, NPC.Center + new Vector2(ball.x, ball.y) - Main.screenPosition, null, Color.White, 0f, tex.TextureCenter(), 1f, SpriteEffects.None, 0f);
                }
                
                balls[i] = ball;
            }

            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>("EEMod/NPCs/Glowshroom/MushroomMage").Value;

            spriteBatch.Draw(tex, NPC.Center - screenPos, tex.Bounds, Color.White, 0f, tex.TextureCenter(), 1f, SpriteEffects.None, 0f);

            spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            EEMod.BloomShader.Parameters["resolution"].SetValue(new Vector2(tex.Width, tex.Height));
            EEMod.BloomShader.Parameters["satLevel"].SetValue(1f);
            EEMod.BloomShader.Parameters["radius"].SetValue(8.0f);

            EEMod.BloomShader.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Draw(tex, NPC.Center - screenPos, tex.Bounds, Color.White, 0f, tex.TextureCenter(), 1f, SpriteEffects.None, 0f);

            spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < balls.Length; i++)
            {
                MushBall ball = balls[i];

                if (ball.inFront == true)
                {
                    Texture2D tex2 = ModContent.Request<Texture2D>("EEMod/Textures/PermafrostRuneLightMap").Value;

                    Main.spriteBatch.Draw(tex2, NPC.Center + new Vector2(ball.x, ball.y) - Main.screenPosition, null, Color.White, 0f, tex2.TextureCenter(), 1f, SpriteEffects.None, 0f);
                }
            }
        }

        public MushBall[] balls = new MushBall[5];
        public struct MushBall
        {
            public float x;
            public float y;

            public int i;

            public bool inFront;

            MushBall(float _x, float _y, int _i, bool _inFront)
            {
                x = _x;
                y = _y;
                i = _i;
                inFront = _inFront;
            }

            public void Update()
            {
                float ticks = (Main.GameUpdateCount + (48 * i)) % 240;

                if (ticks >= 0 && ticks < 120) inFront = true;
                else inFront = false;

                y = 8f * (float)Math.Sin((ticks / 240f) * MathHelper.TwoPi) + 4f * (float)Math.Sin((ticks / 120f) * MathHelper.TwoPi);
                x = -80 * (float)Math.Cos((ticks / 240f) * MathHelper.TwoPi);
            }
        }
    }

    public class MushroomBall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mushroom Ball");
        }

        public override void SetDefaults()
        {
            Projectile.width = 25;
            Projectile.height = 25;
            Projectile.friendly = false;
            Projectile.timeLeft = 240;
            Projectile.penetrate = 3;
            //Projectile.DamageType = DamageClass.Ranged;
            Projectile.damage = 5;
            Projectile.knockBack = 0;
        }

        public override void Kill(int timeLeft)
        {
            
        }

        public override void AI()
        {

        }

        public void DrawMushroom()
        {
            Color colour = Color.DarkGoldenrod * 0.75f;
            Texture2D tex = ModContent.Request<Texture2D>("EEMod/Textures/RadialGradient").Value;

            Helpers.DrawAdditiveFunky(tex, Projectile.Center.ForDraw(), colour, 0.5f, 1f);

            colour = Color.DarkRed;
            tex = ModContent.Request<Texture2D>("EEMod/NPCs/Glowshroom/MushroomBall").Value;

            Helpers.DrawAdditiveFunky(tex, Projectile.Center.ForDraw(), colour, 0.5f, 1f, 0.5f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}