using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles
{
    public class VolcanoArrowProj : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Volcano Arrow Proj");
        }

        public override void SetDefaults()
        {
            Projectile.width = 36;       //projectile width
            Projectile.height = 46;  //projectile height
            Projectile.friendly = true;      //make that the projectile will not damage you
            // Projectile.tileCollide = false;   //make that the projectile will be destroed if it hits the terrain
            Projectile.penetrate = -1;      //how many npc will penetrate
            Projectile.alpha = 255;                               //how many time this projectile has before disepire
            Projectile.light = 0.3f;    // projectile light
            Projectile.ignoreWater = true;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 1000000000;
        }

        public bool visible = true;

        public override void AI()
        {
            Projectile.ai[0] += 0.1f;
            Projectile.position.X = Main.player[Projectile.owner].Center.X - Projectile.width / 2;
            Projectile.position.Y = Main.player[Projectile.owner].Center.Y + (float)Math.Sin(Projectile.ai[0]) * 10 - 100;
            if (!visible)
            {
                Projectile.alpha += 5;
            }
            else
            {
                Projectile.alpha -= 5;
            }

            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }

            if (Projectile.alpha > 255)
            {
                Projectile.alpha = 255;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)  //this make the projectile sprite rotate perfectaly around the player
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.Red * (float)((255 - Projectile.alpha) / (double)255), Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1f, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            return false;
        }
    }
}