using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Runes
{
    public abstract class Rune : EEProjectile
    {
        public virtual int ThisRuneID => 0;

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            // Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 10000000;
            Projectile.ignoreWater = true;
            // Projectile.tileCollide = false;
            Projectile.aiStyle = -1;
            Projectile.damage = 0;
            Projectile.alpha = 255;
            Projectile.ai[1] = 0;
        }

        public override void AI()
        {
            switch (Projectile.ai[1])
            {
                case 0:
                    if (Projectile.ai[0] == 0) Projectile.alpha = 255;
                    Projectile.ai[0]++;
                    if (Projectile.alpha > 0) Projectile.alpha -= 4;

                    Projectile.velocity = new Vector2(0, (float)Math.Sin(Projectile.ai[0] / 20));
                    break;
                case 1:
                    Projectile.alpha += 8;
                    break;
            }

            if (Projectile.alpha >= 255)
            {
                Projectile.Kill();
            }

            if (Vector2.Distance(Projectile.Center, Main.LocalPlayer.Center) <= 32 && Projectile.alpha <= 0)
            {
                Projectile.timeLeft = 32;
                Projectile.ai[1] = 1;
                Main.LocalPlayer.GetModPlayer<EEPlayer>().hasGottenRuneBefore[ThisRuneID] = 1;
            }
            CustomAI();
        }

        public override void PostDraw(Color lightColor)
        {
            CustomPostDraw();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public abstract void CustomAI();

        public abstract void CustomPostDraw();
    }
}