using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Items.FishingPoles;
using Terraria.GameContent;


namespace EEMod.Projectiles.FishingBobbers
{
    public class SailorsPoleBobber : EEProjectile
    {
        private bool initialized = false;
        private Color fishingLineColor;
        public Color PossibleLineColors = Color.White;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sailor's Bobber");
        }

        public override void SetDefaults()
        {
            //These are copied through the CloneDefaults method
            //projectile.aiStyle = 61;
            //projectile.bobber = true;
            //projectile.penetrate = -1;
            Projectile.CloneDefaults(ProjectileID.BobberWooden);
            Projectile.width = 10;
            Projectile.height = 22;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //Create some light based on the color of the line; this could also be in the AI function
            Lighting.AddLight(Projectile.Center, fishingLineColor.R / 255, fishingLineColor.G / 255, fishingLineColor.B / 255);

            //Change these two values in order to change the origin of where the line is being drawn
            int xPositionAdditive = 45;
            float yPositionAdditive = 35f;

            Player player = Main.player[Projectile.owner];
            if (!Projectile.bobber || player.inventory[player.selectedItem].holdStyle <= 0)
            {
                return false;
            }

            float originX = player.MountedCenter.X;
            float originY = player.MountedCenter.Y;
            originY += player.gfxOffY;
            int type = player.inventory[player.selectedItem].type;
            //This variable is used to account for Gravitation Potions
            float gravity = player.gravDir;

            if (type == ModContent.ItemType<SailorsPole>())
            {
                originX += xPositionAdditive * player.direction;
                if (player.direction < 0)
                {
                    originX -= 13f;
                }
                originY -= yPositionAdditive * gravity;
            }

            if (gravity == -1f)
            {
                originY -= 12f;
            }
            Vector2 mountedCenter = new Vector2(originX, originY);
            mountedCenter = player.RotatedRelativePoint(mountedCenter + new Vector2(8f), true) - new Vector2(8f);
            Vector2 lineOrigin = Projectile.Center - mountedCenter;
            bool canDraw = true;
            if (lineOrigin.X == 0f && lineOrigin.Y == 0f)
            {
                return false;
            }

            float projPosMagnitude = lineOrigin.Length();
            projPosMagnitude = 12f / projPosMagnitude;
            lineOrigin.X *= projPosMagnitude;
            lineOrigin.Y *= projPosMagnitude;
            mountedCenter -= lineOrigin;
            lineOrigin = Projectile.Center - mountedCenter;

            while (canDraw)
            {
                float height = 12f;
                float positionMagnitude = lineOrigin.Length();
                if (float.IsNaN(positionMagnitude))
                {
                    break;
                }

                if (positionMagnitude < 20f)
                {
                    height = positionMagnitude - 8f;
                    canDraw = false;
                }
                positionMagnitude = 12f / positionMagnitude;
                lineOrigin.X *= positionMagnitude;
                lineOrigin.Y *= positionMagnitude;
                mountedCenter += lineOrigin;
                lineOrigin = Projectile.Center - mountedCenter;
                if (positionMagnitude > 12f)
                {
                    float positionInverseMultiplier = 0.3f;
                    float absVelocitySum = Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y);
                    if (absVelocitySum > 16f)
                    {
                        absVelocitySum = 16f;
                    }
                    absVelocitySum = 1f - absVelocitySum / 16f;
                    positionInverseMultiplier *= absVelocitySum;
                    absVelocitySum = positionMagnitude / 80f;
                    if (absVelocitySum > 1f)
                    {
                        absVelocitySum = 1f;
                    }
                    positionInverseMultiplier *= absVelocitySum;
                    if (positionInverseMultiplier < 0f)
                    {
                        positionInverseMultiplier = 0f;
                    }
                    absVelocitySum = 1f - Projectile.localAI[0] / 100f;
                    positionInverseMultiplier *= absVelocitySum;
                    if (lineOrigin.Y > 0f)
                    {
                        lineOrigin.Y *= 1f + positionInverseMultiplier;
                        lineOrigin.X *= 1f - positionInverseMultiplier;
                    }
                    else
                    {
                        absVelocitySum = Math.Abs(Projectile.velocity.X) / 3f;
                        if (absVelocitySum > 1f)
                        {
                            absVelocitySum = 1f;
                        }
                        absVelocitySum -= 0.5f;
                        positionInverseMultiplier *= absVelocitySum;
                        if (positionInverseMultiplier > 0f)
                        {
                            positionInverseMultiplier *= 2f;
                        }
                        lineOrigin.Y *= 1f + positionInverseMultiplier;
                        lineOrigin.X *= 1f - positionInverseMultiplier;
                    }
                }
                //This color decides the color of the fishing line. The color is randomized as decided in the AI.
                Color lineColor = Lighting.GetColor((int)mountedCenter.X / 16, (int)(mountedCenter.Y / 16f), fishingLineColor);
                float rotation = (float)Math.Atan2(lineOrigin.Y, lineOrigin.X) - MathHelper.PiOver2;

                Main.spriteBatch.Draw(TextureAssets.FishingLine.Value, new Vector2(mountedCenter.X - Main.screenPosition.X + TextureAssets.FishingLine.Value.Width * 0.5f, mountedCenter.Y - Main.screenPosition.Y + TextureAssets.FishingLine.Value.Height * 0.5f), new Rectangle?(new Rectangle(0, 0, TextureAssets.FishingLine.Value.Width, (int)height)), lineColor, rotation, new Vector2(TextureAssets.FishingLine.Value.Width * 0.5f, 0f), 1f, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}