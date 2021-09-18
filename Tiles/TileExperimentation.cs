using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using EEMod.Tiles.Furniture;

namespace EEMod.Tiles
{
    public class TileExperimentation : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tile Experimentation");
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 16;
            Projectile.alpha = 0;
            Projectile.timeLeft = 10000;
            Projectile.penetrate = -1;
            // Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            // Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale *= 1f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (Projectile.ai[0] >= 10 && !canspawn)
            {
                canspawn = true;
                Helpers.DrawBezierProj(Main.LocalPlayer.Center - new Vector2(200, 200), Main.LocalPlayer.Center, Main.LocalPlayer.Center - new Vector2(120, 100), Main.LocalPlayer.Center - new Vector2(120, 100), 0.015f, MathHelper.Pi, ModContent.ProjectileType<Bridge>(), true);
            }

            return true;
        }

        private bool canspawn = false;

        public override void AI()
        {
            Projectile.ai[0] += 0.1f;
            Projectile.velocity.Y += (float)Math.Sin(Projectile.ai[0]) * 0.1f;
            Rectangle upperPortion = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, 3);
            Rectangle lowerPortion = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y + Projectile.height - 2, Projectile.width, 2);
            Rectangle playerHitBoxFeet = new Rectangle((int)Main.LocalPlayer.position.X, (int)Main.LocalPlayer.position.Y + Main.LocalPlayer.height - (int)(Main.LocalPlayer.velocity.Y / 2), Main.LocalPlayer.width, (int)Math.Round(Projectile.velocity.Y) + (int)(Main.LocalPlayer.velocity.Y / 2));

            if (playerHitBoxFeet.Intersects(upperPortion) && Main.LocalPlayer.velocity.Y >= 0)
            {
                Main.LocalPlayer.velocity.Y = 0;
                Main.LocalPlayer.bodyFrame.Y = 0;
                Main.LocalPlayer.legFrame.Y = 0;
                Main.LocalPlayer.position.Y = Projectile.position.Y - Main.LocalPlayer.height + 1;
            }
        }
    }
}