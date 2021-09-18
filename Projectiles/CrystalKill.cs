using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace EEMod.Projectiles    //We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class CrystalKill : EEProjectile
    {
        public override string Texture => Helpers.EmptyTexture;

        public override void SetDefaults()
        {
            Projectile.width = 12;  //Set the hitbox width
            Projectile.height = 12;
            // Projectile.hostile = false;//Set the hitbox height
            Projectile.friendly = true;  //Tells the game whether it is friendly to players/friendly npcs or not
            Projectile.ignoreWater = true;  //Tells the game whether or not projectile will be affected by water
            Projectile.DamageType = DamageClass.Ranged;  //Tells the game whether it is a ranged projectile or not
            Projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed, -1 infinity
            Projectile.timeLeft = 50;  //The amount of time the projectile is alive for
            Projectile.tileCollide = true;
            Projectile.aiStyle = 21;
        }

        public int hits;

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            hits++;
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }

            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }

            if (hits == 5)
            {
                Projectile.Kill();
            }

            return false;
        }

        private float scale = 1;

        public override void AI()
        {
            Projectile.velocity.Y += 0.15f;
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.15f / 255f, (255 - Projectile.alpha) * 0.45f / 255f, (255 - Projectile.alpha) * 0.05f / 255f);   //this is the light colors
            if (Projectile.timeLeft > 125)
            {
                Projectile.timeLeft = 125;
            }
            if (Projectile.ai[0] > 1f)  //this defines where the flames starts
            {
                scale *= 0.97f;
                for (int i = 0; i < 8; i++)    //this defines how many dust to spawn
                {
                    int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.YellowTorch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 0, new Color(255, 255, 153), 1);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].scale = scale;
                    //  int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.BlueCrystalShard, projectile.velocity.X * 1.2f, projectile.velocity.Y * 1.2f, DustID.BlueCrystalShard, new Color(255, 255, 153), 1); //this defines the flames dust and color parcticles, like when they fall thru ground, change DustID to wat dust you want from Terraria
                }
            }
            else
            {
                Projectile.ai[0] += 1f;
            }
            return;
        }

        public override void Kill(int timeLeft)
        {
            for (var i = 0; i < 20; i++)
            {
                int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.YellowTorch, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-1f, 1f), 6, new Color(255, 255, 153, 255), 2f);
                Main.dust[num].noGravity = true;
                Main.dust[num].velocity *= 1.2f;
                // Main.dust[num].noLight = false;
            }
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
        }

        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            player.AddBuff(BuffID.OnFire, 120);
            player.AddBuff(BuffID.Frostburn, 120);
            player.AddBuff(BuffID.CursedInferno, 120);				//this make so when the projectile/flame hit a npc, gives it the buff  onfire , 80 = 3 seconds
        }
    }
}