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
            npc.ai[0] = 40;
        }

        public override bool CheckActive()
        {
            return false;
        }

        public bool awake = false;
        int blinkTime = 0;
        bool blinking = false;
        int playerHits = 0;
        private int strikeTime = 0;
        private Color strikeColor = Color.White;
        private float eyeAlpha = 1f;
        private Vector2 eyePos;
        private bool firstAwakening = true;
        private int cyanLaserShots;
        private int timer1;
        private int timer2;

        //npc.ai[0] : Health
        //npc.ai[1] : Laser color
        //npc.ai[2] : Timer stuffens?
        //npc.ai[3] : Heartbeat
        //Timer1 : Manages cooldowns - recharge time and laser shot cooldown
        //Timer2 : Second timer?

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
                Color color = addColor;

                if(timer2 != 0)
                    color = Color.Lerp(Color.White, addColor, 5 / timer2);
                #endregion

                #region Drawing the eye and eye particles
                eyePos = (Vector2.Normalize(target.Center - npc.Center) * 3) + npc.Center + new Vector2(-2, 2 + blinkTime) + (new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f)) * (timer2 / 20f));

                Vector2 obesegru = eyePos + (Vector2.UnitX.RotatedByRandom(MathHelper.Pi) * 128);

                EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.12f * (timer2 / 40f)));
                EEMod.Particles.Get("Main").SpawnParticles(obesegru, Vector2.Normalize(eyePos - obesegru) * 3, ModContent.GetTexture("EEMod/Particles/Crystal"), 8, 3f, addColor, new SlowDown(0.98f), new AfterImageTrail(1f), new SetMask(Helpers.RadialMask, 0.8f));

                Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().GetTexture("NPCs/CoralReefs/AquamarineSpireEye"), eyePos.ForDraw(), new Rectangle(0, blinkTime, 8, 8 - blinkTime), color, npc.rotation, new Vector2(4, 4), npc.scale, SpriteEffects.None, 0);
                #endregion

                timeBetween = 35;
                bigTimeBetween = 100;
            }
            else
            {
                if (timer1 < 300 * 60) //If recharging
                {
                    #region Recharging particles
                    EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.18f));
                    Vector2 one = new Vector2(-8, Main.rand.Next(-8, 8)).RotatedBy(1.57f / 2f + npc.ai[3] / 60f);
                    Vector2 two = new Vector2(8, Main.rand.Next(-8, 8)).RotatedBy(1.57f / 2f + npc.ai[3] / 60f);
                    Vector2 three = new Vector2(Main.rand.Next(-8, 8), 8).RotatedBy(1.57f / 2f + npc.ai[3] / 60f);
                    Vector2 four = new Vector2(Main.rand.Next(-8, 8), -8).RotatedBy(1.57f / 2f + npc.ai[3] / 60f);
                    Vector2 offset = new Vector2(-3, (float)Math.Sin(Main.GameUpdateCount / 60f) + 2 + npc.ai[3] / 60f);

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
                        eyeAlpha = timer1 / (300 * 60);
                    }

                    blinkTime = (8 - (timer1 / (300 * 60)) * 8);
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

        private List<Projectile> shields = new List<Projectile>();
        public override void AI()
        {
            Player target = Main.LocalPlayer;

            npc.velocity = Vector2.Zero;

            if (awake)
            {
                #region Backend stuff
                playerHits--;

                if (npc.ai[2] == 1)
                {
                    timer1++;
                    timer2 += 2;
                }

                if (timer1 >= 60)
                {
                    npc.ai[1] = Main.rand.Next(4);
                    timer1 = 0;
                    npc.ai[2] = 0;
                    cyanLaserShots = 0;

                    if (npc.ai[0] > 20)
                    {
                        switch (npc.ai[1])
                        {
                            case 0: //Blue laser
                                timer2 = 120;
                                break;
                            case 1: //Cyan laser
                                timer2 = 180;
                                break;
                            case 2: //Pink laser
                                timer2 = 180;
                                break;
                            case 3: //Purple laser
                                timer2 = 150;
                                break;
                        }
                    }
                    else
                    {
                        switch (npc.ai[1])
                        {
                            case 0: //Blue laser
                                timer2 = 90;
                                break;
                            case 1: //Cyan laser
                                timer2 = 120;
                                break;
                            case 2: //Pink laser
                                timer2 = 120;
                                break;
                            case 3: //Purple laser
                                timer2 = 90;
                                break;
                        }
                    }
                }
                #endregion

                #region Shooting lasers
                if (npc.ai[2] == 0) timer2--;
                if (timer2 <= 0 && npc.ai[2] == 0 && cyanLaserShots < 3)
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
                            timer2 = 20;
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
                                Projectile projectile4 = Projectile.NewProjectileDirect(eyePos, (Vector2.Normalize(target.Center - npc.Center)).RotatedBy(i / 6f) * 4, ModContent.ProjectileType<SpireLaser>(), npc.damage / 3, 0f, default, 0, 4);
                                EEMod.primitives.CreateTrail(new SpirePrimTrail(projectile4, Color.Purple, 30));
                            }
                            Main.PlaySound(SoundID.DD2_LightningBugDeath.SoundId, npc.Center, 2);
                            npc.ai[2] = 1;
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

                        if (Vector2.Distance(laser.Center, npc.Center) <= 56 && laser.ai[0] > 0 && laser.active)
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
            }

            else
            {
                #region Waking up
                if (firstAwakening)
                {
                    timer1 = 300 * 60;
                    for(int i = 0; i < Main.maxTilesX; i++)
                    {
                        for (int j = 0; j < Main.maxTilesY; j++)
                        {
                            if (Main.tile[i, j].type == ModContent.TileType<AquamarineLamp1>() && Main.tile[i, j].frameX == 0 && Main.tile[i, j].frameY == 0)
                            {
                                Projectile proj = Projectile.NewProjectileDirect(new Vector2((i * 16) + 16, (j * 16)), Vector2.Zero, ModContent.ProjectileType<AquamarineLamp1Glow>(), 0, 0, default, 0, 0);
                                shields.Add(proj);
                            }
                        }
                    }

                    for (int i = 0; i < shields.Count; i++)
                    {
                        shields[i].ai[0] = shields.Count;
                        shields[i].ai[1] = i;
                    }

                    firstAwakening = false;
                }

                if (timer1 >= 300 * 60)
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
                    npc.ai[0] -= 3;
                    strikeColor = Color.Blue;
                    break;
                case 2: //Cyan
                    npc.ai[0] -= 2;
                    strikeColor = Color.Cyan;
                    break;
                case 3: //Pink
                    npc.ai[0] -= 1;
                    strikeColor = Color.Magenta;
                    break;
                case 4: //Purple
                    npc.ai[0] -= 2;
                    strikeColor = Color.Purple;
                    break;
            }
            strikeTime = 60;

            proj.Kill();
        } //Called when spire takes damage

        private void WakeUp()
        {
            awake = true;
            npc.ai[0] = 40;
            timer1 = 0;
            npc.ai[1] = 0;
            npc.ai[2] = 0;
            timer2 = 180;
        } //Called on spire awakening

        private void Die()
        {
            awake = false;
            playerHits = 0;
            npc.ai[0] = 0;
            timer1 = 0;
            npc.ai[1] = 0;
            npc.ai[2] = 0;
            timer2 = 0;
        } //Called on spire death
    }
}