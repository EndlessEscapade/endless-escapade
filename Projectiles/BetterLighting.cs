using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Projectiles
{
    public class BetterLighting : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("WhiteBlock");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.alpha = 0;
            projectile.timeLeft = 60000;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale *= 1;
            projectile.light = 0;
        }
        public int yes;
        public override void AI()           //this make that the projectile will face the corect way
        {
            for (int i = EEMod.startingTermination; i <= EEMod.noOfPasses; i++)
            {
                if (Main.netMode != NetmodeID.Server && !Filters.Scene[$"EEMod:Filter{i}"].IsActive() && EEModConfigClient.Instance.BetterLighting)
                {
                    Filters.Scene.Activate($"EEMod:Filter{i}", projectile.Center).GetShader().UseIntensity(0.8f).UseOpacity(EEMod.noOfPasses);
                }
                if (EEModConfigClient.Instance.BetterLighting)
                {
                    Filters.Scene[$"EEMod:Filter{i}"].GetShader().UseIntensity(100).UseOpacity(EEMod.noOfPasses);
                }
                else
                {
                    if (SkyManager.Instance[$"EEMod:Filter{i}"].IsActive()) SkyManager.Instance.Deactivate($"EEMod:Filter{i}", new object[0]);
                }
            }
            projectile.timeLeft = 100;
            projectile.Center = Main.player[(int)projectile.ai[1]].Center;
            yes++;
            projectile.ai[0] += 0.1f;
        }
        public void drawIt()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            if (projectile.ai[0] > 1)
            {
                Main.spriteBatch.Draw(ModContent.GetTexture("EEMod/Projectiles/Nice"), Main.player[(int)projectile.ai[1]].Center - Main.screenPosition - new Vector2(-Main.screenWidth / 2, Main.screenHeight / 2), new Rectangle(0, 0, 174, 174), Color.White * .4f, projectile.rotation + (float)Math.Sin(projectile.ai[0] / 40f), new Vector2(87), 10, SpriteEffects.None, 0);
            }
            if (!EEModConfigClient.Instance.BetterLighting)
            {
                Main.spriteBatch.Draw(ModContent.GetTexture("EEMod/Projectiles/Nice"), Main.player[(int)projectile.ai[1]].Center - Main.screenPosition - new Vector2(-Main.screenWidth / 2, Main.screenHeight / 2), new Rectangle(0, 0, 174, 174), Color.White * 0, projectile.rotation + (float)Math.Sin(projectile.ai[0] / 40f), new Vector2(87), 10, SpriteEffects.None, 0);
               // Main.spriteBatch.Draw(EEPlayer.ScTex, Main.player[(int)projectile.ai[1]].Center - Main.screenPosition, new Rectangle(0, 0, 1980, 1080), Color.White * 0.5f, 0f, new Rectangle(0, 0, 1980, 1080).Size() / 2, 10, SpriteEffects.None, 0);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin();
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            // drawIt();
            return true;
        }
    }
}
