using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Melee.Yoyos
{
    public class KelpThrowBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kelp Throw");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 200;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.extraUpdates = 1;
            projectile.damage = 10;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void AI()
        {
            projectile.velocity = projectile.velocity.RotatedBy(Math.PI / 180) * 0.99f;
            projectile.ai[0]++;
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D boltTex = ModContent.GetTexture("EEMod/Particles/MediumCircle");

            Helpers.DrawAdditive(boltTex, projectile.Center - Main.screenPosition, Color.Goldenrod, 0.2f);
            
            Helpers.DrawAdditive(ModContent.GetTexture("EEMod/Textures/RadialGradient"), projectile.Center - Main.screenPosition, Color.Gold, 0.1f);

            return false;
        }

        public override void Kill(int timeLeft)
        {
            for (var a = 0; a < 2; a++)
            {
                int index = Dust.NewDust(projectile.Center, 22, 22, DustID.Gold, 0, 0);
                Main.dust[index].noGravity = true;
            }
        }
    }
}