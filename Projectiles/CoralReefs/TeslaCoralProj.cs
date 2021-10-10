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

        public override void AI()
        {
            if (Projectile.ai[0] < iterations)
            {
                Vector2 dir = target - Projectile.Center;

                distance = dir.Length() / 16f;

                Vector2 desiredPoint = dir * (Projectile.ai[0] / iterations);

                Vector2 desiredVector = desiredPoint + (Vector2.Normalize(dir - Projectile.Center).RotatedBy(Main.rand.NextFloat(-1.5f, 1.5f)) * distance);

                Projectile.Center += desiredVector;

                Projectile.ai[0]++;

                /*if(Main.rand.NextBool(3))
                {
                    int lightningproj = Projectile.NewProjectile(new ProjectileSource_ProjectileParent(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<TeslaCoralProj>(), 20, 2.5f);

                    if (Main.netMode != NetmodeID.Server)
                    {
                        PrimSystem.primitives.CreateTrail(new AxeLightningPrimTrail(Main.projectile[lightningproj], 2f));
                    }

                    TeslaCoralProj zappy = Main.projectile[lightningproj].ModProjectile as TeslaCoralProj;

                    int val = Main.rand.Next(1, 3);

                    Main.NewText(val);

                    zappy.iterations = val;

                    zappy.target = ((target - Projectile.Center) / (iterations - Projectile.ai[0])).RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f)) * val;
                }*/
            }
            else
            {
                Projectile.Center = target;
            }
        }
    }
}