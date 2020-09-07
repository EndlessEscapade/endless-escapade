using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Mage
{
    public class DalantiniumFanAlt : ModProjectile
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
            projectile.timeLeft = 1000;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k].ForDraw() + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color2 = projectile.GetAlpha(lightColor) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length / 2);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle(0, 0, projectile.width, projectile.height), color2 * 0.5f, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }

        private Vector2 Dist;
        private float lerp;

        public override bool PreAI()
        {
            int firstPhase = 60;
            int secondPhase = 110;
            int thirdPhase = 160;
            projectile.ai[0]++;
            Vector2 dist = projectile.Center - Main.player[projectile.owner].Center;
            if (projectile.ai[0] < firstPhase)
            {
                projectile.velocity += (Main.player[projectile.owner].Center + new Vector2(60, -60) - projectile.Center) / 258f - projectile.velocity * 0.02f;
                projectile.rotation = dist.ToRotation() + MathHelper.Pi  / 4f;
            }
            else if (projectile.ai[0] < secondPhase)
            {
                Dust.NewDust(projectile.position, 30, 30, 123);
                if (projectile.ai[0] == firstPhase)
                {
                    Dist = dist;
                }
                projectile.rotation = dist.ToRotation() + MathHelper.Pi  / 4f + (lerp - 1.57f) / 2f;
                projectile.Center = Main.player[projectile.owner].Center + Dist.RotatedBy(lerp);
                float traverseFunction = (float)Math.Sin((projectile.ai[0] - firstPhase) / ((secondPhase - firstPhase) / 3.14f));
                lerp += (0.1f * traverseFunction * traverseFunction) - 0.02f;
                if (projectile.ai[0] % 3 == 1 && projectile.ai[0] > firstPhase + 10)
                {
                    Projectile.NewProjectile(projectile.Center, Vector2.Normalize(Dist.RotatedBy(lerp)) * 10, ModContent.ProjectileType<DalantiniumFang>(), projectile.damage, projectile.knockBack, projectile.owner);
                    for (int i = 0; i < 360; i += 10)
                    {
                        float xdist = (int)(Math.Sin(i * (Math.PI / 180)) * 2);
                        float ydist = (int)(Math.Cos(i * (Math.PI / 180)) * 5);
                        Vector2 offset = new Vector2(xdist, ydist).RotatedBy(projectile.rotation);
                        Dust dust = Dust.NewDustPerfect(Main.player[projectile.owner].Center + Dist.RotatedBy(lerp) * 1.2f, DustID.PinkFlame, offset * 0.5f);
                        dust.noGravity = true;
                        dust.velocity *= 0.94f;
                        dust.noLight = false;
                        dust.fadeIn = 1f;
                    }
                }
            }
            else if (projectile.ai[0] < thirdPhase)
            {
                if (projectile.ai[0] == secondPhase)
                {
                    Dist = dist;
                }

                projectile.velocity += (Main.player[projectile.owner].Center + new Vector2(0, -100) - projectile.Center) / 200f - projectile.velocity * 0.1f;
                projectile.velocity += new Vector2(Main.rand.NextFloat(-.1f, .1f), Main.rand.NextFloat(-.1f, .1f));
                projectile.rotation += (-MathHelper.Pi  / 4f - projectile.rotation) / 12f;
                if (projectile.ai[0] == thirdPhase - 1)
                {
                    for (int i = 0; i < 360; i += 10)
                    {
                        float xdist = (int)(Math.Sin(i * (Math.PI / 180)) * 10);
                        float ydist = (int)(Math.Cos(i * (Math.PI / 180)) * 10);
                        Vector2 offset = new Vector2(xdist, ydist).RotatedBy(projectile.rotation);
                        Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.PinkFlame, offset * 0.5f);
                        dust.noGravity = true;
                        dust.velocity *= 0.94f;
                        dust.noLight = false;
                        dust.fadeIn = 1f;
                        Projectile.NewProjectile(projectile.Center, offset, ModContent.ProjectileType<DalantiniumFang>(), projectile.damage, projectile.knockBack, projectile.owner);
                    }
                    projectile.Kill();
                }
            }
            return true;
        }
    }
}