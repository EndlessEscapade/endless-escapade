using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Prim;
using EEMod.Tiles;
using EEMod.NPCs.CoralReefs;

namespace EEMod.Items.Weapons.Ranger.Longbows
{
    public class ShimmerShotProj1 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aquamarine Laser");
        }

        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.timeLeft = 30000;
            projectile.ignoreWater = true;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.extraUpdates = 12;
            projectile.hide = true;
            projectile.tileCollide = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Bounce(projectile.modProjectile, oldVelocity);
            projectile.ai[0]++;
            return false;
        }

        public void Bounce(ModProjectile modProj, Vector2 oldVelocity, float bouncyness = 1.5f)
        {
            Projectile projectile = modProj.projectile;
            if (projectile.velocity.X != oldVelocity.X)
            {
                projectile.velocity.X = -oldVelocity.X * bouncyness;
            }

            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.velocity.Y = -oldVelocity.Y * bouncyness;
            }
            Main.PlaySound(SoundID.DD2_WitherBeastDeath, projectile.Center);
        }

        public override void AI()
        {
            if (projectile.ai[0] >= 3)
            {
                projectile.Kill();
            }
        }
    }
}