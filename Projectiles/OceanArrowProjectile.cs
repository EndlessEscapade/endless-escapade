using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace EEMod.Projectiles
{
    public class OceanArrowProjectile : VisualProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Quartz");
        }

        public override void SetDefaults()
        {
            Projectile.width = 36;       //projectile width
            Projectile.height = 46;  //projectile height
            Projectile.friendly = true;      //make that the projectile will not damage you
            // Projectile.tileCollide = false;   //make that the projectile will be destroed if it hits the terrain
            Projectile.penetrate = -1;      //how many npc will penetrate
            Projectile.alpha = 255;                               //how many time this projectile has before disepire
            Projectile.light = 0;    // projectile light
            Projectile.ignoreWater = true;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 1000000000;
        }

        public override void AI()
        {
            Projectile.ai[0] += 0.1f;
            Projectile.position.X = Main.player[Projectile.owner].Center.X - Projectile.width / 2 + (float)Math.Sin(Projectile.ai[0]) * 10 - 100;
            Projectile.position.Y = Main.player[Projectile.owner].Center.Y;
            if (Projectile.ai[1] == 0)
            {
                Projectile.alpha += 5;
            }
            else
            {
                Projectile.alpha -= 5;
            }

            Helpers.Clamp(ref Projectile.alpha, 0, 255);
        }

        public override bool PreDraw(ref Color lightColor)  //this make the projectile sprite rotate perfectaly around the player
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.Cyan * ((255f - Projectile.alpha) / 255f), Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1f, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            return false;
        }
    }
}