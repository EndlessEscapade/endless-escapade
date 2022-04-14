using EEMod.Prim;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace EEMod.Projectiles
{
    public class TornSails : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Torn Sails");
        }

        public override void SetDefaults()
        {
            Projectile.width = 36;       //projectile width
            Projectile.height = 46;  //projectile height
            Projectile.friendly = true;      //make that the projectile will not damage you
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 1000000000;
        }

        public override void AI()
        {
            if (Projectile.ai[1] == 0)
            {
                PrimitiveSystem.primitives.CreateTrail(new TornSailPrims(Projectile, Projectile.Center + new Vector2(0, 2), Projectile.Center + new Vector2(-160, 40 + 2), 1, 1f, true, 24f, 1f));
                PrimitiveSystem.primitives.CreateTrail(new TornSailPrims(Projectile, Projectile.Center, Projectile.Center + new Vector2(-160, 40), 1, 1f, false, 24f, 1f));

                PrimitiveSystem.primitives.CreateTrail(new TornSailPrims(Projectile, Projectile.Center + new Vector2(0, 32 + 2), Projectile.Center + new Vector2(-100, 60 + 2), 0, 1.5f, true, 12f, 1f));
                PrimitiveSystem.primitives.CreateTrail(new TornSailPrims(Projectile, Projectile.Center + new Vector2(0, 32), Projectile.Center + new Vector2(-100, 60), 0, 1.5f, false, 12f, 1f));

                PrimitiveSystem.primitives.CreateTrail(new TornSailPrims(Projectile, Projectile.Center + new Vector2(0, 52 + 2), Projectile.Center + new Vector2(-120, 86 + 2), 3, 1.2f, true, 18f, 1f));
                PrimitiveSystem.primitives.CreateTrail(new TornSailPrims(Projectile, Projectile.Center + new Vector2(0, 52), Projectile.Center + new Vector2(-120, 86), 3, 1.2f, false, 18f, 1f));

                Projectile.ai[1] = 1;
            }
        }

        public override bool PreDraw(ref Color lightColor)  //this make the projectile sprite rotate perfectaly around the player
        {
            return false;
        }
    }
}