using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Melee
{
    public class DalantiniumKusiragamaProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dalantinium Kusiragama");
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

        public override void AI()
        {
            
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            
        }
    }
}