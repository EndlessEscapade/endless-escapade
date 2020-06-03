using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace EEMod.Projectiles.Ranger
{
    public class NapalmProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 16;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 500;
        }

        private int dustTimer;

        public override void AI()
        {
            //if  (projectile.localAI[0] != 0)
			//{
				//projectile.Center = Main.projectile[(int)localAI[0]].Center;
			//}

            if (dustTimer++ >= 5)
            {
                Dust.NewDust(projectile.Center, 2, 2, DustID.Fire, 0, 0, 255, Color.Orange, 0.5f);
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.position = target.position; // i gtg rn ;-; cya
            	projectile.localAI[0] = target.whoAmI;
		}
	}
}