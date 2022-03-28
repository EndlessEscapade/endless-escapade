using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Runes
{
    public class BubblingWatersRune : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubbling Waters Rune");
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 100000;
            // Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.aiStyle = -1;
            Projectile.damage = 0;
        }

        public override void AI()
        {
            if (Projectile.ai[1] == 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_Parent(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<BubblingWatersRuneBubble>(), 0, 0, Projectile.whoAmI);
                }
            }

            Projectile.ai[1]++;
            if (Projectile.ai[0] > 0)
                Projectile.alpha += 4;

            Projectile.Center += new Vector2(0, (float)Math.Sin(flash * 3) / 20);
        }

        private float flash = 0;

        public override bool PreDraw(ref Color lightColor)
        {
            flash += 0.01f;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            if (Projectile.ai[1] > 120)
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/Projectiles/Nice").Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 174, 174), lightColor * Math.Abs((float)Math.Sin(flash)) * 0.5f * ((255 - Projectile.alpha) / 255), Projectile.rotation + flash, new Vector2(174, 174) / 2, 1, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            return true;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Projectile.ai[1] > 120)
            {
                Projectile.timeLeft = 64;
                Projectile.ai[0]++;
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