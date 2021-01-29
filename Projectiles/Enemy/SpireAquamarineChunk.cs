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
    public class SpireAquamarineChunk : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aquamarine Chunk");
        }

        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 40;
            projectile.timeLeft = 30000;
            projectile.ignoreWater = true;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.extraUpdates = 12;
            projectile.tileCollide = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.Kill();
            return true;
        }

        public override void AI()
        {
            
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Main.spriteBatch.Draw(mod.GetTexture("Projectiles/Enemy/SpireAquamarineChunkGlow"), projectile.Center.ForDraw(), Color.White * projectile.ai[0]);
        }
    }
}