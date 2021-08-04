using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.NPCs.Bosses.Kraken    //We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class KramkenRocc : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ink Blob");
        }

        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 30;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.friendly = false;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.damage = 50;
            Projectile.light = .5f;
            Projectile.scale = Main.rand.NextFloat(0.5f, 1.5f);
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.05f;
            Projectile.rotation = Projectile.velocity.Y / 8f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return true;
        }

        public override void Kill(int timeLeft)
        {
        }
    }
}