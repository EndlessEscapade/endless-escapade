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
using EEMod.Items.Weapons.Melee;
using System.Collections.Generic;

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
        private int pseudoHealth = 40;
        private int strikeTime = 0;
        private Color strikeColor = Color.White;
        private float eyeAlpha = 1f;
        private Vector2 eyePos;
        private bool firstAwakening = true;
        private int cyanLaserShots;
        private bool activatingLamps = true;
        private List<Vector2> nearbyLamps = new List<Vector2>();

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Player target = Main.LocalPlayer;

            float timeBetween;
            float bigTimeBetween;
            if (awake)
            {
                #region Blinking
                if (!blinking && Main.rand.NextBool(240) && blinkTime <= 0)
                    blinking = true;
                if (!blinking && blinkTime > 0)
                    blinkTime--;
                if (blinkTime == 8)
                    blinking = false;
                if (blinking && blinkTime < 8)
                    blinkTime++;
                #endregion

                #region Setting eye color
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
                    case 3: //Purple laser
                        addColor = Color.Purple;
                        break;
                }
                Color color = Color.Lerp(Color.White, addColor, 5 / npc.ai[3]);
                #endregion

                #region Drawing the eye and eye particles
                eyePos = (Vector2.Normalize(target.Center - npc.Center) * 3) + npc.Center + new Vector2(-2, 2 + blinkTime) + (new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f)) * (npc.ai[3] / 20));

                Vector2 obesegru = eyePos + (Vector2.UnitX.RotatedByRandom(MathHelper.Pi) * 128);

                EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.18f));
                EEMod.Particles.Get("Main").SpawnParticles(obesegru, Vector2.Normalize(eyePos - obesegru) * 3, ModContent.GetTexture("EEMod/Particles/Cross"), 9, 1.5f, addColor, new SlowDown(0.98f), new AfterImageTrail(1f), new SetMask(Helpers.RadialMask, 0.6f));

                Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().GetTexture("NPCs/CoralReefs/AquamarineSpireEye"), eyePos.ForDraw(), new Rectangle(0, blinkTime, 8, 8 - blinkTime), color, npc.rotation, new Vector2(4, 4), npc.scale, SpriteEffects.None, 0);
                #endregion

                timeBetween = 35;
                bigTimeBetween = 100;
            }
            else
            {
                if (npc.ai[0] < 300 * 60) //If recharging
                {
                    #region Recharging particles
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
                    #endregion

                    #region Recharging eye
                    if (npc.ai[2] == 0)
                    {
                        if (eyeAlpha > 0 && playerHits == 0) eyeAlpha -= 0.02f;
                        if (blinkTime < 8) blinkTime++;
                        if (eyeAlpha <= 0) npc.ai[2] = 1;
                    }
                    else
                    {
                        eyeAlpha = npc.ai[0] / (300 * 60);
                    }

                    blinkTime = (int)(8 - (npc.ai[0] / (300 * 60)) * 8);
                    #endregion
                }
                else
                {
                    #region Blinking
                    if (!blinking && Main.rand.NextBool(180) && blinkTime <= 0)
                        blinking = true;
                    if (!blinking && blinkTime > 0)
                        blinkTime--;
                    if (blinkTime == 8)
                        blinking = false;
                    if (blinking && blinkTime < 8)
                        blinkTime++;
                    #endregion
                }

                #region Drawing eye
                eyePos = (Vector2.Normalize(target.Center - npc.Center) * 3) + npc.Center + new Vector2(-2, 2 + blinkTime) + (new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f)) * playerHits);

                Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().GetTexture("NPCs/CoralReefs/AquamarineSpireEye"), eyePos.ForDraw(), new Rectangle(0, blinkTime, 8, 8 - blinkTime), Color.White * eyeAlpha, npc.rotation, new Vector2(4, 4), npc.scale, SpriteEffects.None, 0);
                #endregion

                timeBetween = 70;
                bigTimeBetween = 200;

                npc.ai[0]++;
            }

            #region Heartbeat
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
            #endregion
        }

        private int k = 0;
        private bool funi = true;
        public override void AI()
        {
            Player target = Main.LocalPlayer;

            npc.velocity = Vector2.Zero;

            if (awake)
            {
                #region Activating lamps when spire enrages
                if (pseudoHealth <= 20 && activatingLamps)
                {
                    if (funi)
                    {
                        npc.ai[3] = (nearbyLamps.Count * 20);
                        funi = false;
                    }

                    npc.ai[3]--;
                    if (npc.ai[3] % 20 == 0)
                    {
                        Projectile proj = Projectile.NewProjectileDirect(eyePos, Vector2.Normalize(nearbyLamps[k] - eyePos) * 4, ModContent.ProjectileType<WhiteSpireLaser>(), 0, 0f);
                        EEMod.primitives.CreateTrail(new SpirePrimTrail(proj, Color.White, 20));
                        proj.ai[0] = nearbyLamps[k].X;
                        proj.ai[1] = nearbyLamps[k].Y;
                        k++;
                    }

                    if (npc.ai[3] <= 0)
                    {
                        activatingLamps = false;
                        k = 180;
                    }
                }
                else
                {
                    #endregion

                #region Backend stuff
                    playerHits--;

                    if (npc.ai[2] == 1)
                    {
                        npc.ai[0]++;
                        npc.ai[3] += 2;
                    }

                    if (npc.ai[0] >= 60)
                    {
                        npc.ai[1] = Main.rand.Next(4);
                        npc.ai[0] = 0;
                        npc.ai[2] = 0;
                        cyanLaserShots = 0;
                        if (pseudoHealth > 20)
                        {
                            switch (npc.ai[1])
                            {
                                case 0: //Blue laser
                                    npc.ai[3] = 120;
                                    break;
                                case 1: //Cyan laser
                                    npc.ai[3] = 180;
                                    break;
                                case 2: //Pink laser
                                    npc.ai[3] = 180;
                                    break;
                                case 3: //Purple laser
                                    npc.ai[3] = 150;
                                    break;
                            }
                        }
                        else
                        {
                            switch (npc.ai[1])
                            {
                                case 0: //Blue laser
                                    npc.ai[3] = 90;
                                    break;
                                case 1: //Cyan laser
                                    npc.ai[3] = 120;
                                    break;
                                case 2: //Pink laser
                                    npc.ai[3] = 120;
                                    break;
                                case 3: //Purple laser
                                    npc.ai[3] = 120;
                                    break;
                            }
                        }
                    }
                    #endregion

                #region Shooting lasers
                    if (npc.ai[2] == 0) npc.ai[3]--;
                    if (npc.ai[3] <= 0 && npc.ai[2] == 0 && cyanLaserShots < 3)
                    {
                        switch (npc.ai[1])
                        {
                            case 0: //Blue laser
                                Projectile projectile = Projectile.NewProjectileDirect(eyePos, Vector2.Normalize(target.Center - npc.Center) * 4, ModContent.ProjectileType<SpireLaser>(), npc.damage, 0f, default, 0, 1);
                                EEMod.primitives.CreateTrail(new SpirePrimTrail(projectile, Color.Blue, 80));
                                Main.PlaySound(SoundID.DD2_LightningBugDeath.SoundId, npc.Center, 2);
                                npc.ai[2] = 1;
                                break;
                            case 1: //Cyan laser
                                Projectile projectile2 = Projectile.NewProjectileDirect(eyePos, Vector2.Normalize(target.Center - npc.Center) * 4, ModContent.ProjectileType<SpireLaser>(), npc.damage / 2, 0f, default, 0, 2);
                                EEMod.primitives.CreateTrail(new SpirePrimTrail(projectile2, Color.Cyan, 40));
                                Main.PlaySound(SoundID.DD2_LightningBugDeath.SoundId, npc.Center, 2);
                                npc.ai[3] = 10;
                                cyanLaserShots++;
                                if (cyanLaserShots >= 3) npc.ai[2] = 1;
                                break;
                            case 2: //Pink laser
                                for (int i = -1; i < 2; i++)
                                {
                                    Projectile projectile3 = Projectile.NewProjectileDirect(eyePos, (Vector2.Normalize(target.Center - npc.Center)).RotatedBy(i / 4f) * 4, ModContent.ProjectileType<SpireLaser>(), npc.damage / 3, 0f, default, 0, 3);
                                    EEMod.primitives.CreateTrail(new SpirePrimTrail(projectile3, Color.Magenta, 30));
                                }
                                Main.PlaySound(SoundID.DD2_LightningBugDeath.SoundId, npc.Center, 2);
                                npc.ai[2] = 1;
                                break;
                            case 3: //Purple laser
                                for (int i = -1; i < 2; i += 2)
                                {
                                    Projectile projectile4 = Projectile.NewProjectileDirect(eyePos, (Vector2.Normalize(target.Center - npc.Center)).RotatedBy(i / 6f) * 4, ModContent.ProjectileType<SpireLaser>(), npc.damage / 3, 0f, default, 0, 3);
                                    EEMod.primitives.CreateTrail(new SpirePrimTrail(projectile4, Color.Purple, 30));
                                }
                                Main.PlaySound(SoundID.DD2_LightningBugDeath.SoundId, npc.Center, 2);
                                npc.ai[2] = 1;
                                break;
                        }
                    }
                    #endregion

                    #region Shooting lamp lasers
                    if (!funi)
                    {
                        k--;
                        if(k <= 0)
                        {
                            Vector2 obesityepidemic = nearbyLamps[Main.rand.Next(0, nearbyLamps.Count)];
                            Projectile projectile2 = Projectile.NewProjectileDirect(obesityepidemic, Vector2.Normalize(target.Center - obesityepidemic) * 4, ModContent.ProjectileType<SpireLaser>(), npc.damage / 2, 0f, default, 1, 2);
                            EEMod.primitives.CreateTrail(new SpirePrimTrail(projectile2, Color.Cyan, 40));
                            k = 120;
                        }
                    }
                    #endregion

                    #region Taking damage
                    for (int i = 0; i < Main.projectile.Length - 1; i++)
                    {
                        if (Main.projectile[i].type == ModContent.ProjectileType<SpireLaser>())
                        {
                            Projectile laser = Main.projectile[i];

                            if (Vector2.Distance(laser.Center, npc.Center) <= 3 * 16 && laser.ai[0] > 0 && laser.active)
                            {
                                TakeDamage(laser);

                                if (pseudoHealth <= 0)
                                {
                                    Die();

                                    int item = Item.NewItem(new Rectangle((int)eyePos.X, (int)eyePos.Y, 0, 0), ModContent.ItemType<PrismaticBlade>());
                                    Main.item[item].velocity = laser.velocity;
                                }
                                break;
                            }
                        }
                    }
                }
#endregion
            }
            else
            {
                #region Waking up
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
                #endregion
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
                case 4: //Purple
                    pseudoHealth -= 2;
                    strikeColor = Color.Purple;
                    break;
            }
            strikeTime = 60;

            proj.Kill();
        } //Called when spire takes damage

        private void WakeUp()
        {
            awake = true;
            pseudoHealth = 40;
            npc.ai[0] = 0;
            npc.ai[1] = 0;
            npc.ai[2] = 0;
            npc.ai[3] = 0;

            for (int i = (int)(npc.Center.X / 16) - 50; i < (npc.Center.X / 16) + 50; i++)
            {
                for (int j = (int)(npc.Center.Y / 16) - 50; j < (npc.Center.Y / 16) + 50; j++)
                {
                    if (Main.tile[i, j].type == ModContent.TileType<AquamarineLamp1>() && Main.tile[i, j].frameX == 0 && Main.tile[i, j].frameY == 0)
                    {
                        nearbyLamps.Add(new Vector2(i + 1, j - 0.5f) * 16);
                    }
                }
            }
        } //Called on spire awakening

        private void Die()
        {
            awake = false;
            playerHits = 0;
            pseudoHealth = 0;
            npc.ai[0] = 0;
            npc.ai[1] = 0;
            npc.ai[2] = 0;
            npc.ai[3] = 0;
        } //Called on spire death
    }
}