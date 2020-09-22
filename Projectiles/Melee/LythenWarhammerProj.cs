using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Projectiles.Melee
{
    public class LythenWarhammerProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lythen Warhammer");
        }

        public override void SetDefaults()
        {
            projectile.width = 50;
            projectile.height = 52;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.scale = 1f;

            projectile.melee = true;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.damage = 20;
            projectile.knockBack = 4.5f;
        }

        int nearbyProjectiles = 0;
        public override void AI()
        {
            projectile.ai[0]++;
            nearbyProjectiles = 0;
            if (projectile.ai[0] < 30)
            {
                projectile.rotation += 0.1f;
                projectile.ai[1] = 4.8f;

            }
            if (projectile.ai[0] >= 30 && projectile.ai[0] < 640)
            {
                projectile.velocity *= 0.95f;

                projectile.rotation += (projectile.ai[1] / 48);

                if (projectile.ai[1] < 32) projectile.ai[1] += 0.2f;

                for (int j = 0; j < Main.projectile.Length - 1; j++)
                    if (Main.projectile[j].type == ModContent.ProjectileType<LythenWarhammerProj>() && j != projectile.whoAmI && Vector2.Distance(projectile.Center, Main.projectile[j].Center) <= 768 && Main.projectile[j].ai[0] >= 30 && Main.projectile[j].ai[0] < 640 && projectile.ai[1] >= 32 && Main.projectile[j].ai[1] >= 32 && projectile.ai[0] % 2 == 0)
                    {
                        for (int k = 0; k < 15; k++)
                        {
                            Vector2 normalization = Vector2.Normalize(Main.projectile[j].Center - projectile.Center);
                            Dust dust = Dust.NewDustPerfect(Vector2.Lerp(projectile.Center + (normalization * 48), Main.projectile[j].Center - (normalization * 48), Helpers.Clamp(k + Main.rand.NextFloat(-1, 1), 0, 20) / 20f), 111, Velocity: Vector2.Zero);
                            dust.noGravity = true;
                        }
                        nearbyProjectiles++;
                    }

                projectile.damage *= (int)MathHelper.Clamp(nearbyProjectiles / 3, 1, 1.5f);

                float radius = 48;
                int numLocations = 32;
                if (nearbyProjectiles >= 3)
                {
                    //radius += (float)Math.Sin(projectile.ai[0] / 20) * 8;
                    projectile.scale = Helpers.Clamp((float)Math.Sin(projectile.ai[0] / 20) + 1, 0.9f, 1.1f);
                }
                else
                {
                    //radius = 48;
                    projectile.scale = 1;
                }

                for (int i = 0; i < projectile.ai[1]; i++)
                {
                    Vector2 position = projectile.Center + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / numLocations * i) + Main.rand.NextFloat(-0.1f, 0.1f)) * radius;
                    Dust dust = Dust.NewDustPerfect(position, 111);
                    dust.velocity = Vector2.Zero;
                    dust.noGravity = true;
                }
            }
            if(projectile.ai[0] >= 640)
            {
                if(projectile.ai[1] > 0) projectile.ai[1]--;
                projectile.rotation += 0.75f;

                projectile.velocity = Vector2.Normalize(projectile.Center - Main.player[projectile.owner].Center) * -16;

                if(Vector2.Distance(Main.player[projectile.owner].Center, projectile.Center) <= 16)
                {
                    Main.player[projectile.owner].velocity += projectile.velocity / 2;
                    projectile.Kill();
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if(projectile.ai[0] >= 30)
                Main.spriteBatch.Draw(TextureCache.Extra_49, projectile.Center - Main.screenPosition, null, new Color(97, 215, 248, 0) * MathHelper.Clamp(nearbyProjectiles / 3, 1, 1.5f), 0f, new Vector2(50, 50), (projectile.ai[1]/32), SpriteEffects.None, 0f);
            return base.PreDraw(spriteBatch, lightColor);
        }
    }
}