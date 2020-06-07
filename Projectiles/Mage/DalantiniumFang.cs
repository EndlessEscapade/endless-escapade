using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace InteritosMod.Projectiles.Mage
{
    public class DalantiniumFang : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dalantinium Fang");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.PineNeedleFriendly); // CloneDefaults overrides some values
            projectile.width = 4;
            projectile.height = 4;
            projectile.timeLeft = 600;
            projectile.ignoreWater = true;
            projectile.hostile = false;
            projectile.friendly = true;
            aiType = ProjectileID.PineNeedleFriendly;
            projectile.penetrate = 1;
        }
        public override bool PreKill(int timeLeft)
        {
            projectile.type = ProjectileID.PineNeedleFriendly;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Dust.NewDust(projectile.Center, 0, 0, DustID.Blood, 0, 0, 0, Color.Gray, 1);

            return true;
        }
    }
}