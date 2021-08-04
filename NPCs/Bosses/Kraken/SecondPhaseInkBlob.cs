using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.Bosses.Kraken    //We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class SecondPhaseInkBlob : EEProjectile
    {
        public static short customGlowMask = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ink Blob");
        }

        public override void SetDefaults()
        {
            Projectile.width = 120;
            Projectile.height = 120;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 2000;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.damage = 60;
            Projectile.light = 1f;
            Projectile.scale = 1;
            Projectile.alpha = 255;
        }

        private Vector2 start;
        private readonly Vector2[] yeet = new Vector2[2];
        KrakenHead krakenHead => Main.npc[(int)Projectile.ai[1]].modNPC as KrakenHead;

        public override void AI()
        {
            if (Main.npc[(int)Projectile.ai[1]].life <= 0)
            {
                Projectile.Kill();
            }
            if (Projectile.ai[0] == 0)
            {
                Projectile.scale = 0;
                start = Projectile.Center;
            }
            Projectile.ai[0]++;
            if (Projectile.alpha > 0 && Projectile.ai[0] > 50)
            {
                Projectile.alpha--;
            }

            if (Projectile.ai[0] > 100)
            {
                Projectile.scale += (1 - Projectile.scale) / 64f;
            }

            if (Projectile.ai[0] > 300)
            {
                Vector2 speed = new Vector2(0, -15).RotatedBy(Projectile.ai[0] / 30f);
                float projectileknockBack = 4f;
                int projectiledamage = 40;
                if (Main.rand.Next(4) == 0)
                {
                    Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, speed.X, speed.Y, ModContent.ProjectileType<InkSpew>(), projectiledamage, projectileknockBack, Main.npc[(int)Projectile.ai[1]].target, 0f, 1);
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < krakenHead.smolBloons.Length; i++)
            {
                if (Vector2.DistanceSquared(krakenHead.bigBloons[i], start) < 20 * 20)
                {
                    krakenHead.bigBloons[i] = Vector2.Zero;
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