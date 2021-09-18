using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Mage
{
    public class SpiritPistolProjectileSecondary : EEProjectile
    {
        public override string Texture => Helpers.EmptyTexture;

        public override void SetDefaults()
        {
            Projectile.width = 8;       //projectile width
            Projectile.height = 8;  //projectile height
            Projectile.friendly = true;      //make that the projectile will not damage you
            Projectile.DamageType = DamageClass.Magic;     //
            // Projectile.tileCollide = false;   //make that the projectile will be destroed if it hits the terrain
            Projectile.penetrate = -1;      //how many npc will penetrate
                                            //how many time this projectile has before disepire
            Projectile.light = 0.3f;    // projectile light
            Projectile.ignoreWater = true;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;
            Projectile.alpha = 255;
        }

        private float radius = 0;

        public override void AI()
        {
            Projectile.Center = Main.projectile[(int)Projectile.ai[1]].Center + Vector2.UnitY.RotatedBy(Projectile.ai[0]) * radius;
            if (radius < 48)
            {
                radius++;
            }

            Projectile.ai[0] += 0.1f;
        }
    }
}