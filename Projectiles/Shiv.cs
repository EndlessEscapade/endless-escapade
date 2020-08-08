using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;
using System.Collections.Generic;

namespace EEMod.Projectiles
{
    public abstract class Shiv : ModProjectile
    {
        public virtual float rotationalCoverage => (float)Math.PI;
        public virtual float RotationalOffset => (float)Math.PI / 2f;
        protected float progression => (projOwner.itemAnimation / (float)projOwner.itemAnimationMax);
        public virtual float dirtSmashIntensity => 12;
        public virtual int shakeLength => 20;
        public virtual int shakeIntensity => 3;
        public virtual int AoE => 1000;
        public virtual bool canCrash => false;
        protected Player projOwner => Main.player[projectile.owner];
        public virtual float damageIncreaseOverTime => 0.01f;
        public virtual float weight => 1;

        public float damageMultiplier = 1;

        public virtual List<int> exclude => new List<int> { };
        public float xDis;
        public override void AI()
        {
            float perc = projOwner.itemAnimation / (float)projOwner.itemAnimationMax;
            if (projOwner.itemAnimation == projOwner.itemAnimationMax - 1)
            {
                do
                {
                    projectile.ai[0] = Main.rand.Next(0, 7);
                } while (exclude.Contains((int)projectile.ai[0]));
            }
            projectile.direction = projOwner.direction;
            projOwner.heldProj = projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;
            if (projectile.ai[0] != 4)
            {
                projectile.position.X = projOwner.Center.X - projectile.width / 2;
                projectile.position.Y = projOwner.Center.Y - projectile.height / 2;
            }
            if (projectile.ai[0] == 0) //schpeen
            {
                projectile.rotation += 0.2f;
            }
            if (projectile.ai[0] == 1) //schtab
            {
                projectile.rotation = (float)Math.PI / 4 + Main.rand.NextFloat(-0.2f, 0.2f) + (Main.MouseWorld - projOwner.Center).ToRotation();
                float percentageX = (float)Math.Sin(projectile.rotation);
                float percentageY = (float)Math.Cos(projectile.rotation);
                projectile.position.X += projOwner.itemAnimation % (projOwner.itemAnimationMax / 5) * percentageX;
                projectile.position.Y += projOwner.itemAnimation % (projOwner.itemAnimationMax / 5) * percentageY;
            }
            if (projectile.ai[0] == 2) //dasche
            {
                if (projOwner.direction == 1)
                {
                    projectile.rotation += ((float)(Math.PI * .25f) - projectile.rotation) / 8f;
                    if (Math.Abs(projectile.rotation - (float)(Math.PI * .25f)) < 0.02f)
                    {
                        projOwner.velocity.X = 5 * projOwner.direction;
                        xDis += (projOwner.itemAnimationMax - projOwner.itemAnimation) * 0.04f * projOwner.direction;
                        projectile.position.X += xDis;
                    }
                    else
                    {
                        xDis -= (projOwner.itemAnimationMax - projOwner.itemAnimation) * 0.05f * projOwner.direction;
                        projectile.position.X += xDis;
                    }
                }
                else
                {
                    projectile.rotation += ((float)(Math.PI * 1.25f) - projectile.rotation) / 8f;
                    if (Math.Abs(projectile.rotation - (float)(Math.PI * 1.25f)) < 0.02f)
                    {
                        projOwner.velocity.X = 5 * projOwner.direction;
                        xDis += (projOwner.itemAnimationMax - projOwner.itemAnimation) * 0.04f * projOwner.direction;
                        projectile.position.X += xDis;
                    }
                    else
                    {
                        xDis -= (projOwner.itemAnimationMax - projOwner.itemAnimation) * 0.05f * projOwner.direction;
                        projectile.position.X += xDis;
                    }
                }

            }
            if (projectile.ai[0] == 3) //crasche
            {

                projectile.rotation = -(float)Math.PI / 4;
                if (perc > 0.9f)
                {
                    projectile.alpha = 255;
                }
                else
                {
                    projectile.alpha -= 3;
                }
                if (perc > 0.5f)
                {
                    xDis += (200 - xDis) / 16f;
                    projectile.position.Y -= xDis;
                }
                else
                {
                    Vector2 tilePos = projOwner.position / 16f;
                    Tile tile = Main.tile[(int)tilePos.X + projOwner.direction, (int)tilePos.Y + 3];
                    Tile tile2 = Main.tile[(int)tilePos.X + projOwner.direction, (int)tilePos.Y + 4];
                    xDis += (-4 - xDis) / 16f;
                    projectile.position.Y -= xDis;
                    if (Main.tile[(int)tilePos.X + projOwner.direction, (int)tilePos.Y + 3].active() &&
                    (Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type] && tile.frameY == 0) &&
                    Main.tile[(int)tilePos.X + projOwner.direction, (int)tilePos.Y + 4].active() &&
                    (Main.tileSolid[tile2.type] || Main.tileSolidTop[tile2.type] && tile2.frameY == 0) &&
                    tile.type != TileID.Trees &&
                    tile2.type != TileID.Trees)
                    {
                        Main.LocalPlayer.GetModPlayer<EEPlayer>().FixateCameraOn(projectile.Center, 16f, true, false, (int)(shakeIntensity * damageMultiplier));
                        for (var i = 0; i < 20; i++)
                        {
                            int num = Dust.NewDust(projOwner.Center + new Vector2((i * 10) - 100, projectile.height / 2f - 16), 2, 2, DustID.Dirt, 0, -Math.Abs(i - 10), 6, default, 0.7f);
                            Main.dust[num].noGravity = false;
                            Main.dust[num].velocity.X *= 0.7f;
                            Main.dust[num].noLight = false;
                        }
                        if (perc < 0.2f)
                        {
                            projectile.width = 1000;
                            projectile.height = 1000;
                            projectile.alpha = 255;
                        }
                    }
                }
            }
            if (projectile.ai[0] == 4) //throe
            {
                float radial = 75;
                float inverseSpeed = 100;
                float dampeningEffect = 0.07f;
                Vector2 goTo = Main.MouseWorld;
                if (goTo.X < projOwner.Center.X - radial)
                {
                    goTo.X = projOwner.Center.X - radial;
                }
                if (goTo.X > projOwner.Center.X + radial)
                {
                    goTo.X = projOwner.Center.X + radial;
                }
                if (goTo.Y < projOwner.Center.Y - radial)
                {
                    goTo.Y = projOwner.Center.Y - radial;
                }
                if (goTo.Y > projOwner.Center.Y + radial)
                {
                    goTo.Y = projOwner.Center.Y + radial;
                }
                if (projectile.ai[0] == 1)
                {
                    goTo = projOwner.Center + new Vector2(0, 50);
                }
                projectile.velocity += (goTo - projectile.Center) / inverseSpeed - (projectile.velocity * dampeningEffect); projectile.rotation += projectile.velocity.X / 16f;
                projectile.rotation += projectile.velocity.X / 128f;
            }
            if (projectile.ai[0] == 5) //across da skrene
            {
                xDis += (6000 - xDis) / 64f;
                projectile.position.X += -2000 + xDis;
                projectile.rotation = (float)Math.PI / 4;
            }
            if (projectile.ai[0] == 6) //jump and smasche
            {
                if (perc > 0.9f)
                {
                    projOwner.velocity.Y -= 2f;
                }
                else if (perc < 0.5f)
                {
                    Vector2 tilePos = projOwner.position / 16f;
                    Tile tile = Main.tile[(int)tilePos.X + projOwner.direction, (int)tilePos.Y + 3];
                    Tile tile2 = Main.tile[(int)tilePos.X + projOwner.direction, (int)tilePos.Y + 4];
                    projOwner.velocity.Y += 2f;
                    projectile.rotation += (-(float)Math.PI / 4 - projectile.rotation) / 4f;
                    if (Main.tile[(int)tilePos.X + projOwner.direction, (int)tilePos.Y + 3].active() &&
                    (Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type] && tile.frameY == 0) &&
                    Main.tile[(int)tilePos.X + projOwner.direction, (int)tilePos.Y + 4].active() &&
                    (Main.tileSolid[tile2.type] || Main.tileSolidTop[tile2.type] && tile2.frameY == 0) &&
                    tile.type != TileID.Trees &&
                    tile2.type != TileID.Trees)
                    {
                        if (perc < 0.3f)
                        {
                            projectile.width = 1000;
                            projectile.height = 1000;
                            projectile.alpha = 255;

                            Main.LocalPlayer.GetModPlayer<EEPlayer>().FixateCameraOn(projectile.Center, 16f, true, false, (int)(shakeIntensity * damageMultiplier));
                            for (var i = 0; i < 10; i++)
                            {
                                int num = Dust.NewDust(projOwner.Center + new Vector2((i * 10) - 50, projectile.height / 2f - 16), 2, 2, DustID.Dirt, 0, -Math.Abs(i - 10) * 0.5f, 6, default, 0.7f);
                                Main.dust[num].noGravity = false;
                                Main.dust[num].velocity.X *= 0.7f;
                                Main.dust[num].noLight = false;
                            }
                        }
                    }
                }
            }
            if (projOwner.itemAnimation <= 1)
            {
                projectile.Kill();
                Main.LocalPlayer.GetModPlayer<EEPlayer>().TurnCameraFixationsOff();
            }
        }
    }
}
