using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles.CoralReefs
{
    public class KelpFlowerOrb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("KelpFlowerOrb");
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.alpha = 0;
            projectile.scale = 1f;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 360;
        }

        private Player player;
        private float initialRot;

        public override void AI()
        {
            if (projectile.ai[0] == 0)
            {
                player = Main.LocalPlayer;

                projectile.rotation = projectile.velocity.ToRotation();
                initialRot = projectile.rotation;
            }

            projectile.ai[0]++;

            projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(player.Center) * 16f, (projectile.ai[0] / 300f));

            projectile.rotation = projectile.velocity.ToRotation();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Helpers.DrawAdditive(ModContent.GetTexture("EEMod/Textures/SmoothFadeOut"), projectile.Center, Color.LightYellow, 0.2f);

            Texture2D tex = ModContent.GetTexture("EEMod/Projectiles/CoralReefs/KelpFlowerOrb");
            Main.spriteBatch.Draw(tex, projectile.Center, tex.Bounds, Color.White, projectile.rotation, new Vector2(2, 2), 1f, SpriteEffects.None, 0f);

            return false;
        }
    }
}