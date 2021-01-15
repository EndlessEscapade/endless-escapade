using EEMod.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Tiles.Foliage.Coral;
using EEMod.Projectiles.Enemy;
using EEMod.Prim;

namespace EEMod.NPCs.CoralReefs
{
    public class AquamarineSpire : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aquamarine Spire");
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.friendly = true;
            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;
            npc.lifeMax = 1000000;
            npc.width = 320;
            npc.height = 416;
            npc.noGravity = true;
            npc.lavaImmune = true;
            npc.noTileCollide = true;
            npc.dontTakeDamage = true;
            npc.damage = 0;
            npc.behindTiles = true;
        }

        public override bool CheckActive()
        {
            return false;
        }

        public bool awake = false;
        float HeartBeat;
        int blinkTime = 0;
        bool blinking = false;
        int playerHits = 0;
        private int chargeTime = 0;
        private int pseudoHealth = 20;
        private int strikeTime = 0;
        private Color strikeColor = Color.White;
        private float eyeAlpha = 1f;
        private Vector2 eyePos;
        private bool firstAwakening = true;

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Player target = Main.LocalPlayer;

            float timeBetween;
            float bigTimeBetween;
            if (awake)
            {
                if (!blinking && Main.rand.NextBool(240) && blinkTime <= 0)
                    blinking = true;
                if (!blinking && blinkTime > 0)
                    blinkTime--;
                if (blinkTime == 8)
                    blinking = false;
                if (blinking && blinkTime < 8)
                    blinkTime++;

                Color addColor = Color.White;
                switch (npc.ai[1])
                {
                    case 0: //Blue laser
                        addColor = Color.Blue;
                        break;
                    case 1: //Cyan laser
                        addColor = Color.Cyan;
                        break;
                    case 2: //Pink laser
                        addColor = Color.Magenta;
                        break;
                }
                Color color = Color.Lerp(Color.White, addColor, 5 / npc.ai[3]);

                eyePos = (Vector2.Normalize(target.Center - npc.Center) * 3) + npc.Center + new Vector2(-2, 2 + blinkTime) + (new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f)) * (npc.ai[3] / 20));

                Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().GetTexture("NPCs/CoralReefs/AquamarineSpireEye"), eyePos.ForDraw(), new Rectangle(0, blinkTime, 8, 8 - blinkTime), color, npc.rotation, new Vector2(4, 4), npc.scale, SpriteEffects.None, 0);

                timeBetween = 35;
                bigTimeBetween = 100;
            }
            else
            {
                if (npc.ai[0] < 300 * 60) //If recharging
                {
                    EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.18f));
                    Vector2 one = new Vector2(-8, Main.rand.Next(-8, 8)).RotatedBy(1.57f / 2f + HeartBeat / 60f);
                    Vector2 two = new Vector2(8, Main.rand.Next(-8, 8)).RotatedBy(1.57f / 2f + HeartBeat / 60f);
                    Vector2 three = new Vector2(Main.rand.Next(-8, 8), 8).RotatedBy(1.57f / 2f + HeartBeat / 60f);
                    Vector2 four = new Vector2(Main.rand.Next(-8, 8), -8).RotatedBy(1.57f / 2f + HeartBeat / 60f);
                    Vector2 offset = new Vector2(-3, (float)Math.Sin(Main.GameUpdateCount / 60f) + 2 + HeartBeat / 60f);
                    int scale = 4;
                    EEMod.Particles.Get("Main").SpawnParticles(npc.Center + one * scale + offset, -Vector2.Normalize(one) / 2f, ModContent.GetTexture("EEMod/Particles/Crystal"), 30, 1, Color.White, new SlowDown(0.95f), new AfterImageTrail(1f), new SetMask(Helpers.RadialMask, 0.6f));
                    EEMod.Particles.Get("Main").SpawnParticles(npc.Center + two * scale + offset, -Vector2.Normalize(two) / 2f, ModContent.GetTexture("EEMod/Particles/Crystal"), 30, 1, Color.White, new SlowDown(0.95f), new AfterImageTrail(1f), new SetMask(Helpers.RadialMask, 0.6f));
                    EEMod.Particles.Get("Main").SpawnParticles(npc.Center + three * scale + offset, -Vector2.Normalize(three) / 2f, ModContent.GetTexture("EEMod/Particles/Crystal"), 30, 1, Color.White, new SlowDown(0.95f), new AfterImageTrail(1f), new SetMask(Helpers.RadialMask, 0.6f));
                    EEMod.Particles.Get("Main").SpawnParticles(npc.Center + four * scale + offset, -Vector2.Normalize(four) / 2f, ModContent.GetTexture("EEMod/Particles/Crystal"), 30, 1, Color.White, new SlowDown(0.95f), new AfterImageTrail(1f), new SetMask(Helpers.RadialMask, 0.6f));

                    if (npc.ai[2] == 0)
                    {
                        if (eyeAlpha > 0 && playerHits == 0) eyeAlpha -= 0.02f;
                        if (eyeAlpha <= 0) npc.ai[2] = 1;
                    }
                    else
                    {
                        eyeAlpha = npc.ai[0] / (300 * 60);
                    }

                    blinkTime = 0;
                }
                else
                {
                    if (!blinking && Main.rand.NextBool(180) && blinkTime <= 0)
                        blinking = true;
                    if (!blinking && blinkTime > 0)
                        blinkTime--;
                    if (blinkTime == 8)
                        blinking = false;
                    if (blinking && blinkTime < 8)
                        blinkTime++;
                }

                eyePos = (Vector2.Normalize(target.Center - npc.Center) * 3) + npc.Center + new Vector2(-2, 2 + blinkTime) + (new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f)) * playerHits);

                Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().GetTexture("NPCs/CoralReefs/AquamarineSpireEye"), eyePos.ForDraw(), new Rectangle(0, blinkTime, 8, 8 - blinkTime), Color.White * eyeAlpha, npc.rotation, new Vector2(4, 4), npc.scale, SpriteEffects.None, 0);
                timeBetween = 70;
                bigTimeBetween = 200;

                npc.ai[0]++;
            }

            if (Main.GameUpdateCount % 200 < timeBetween)
            {
                HeartBeat = Math.Abs((float)Math.Sin((Main.GameUpdateCount % bigTimeBetween) * (6.28f / timeBetween))) * (1 - (Main.GameUpdateCount % bigTimeBetween) / (timeBetween * 1.5f));
            }
            else
            {
                HeartBeat = 0;
            }

            if (strikeTime > 0) strikeTime--;
            Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().GetTexture("NPCs/CoralReefs/AquamarineSpireGlow"), npc.Center.ForDraw() + new Vector2(0, 4), npc.frame, Color.Lerp(Color.White * HeartBeat, strikeColor, strikeTime / 60f), npc.rotation, npc.frame.Size() / 2, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }

        public override void AI()
        {
            Player target = Main.LocalPlayer;

            npc.velocity = Vector2.Zero;

            if (awake)
            { 
                playerHits--;

                if (npc.ai[2] == 1)
                {
                    npc.ai[0]++;
                    npc.ai[3] += 2;
                }

                if(npc.ai[0] >= 60)
                {
                    npc.ai[1] = Main.rand.Next(3);
                    npc.ai[0] = 0;
                    npc.ai[2] = 0;
                    switch (npc.ai[1])
                    {
                        case 0: //Blue laser
                            npc.ai[3] = 120;
                            break;
                        case 1: //Cyan laser
                            npc.ai[3] = 90;
                            break;
                        case 2: //Pink laser
                            npc.ai[3] = 90;
                            break;
                    }
                }
               

                if (npc.ai[2] == 0) npc.ai[3]--;
                if(npc.ai[3] <= 0 && npc.ai[2] == 0)
                {
                    switch (npc.ai[1])
                    {
                        case 0: //Blue laser
                            Projectile projectile = Projectile.NewProjectileDirect(eyePos, Vector2.Normalize(target.Center - npc.Center) * 4, ModContent.ProjectileType<SpireLaser>(), npc.damage, 0f, default, 0, 1);
                            EEMod.primitives.CreateTrail(new SpirePrimTrail(projectile, Color.Blue, 80));
                            Main.PlaySound(SoundID.DD2_LightningBugDeath.SoundId, npc.Center, 2);
                            break;
                        case 1: //Cyan laser
                            Projectile projectile2 = Projectile.NewProjectileDirect(eyePos, Vector2.Normalize(target.Center - npc.Center) * 4, ModContent.ProjectileType<SpireLaser>(), npc.damage / 2, 0f, default, 0, 2);
                            EEMod.primitives.CreateTrail(new SpirePrimTrail(projectile2, Color.Cyan, 40));
                            Main.PlaySound(SoundID.DD2_LightningBugDeath.SoundId, npc.Center, 2);
                            break;
                        case 2: //Pink laser
                            for(int i = -1; i < 2; i++)
                            {
                                Projectile projectile3 = Projectile.NewProjectileDirect(eyePos, (Vector2.Normalize(target.Center - npc.Center)).RotatedBy(i / 4f) * 4, ModContent.ProjectileType<SpireLaser>(), npc.damage / 3, 0f, default, 0, 3);
                                EEMod.primitives.CreateTrail(new SpirePrimTrail(projectile3, Color.Magenta, 30));
                            }
                            Main.PlaySound(SoundID.DD2_LightningBugDeath.SoundId, npc.Center, 2);
                            break;
                    }

                    npc.ai[2] = 1;
                }


                for (int i = 0; i < Main.projectile.Length - 1; i++)
                {
                    if (Main.projectile[i].type == ModContent.ProjectileType<SpireLaser>())
                    {
                        Projectile laser = Main.projectile[i];

                        if (Vector2.Distance(laser.Center, npc.Center) <= 3 * 16 && laser.ai[0] > 0 && laser.active)
                        {
                            TakeDamage(laser);
                            break;
                        }
                    }
                }

                if(pseudoHealth <= 0)
                {
                    Die();
                }
            }
            else
            {
                if (firstAwakening)
                {
                    npc.ai[0] = 300 * 60;
                    firstAwakening = false;
                }

                if (npc.ai[0] >= 300 * 60)
                {
                    npc.ai[1]--;
                    if (target.controlUseItem && target.HeldItem.pick > 0 && npc.Hitbox.Intersects(target.Hitbox) && npc.ai[1] <= 0)
                    {
                        playerHits++;
                        npc.ai[1] = target.HeldItem.useTime;
                        Main.PlaySound(SoundID.DD2_CrystalCartImpact, npc.Center);
                    }
                    if (playerHits >= 5)
                    {
                        WakeUp();
                        Main.PlaySound(SoundID.DD2_WitherBeastCrystalImpact, npc.Center);
                    }
                }
            }
        }

        private void TakeDamage(Projectile proj)
        {
            switch (proj.ai[1])
            {
                case 0:
                    break;
                case 1: //Blue
                    pseudoHealth -= 3;
                    strikeColor = Color.Blue;
                    break;
                case 2: //Cyan
                    pseudoHealth -= 2;
                    strikeColor = Color.Cyan;
                    break;
                case 3: //Pink
                    pseudoHealth -= 1;
                    strikeColor = Color.Magenta;
                    break;
            }
            strikeTime = 60;

            proj.Kill();
        }

        private void WakeUp()
        {
            awake = true;
            pseudoHealth = 20;
            npc.ai[0] = 0;
            npc.ai[1] = 0;
            npc.ai[2] = 0;
            npc.ai[3] = 0;
        }

        private void Die()
        {
            awake = false;
            playerHits = 0;
            pseudoHealth = 0;
            npc.ai[0] = 0;
            npc.ai[1] = 0;
            npc.ai[2] = 0;
            npc.ai[3] = 0;
        }
    }
}