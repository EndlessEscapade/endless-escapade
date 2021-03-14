using EEMod.Buffs.Buffs;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Projectiles.Summons
{
    public class BabyHydros : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Baby Hydros");
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 48;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.ignoreWater = false;
            projectile.tileCollide = false;
            projectile.extraUpdates = 1;
            projectile.aiStyle = 0;
            projectile.minion = true;
            projectile.damage = 0;
            projectile.knockBack = 0;
            projectile.minionSlots = 1;
        }

        private int attackTimer = 90;
        private bool firstFrame = true;

        public override void AI()
        {
            if (firstFrame)
            {
                Main.LocalPlayer.AddBuff(ModContent.BuffType<BabyHydrosBuff>(), 300000, true);
                projectile.damage = 0;
                projectile.knockBack = 0;
                firstFrame = false;
            }
            projectile.spriteDirection = -Main.LocalPlayer.direction;

            attackTimer--;
            if (attackTimer <= 0)
            {
                for (int i = 0; i < 200; i++)
                {
                    NPC target = Main.npc[i];
                    float shootToX = target.BottomRight.X - projectile.Center.X;
                    float shootToY = target.Center.Y - projectile.Center.Y;
                    float distance = (float)Math.Sqrt(shootToX * shootToX + shootToY * shootToY);
                    if (distance < 1080f && !target.friendly && target.active)
                    {
                        distance = 3f / distance;
                        shootToX *= distance * 5;
                        shootToY *= distance * 5;
                        int proj = Projectile.NewProjectile(projectile.Center, new Vector2(shootToX, shootToY), ModContent.ProjectileType<HydroBurst>(), 15, 0);
                        Main.projectile[proj].netUpdate = true;
                        projectile.netUpdate = true;
                        break;
                    }
                    attackTimer = 90;
                }
            }

            projectile.position = new Vector2(Main.LocalPlayer.position.X, Main.LocalPlayer.Top.Y - 80);

            if (!Main.LocalPlayer.HasBuff(ModContent.BuffType<BabyHydrosBuff>()))
            {
                projectile.Kill();
            }
        }

        public override void Kill(int timeleft)
        {
            if (Main.LocalPlayer.HasBuff(ModContent.BuffType<BabyHydrosBuff>()))
            {
                Main.LocalPlayer.ClearBuff(ModContent.BuffType<BabyHydrosBuff>());
            }
        }
    }
}