using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

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
            projectile.tileCollide = false;   //make that the projectile will be destroed if it hits the terrain
            projectile.penetrate = -1;      //how many npc will penetrate
                                            //how many time this projectile has before disepire
            projectile.light = 0.3f;    // projectile light
            projectile.ignoreWater = true;
            projectile.aiStyle = 0;
            projectile.timeLeft = 300;
            projectile.alpha = 255;
        }

        private float radius = 0;

        public override void AI()
        {
            projectile.Center = Main.projectile[(int)projectile.ai[1]].Center + Vector2.UnitY.RotatedBy(projectile.ai[0]) * radius;
            if (radius < 48)
            {
                radius++;
            }

            projectile.ai[0] += 0.1f;
        }
    }
}