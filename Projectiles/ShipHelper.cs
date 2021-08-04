using Terraria.ModLoader;

namespace EEMod.Projectiles
{
    public class ShipHelper : EEProjectile
    {
        public override string Texture => Helpers.EmptyTexture;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.hostile = false;
            Projectile.friendly = true;  //Tells the game whether it is friendly to players/friendly npcs or not
            Projectile.ignoreWater = true;  //Tells the game whether or not projectile will be affected by water
            Projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed, -1 infinity
            Projectile.timeLeft = 600;  //The amount of time the projectile is alive for
            Projectile.tileCollide = false;
        }
    }
}