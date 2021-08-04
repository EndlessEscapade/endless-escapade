using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Prim;
using EEMod.Tiles;
using EEMod.NPCs.CoralReefs;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Extensions;

namespace EEMod.Projectiles.Enemy
{
    public class SpireAquamarineChunk : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aquamarine Chunk");
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 36;
            Projectile.timeLeft = 30000;
            Projectile.ignoreWater = true;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 12;
            Projectile.tileCollide = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return true;
        }

        public override void AI()
        {
            Projectile.velocity.Y *= 1.003f;
            Projectile.rotation += Projectile.velocity.Y / 128f;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {

        }
    }
}
