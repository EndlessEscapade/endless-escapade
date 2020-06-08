using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Microsoft.Xna.Framework;

namespace EEMod.NPCs.Bosses.Hydros
{
    public class HydrosBeam : ModProjectile
    {
        public override string Texture => Helpers.EmptyTexture;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hydros Beam");
        }
        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.alpha = 0;
            projectile.timeLeft = 120;
        }
        public override void AI()
        {
            if (projectile.localAI[0] == 0f)
            {
                int p = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, ProjectileType<HydroBeam>(), projectile.damage, 0f, Main.myPlayer, 0, projectile.ai[1]);
                projectile.velocity = Vector2.Zero;
                Main.projectile[p].netUpdate = true;
                projectile.localAI[0] = 1;
            }
        }
    }
}