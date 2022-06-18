using EEMod.Extensions;
using EEMod.Prim;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs.Goblins.Scrapwizard
{
    public class FlameSpiral : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shadowfire");
        }

        public override void SetDefaults()
        {
            Projectile.width = 0;
            Projectile.height = 0;

            Projectile.alpha = 0;

            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.scale = 1f;

            Projectile.aiStyle = -1;

            Projectile.tileCollide = false;

            Projectile.damage = 20;

            Projectile.timeLeft = 300;
        }


        public override void AI()
        {
            Projectile.ai[0]++;

            if (Projectile.ai[0] % 4 == 0) Projectile.ai[1]++;

            if (Projectile.ai[1] == 0) animFlameHeight = 0f;
            if (Projectile.ai[1] == 1) animFlameHeight = 0.25f;
            if (Projectile.ai[1] == 2) animFlameHeight = 0.5f;
            if (Projectile.ai[1] == 3) animFlameHeight = 0.8f;
            if (Projectile.ai[1] == 4) animFlameHeight = 1f;
            if (Projectile.ai[1] == 5) animFlameHeight = 0.5f;
        }

        public float animFlameHeight;

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/FlameSpiral").Value, Projectile.position, new Rectangle(0, (int)Projectile.ai[1] * Projectile.height, Projectile.width, Projectile.height), Color.White);

            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.spriteDirection == 1 ? Projectile.BottomLeft : Projectile.BottomRight, Vector2.Lerp(Projectile.spriteDirection == 1 ? Projectile.BottomLeft : Projectile.BottomRight, Projectile.spriteDirection == 1 ? Projectile.TopRight : Projectile.TopLeft, animFlameHeight));
        }
    }
}