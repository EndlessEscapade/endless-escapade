using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles
{
    public class Shard2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shard");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 8;
            projectile.alpha = 0;
            projectile.timeLeft = 600;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.scale *= 0.7f;
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
        public override void AI()
        {
            //  LookInDirectionP(projectile.velocity);
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            projectile.velocity.Y = projectile.velocity.Y + 0.5f;
            if (projectile.velocity.Y > 56f)
            {
                projectile.velocity.Y = 56f;
            }
        }
    }
}