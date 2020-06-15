using Terraria.ModLoader;

namespace EEMod.Projectiles
{
    public class ShipHelper : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 16;  
            projectile.height = 16;
            projectile.hostile = false;
            projectile.friendly = true;  //Tells the game whether it is friendly to players/friendly npcs or not
            projectile.ignoreWater = true;  //Tells the game whether or not projectile will be affected by water
            projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed, -1 infinity
            projectile.timeLeft = 600;  //The amount of time the projectile is alive for
            projectile.tileCollide = false;
        }
    }
}
