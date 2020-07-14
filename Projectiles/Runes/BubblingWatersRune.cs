using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Effects;

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
        public override void AI()           //this make that the projectile will face the corect way
        {
            projectile.damage = 0;
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
            Filters.Scene["EEMod:Shockwave"].GetShader().UseProgress(progress).UseOpacity(distortStrength * (1 - progress / 3f));

            if (projectile.ai[1] == 160)
            {
                projectile.ai[1] = 0;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            damage = 0;
            target.GetModPlayer<EEPlayer>().hasGottenRuneBefore[2] = 1;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //target.AddBuff(BuffID.Chilled, 100);
        }

        public override void Kill(int timeleft)
        {
            if (Main.netMode != NetmodeID.Server && Filters.Scene["EEMod:Shockwave"].IsActive())
            {
                Filters.Scene["EEMod:Shockwave"].Deactivate();
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
