using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Melee
{
    public class FeatheredChakramProjectileAlt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Feathered Chakram");
        }

        public override void SetDefaults()
        {
            projectile.width = 44;
            projectile.height = 60;
            projectile.aiStyle = -1;
            projectile.melee = true;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.extraUpdates = 2;
            projectile.tileCollide = false;
        }

        float radius = 16;
        public override void AI()
        {
            int dust = Dust.NewDust(projectile.Center, 0, 0, 127);
            projectile.Center = Main.LocalPlayer.Center + Vector2.UnitY.RotatedBy(projectile.ai[0]) * radius;
            projectile.rotation+=5;
            radius++;
            projectile.ai[0]+=0.1f;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 180);
        }
    }
}