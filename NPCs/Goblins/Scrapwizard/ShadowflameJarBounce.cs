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
    public class ShadowflameJarBounce : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shadowfire Concoction");
        }

        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;

            Projectile.alpha = 0;

            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.scale = 1f;

            Projectile.aiStyle = -1;

            Projectile.tileCollide = true;

            Projectile.damage = 20;

            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
            Projectile.velocity.X = MathHelper.Clamp(Projectile.velocity.X, -12f, 12f);

            Projectile.rotation += Projectile.velocity.X * 0.05f;

            Projectile.velocity.Y += 0.4f;
        }

        public int bounces = 2;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            //explode into flames or whatever

            if (bounces > 0)
            {
                bounces--;

                Projectile.velocity.Y = -Projectile.oldVelocity.Y * 0.8f;

                return false;
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.UnitY.RotatedBy(Main.rand.NextFloat(-3.14f, 3.14f)) * 4f, ModContent.ProjectileType<ShadowEmberTemp>(), 0, 0);
                }

                Projectile.Kill();

                return true;
            }
        }
    }
}