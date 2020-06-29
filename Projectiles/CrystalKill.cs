using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.Projectiles    //We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class CrystalKill : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 12;  //Set the hitbox width
            projectile.height = 12;
            projectile.hostile = false;//Set the hitbox height
            projectile.friendly = true;  //Tells the game whether it is friendly to players/friendly npcs or not
            projectile.ignoreWater = true;  //Tells the game whether or not projectile will be affected by water
            projectile.ranged = true;  //Tells the game whether it is a ranged projectile or not
            projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed, -1 infinity
            projectile.timeLeft = 50;  //The amount of time the projectile is alive for
            projectile.tileCollide = true;
            projectile.aiStyle = 21;
        }

        public int hits;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            hits++;
            if (projectile.velocity.X != oldVelocity.X)
                projectile.velocity.X = -oldVelocity.X;

            if (projectile.velocity.Y != oldVelocity.Y)
                projectile.velocity.Y = -oldVelocity.Y;

            if (hits == 5) projectile.Kill();

            return false;
        }

        private float scale = 1;
        public override void AI()
        {
            projectile.velocity.Y += 0.15f;
            Lighting.AddLight(projectile.Center, ((255 - projectile.alpha) * 0.15f) / 255f, ((255 - projectile.alpha) * 0.45f) / 255f, ((255 - projectile.alpha) * 0.05f) / 255f);   //this is the light colors
            if (projectile.timeLeft > 125)
            {
                projectile.timeLeft = 125;
            }
            if (projectile.ai[0] > 1f)  //this defines where the flames starts
            {
                scale *= 0.97f;
                for (int i = 0; i < 8; i++)    //this defines how many dust to spawn
                {
                    int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 64, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 0, new Color(255, 255, 153), 1);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].scale = scale;
                    //  int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.BlueCrystalShard, projectile.velocity.X * 1.2f, projectile.velocity.Y * 1.2f, DustID.BlueCrystalShard, new Color(255, 255, 153), 1); //this defines the flames dust and color parcticles, like when they fall thru ground, change DustID to wat dust you want from Terraria
                }
            }
            else
            {
                projectile.ai[0] += 1f;
            }
            return;
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 20; i++)
            {
                int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 64, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-1f, 1f), 6, new Color(255, 255, 153, 255), 2f);
                Main.dust[num].noGravity = true;
                Main.dust[num].velocity *= 1.2f;
                Main.dust[num].noLight = false;
            }
            Main.PlaySound(SoundID.Item27, projectile.position);
        }

        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            player.AddBuff(BuffID.OnFire, 120);
            player.AddBuff(BuffID.Frostburn, 120);
            player.AddBuff(BuffID.CursedInferno, 120);				//this make so when the projectile/flame hit a npc, gives it the buff  onfire , 80 = 3 seconds
        }
    }
}
