using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles
{
    public class WhiteBlock : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("WhiteBlock");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.alpha = 0;
            Projectile.timeLeft = 60000;
            Projectile.penetrate = -1;
            // Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            // Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale *= 0.75f;
        }

        //private void LookToPlayer() // unused
        //{
        //    Player player = Main.player[projectile.owner];
        //    Vector2 look = Main.player[projectile.owner].Center - projectile.Center;
        //    LookInDirectionP(look);
        //}
        //private void LookInDirectionP(Vector2 look)
        //{
        //    float angle = 0.5f * MathHelper.Pi;
        //    if (look.X != 0f)
        //    {
        //        angle = (float)Math.Atan(look.Y / look.X);
        //    }
        //    else if (look.Y < 0f)
        //    {
        //        angle += MathHelper.Pi;
        //    }
        //    if (look.X < 0f)
        //    {
        //        angle += MathHelper.Pi;
        //    }
        //    projectile.rotation = angle;
        //}
        public Texture2D itemTexture;

        public override void AI()
        {
            Projectile.ai[0] += 5;
            if (Projectile.ai[0] >= 255)
            {
                Projectile.ai[0] = 0;
            }
            Projectile.alpha = (int)Projectile.ai[0];
            Projectile.scale = 0.75f + Projectile.ai[0] / 255;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Main.spriteBatch.Draw(itemTexture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, Projectile.Size / 2f, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0);
            return true;
        }
    }
}