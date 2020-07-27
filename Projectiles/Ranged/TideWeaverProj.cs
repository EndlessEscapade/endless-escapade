using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;

namespace EEMod.Projectiles.Ranged
{
    public class TideWeaverProj : Longbow
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Atlantean Warhammer");
            Main.projFrames[projectile.type] = 7;
        }
        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 72;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.scale = 1f;
            projectile.alpha = 0;

            projectile.hide = true;
            projectile.ownerHitCheck = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.damage = 20;
            projectile.knockBack = 4.5f;
        }

        public override void AI()
        {
            base.AI();
            projectile.frameCounter++;
            if (projectile.frameCounter > 8 && projectile.frame < 6)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
        }
        public override float speedOfArrow => 3.5f;
        public override float minGrav => 0.3f;
        public override float ropeThickness => 40f;
    }
}
