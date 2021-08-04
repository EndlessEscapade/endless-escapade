using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Mage
{
    public class DalantiniumFanAlt : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dalantinium Fan");
        }

        public override void SetDefaults()
        {
            Projectile.hostile = false;
            Projectile.magic = true;
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 1000;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[Projectile.type].Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k].ForDraw() + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color2 = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length / 2);
                spriteBatch.Draw(Main.projectileTexture[Projectile.type], drawPos, new Rectangle(0, 0, Projectile.width, Projectile.height), color2 * 0.5f, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
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
                    Vector2 offset = new Vector2(xdist, ydist).RotatedBy(Projectile.rotation);
                    Dust dust = Dust.NewDustPerfect(Main.player[Projectile.owner].Center + Dist.RotatedBy(lerp) * 1.2f, 219, offset * 0.5f, 0, Color.Red);
                    dust.noGravity = true;
                    dust.velocity *= 0.94f;
                    dust.noLight = false;
                    dust.fadeIn = 1f;
                }
            }
            Projectile.ai[0]++;
            if (Projectile.ai[0] == 1)
            {
                Dist = Vector2.Normalize(Main.MouseWorld - Main.player[Projectile.owner].Center) * 100;
            }
            Vector2 dist = Projectile.Center - Main.player[Projectile.owner].Center;
            if (Projectile.ai[0] < firstPhase)
            {
                Projectile.velocity += (Main.player[Projectile.owner].Center + Dist - Projectile.Center) / 202f - Projectile.velocity * 0.02f;
                Projectile.rotation = dist.ToRotation() + (float)Math.PI / 4f;

            }
            else if (Projectile.ai[0] < secondPhase)
            {
                Dust.NewDust(Projectile.position, 30, 30, DustID.Fireworks);
                if (Projectile.ai[0] == firstPhase)
                {
                    Dist = dist;
                }
                Projectile.rotation = dist.ToRotation() + (float)Math.PI / 4f + (lerp - 1.57f) / 2f;
                Projectile.Center = Main.player[Projectile.owner].Center + Dist.RotatedBy(lerp);
                float traverseFunction = (float)Math.Sin((Projectile.ai[0] - firstPhase) / ((secondPhase - firstPhase) / 3.14f));
                lerp += (0.18f * traverseFunction * traverseFunction) - 0.02f;
                if (Projectile.ai[0] % 2 == 1 && Projectile.ai[0] > firstPhase + 1)
                {
                    Projectile.NewProjectile(Projectile.Center, Vector2.Normalize(Dist.RotatedBy(lerp)) * 10, ModContent.ProjectileType<DalantiniumFang>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    circleDustPattern();
                }
            }
            else if (Projectile.ai[0] < thirdPhase)
            {
                Dust.NewDust(Projectile.position, 30, 30, DustID.Water_Crimson);
                Projectile.rotation = dist.ToRotation() + (float)Math.PI / 4f + (lerp - 1.57f) / 2f;
                Projectile.Center = Main.player[Projectile.owner].Center + Dist.RotatedBy(lerp);
                float traverseFunction = (float)Math.Sin((Projectile.ai[0] - secondPhase) / ((thirdPhase - secondPhase) / 3.14f));
                lerp -= (0.18f * traverseFunction * traverseFunction);
                if (Projectile.ai[0] % 2 == 1 && Projectile.ai[0] > secondPhase + 1)
                {
                    Projectile.NewProjectile(Projectile.Center, Vector2.Normalize(Dist.RotatedBy(lerp)) * 10, ModContent.ProjectileType<DalantiniumFang>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    circleDustPattern();
                }
                if (Projectile.ai[0] == thirdPhase - 5)
                {
                    Projectile.Kill();
                    for (var i = 0; i < 40; i++)
                    {
                        int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Fireworks, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-2f, -1f), 6, default, Projectile.scale);
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
