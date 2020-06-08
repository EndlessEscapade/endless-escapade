using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Pets.Light
{
	public class FlameSpirit : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Green Flame Spirit");
			Main.projFrames[projectile.type] = 9;
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
			ProjectileID.Sets.LightPet[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 28;
			projectile.penetrate = -1;
			projectile.netImportant = true;
			projectile.timeLeft *= 5;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.scale = 0.9f;
			projectile.tileCollide = false;
		}

		private const int fadeInTicks = 30;
		private const int fullBrightTicks = 200;
		private const int fadeOutTicks = 30;
		private const int range = 500;
		// private readonly int rangeHypoteneus = (int)Math.Sqrt(range * range + range * range);

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			EEPlayer modPlayer = player.GetModPlayer<EEPlayer>();
			if (!player.active)
			{
				projectile.active = false;
				return;
			}
			if (player.dead)
			{
				modPlayer.FlameSpirit = false;
                return;
			}
			if (modPlayer.FlameSpirit)
				projectile.timeLeft = 2;
			// projectile.ai[1]++;
			if (++projectile.ai[1] > 1000 && (int)projectile.ai[0] % 100 == 0)
			{
				for (int i = 0; i < Main.npc.Length - 1; i++)
				{
                    // player.Distance(Main.npc[i].Center) < rangeHypoteneus
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && player.WithinRange(npc.Center, range))
					{
						Vector2 vectorToEnemy = Main.npc[i].Center - projectile.Center;
						projectile.velocity += 10f * Vector2.Normalize(vectorToEnemy);
						projectile.ai[1] = 0f;
						projectile.light = 2f;
						break;
					}
				}
			}
			projectile.rotation += projectile.velocity.X / 20f;
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.9f / 255f, (255 - projectile.alpha) * 0.1f / 255f, (255 - projectile.alpha) * 0.3f / 255f);
			if (projectile.velocity.LengthSquared() > 1f)
			{
				projectile.velocity *= .98f;
			}
            if (projectile.velocity == Vector2.Zero) //.Length() == 0)
			{
                projectile.velocity = new Vector2(2, 0).RotatedByRandom(MathHelper.TwoPi); // Vector2.UnitX.RotatedBy(Main.rand.NextFloat() * Math.PI * 2);
				projectile.netUpdate = true;
				// projectile.velocity *= 2f;
			}
			projectile.ai[0]++;
			if (projectile.ai[0] < fadeInTicks)
			{
				projectile.alpha = (int)(255 - 255 * projectile.ai[0] / fadeInTicks);
			}
			else if (projectile.ai[0] < fadeInTicks + fullBrightTicks)
			{
				projectile.alpha = 0;
				if (Main.rand.NextBool(6))
				{
					int num145 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 73, 0f, 0f, 200, default, 0.8f);
					Main.dust[num145].velocity *= 0.3f;
				}
			}
			else if (projectile.ai[0] < fadeInTicks + fullBrightTicks + fadeOutTicks)
			{
				projectile.alpha = (int)(255 * (projectile.ai[0] - fadeInTicks - fullBrightTicks) / fadeOutTicks);
			}
			else
			{
				projectile.Center = new Vector2(Main.rand.Next((int)player.Center.X - range, (int)player.Center.X + range), Main.rand.Next((int)player.Center.Y - range, (int)player.Center.Y + range));
				projectile.ai[0] = 0;
				Vector2 vectorToPlayer = player.Center - projectile.Center;
				projectile.velocity = 2f * Vector2.Normalize(vectorToPlayer);
                projectile.netUpdate = true; // random velocity sync
			}
			// Vector2.Distance(player.Center, projectile.Center) > rangeHypoteneus
			if (!player.WithinRange(projectile.Center, 500))
			{
				projectile.Center = new Vector2(Main.rand.Next((int)player.Center.X - range, (int)player.Center.X + range), Main.rand.Next((int)player.Center.Y - range, (int)player.Center.Y + range));
				projectile.ai[0] = 0;
				Vector2 vectorToPlayer = player.Center - projectile.Center;
				projectile.velocity += 2f * Vector2.Normalize(vectorToPlayer);
                projectile.netUpdate = true; // random velocity sync
			}
			if ((int)projectile.ai[0] % 100 == 0)
			{
                // MathHelper.ToRadians(90)
                projectile.velocity = projectile.velocity.RotatedByRandom(MathHelper.PiOver2);
                projectile.netUpdate = true; // random direction sync
			}
		}
	}
}