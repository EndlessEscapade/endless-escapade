using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Effects;

namespace EEMod.Projectiles.Runes
{
    public class BubblingWatersBubble : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubbling Waters Bubble");
            Main.projFrames[projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.width = 136;
            projectile.height = 136;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 100000;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.extraUpdates = 1;
            projectile.aiStyle = -1;
            projectile.damage = 0;
        }

        public override void AI()           //this make that the projectile will face the corect way
        {
            projectile.Center = Main.player[projectile.owner].Center;
            projectile.ai[0]++;
            projectile.scale += (float)Math.Sin(projectile.ai[0]/20)/120;

            projectile.ai[1]++;
            if(projectile.ai[1] >= 120)
            {
                Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<BubblingWatersBubbleSmall>(), 0, 0, Owner: Main.myPlayer);
                projectile.ai[1] = 0;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.Kill();
        }

        public override void Kill(int timeleft)
        {
            Main.player[projectile.owner].GetModPlayer<EEPlayer>().bubbleRuneBubble = 0;
        }
    }
}
