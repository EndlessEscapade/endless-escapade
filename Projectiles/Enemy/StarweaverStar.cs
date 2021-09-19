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
    public class StarweaverStar : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Constellation");
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.timeLeft = 1000000;
            Projectile.ignoreWater = true;
            Projectile.hostile = true;
            // Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 12;
            Projectile.tileCollide = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Main.projectile[(int)Projectile.ai[0]] != null && Projectile.ai[0] > 0)
            {
                Vector2 vec = Main.projectile[(int)Projectile.ai[0]].Center;

                Texture2D starChain = Mod.Assets.Request<Texture2D>("Projectiles/Enemy/StarweaverStarChain").Value;

                float n = 1 / (vec - Projectile.Center).Length();

                for (float k = 0; k < 1; k += n)
                {
                    Main.spriteBatch.Draw(starChain, Projectile.Center + (vec - Projectile.Center) * k - Main.screenPosition, starChain.Frame(), Color.Gold * (0.4f + (float)(Math.Sin(Main.GameUpdateCount / 10f) / 10f)), (Main.projectile[(int)Projectile.ai[0]].Center - Projectile.Center).ToRotation(), starChain.Frame().Size() / 2f, 0.5f + (float)(Math.Sin(Main.GameUpdateCount / 10f) / 5f), SpriteEffects.None, 0f);
                }
            }

            Texture2D mask = Mod.Assets.Request<Texture2D>("Textures/SmoothFadeOut").Value;
            Helpers.DrawAdditive(mask, Projectile.Center - Main.screenPosition, Color.Yellow * 0.3f, Projectile.scale + (float)(Math.Sin(Main.GameUpdateCount / 10f) / 3f), Projectile.rotation);

            Texture2D star2 = Mod.Assets.Request<Texture2D>("Projectiles/Enemy/StarweaverStarGlow").Value;
            Main.spriteBatch.Draw(star2, Projectile.Center - Main.screenPosition, star2.Frame(), Color.Gold * 0.3f, Projectile.rotation, star2.Frame().Size() / 2f, Projectile.scale + (float)(Math.Sin(Main.GameUpdateCount / 10f) / 5f), SpriteEffects.None, 0f);

            Texture2D star = Mod.Assets.Request<Texture2D>("Projectiles/Enemy/StarweaverStar").Value;
            Main.spriteBatch.Draw(star, Projectile.Center - Main.screenPosition, star.Frame(), Color.Yellow, Projectile.rotation, star.Frame().Size() / 2f, Projectile.scale + (float)(Math.Sin(Main.GameUpdateCount / 10f) / 10f), SpriteEffects.None, 0f);
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