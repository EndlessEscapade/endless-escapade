using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria.ID;
using EEMod.Prim;
namespace EEMod.Projectiles.Ranged
{
    public class BlitzBubble : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubble");
        }

        public override void SetDefaults()
		{
			projectile.aiStyle = -1;
			projectile.width = 32;
			projectile.height = 32;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.hostile = false;
            projectile.ranged = true;
            projectile.penetrate = 1;
			projectile.timeLeft = 200;
			projectile.alpha = 110;
		}

		public override void AI()
		{
			if (projectile.timeLeft == 200) {
				projectile.scale = Main.rand.NextFloat(0.95f, 1.35f);
			}
			projectile.velocity.X *= 0.99f;
			projectile.velocity.Y -= 0.015f;
            var list = Main.projectile.Where(x => x.Hitbox.Intersects(projectile.Hitbox));
			foreach (var proj in list) {
                if (proj.ranged && proj.active && proj.friendly && !proj.hostile && (proj.width <= 6 || proj.height <= 6)) 
                {
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().Shake = 6;
                    projectile.timeLeft = 1;
                    CombatText.NewText(new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height), new Color(255, 155, 0, 100),
                    "Blitz!");
                    for (int i = 0; i < 20; i++) {
                        int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 165, 0f, -2f, 0, default(Color), 2f);
                        Main.dust[num].noGravity = true;
                        Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                        Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                        Main.dust[num].scale *= .4f;
                        if (Main.dust[num].position != projectile.Center)
                            Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 8f;
                    }
                    if (Main.netMode != NetmodeID.Server)
                    {
                        EEMod.primitives.CreateTrail(new BubbleBlitzerPrimTrail(proj));
                    }
                    proj.damage = (int)(proj.damage * 1.71f);
                }
            }

            projectile.rotation = projectile.velocity.X / 32;
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 54);
			for (int i = 0; i < 20; i++) {
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 165, 0f, -2f, 0, default(Color), 2f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].scale *= .4f;
				if (Main.dust[num].position != projectile.Center)
					Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 3f;
			}
		}
    }
}