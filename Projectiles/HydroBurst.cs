using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles
{
    public class HydroBurst : ModProjectile
    {
        public override string Texture => Helpers.EmptyTexture;

        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.hostile = false;
            projectile.friendly = true;  //Tells the game whether it is friendly to players/friendly npcs or not
            projectile.ignoreWater = true;  //Tells the game whether or not projectile will be affected by water
            projectile.minion = true;  //Tells the game whether it is a ranged projectile or not
            projectile.penetrate = 1; //Tells the game how many enemies it can hit before being destroyed, -1 infinity
            projectile.timeLeft = 125;  //The amount of time the projectile is alive for
            projectile.tileCollide = true;
        }

        public override void AI()
        {
            projectile.damage = 12;
            projectile.knockBack = 3;
            for (int i = 0; i < 5; i++)    //this defines how many dust to spawn
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 16);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.Kill();
            return false;
        }
    }
}