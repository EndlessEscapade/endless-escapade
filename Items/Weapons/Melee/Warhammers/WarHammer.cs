using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Melee.Warhammers
{
    public abstract class WarHammer : EEProjectile
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

        public override void AI()
        {
            bool isFlying = false;
            Vector2 tilePos = projOwner.position / 16f;
            Projectile.direction = projOwner.direction;
            projOwner.heldProj = Projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;
            Projectile.position.X = projOwner.Center.X - Projectile.width / 2;
            Projectile.position.Y = projOwner.Center.Y - Projectile.height / 2;
            if (damageMultiplier == 1)
            {
                if (projOwner.direction == 1)
                {
                    Projectile.rotation = -(rotationalCoverage * progression) + RotationalOffset;
                }
                else
                {
                    Projectile.rotation = RotationalOffset - rotationalCoverage + (rotationalCoverage * progression);
                    Projectile.spriteDirection = -1;
                }
            }
            if (canCrash)
            {
                Tile tile = Framing.GetTileSafely((int)tilePos.X + projOwner.direction, (int)tilePos.Y + 3);
                Tile tile2 = Framing.GetTileSafely((int)tilePos.X + projOwner.direction, (int)tilePos.Y + 4);
                if ((!Framing.GetTileSafely((int)tilePos.X + projOwner.direction, (int)tilePos.Y + 3).IsActive &&
                    (!Main.tileSolid[tile.type] || !Main.tileSolidTop[tile.type]) &&
                    !Framing.GetTileSafely((int)tilePos.X + projOwner.direction, (int)tilePos.Y + 4).IsActive &&
                    (!Main.tileSolid[tile2.type] || !Main.tileSolidTop[tile2.type]) ||
                    tile.type == TileID.Trees ||
                    tile2.type == TileID.Trees) && Projectile.ai[1] == 0)
                {
                    projOwner.velocity.Y += weight;
                    damageMultiplier += damageIncreaseOverTime;
                    isFlying = true;
                    if (projOwner.direction == 1)
                    {
                        Projectile.rotation = (float)(Math.PI * .75f);
                    }
                    else
                    {
                        Projectile.rotation = (float)(Math.PI * 1.25f);
                    }
                }
                else
                {
                    Projectile.ai[1] = 1;
                }
                if (!isFlying && damageMultiplier != 1 && Projectile.ai[0] == 0)
                {
                    Projectile.ai[0]++;
                }
                if (damageMultiplier != 1 && Projectile.ai[0] >= 1)
                {
                    if (Projectile.ai[0] == 2)
                    {
                        Projectile.damage = (int)(Projectile.damage * damageMultiplier);
                        for (var i = 0; i < 20; i++)
                        {
                            int num = Dust.NewDust(projOwner.Center + new Vector2(Projectile.width / 2 * projOwner.direction, Projectile.height / 2f - 16), 2, 2, DustID.Dirt, 0, Main.rand.NextFloat(-dirtSmashIntensity, -1f), 6, new Color(255, 217, 184, 255), 1);
                            Main.dust[num].noGravity = true;
                            Main.dust[num].velocity.X *= 0.7f;
                            // Main.dust[num].noLight = false;
                        }
                    }
                    Projectile.ai[0]++;
                    if (Projectile.ai[0] > 6)
                    {
                        Main.LocalPlayer.GetModPlayer<EEPlayer>().FixateCameraOn(Projectile.Center, 16f, true, false, (int)(shakeIntensity * damageMultiplier));
                        if (Projectile.ai[0] > shakeLength + 10)
                        {
                            Main.LocalPlayer.GetModPlayer<EEPlayer>().TurnCameraFixationsOff();
                            Projectile.Kill();
                        }
                        Projectile.alpha = 255;
                        Projectile.width = 1000;
                        Projectile.height = 1000;
                    }
                }
            }
            if (projOwner.itemAnimation <= 1 && damageMultiplier == 1)
            {
                Tile tile = Framing.GetTileSafely((int)tilePos.X + projOwner.direction, (int)tilePos.Y + 3);
                Tile tile2 = Framing.GetTileSafely((int)tilePos.X + projOwner.direction, (int)tilePos.Y + 4);
                if (Framing.GetTileSafely((int)tilePos.X + projOwner.direction, (int)tilePos.Y + 3).IsActive &&
                    (Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]) ||
                    Framing.GetTileSafely((int)tilePos.X + projOwner.direction, (int)tilePos.Y + 4).IsActive &&
                    (Main.tileSolid[tile2.type] || Main.tileSolidTop[tile2.type]) &&
                    tile.type != TileID.Trees &&
                    tile2.type != TileID.Trees)
                {
                    Projectile.ai[0]++;
                    if (Projectile.ai[0] == 1)
                    {
                        for (var i = 0; i < 20; i++)
                        {
                            int num = Dust.NewDust(projOwner.Center + new Vector2(Projectile.width / 2 * projOwner.direction, Projectile.height / 2f - 16), 2, 2, DustID.Dirt, 0, Main.rand.NextFloat(-dirtSmashIntensity, -1f), 6, new Color(255, 217, 184, 255), 1);
                            Main.dust[num].noGravity = true;
                            Main.dust[num].velocity.X *= 0.7f;
                            // Main.dust[num].noLight = false;
                        }
                    }

                    Main.LocalPlayer.GetModPlayer<EEPlayer>().FixateCameraOn(Projectile.Center, 16f, true, false, shakeIntensity);
                    if (Projectile.ai[0] > shakeLength)
                    {
                        Main.LocalPlayer.GetModPlayer<EEPlayer>().TurnCameraFixationsOff();
                        Projectile.Kill();
                    }
                    Projectile.alpha = 255;
                    Projectile.width = 1000;
                    Projectile.height = 1000;
                }
                else
                {
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().TurnCameraFixationsOff();
                    Projectile.Kill();
                }
            }
        }
    }
}