using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using EEMod.Buffs.Buffs;
using Microsoft.Xna.Framework.Graphics;

namespace EEMod.Projectiles.Summons
{
    public class AkumoMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Minikumo");
            Main.projFrames[projectile.type] = 4;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = false;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 56;
            projectile.height = 56;
            projectile.penetrate = -1;
            projectile.minion = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.minionSlots = 1;
            projectile.friendly = true;
            projectile.damage = 50;
            projectile.knockBack = 4f;
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

            //NPC target = Main.npc[Helpers.GetNearestNPC(projectile.Center)];

            int npcs = Main.npc.Length - 1;
            NPC target = Main.npc[0];
            for (int i = 0; i < npcs; i++)
            {
                if (Vector2.Distance(Main.npc[i].Center, projectile.Center) < Vector2.Distance(target.Center, projectile.Center))
                {
                    target = Main.npc[i];
                }
            }

            projectile.ai[1]++;
            if (Vector2.Distance(target.Center, projectile.Center) <= 2000)
                projectile.ai[0] = 0;
            else
            {
                if (projectile.ai[1] >= 300)
                {
                    projectile.ai[0] = 1; //projectile.ai[0] = Main.rand.Next(1, 4);
                    projectile.ai[1] = 0;
                    projectileAiCont[1] = 0;
                }
            }

            if(projectile.ai[0] == 0)
            {
                frameSpeed = 5;
                projectileAiCont[0] = 0;
            }
            if (projectile.ai[0] == 1)
            {
                #region Fire attack


                frameSpeed = 3;
                if(projectileAiCont[0] < 5)
                    projectileAiCont[0] += 0.1f;
                minionGlow = Color.Orange;
                if (Vector2.Distance(target.Center, projectile.Center) <= 320)
                    target.AddBuff(BuffID.OnFire, 600);

                projectileAiCont[2] += 0.02f;
                float radius = 40;
                if(projectileAiCont[1] < 24)
                    projectileAiCont[1]++;
                for (int j = 0; j < projectileAiCont[1]; j++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 position = projectile.Center + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(360f / 6 * i) + (j*2) - projectileAiCont[2]) * radius;
                        Dust dust = Dust.NewDustPerfect(position, DustID.Fire);
                        dust.noGravity = true;
                    }
                    radius += 4;
                }
                Main.NewText(projectileAiCont[0]);


                #endregion
            }
            if (projectile.ai[0] == 2)
            {
                frameSpeed = 5;
                frameSpeed = 3;
                projectileAiCont[0] += 0.1f;
                minionGlow = Color.OrangeRed;
                if (Vector2.Distance(target.Center, projectile.Center) <= 320)
                    target.AddBuff(BuffID.OnFire, 600);
            }
            if (projectile.ai[0] == 3)
            {
                frameSpeed = 5;
                frameSpeed = 3;
                projectileAiCont[0] += 0.1f;
                minionGlow = Color.Red;
                if (Vector2.Distance(target.Center, projectile.Center) <= 320)
                    target.AddBuff(BuffID.OnFire, 600);
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float funnySin = (float)Math.Sin(projectileAiCont[0]);
            Texture2D texture = mod.GetTexture("Projectiles/Summons/AkumoMinionGlow");
            Main.spriteBatch.Draw(texture, projectile.position - Main.screenPosition + new Vector2(funnySin * 10, 0), texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame), minionGlow * funnySin * 0.5f);
            Main.spriteBatch.Draw(texture, projectile.position - Main.screenPosition + new Vector2(0, funnySin * 10), texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame), minionGlow * funnySin * 0.5f);
            Main.spriteBatch.Draw(texture, projectile.position - Main.screenPosition + new Vector2(funnySin * -10, 0), texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame), minionGlow * funnySin * 0.5f);
            Main.spriteBatch.Draw(texture, projectile.position - Main.screenPosition + new Vector2(0, funnySin * -10), texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame), minionGlow * funnySin * 0.5f);
            Main.spriteBatch.Draw(texture, projectile.position - Main.screenPosition, texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame), minionGlow * funnySin * 0.75f);
        }

        public override void Kill(int timeleft)
        {
            if (Main.LocalPlayer.HasBuff(ModContent.BuffType<AkumoBuff>()))
                Main.LocalPlayer.ClearBuff(ModContent.BuffType<AkumoBuff>());
        }
    }
}
