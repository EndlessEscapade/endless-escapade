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
    public class SpireLaserAlt : EEProjectile
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
            return true;
        }
    }
}