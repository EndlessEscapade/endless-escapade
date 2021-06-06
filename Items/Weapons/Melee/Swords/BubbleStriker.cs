using EEMod.Items.Weapons.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace EEMod.Items.Weapons.Melee.Swords
{
	public class BubbleStriker : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bubble Striker");
		}

		public override void SetDefaults()
		{
			item.damage = 20;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useAnimation = 25;
			item.useTime = 25;
			item.shootSpeed = 0;
			item.knockBack = 6.5f;
			item.width = 46;
			item.height = 48;
			item.scale = 1f;
			item.rare = ItemRarityID.Purple;
			item.value = Item.sellPrice(silver: 10);

			item.melee = true;
			item.autoReuse = false;

			item.UseSound = SoundID.Item1;
		}
		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			for (int i = 0; i < 5; i++)
			{
				Projectile.NewProjectileDirect(target.Center, Main.rand.NextFloat(6.28f).ToRotationVector2() * new Vector2(8, 0), ModContent.ProjectileType<BubbleStrikerProj>(), item.damage, item.knockBack, player.whoAmI, target.whoAmI).scale = Main.rand.NextFloat(0.85f, 1.15f);
			}
		}
	}
	public class BubbleStrikerProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bubble");
		}
		public override void SetDefaults()
		{
			projectile.penetrate = 1;
			projectile.tileCollide = true;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.width = projectile.height = 32;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			projectile.timeLeft = 100;
			projectile.usesLocalNPCImmunity = true;
			projectile.ownerHitCheck = true;
		}
		int enemyID;
		public bool stuck = false;
		Vector2 offset = Vector2.Zero;
		const int EXPLOSIONRADIUS = 50;
		public bool pop = false;
		//TODO: Move methods to top + method breaks

		//TODO: Turn needles into getnset
		public override void AI()
		{
			if (stuck)
			{
				NPC target = Main.npc[enemyID];
				if ((!target.active || pop) && !projectile.friendly)
				{
					if (Main.LocalPlayer.GetModPlayer<EEPlayer>().Shake < 20)
						Main.LocalPlayer.GetModPlayer<EEPlayer>().Shake +=1;
					for (int i = 0; i < 20; i++)
					{
						int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.FungiHit, 0f, -2f, 0, default(Color), 2f);
						Main.dust[num].noGravity = true;
						Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].scale *= .4f;
						if (Main.dust[num].position != projectile.Center)
							Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 8f;
					}
					projectile.alpha = 255;
					if (projectile.timeLeft > 2)
						projectile.timeLeft = 2;
					projectile.friendly = true;
					projectile.penetrate = -1;

					projectile.position -= new Vector2(EXPLOSIONRADIUS, EXPLOSIONRADIUS);
					projectile.width = EXPLOSIONRADIUS * 2;
					projectile.height = EXPLOSIONRADIUS * 2;
					Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 54);
					for (int i = 0; i < Main.projectile.Length; i++)
					{
						Projectile proj = Main.projectile[i];
						if (proj.modProjectile is BubbleStrikerProj bubble && proj.owner == projectile.owner && bubble.stuck)
							bubble.pop = true;
					}
				}
				else
				{
					projectile.position = target.position + offset;
				}
			}
			else
			{
				projectile.velocity.X *= 0.99f;
				projectile.velocity.Y -= 0.015f;
				projectile.rotation = projectile.velocity.X / 32;
			}
		}
		public override bool? CanHitNPC(NPC target)
		{
			if (stuck && target.immune[projectile.owner] <= 0 && projectile.localNPCImmunity[target.whoAmI] <= 0)
				return base.CanHitNPC(target);
			if (!stuck && target.life > 0 && !target.friendly && target.whoAmI != projectile.ai[0] && Collision.CheckAABBvAABBCollision(target.position, new Vector2(target.width, target.height), projectile.position, new Vector2(projectile.width, projectile.height)))
			{
				if (Main.rand.Next(Math.Max(target.GetGlobalNPC<BubbleStrikerNPC>().bubbles, 1)) == 0)
				{
					projectile.penetrate++;
					stuck = true;
					projectile.friendly = false;
					projectile.tileCollide = false;
					enemyID = target.whoAmI;
					offset = projectile.position - target.position;
					offset -= projectile.velocity;
					projectile.timeLeft = 400;
					target.GetGlobalNPC<BubbleStrikerNPC>().bubbles++;
				}
			}
			return false;
		}
        public override void Kill(int timeLeft)
        {
			if (stuck)
			{
				NPC target = Main.npc[enemyID];
				target.GetGlobalNPC<BubbleStrikerNPC>().bubbles--;
			}
			base.Kill(timeLeft);
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			crit = true;
			base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			int cooldown = 20;
			projectile.localNPCImmunity[target.whoAmI] = 20;
			target.immune[projectile.owner] = cooldown;
		}
	}
	public class BubbleStrikerNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;
		public int bubbles = 1;
	}
}