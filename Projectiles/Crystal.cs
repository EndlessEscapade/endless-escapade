using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles
{
    public class Crystal : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 12;  
            projectile.height = 12;
            projectile.hostile = false;
            projectile.friendly = true;  //Tells the game whether it is friendly to players/friendly npcs or not
            projectile.ignoreWater = true;  //Tells the game whether or not projectile will be affected by water
            projectile.ranged = true;  //Tells the game whether it is a ranged projectile or not
            projectile.penetrate = 1; //Tells the game how many enemies it can hit before being destroyed, -1 infinity
            projectile.timeLeft = 125;  //The amount of time the projectile is alive for
            projectile.tileCollide = true;
        }
        
        public override void AI()
        {
            Lighting.AddLight(projectile.Center, ((255 - projectile.alpha) * 0.15f) / 255f, ((255 - projectile.alpha) * 0.45f) / 255f, ((255 - projectile.alpha) * 0.05f) / 255f);   //this is the light colors
            if (projectile.timeLeft > 125)
            {
                projectile.timeLeft = 125;
            }
            if (projectile.ai[0] > 1f)  //this defines where the flames starts
            {
                
                for(int i = 0; i<15; i++)    //this defines how many dust to spawn
                {
                    int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 64, projectile.velocity.X * 0.3f, projectile.velocity.Y * 0.3f, 0, new Color(255, 255, 153), 1);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true;
                }
            }
            else
            {
                projectile.ai[0] += 1f;
            }
            return;

        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 4; i++)
            {
                int projHolder = Main.rand.Next(0, 1);
                float speedX = -(projectile.velocity.X * Main.rand.NextFloat(-.1f, .8f) + Main.rand.NextFloat(-.4f, 2f));
                float speedY = -(projectile.velocity.Y * Main.rand.Next(0, 30) * 0.01f + Main.rand.NextFloat(-12f, 12.1f));
                if (projHolder == 0)
                    Projectile.NewProjectile(projectile.Center.X + speedX, projectile.Center.Y + speedY, speedX*1.3f, speedY, mod.ProjectileType("CrystalKill"), (int)(projectile.damage * 0.7), 0f, projectile.owner, 0f, 0f);
                if (projHolder == 1)
                    Projectile.NewProjectile(projectile.Center.X + speedX, projectile.Center.Y + speedY, speedX*1.3f, speedY, mod.ProjectileType("CrystalKill"), (int)(projectile.damage * 0.7), 0f, projectile.owner, 0f, 0f);
                Main.PlaySound(SoundID.Item27, projectile.position);
            }
            for (var i = 0; i < 20; i++)
            {
                int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 64, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-1f, 1f), 6, new Color(255, 255, 153, 255), 2f);
                Main.dust[num].noGravity = true;
                Main.dust[num].velocity *= 1.2f;
                Main.dust[num].noLight = false;
            }
        }
        

        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            player.AddBuff(BuffID.OnFire, 120);
            player.AddBuff(BuffID.Frostburn, 120);
            player.AddBuff(BuffID.CursedInferno, 120);				//this make so when the projectile/flame hit a npc, gives it the buff  onfire , 80 = 3 seconds
        }
        private void LookInDirectionP(Vector2 look)
        {
            float angle = 0.5f * (float)Math.PI;
            if (look.X != 0f)
            {
                angle = (float)Math.Atan(look.Y / look.X);
            }
            else if (look.Y < 0f)
            {
                angle += (float)Math.PI;
            }
            if (look.X < 0f)
            {
                angle += (float)Math.PI;
            }
            projectile.rotation = angle;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.Kill();
            return false;
        }

    }
}
