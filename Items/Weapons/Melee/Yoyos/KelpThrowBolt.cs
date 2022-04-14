using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Melee.Yoyos
{
    public class KelpThrowBolt : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kelp Throw");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            // Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 200;
            Projectile.friendly = true;
            // Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.damage = 10;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void AI()
        {
            Projectile.velocity = Projectile.velocity.RotatedBy(Math.PI / 180) * 0.99f;
            Projectile.ai[0]++;
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D boltTex = ModContent.Request<Texture2D>("EEMod/Particles/MediumCircle").Value;

            Helpers.DrawAdditive(boltTex, Projectile.Center - Main.screenPosition, Color.Goldenrod, 0.2f);
            
            Helpers.DrawAdditive(ModContent.Request<Texture2D>("EEMod/Textures/RadialGradient").Value, Projectile.Center - Main.screenPosition, Color.Gold, 0.1f);

            return false;
        }
    }
}