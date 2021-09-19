using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Prim;
using EEMod.Tiles;
using EEMod.NPCs.CoralReefs;
using Terraria.Audio;

namespace EEMod.Projectiles.Enemy
{
    public class WideSpireLaser : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aquamarine Laser");
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.timeLeft = 1200;
            Projectile.ignoreWater = true;
            Projectile.hostile = true;
            // Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 12;
            Projectile.hide = true;
            Projectile.tileCollide = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Bounce(Projectile.ModProjectile, oldVelocity);
            return false;
        }

        public void Bounce(ModProjectile modProj, Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.DD2_WitherBeastDeath, Projectile.Center);

            for (int i = -1; i < 2; i++)
            {
                Projectile projectile4 = Projectile.NewProjectileDirect(new Terraria.DataStructures.ProjectileSource_ProjectileParent(Projectile), Projectile.Center, Vector2.Normalize(Projectile.Center - Main.LocalPlayer.Center).RotatedBy(i / 6f) * 2, ModContent.ProjectileType<SpireLaser>(), Projectile.damage / 3, 0f, default, 1, 4);
                PrimSystem.primitives.CreateTrail(new SpirePrimTrail(projectile4, Color.Lerp(Color.Cyan, Color.Magenta, (i + 1) / 2f), 30));
            }

            Projectile.Kill();
        }
    }
}