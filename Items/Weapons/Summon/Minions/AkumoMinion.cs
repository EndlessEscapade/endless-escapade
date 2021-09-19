using EEMod.Buffs.Buffs;
using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.Items.Weapons.Summon.Minions
{
    public class AkumoMinion : EEProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Minikumo");
            Main.projFrames[Projectile.type] = 4;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = false;
            ProjectileID.Sets.CountsAsHoming[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 56;
            Projectile.height = 58;
            Projectile.penetrate = -1;
            Projectile.minion = true;
            // Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 1;
            Projectile.friendly = true;
            Projectile.damage = 50;
            Projectile.knockBack = 4f;
            // Projectile.hostile = false;
        }

        private Color minionGlow;
        private readonly float[] projectileAiCont = new float[6];
        private readonly int[] reinforcements = new int[3];
        private int frameSpeed = 4;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            /*            if (!player.HasBuff(ModContent.BuffType<AkumoBuff>()))
                            projectile.Kill();*/

            //Animation
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= frameSpeed)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }

            int npcs = Main.npc.Length - 1;
            NPC target = null;
            int closestDist = 10000000;
            for (int i = 0; i < npcs; i++)
            {
                if (Vector2.Distance(Main.npc[i].Center, Projectile.Center) < closestDist && Main.npc[i].active && !Main.npc[i].friendly)
                {
                    target = Main.npc[i];
                    closestDist = (int)Vector2.Distance(Main.npc[i].Center, Projectile.Center);
                }
            }
            if (target != null)
            {
                Projectile.ai[1] = Main.npc[target.whoAmI].whoAmI;
            }

            projectileAiCont[4]++;
            if (target == null || Vector2.Distance(target.Center, Projectile.Center) >= 12800)
            {
                Projectile.ai[0] = 0;
            }
            else
            {
                if (projectileAiCont[4] >= 300)
                {
                    Projectile.ai[0] = Main.rand.Next(2, 4);
                    projectileAiCont[4] = 0;
                    projectileAiCont[1] = Projectile.ai[0] == 3 || Projectile.ai[0] == 2 ? 45 : 0;
                    projectileAiCont[0] = 0;
                    projectileAiCont[2] = 0;
                    projectileAiCont[3] = 1;
                }
            }

            Projectile.spriteDirection = Projectile.velocity.X > 0 ? -1 : 1;
            Projectile.rotation = Projectile.velocity.X / 12;

            #region Attacks

            if (Main.player[Projectile.owner].statLife <= Main.player[Projectile.owner].statLifeMax && reinforcements[2] == default && target != null)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (reinforcements[i] == default)
                    {
                        Main.NewText(i + "AE");
                        reinforcements[i] = Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_ProjectileParent(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<AkumoReinforcement>(), 50, 2f, Owner: Main.myPlayer, ai0: MathHelper.TwoPi / 3 * i, ai1: target.whoAmI);
                    }
                }
            }
            if (Projectile.ai[0] == 0)
            {
                frameSpeed = 6;
                if (projectileAiCont[0] > 0)
                {
                    projectileAiCont[0] -= 0.1f;
                }

                Player owner = Main.player[Projectile.owner];
                if (!Projectile.WithinRange(owner.MountedCenter, 1600)) //(Vector2.Distance(projectile.Center, Main.player[projectile.owner].Center) >= 1600)
                {
                    minionGlow = Color.White;
                    projectileAiCont[0] = 2.5f;
                    Projectile.velocity = Vector2.Zero;
                    Projectile.Center = owner.Center + new Vector2(Main.rand.Next(-80, 80), Main.rand.Next(-80, 80));
                }

                Projectile.velocity -= Vector2.Normalize(Projectile.Center - Main.player[Projectile.owner].Center) / 8;
                if ((Projectile.velocity.X + Projectile.velocity.Y) / 2 < 4)
                {
                    Projectile.velocity *= 1.001f;
                }
                else
                {
                    Projectile.velocity *= 0.96f;
                }
            }
            /*if (projectile.ai[0] == 1)
            {
                #region Fire attack

                projectile.velocity = -Vector2.Normalize(projectile.Center - new Vector2(target.Center.X + (float)Math.Sin(projectileAiCont[2]) * 128, target.position.Y - 120)) * 2;

                if (Vector2.Distance(projectile.Center, new Vector2(target.Center.X + (float)Math.Sin(projectileAiCont[2]) * 128, target.position.Y - 120)) <= 32)
                {
                    projectile.spriteDirection = target.Center.X >= projectile.Center.X ? -1 : 1;
                    projectile.rotation = 0;
                }

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
                        if (Vector2.Distance(npc.Center, projectile.Center) <= 640)
                            target.AddBuff(BuffID.OnFire, 600);
                    }
                }

                for (int j = 0; j < projectileAiCont[1]; j++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 position = projectile.Center + new Vector2(0, 4) + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / 6 * i) + (j*2) - projectileAiCont[2]) * radius;
                        Dust dust = Dust.NewDustPerfect(position, DustID.Lava);
                        dust.noGravity = true;
                    }
                    radius += 4;
                }

                #endregion Fire attack
            }*/

            if (Projectile.ai[0] == 2)
            {
                #region Feather attack

                Projectile.velocity *= 0.98f;

                frameSpeed = 5;
                if (projectileAiCont[0] < 5)
                {
                    projectileAiCont[0] += 0.15f;
                }

                minionGlow = Color.OrangeRed;

                projectileAiCont[2] += 0.02f;
                Projectile.velocity = Vector2.Normalize(Projectile.Center - new Vector2(target.Center.X + (float)Math.Sin(projectileAiCont[2]) * 128, target.position.Y - 120)) * -2 - Projectile.velocity * 0.03f;
                if (Vector2.DistanceSquared(Projectile.Center, new Vector2(target.Center.X + (float)Math.Sin(projectileAiCont[2]) * 128, target.position.Y - 120)) <= 4 * 4)
                {
                    Projectile.spriteDirection = target.Center.X >= Projectile.Center.X ? -1 : 1;
                    Projectile.rotation = 0;
                }
                Projectile.velocity *= 1.005f;
                projectileAiCont[1]++;
                if (projectileAiCont[1] >= 45)
                {
                    for (int l = 0; l < 5; l++)
                    {
                        Vector2 newPos = new Vector2(target.Center.X + Main.rand.Next(-80, 81), target.position.Y - 80);
                        Vector2 newVel = Vector2.Normalize(newPos - target.Center) * -16;
                        Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_ProjectileParent(Projectile), newPos, newVel, ModContent.ProjectileType<AkumoMinionProjectile>(), 50, 2f, player.whoAmI);
                    }
                    projectileAiCont[0] = 0;
                    projectileAiCont[1] = 0;
                }

                #endregion Feather attack
            }

            if (Projectile.ai[0] == 3)
            {
                #region Dash attack

                frameSpeed = 3;
                if (projectileAiCont[0] < 5)
                {
                    projectileAiCont[0] += 0.15f;
                }
                else
                {
                    projectileAiCont[1]++;
                    Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Lava);
                    dust.noGravity = true;
                    if (projectileAiCont[1] >= 20 / projectileAiCont[3])
                    {
                        Projectile.velocity = Vector2.Normalize(Projectile.Center - target.Center) * -8 * projectileAiCont[3];
                        projectileAiCont[1] = 0;
                        projectileAiCont[0] = 0;
                    }
                    else
                        if (projectileAiCont[3] < 2)
                    {
                        projectileAiCont[3] *= 1.005f;
                    }
                }
                minionGlow = Color.Red;

                #endregion Dash attack
            }

            #endregion Attacks
        }

        public override void PostDraw(Color lightColor)
        {
            float funnySin = (float)Math.Sin(projectileAiCont[0]);
            Texture2D texture = ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Projectiles/Summons/AkumoMinionGlow").Value;
            Vector2 funny = Projectile.Center.ForDraw();
            Main.spriteBatch.Draw(texture, new Rectangle((int)(funny + new Vector2(funnySin * 10, 0)).X, (int)(funny + new Vector2(funnySin * 10, 0)).Y, Projectile.width, Projectile.height), texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame), minionGlow * funnySin * 0.5f, Projectile.rotation, Projectile.Center, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : default, default);
            Main.spriteBatch.Draw(texture, new Rectangle((int)(funny + new Vector2(funnySin * 0, 10)).X, (int)(funny + new Vector2(funnySin * 10, 0)).Y, Projectile.width, Projectile.height), texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame), minionGlow * funnySin * 0.5f, Projectile.rotation, Projectile.Center, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : default, default);
            Main.spriteBatch.Draw(texture, new Rectangle((int)(funny + new Vector2(funnySin * -10, 0)).X, (int)(funny + new Vector2(funnySin * 10, 0)).Y, Projectile.width, Projectile.height), texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame), minionGlow * funnySin * 0.5f, Projectile.rotation, Projectile.Center, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : default, default);
            Main.spriteBatch.Draw(texture, new Rectangle((int)(funny + new Vector2(funnySin * 0, -10)).X, (int)(funny + new Vector2(funnySin * 10, 0)).Y, Projectile.width, Projectile.height), texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame), minionGlow * funnySin * 0.5f, Projectile.rotation, Projectile.Center, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : default, default);
            Main.spriteBatch.Draw(texture, new Rectangle((int)funny.X, (int)funny.Y, Projectile.width, Projectile.height), texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame), minionGlow * funnySin * 0.75f, Projectile.rotation, Projectile.Center, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : default, default);
        }

        /*public override bool PreDraw(ref Color lightColor)
        {
            if (projectile.ai[0] == 3)
                AfterImage.DrawAfterimage(spriteBatch, TextureAssets.Projectile[projectile.type].Value, 0, projectile, 1.5f, 1f, 3, false, 0f, 0f, new Color(lightColor.R, lightColor.G, lightColor.B), overrideFrameCount: 4, overrideFrame: new Rectangle(0, projectile.frame * 56, 56, 58));
            return true;
            spriteBatch.Draw(texture, projectile.position - Main.screenPosition + new Vector2(funnySin * 10, 0), texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame), minionGlow * funnySin * 0.5f);
            spriteBatch.Draw(texture, projectile.position - Main.screenPosition + new Vector2(0, funnySin * 10), texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame), minionGlow * funnySin * 0.5f);
            spriteBatch.Draw(texture, projectile.position - Main.screenPosition + new Vector2(funnySin * -10, 0), texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame), minionGlow * funnySin * 0.5f);
            spriteBatch.Draw(texture, projectile.position - Main.screenPosition + new Vector2(0, funnySin * -10), texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame), minionGlow * funnySin * 0.5f);
            spriteBatch.Draw(texture, projectile.position - Main.screenPosition, texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame), minionGlow * funnySin * 0.75f);
        }*/

        public override void Kill(int timeleft)
        {
            if (Main.LocalPlayer.HasBuff(ModContent.BuffType<AkumoBuff>()))
            {
                Main.LocalPlayer.ClearBuff(ModContent.BuffType<AkumoBuff>());
            }
        }
    }
}