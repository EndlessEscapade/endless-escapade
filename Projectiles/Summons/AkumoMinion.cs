using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using EEMod.Buffs.Buffs;
using Microsoft.Xna.Framework.Graphics;
using EEMod.Projectiles.Mage;
using EEMod.Extensions; 

namespace EEMod.Projectiles.Summons
{
    public class AkumoMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Minikumo");
            Main.projFrames[projectile.type] = 4;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = false;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 56;
            projectile.height = 58;
            projectile.penetrate = -1;
            projectile.minion = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.minionSlots = 1;
            projectile.friendly = true;
            projectile.damage = 50;
            projectile.knockBack = 4f;
            projectile.hostile = false;
        }

        Color minionGlow;
        float[] projectileAiCont = new float[6];
        int frameSpeed = 4;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (!player.HasBuff(ModContent.BuffType<AkumoBuff>()))
                projectile.Kill();

            //Animation
            projectile.frameCounter++;
            if (projectile.frameCounter >= frameSpeed)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }

            int npcs = Main.npc.Length - 1;
            NPC target = null;
            int closestDist = 10000000;
            for (int i = 0; i < npcs; i++)
            {
                if (Vector2.Distance(Main.npc[i].Center, projectile.Center) < closestDist && Main.npc[i].active)
                {
                    target = Main.npc[i];
                    closestDist = (int)Vector2.Distance(Main.npc[i].Center, projectile.Center);
                }
            }


            projectile.ai[1]++;
            if (target == null || Vector2.Distance(target.Center, projectile.Center) >= 12800)
                projectile.ai[0] = 0;
            else
            {
                if (projectile.ai[1] >= 180)
                {
                    projectile.ai[0] = Main.rand.Next(1, 4);
                    projectile.ai[1] = 0;
                    projectileAiCont[1] = projectile.ai[0] == 3 || projectile.ai[0] == 2 ? 45 : 0;
                    projectileAiCont[0] = 0;
                    projectileAiCont[2] = 0;
                }
            }

            projectile.spriteDirection = projectile.velocity.X > 0 ? -1 : 1;
            projectile.rotation = projectile.velocity.X / 12;


            #region Attacks


            if (projectile.ai[0] == 0)
            {
                frameSpeed = 6;
                if (projectileAiCont[0] > 0)
                    projectileAiCont[0] -= 0.1f;

                if (Vector2.Distance(projectile.Center, Main.player[projectile.owner].Center) >= 1600)
                {
                    minionGlow = Color.White;
                    projectileAiCont[0] = 2.5f;
                    projectile.velocity = Vector2.Zero;
                    projectile.Center = Main.player[projectile.owner].Center + new Vector2(Main.rand.Next(-80, 80), Main.rand.Next(-80, 80));
                }

                projectile.velocity -= Vector2.Normalize(projectile.Center - Main.player[projectile.owner].Center)/8;
                if((projectile.velocity.X + projectile.velocity.Y)/2 < 4)
                    projectile.velocity *= 1.005f;
                else
                    projectile.velocity *= 0.97f;
            }
            if (projectile.ai[0] == 1)
            {
                #region Fire attack


                projectile.velocity += -Vector2.Normalize(projectile.Center - Vector2.Add(Main.player[projectile.owner].Center, target.Center)/2) - projectile.velocity * 0.03f;

                frameSpeed = 4;
                if(projectileAiCont[0] < 5)
                    projectileAiCont[0] += 0.1f;
                minionGlow = Color.Orange;

                projectileAiCont[2] += 0.02f;
                float radius = 40;
                if (projectileAiCont[1] < 24)
                {
                    projectileAiCont[1]++;
                    for (int k = 0; k < Main.npc.Length - 1; k++)
                    {
                        NPC npc = Main.npc[k];
                        if (Vector2.Distance(npc.Center, projectile.Center) <= 320)
                            target.AddBuff(BuffID.OnFire, 600);
                    }
                }

                for (int j = 0; j < projectileAiCont[1]; j++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 position = projectile.Center + new Vector2(0, 4) + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / 6 * i) + (j*2) - projectileAiCont[2]) * radius;
                        Dust dust = Dust.NewDustPerfect(position, DustID.Fire);
                        dust.noGravity = true;
                    }
                    radius += 4;
                }


                #endregion
            }

            if (projectile.ai[0] == 2)
            {
                #region Feather attack


                projectile.velocity *= 0.98f;

                frameSpeed = 5;
                if (projectileAiCont[0] < 5)
                    projectileAiCont[0] += 0.05f;
                minionGlow = Color.OrangeRed;

                projectile.velocity = Vector2.Normalize(projectile.Center - new Vector2(target.Center.X, target.position.Y - 120)) * -2 - projectile.velocity * 0.03f;
                if(Vector2.Distance(projectile.Center, new Vector2(target.Center.X, target.position.Y - 120)) <= 16)
                    projectile.spriteDirection = 1;
                projectile.velocity *= 1.005f;
                projectileAiCont[1]++;
                if (projectileAiCont[1] >= 45)
                {
                    for (int l = 0; l < 5; l++)
                    {
                        Vector2 newPos = new Vector2(target.Center.X + Main.rand.Next(-80, 81), target.position.Y - 80);
                        Vector2 newVel = Vector2.Normalize(newPos - target.Center) * -16;
                        Projectile.NewProjectile(newPos, newVel, ModContent.ProjectileType<AkumoMinionProjectile>(), 50, 2f, player.whoAmI);
                    }
                    projectileAiCont[1] = 0;
                }


                #endregion
            }

            if (projectile.ai[0] == 3)
            {
                #region Dash attack


                frameSpeed = 3;
                if (projectileAiCont[0] < 5)
                    projectileAiCont[0] += 0.15f;
                else
                {
                    projectileAiCont[1]++;
                    Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.Fire);
                    dust.noGravity = true;
                    if (projectileAiCont[1] >= 45)
                    {
                        projectile.velocity = Vector2.Normalize(projectile.Center - target.Center) * -8;
                        projectileAiCont[1] = 0;
                        projectileAiCont[0] = 0;
                    }
                    else
                        projectile.velocity *= 1.02f;
                }
                minionGlow = Color.Red;


                #endregion
            }


            #endregion
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float funnySin = (float)Math.Sin(projectileAiCont[0]);
            Texture2D texture = mod.GetTexture("Projectiles/Summons/AkumoMinionGlow");
            Vector2 funny = projectile.Center.ForDraw();
            Main.spriteBatch.Draw(texture, new Rectangle((int)(funny + new Vector2(funnySin * 10, 0)).X, (int)(funny + new Vector2(funnySin * 10, 0)).Y, projectile.width, projectile.height), texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame), minionGlow * funnySin * 0.5f, projectile.rotation, new Vector2(projectile.width/2, projectile.height/2), projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : default, default);
            Main.spriteBatch.Draw(texture, new Rectangle((int)(funny + new Vector2(funnySin * 0, 10)).X, (int)(funny + new Vector2(funnySin * 10, 0)).Y, projectile.width, projectile.height), texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame), minionGlow * funnySin * 0.5f, projectile.rotation, new Vector2(projectile.width / 2, projectile.height / 2), projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : default, default);
            Main.spriteBatch.Draw(texture, new Rectangle((int)(funny + new Vector2(funnySin * -10, 0)).X, (int)(funny + new Vector2(funnySin * 10, 0)).Y, projectile.width, projectile.height), texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame), minionGlow * funnySin * 0.5f, projectile.rotation, new Vector2(projectile.width / 2, projectile.height / 2), projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : default, default);
            Main.spriteBatch.Draw(texture, new Rectangle((int)(funny + new Vector2(funnySin * 0, -10)).X, (int)(funny + new Vector2(funnySin * 10, 0)).Y, projectile.width, projectile.height), texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame), minionGlow * funnySin * 0.5f, projectile.rotation, new Vector2(projectile.width / 2, projectile.height / 2), projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : default, default);
            Main.spriteBatch.Draw(texture, new Rectangle((int)funny.X, (int)funny.Y, projectile.width, projectile.height), texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame), minionGlow * funnySin * 0.75f, projectile.rotation, new Vector2(projectile.width / 2, projectile.height / 2), projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : default, default);
        }

        public override void Kill(int timeleft)
        {
            if (Main.LocalPlayer.HasBuff(ModContent.BuffType<AkumoBuff>()))
                Main.LocalPlayer.ClearBuff(ModContent.BuffType<AkumoBuff>());
        }
    }
}