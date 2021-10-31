using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.DataStructures;
using EEMod.Prim;

namespace EEMod.Projectiles.CoralReefs
{
    public class TeslaCoralProj : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightning Bolt");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            // Projectile.hostile = false;
            Projectile.friendly = true;
            // Projectile.tileCollide = false;
            Projectile.damage = 0;
            Projectile.timeLeft = iterations;
            Projectile.alpha = 0;
            Projectile.hide = true;
        }

        public Vector2 target = Vector2.Zero;
        public int iterations = 8;
        public float distance = 24;

        public Projectile projTarget;

        public override void AI()
        {
            if (projTarget != null) target = projTarget.Center;

            if (Projectile.ai[0] < iterations)
            {

                Vector2 dir = target - Projectile.Center;

                distance = dir.Length() / 16f;

                Vector2 desiredPoint = dir * (Projectile.ai[0] / iterations);

                Vector2 desiredVector = desiredPoint + (Vector2.Normalize(dir - Projectile.Center).RotatedBy(Main.rand.NextFloat(-1.5f, 1.5f)) * distance);

                Projectile.Center += desiredVector;

                Projectile.ai[0]++;

                if (Projectile.ai[0] == iterations && projTarget != null) Projectile.Center = projTarget.Center;

                /*if (Main.rand.NextBool(2) && Projectile.ai[1] == 0)
                {
                    int lightningproj = Projectile.NewProjectile(new ProjectileSource_ProjectileParent(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<TeslaCoralProj>(), Projectile.damage / 2, Projectile.knockBack / 2f, default);

                    Main.projectile[lightningproj].ai[1] = 1;

                    if (Main.netMode != NetmodeID.Server)
                    {
                        PrimitiveSystem.primitives.CreateTrail(new AxeLightningPrimTrail(Main.projectile[lightningproj], 2f));
                    }

                    TeslaCoralProj zappy = Main.projectile[lightningproj].ModProjectile as TeslaCoralProj;

                    zappy.target = Projectile.Center + desiredVector;
                    zappy.iterations = 2;
                }*/
            }
            else
            {
                Projectile.Center = target;
            }
        }
    }
}