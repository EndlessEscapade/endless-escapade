using EEMod.Buffs.Buffs;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Extensions;

namespace EEMod.Projectiles.Mage
{
    public class AncientBubbleSmall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Bubble");
        }

        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 22;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.timeLeft = 720;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
        }

        public override void AI()
        {
            var list = Main.projectile.Where(x => x.Hitbox.Intersects(projectile.Hitbox));
            foreach (var proj in list)
            {
                if(proj.type == ModContent.ProjectileType<AncientBubbleSmall>() && proj.whoAmI != projectile.whoAmI)
                {
                    Projectile.NewProjectile((projectile.Center + proj.Center) / 2f, projectile.velocity, ModContent.ProjectileType<AncientBubbleMedium>(), (int)(projectile.damage * 1.5f), 1f);
                    proj.Kill();
                    projectile.Kill();
                }
            }
            projectile.velocity.Y -= 0.1f;

            projectile.rotation = projectile.velocity.X / 16f;
        }
    }

    public class AncientBubbleMedium : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Bubble");
        }

        public override void SetDefaults()
        {
            projectile.width = 38;
            projectile.height = 38;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.timeLeft = 720;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
        }

        public override void AI()
        {
            var list = Main.projectile.Where(x => x.Hitbox.Intersects(projectile.Hitbox));
            foreach (var proj in list)
            {
                if (proj.type == ModContent.ProjectileType<AncientBubbleMedium>() && proj.whoAmI != projectile.whoAmI)
                {
                    Projectile.NewProjectile((projectile.Center + proj.Center) / 2f, projectile.velocity, ModContent.ProjectileType<AncientBubbleLarge>(), (int)(projectile.damage * 1.5f), 1f);
                    proj.Kill();
                    projectile.Kill();
                }
            }
            projectile.velocity.Y -= 0.1f;

            projectile.rotation = projectile.velocity.X / 16f;
        }
    }

    public class AncientBubbleLarge : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Bubble");
        }

        public override void SetDefaults()
        {
            projectile.width = 74;
            projectile.height = 74;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.timeLeft = 720;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
        }

        public override void AI()
        {
            if(projectile.Hitbox.Contains(Main.MouseWorld.ToPoint()) && Main.LocalPlayer.controlUseItem)
            {
                Main.NewText("trolled!");
                CombatText.NewText(new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height), new Color(255, 155, 0, 100), "Pop!");
                projectile.Kill();
            }
            projectile.velocity.Y -= 0.1f;

            projectile.rotation = projectile.velocity.X / 16f;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.Draw(mod.GetTexture("Projectiles/Mage/AncientBubbleLargeFlash"), projectile.Center.ForDraw(), projectile.getRect(), Color.White * Math.Sin(Main.GameUpdateCount / 20f).PositiveSin(), projectile.rotation, projectile.getRect().Size() / 2f, projectile.scale, SpriteEffects.None, 0f);
        }
    }
}