using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using System;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Melee
{
    public class BubbleStrikerProjectile : Shiv
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubble Striker");
        }

        public override void SetDefaults()
        {
            projectile.width = 46;
            projectile.height = 48;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.scale = 1f;
            projectile.alpha = 0;
            projectile.timeLeft = 159;

            projectile.ownerHitCheck = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.damage = 20;
            projectile.knockBack = 4.5f;
            projectile.ai[0] = 0.1f;
            projectile.rotation = MathHelper.Pi;
        }

        public override void AI()
        {
            Player owner = Main.player[projectile.owner];
            projectile.Center = owner.Center;
            projectile.spriteDirection = -owner.direction;

            if (projectile.ai[1] < 120)
            {
                if (owner.channel)
                    projectile.ai[1]++;

                projectile.ai[0] = 0.1f + (projectile.ai[1] / 360f);
                projectile.rotation += projectile.ai[0] * -owner.direction;
            }
            else
            {
                if(projectile.ai[1] == 120)
                {
                    Main.PlaySound(SoundID.NPCDeath7, projectile.Center);
                }
                projectile.ai[1]++;
                if(projectile.ai[0] != 0)
                    owner.velocity += new Vector2(owner.direction * 2, -18);

                if((projectile.ai[1] - 120) % 10 == 0)
                {
                    Projectile.NewProjectile(projectile.Center, owner.velocity, ModContent.ProjectileType<WaterDragonsBubble>(), projectile.damage / 2, 0);
                }

                projectile.ai[0] = 0;
            }
        }
    }
}