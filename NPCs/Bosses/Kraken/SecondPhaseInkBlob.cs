using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.Bosses.Kraken    //We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class SecondPhaseInkBlob : ModProjectile
    {

        public static short customGlowMask = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ink Blob");
        }

        public override void SetDefaults()
        {
            projectile.width = 120;
            projectile.height = 120;
            projectile.hostile = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 2000;
            projectile.friendly = false;
            projectile.tileCollide = false;
            projectile.damage = 60;
            projectile.light = 1f;
            projectile.scale = 1;
            projectile.alpha = 255;
        }
        Vector2 start;
        Vector2[] yeet = new Vector2[2];
        KrakenHead krakenHead => Main.npc[(int)projectile.ai[1]].modNPC as KrakenHead;
        bool yes = false;
        int Timer; //I will sync it I swear
        public override void AI()
        {
            if (projectile.ai[0] == 0)
            {
                projectile.scale = 0;
                start = projectile.Center;
            }
            projectile.ai[0]++;
            if (projectile.alpha > 0 && projectile.ai[0] > 50)
                projectile.alpha--;
            if (projectile.ai[0] > 100)
                projectile.scale += (1 - projectile.scale) / 64f;

            if (projectile.ai[0] > 300)
            {
                Vector2 speed = new Vector2(0, -15).RotatedBy(projectile.ai[0] / 30f);
                float projectileknockBack = 4f;
                int projectiledamage = 40;
                if (Main.rand.Next(4) == 0)
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, speed.X, speed.Y, mod.ProjectileType("InkSpew"), projectiledamage, projectileknockBack, Main.npc[(int)projectile.ai[1]].target, 0f, 1);
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < krakenHead.smolBloons.Length; i++)
            {
                if (Vector2.Distance(krakenHead.bigBloons[i], start) < 20)
                {
                    krakenHead.bigBloons[i] = Vector2.Zero;
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
