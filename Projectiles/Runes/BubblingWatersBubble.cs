using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Runes
{
    public class BubblingWatersBubble : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubbling Waters Bubble");
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 136;
            Projectile.height = 136;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            // Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 100000;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
            Projectile.aiStyle = -1;
            Projectile.damage = 0;
        }

        public override void AI()           //this make that the projectile will face the corect way
        {
            Projectile.Center = Main.player[Projectile.owner].Center;
            Projectile.ai[0]++;
            Projectile.scale += (float)Math.Sin(Projectile.ai[0] / 20) / 120;

            Projectile.ai[1]++;
            if (Projectile.ai[1] >= 120)
            {
                Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_Parent(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<BubblingWatersBubbleSmall>(), 0, 0, Owner: Main.myPlayer);
                Projectile.ai[1] = 0;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.Kill();
        }

        public override void Kill(int timeleft)
        {
            Main.player[Projectile.owner].GetModPlayer<EEPlayer>().bubbleRuneBubble = 0;
        }
    }
}