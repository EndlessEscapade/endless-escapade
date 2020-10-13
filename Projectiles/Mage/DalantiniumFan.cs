using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Mage
{
    public class DalantiniumFan : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dalantinium Fan");
        }

        public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.magic = true;
            projectile.width = 34;
            projectile.height = 34;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.penetrate = 1;
            projectile.tileCollide = false;
            projectile.timeLeft = 999999;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        private Vector2 direction = Vector2.Zero;
        private int degrees = 0;
        public Vector2 DrawPos;
        private float lerp;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            double radians = degrees.ToRadians();
            Player player = Main.player[projectile.owner];
            direction.Normalize();
            Vector2 direction2 = direction * 4;
            direction *= (float)(Math.Sin(projectile.ai[0] * 0.2f) * 3);
            if (projectile.ai[0] % 10 == 1)
            {
                Projectile.NewProjectile(player.Center + (direction2 * 5), new Vector2((float)Math.Sin(-radians - 1.57), (float)Math.Cos(-radians - 1.57)) * 10, ModContent.ProjectileType<DalantiniumFang>(), projectile.damage, projectile.knockBack, projectile.owner);
            }
            if (boost > 2000 && boost % 1500 <= 200)
            {
                lerp += 0.1f;
                //port to Lerp method
                lightColor.R = (byte)(lightColor.R + ((Color.White.R * 10) - lightColor.R) * lerp);
                lightColor.G = (byte)(lightColor.G + ((Color.White.G * 10) - lightColor.G) * lerp);
                lightColor.B = (byte)(lightColor.B + ((Color.White.B * 10) - lightColor.B) * lerp);
            }
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k].ForDraw() + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color2 = projectile.GetAlpha(lightColor) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length / 2);
                if (boost > 2000 && boost % 4000 <= 200)
                {
                    lerp += 0.1f;
                    color2.R = (byte)(color2.R + (Color.HotPink.R - color2.R) * lerp);
                    color2.G = (byte)(color2.G + (Color.HotPink.G - color2.G) * lerp);
                    color2.B = (byte)(color2.B + (Color.HotPink.B - color2.B) * lerp);
                }
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle(0, 0, projectile.width, projectile.height), color2 * 0.5f, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public int boost;
        public bool chungus;

        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            player.heldProj = projectile.whoAmI;
            if (projectile.ai[0] == 0)
            {
                direction = Main.MouseWorld - (player.Center - new Vector2(4, 4));
                direction.Normalize();
                direction *= 7f;
                degrees = (int)((direction.ToRotation() - MathHelper.Pi) * 57);
                int chooser = Main.rand.Next(0, 2);
                if (chooser == 0)
                {
                    projectile.ai[1] = Main.rand.Next(-11, -8);
                }

                if (chooser == 1)
                {
                    projectile.ai[1] = Main.rand.Next(8, 11);
                }

                degrees -= (int)projectile.ai[1] * 8;

                projectile.netUpdate = true;
            }
            projectile.ai[0]++;
            if (projectile.ai[0] < 15)
            {
                degrees += (int)projectile.ai[1];
            }
            else
            {
                player.itemAnimation = 1;
                player.itemTime = 1;
                projectile.active = false;
            }
            if (player.itemAnimation <= 0)
            {
                player.itemAnimation = 1;
                player.itemTime = 1;
            }

            DrawPos = Main.player[projectile.owner].Center + (degrees + 180).ToRadians().ToRotationVector2() * 50;
            projectile.Center = Main.player[projectile.owner].Center + (degrees + 180).ToRadians().ToRotationVector2() * 15;
            projectile.rotation = degrees.ToRadians() + 3.9f;
            return true;
        }
    }
}
