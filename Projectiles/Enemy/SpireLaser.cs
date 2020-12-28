using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Prim;
using EEMod.Tiles;

namespace EEMod.Projectiles.Enemy
{
    public class SpireLaser : ModProjectile
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
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.extraUpdates = 12;
            projectile.hide = true;
            projectile.tileCollide = true;
        }

        public override void AI()
        {
            Tile currentTile = Main.tile[(int)(projectile.Center.X / 16), (int)(projectile.Center.Y / 16)];
            if (currentTile != null && currentTile.active() && currentTile.type == ModContent.TileType<EmptyTile>() && projectile.ai[0] == 0)
            {
                projectile.velocity *= -2;
                projectile.timeLeft = 250;
                projectile.ai[0] = 1;
                Main.NewText("CRUMPETS!");
            }
        }
    }
}