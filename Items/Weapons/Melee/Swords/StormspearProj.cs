using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Melee.Swords
{
    public class StormspearProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stormspear");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.hide = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 6;
            projectile.ownerHitCheck = true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter);

            projectile.direction = player.direction;
            player.heldProj = projectile.whoAmI;
            projectile.Center = vector;

            if (player.dead)
            {
                projectile.Kill();
                return;
            }

            if (!player.frozen)
            {
                projectile.spriteDirection = (projectile.direction = player.direction);
                projectile.alpha -= 127;

                if (projectile.alpha < 0)
                {
                    projectile.alpha = 0;
                }

                float num7 = (float)player.itemAnimation / (float)player.itemAnimationMax;
                float num8 = 1f - num7;
                float num9 = projectile.velocity.ToRotation();
                float num10 = projectile.velocity.Length();
                float num11 = 22f;

                Vector2 spinningpoint2 = new Vector2(1f, 0f).RotatedBy((float)Math.PI + num8 * ((float)Math.PI * 2f)) * new Vector2(num10, projectile.ai[0]);
                projectile.position += spinningpoint2.RotatedBy(num9) + new Vector2(num10 + num11, 0f).RotatedBy(num9);
                Vector2 value2 = vector + spinningpoint2.RotatedBy(num9) + new Vector2(num10 + num11 + 40f, 0f).RotatedBy(num9);
                projectile.rotation = ((value2 - vector).SafeNormalize(Vector2.UnitX).ToRotation() + (float)Math.PI / 4f * (float)player.direction) - MathHelper.PiOver4 + MathHelper.Pi;

                if (projectile.spriteDirection == -1)
                {
                    //projectile.rotation += (float)Math.PI;
                }

                if (projectile.spriteDirection == 1)
                {
                    projectile.rotation -= MathHelper.PiOver2;
                }

                (projectile.Center - vector).SafeNormalize(Vector2.Zero);
                (value2 - vector).SafeNormalize(Vector2.Zero);
                Vector2 vector4 = projectile.velocity.SafeNormalize(Vector2.UnitY);

                if ((player.itemAnimation == 2 || player.itemAnimation == 6 || player.itemAnimation == 10) && projectile.owner == Main.myPlayer)
                {
                    Vector2 velocity = vector4 + Main.rand.NextVector2Square(-0.2f, 0.2f);
                    velocity *= 12f;

                    switch (player.itemAnimation)
                    {
                        case 2:
                            velocity = vector4.RotatedBy(0.38397246599197388);
                            break;
                        case 6:
                            velocity = vector4.RotatedBy(-0.38397246599197388);
                            break;
                        case 10:
                            velocity = vector4.RotatedBy(0.0);
                            break;
                    }

                    velocity *= 10f + Main.rand.Next(4);
                    Main.NewText("Peaked!");
                }
            }

            if (player.itemAnimation == 2)
            {
                projectile.Kill();
                player.reuseDelay = 2;
            }
        }
    }
}