using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.Projectiles.Mage
{
    public class SpiritPistolProjectileSecondary : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 12;       //projectile width
            projectile.height = 12;  //projectile height
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

        float radius = 16;
        public override void AI()
        {
            projectile.Center = Main.projectile[(int)projectile.ai[0]].position + Vector2.UnitY.RotatedBy(projectile.ai[0]) * radius;
            radius++;
            projectile.ai[0] += 0.1f;
        }
    }
}
