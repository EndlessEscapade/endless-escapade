using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Ranged
{
    public class DalantiniumSpike : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dalantinium Spike");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 12;
            projectile.timeLeft = 600;
            projectile.ignoreWater = true;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.penetrate = -1;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Dust.NewDust(projectile.Center, 0, 0, DustID.Blood, 0, 0, 0, Color.Gray, 1);

            return true;
        }

        public override void AI()
        {
            projectile.velocity *= 1.005f;

            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;

            projectile.ai[0]++;
            if (projectile.ai[0] % 15 == 0)
            {
                for (int i = 0; i < 360; i += 10)
                {
                    float xdist = (float)(Math.Sin(i * (Math.PI / 180)) * 2);
                    float ydist = (float)(Math.Cos(i * (Math.PI / 180)) * 1);
                    Vector2 offset = new Vector2(xdist, ydist).RotatedBy(projectile.rotation);
                    Dust dust = Dust.NewDustPerfect(projectile.Center, 219, offset * 0.5f, 0, Color.Red);
                    dust.noGravity = true;
                    dust.velocity *= 0.94f;
                    dust.noLight = false;
                    dust.fadeIn = 1f;
                }
            }
        }

        /*public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (projectile.ai[0] >= 120)
                AfterImage.DrawAfterimage(spriteBatch, Main.npcTexture[projectile.type], 0, projectile, 1.5f, 1f, 3, false, 0f, 0f, new Color(drawColor.R, drawColor.G, drawColor.B, 150));
            return true;
        }*/
    }
}