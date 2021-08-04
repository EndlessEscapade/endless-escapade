using EEMod.Items.Weapons.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace EEMod.Items.Weapons.Melee.Swords
{
	public class BubbleStriker : EEItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bubble Striker");
		}

		public override void SetDefaults()
		{
			Item.damage = 20;
			Item.useStyle = ItemUseStyleID.SwingThrow;
			Item.useAnimation = 25;
			Item.useTime = 25;
			Item.shootSpeed = 0;
			Item.knockBack = 6.5f;
			Item.width = 46;
			Item.height = 48;
			Item.scale = 1f;
			Item.rare = ItemRarityID.Purple;
			Item.value = Item.sellPrice(silver: 10);

			Item.melee = true;
			Item.autoReuse = false;

			Item.UseSound = SoundID.Item1;
		}
		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			for (int i = 0; i < 5; i++)
			{
				Projectile.NewProjectileDirect(target.Center, Main.rand.NextFloat(6.28f).ToRotationVector2() * new Vector2(8, 0), ModContent.ProjectileType<BubbleStrikerProj>(), Item.damage, Item.knockBack, player.whoAmI, target.whoAmI).scale = Main.rand.NextFloat(0.85f, 1.15f);
			}
		}
	}
	public class BubbleStrikerProj : EEProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bubble");
		}
		public override void SetDefaults()
		{
			Projectile.penetrate = 1;
			Projectile.tileCollide = true;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.width = Projectile.height = 32;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			Projectile.timeLeft = 100;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.ownerHitCheck = true;
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
				if ((!target.active || pop) && !Projectile.friendly)
				{
					if (Main.LocalPlayer.GetModPlayer<EEPlayer>().Shake < 20)
						Main.LocalPlayer.GetModPlayer<EEPlayer>().Shake +=1;
					for (int i = 0; i < 20; i++)
					{
						int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.FungiHit, 0f, -2f, 0, default(Color), 2f);
						Main.dust[num].noGravity = true;
						Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].scale *= .4f;
						if (Main.dust[num].position != Projectile.Center)
							Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 8f;
					}
					Projectile.alpha = 255;
					if (Projectile.timeLeft > 2)
						Projectile.timeLeft = 2;
					Projectile.friendly = true;
					Projectile.penetrate = -1;

					Projectile.position -= new Vector2(EXPLOSIONRADIUS, EXPLOSIONRADIUS);
					Projectile.width = EXPLOSIONRADIUS * 2;
					Projectile.height = EXPLOSIONRADIUS * 2;
					Main.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 54);
					for (int i = 0; i < Main.projectile.Length; i++)
					{
						Projectile proj = Main.projectile[i];
						if (proj.modProjectile is BubbleStrikerProj bubble && proj.owner == Projectile.owner && bubble.stuck)
							bubble.pop = true;
					}
				}
				else
				{
					Projectile.position = target.position + offset;
				}
			}
			else
			{
				Projectile.velocity.X *= 0.99f;
				Projectile.velocity.Y -= 0.015f;
				Projectile.rotation = Projectile.velocity.X / 32;
			}
		}
		public override bool? CanHitNPC(NPC target)
		{
			if (stuck && target.immune[Projectile.owner] <= 0 && Projectile.localNPCImmunity[target.whoAmI] <= 0)
				return base.CanHitNPC(target);
			if (!stuck && target.life > 0 && !target.friendly && target.whoAmI != Projectile.ai[0] && Collision.CheckAABBvAABBCollision(target.position, new Vector2(target.width, target.height), Projectile.position, new Vector2(Projectile.width, Projectile.height)))
			{
				if (Main.rand.Next(Math.Max(target.GetGlobalNPC<BubbleStrikerNPC>().bubbles, 1)) == 0)
				{
					Projectile.penetrate++;
					stuck = true;
					Projectile.friendly = false;
					Projectile.tileCollide = false;
					enemyID = target.whoAmI;
					offset = Projectile.position - target.position;
					offset -= Projectile.velocity;
					Projectile.timeLeft = 400;
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
			Projectile.localNPCImmunity[target.whoAmI] = 20;
			target.immune[Projectile.owner] = cooldown;
		}
	}
	public class BubbleStrikerNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;
		public int bubbles = 1;
	}
}