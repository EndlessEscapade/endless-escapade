using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EEMod.NPCs
{
    public class CrystalSword : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.hostile = false;
            projectile.friendly = true;  //Tells the game whether it is friendly to players/friendly npcs or not
            projectile.ignoreWater = true;  //Tells the game whether or not projectile will be affected by water
            projectile.ranged = true;  //Tells the game whether it is a ranged projectile or not
            projectile.penetrate = 1; //Tells the game how many enemies it can hit before being destroyed, -1 infinity
            projectile.timeLeft = 125;  //The amount of time the projectile is alive for
            projectile.tileCollide = true;
        }

        public override void AI()
        {
          float magVel = (float)Math.Sqrt(projectile.velocity.X * projectile.velocity.X + projectile.velocity.Y * projectile.velocity.Y);
          float num166 = projectile.localAI[0];
          if (num166 == 0f)
          {
              projectile.localAI[0] = magVel;
              num166 = magVel;
          }
          float projPosX = projectile.position.X;
          float projPosY = projectile.position.Y;
          float distFromNpc = 300f;
          bool canHome = false;
          int indexOfNpc = 0;
          if (projectile.ai[1] == 0f)
          {
              for (int indexOfNpcChecker = 0; indexOfNpcChecker < 200; indexOfNpcChecker++)
              {
                  if (!Main.npc[indexOfNpcChecker].CanBeChasedBy(projectile) || projectile.ai[1] != 0f && projectile.ai[1] != indexOfNpcChecker + 1)
                  {
                      continue;
                  }

                  float num172 = Main.npc[indexOfNpcChecker].position.X + Main.npc[indexOfNpcChecker].width / 2f;
                  float num173 = Main.npc[indexOfNpcChecker].position.Y + Main.npc[indexOfNpcChecker].height / 2f;
                  float num174 = Math.Abs(projectile.position.X + projectile.width / 2f - num172) + Math.Abs(projectile.position.Y + projectile.height / 2f - num173);
                  if (!(num174 < distFromNpc) || !Collision.CanHit(new Vector2(projectile.position.X + projectile.width / 2f, projectile.position.Y + projectile.height / 2f), 1, 1,
                          Main.npc[indexOfNpcChecker].position, Main.npc[indexOfNpcChecker].width, Main.npc[indexOfNpcChecker].height))
                  {
                      continue;
                  }

                  distFromNpc = num174;
                  projPosX = num172;
                  projPosY = num173;
                  canHome = true;
                  indexOfNpc = indexOfNpcChecker;
              }

              if (canHome)
              {
                  projectile.ai[1] = indexOfNpc + 1;
              }

              canHome = false;
          }

          if (projectile.ai[1] > 0f)
          {
              int indexOfNewNpc = (int)(projectile.ai[1] - 1f);
              if (Main.npc[indexOfNewNpc].active && Main.npc[indexOfNewNpc].CanBeChasedBy(projectile, true) && !Main.npc[indexOfNewNpc].dontTakeDamage)
              {
                  float npcPosHomeX = Main.npc[indexOfNewNpc].position.X + Main.npc[indexOfNewNpc].width / 2;
                  float npcPosHomeY = Main.npc[indexOfNewNpc].position.Y + Main.npc[indexOfNewNpc].height / 2;
                  float npcDistHome = Math.Abs(projectile.position.X + projectile.width / 2 - npcPosHomeX) + Math.Abs(projectile.position.Y + projectile.height / 2 - npcPosHomeY);
                  if (npcDistHome < 1000f)
                  {
                      canHome = true;
                      projPosX = Main.npc[indexOfNewNpc].position.X + Main.npc[indexOfNewNpc].width / 2;
                      projPosY = Main.npc[indexOfNewNpc].position.Y + Main.npc[indexOfNewNpc].height / 2;
                  }
              }
              else
              {
                  projectile.ai[1] = 0f;
              }
          }

          if (!projectile.friendly)
          {
              canHome = false;
          }

          if (canHome)
          {
              float num179 = num166;
              Vector2 vector19 = new Vector2(projectile.position.X + projectile.width * 0.5f, projectile.position.Y + projectile.height * 0.5f);
              float num180 = projPosX - vector19.X;
              float num181 = projPosY - vector19.Y;
              float num182 = (float)Math.Sqrt(num180 * num180 + num181 * num181);
              num182 = num179 / num182;
              num180 *= num182;
              num181 *= num182;
              int num183 = 8;
              projectile.velocity.X = (projectile.velocity.X * (num183 - 1) + num180) / num183;
              projectile.velocity.Y = (projectile.velocity.Y * (num183 - 1) + num181) / num183;
          }
            Lighting.AddLight(projectile.Center, ((255 - projectile.alpha) * 0.15f) / 255f, ((255 - projectile.alpha) * 0.45f) / 255f, ((255 - projectile.alpha) * 0.05f) / 255f);   //this is the light colors
            if (projectile.timeLeft > 125)
            {
                projectile.timeLeft = 125;
            }
            if (projectile.ai[0] > 1f)  //this defines where the flames starts
            {

                for(int i = 0; i<15; i++)    //this defines how many dust to spawn
                {
                    int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 64, projectile.velocity.X * 0.3f, projectile.velocity.Y * 0.3f, 0, new Color(255, 255, 153), 1);   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                    Main.dust[dust].noGravity = true;
                }
            }
            else
            {
                projectile.ai[0] += 1f;
            }
            return;

        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 4; i++)
            {
                int projHolder = Main.rand.Next(0, 1);
                float speedX = -(projectile.velocity.X * Main.rand.NextFloat(-.1f, .8f) + Main.rand.NextFloat(-.4f, 2f));
                float speedY = -(projectile.velocity.Y * Main.rand.Next(0, 30) * 0.01f + Main.rand.NextFloat(-12f, 12.1f));
                if (projHolder == 0)
                    Projectile.NewProjectile(projectile.Center.X + speedX, projectile.Center.Y + speedY, speedX*1.3f, speedY, mod.ProjectileType("CrystalKill"), (int)(projectile.damage * 0.7), 0f, projectile.owner, 0f, 0f);
                if (projHolder == 1)
                    Projectile.NewProjectile(projectile.Center.X + speedX, projectile.Center.Y + speedY, speedX*1.3f, speedY, mod.ProjectileType("CrystalKill"), (int)(projectile.damage * 0.7), 0f, projectile.owner, 0f, 0f);
                Main.PlaySound(SoundID.Item27, projectile.position);
            }
            for (var i = 0; i < 20; i++)
            {
                int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 64, Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-1f, 1f), 6, new Color(255, 255, 153, 255), 2f);
                Main.dust[num].noGravity = true;
                Main.dust[num].velocity *= 1.2f;
                Main.dust[num].noLight = false;
            }
        }


        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            player.AddBuff(BuffID.OnFire, 120);
            player.AddBuff(BuffID.Frostburn, 120);
            player.AddBuff(BuffID.CursedInferno, 120);				//this make so when the projectile/flame hit a npc, gives it the buff  onfire , 80 = 3 seconds
        }
        private void LookInDirectionP(Vector2 look)
        {
            float angle = 0.5f * (float)Math.PI;
            if (look.X != 0f)
            {
                angle = (float)Math.Atan(look.Y / look.X);
            }
            else if (look.Y < 0f)
            {
                angle += (float)Math.PI;
            }
            if (look.X < 0f)
            {
                angle += (float)Math.PI;
            }
            projectile.rotation = angle;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.Kill();
            return false;
        }

    }
}
