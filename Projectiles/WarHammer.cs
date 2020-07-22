using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;

namespace EEMod.Projectiles
{
    public abstract class WarHammer : ModProjectile
    {
        public virtual float rotationalCoverage => (float)Math.PI;
        public virtual float RotationalOffset => (float)Math.PI / 2f;
        protected float progression => (projOwner.itemAnimation / (float)projOwner.itemAnimationMax);
        public virtual float dirtSmashIntensity => 12;
        public virtual int shakeLength => 20;
        public virtual int shakeIntensity => 3;
        public virtual int AoE => 1000;
        protected Player projOwner => Main.player[projectile.owner];

        public override void AI()
        {
            Vector2 tilePos = projOwner.position / 16f;
            projectile.direction = projOwner.direction;
            projOwner.heldProj = projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;
            projectile.position.X = projOwner.Center.X - projectile.width/2;
            projectile.position.Y = projOwner.Center.Y - projectile.height / 2;
            if (projOwner.direction == 1)
            {
                projectile.rotation = -(rotationalCoverage * progression) + RotationalOffset;
            }
            else
            {
                projectile.rotation = (RotationalOffset - rotationalCoverage) + (rotationalCoverage * progression);
                projectile.spriteDirection = -1;
            }
            if (projOwner.itemAnimation <= 1)
            {
                if (Main.tile[(int)tilePos.X + projOwner.direction, (int)tilePos.Y + 3].active())
                {
                    projectile.ai[0]++;
                    if (projectile.ai[0] == 1)
                        for (var i = 0; i < 20; i++)
                        {
                            int num = Dust.NewDust(projOwner.Center + new Vector2(projectile.width / 2 * projOwner.direction, projectile.height / 2f - 16), 2, 2, DustID.Dirt, 0, Main.rand.NextFloat(-dirtSmashIntensity, -1f), 6, new Color(255, 217, 184, 255), 1);
                            Main.dust[num].noGravity = true;
                            Main.dust[num].velocity.X *= 0.7f;
                            Main.dust[num].noLight = false;
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
