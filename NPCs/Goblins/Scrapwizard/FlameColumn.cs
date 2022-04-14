using EEMod.Extensions;
using EEMod.Prim;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.Goblins.Scrapwizard
{
    public class FlameColumn : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shadowfire");
        }

        public override void SetDefaults()
        {
            Projectile.width = 0;
            Projectile.height = 0;

            Projectile.alpha = 0;

            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.scale = 1f;

            Projectile.aiStyle = -1;

            Projectile.tileCollide = false;

            Projectile.damage = 20;

            Projectile.timeLeft = 300;
        }

        ShadowflameCampfirePrims trail1;
        ShadowflameCampfirePrims trail2;
        ShadowflameCampfirePrims trail3;

        public float heightFloat;

        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;

            if(Projectile.ai[0] == 0)
            {
                heightFloat = 0f;

                PrimitiveSystem.primitives.CreateTrail(trail1 = new ShadowflameCampfirePrims(Color.Violet * 0.2f, Projectile.position, Projectile.position + new Vector2(0, -(heightFloat / 2)), Projectile.position + new Vector2(0, -heightFloat), 34, 20, true, 1));
                PrimitiveSystem.primitives.CreateTrail(trail2 = new ShadowflameCampfirePrims(Color.Violet, Projectile.position, Projectile.position + new Vector2(0, -(heightFloat / 2) + 4), Projectile.position + new Vector2(0, -heightFloat + 4), 30, 20, true, 1));
                PrimitiveSystem.primitives.CreateTrail(trail3 = new ShadowflameCampfirePrims(Color.Violet, Projectile.position, Projectile.position + new Vector2(0, -(heightFloat / 2)), Projectile.position + new Vector2(0, -heightFloat), 32, 20, false, 1));
            }
            else if(Projectile.ai[0] < 40)
            {
                heightFloat = 0f;
            }
            else if (Projectile.ai[0] < 60)
            {
                heightFloat += 15f;
            }
            else if (Projectile.ai[0] < 90)
            {
                heightFloat -= 12f;
            }
            else if (Projectile.ai[0] == 120)
            {
                Projectile.Kill();
            }

            Projectile.ai[0]++;

            trail1.startPoint = Projectile.position;
            trail1.controlPoint = Projectile.position + new Vector2(0, -(heightFloat / 2));
            trail1.endPoint = Projectile.position + new Vector2(0, -heightFloat);

            trail2.startPoint = Projectile.position;
            trail2.controlPoint = Projectile.position + new Vector2(0, -(heightFloat / 2) + 4);
            trail2.endPoint = Projectile.position + new Vector2(0, -heightFloat + 4);

            trail3.startPoint = Projectile.position;
            trail3.controlPoint = Projectile.position + new Vector2(0, -(heightFloat / 2));
            trail3.endPoint = Projectile.position + new Vector2(0, -heightFloat);
        }

        public override void Kill(int timeLeft)
        {
            trail1.Dispose();
            trail2.Dispose();
            trail3.Dispose();

            base.Kill(timeLeft);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return base.Colliding(new Rectangle((int)Projectile.position.X - (10 / 2), (int)Projectile.position.Y - (int)heightFloat, 10, (int)heightFloat), targetHitbox);
        }
    }
}