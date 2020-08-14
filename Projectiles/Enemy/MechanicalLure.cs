using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace EEMod.Projectiles.Enemy
{
    public class MechanicalLure : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mechanical Lure");
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

        public override void AI()
        {
            projectile.velocity = new Vector2(0, 1);
        }

        public override void Kill(int timeLeft)
        {

        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {

        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.Kill();
            return true;
        }
    }
}
