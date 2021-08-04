using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Melee
{
    public class DalantiniumKusiragamaProjectile : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dalantinium Kusiragama");
        }

        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 60;
            Projectile.aiStyle = -1;
            Projectile.melee = true;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.extraUpdates = 2;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            
        }
    }
}