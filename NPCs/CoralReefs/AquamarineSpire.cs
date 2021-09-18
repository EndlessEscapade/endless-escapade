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
using EEMod.Items.Weapons.Melee.Swords;
using System.Collections.Generic;
using EEMod.Tiles.EmptyTileArrays;
using EEMod.Tiles;
using Terraria.Audio;
using Terraria.GameContent;

namespace EEMod.NPCs.CoralReefs
{
    public class AquamarineSpire : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aquamarine Spire");
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.friendly = true;
            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath28;
            NPC.lifeMax = 1000000;
            NPC.width = 320;
            NPC.height = 416;
            NPC.noGravity = true;
            NPC.lavaImmune = true;
            NPC.noTileCollide = true;
            NPC.dontTakeDamage = true;
            NPC.damage = 60;
            NPC.behindTiles = true;
            NPC.ai[0] = 40;

            //musicPriority = MusicPriority.BossLow;
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

            if(drawColor.ToVector3() == Vector3.Zero)
            {
                drawColor = new Color(0.1f, 0.1f, 0.1f, 255f);
            }

            EEMod.SpireShader.Parameters["alpha"].SetValue((NPC.ai[0] <= 20 && awake) ? 4 - (alpha * 2 % 4) : 6 - (alpha * 2 % 6));
            EEMod.SpireShader.Parameters["shineSpeed"].SetValue(NPC.ai[0] <= 20 ? 0.4f : 0.2f);
            EEMod.SpireShader.Parameters["tentacle"].SetValue(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/SpireLightMap").Value);
            EEMod.SpireShader.Parameters["shaderLerp"].SetValue(1f);
            EEMod.SpireShader.Parameters["lightColor"].SetValue(drawColor.ToVector3());
            EEMod.SpireShader.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, 4), TextureAssets.Npc[NPC.type].Value.Bounds, drawColor, NPC.rotation, TextureAssets.Npc[NPC.type].Value.Size() / 2f, NPC.scale, SpriteEffects.None, 0f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

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
                switch (NPC.ai[1])
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
                eyePos = (Vector2.Normalize(target.Center - NPC.Center) * (3 * eyeRecoil)) + NPC.Center + new Vector2(-2, 2 + blinkTime) + (new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f)) * (timer1 / 20f));

                Vector2 obesegru = eyePos + (Vector2.UnitX.RotatedByRandom(MathHelper.Pi) * 96);

                EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(Helpers.Clamp(0.25f * ((timer1 - 10) / 40f), 0, 1)));
                EEMod.MainParticles.SpawnParticles(obesegru, Vector2.Normalize(eyePos - obesegru) * 5, ModContent.Request<Texture2D>("EEMod/Particles/SmallCircle").Value, 7, 3f, addColor, new SlowDown(0.943f), new AfterImageTrail(0.6f), new SetMask(Helpers.RadialMask, 0.9f));

                Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("NPCs/CoralReefs/AquamarineSpireEye").Value, eyePos.ForDraw(), new Rectangle(0, blinkTime, 8, 8 - blinkTime), color, NPC.rotation, new Vector2(4, 4), NPC.scale, SpriteEffects.None, 1f);
                #endregion

                timeBetween = 35;
                bigTimeBetween = 100;
            }

            else
            {
                //music = -1;
                if (timer1 < 300 * 60) //If recharging
                {
                    #region Recharging eye
                    if (NPC.ai[2] == 0)
                    {
                        if (eyeAlpha > 0 && NPC.ai[0] == 0) eyeAlpha -= 0.02f;
                        if (blinkTime < 8) blinkTime++;
                        if (eyeAlpha <= 0) NPC.ai[2] = 1;
                    }
                    else
                    {
                        eyeAlpha = timer1 / (300 * 60);
                    }

                    blinkTime = (8 - (timer1 / (300 * 60)) * 8);
                    #endregion

                    strikeTime = 61;
                    strikeColor = Lighting.GetColor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16);
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

                    eyeAlpha = NPC.ai[0] / 5f;
                }

                #region Recharging particles
                EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.18f));
                Vector2 one = new Vector2(-8, Main.rand.Next(-8, 8)).RotatedBy(1.57f / 2f + NPC.ai[3] / 60f);
                Vector2 two = new Vector2(8, Main.rand.Next(-8, 8)).RotatedBy(1.57f / 2f + NPC.ai[3] / 60f);
                Vector2 three = new Vector2(Main.rand.Next(-8, 8), 8).RotatedBy(1.57f / 2f + NPC.ai[3] / 60f);
                Vector2 four = new Vector2(Main.rand.Next(-8, 8), -8).RotatedBy(1.57f / 2f + NPC.ai[3] / 60f);
                Vector2 offset = new Vector2(-3, (float)Math.Sin(Main.GameUpdateCount / 60f) + 2 + NPC.ai[3] / 60f);

                int scale = 4;
                EEMod.MainParticles.SpawnParticles(NPC.Center + one * scale + offset, -Vector2.Normalize(one) / 2f, ModContent.Request<Texture2D>("EEMod/Particles/SmallCircle").Value, 30, 1, Color.White, new SlowDown(0.95f), new AfterImageTrail(1f), new SetMask(Helpers.RadialMask, 0.6f));
                EEMod.MainParticles.SpawnParticles(NPC.Center + two * scale + offset, -Vector2.Normalize(two) / 2f, ModContent.Request<Texture2D>("EEMod/Particles/SmallCircle").Value, 30, 1, Color.White, new SlowDown(0.95f), new AfterImageTrail(1f), new SetMask(Helpers.RadialMask, 0.6f));
                EEMod.MainParticles.SpawnParticles(NPC.Center + three * scale + offset, -Vector2.Normalize(three) / 2f, ModContent.Request<Texture2D>("EEMod/Particles/SmallCircle").Value, 30, 1, Color.White, new SlowDown(0.95f), new AfterImageTrail(1f), new SetMask(Helpers.RadialMask, 0.6f));
                EEMod.MainParticles.SpawnParticles(NPC.Center + four * scale + offset, -Vector2.Normalize(four) / 2f, ModContent.Request<Texture2D>("EEMod/Particles/SmallCircle").Value, 30, 1, Color.White, new SlowDown(0.95f), new AfterImageTrail(1f), new SetMask(Helpers.RadialMask, 0.6f));
                #endregion

                #region Drawing eye
                eyePos = (new Vector2(0, 1) * 3) + NPC.Center + new Vector2(-2, 2) + (new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f)) * NPC.ai[0]);

                Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("NPCs/CoralReefs/AquamarineSpireEye").Value, eyePos.ForDraw(), new Rectangle(0, blinkTime, 8, 8 - blinkTime), Color.White * eyeAlpha, NPC.rotation, new Vector2(4, 4), NPC.scale, SpriteEffects.None, 0);
                #endregion

                timeBetween = 70;
                bigTimeBetween = 200;

                timer1++;
            }

            #region Heartbeat
            if (Main.GameUpdateCount % bigTimeBetween < timeBetween)
            {
                NPC.ai[3] = Math.Abs((float)Math.Sin((Main.GameUpdateCount % bigTimeBetween) * (6.28f / timeBetween))) * (1 - (Main.GameUpdateCount % bigTimeBetween) / (timeBetween * 1.5f));
            }
            else
            {
                NPC.ai[3] = 0;
            }

            if (strikeTime > 0) strikeTime--;
            Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("NPCs/CoralReefs/AquamarineSpireGlow").Value, NPC.Center.ForDraw() + new Vector2(0, 4), NPC.frame, Color.Lerp(Color.White * NPC.ai[3], strikeColor, strikeTime / 60f), NPC.rotation, NPC.frame.Size() / 2, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            #endregion
        }

        public List<Projectile> shields = new List<Projectile>();
        public override void AI()
        {
            Player target = Main.player[Helpers.GetNearestAlivePlayer(NPC)];

            NPC.velocity = Vector2.Zero;

            if (awake)
            {
                #region Choosing attack cooldowns

                NPC.ai[2]++;

                if (NPC.ai[2] >= 1)
                {
                    NPC.ai[1] = Main.rand.Next(4);
                    specialLaserShots = 0;

                    if (NPC.ai[0] > 20)
                    {
                        switch (NPC.ai[1])
                        {
                            case 0: //Blue laser
                                NPC.ai[2] = -120;
                                break;
                            case 1: //Pink laser
                                NPC.ai[2] = -150;
                                break;
                            case 2: //White laser
                                NPC.ai[2] = -150;
                                break;
                            case 3: //Cyan laser
                                NPC.ai[2] = -150;
                                break;
                        }
                    }
                    else
                    {
                        switch (NPC.ai[1])
                        {
                            case 0: //Blue laser
                                NPC.ai[2] = -84;
                                break;
                            case 1: //Pink laser
                                NPC.ai[2] = -105;
                                break;
                            case 2: //White laser
                                NPC.ai[2] = -105;
                                break;
                            case 3: //Cyan laser
                                NPC.ai[2] = -105;
                                break;
                        }
                    }
                    timer1 = -(int)NPC.ai[2];
                }

                if (NPC.ai[2] < 0)
                    timer1--;

                if (NPC.ai[2] >= 0 && NPC.ai[2] < 60)
                    timer1 += 3;
                #endregion

                #region Shooting lasers
                if (NPC.ai[2] == 0 && ((specialLaserShots < 3 && NPC.ai[1] == 1) || (specialLaserShots < 2 && NPC.ai[1] == 3) || (NPC.ai[1] != 3 && NPC.ai[1] != 1)))
                {
                    switch (NPC.ai[1])
                    {
                        case 0: //Blue laser
                            Projectile projectile = Projectile.NewProjectileDirect(new Terraria.DataStructures.ProjectileSource_NPC(NPC), eyePos, Vector2.Normalize(target.Center - NPC.Center) * 2, ModContent.ProjectileType<SpireLaser>(), NPC.damage, 0f, default, 0, 1);
                            EEMod.primitives.CreateTrail(new SpirePrimTrail(projectile, Color.Lerp(Color.Navy, Color.LightBlue, Main.rand.NextFloat(0, 1)), 50));

                            //SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/SpireShoot"), NPC.Center);
                            eyeRecoil = -0.5f;
                            break;

                        case 1: //Pink laser
                            Projectile projectile2 = Projectile.NewProjectileDirect(new Terraria.DataStructures.ProjectileSource_NPC(NPC), eyePos, Vector2.Normalize(target.Center - NPC.Center) * 2, ModContent.ProjectileType<SpireLaser>(), NPC.damage / 2, 0f, default, 0, 2);
                            EEMod.primitives.CreateTrail(new SpirePrimTrail(projectile2, Color.Lerp(Color.Purple, Color.Pink, Main.rand.NextFloat(0, 1)), 40));

                            //SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/SpireShoot"), NPC.Center);

                            NPC.ai[2] = -20;
                            eyeRecoil = -0.5f;
                            timer1 = 20;
                            specialLaserShots++;
                            if (specialLaserShots >= (NPC.ai[0] <= 20 ? 4 : 3)) NPC.ai[2] = 1;
                            break;

                        case 2: //White laser
                            Projectile projectile3 = Projectile.NewProjectileDirect(new Terraria.DataStructures.ProjectileSource_NPC(NPC), eyePos, Vector2.Normalize(target.Center - NPC.Center) * 2, ModContent.ProjectileType<WideSpireLaser>(), NPC.damage / 2, 0f, default, 0, 3);
                            EEMod.primitives.CreateTrail(new SpirePrimTrail(projectile3, Color.White, 80));

                            //SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/SpireShoot"), NPC.Center);
                            eyeRecoil = -0.5f;
                            break;

                        case 3: //Cyan laser
                            if (specialLaserShots == 0)
                            {
                                Projectile projectile4 = Projectile.NewProjectileDirect(new Terraria.DataStructures.ProjectileSource_NPC(NPC), eyePos, Vector2.Normalize(target.Center - NPC.Center) * 2, ModContent.ProjectileType<SpireLaser>(), NPC.damage / 3, 0f, default, 0, 1);
                                EEMod.primitives.CreateTrail(new SpirePrimTrail(projectile4, Color.Lerp(Color.LightCyan, Color.DarkCyan, Main.rand.NextFloat(0, 1)), 30));
                                specialLaserShots++;
                                NPC.ai[2] = -30;
                            }
                            if (specialLaserShots == 1)
                            {
                                for (int i = -1; i < 2; i += 2)
                                {
                                    Projectile projectile4 = Projectile.NewProjectileDirect(new Terraria.DataStructures.ProjectileSource_NPC(NPC), eyePos, (Vector2.Normalize(target.Center - NPC.Center)).RotatedBy(i / 6f) * 2, ModContent.ProjectileType<SpireLaser>(), NPC.damage / 3, 0f, default, 0, 4);
                                    EEMod.primitives.CreateTrail(new SpirePrimTrail(projectile4, Color.Lerp(Color.LightCyan, Color.DarkCyan, Main.rand.NextFloat(0, 1)), 30));
                                }
                                specialLaserShots++;
                                NPC.ai[2] = 1;
                            }

                            //SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/SpireShoot").WithVolume(1f).WithPitchVariance(0f), NPC.Center);
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

                        if (Vector2.Distance(laser.Center, NPC.Center) <= 32 && laser.ai[0] > 0 && laser.active)
                        {
                            TakeDamage(laser);
                            SoundEngine.PlaySound(SoundID.NPCDeath56, NPC.Center);

                            if (NPC.ai[0] <= 0)
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

                    for (int i = (int)(NPC.Center.X / 16) - 150; i < (int)(NPC.Center.X / 16) + 150; i++)
                    {
                        for (int j = (int)(NPC.Center.Y / 16) - 150; j < (int)(NPC.Center.Y / 16) + 150; j++)
                        {
                            if (WorldGen.InWorld(i, j) && Framing.GetTileSafely(i, j).type == ModContent.TileType<AquamarineLamp1>() && Framing.GetTileSafely(i, j).frameX == 0 && Framing.GetTileSafely(i, j).frameY == 0)
                            {
                                Projectile proj = Projectile.NewProjectileDirect(new Terraria.DataStructures.ProjectileSource_NPC(NPC), new Vector2((i * 16) + 16, (j * 16) - 12), Vector2.Zero, ModContent.ProjectileType<AquamarineLamp1Glow>(), 0, 0, default, 0, 0);
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
                    NPC.ai[1]--;
                    if (NPC.ai[1] <= -60 * 15)
                    {
                        if (NPC.ai[0] > 0) NPC.ai[0]--;
                    }

                    if (target.controlUseItem && target.HeldItem.pick > 0 && NPC.Hitbox.Intersects(target.Hitbox) && NPC.ai[1] <= 0)
                    {
                        NPC.ai[0]++;
                        NPC.ai[1] = target.HeldItem.useTime;
                        SoundEngine.PlaySound(SoundID.DD2_CrystalCartImpact, NPC.Center);
                    }
                    if (NPC.ai[0] >= 5)
                    {
                        WakeUp();
                        SoundEngine.PlaySound(SoundID.DD2_WitherBeastCrystalImpact, NPC.Center);
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
                        NPC.ai[0] -= 3;
                        strikeColor = Color.Blue;
                        break;
                    case 2: //Pink
                        NPC.ai[0] -= 2;
                        strikeColor = Color.Magenta;
                        break;
                    case 3: //White
                        NPC.ai[0] -= 1;
                        strikeColor = Color.White;
                        break;
                    case 4: //Cyan
                        NPC.ai[0] -= 2;
                        strikeColor = Color.Cyan;
                        break;
                }
                strikeTime = 60;
            }
            else
            {
                NPC.ai[0] -= 3;
                strikeColor = Color.White;
            }

            proj.Kill();
        } //Called when spire takes damage

        private void WakeUp()
        {
            awake = true;
            NPC.ai[0] = 40;
            timer1 = 180;
            NPC.ai[1] = 0;
            NPC.ai[2] = 0;
        } //Called on spire awakening

        private void Die()
        {
            awake = false;
            NPC.ai[0] = 0;
            NPC.ai[0] = 0;
            timer1 = 0;
            NPC.ai[1] = 0;
            NPC.ai[2] = 0;
        } //Called on spire death
    }
}