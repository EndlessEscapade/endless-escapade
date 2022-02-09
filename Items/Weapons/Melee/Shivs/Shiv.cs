using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Melee.Shivs
{
    public abstract class Shiv : EEProjectile
    {
        public virtual float rotationalCoverage => MathHelper.Pi;
        public virtual float RotationalOffset => MathHelper.PiOver2;
        protected float progression => projOwner.itemAnimation / (float)projOwner.itemAnimationMax;
        public virtual float dirtSmashIntensity => 12;
        public virtual int shakeLength => 20;
        public virtual int shakeIntensity => 3;
        public virtual int AoE => 1000;
        public virtual bool canCrash => false;
        protected Player projOwner => Main.player[Projectile.owner];
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
                    Projectile.ai[0] = Main.rand.Next(0, 7);
                } while (exclude.Contains((int)Projectile.ai[0]));
            }
            Projectile.direction = projOwner.direction;
            projOwner.heldProj = Projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;
            if (Projectile.ai[0] != 4)
            {
                Projectile.position.X = projOwner.Center.X - Projectile.width / 2;
                Projectile.position.Y = projOwner.Center.Y - Projectile.height / 2;
            }
            if (Projectile.ai[0] == 0) //schpeen
            {
                Projectile.rotation += 0.2f;
            }
            if (Projectile.ai[0] == 1) //schtab
            {
                Projectile.rotation = MathHelper.Pi / 4 + Main.rand.NextFloat(-0.2f, 0.2f) + (Main.MouseWorld - projOwner.Center).ToRotation();
                float percentageX = (float)Math.Sin(Projectile.rotation);
                float percentageY = (float)Math.Cos(Projectile.rotation);
                Projectile.position.X += projOwner.itemAnimation % (projOwner.itemAnimationMax / 5) * percentageX;
                Projectile.position.Y += projOwner.itemAnimation % (projOwner.itemAnimationMax / 5) * percentageY;
            }
            if (Projectile.ai[0] == 2) //dasche
            {
                if (projOwner.direction == 1)
                {
                    Projectile.rotation += ((float)(Math.PI * .25f) - Projectile.rotation) / 8f;
                    if (Math.Abs(Projectile.rotation - (float)(Math.PI * .25f)) < 0.02f)
                    {
                        projOwner.velocity.X = 5 * projOwner.direction;
                        xDis += (projOwner.itemAnimationMax - projOwner.itemAnimation) * 0.04f * projOwner.direction;
                        Projectile.position.X += xDis;
                    }
                    else
                    {
                        xDis -= (projOwner.itemAnimationMax - projOwner.itemAnimation) * 0.05f * projOwner.direction;
                        Projectile.position.X += xDis;
                    }
                }
                else
                {
                    Projectile.rotation += ((float)(Math.PI * 1.25f) - Projectile.rotation) / 8f;
                    if (Math.Abs(Projectile.rotation - (float)(Math.PI * 1.25f)) < 0.02f)
                    {
                        projOwner.velocity.X = 5 * projOwner.direction;
                        xDis += (projOwner.itemAnimationMax - projOwner.itemAnimation) * 0.04f * projOwner.direction;
                        Projectile.position.X += xDis;
                    }
                    else
                    {
                        xDis -= (projOwner.itemAnimationMax - projOwner.itemAnimation) * 0.05f * projOwner.direction;
                        Projectile.position.X += xDis;
                    }
                }
            }
            if (Projectile.ai[0] == 3) //crasche
            {

                Projectile.rotation = -MathHelper.Pi / 4;
                if (perc > 0.9f)
                {
                    Projectile.alpha = 255;
                }
                else
                {
                    Projectile.alpha -= 3;
                }
                if (perc > 0.5f)
                {
                    xDis += (200 - xDis) / 16f;
                    Projectile.position.Y -= xDis;
                }
                else
                {
                    Vector2 tilePos = projOwner.position / 16f;
                    Tile tile = Framing.GetTileSafely((int)tilePos.X + projOwner.direction, (int)tilePos.Y + 3);
                    Tile tile2 = Framing.GetTileSafely((int)tilePos.X + projOwner.direction, (int)tilePos.Y + 4);
                    xDis += (-4 - xDis) / 16f;
                    Projectile.position.Y -= xDis;
                    if (Framing.GetTileSafely((int)tilePos.X + projOwner.direction, (int)tilePos.Y + 3).HasTile &&
                    (Main.tileSolid[tile.TileType] || Main.tileSolidTop[tile.TileType] && tile.TileFrameY == 0) &&
                    Framing.GetTileSafely((int)tilePos.X + projOwner.direction, (int)tilePos.Y + 4).HasTile &&
                    (Main.tileSolid[tile2.TileType] || Main.tileSolidTop[tile2.TileType] && tile2.TileFrameY == 0) &&
                    tile.TileType != TileID.Trees &&
                    tile2.TileType != TileID.Trees)
                    {
                        Main.LocalPlayer.GetModPlayer<EEPlayer>().FixateCameraOn(Projectile.Center, 16f, true, false, (int)(shakeIntensity * damageMultiplier));
                        for (var i = 0; i < 20; i++)
                        {
                            int num = Dust.NewDust(projOwner.Center + new Vector2((i * 10) - 100, Projectile.height / 2f - 16), 2, 2, DustID.Dirt, 0, -Math.Abs(i - 10), 6, default, 0.7f);
                            // Main.dust[num].noGravity = false;
                            Main.dust[num].velocity.X *= 0.7f;
                            // Main.dust[num].noLight = false;
                        }
                        if (perc < 0.2f)
                        {
                            Projectile.width = 1000;
                            Projectile.height = 1000;
                            Projectile.alpha = 255;
                        }
                    }
                }
            }
            if (Projectile.ai[0] == 4) //throe
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
                if (Projectile.ai[0] == 1)
                {
                    goTo = projOwner.Center + new Vector2(0, 50);
                }
                Projectile.velocity += (goTo - Projectile.Center) / inverseSpeed - (Projectile.velocity * dampeningEffect); Projectile.rotation += Projectile.velocity.X / 16f;
                Projectile.rotation += Projectile.velocity.X / 128f;
            }
            if (Projectile.ai[0] == 5) //across da skrene
            {
                xDis += (6000 - xDis) / 64f;
                Projectile.position.X += -2000 + xDis;
                Projectile.rotation = MathHelper.Pi / 4;
            }
            if (Projectile.ai[0] == 6) //jump and smasche
            {
                if (perc > 0.9f)
                {
                    projOwner.velocity.Y -= 2f;
                }
                else if (perc < 0.5f)
                {
                    Vector2 tilePos = projOwner.position / 16f;
                    Tile tile = Framing.GetTileSafely((int)tilePos.X + projOwner.direction, (int)tilePos.Y + 3);
                    Tile tile2 = Framing.GetTileSafely((int)tilePos.X + projOwner.direction, (int)tilePos.Y + 4);
                    projOwner.velocity.Y += 2f;
                    Projectile.rotation += (-MathHelper.Pi / 4 - Projectile.rotation) / 4f;
                    if (Framing.GetTileSafely((int)tilePos.X + projOwner.direction, (int)tilePos.Y + 3).HasTile &&
                    (Main.tileSolid[tile.TileType] || Main.tileSolidTop[tile.TileType] && tile.TileFrameY == 0) &&
                    Framing.GetTileSafely((int)tilePos.X + projOwner.direction, (int)tilePos.Y + 4).HasTile &&
                    (Main.tileSolid[tile2.TileType] || Main.tileSolidTop[tile2.TileType] && tile2.TileFrameY == 0) &&
                    tile.TileType != TileID.Trees &&
                    tile2.TileType != TileID.Trees)
                    {
                        if (perc < 0.3f)
                        {
                            Projectile.width = 1000;
                            Projectile.height = 1000;
                            Projectile.alpha = 255;

                            Main.LocalPlayer.GetModPlayer<EEPlayer>().FixateCameraOn(Projectile.Center, 16f, true, false, (int)(shakeIntensity * damageMultiplier));
                            for (var i = 0; i < 10; i++)
                            {
                                int num = Dust.NewDust(projOwner.Center + new Vector2((i * 10) - 50, Projectile.height / 2f - 16), 2, 2, DustID.Dirt, 0, -Math.Abs(i - 10) * 0.5f, 6, default, 0.7f);
                                // Main.dust[num].noGravity = false;
                                Main.dust[num].velocity.X *= 0.7f;
                                // Main.dust[num].noLight = false;
                            }
                        }
                    }
                }
            }
            if (projOwner.itemAnimation <= 1)
            {
                Projectile.Kill();
                Main.LocalPlayer.GetModPlayer<EEPlayer>().TurnCameraFixationsOff();
            }
        }
    }
}