using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace EEMod.NPCs.Bosses.Hydros
{
    public class HydrosBeam : EEProjectile
    {
        public override string Texture => Helpers.EmptyTexture;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydros Beam");
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
            Projectile.timeLeft = 120;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0f)
            {
                int p = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, ProjectileType<HydroBeam>(), Projectile.damage, 0f, Main.myPlayer, 0, Projectile.ai[1]);
                Projectile.velocity = Vector2.Zero;
                Main.projectile[p].netUpdate = true;
                Projectile.localAI[0] = 1;
            }
        }
    }
}