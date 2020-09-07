using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.NPCs.Bosses.Kraken    //We need this to basically indicate the folder where it is to be read from, so you the texture will load correctly
{
    public class KramkenRocc : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ink Blob");
        }

        public override void SetDefaults()
        {
            projectile.width = 36;
            projectile.height = 30;
            projectile.hostile = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 600;
            projectile.friendly = false;
            projectile.tileCollide = true;
            projectile.extraUpdates = 1;
            projectile.damage = 50;
            projectile.light = .5f;
            projectile.scale = Main.rand.NextFloat(0.5f, 1.5f);
        }

        private Vector2 start;
        private KrakenHead krakenHead => Main.npc[(int)projectile.ai[1]].modNPC as KrakenHead;

        public override void AI()
        {
            projectile.velocity.Y += 0.05f;
            projectile.rotation = projectile.velocity.Y / 8f;
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