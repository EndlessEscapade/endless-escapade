using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Enemy
{
    public class MechanicalLure : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mechanical Lure");
        }

        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 16;
            projectile.alpha = 0;
            projectile.timeLeft = 1200;
            projectile.penetrate = -1;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.scale = 1f;
            projectile.aiStyle = -1;
            projectile.spriteDirection = -1;
        }

        public override void AI()
        {
            if (projectile.ai[0] == 0)
                projectile.velocity = new Vector2(0, 2);
            if (projectile.ai[0] == 1)
            {
                projectile.velocity = new Vector2(0, -4);
                Main.player[(int)projectile.ai[1]].Center = projectile.Center;
            }
            if (projectile.Center.Y < Main.npc[projectile.owner].Center.Y)
                projectile.Kill();
        }

        public override void Kill(int timeLeft)
        {
            Main.npc[projectile.owner].ai[3] = 0;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            projectile.ai[1] = target.whoAmI;
            projectile.ai[0] = 1;
        }
    }
}