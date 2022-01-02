using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace EEMod.NPCs.Bosses.Akumo
{
    public class AkumoFeather : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Feather");
        }

        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 18;
            // Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            // Projectile.tileCollide = false;
            Projectile.timeLeft = 800;
        }

        public override bool PreAI()
        {
            if (Main.rand.NextBool(6))
            {
                int num25 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Lava, 0f, 0f, 100, default, 1f);
                Main.dust[num25].velocity *= 0.3f;
                Main.dust[num25].noGravity = true;
                Main.dust[num25].noLight = true;
            }
            return true;
        }

        private readonly float speed = 8;
        private readonly float xAddon = Main.rand.NextFloat(-.1f, .1f);

        public override void AI()
        {
            Projectile.ai[0] += 0.1f;
            if (Main.rand.NextBool(2))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Lava, 0.0f, 0.0f, 100, new Color(), 1f);
            }
            Projectile.rotation = Projectile.velocity.ToRotation(); // projectile faces sprite right
            Projectile.velocity.Y = speed * (float)Math.Sin(Projectile.ai[0]) + 10;
            Projectile.velocity.X += xAddon;
        }

        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = EEMod.Instance.Assets.Request<Texture2D>("NPCs/Bosses/Akumo/AkumoFeather").Value;
            AfterImage.DrawAfterimage(Main.spriteBatch, texture, 0, Projectile, 1.5f, 1f, 2, false, 0f, 0f, new Color(lightColor.R, lightColor.G, lightColor.B, 150));
            return true;
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.position);
            for (int i = 0; i < 20; i++)
            {
                int index2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Lava, 0.0f, 0.0f, 100, new Color(), 1f);
                Main.dust[index2].velocity *= 1.1f;
                Main.dust[index2].scale *= 0.99f;
            }
        }
    }
}