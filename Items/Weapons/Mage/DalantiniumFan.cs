using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Mage
{
    public class DalantiniumFan : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dalantinium Fan");
        }

        public override void SetDefaults()
        {
            // Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.aiStyle = -1;
            // Projectile.friendly = false;
            Projectile.penetrate = 1;
            // Projectile.tileCollide = false;
            Projectile.timeLeft = 999999;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        private Vector2 direction = Vector2.Zero;
        private int degrees = 0;
        public Vector2 DrawPos;
        private float lerp;

        public override bool PreDraw(ref Color lightColor)
        {
            double radians = degrees.ToRadians();
            Player player = Main.player[Projectile.owner];
            direction.Normalize();
            Vector2 direction2 = direction * 4;
            direction *= (float)(Math.Sin(Projectile.ai[0] * 0.2f) * 3);
            if (Projectile.ai[0] % 10 == 1)
            {
                Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_Parent(Projectile), player.Center + (direction2 * 5), new Vector2((float)Math.Sin(-radians - 1.57), (float)Math.Cos(-radians - 1.57)) * 10, ModContent.ProjectileType<DalantiniumFang>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            if (boost > 2000 && boost % 1500 <= 200)
            {
                lerp += 0.1f;
                //port to Lerp method
                lightColor.R = (byte)(lightColor.R + ((Color.White.R * 10) - lightColor.R) * lerp);
                lightColor.G = (byte)(lightColor.G + ((Color.White.G * 10) - lightColor.G) * lerp);
                lightColor.B = (byte)(lightColor.B + ((Color.White.B * 10) - lightColor.B) * lerp);
            }
            Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k].ForDraw() + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color2 = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length / 2);
                if (boost > 2000 && boost % 4000 <= 200)
                {
                    lerp += 0.1f;
                    color2.R = (byte)(color2.R + (Color.HotPink.R - color2.R) * lerp);
                    color2.G = (byte)(color2.G + (Color.HotPink.G - color2.G) * lerp);
                    color2.B = (byte)(color2.B + (Color.HotPink.B - color2.B) * lerp);
                }
                Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos, new Rectangle(0, 0, Projectile.width, Projectile.height), color2 * 0.5f, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public int boost;
        public bool chungus;

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            if (Projectile.ai[0] == 0)
            {
                direction = Main.MouseWorld - (player.Center - new Vector2(4, 4));
                direction.Normalize();
                direction *= 7f;
                degrees = (int)((direction.ToRotation() - MathHelper.Pi) * 57);
                int chooser = Main.rand.Next(0, 2);
                if (chooser == 0)
                {
                    Projectile.ai[1] = Main.rand.Next(-11, -8);
                }

                if (chooser == 1)
                {
                    Projectile.ai[1] = Main.rand.Next(8, 11);
                }

                degrees -= (int)Projectile.ai[1] * 8;

                Projectile.netUpdate = true;
            }
            Projectile.ai[0]++;
            if (Projectile.ai[0] < 15)
            {
                degrees += (int)Projectile.ai[1];
            }
            else
            {
                player.itemAnimation = 1;
                player.itemTime = 1;
                // Projectile.active = false;
            }
            if (player.itemAnimation <= 0)
            {
                player.itemAnimation = 1;
                player.itemTime = 1;
            }

            DrawPos = Main.player[Projectile.owner].Center + (degrees + 180).ToRadians().ToRotationVector2() * 50;
            Projectile.Center = Main.player[Projectile.owner].Center + (degrees + 180).ToRadians().ToRotationVector2() * 15;
            Projectile.rotation = degrees.ToRadians() + 3.9f;
            return true;
        }
    }
}
