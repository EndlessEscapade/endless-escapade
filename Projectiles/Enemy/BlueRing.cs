using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Enemy
{
    public class BlueRing : EEProjectile
    {
        public override string Texture => Helpers.EmptyTexture;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blue Ring");
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 8;
            Projectile.alpha = 0;
            Projectile.timeLeft = 1200;
            Projectile.penetrate = 1;
            Projectile.hostile = true;
            // Projectile.friendly = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.spriteDirection = -1;
        }

        private float radius = 0;

        public override void AI()
        {
            Vector2 origin = Projectile.Center;
            Projectile.ai[0]++;
            radius = 8 * (float)Math.Sin(Projectile.ai[0] / 10) + 24;
            int numLocations = 40;
            for (int i = 0; i < numLocations; i++)
            {
                Vector2 position = origin + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / numLocations * i)) * radius;
                //'position' will be a point on a circle around 'origin'.  If you're using this to spawn dust, use Dust.NewDustPerfect
                Dust dust = Dust.NewDustPerfect(position, 59);
                dust.noGravity = true;
                // dust.noLight = false;
                dust.fadeIn = 1f;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Venom, 300);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return true;
        }
    }
}