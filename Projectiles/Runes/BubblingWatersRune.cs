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
            projectile.width = 64;
            projectile.height = 64;
            projectile.friendly = true;
            projectile.hostile = true;
            projectile.ranged = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 100000;
            projectile.ignoreWater = false;
            projectile.tileCollide = true;
            projectile.extraUpdates = 1;
            projectile.aiStyle = -1;
            projectile.damage = 0;
        }

        public override void AI()
        {
            if (projectile.ai[1] == 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<BubblingWatersRuneBubble>(), 0, 0, projectile.whoAmI);
                }
            }

            projectile.ai[1]++;
            if (projectile.ai[0] > 0)
                projectile.alpha += 4;

            projectile.Center += new Vector2(0, (float)Math.Sin(flash * 3) / 20);
        }

        private float flash = 0;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            flash += 0.01f;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            Main.NewText((255 - projectile.alpha) / 255);
            if (projectile.ai[1] > 120)
                spriteBatch.Draw(ModContent.GetTexture("EEMod/Projectiles/Nice"), projectile.Center - Main.screenPosition, new Rectangle(0, 0, 174, 174), lightColor * Math.Abs((float)Math.Sin(flash)) * 0.5f * ((255 - projectile.alpha) / 255), projectile.rotation + flash, new Vector2(174, 174) / 2, 1, SpriteEffects.None, 0);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            return true;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (projectile.ai[1] > 120)
            {
                projectile.timeLeft = 64;
                projectile.ai[0]++;
                flash = 0;
                target.GetModPlayer<EEPlayer>().hasGottenRuneBefore[2] = 1;
            }
        }

        /*public override void Kill(int timeleft)
        {
            Main.PlaySound(SoundID.Item27, projectile.position);
            for (var i = 0; i < 20; i++)
            {
                int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 123, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-1f, 1f), 6, new Color(255, 217, 184, 255), projectile.scale * 0.5f);
                Main.dust[num].noGravity = true;
                Main.dust[num].velocity *= 2.5f;
                Main.dust[num].noLight = false;
            }
        }*/
    }
}