using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Enemy
{
    public class BlueRing : ModProjectile
    {
        public override string Texture => Helpers.EmptyTexture;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blue Ring");
        }

        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 8;
            projectile.alpha = 0;
            projectile.timeLeft = 1200;
            projectile.penetrate = 1;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.scale = 1f;
            projectile.aiStyle = -1;
            projectile.spriteDirection = -1;
        }

        private float radius = 0;

        public override void AI()
        {
            Vector2 origin = projectile.Center;
            projectile.ai[0]++;
            radius = 8 * (float)Math.Sin(projectile.ai[0] / 10) + 24;
            int numLocations = 40;
            for (int i = 0; i < numLocations; i++)
            {
                Vector2 position = origin + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / numLocations * i)) * radius;
                //'position' will be a point on a circle around 'origin'.  If you're using this to spawn dust, use Dust.NewDustPerfect
                Dust dust = Dust.NewDustPerfect(position, 59);
                dust.noGravity = true;
                dust.noLight = false;
                dust.fadeIn = 1f;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Venom, 300);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.Kill();
            return true;
        }
    }
}