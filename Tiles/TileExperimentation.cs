using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Tiles
{
    public class TileExperimentation : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tile Experimentation");
        }

        public override void SetDefaults()
        {
            projectile.width = 64;
            projectile.height = 16;
            projectile.alpha = 0;
            projectile.timeLeft = 10000;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale *= 1f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (projectile.ai[0] >= 10 && !canspawn)
            {
                canspawn = true;
                Helpers.DrawBezierProj(Main.LocalPlayer.Center - new Vector2(200, 200), Main.LocalPlayer.Center, Main.LocalPlayer.Center - new Vector2(120, 100), Main.LocalPlayer.Center - new Vector2(120, 100), 0.015f, (float)Math.PI, ModContent.ProjectileType<Bridge>(), true);
            }

            return true;
        }
        bool canspawn = false;
        public override void AI()
        {
            projectile.ai[0] += 0.1f;
            projectile.velocity.Y += (float)Math.Sin(projectile.ai[0]) * 0.1f;
            Rectangle upperPortion = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, 3);
            Rectangle lowerPortion = new Rectangle((int)projectile.position.X, (int)projectile.position.Y + projectile.height - 2, projectile.width, 2);
            Rectangle playerHitBoxFeet = new Rectangle((int)Main.LocalPlayer.position.X, (int)Main.LocalPlayer.position.Y + Main.LocalPlayer.height - (int)(Main.LocalPlayer.velocity.Y / 2), Main.LocalPlayer.width, (int)Math.Round(projectile.velocity.Y) + (int)(Main.LocalPlayer.velocity.Y / 2));

            if (playerHitBoxFeet.Intersects(upperPortion) && Main.LocalPlayer.velocity.Y >= 0)
            {
                Main.LocalPlayer.velocity.Y = 0;
                Main.LocalPlayer.bodyFrame.Y = 0;
                Main.LocalPlayer.legFrame.Y = 0;
                Main.LocalPlayer.position.Y = projectile.position.Y - Main.LocalPlayer.height + 1;
            }
        }

    }
}

