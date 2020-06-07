using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace InteritosMod.NPCs.Akumo
{
    public class AkumoFeather : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Feather");
        }

        public override void SetDefaults()
        {
            projectile.width = 44;
            projectile.height = 18;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.ranged = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 800;
        }
        public override bool PreAI()
        {
            if (Main.rand.Next(6) == 0)
            {
                int num25 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 35, 0f, 0f, 100, default, 1f);
                Main.dust[num25].velocity *= 0.3f;
                Main.dust[num25].noGravity = true;
                Main.dust[num25].noLight = true;
            }
            return true;
        }
        private float speed = 8;
        private float xAddon = Main.rand.NextFloat(-.1f, .1f);
        public override void AI()
        {
            projectile.ai[0] += 0.1f;
            if (Main.rand.Next(2) == 0)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 35, 0.0f, 0.0f, 100, new Color(), 1f);
            }
            projectile.rotation = projectile.velocity.ToRotation(); // projectile faces sprite right
            projectile.velocity.Y = speed * (float)Math.Sin(projectile.ai[0]) + 10;
            projectile.velocity.X += xAddon;
        }

        public override void OnHitPlayer(Player player, int damage, bool crit)
        {

        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = mod.GetTexture("NPCs/Akumo/AkumoFeather");
            AfterImage.DrawAfterimage(spriteBatch, texture, 0, projectile, 1.5f, 1f, 2, false, 0f, 0f, new Color(lightColor.R, lightColor.G, lightColor.B, 150));
            return true;
        }
        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.DD2_ExplosiveTrapExplode, projectile.position);
            for (int i = 0; i < 20; i++)
            {
                int index2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 35, 0.0f, 0.0f, 100, new Color(), 1f);
                Main.dust[index2].velocity *= 1.1f;
                Main.dust[index2].scale *= 0.99f;
            }
        }
    }
}
