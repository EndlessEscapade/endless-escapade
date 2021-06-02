using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Prim;
using EEMod.Tiles;
using EEMod.NPCs.CoralReefs;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Extensions;

namespace EEMod.Projectiles.Enemy
{
    public class StarweaverStar : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Constellation");
        }

        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 22;
            projectile.timeLeft = 1000000;
            projectile.ignoreWater = true;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.extraUpdates = 12;
            projectile.tileCollide = true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (Main.projectile[(int)projectile.ai[0]] != null && projectile.ai[0] > 0)
            {
                Vector2 vec = Main.projectile[(int)projectile.ai[0]].Center;

                Texture2D starChain = mod.GetTexture("Projectiles/Enemy/StarweaverStarChain");

                float n = 1 / (vec - projectile.Center).Length();

                for (float k = 0; k < 1; k += n)
                {
                    spriteBatch.Draw(starChain, projectile.Center + (vec - projectile.Center) * k - Main.screenPosition, starChain.Frame(), Color.Gold * (0.4f + (float)(Math.Sin(Main.GameUpdateCount / 10f) / 10f)), (Main.projectile[(int)projectile.ai[0]].Center - projectile.Center).ToRotation(), starChain.Frame().Size() / 2f, 0.5f + (float)(Math.Sin(Main.GameUpdateCount / 10f) / 5f), SpriteEffects.None, 0f);
                }
            }

            Texture2D mask = mod.GetTexture("Textures/SmoothFadeOut");
            Helpers.DrawAdditive(mask, projectile.Center - Main.screenPosition, Color.Yellow * 0.3f, projectile.scale + (float)(Math.Sin(Main.GameUpdateCount / 10f) / 3f), projectile.rotation);

            Texture2D star2 = mod.GetTexture("Projectiles/Enemy/StarweaverStarGlow");
            spriteBatch.Draw(star2, projectile.Center - Main.screenPosition, star2.Frame(), Color.Gold * 0.3f, projectile.rotation, star2.Frame().Size() / 2f, projectile.scale + (float)(Math.Sin(Main.GameUpdateCount / 10f) / 5f), SpriteEffects.None, 0f);

            Texture2D star = mod.GetTexture("Projectiles/Enemy/StarweaverStar");
            spriteBatch.Draw(star, projectile.Center - Main.screenPosition, star.Frame(), Color.Yellow, projectile.rotation, star.Frame().Size() / 2f, projectile.scale + (float)(Math.Sin(Main.GameUpdateCount / 10f) / 10f), SpriteEffects.None, 0f);
            return false;
        }

        /*public override void AI()
        {
            projectile.ai[1]++;

            if(projectile.ai[1] == 120)
            {
                projectile.velocity = Vector2.Normalize();
            }
        }*/
    }
}