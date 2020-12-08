using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Projectiles
{
    public abstract class WarHammer : ModProjectile
    {
        public virtual float rotationalCoverage => MathHelper.Pi;
        public virtual float RotationalOffset => MathHelper.PiOver2;
        protected float progression => projOwner.itemAnimation / (float)projOwner.itemAnimationMax;
        public virtual float dirtSmashIntensity => 12;
        public virtual int shakeLength => 20;
        public virtual int shakeIntensity => 3;
        public virtual int AoE => 1000;
        public virtual bool canCrash => false;
        protected Player projOwner => Main.player[projectile.owner];
        public virtual float damageIncreaseOverTime => 0.01f;
        public virtual float weight => 1;

        public float damageMultiplier = 1;

        public override void AI()
        {
            bool isFlying = false;
            Vector2 tilePos = projOwner.position / 16f;
            projectile.direction = projOwner.direction;
            projOwner.heldProj = projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;
            projectile.position.X = projOwner.Center.X - projectile.width / 2;
            projectile.position.Y = projOwner.Center.Y - projectile.height / 2;
            if (damageMultiplier == 1)
            {
                if (projOwner.direction == 1)
                {
                    projectile.rotation = -(rotationalCoverage * progression) + RotationalOffset;
                }
                else
                {
                    projectile.rotation = RotationalOffset - rotationalCoverage + (rotationalCoverage * progression);
                    projectile.spriteDirection = -1;
                }
            }
            if (canCrash)
            {
                Tile tile = Main.tile[(int)tilePos.X + projOwner.direction, (int)tilePos.Y + 3];
                Tile tile2 = Main.tile[(int)tilePos.X + projOwner.direction, (int)tilePos.Y + 4];
                if ((!Main.tile[(int)tilePos.X + projOwner.direction, (int)tilePos.Y + 3].active() &&
                    (!Main.tileSolid[tile.type] || !Main.tileSolidTop[tile.type]) &&
                    !Main.tile[(int)tilePos.X + projOwner.direction, (int)tilePos.Y + 4].active() &&
                    (!Main.tileSolid[tile2.type] || !Main.tileSolidTop[tile2.type]) ||
                    tile.type == TileID.Trees ||
                    tile2.type == TileID.Trees) && projectile.ai[1] == 0)
                {
                    projOwner.velocity.Y += weight;
                    damageMultiplier += damageIncreaseOverTime;
                    isFlying = true;
                    if (projOwner.direction == 1)
                    {
                        projectile.rotation = (float)(Math.PI * .75f);
                    }
                    else
                    {
                        projectile.rotation = (float)(Math.PI * 1.25f);
                    }
                }
                else
                {
                    projectile.ai[1] = 1;
                }
                if (!isFlying && damageMultiplier != 1 && projectile.ai[0] == 0)
                {
                    projectile.ai[0]++;
                }
                if (damageMultiplier != 1 && projectile.ai[0] >= 1)
                {
                    if (projectile.ai[0] == 2)
                    {
                        projectile.damage = (int)(projectile.damage * damageMultiplier);
                        for (var i = 0; i < 20; i++)
                        {
                            int num = Dust.NewDust(projOwner.Center + new Vector2(projectile.width / 2 * projOwner.direction, projectile.height / 2f - 16), 2, 2, DustID.Dirt, 0, Main.rand.NextFloat(-dirtSmashIntensity, -1f), 6, new Color(255, 217, 184, 255), 1);
                            Main.dust[num].noGravity = true;
                            Main.dust[num].velocity.X *= 0.7f;
                            Main.dust[num].noLight = false;
                        }
                    }
                    projectile.ai[0]++;
                    if (projectile.ai[0] > 6)
                    {
                        Main.LocalPlayer.GetModPlayer<EEPlayer>().FixateCameraOn(projectile.Center, 16f, true, false, (int)(shakeIntensity * damageMultiplier));
                        if (projectile.ai[0] > shakeLength + 10)
                        {
                            Main.LocalPlayer.GetModPlayer<EEPlayer>().TurnCameraFixationsOff();
                            projectile.Kill();
                        }
                        projectile.alpha = 255;
                        projectile.width = 1000;
                        projectile.height = 1000;
                    }
                }
            }
            if (projOwner.itemAnimation <= 1 && damageMultiplier == 1)
            {
                Tile tile = Main.tile[(int)tilePos.X + projOwner.direction, (int)tilePos.Y + 3];
                Tile tile2 = Main.tile[(int)tilePos.X + projOwner.direction, (int)tilePos.Y + 4];
                if (Main.tile[(int)tilePos.X + projOwner.direction, (int)tilePos.Y + 3].active() &&
                    (Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]) ||
                    Main.tile[(int)tilePos.X + projOwner.direction, (int)tilePos.Y + 4].active() &&
                    (Main.tileSolid[tile2.type] || Main.tileSolidTop[tile2.type]) &&
                    tile.type != TileID.Trees &&
                    tile2.type != TileID.Trees)
                {
                    projectile.ai[0]++;
                    if (projectile.ai[0] == 1)
                    {
                        for (var i = 0; i < 20; i++)
                        {
                            int num = Dust.NewDust(projOwner.Center + new Vector2(projectile.width / 2 * projOwner.direction, projectile.height / 2f - 16), 2, 2, DustID.Dirt, 0, Main.rand.NextFloat(-dirtSmashIntensity, -1f), 6, new Color(255, 217, 184, 255), 1);
                            Main.dust[num].noGravity = true;
                            Main.dust[num].velocity.X *= 0.7f;
                            Main.dust[num].noLight = false;
                        }
                    }

                    Main.LocalPlayer.GetModPlayer<EEPlayer>().FixateCameraOn(projectile.Center, 16f, true, false, shakeIntensity);
                    if (projectile.ai[0] > shakeLength)
                    {
                        Main.LocalPlayer.GetModPlayer<EEPlayer>().TurnCameraFixationsOff();
                        projectile.Kill();
                    }
                    projectile.alpha = 255;
                    projectile.width = 1000;
                    projectile.height = 1000;
                }
                else
                {
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().TurnCameraFixationsOff();
                    projectile.Kill();
                }
            }
        }
    }
}