using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Prophecy.Weapons
{
    public class QuartzProj : ModProjectile
    {
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
            projectile.timeLeft = 300;
        }

        public override void AI()
        {
            projectile.rotation += projectile.velocity.X / 7;
            projectile.velocity.X *= 0.991f;
            projectile.velocity.Y *= 0.991f;

            projectile.ai[0]++;
            if (projectile.ai[0] % 30 == 0)
            {
                for (var i = 0; i < 5; i++)
                {
                    int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 123, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-1f, 1f), 6, new Color(255, 217, 184, 255), projectile.scale * 0.5f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].velocity *= 2.5f;
                    Main.dust[num].noLight = false;
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
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Quartz");
        }
    }
}
