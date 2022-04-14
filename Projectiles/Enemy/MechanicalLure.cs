using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Enemy
{
    public class MechanicalLure : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mechanical Lure");
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 16;
            Projectile.alpha = 0;
            Projectile.timeLeft = 1200;
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            // Projectile.friendly = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.spriteDirection = -1;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.velocity = new Vector2(0, 2);
            }

            if (Projectile.ai[0] == 1)
            {
                Projectile.velocity = new Vector2(0, -4);
                Main.player[(int)Projectile.ai[1]].Center = Projectile.Center;
            }
            if (Projectile.Center.Y < Main.npc[Projectile.owner].Center.Y)
            {
                Projectile.Kill();
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.npc[Projectile.owner].ai[3] = 0;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            Projectile.ai[1] = target.whoAmI;
            Projectile.ai[0] = 1;
        }
    }
}