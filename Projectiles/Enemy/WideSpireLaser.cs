using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Prim;
using EEMod.Tiles;
using EEMod.NPCs.CoralReefs;

namespace EEMod.Projectiles.Enemy
{
    public class WideSpireLaser : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aquamarine Laser");
        }

        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.timeLeft = 1200;
            projectile.ignoreWater = true;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.extraUpdates = 12;
            projectile.hide = true;
            projectile.tileCollide = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Bounce(projectile.modProjectile, oldVelocity);
            return false;
        }

        public void Bounce(ModProjectile modProj, Vector2 oldVelocity, float bouncyness = 1.5f)
        {
            Main.PlaySound(SoundID.DD2_WitherBeastDeath, projectile.Center);

            for (int i = -1; i < 2; i += 1)
            {
                Projectile projectile4 = Projectile.NewProjectileDirect(projectile.Center, (-oldVelocity).RotatedBy(i / 6f) * 2 * bouncyness, ModContent.ProjectileType<SpireLaser>(), projectile.damage / 3, 0f, default, 0, 4);
                EEMod.primitives.CreateTrail(new SpirePrimTrail(projectile4, Color.Lerp(Color.Cyan, Color.Pink, (i + 1) / 2), 30));
            }

            projectile.Kill();
        }
    }
}