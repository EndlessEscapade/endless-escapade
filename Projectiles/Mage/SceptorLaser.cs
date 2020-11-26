using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Prim;

namespace EEMod.Projectiles.Mage
{
    public class SceptorLaser : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aquamarine Laser");
        }

        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.timeLeft = 250;
            projectile.ignoreWater = true;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.extraUpdates = 12;
            projectile.hide = true;
        }

        public override void AI()
        {
            if (projectile.timeLeft > 6)
            {
                var list = Main.projectile.Where(x => x.Hitbox.Intersects(projectile.Hitbox));
                foreach (var proj in list)
                {
                    if (proj.type == ModContent.ProjectileType<SceptorPrism>() && proj.active)
                    {
                        for (float i = -0.6f; i <= 0.6f; i += 0.4f)
                        {
                            int proj2 = Projectile.NewProjectile(proj.Center - (Vector2.UnitY.RotatedBy((double)i) * 60), 5 * Vector2.UnitY.RotatedBy((double)i), ModContent.ProjectileType<SceptorLaserTwo>(), projectile.damage, projectile.knockBack, projectile.owner);
                            EEMod.primitives.CreateTrail(new SceptorPrimTrailTwo(Main.projectile[proj2]));
                        }
                        projectile.timeLeft = 6;
                    }
                }
            }
        }

    }
    public class SceptorLaserTwo : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aquamarine Laser");
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.timeLeft = 120;
            projectile.ignoreWater = true;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.extraUpdates = 12;
            projectile.tileCollide = false;
            projectile.hide = true;
        }
    }
}