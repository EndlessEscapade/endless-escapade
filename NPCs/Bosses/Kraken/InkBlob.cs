using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.Bosses.Kraken    //We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class InkBlob : EEProjectile
    {
        public static short customGlowMask = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ink Blob");
        }

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 2000;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.damage = 60;
            Projectile.light = 1f;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        private Vector2 start;
        private Vector2[] yeet = new Vector2[2];
        KrakenHead krakenHead => Main.npc[(int)Projectile.ai[1]].modNPC as KrakenHead;
        private bool yes = false;
        private int Timer; //I will sync it I swear

        public override void AI()
        {
            if (yes)
            {
                Projectile.timeLeft = 100;
                Timer++;
                if (Timer > 120)
                {
                    Projectile.velocity = ((yeet[0] + yeet[1]) / 2 - Projectile.Center) / 32f;
                    Projectile.alpha++;
                    Projectile.alpha = Helpers.Clamp(Projectile.alpha, 0, 255);
                    if (Projectile.alpha >= 255)
                    {
                        if (krakenHead.smolBloons[0] != Vector2.Zero)
                        {
                            krakenHead.smolBloons[0] = Vector2.Zero;
                        }

                        if (krakenHead.smolBloons[1] != Vector2.Zero)
                        {
                            krakenHead.smolBloons[1] = Vector2.Zero;
                        }

                        Projectile.Kill();
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
                    Dust.NewDust(Projectile.Center, 22, 22, DustID.Blood, (start.X - Projectile.Center.X) / 16f, (start.Y - Projectile.Center.Y) / 16f, 0, Color.Black, 2);
                }
            }
            if (Projectile.ai[0] == 0)
            {
                if (Main.rand.Next(4) == 0)
                {
                    Dust.NewDust(Projectile.Center, 22, 22, DustID.Blood, (start.X - Projectile.Center.X) / 16f, (start.Y - Projectile.Center.Y) / 16f, 0, Color.Black, 2);
                }

                start = Projectile.Center;
            }
            if (Timer < 120)
            {
                Projectile.velocity = Projectile.velocity.RotatedBy(Math.PI / 180) * 0.98f;
            }
            Projectile.ai[0]++;
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
            if (Main.npc[(int)Projectile.ai[1]].life <= 0)
            {
                Projectile.Kill();
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

            Main.PlaySound(SoundID.Item27, Projectile.position);
            for (var i = 0; i < 5; i++)
            {
                int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-4f, 4f), 6, Color.Black, 2);
            }
        }
    }
}