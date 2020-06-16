using Terraria.ModLoader;

namespace EEMod.Projectiles
{
    public class WhiteBlock : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("WhiteBlock");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.alpha = 0;
            projectile.timeLeft = 60000;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale *= 1;
        }

        //private void LookToPlayer() // unused
        //{
        //    Player player = Main.player[projectile.owner];
        //    Vector2 look = Main.player[projectile.owner].Center - projectile.Center;
        //    LookInDirectionP(look);
        //}
        //private void LookInDirectionP(Vector2 look)
        //{
        //    float angle = 0.5f * (float)Math.PI;
        //    if (look.X != 0f)
        //    {
        //        angle = (float)Math.Atan(look.Y / look.X);
        //    }
        //    else if (look.Y < 0f)
        //    {
        //        angle += (float)Math.PI;
        //    }
        //    if (look.X < 0f)
        //    {
        //        angle += (float)Math.PI;
        //    }
        //    projectile.rotation = angle;
        //}
        public override void AI()
        {
            projectile.ai[0] += 5;
            if (projectile.ai[0] >= 255)
            {
                projectile.ai[0] = 0;
            }
            projectile.alpha = (int)projectile.ai[0];
            projectile.scale = 1 + projectile.ai[0] / (float)255;
        }
    }
}