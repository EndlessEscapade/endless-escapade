using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using EEMod.Buffs.Buffs;

namespace EEMod.Projectiles.Summons
{
    public class FlamingPumpkinProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flaming Pumpkin");
            Main.projFrames[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 18;
            projectile.aiStyle = -1;
            projectile.penetrate = 1;
            projectile.timeLeft = 720;
            projectile.minion = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ai[0] = 60;
        }

        public override void AI()
        {
            EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.5f));




            projectile.ai[1]++;
            if(projectile.ai[1] < 600)
            {
                Vector2 targetPos = Vector2.Zero;

                for (int i = 0; i < Main.npc.Length - 1; i++)
                    if (Vector2.DistanceSquared(Main.npc[i].Center, projectile.Center) < Vector2.DistanceSquared(targetPos, projectile.Center))
                        targetPos = Main.npc[i].Center;

                if(projectile.velocity.Y >= -0.01f && projectile.velocity.Y <= 0.01f && targetPos != Vector2.Zero)
                {
                    projectile.velocity.Y -= 6;
                }

                projectile.velocity.X += Vector2.Normalize(targetPos - projectile.Center).X / 24f;
                MathHelper.Clamp(projectile.velocity.X, -6, 6);

                projectile.rotation = projectile.velocity.X / 12f;
                projectile.velocity.Y += 0.2f;
            }

            if (projectile.ai[1] >= 600 && projectile.ai[1] < 720)
            {
                projectile.velocity.X *= 0.93f;

                projectile.rotation *= 0.5f;

                if(projectile.velocity.X <= 0.02f && projectile.velocity.X >= -0.02f && projectile.velocity.Y <= 0.02f && projectile.velocity.Y >= -0.02f)
                {
                    projectile.frameCounter++;
                    if (projectile.frameCounter >= 4)
                    {
                        projectile.frameCounter = 0;
                        projectile.frame++;
                        if (projectile.frame >= Main.projFrames[projectile.type])
                            projectile.frame = 0;
                    }
                    projectile.velocity = Vector2.Zero;
                }

                projectile.velocity.Y += 0.2f;
            }




            Color lightColor = Color.White;
            switch (projectile.ai[0])
            {
                case 0:
                    lightColor = Color.Red;
                    EEMod.Particles.Get("Main").SpawnParticles(projectile.Center, new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)), 2, Color.Lerp(Color.DarkRed, Color.OrangeRed, Main.rand.NextFloat(0f, 1f)), new SlowDown(0.97f), new RotateVelocity(Main.rand.NextFloat(-.08f, .08f)), new RotateTexture(0.02f));
                    break;
                case 1:
                    lightColor = Color.Orange;
                    EEMod.Particles.Get("Main").SpawnParticles(projectile.Center, new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)), 2, Color.Lerp(Color.OrangeRed, Color.Goldenrod, Main.rand.NextFloat(0f, 1f)), new SlowDown(0.97f), new RotateVelocity(Main.rand.NextFloat(-.08f, .08f)), new RotateTexture(0.02f));
                    break;
                case 2:
                    lightColor = Color.Yellow;
                    EEMod.Particles.Get("Main").SpawnParticles(projectile.Center, new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)), 2, Color.Lerp(Color.Goldenrod, Color.LightYellow, Main.rand.NextFloat(0f, 1f)), new SlowDown(0.97f), new RotateVelocity(Main.rand.NextFloat(-.08f, .08f)), new RotateTexture(0.02f));
                    break;
            }

            Lighting.AddLight(projectile.Center, new Vector3(lightColor.R, lightColor.G, lightColor.B)/500);
        }

        private int[,] arrae =
            {
                { 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0 },
                { 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0 },
                { 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
                { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1 },
                { 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1 },
                { 1, 0, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 0, 1 },
                { 0, 1, 0, 0, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0, 1, 0 },
                { 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0 },
                { 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0 }
            };

        public override void Kill(int timeLeft)
        {
            for(int j = 0; j < 13; j++)
            {
                for (int i = 0; i < 18; i++)
                {
                    if (arrae[j, i] == 1)
                    {
                        EEMod.Particles.Get("Main").SpawnParticles(projectile.Center + new Vector2(i - 9, (j - 6)/2) * 2, Vector2.Normalize(projectile.Center - (projectile.Center - new Vector2(i - 9, j - 6))), 3, Color.Lerp(Color.Red, Color.Yellow, Main.rand.NextFloat(0f, 1f)), new SlowDown(0.95f), new RotateTexture(0.01f));
                    }
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
    }
}