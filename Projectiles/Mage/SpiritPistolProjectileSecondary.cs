using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace EEMod.Projectiles.Mage
{
    public class SpiritPistolProjectileSecondary : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 8;       //projectile width
            projectile.height = 8;  //projectile height
            projectile.friendly = true;      //make that the projectile will not damage you
            projectile.magic = true;     //
            projectile.tileCollide = true;   //make that the projectile will be destroed if it hits the terrain
            projectile.penetrate = 1;      //how many npc will penetrate
                                           //how many time this projectile has before disepire
            projectile.light = 0.3f;    // projectile light
            projectile.ignoreWater = true;
            projectile.aiStyle = 0;
            projectile.timeLeft = 300;
        }

        float radius = 0;
        public override void AI()
        {
            projectile.Center = Main.projectile[(int)projectile.ai[1]].Center + Vector2.UnitY.RotatedBy(projectile.ai[0]) * radius;
            if(radius < 48)
                radius++;
            projectile.ai[0] += 0.1f;

            for (int i = 0; i < 180; i += 10)
            {
                float xdist = (int)(Math.Sin(Math.Sin(i * (Math.PI / 180))) * 5);
                float ydist = (int)(Math.Cos(Math.Cos(i * (Math.PI / 180))) * 5);
                Vector2 offset = new Vector2(xdist, ydist).RotatedBy(projectile.rotation);
                Dust dust = Dust.NewDustPerfect(projectile.Center + offset, 111, offset * 0.5f);
                dust.noGravity = true;
                dust.velocity *= 0.94f;
                dust.noLight = false;
                dust.fadeIn = 1f;
            }
        }
    }
}
