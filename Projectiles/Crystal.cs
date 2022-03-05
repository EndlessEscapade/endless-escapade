using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace EEMod.Projectiles
{
    public class Crystal : EEProjectile
    {
        public override string Texture => Helpers.EmptyTexture;

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            // Projectile.hostile = false;
            Projectile.friendly = true;  //Tells the game whether it is friendly to players/friendly npcs or not
            Projectile.ignoreWater = true;  //Tells the game whether or not projectile will be affected by water
            Projectile.DamageType = DamageClass.Ranged;  //Tells the game whether it is a ranged projectile or not
            Projectile.penetrate = 1; //Tells the game how many enemies it can hit before being destroyed, -1 infinity
            Projectile.timeLeft = 125;  //The amount of time the projectile is alive for
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.15f / 255f, (255 - Projectile.alpha) * 0.45f / 255f, (255 - Projectile.alpha) * 0.05f / 255f);   //this is the light colors
            if (Projectile.timeLeft > 125)
            {
                Projectile.timeLeft = 125;
            }
            if (Projectile.ai[0] > 1f)  //this defines where the flames starts
            {
                for (int i = 0; i < 15; i++)    //this defines how many dust to spawn
                {
                    int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.YellowTorch, Projectile.velocity.X * 0.3f, Projectile.velocity.Y * 0.3f, 0, new Color(255, 255, 153), 1);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true;
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
            for (int i = 0; i < 4; i++)
            {
                int projHolder = Main.rand.Next(1);
                float speedX = -(Projectile.velocity.X * Main.rand.NextFloat(-.1f, .8f) + Main.rand.NextFloat(-.4f, 2f));
                float speedY = -(Projectile.velocity.Y * Main.rand.Next(30) * 0.01f + Main.rand.NextFloat(-12f, 12.1f));
                if (projHolder == 0 || projHolder == 1)
                {
                    Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_Parent(Projectile), Projectile.Center.X + speedX, Projectile.Center.Y + speedY, speedX * 1.3f, speedY, ModContent.ProjectileType<CrystalKill>(), (int)(Projectile.damage * 0.7), 0f, Projectile.owner, 0f, 0f);
                }

                SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
            }
            for (var i = 0; i < 20; i++)
            {
                int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.YellowTorch, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-1f, 1f), 6, new Color(255, 255, 153, 255), 2f);
                Main.dust[num].noGravity = true;
                Main.dust[num].velocity *= 1.2f;
                // Main.dust[num].noLight = false;
            }
        }

        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            player.AddBuff(BuffID.OnFire, 120);
            player.AddBuff(BuffID.Frostburn, 120);
            player.AddBuff(BuffID.CursedInferno, 120);				//this make so when the projectile/flame hit a npc, gives it the buff  onfire , 80 = 3 seconds
        }

        //private void LookInDirectionP(Vector2 look) // unused
        //{
        //    float angle = 0.5f * MathHelper.Pi;
        //    if (look.X != 0f)
        //    {
        //        angle = (float)Math.Atan(look.Y / look.X);
        //    }
        //    else if (look.Y < 0f)
        //    {
        //        angle += MathHelper.Pi;
        //    }
        //    if (look.X < 0f)
        //    {
        //        angle += MathHelper.Pi;
        //    }
        //    projectile.rotation = angle;
        //}

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
    }
}