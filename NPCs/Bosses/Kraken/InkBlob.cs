using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.Bosses.Kraken    //We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class InkBlob : ModProjectile
    {
        public static short customGlowMask = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ink Blob");
        }

        public override void SetDefaults()
        {
            projectile.width = 60;
            projectile.height = 60;
            projectile.hostile = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 2000;
            projectile.friendly = false;
            projectile.tileCollide = false;
            projectile.extraUpdates = 1;
            projectile.damage = 60;
            projectile.light = 1f;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        private Vector2 start;
        private Vector2[] yeet = new Vector2[2];
        KrakenHead krakenHead => Main.npc[(int)projectile.ai[1]].modNPC as KrakenHead;
        private bool yes = false;
        private int Timer; //I will sync it I swear

        public override void AI()
        {
            if (yes)
            {
                projectile.timeLeft = 100;
                Timer++;
                if (Timer > 120)
                {
                    projectile.velocity = ((yeet[0] + yeet[1]) / 2 - projectile.Center) / 32f;
                    projectile.alpha++;
                    projectile.alpha = Helpers.Clamp(projectile.alpha, 0, 255);
                    if (projectile.alpha >= 255)
                    {
                        if (krakenHead.smolBloons[0] != Vector2.Zero)
                        {
                            krakenHead.smolBloons[0] = Vector2.Zero;
                        }

                        if (krakenHead.smolBloons[1] != Vector2.Zero)
                        {
                            krakenHead.smolBloons[1] = Vector2.Zero;
                        }

                        projectile.Kill();
                    }
                }
            }
            if (krakenHead.smolBloons[0] != Vector2.Zero && krakenHead.smolBloons[1] != Vector2.Zero)
            {
                yes = true;
                yeet = krakenHead.smolBloons;
            }
            else
            {
                if (Main.rand.Next(4) == 0)
                {
                    Dust.NewDust(projectile.Center, 22, 22, DustID.Blood, (start.X - projectile.Center.X) / 16f, (start.Y - projectile.Center.Y) / 16f, 0, Color.Black, 2);
                }
            }
            if (projectile.ai[0] == 0)
            {
                if (Main.rand.Next(4) == 0)
                {
                    Dust.NewDust(projectile.Center, 22, 22, DustID.Blood, (start.X - projectile.Center.X) / 16f, (start.Y - projectile.Center.Y) / 16f, 0, Color.Black, 2);
                }

                start = projectile.Center;
            }
            if (Timer < 120)
            {
                projectile.velocity = projectile.velocity.RotatedBy(Math.PI / 180) * 0.98f;
            }
            projectile.ai[0]++;
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
            if (Main.npc[(int)projectile.ai[1]].life <= 0)
            {
                projectile.Kill();
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < krakenHead.smolBloons.Length; i++)
            {
                if (Vector2.DistanceSquared(krakenHead.smolBloons[i], start) < 20 * 20)
                {
                    krakenHead.smolBloons[i] = Vector2.Zero;
                }
            }

            Main.PlaySound(SoundID.Item27, projectile.position);
            for (var i = 0; i < 5; i++)
            {
                int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Blood, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-4f, 4f), 6, Color.Black, 2);
            }
        }
    }
}