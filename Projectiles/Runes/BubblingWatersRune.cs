using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Runes
{
    public class BubblingWatersRune : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubbling Waters Rune");
            Main.projFrames[projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 52;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.hostile = true;
            projectile.ranged = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 100000;
            projectile.ignoreWater = false;
            projectile.tileCollide = true;
            projectile.extraUpdates = 1;
            projectile.aiStyle = -1;
            projectile.arrow = true;
            projectile.damage = 0;
        }

        public int rippleCount = 3;
        public int rippleSize = 500;
        public int rippleSpeed = 200;
        public float distortStrength = 200;
        public int yes;

        public override void AI()           //this make that the projectile will face the corect way
        {
            yes++;
            if (yes > 120)
            {
                if (projectile.ai[0] > 1)
                {
                    projectile.ai[0] += 0.1f;
                }
                projectile.damage = 1;
                projectile.velocity = Vector2.Zero;
                projectile.ai[1] += 0.5f;
                //projectile.velocity.Y = (float)Math.Sin(projectile.ai[1]/16) / 4;
                if (projectile.ai[0] == 0)
                {
                    if (Main.netMode != NetmodeID.Server && !Filters.Scene["EEMod:Shockwave"].IsActive())
                    {
                        Filters.Scene.Activate("EEMod:Shockwave", projectile.Center).GetShader().UseColor(rippleCount, rippleSize, rippleSpeed).UseTargetPosition(projectile.Center);
                        projectile.ai[0] = 1;
                    }
                }
                float progress = (180 - projectile.ai[1]) / 720f;
                progress *= .3f;
                distortStrength = (projectile.ai[1] * 2);
                //  Filters.Scene["EEMod:Shockwave"].GetShader().UseProgress(progress).UseOpacity(distortStrength * (1 - progress / 3f));
                //  Filters.Scene["EEMod:WhiteFlash"].GetShader().UseOpacity(projectile.ai[0]);

                if (projectile.ai[1] == 160)
                {
                    projectile.ai[1] = 0;
                }
            }
        }

        private float flash = 0;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            flash += 0.01f;
            if (flash == 2)
            {
                flash = 10;
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            if (projectile.ai[0] > 1)
            {
                spriteBatch.Draw(ModContent.GetTexture("EEMod/Projectiles/Nice"), projectile.Center - Main.screenPosition, new Rectangle(0, 0, 174, 174), lightColor * flash * 0.5f, projectile.rotation + flash, new Vector2(174, 174) / 2, projectile.ai[0], SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.Draw(ModContent.GetTexture("EEMod/Projectiles/Nice"), projectile.Center - Main.screenPosition, new Rectangle(0, 0, 174, 174), lightColor * Math.Abs((float)Math.Sin(flash)) * 0.5f, projectile.rotation + flash, new Vector2(174, 174) / 2, projectile.ai[0], SpriteEffects.None, 0);
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            return true;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (yes > 120)
            {
                projectile.timeLeft = 3600;
                projectile.ai[0]++;
                flash = 0;
                target.GetModPlayer<EEPlayer>().hasGottenRuneBefore[2] = 1;
                if (Main.netMode != NetmodeID.Server && !Filters.Scene["EEMod:WhiteFlash"].IsActive())
                {
                    EEMod.isAscending = true;
                    Filters.Scene.Activate("EEMod:WhiteFlash", projectile.Center).GetShader().UseOpacity(projectile.ai[0]);
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //target.AddBuff(BuffID.Chilled, 100);
        }

        public override void Kill(int timeleft)
        {
            EEMod.isAscending = false;
            EEMod.AscentionHandler = 0;
            if (Main.netMode != NetmodeID.Server && Filters.Scene["EEMod:Shockwave"].IsActive())
            {
                Filters.Scene["EEMod:Shockwave"].Deactivate();
            }
            if (Main.netMode != NetmodeID.Server && Filters.Scene["EEMod:WhiteFlash"].IsActive())
            {
                Filters.Scene["EEMod:WhiteFlash"].Deactivate();
            }
            //Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 50);
            Main.PlaySound(SoundID.Item27, projectile.position);
            for (var i = 0; i < 20; i++)
            {
                int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 123, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-1f, 1f), 6, new Color(255, 217, 184, 255), projectile.scale * 0.5f);
                Main.dust[num].noGravity = true;
                Main.dust[num].velocity *= 2.5f;
                Main.dust[num].noLight = false;
            }
        }
    }
}