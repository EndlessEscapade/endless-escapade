using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Melee.Boomerangs
{
    public class FeatheredChakramProjectileAlt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Feathered Chakram");
        }

        public override void SetDefaults()
        {
            projectile.width = 44;
            projectile.height = 60;
            projectile.aiStyle = -1;
            projectile.melee = true;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.extraUpdates = 2;
            projectile.tileCollide = false;
        }

        private Vector2 GoTo;

        private NPC HomeOnTarget()
        {
            const bool homingCanAimAtWetEnemies = true;
            const float homingMaximumRangeInPixels = 500;

            int selectedTarget = -1;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC target = Main.npc[i];
                if (target.active && (!target.wet || homingCanAimAtWetEnemies) && target.type != NPCID.TargetDummy)
                {
                    float distance = projectile.Distance(target.Center);
                    if (distance <= homingMaximumRangeInPixels &&
                        (
                            selectedTarget == -1 || //there is no selected target
                            projectile.Distance(Main.npc[selectedTarget].Center) > distance)
                    )
                    {
                        selectedTarget = i;
                    }
                }
            }
            if (selectedTarget == -1)
            {
                return null;
            }

            return Main.npc[selectedTarget];
        }

        private NPC npc;
        private readonly int dist = 300;
        private Color LerpColour = Color.Red;

        public override void AI()
        {
            alphaCounter += 0.04f;
            npc = HomeOnTarget();
            projectile.rotation += projectile.velocity.X / 50f;
            Vector2[] suitablePosses = { new Vector2(dist, -dist), new Vector2(-dist, -dist), new Vector2(-dist, dist), new Vector2(dist, dist) };
            projectile.ai[0]++;
            projectile.velocity *= 0.98f;
            if (npc != null)
            {
                LerpColour.R += (byte)((Color.OrangeRed.R - LerpColour.R) / 32f);
                LerpColour.G += (byte)((Color.OrangeRed.G - LerpColour.G) / 32f);
                LerpColour.B += (byte)((Color.OrangeRed.B - LerpColour.B) / 32f);
            }
            else
            {
                LerpColour.R += (byte)((Color.Yellow.R - LerpColour.R) / 32f);
                LerpColour.G += (byte)((Color.Yellow.G - LerpColour.G) / 32f);
                LerpColour.B += (byte)((Color.Yellow.B - LerpColour.B) / 32f);
            }
            projectile.ai[1] = (float)Math.Sin(projectile.ai[0] * 0.03f) * 0.7f;
            Player player = Main.player[projectile.owner];
            if (player.controlUseItem && projectile.ai[0] <= 3 || projectile.ai[0] % 50 == 0)
            {
                GoTo = Main.MouseWorld + suitablePosses[Helpers.FillPseudoRandomUniform<int>(4)[Main.rand.Next(0, 4)]] * 0.6f;
            }
            else
            {
                if (npc != null)
                {
                    projectile.velocity += (GoTo - projectile.Center) / 128f - projectile.velocity * 0.1f;
                }
                else
                {
                    projectile.velocity += (Main.MouseWorld - projectile.Center) / 500f - projectile.velocity * 0.05f;
                }

                projectile.velocity += new Vector2((float)Math.Sin(projectile.ai[0] * 0.05f) * 0.3f, (float)Math.Cos(projectile.ai[0] * 0.02f) * 0.3f);
                if (projectile.ai[0] % (npc == null ? 100 : 20) == 0)
                {
                    for (int i = 0; i < Main.projectile.Length - 1; i++)
                    {
                        if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<FeatheredChakramProjectileAlt>())
                        {
                            if (Vector2.DistanceSquared(Main.projectile[i].Center, projectile.Center) < 600 * 600)
                            {
                                for (float j = 0; j <= 1; j += 0.02f)
                                {
                                    Vector2 Lerped = projectile.Center + (Main.projectile[i].Center - projectile.Center) * j + new Vector2((float)Math.Sin(j * 20) * 10, (float)Math.Cos(j * 20) * 10);
                                    Dust dust = Dust.NewDustPerfect(Lerped, DustID.Fire, Vector2.Zero);
                                    dust.fadeIn = 1f;
                                    dust.noGravity = true;
                                    dust.color = npc == null ? Color.Green : default;
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < Main.projectile.Length - 1; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].type == ModContent.ProjectileType<FCHandler>())
                {
                    FCHandler Handler = Main.projectile[i].modProjectile as FCHandler;
                    for (int j = 0; j < 4; j++)
                    {
                        Handler.projectileIndex[j] = -1;
                    }
                }
            }
            for (var a = 0; a < 50; a++)
            {
                Vector2 vector = new Vector2(0, 20).RotatedBy(Math.PI * 0.04) * a;
                int index = Dust.NewDust(projectile.Center, 1, 1, DustID.Fire, vector.X, vector.Y, 0, default, 1f);
                Main.dust[index].velocity *= 1.1f;
                Main.dust[index].noGravity = true;
            }
        }

        private float alphaCounter = 0;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //    Main.spriteBatch.Draw(TextureCache.GradientEffect, projectile.Center - Main.screenPosition, new Rectangle(0, 0, ModContent.GetInstance<EEMod>().GetTexture("Masks/Extra_49").Width, ModContent.GetInstance<EEMod>().GetTexture("Masks/Extra_49").Height), LerpColour * 0.4f, projectile.rotation, new Rectangle(0, 0, TextureCache.GradientEffect.Width, ModContent.GetInstance<EEMod>().GetTexture("Masks/Extra_49").Height).Size() / 2, projectile.ai[1] * 0.5f, SpriteEffects.None, 0);
            //  AfterImage.DrawAfterimage(spriteBatch, Main.projectileTexture[projectile.type], 0, projectile, 1.5f, 1f, 6, false, 0f, 0f, new Color(lightColor.R, lightColor.G, lightColor.B, 150));
            float sineAdd = (float)Math.Sin(alphaCounter) + 3;
            Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().GetTexture("Masks/Extra_49"), projectile.Center - Main.screenPosition, null, new Color(LerpColour.R, LerpColour.G, LerpColour.B, 0), 0f, new Vector2(50, 50), Math.Abs(0.33f * (sineAdd + 1)) * projectile.ai[1], SpriteEffects.None, 0f);
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 180);
        }
    }
}