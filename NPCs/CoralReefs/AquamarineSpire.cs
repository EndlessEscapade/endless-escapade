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
using EEMod.Tiles.Foliage.Aquamarine;
using EEMod.Projectiles.Enemy;
using EEMod.Prim;
using EEMod.Items.Weapons.Melee;
using System.Collections.Generic;
using EEMod.Tiles.EmptyTileArrays;
using EEMod.Tiles;


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
            npc.damage = 60;
            npc.behindTiles = true;
            npc.ai[0] = 40;

            musicPriority = MusicPriority.BossLow;
        }

        public override bool CheckActive()
        {
            return false;
        }

        public bool awake = false;
        int blinkTime = 0;
        bool blinking = false;
        private int strikeTime = 0;
        private Color strikeColor = Color.White;
        private float eyeAlpha = 1f;
        private Vector2 eyePos;
        private bool firstAwakening = true;
        private int specialLaserShots;
        private int timer1;
        private float eyeRecoil;

        //npc.ai[0] : Player hits on spire / Health
        //npc.ai[1] : Laser color
        //npc.ai[2] : Timer stuffens?
        //npc.ai[3] : Heartbeat
        //Timer1 : Manages cooldowns - recharge time and laser shot cooldown

        private float alpha;
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            alpha += 0.05f;

            EEMod.SpireShader.Parameters["alpha"].SetValue((npc.ai[0] <= 20 && awake) ? 4 - (alpha * 2 % 4) : 6 - (alpha * 2 % 6));
            EEMod.SpireShader.Parameters["shineSpeed"].SetValue(npc.ai[0] <= 20 ? 0.4f : 0.2f);
            EEMod.SpireShader.Parameters["tentacle"].SetValue(ModContent.GetInstance<EEMod>().GetTexture("ShaderAssets/SpireLightMap"));
            EEMod.SpireShader.Parameters["shaderLerp"].SetValue(1f);
            EEMod.SpireShader.Parameters["lightColor"].SetValue(drawColor.ToVector3());
            EEMod.SpireShader.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, 4), Main.npcTexture[npc.type].Bounds, drawColor, npc.rotation, Main.npcTexture[npc.type].Size() / 2f, npc.scale, SpriteEffects.None, 0f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }

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
                    case 1: //Pink laser
                        addColor = Color.Magenta;
                        break;
                    case 2: //White laser
                        addColor = Color.White;
                        break;
                    case 3: //Cyan laser
                        addColor = Color.Cyan;
                        break;
                }
                Color color = addColor;

                if (timer1 != 0)
                    color = Color.Lerp(Color.White, addColor, 10 / timer1);
                #endregion


                if (eyeRecoil <= 1) eyeRecoil += 0.05f;

                #region Drawing the eye and eye particles
                blinkTime = 0;
                eyePos = (Vector2.Normalize(target.Center - npc.Center) * (3 * eyeRecoil)) + npc.Center + new Vector2(-2, 2 + blinkTime) + (new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f)) * (timer1 / 20f));

                Vector2 obesegru = eyePos + (Vector2.UnitX.RotatedByRandom(MathHelper.Pi) * 96);

                EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(Helpers.Clamp(0.25f * ((timer1 - 10) / 40f), 0, 1)));
                EEMod.Particles.Get("Main").SpawnParticles(obesegru, Vector2.Normalize(eyePos - obesegru) * 5, ModContent.GetTexture("EEMod/Particles/SmallCircle"), 7, 3f, addColor, new SlowDown(0.943f), new AfterImageTrail(0.6f), new SetMask(Helpers.RadialMask, 0.9f));

                Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().GetTexture("NPCs/CoralReefs/AquamarineSpireEye"), eyePos.ForDraw(), new Rectangle(0, blinkTime, 8, 8 - blinkTime), color, npc.rotation, new Vector2(4, 4), npc.scale, SpriteEffects.None, 1f);
                #endregion

                timeBetween = 35;
                bigTimeBetween = 100;
            }

            else
            {
                music = -1;
                if (timer1 < 300 * 60) //If recharging
                {
                    #region Recharging eye
                    if (npc.ai[2] == 0)
                    {
                        if (eyeAlpha > 0 && npc.ai[0] == 0) eyeAlpha -= 0.02f;
                        if (blinkTime < 8) blinkTime++;
                        if (eyeAlpha <= 0) npc.ai[2] = 1;
                    }
                    else
                    {
                        eyeAlpha = timer1 / (300 * 60);
                    }

                    blinkTime = (8 - (timer1 / (300 * 60)) * 8);
                    #endregion

                    strikeTime = 61;
                    strikeColor = Lighting.GetColor((int)npc.Center.X / 16, (int)npc.Center.Y / 16);
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

                    eyeAlpha = npc.ai[0] / 5f;
                }

                #region Recharging particles
                EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.18f));
                Vector2 one = new Vector2(-8, Main.rand.Next(-8, 8)).RotatedBy(1.57f / 2f + npc.ai[3] / 60f);
                Vector2 two = new Vector2(8, Main.rand.Next(-8, 8)).RotatedBy(1.57f / 2f + npc.ai[3] / 60f);
                Vector2 three = new Vector2(Main.rand.Next(-8, 8), 8).RotatedBy(1.57f / 2f + npc.ai[3] / 60f);
                Vector2 four = new Vector2(Main.rand.Next(-8, 8), -8).RotatedBy(1.57f / 2f + npc.ai[3] / 60f);
                Vector2 offset = new Vector2(-3, (float)Math.Sin(Main.GameUpdateCount / 60f) + 2 + npc.ai[3] / 60f);

                int scale = 4;
                EEMod.Particles.Get("Main").SpawnParticles(npc.Center + one * scale + offset, -Vector2.Normalize(one) / 2f, ModContent.GetTexture("EEMod/Particles/SmallCircle"), 30, 1, Color.White, new SlowDown(0.95f), new AfterImageTrail(1f), new SetMask(Helpers.RadialMask, 0.6f));
                EEMod.Particles.Get("Main").SpawnParticles(npc.Center + two * scale + offset, -Vector2.Normalize(two) / 2f, ModContent.GetTexture("EEMod/Particles/SmallCircle"), 30, 1, Color.White, new SlowDown(0.95f), new AfterImageTrail(1f), new SetMask(Helpers.RadialMask, 0.6f));
                EEMod.Particles.Get("Main").SpawnParticles(npc.Center + three * scale + offset, -Vector2.Normalize(three) / 2f, ModContent.GetTexture("EEMod/Particles/SmallCircle"), 30, 1, Color.White, new SlowDown(0.95f), new AfterImageTrail(1f), new SetMask(Helpers.RadialMask, 0.6f));
                EEMod.Particles.Get("Main").SpawnParticles(npc.Center + four * scale + offset, -Vector2.Normalize(four) / 2f, ModContent.GetTexture("EEMod/Particles/SmallCircle"), 30, 1, Color.White, new SlowDown(0.95f), new AfterImageTrail(1f), new SetMask(Helpers.RadialMask, 0.6f));
                #endregion

                #region Drawing eye
                eyePos = (new Vector2(0, 1) * 3) + npc.Center + new Vector2(-2, 2) + (new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f)) * npc.ai[0]);

                Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().GetTexture("NPCs/CoralReefs/AquamarineSpireEye"), eyePos.ForDraw(), new Rectangle(0, blinkTime, 8, 8 - blinkTime), Color.White * eyeAlpha, npc.rotation, new Vector2(4, 4), npc.scale, SpriteEffects.None, 0);
                #endregion

                timeBetween = 70;
                bigTimeBetween = 200;

                timer1++;
            }

            #region Heartbeat
            if (Main.GameUpdateCount % bigTimeBetween < timeBetween)
            {
                npc.ai[3] = Math.Abs((float)Math.Sin((Main.GameUpdateCount % bigTimeBetween) * (6.28f / timeBetween))) * (1 - (Main.GameUpdateCount % bigTimeBetween) / (timeBetween * 1.5f));
            }
            else
            {
                npc.ai[3] = 0;
            }

            if (strikeTime > 0) strikeTime--;
            Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().GetTexture("NPCs/CoralReefs/AquamarineSpireGlow"), npc.Center.ForDraw() + new Vector2(0, 4), npc.frame, Color.Lerp(Color.White * npc.ai[3], strikeColor, strikeTime / 60f), npc.rotation, npc.frame.Size() / 2, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            #endregion
        }

        public List<Projectile> shields = new List<Projectile>();
        public override void AI()
        {
            Player target = Main.player[Helpers.GetNearestAlivePlayer(npc)];

            npc.velocity = Vector2.Zero;

            if (awake)
            {
                #region Choosing attack cooldowns

                npc.ai[2]++;

                if (npc.ai[2] >= 1)
                {
                    npc.ai[1] = Main.rand.Next(4);
                    specialLaserShots = 0;

                    if (npc.ai[0] > 20)
                    {
                        switch (npc.ai[1])
                        {
                            case 0: //Blue laser
                                npc.ai[2] = -120;
                                break;
                            case 1: //Pink laser
                                npc.ai[2] = -150;
                                break;
                            case 2: //White laser
                                npc.ai[2] = -150;
                                break;
                            case 3: //Cyan laser
                                npc.ai[2] = -150;
                                break;
                        }
                    }
                    else
                    {
                        switch (npc.ai[1])
                        {
                            case 0: //Blue laser
                                npc.ai[2] = -84;
                                break;
                            case 1: //Pink laser
                                npc.ai[2] = -105;
                                break;
                            case 2: //White laser
                                npc.ai[2] = -105;
                                break;
                            case 3: //Cyan laser
                                npc.ai[2] = -105;
                                break;
                        }
                    }
                    timer1 = -(int)npc.ai[2];
                }

                if (npc.ai[2] < 0)
                    timer1--;

                if (npc.ai[2] >= 0 && npc.ai[2] < 60)
                    timer1 += 3;
                #endregion

                #region Shooting lasers
                if (npc.ai[2] == 0 && ((specialLaserShots < 3 && npc.ai[1] == 1) || (specialLaserShots < 2 && npc.ai[1] == 3) || (npc.ai[1] != 3 && npc.ai[1] != 1)))
                {
                    switch (npc.ai[1])
                    {
                        case 0: //Blue laser
                            Projectile projectile = Projectile.NewProjectileDirect(eyePos, Vector2.Normalize(target.Center - npc.Center) * 2, ModContent.ProjectileType<SpireLaser>(), npc.damage, 0f, default, 0, 1);
                            EEMod.primitives.CreateTrail(new SpirePrimTrail(projectile, Color.Lerp(Color.Navy, Color.LightBlue, Main.rand.NextFloat(0, 1)), 50));

                            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/SpireShoot"), npc.Center);
                            eyeRecoil = -0.5f;
                            break;

                        case 1: //Pink laser
                            Projectile projectile2 = Projectile.NewProjectileDirect(eyePos, Vector2.Normalize(target.Center - npc.Center) * 2, ModContent.ProjectileType<SpireLaser>(), npc.damage / 2, 0f, default, 0, 2);
                            EEMod.primitives.CreateTrail(new SpirePrimTrail(projectile2, Color.Lerp(Color.Purple, Color.Pink, Main.rand.NextFloat(0, 1)), 40));

                            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/SpireShoot"), npc.Center);

                            npc.ai[2] = -20;
                            eyeRecoil = -0.5f;
                            timer1 = 20;
                            specialLaserShots++;
                            if (specialLaserShots >= (npc.ai[0] <= 20 ? 4 : 3)) npc.ai[2] = 1;
                            break;

                        case 2: //White laser
                            Projectile projectile3 = Projectile.NewProjectileDirect(eyePos, Vector2.Normalize(target.Center - npc.Center) * 2, ModContent.ProjectileType<WideSpireLaser>(), npc.damage / 2, 0f, default, 0, 3);
                            EEMod.primitives.CreateTrail(new SpirePrimTrail(projectile3, Color.White, 80));

                            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/SpireShoot"), npc.Center);
                            eyeRecoil = -0.5f;
                            break;

                        case 3: //Cyan laser
                            if (specialLaserShots == 0)
                            {
                                Projectile projectile4 = Projectile.NewProjectileDirect(eyePos, Vector2.Normalize(target.Center - npc.Center) * 2, ModContent.ProjectileType<SpireLaser>(), npc.damage / 3, 0f, default, 0, 1);
                                EEMod.primitives.CreateTrail(new SpirePrimTrail(projectile4, Color.Lerp(Color.LightCyan, Color.DarkCyan, Main.rand.NextFloat(0, 1)), 30));
                                specialLaserShots++;
                                npc.ai[2] = -30;
                            }
                            if (specialLaserShots == 1)
                            {
                                for (int i = -1; i < 2; i += 2)
                                {
                                    Projectile projectile4 = Projectile.NewProjectileDirect(eyePos, (Vector2.Normalize(target.Center - npc.Center)).RotatedBy(i / 6f) * 2, ModContent.ProjectileType<SpireLaser>(), npc.damage / 3, 0f, default, 0, 4);
                                    EEMod.primitives.CreateTrail(new SpirePrimTrail(projectile4, Color.Lerp(Color.LightCyan, Color.DarkCyan, Main.rand.NextFloat(0, 1)), 30));
                                }
                                specialLaserShots++;
                                npc.ai[2] = 1;
                            }

                            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/SpireShoot").WithVolume(1f).WithPitchVariance(0f), npc.Center);
                            eyeRecoil = -0.5f;
                            break;
                    }
                }
                #endregion

                #region Taking damage
                for (int i = 0; i < Main.projectile.Length - 1; i++)
                {
                    if (Main.projectile[i].type == ModContent.ProjectileType<SpireLaser>())
                    {
                        Projectile laser = Main.projectile[i];

                        if (Vector2.Distance(laser.Center, npc.Center) <= 32 && laser.ai[0] > 0 && laser.active)
                        {
                            TakeDamage(laser);
                            Main.PlaySound(SoundID.NPCDeath56, npc.Center);

                            if (npc.ai[0] <= 0)
                            {
                                Die();

                                int item = Item.NewItem(new Rectangle((int)eyePos.X, (int)eyePos.Y, 0, 0), ModContent.ItemType<PrismaticBlade>());
                                Main.item[item].velocity = laser.velocity;
                            }
                            break;
                        }
                    }
                }
                #endregion

                for (int i = 0; i < Main.player.Length - 1; i++)
                {
                    Player player = Main.player[i];
                    if (player.IsAlive())
                        player.AddBuff(BuffID.NoBuilding, 60, true);
                }

            }

            else
            {
                #region Waking up
                if (firstAwakening)
                {
                    timer1 = 300 * 60;

                    for (int i = (int)(npc.Center.X / 16) - 150; i < (int)(npc.Center.X / 16) + 150; i++)
                    {
                        for (int j = (int)(npc.Center.Y / 16) - 150; j < (int)(npc.Center.Y / 16) + 150; j++)
                        {
                            if (WorldGen.InWorld(i, j) && Framing.GetTileSafely(i, j).type == ModContent.TileType<AquamarineLamp1>() && Framing.GetTileSafely(i, j).frameX == 0 && Framing.GetTileSafely(i, j).frameY == 0)
                            {
                                Projectile proj = Projectile.NewProjectileDirect(new Vector2((i * 16) + 16, (j * 16) - 12), Vector2.Zero, ModContent.ProjectileType<AquamarineLamp1Glow>(), 0, 0, default, 0, 0);
                                if (shields.Count < 10)
                                {
                                    shields.Add(proj);
                                }
                            }
                        }
                    }

                    firstAwakening = false;
                }

                if (timer1 >= 300 * 60)
                {
                    npc.ai[1]--;
                    if (npc.ai[1] <= -60 * 15)
                    {
                        if (npc.ai[0] > 0) npc.ai[0]--;
                    }

                    if (target.controlUseItem && target.HeldItem.pick > 0 && npc.Hitbox.Intersects(target.Hitbox) && npc.ai[1] <= 0)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = target.HeldItem.useTime;
                        Main.PlaySound(SoundID.DD2_CrystalCartImpact, npc.Center);
                    }
                    if (npc.ai[0] >= 5)
                    {
                        WakeUp();
                        Main.PlaySound(SoundID.DD2_WitherBeastCrystalImpact, npc.Center);
                    }
                }

                #endregion
            }

            for (int i = 0; i < shields.Count; i++)
            {
                shields[i].ai[0] = shields.Count;
                shields[i].ai[1] = i;
            }
        }

        private void TakeDamage(Projectile proj)
        {
            if (proj.type == ModContent.ProjectileType<SpireLaser>())
            {
                switch (proj.ai[1])
                {
                    case 0:
                        break;
                    case 1: //Blue
                        npc.ai[0] -= 3;
                        strikeColor = Color.Blue;
                        break;
                    case 2: //Pink
                        npc.ai[0] -= 2;
                        strikeColor = Color.Magenta;
                        break;
                    case 3: //White
                        npc.ai[0] -= 1;
                        strikeColor = Color.White;
                        break;
                    case 4: //Cyan
                        npc.ai[0] -= 2;
                        strikeColor = Color.Cyan;
                        break;
                }
                strikeTime = 60;
            }
            else
            {
                npc.ai[0] -= 3;
                strikeColor = Color.White;
            }

            proj.Kill();
        } //Called when spire takes damage

        private void WakeUp()
        {
            awake = true;
            npc.ai[0] = 40;
            timer1 = 180;
            npc.ai[1] = 0;
            npc.ai[2] = 0;
        } //Called on spire awakening

        private void Die()
        {
            awake = false;
            npc.ai[0] = 0;
            npc.ai[0] = 0;
            timer1 = 0;
            npc.ai[1] = 0;
            npc.ai[2] = 0;
        } //Called on spire death
    }
}