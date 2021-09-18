using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.Bosses.Kraken    //We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class InkSpew : EEProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 12;  //Set the hitbox width
            Projectile.height = 12;
            Projectile.hostile = true;//Set the hitbox height
            // Projectile.friendly = false;  //Tells the game whether it is friendly to players/friendly npcs or not
            Projectile.ignoreWater = true;  //Tells the game whether or not projectile will be affected by water
            Projectile.DamageType = DamageClass.Ranged;  //Tells the game whether it is a ranged projectile or not
            Projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed, -1 infinity
            Projectile.timeLeft = 27;  //The amount of time the projectile is alive for
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            if (Projectile.timeLeft > 60)
            {
                Projectile.timeLeft = 60;
            }
            if (Projectile.ai[1] == 1)
            {
                Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.15f / 255f, (255 - Projectile.alpha) * 0.15f / 255f, (255 - Projectile.alpha) * .6f / 255f);   //this is the light colors

                if (Projectile.ai[0] > 1f)
                {
                    if (Main.rand.Next(4) == 0)
                    {
                        int dust = Dust.NewDust(Projectile.position, 1, 1, DustID.Blood, Projectile.velocity.X * 1.2f, Projectile.velocity.Y, 0, Color.Black, 3.5f);
                        Main.dust[dust].noGravity = true;
                        int dust2 = Dust.NewDust(Projectile.position, 1, 1, DustID.Blood, Projectile.velocity.X * 1.2f, Projectile.velocity.Y, 9, Color.Black, 3f);
                        Main.dust[dust2].noGravity = true;
                    }
                }
                else
                {
                    Projectile.ai[0]++;
                }
            }
            return;
        }

        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
    }
}