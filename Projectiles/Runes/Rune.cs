using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Runes
{
    public abstract class Rune : ModProjectile
    {
        public virtual int ThisRuneID => 0;

        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 10000000;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.aiStyle = -1;
            projectile.damage = 0;
            projectile.alpha = 255;
            projectile.ai[1] = 0;
        }

        public override void AI()
        {
            switch (projectile.ai[1])
            {
                case 0:
                    if (projectile.ai[0] == 0) projectile.alpha = 255;
                    projectile.ai[0]++;
                    if (projectile.alpha > 0) projectile.alpha -= 4;

                    projectile.velocity = new Vector2(0, (float)Math.Sin(projectile.ai[0] / 20));
                    break;
                case 1:
                    projectile.alpha += 8;
                    break;
            }

            if (projectile.alpha >= 255)
            {
                projectile.Kill();
            }

            if (Vector2.Distance(projectile.Center, Main.LocalPlayer.Center) <= 32 && projectile.alpha <= 0)
            {
                projectile.timeLeft = 32;
                projectile.ai[1] = 1;
                Main.LocalPlayer.GetModPlayer<EEPlayer>().hasGottenRuneBefore[ThisRuneID] = 1;
            }
            Main.NewText(projectile.ai[1]);

            CustomAI();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public abstract void CustomAI();
    }
}