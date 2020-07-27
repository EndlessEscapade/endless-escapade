using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace EEMod.Projectiles.Mage
{
    public class CyanoburstTomeProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cyanoburst Plankton");
        }

        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.timeLeft = 420;
            projectile.ignoreWater = true;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.penetrate = -1;
        }

        public override void Kill(int timeLeft)
        {
            //Projectile.NewProjectile(projectile.position, Vector2.Zero, );
        }
    }
}