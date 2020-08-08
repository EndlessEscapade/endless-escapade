using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace EEMod.Projectiles.Mage
{
    public class SpiritPistolProjectileMain : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 12;       //projectile width
            projectile.height = 12;  //projectile height
            projectile.friendly = true;      //make that the projectile will not damage you
            projectile.magic = true;     //
            projectile.tileCollide = true;   //make that the projectile will be destroed if it hits the terrain
            projectile.penetrate = -1;      //how many npc will penetrate
                                            //how many time this projectile has before disepire
            projectile.light = 0.3f;    // projectile light
            projectile.ignoreWater = true;
            projectile.aiStyle = 0;
            projectile.timeLeft = 300;
            projectile.alpha = 255;
        }

        bool firstFrame = true;
        int[] linkedProj = new int[6];
        bool a = false;
        public override void AI()
        {
            if (firstFrame)
            {
                for (int i = 0; i < 6; i++)
                {
                    int proj = Projectile.NewProjectile(projectile.position, Vector2.Zero, ModContent.ProjectileType<SpiritPistolProjectileSecondary>(), projectile.damage, projectile.knockBack, Owner: projectile.owner, ai0: i * (MathHelper.TwoPi / 6), ai1: projectile.whoAmI);
                    linkedProj[i] = proj;
                }
                firstFrame = false;
            }
            projectile.rotation = projectile.velocity.ToRotation();
        }

        public override void Kill(int timeLeft)
        {
            foreach (int proj in linkedProj)
            {
                Main.projectile[proj].Kill();
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.velocity = Vector2.Zero;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.velocity = Vector2.Zero;
            return false;
        }
    }
}
