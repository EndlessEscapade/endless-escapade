using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
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
            projectile.timeLeft = 125;  //The amount of time the projectile is alive for  
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, ((255 - projectile.alpha) * 0.15f) / 255f, ((255 - projectile.alpha) * 0.45f) / 255f, ((255 - projectile.alpha) * 0.05f) / 255f);   //this is the light colors
            if (projectile.timeLeft > 125)
            {
                projectile.timeLeft = 125;
            }
            if (projectile.ai[0] > 1f) 
            {
                if (Main.rand.Next(6) == 0)     
                {
                    int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 113, projectile.velocity.X * 1.2f, projectile.velocity.Y * 1.2f, 130, Color.AliceBlue, 2);    
                    Main.dust[dust].noGravity = true; 
                    int dust2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 113, projectile.velocity.X * 1.2f, projectile.velocity.Y * 1.2f, 130, Color.AliceBlue, 1); 
                }
            }
            else
            {
                projectile.ai[0]++;
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