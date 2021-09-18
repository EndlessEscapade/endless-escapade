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
    public class StormspearProj : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stormspear");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.hide = true;
            // Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;
            Projectile.ownerHitCheck = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter);

            Projectile.direction = player.direction;
            player.heldProj = Projectile.whoAmI;
            Projectile.Center = vector;

            if (player.dead)
            {
                Projectile.Kill();
                return;
            }

            if (!player.frozen)
            {
                Projectile.spriteDirection = (Projectile.direction = player.direction);
                Projectile.alpha -= 127;

                if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }

                float num7 = (float)player.itemAnimation / (float)player.itemAnimationMax;
                float num8 = 1f - num7;
                float num9 = Projectile.velocity.ToRotation();
                float num10 = Projectile.velocity.Length();
                float num11 = 22f;

                Vector2 spinningpoint2 = new Vector2(1f, 0f).RotatedBy((float)Math.PI + num8 * ((float)Math.PI * 2f)) * new Vector2(num10, Projectile.ai[0]);
                Projectile.position += spinningpoint2.RotatedBy(num9) + new Vector2(num10 + num11, 0f).RotatedBy(num9);
                Vector2 value2 = vector + spinningpoint2.RotatedBy(num9) + new Vector2(num10 + num11 + 40f, 0f).RotatedBy(num9);
                Projectile.rotation = ((value2 - vector).SafeNormalize(Vector2.UnitX).ToRotation() + (float)Math.PI / 4f * (float)player.direction) - MathHelper.PiOver4 + MathHelper.Pi;

                if (Projectile.spriteDirection == -1)
                {
                    //projectile.rotation += (float)Math.PI;
                }

                if (Projectile.spriteDirection == 1)
                {
                    Projectile.rotation -= MathHelper.PiOver2;
                }

                (Projectile.Center - vector).SafeNormalize(Vector2.Zero);
                (value2 - vector).SafeNormalize(Vector2.Zero);
                Vector2 vector4 = Projectile.velocity.SafeNormalize(Vector2.UnitY);

                if ((player.itemAnimation == 2 || player.itemAnimation == 6 || player.itemAnimation == 10) && Projectile.owner == Main.myPlayer)
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
                Projectile.Kill();
                player.reuseDelay = 2;
            }
        }
    }
}