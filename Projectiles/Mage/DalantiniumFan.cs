using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
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
		}

		int counter = 0;
		bool firing = false;
		Vector2 direction = Vector2.Zero;
        int swingMomentum = 0;
        int degrees = 0;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            double radians = degrees * (Math.PI / 180);
            Color color = lightColor;

            Player player = Main.player[projectile.owner];
            Rectangle? sourceRectangle = null;
            direction.Normalize();
            Vector2 direction2 = direction * 4;
            direction *= (float)(Math.Sin(counter * 0.2) * 3);
            if (counter % 5 == 1)
            {
                Projectile.NewProjectile(player.Center + (direction2 * 5), new Vector2((float)Math.Sin((radians * -1) - 1.57), (float)Math.Cos((radians * -1) - 1.57)) * 6, ModContent.ProjectileType<DalantiniumFang>(), projectile.damage, projectile.knockBack, projectile.owner);
            }
            Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center - Main.screenPosition + direction + new Vector2(0, -2) - direction2, sourceRectangle, color, (float)radians + 3.9f, new Vector2(0, 34), 1f, SpriteEffects.None, 0);
            return false;
        }

        public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			player.heldProj = projectile.whoAmI;
			if (counter == 0) {
				direction = Main.MouseWorld - (player.Center - new Vector2(4, 4));
				direction.Normalize();
				direction *= 7f;
                degrees = (int)((direction.ToRotation() - 3.14) * 57);

                swingMomentum = Main.rand.Next(-6, 9);
                degrees -= swingMomentum * 8;
            }
            counter++;
            if (counter < 15)
            {
                degrees += swingMomentum;
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
            return true;
		}
	}
}
