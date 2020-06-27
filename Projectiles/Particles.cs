
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace EEMod.Projectiles
{
    public class Particles : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubble");
        }

        public override void SetDefaults()
        {
            projectile.width = 2;
            projectile.height = 2;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.scale = 1.2f;
            projectile.tileCollide = false;
            projectile.light = 0;
        }
        int sinControl;
        public override void AI()
        {
            projectile.scale = projectile.ai[0];
            projectile.alpha = (int)projectile.ai[1];
            projectile.position.Y--;
            sinControl++;
            if(projectile.ai[1] < 140)
            projectile.velocity.X += (float)Math.Sin(sinControl/(projectile.ai[1]/13))/ (projectile.ai[1]/2);
            else if(projectile.ai[1] < 160)
            projectile.position.X += (float)Math.Sin(sinControl / (projectile.ai[1] / 13)) / (projectile.ai[1] / 4);
            else
            projectile.position.X -= (float)Math.Sin(sinControl /(projectile.ai[1] / 13)) / (projectile.ai[1]);
        }
        public override void Kill(int timeLeft)
        {
            for (var a = 0; a < 10; a++)
            {
                Vector2 vector = new Vector2(0, 10).RotatedBy(((Math.PI * 0.2) * a), default);
                int index = Dust.NewDust(projectile.Center, 22, 22, DustID.BlueCrystalShard, vector.X, vector.Y, 0, Color.Blue, 1f);
                Main.dust[index].velocity *= 1.3f;
                Main.dust[index].noGravity = true;
            }
        }
        public float flash;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            flash += 0.01f;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
                Main.spriteBatch.Draw(ModContent.GetTexture("EEMod/Projectiles/Particles"), projectile.Center - Main.screenPosition, new Rectangle(0, 0, 174, 174), lightColor * Math.Abs((float)Math.Sin(flash)) * 2, projectile.rotation + flash, new Rectangle(0, 0, 174, 174).Size() / 2, projectile.ai[0], SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin();
            return false;
        }
        }
}
