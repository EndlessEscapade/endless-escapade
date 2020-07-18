using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.Bosses.Kraken    //We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class InkSpew : ModProjectile
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
            projectile.timeLeft = 27;  //The amount of time the projectile is alive for  
            projectile.tileCollide = true;
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
                        int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 1, 1, DustID.Blood, projectile.velocity.X * 1.2f, projectile.velocity.Y, 0, Color.Black, 3.5f);
                        Main.dust[dust].noGravity = true;
                        int dust2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 1, 1, DustID.Blood, projectile.velocity.X * 1.2f, projectile.velocity.Y, 9, Color.Black, 3f);
                        Main.dust[dust].noGravity = true;
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