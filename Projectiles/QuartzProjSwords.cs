using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace EEMod.Projectiles
{
    public class QuartzProjSwords : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Quartz");
        }

        public override void SetDefaults()
        {
            projectile.width = 12;       //projectile width
            projectile.height = 12;  //projectile height
            projectile.friendly = true;      //make that the projectile will not damage you
            projectile.magic = true;     //
            projectile.tileCollide = true;   //make that the projectile will be destroed if it hits the terrain
            projectile.penetrate = 1;      //how many npc will penetrate
                                           //how many time this projectile has before disepire
            projectile.light = 0.3f;    // projectile light
            projectile.ignoreWater = true;
            projectile.aiStyle = 0;
            projectile.timeLeft = 150;
        }

        public override void AI()
        {
            projectile.rotation += projectile.velocity.X / 7;
            projectile.velocity.X *= 0.961f;
            projectile.velocity.Y *= 0.961f;

            projectile.ai[0]++;
            if (projectile.ai[0] % 10 == 0)
            {
                for (var i = 0; i < 5; i++)
                {
                    int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 123, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f), 6, new Color(255, 217, 184, 255), projectile.scale * 0.5f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].velocity *= 2.5f;
                    Main.dust[num].noLight = false;
                }
            }
            for (int j = 0; j < 200; j++)
            {

                if (Main.projectile[j].type == ModContent.ProjectileType<QuartzProjSwords>() && Main.projectile[j].type != projectile.whoAmI && projectile.velocity.Length() < 5)
                {
                    float oldVel = (float)Math.Sqrt(Main.projectile[j].velocity.X * Main.projectile[j].velocity.X + Main.projectile[j].velocity.Y * Main.projectile[j].velocity.Y);
                    float vel = (float)projectile.velocity.Length();
                    float distX = Main.projectile[j].position.X - projectile.position.X;
                    float distY = Main.projectile[j].position.Y - projectile.position.Y;
                    float dist = (float)Math.Sqrt(distX * distX + distY * distY);
                    if (dist < projectile.width && j != projectile.whoAmI && oldVel > vel)
                    {
                        for (var i = 0; i < 10; i++)
                        {
                            int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 123, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f), 6, new Color(255, 217, 184, 255), projectile.scale * 0.5f);
                            Main.dust[num].noGravity = true;
                            Main.dust[num].velocity *= 2.5f;
                            Main.dust[num].noLight = false;
                        }
                        projectile.timeLeft = 20;
                        projectile.velocity += Main.projectile[j].velocity / 3;
                        Main.projectile[j].velocity *= -1f;
                    }
                }

            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.timeLeft = 10;
            projectile.alpha = 255;
            projectile.light = 0.6f;
            return false;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item27, projectile.position);
            for (var i = 0; i < 20; i++)
            {
                int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 123, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-1f, 1f), 6, new Color(255, 217, 184, 255), projectile.scale * 0.5f);
                Main.dust[num].noGravity = true;
                Main.dust[num].velocity *= 2.5f;
                Main.dust[num].noLight = false;
            }
        }
    }
}
