using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Mage
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
            int firstPhase = 30;
            int secondPhase = 45;
            int thirdPhase = 60;
            void circleDustPattern()
            {
                for (int i = 0; i < 360; i += 10)
                {
                    float xdist = (float)(Math.Sin(i * (Math.PI / 180)) * 1);
                    float ydist = (float)(Math.Cos(i * (Math.PI / 180)) * 2);
                    Vector2 offset = new Vector2(xdist, ydist).RotatedBy(projectile.rotation);
                    Dust dust = Dust.NewDustPerfect(Main.player[projectile.owner].Center + Dist.RotatedBy(lerp) * 1.2f, 219, offset * 0.5f, 0, Color.Red);
                    dust.noGravity = true;
                    dust.velocity *= 0.94f;
                    dust.noLight = false;
                    dust.fadeIn = 1f;
                }
            }
            projectile.ai[0]++;
            if (projectile.ai[0] == 1)
            {
                Dist = Vector2.Normalize(Main.MouseWorld - Main.player[projectile.owner].Center) * 100;
            }
            Vector2 dist = projectile.Center - Main.player[projectile.owner].Center;
            if (projectile.ai[0] < firstPhase)
            {
                projectile.velocity += (Main.player[projectile.owner].Center + Dist - projectile.Center) / 202f - projectile.velocity * 0.02f;
                projectile.rotation = dist.ToRotation() + (float)Math.PI / 4f;

            }
            else if (projectile.ai[0] < secondPhase)
            {
                Dust.NewDust(projectile.position, 30, 30, DustID.Fireworks);
                if (projectile.ai[0] == firstPhase)
                {
                    Dist = dist;
                }
                projectile.rotation = dist.ToRotation() + (float)Math.PI / 4f + (lerp - 1.57f) / 2f;
                projectile.Center = Main.player[projectile.owner].Center + Dist.RotatedBy(lerp);
                float traverseFunction = (float)Math.Sin((projectile.ai[0] - firstPhase) / ((secondPhase - firstPhase) / 3.14f));
                lerp += (0.18f * traverseFunction * traverseFunction) - 0.02f;
                if (projectile.ai[0] % 2 == 1 && projectile.ai[0] > firstPhase + 1)
                {
                    Projectile.NewProjectile(projectile.Center, Vector2.Normalize(Dist.RotatedBy(lerp)) * 10, ModContent.ProjectileType<DalantiniumFang>(), projectile.damage, projectile.knockBack, projectile.owner);
                    circleDustPattern();
                }
            }
            else if (projectile.ai[0] < thirdPhase)
            {
                Dust.NewDust(projectile.position, 30, 30, DustID.Water_Crimson);
                projectile.rotation = dist.ToRotation() + (float)Math.PI / 4f + (lerp - 1.57f) / 2f;
                projectile.Center = Main.player[projectile.owner].Center + Dist.RotatedBy(lerp);
                float traverseFunction = (float)Math.Sin((projectile.ai[0] - secondPhase) / ((thirdPhase - secondPhase) / 3.14f));
                lerp -= (0.18f * traverseFunction * traverseFunction);
                if (projectile.ai[0] % 2 == 1 && projectile.ai[0] > secondPhase + 1)
                {
                    Projectile.NewProjectile(projectile.Center, Vector2.Normalize(Dist.RotatedBy(lerp)) * 10, ModContent.ProjectileType<DalantiniumFang>(), projectile.damage, projectile.knockBack, projectile.owner);
                    circleDustPattern();
                }
                if (projectile.ai[0] == thirdPhase - 5)
                {
                    projectile.Kill();
                    for (var i = 0; i < 40; i++)
                    {
                        int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fireworks, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-2f, -1f), 6, default, projectile.scale);
                        Main.dust[num].noGravity = true;
                        Main.dust[num].velocity *= 2.5f;
                        Main.dust[num].noLight = false;
                    }
                }
            }
            return true;
        }
    }
}
