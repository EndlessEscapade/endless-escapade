using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace EEMod.Projectiles.OceanMap
{
    public class MessageInABottle : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Message in a Bottle");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 18;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.scale = 1f;
        }

        public bool sinking;
        public override void AI()
        {
            if (!sinking)
                projectile.velocity = new Vector2(0.5f, 0);
            else
                Sink();
        }

        private int sinkTimer = 32;
        public void Sink()
        {
            projectile.velocity.X = 0;
            projectile.velocity.Y = 0.5f;
            projectile.alpha += 8;
            sinkTimer--;
            if (sinkTimer <= 0)
            {
                projectile.Kill();
            }
        }
    }
}
