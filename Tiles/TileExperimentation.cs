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

        public Vector2 pos1;
        public Vector2 pos2;

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.timeLeft = 10000;
            if (!canspawn)
            {
                canspawn = true;

                Helpers.DrawBezierProj(pos1, pos2, ((pos2 + pos1) / 2f) + new Vector2(0, 2), ((pos2 + pos1) / 2f) + new Vector2(0), 0.04f, MathHelper.Pi, ModContent.ProjectileType<Bridge>(), true);
            }

            return false;
        }

        public override void Kill(int timeLeft)
        {
            Projectile.timeLeft = 10000;
        }

        private bool canspawn = false;

        /*public override void AI()
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
        }*/
    }
}