using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Projectiles
{
    public class SandvekKunai : Javelin
    {
        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 15;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.penetrate = 1;
            maxStickingJavelins = 3;
            //projectile.GetGlobalProjectile<ImplaingProjectile>().CanImpale = true;
            //projectile.GetGlobalProjectile<ImplaingProjectile>().damagePerImpaler = 5;  Didn't work, couldn't find where this was created
            //rotationOffset = (float)Math.PI / 4;
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 50; i++)
            {
                int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 0, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f), 6, default, projectile.scale);
                Main.dust[num].noGravity = false;
                Main.dust[num].velocity *= 1.5f;
                Main.dust[num].noLight = false;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(texture, new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y + 2),
                        new Rectangle(0, 0, texture.Width, texture.Height), projectile.GetAlpha(lightColor), projectile.rotation,
                        new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}
