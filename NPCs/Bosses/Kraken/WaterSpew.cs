using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.NPCs.Bosses.Kraken    //We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class WaterSpew : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 12;  //Set the hitbox width
            projectile.height = 12;
            projectile.hostile = true;//Set the hitbox height
            projectile.friendly = false;  //Tells the game whether it is friendly to players/friendly npcs or not
            projectile.ignoreWater = true;  //Tells the game whether or not projectile will be affected by water
            projectile.ranged = true;  //Tells the game whether it is a ranged projectile or not
            projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed, -1 infinity
            projectile.timeLeft = 70;  //The amount of time the projectile is alive for
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (projectile.timeLeft > 60)
            {
                projectile.timeLeft = 60;
            }
            if (projectile.ai[1] == 1)
            {
                Lighting.AddLight(projectile.Center, ((255 - projectile.alpha) * 0.15f) / 255f, ((255 - projectile.alpha) * 0.15f) / 255f, ((255 - projectile.alpha) * .6f) / 255f);   //this is the light colors

                if (projectile.ai[0] > 1f)
                {
                    if (Main.rand.Next(4) == 0)
                    {
                        int dust = Dust.NewDust(projectile.position, 1, 1, 113, projectile.velocity.X * 1.2f, projectile.velocity.Y, 0, Color.Black, 2);
                        Main.dust[dust].noGravity = true;
                        int dust2 = Dust.NewDust(projectile.position, 1, 1, 113, projectile.velocity.X * 1.2f, projectile.velocity.Y, 9, Color.Black, 1f);
                        Main.dust[dust2].noGravity = true;
                    }
                }
                else
                {
                    projectile.ai[0]++;
                }
            }
            return;
        }

        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.Kill();
            return false;
        }
    }
}