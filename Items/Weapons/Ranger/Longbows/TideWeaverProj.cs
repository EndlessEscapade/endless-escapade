using Terraria;

namespace EEMod.Items.Weapons.Ranger.Longbows
{
    public class TideWeaverProj : Longbow
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Atlantean Warhammer");
            Main.projFrames[Projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 72;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.alpha = 0;

            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.melee = true;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.damage = 20;
            Projectile.knockBack = 4.5f;
        }

        public override void AI()
        {
            base.AI();
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 8 && Projectile.frame < 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
        }

        public override float speedOfArrow => 3.5f;
        public override float minGrav => 0.3f;
        public override float ropeThickness => 40f;
    }
}