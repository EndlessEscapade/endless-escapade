using System;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Projectiles;

namespace EEMod.Items
{
    public class TennisRachetProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grad");
        }

        public override void SetDefaults()
        {
            projectile.width = 114;
            projectile.height = 50;
            projectile.timeLeft = 600;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale *= 1;
            projectile.alpha = 255;
        }
        int frame;
        int numOfFrames = 8;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = Main.projectileTexture[projectile.type];
            Main.spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, new Rectangle(0,(tex.Height / numOfFrames) * frame, tex.Width,tex.Height/numOfFrames), lightColor * (1 -(projectile.alpha/255f)), projectile.rotation, new Rectangle(0, (tex.Height / numOfFrames) * frame, tex.Width, tex.Height / numOfFrames).Size()/2, projectile.scale, projectile.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            return false;
        }
        Projectile gottenBoll;
        Vector2 pos;
        int coolDown;
        public override void AI()
        {
            if(coolDown > 0)
            {
                coolDown--;
            }
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                if (Main.projectile[i].type == ModContent.ProjectileType<TenisBoll>()
                    && Main.projectile[i].active
                    && (Main.projectile[i].Center - projectile.Center).Length() < (Main.projectile[i].Center - pos).Length())
                {
                    gottenBoll = Main.projectile[i];
                }
            }
                Rectangle RacketFrame = new Rectangle((int)projectile.position.X + 10, (int)projectile.position.Y + 2, projectile.width - 10, projectile.height - 2);
                Rectangle BallFrame = new Rectangle((int)gottenBoll.Center.X, (int)gottenBoll.Center.Y, 6, 6);
                if (RacketFrame.Intersects(BallFrame) && gottenBoll != null && coolDown == 0)
                {
                    coolDown = 40;
                    gottenBoll.velocity = projectile.velocity;
                }
            Player player = Main.player[projectile.owner];
            if (projectile.alpha > 0)
            projectile.alpha--;
            float radial = 75;
            float inverseSpeed = 100;
            float dampeningEffect = 0.07f;
            projectile.timeLeft = 100;
            Vector2 goTo = Main.MouseWorld;
            if (player.direction == 1)
            {
                frame = (int)((projectile.Center.X - player.Center.X) / radial * (numOfFrames * 0.5f) + (numOfFrames * 0.5f));
                projectile.rotation = 0;
            }
            else
            {
                frame = (int)((player.Center.X - projectile.Center.X) / radial * (numOfFrames * 0.5f) + (numOfFrames * 0.5f));
                projectile.rotation = (float)Math.PI;
            }
            frame = (int)MathHelper.Clamp(frame, 0, numOfFrames - 1);
            if (goTo.X < player.Center.X - radial)
            {
                goTo.X = player.Center.X - radial;
            }
            if (goTo.X > player.Center.X + radial)
            {
                goTo.X = player.Center.X + radial;
            }
            if (goTo.Y < player.Center.Y - radial)
            {
                goTo.Y = player.Center.Y - radial;
            }
            if (goTo.Y > player.Center.Y + radial)
            {
                goTo.Y = player.Center.Y + radial;
            }
            projectile.velocity += (goTo - projectile.Center) / inverseSpeed - (projectile.velocity * dampeningEffect);
            
        }
    }
}
