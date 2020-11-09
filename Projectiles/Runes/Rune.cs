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
        public virtual int RuneID => 0;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 10000000;
            projectile.ignoreWater = false;
            projectile.tileCollide = false;
            projectile.aiStyle = -1;
            projectile.damage = 0;
            projectile.alpha = 255;
        }

        public override void AI()
        {
            switch (projectile.ai[1])
            {
                case 0:
                    projectile.ai[0]++;
                    projectile.alpha -= 4;
                    Helpers.Clamp(projectile.alpha, 0, 256);

                    projectile.velocity = new Vector2(0, (float)Math.Sin(projectile.ai[0] * 3) / 20);
                    break;
                case 1:
                    projectile.alpha += 8;
                    break;
            }

            if (Vector2.Distance(projectile.Center, Main.LocalPlayer.Center) <= 48)
            {
                projectile.timeLeft = 32;
                projectile.ai[1] = 1;
                Main.LocalPlayer.GetModPlayer<EEPlayer>().hasGottenRuneBefore[RuneID] = 1;
                projectile.Kill();
            }
        }
    }
}