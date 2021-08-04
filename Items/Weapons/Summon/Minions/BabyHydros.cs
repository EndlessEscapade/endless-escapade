using EEMod.Buffs.Buffs;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Summon.Minions
{
    public class BabyHydros : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Baby Hydros");
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 48;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.aiStyle = 0;
            Projectile.minion = true;
            Projectile.damage = 0;
            Projectile.knockBack = 0;
            Projectile.minionSlots = 1;
        }

        private int attackTimer = 90;
        private bool firstFrame = true;

        public override void AI()
        {
            if (firstFrame)
            {
                Main.LocalPlayer.AddBuff(ModContent.BuffType<BabyHydrosBuff>(), 300000, true);
                Projectile.damage = 0;
                Projectile.knockBack = 0;
                firstFrame = false;
            }
            Projectile.spriteDirection = -Main.LocalPlayer.direction;

            attackTimer--;
            if (attackTimer <= 0)
            {
                for (int i = 0; i < 200; i++)
                {
                    NPC target = Main.npc[i];
                    float shootToX = target.BottomRight.X - Projectile.Center.X;
                    float shootToY = target.Center.Y - Projectile.Center.Y;
                    float distance = (float)Math.Sqrt(shootToX * shootToX + shootToY * shootToY);
                    if (distance < 1080f && !target.friendly && target.active)
                    {
                        distance = 3f / distance;
                        shootToX *= distance * 5;
                        shootToY *= distance * 5;
                        //int proj = Projectile.NewProjectile(projectile.Center, new Vector2(shootToX, shootToY), ModContent.ProjectileType<HydroBurst>(), 15, 0);
                        //Main.projectile[proj].netUpdate = true;
                        Projectile.netUpdate = true;
                        break;
                    }
                    attackTimer = 90;
                }
            }

            Projectile.position = new Vector2(Main.LocalPlayer.position.X, Main.LocalPlayer.Top.Y - 80);

            if (!Main.LocalPlayer.HasBuff(ModContent.BuffType<BabyHydrosBuff>()))
            {
                Projectile.Kill();
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