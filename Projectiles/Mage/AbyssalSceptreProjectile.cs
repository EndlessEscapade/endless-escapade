using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Projectiles.Mage
{
    public class AbyssalSceptreProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phantom Fish");
        }

        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 8;
            projectile.timeLeft = 420;
            projectile.ignoreWater = true;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.penetrate = 1;
        }

        public override void AI()
        {
            if (isClone)
            {
                projectile.ai[0]++;
                if (projectile.ai[0] == 1)
                    for (var a = 0; a < 50; a++)
                    {
                        Vector2 vector = new Vector2(0, 20).RotatedBy(((Math.PI * 0.04) * a), default);
                        int index = Dust.NewDust(projectile.Center, 22, 22, DustID.SolarFlare, vector.X, vector.Y, 0, new Color(0, 255, 0), 1f);
                        Main.dust[index].velocity *= .5f;
                        Main.dust[index].noGravity = true;
                    }
            }
            Dust.NewDust(projectile.position, projectile.width, projectile.height, 75);
            projectile.rotation = projectile.velocity.ToRotation();
        }

        private bool isClone;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!isClone)
            {
                float rot = Main.rand.NextFloat(6.28f);
                Vector2 position = target.Center + Vector2.One.RotatedBy(rot) * 300;
                for (var a = 0; a < 50; a++)
                {
                    Vector2 vector = new Vector2(0, 20).RotatedBy(((Math.PI * 0.04) * a), default);
                    int index = Dust.NewDust(projectile.Center, 22, 22, DustID.SolarFlare, vector.X, vector.Y, 0, new Color(0, 255, 0), 1f);
                    Main.dust[index].velocity *= 1.1f;
                    Main.dust[index].noGravity = true;
                }
                Vector2 velocity = Vector2.One.RotatedBy(rot) * -1 * 16f;
                int newProj = Projectile.NewProjectile(position, velocity, ModContent.ProjectileType<AbyssalSceptreProjectile>(), damage, projectile.knockBack, projectile.owner, 0, 0);
                Main.projectile[newProj].tileCollide = true;
                Main.projectile[newProj].timeLeft = 60;
                (Main.projectile[newProj].modProjectile as AbyssalSceptreProjectile).isClone = true;
            }
            Dust.NewDust(projectile.position, projectile.width, projectile.height, 75);
        }
    }
}
