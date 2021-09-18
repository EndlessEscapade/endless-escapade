using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles
{
    public class HydroBurst : EEProjectile
    {
        public override string Texture => Helpers.EmptyTexture;

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            // Projectile.hostile = false;
            Projectile.friendly = true;  //Tells the game whether it is friendly to players/friendly npcs or not
            Projectile.ignoreWater = true;  //Tells the game whether or not projectile will be affected by water
            Projectile.minion = true;  //Tells the game whether it is a ranged projectile or not
            Projectile.penetrate = 1; //Tells the game how many enemies it can hit before being destroyed, -1 infinity
            Projectile.timeLeft = 125;  //The amount of time the projectile is alive for
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            Projectile.damage = 12;
            Projectile.knockBack = 3;
            for (int i = 0; i < 5; i++)    //this defines how many dust to spawn
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Cloud);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
    }
}