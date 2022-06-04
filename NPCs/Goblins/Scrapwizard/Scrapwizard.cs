using EEMod.Extensions;
using EEMod.Prim;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Tiles.Furniture.GoblinFort;
using System.Collections.Generic;

namespace EEMod.NPCs.Goblins.Scrapwizard
{
    [AutoloadBossHead]
    public class Scrapwizard : EENPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Goblin Scrapwizard");
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;

            NPC.HitSound = SoundID.NPCHit40;
            NPC.DeathSound = SoundID.NPCDeath42;

            NPC.alpha = 0;

            NPC.lifeMax = 3300;
            NPC.defense = 10;

            NPC.width = 60;
            NPC.height = 70;

            NPC.friendly = false;

            NPC.damage = 20;

            NPC.knockBackResist = 0.9f;

            NPC.noGravity = true;

            NPC.boss = true;

            NPC.BossBar = ModContent.GetInstance<ScrapwizardHealthBar>();
        }

        public override bool CheckActive()
        {
            return false;
        }

        public Rectangle myRoom;

        public int attackDelay;

        public int currentAttack;

        public bool mountedOnBrute;
        public GuardBrute myGuard;
        public bool bruteDead;
        public float guardShield;

        public bool fightBegun;

        public float initialPosX;

        public List<Projectile> chandeliers;
        public int resetVal;

        public List<Projectile> tables;
        public int tableTicks;

        public List<Projectile> scrapbits;
        public List<Projectile> activeScrap;

        public float teleportFloat;

        public bool threeSwing;
        public Projectile potChandelier;
        public float swingDir;
        public bool nextToTheLeft;

        public Projectile attackChandelier;
        public Vector2 attackVector;

        public Vector2 archivePoint1;
        public Vector2 archivePoint2;
        public Vector2 archivePoint3;

        public ref float myGuardID => ref NPC.ai[0];
        public ref float attackTimer => ref NPC.ai[1];
        public ref float activeChandelierID => ref NPC.ai[2];
        public ref float chandelierSwingTimer => ref NPC.ai[3];

        public override void AI()
        {
            NPC.TargetClosest();
            Player target = Main.player[NPC.target];

            if (target.Center.X < NPC.Center.X) NPC.direction = -1;
            else NPC.direction = 1;
            NPC.spriteDirection = NPC.direction;

            if (!bruteDead) //Phase 1
            {
                NPC.dontTakeDamage = true;

                #region Determining gravity
                if (mountedOnBrute)
                {
                    NPC.Center = myGuard.NPC.Center + new Vector2(-40 * myGuard.NPC.spriteDirection, -32);
                    NPC.rotation = 0f;
                }
                else
                {
                    NPC.velocity.Y += 0.48f; //Force of gravity
                }
                #endregion

                #region Starting the fight
                if (!fightBegun)
                {
                    #region Initializing
                    if (NPC.ai[1] == 0)
                    {
                        myGuard = Main.npc[(int)NPC.ai[0]].ModNPC as GuardBrute;

                        myRoom = myGuard.myRoom;

                        NPC.Center = new Vector2(myRoom.X + (56 * 16), myRoom.Y + (20 * 16));

                        NPC.ai[1]++; //Ticker
                    }
                    #endregion

                    if (NPC.ai[1] > 1)
                    {
                        NPC.ai[1]++;

                        NPC.rotation += NPC.velocity.X / 4f;
                    }

                    if ((Vector2.DistanceSquared(target.Center, NPC.Center) <= 320 * 320 || myGuard.NPC.life < myGuard.NPC.lifeMax) && NPC.ai[1] == 1)
                    {
                        //SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot("EEMod/Assets/Sounds/goblinlaugh2"));

                        NPC.velocity.X = (myGuard.NPC.Center.X - (NPC.Center.X - (-40 * myGuard.NPC.direction))) / 60f;

                        NPC.velocity.Y = -15f;
                        NPC.ai[1]++;
                    }

                    if(NPC.ai[1] >= 60)
                    {
                        NPC.ai[1] = 0;
                        fightBegun = true;
                        mountedOnBrute = true;
                        NPC.velocity = Vector2.Zero;
                        currentAttack = 0;
                        myGuard.fightBegun = true;

                        chandeliers = new List<Projectile>();

                        for(int i = 0; i < Main.maxProjectiles; i++)
                        {
                            if(Main.projectile[i].type == ModContent.ProjectileType<GoblinChandelierLight>())
                            {
                                chandeliers.Add(Main.projectile[i]);
                            }
                        }

                        //SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot("EEMod/Assets/Sounds/goblingrr"));
                    }
                }
                #endregion

                else
                {
                    switch (currentAttack)
                    {
                        case 0: //teleports the brute behind the player and then does a rapid slam
                            //less than a second to teleport

                            if(NPC.ai[1] < 40)
                            {

                            }

                            else if(NPC.ai[1] == 40)
                            {

                            }

                            //less than a second to telegraph
                            else if (NPC.ai[1] < 80)
                            {

                            }

                            else if (NPC.ai[1] == 80)
                            {
                                //SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot("EEMod/Assets/Sounds/goblingrunt2"));
                            }

                            //SLAM
                            else if (NPC.ai[1] < 120)
                            {

                            }

                            //next attack
                            else
                            {
                                PickNewAttack(true);
                            }
                            break;
                        case 1: //Brute leaps towards the player, and does a punch down when above, and slams fist into the ground so he gets stuck briefly,
                                //sends out shadowflame fire ripples to either side of him
                            //less than a second to teleport
                            if (NPC.ai[1] % 80 < 40)
                            {

                            }

                            else if (NPC.ai[1] % 80 == 40)
                            {
                                //warp
                            }

                            else
                            {
                                if(myGuard.NPC.velocity.Y <= 0 && NPC.ai[1] >= 80)
                                {
                                    PickNewAttack(true);
                                }
                            }

                            //throws potion down and explodes into flames

                            //slams down and reteleports, repeat twice more

                            //next attack
                            break;
                        case 2: //teleports to one corner of the arena, punches rapidly sending out shadowflame shockwaves, and gets stuck at the end
                            if(NPC.ai[1] > 200)
                            {
                                PickNewAttack(true);
                            }
                            break;
                        case 3: //Scrapwizard shoots hard-hitting and fast shadowflame bolts(not hitscan) at the player while brute moves slowly towards the player
                            break;
                    }

                    NPC.ai[1]++;
                }

                guardShield = (float)myGuard.NPC.life / (float)myGuard.NPC.lifeMax;
            }
            else //Phase 2
            {
                tableTicks++;

                guardShield = 0f;

                NPC.dontTakeDamage = false;

                NPC.knockBackResist = 0f;

                #region Initializing
                if (!fightBegun)
                {
                    NPC.velocity.Y += 0.48f;

                    if (NPC.ai[1] < 40)
                    {
                        if (teleportFloat < 1) teleportFloat += 1 / 40f;
                    }
                    else if (NPC.ai[1] == 40)
                    {
                        NPC.Center = myRoom.Center.ToVector2() + new Vector2(0, 13 * 16);

                        int tableIndex = 0;

                        tables = new List<Projectile>();

                        for (int i = 0; i < Main.maxTilesX; i++)
                        {
                            for (int j = 0; j < Main.maxTilesY; j++)
                            {
                                if (WorldGen.InWorld(i, j) && Framing.GetTileSafely(i, j).TileType == ModContent.TileType<GoblinBanquetTable>() &&
                                    Framing.GetTileSafely(i, j).TileFrameX == 0 && Framing.GetTileSafely(i, j).TileFrameY == 0)
                                {
                                    int newTable = Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_Parent(NPC), new Vector2(i * 16, j * 16) + new Vector2(80, 16), Vector2.Zero, ModContent.ProjectileType<PhantomTable>(), 0, 0, ai0: 0, ai1: tableIndex);

                                    Main.projectile[newTable].ai[1] = tableIndex;

                                    tables.Add(Main.projectile[newTable]);

                                    tableIndex++;
                                }
                            }
                        }

                        scrapbits = new List<Projectile>();
                        activeScrap = new List<Projectile>();

                        for (int i = 0; i < 6; i++)
                        {
                            scrapbits.Add(Projectile.NewProjectileDirect(new Terraria.DataStructures.EntitySource_Parent(NPC), NPC.Center, Vector2.Zero, ModContent.ProjectileType<LargeScrap>(), 0, 0));
                        }
                    }
                    else if (NPC.ai[1] < 80)
                    {
                        if (teleportFloat > 0) teleportFloat -= 1 / 40f;
                    }
                    else if (NPC.ai[1] < 100) { }
                    else if (NPC.ai[1] == 100)
                    {
                        //TODO
                        //Jump up to chandeliers instead of just starting the fight

                        float dist = 100000000;
                        Projectile myProj = null;
                        for(int i = 0; i < chandeliers.Count; i++)
                        {
                            if(Vector2.Distance(NPC.Center, chandeliers[i].Center) < dist)
                            {
                                dist = Vector2.Distance(NPC.Center, chandeliers[i].Center);
                                myProj = chandeliers[i];
                            }
                        }

                        NPC.ai[2] = myProj.whoAmI;

                        NPC.velocity = new Vector2((myProj.Center.X - NPC.Center.X) / 60f, -22.2f);

                        //SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot("EEMod/Assets/Sounds/goblincry1"));
                    }
                    else if (NPC.ai[1] < 160)
                    {
                        NPC.rotation -= 0.25f;
                    }
                    else if (NPC.ai[1] >= 160)
                    {
                        fightBegun = true;
                        NPC.ai[1] = 0;
                        NPC.ai[3] = 0;
                        currentAttack = 0;

                        NPC.velocity.Y = 0;
                        NPC.velocity.X = 0;

                        NPC.rotation = 0f;

                        for(int i = 1; i < 80; i++)
                        {
                            Vector2 flamePos = Vector2.Lerp(myRoom.BottomLeft(), myRoom.BottomRight(), i / 80f) + new Vector2(0, -8);

                            Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_Parent(NPC), flamePos, Vector2.Zero, ModContent.ProjectileType<Shadowfire>(), 0, 0);
                        }
                    }

                    if(NPC.ai[1] >= 40)
                    {
                        foreach (Projectile table in tables)
                        {
                            (table.ModProjectile as PhantomTable).oldCenter = (table.ModProjectile as PhantomTable).desiredCenter;
                            (table.ModProjectile as PhantomTable).desiredCenter =
                                myRoom.Center.ToVector2() + new Vector2(0, 128) +
                                new Vector2(
                                    (float)Math.Cos(((tableTicks + (700 * 3.14f)) / 700f) + ((table.ai[1]) * (MathHelper.Pi / 3.5f))) * 600f,
                                    (float)Math.Sin(((tableTicks + (350 * 3.14f)) / 350f) + ((table.ai[1]) * (MathHelper.TwoPi / 3.5f))) * 100f);
                            (table.ModProjectile as PhantomTable).falseVelocity = ((table.ModProjectile as PhantomTable).desiredCenter - (table.ModProjectile as PhantomTable).desiredCenter);
                        }
                    }

                    NPC.ai[1]++;
                }
                #endregion
                else
                {
                    #region Table management
                    foreach (Projectile table in tables)
                    {
                        (table.ModProjectile as PhantomTable).oldCenter = (table.ModProjectile as PhantomTable).desiredCenter;
                        (table.ModProjectile as PhantomTable).desiredCenter =
                            myRoom.Center.ToVector2() + new Vector2(0, 128) +
                            new Vector2(
                                    (float)Math.Cos(((tableTicks + (700 * 3.14f)) / 700f) + ((table.ai[1]) * (MathHelper.Pi / 3.5f))) * 600f,
                                    (float)Math.Sin(((tableTicks + (350 * 3.14f)) / 350f) + ((table.ai[1]) * (MathHelper.TwoPi / 3.5f))) * 100f);
                        (table.ModProjectile as PhantomTable).falseVelocity = ((table.ModProjectile as PhantomTable).desiredCenter - (table.ModProjectile as PhantomTable).desiredCenter);
                    }
                    #endregion

                    #region Attacking logic
                    switch (currentAttack)
                    {
                        case 0: //turns all the chandelier flames but the one he's on into shadowflame, telegraph beams
                            if (NPC.ai[1] % 100 == 1) 
                            {
                                List<Projectile> potChandeliers = new List<Projectile>();

                                foreach(Projectile chandelier in chandeliers)
                                {
                                    if(Vector2.Distance(chandelier.Center, target.Center) <= 40 * 16)
                                    {
                                        potChandeliers.Add(chandelier);
                                    }
                                }

                                attackChandelier = potChandeliers[Main.rand.Next(0, potChandeliers.Count)];
                                attackVector = target.Center;
                            }
                            else if (NPC.ai[1] % 100 < 40)
                            {
                                attackVector = target.Center;
                            }
                            else if (NPC.ai[1] % 100 == 40)
                            {
                                //lock player's position
                                attackVector = target.Center;

                                int projOne = Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_Parent(NPC), (attackChandelier.ModProjectile as GoblinChandelierLight).trails[0].startPoint, (Vector2.Normalize(attackVector - (attackChandelier.ModProjectile as GoblinChandelierLight).trails[0].startPoint) * 20f), ModContent.ProjectileType<ChandelierBolt>(), 0, 0);
                                int projTwo = Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_Parent(NPC), (attackChandelier.ModProjectile as GoblinChandelierLight).trails[3].startPoint, (Vector2.Normalize(attackVector - (attackChandelier.ModProjectile as GoblinChandelierLight).trails[3].startPoint) * 20f), ModContent.ProjectileType<ChandelierBolt>(), 0, 0);
                                int projThree = Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_Parent(NPC), (attackChandelier.ModProjectile as GoblinChandelierLight).trails[6].startPoint, (Vector2.Normalize(attackVector - (attackChandelier.ModProjectile as GoblinChandelierLight).trails[6].startPoint) * 20f), ModContent.ProjectileType<ChandelierBolt>(), 0, 0);

                                PrimitiveSystem.primitives.CreateTrail(new ShadowflamePrimTrail(Main.projectile[projOne], Color.DarkViolet, 6, 8, true));
                                PrimitiveSystem.primitives.CreateTrail(new ShadowflamePrimTrail(Main.projectile[projOne], Color.DarkViolet * 0.5f, 10, 8));

                                PrimitiveSystem.primitives.CreateTrail(new ShadowflamePrimTrail(Main.projectile[projTwo], Color.DarkViolet, 6, 8, true));
                                PrimitiveSystem.primitives.CreateTrail(new ShadowflamePrimTrail(Main.projectile[projTwo], Color.DarkViolet * 0.5f, 10, 8));

                                PrimitiveSystem.primitives.CreateTrail(new ShadowflamePrimTrail(Main.projectile[projThree], Color.DarkViolet, 6, 8, true));
                                PrimitiveSystem.primitives.CreateTrail(new ShadowflamePrimTrail(Main.projectile[projThree], Color.DarkViolet * 0.5f, 10, 8));
                            }
                            else if (NPC.ai[1] % 100 < 99) 
                            {
                                (attackChandelier.ModProjectile as GoblinChandelierLight).hideFlames = true;
                            }
                            else
                            {
                                (attackChandelier.ModProjectile as GoblinChandelierLight).hideFlames = false;
                            }

                            if(NPC.ai[1] >= 300)
                            {
                                PickNewAttack(false);
                            }

                            break;
                        case 1: //combines the chandelier flames into one big fireball each and casts them down with a meteor fashion towards the player
                            if (NPC.ai[1] % 100 == 0)
                            {
                                List<Projectile> potChandeliers = new List<Projectile>();

                                foreach (Projectile chandelier in chandeliers)
                                {
                                    if (Vector2.Distance(chandelier.Center, target.Center) <= 40 * 16)
                                    {
                                        potChandeliers.Add(chandelier);
                                    }
                                }

                                attackChandelier = potChandeliers[Main.rand.Next(0, potChandeliers.Count)];
                                attackVector = target.Center;
                            }
                            else if (NPC.ai[1] % 100 < 48)
                            {
                                (attackChandelier.ModProjectile as GoblinChandelierLight).flameDist--;

                                attackVector = target.Center;
                            }
                            else if (NPC.ai[1] % 100 == 60)
                            {
                                //lock player's position
                                attackVector = target.Center;

                                (attackChandelier.ModProjectile as GoblinChandelierLight).hideFlames = true;

                                Vector2 velocity = Vector2.Normalize(target.Center - attackChandelier.Center) * 16f;

                                int projOne = Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_Parent(NPC), attackChandelier.Center, velocity, ModContent.ProjectileType<ChandelierMeteor>(), 0, 0);
                                
                                PrimitiveSystem.primitives.CreateTrail(new ShadowflamePrimTrail(Main.projectile[projOne], Color.DarkViolet, 12, 14, true));
                                PrimitiveSystem.primitives.CreateTrail(new ShadowflamePrimTrail(Main.projectile[projOne], Color.DarkViolet * 0.5f, 18, 14));
                            }
                            else if (NPC.ai[1] % 100 < 99)
                            {

                            }
                            else
                            {
                                (attackChandelier.ModProjectile as GoblinChandelierLight).flameDist = 48;
                                (attackChandelier.ModProjectile as GoblinChandelierLight).hideFlames = false;
                            }

                            if (NPC.ai[1] >= 300)
                            {
                                PickNewAttack(false);
                            }

                            break;
                        case 3: //lengthens a chandelier and leans down to throw more shadowflame molotovs at the player
                            if (NPC.ai[1] % 40 == 0)
                            {
                                Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_Parent(NPC), NPC.Center, (Vector2.Normalize(target.Center - NPC.Center) * 6f), ModContent.ProjectileType<ShadowflameJarBounce>(), 0, 0);
                            }

                            if (NPC.ai[1] == 0)
                            {
                                if (nextToTheLeft)
                                {
                                    //resetVal = (int)(NPC.ai[3] / 310) + 90;
                                    resetVal = (int)(NPC.ai[3] / 310) + 0;
                                }
                                else
                                {
                                    resetVal = (int)(NPC.ai[3] / 310) + 45;
                                }

                                (Main.projectile[(int)NPC.ai[2]].ModProjectile as GoblinChandelierLight).chainLength++;

                                (Main.projectile[(int)NPC.ai[2]].ModProjectile as GoblinChandelierLight).disabled = true;
                            }
                            if (NPC.ai[1] < 180)
                            {
                                (Main.projectile[(int)NPC.ai[2]].ModProjectile as GoblinChandelierLight).chainLength++;

                                (Main.projectile[(int)NPC.ai[2]].ModProjectile as GoblinChandelierLight).disabled = true;
                            }
                            else if (NPC.ai[1] < 360)
                            {
                                (Main.projectile[(int)NPC.ai[2]].ModProjectile as GoblinChandelierLight).disabled = true;
                            }
                            else if (NPC.ai[1] < 540) 
                            {
                                (Main.projectile[(int)NPC.ai[2]].ModProjectile as GoblinChandelierLight).chainLength--;

                                (Main.projectile[(int)NPC.ai[2]].ModProjectile as GoblinChandelierLight).disabled = true;
                            }
                            else
                            {
                                (Main.projectile[(int)NPC.ai[2]].ModProjectile as GoblinChandelierLight).chainLength = 80;

                                (Main.projectile[(int)NPC.ai[2]].ModProjectile as GoblinChandelierLight).disabled = false;

                                PickNewAttack(false);
                            }

                            break;
                        case 2: // drops a chandelier down to you rapidly and the flames explode, pulls the chandelier back up afterward, repeat 5 times - come back to this
                            if (NPC.ai[1] % 120 == 0)
                            {
                                NPC.ai[1]++;
                            }

                            if (NPC.ai[1] % 120 == 0)
                            {
                                float currDist = 1000000;
                                foreach (Projectile chandelier in chandeliers)
                                {
                                    if (Vector2.Distance(chandelier.Center, target.Center) < currDist && 
                                        (Main.projectile[(int)NPC.ai[2]] != chandelier) &&
                                        chandelier != potChandelier)
                                    {
                                        currDist = Vector2.Distance(chandelier.Center, target.Center);

                                        attackChandelier = chandelier;
                                    }
                                }

                                //pick chandelier and start drop
                            }
                            else if (NPC.ai[1] % 120 < 60)
                            {
                                (attackChandelier.ModProjectile as GoblinChandelierLight).disabled = true;

                                if ((attackChandelier.ModProjectile as GoblinChandelierLight).chainLength < 16 * 33)
                                {
                                    (attackChandelier.ModProjectile as GoblinChandelierLight).chainLength += (int)((NPC.ai[1] % 120) * 0.25f);
                                }

                                //drop chandelier
                            }
                            else if (NPC.ai[1] % 120 < 120 - 1)
                            {
                                (attackChandelier.ModProjectile as GoblinChandelierLight).disabled = true;

                                if ((attackChandelier.ModProjectile as GoblinChandelierLight).chainLength > 80)
                                {
                                    (attackChandelier.ModProjectile as GoblinChandelierLight).chainLength -= 8;
                                }

                                //reel it back up
                            }
                            else
                            {
                                (attackChandelier.ModProjectile as GoblinChandelierLight).chainLength = 80;

                                (attackChandelier.ModProjectile as GoblinChandelierLight).disabled = false;

                                //reset the chandelier
                            }
                            
                            if(NPC.ai[1] >= (120 * 5) - 1)
                            {
                                PickNewAttack(false);
                            }


                            break;
                        case 4: //railgun shoots bits of scrap at yoy that stick to tables and explode as mines        SCRAAAAPA

                            //0-40: Loading the bolts
                            //40-60: Bolts freeze
                            //60-80: Bolts fire

                            if(NPC.ai[1] == 0)
                            {
                                activeScrap.Clear();

                                while (activeScrap.Count < 4)
                                {
                                    foreach (Projectile bit in scrapbits)
                                    {
                                        if (activeScrap.Count < 4 && bit.ModProjectile is LargeScrap) activeScrap.Add(bit);
                                    }
                                }

                                (activeScrap[0].ModProjectile as LargeScrap).movementDuration = 40;
                                (activeScrap[1].ModProjectile as LargeScrap).movementDuration = 40;
                                (activeScrap[2].ModProjectile as LargeScrap).movementDuration = 40;
                                (activeScrap[3].ModProjectile as LargeScrap).movementDuration = 40;
                            }

                            if (NPC.ai[1] >= 322)
                            {
                                PickNewAttack(false);
                            }

                            if (NPC.ai[1] % 161 < 40)
                            {
                                (activeScrap[0].ModProjectile as LargeScrap).AttackPhase = 1;
                                (activeScrap[1].ModProjectile as LargeScrap).AttackPhase = 1;

                                if (NPC.ai[1] % 161 == 0)
                                {
                                    float randFloat = Main.rand.NextFloat(0, MathHelper.Pi);
                                    float randFloat2 = Main.rand.NextFloat(0, MathHelper.Pi);

                                    (activeScrap[0].ModProjectile as LargeScrap).desiredPosition = new Vector2(myRoom.Center.X, myRoom.Top) + new Vector2(0, 240) + new Vector2(myRoom.Width * 0.33f * (float)Math.Cos(randFloat), -120 * (float)Math.Sin(randFloat));
                                    (activeScrap[1].ModProjectile as LargeScrap).desiredPosition = new Vector2(myRoom.Center.X, myRoom.Top) + new Vector2(0, 240) + new Vector2(myRoom.Width * 0.33f * (float)Math.Cos(randFloat2), -120 * (float)Math.Sin(randFloat2));
                                }

                                (activeScrap[0].ModProjectile as LargeScrap).desiredRotation = (target.Center - (activeScrap[0].ModProjectile as LargeScrap).desiredPosition).ToRotation();
                                (activeScrap[1].ModProjectile as LargeScrap).desiredRotation = (target.Center - (activeScrap[1].ModProjectile as LargeScrap).desiredPosition).ToRotation();
                            }
                            else if (NPC.ai[1] % 161 < 60)
                            {
                                (activeScrap[0].ModProjectile as LargeScrap).desiredRotation = (target.Center - (activeScrap[0].ModProjectile as LargeScrap).desiredPosition).ToRotation();
                                (activeScrap[1].ModProjectile as LargeScrap).desiredRotation = (target.Center - (activeScrap[1].ModProjectile as LargeScrap).desiredPosition).ToRotation();
                            }
                            else if (NPC.ai[1] % 161 < 120 && NPC.ai[1] % 161 >= 80)
                            {
                                (activeScrap[2].ModProjectile as LargeScrap).AttackPhase = 1;
                                (activeScrap[3].ModProjectile as LargeScrap).AttackPhase = 1;

                                if (NPC.ai[1] % 161 == 80)
                                {
                                    float randFloat = Main.rand.NextFloat(0, MathHelper.Pi);
                                    float randFloat2 = Main.rand.NextFloat(0, MathHelper.Pi);

                                    (activeScrap[2].ModProjectile as LargeScrap).desiredPosition = new Vector2(myRoom.Center.X, myRoom.Top) + new Vector2(0, 240) + new Vector2(myRoom.Width * 0.33f * (float)Math.Cos(randFloat), -120 * (float)Math.Sin(randFloat));
                                    (activeScrap[3].ModProjectile as LargeScrap).desiredPosition = new Vector2(myRoom.Center.X, myRoom.Top) + new Vector2(0, 240) + new Vector2(myRoom.Width * 0.33f * (float)Math.Cos(randFloat2), -120 * (float)Math.Sin(randFloat2));
                                }

                                (activeScrap[2].ModProjectile as LargeScrap).desiredRotation = (target.Center - (activeScrap[2].ModProjectile as LargeScrap).desiredPosition).ToRotation();
                                (activeScrap[3].ModProjectile as LargeScrap).desiredRotation = (target.Center - (activeScrap[3].ModProjectile as LargeScrap).desiredPosition).ToRotation();
                            }
                            else if (NPC.ai[1] % 161 < 140)
                            {
                                (activeScrap[2].ModProjectile as LargeScrap).desiredRotation = (target.Center - (activeScrap[2].ModProjectile as LargeScrap).desiredPosition).ToRotation();
                                (activeScrap[3].ModProjectile as LargeScrap).desiredRotation = (target.Center - (activeScrap[3].ModProjectile as LargeScrap).desiredPosition).ToRotation();
                            }
                            else if (NPC.ai[1] % 161 == 160)
                            {
                                Projectile temp = activeScrap[0];
                                activeScrap[0] = activeScrap[1];
                                activeScrap[1] = temp;

                                Projectile temp2 = activeScrap[2];
                                activeScrap[2] = activeScrap[3];
                                activeScrap[3] = temp2;
                            }

                            break;
                        case 5: // scrap wall attack where you must get in a good position or you get torn up        SCRAAAAPA

                            break;
                        case 6: //morphs the scrap into a sword and starts swinging, up cut, down cut, spin attack, and final up cut and the shards fly up        SCRAAAAPA

                            break;
                    }
                    #endregion

                    #region Chandelier swinging logic
                    if (NPC.ai[3] % 310 == 0)
                    {
                        List<Projectile> potentialChandeliers = new List<Projectile>();

                        potChandelier = null;

                        foreach (Projectile chandelier in chandeliers)
                        {
                            if (Vector2.Distance(chandelier.Center, NPC.Center) < 20 * 16 && chandelier != Main.projectile[(int)NPC.ai[2]] && !(chandelier.ModProjectile as GoblinChandelierLight).disabled)
                            {
                                potentialChandeliers.Add(chandelier);
                            }
                        }

                        potChandelier = potentialChandeliers[Main.rand.Next(0, potentialChandeliers.Count)];

                        if (nextToTheLeft)
                        {
                            resetVal = (int)(NPC.ai[3] / 310) + 0;
                            NPC.ai[3] = (int)(NPC.ai[3] / 310) + 0;
                        }
                        else
                        {
                            resetVal = (int)(NPC.ai[3] / 310) + 45;
                            NPC.ai[3] = (int)(NPC.ai[3] / 310) + 45;
                        }

                        if ((potChandelier.ModProjectile as GoblinChandelierLight).anchorPos16.X < (Main.projectile[(int)NPC.ai[2]].ModProjectile as GoblinChandelierLight).anchorPos16.X)
                        {
                            nextToTheLeft = true;
                        }
                        else
                        {
                            nextToTheLeft = false;
                        }

                        GoblinChandelierLight skrunkle = (Main.projectile[(int)NPC.ai[2]].ModProjectile as GoblinChandelierLight);

                        skrunkle.axisRotation = (float)Math.Sin(((NPC.ai[3] % 310) * MathHelper.TwoPi) / 90f) * 0.5f;

                        NPC.Center = skrunkle.anchorPos16 +
                            ((Vector2.UnitY).RotatedBy(skrunkle.axisRotation)) * (skrunkle.chainLength - 40);

                        NPC.rotation = skrunkle.axisRotation;
                    }
                    else if (NPC.ai[3] % 310 < 180) //Actively swinging on a chandelier
                    {
                        GoblinChandelierLight skrunkle = (Main.projectile[(int)NPC.ai[2]].ModProjectile as GoblinChandelierLight);

                        skrunkle.axisRotation = (float)Math.Sin(((NPC.ai[3] % 310) * MathHelper.TwoPi) / 90f) * 0.5f;

                        NPC.Center = skrunkle.anchorPos16 + 
                            ((Vector2.UnitY).RotatedBy(skrunkle.axisRotation)) * (skrunkle.chainLength - 40);

                        NPC.rotation = skrunkle.axisRotation;
                    }
                    else if (NPC.ai[3] % 310 < 270) //Picking the next chandelier to swing to, and starting the jump
                    {
                        if ((nextToTheLeft && NPC.ai[3] == 180) || (!nextToTheLeft && NPC.ai[3] == 225))
                        {
                            if(potChandelier == null || (Main.projectile[(int)NPC.ai[2]].ModProjectile as GoblinChandelierLight).disabled)
                            {
                                NPC.ai[3] = resetVal;

                                GoblinChandelierLight skrunkle = (Main.projectile[(int)NPC.ai[2]].ModProjectile as GoblinChandelierLight);

                                skrunkle.axisRotation = (float)Math.Sin(((NPC.ai[3] % 310) * MathHelper.TwoPi) / 90f) * 0.5f;

                                NPC.Center = skrunkle.anchorPos16 +
                                    ((Vector2.UnitY).RotatedBy(skrunkle.axisRotation)) * (skrunkle.chainLength - 40);

                                NPC.rotation = skrunkle.axisRotation;

                                skrunkle.axisRotation = (float)Math.Sin(((NPC.ai[3] % 310) * MathHelper.TwoPi) / 90f) * 0.5f;

                                NPC.ai[3]++;

                                return;
                            }

                            //Ready to swing

                            (Main.projectile[(int)NPC.ai[2]].ModProjectile as GoblinChandelierLight).rotationVelocity = (NPC.ai[3] == 180 ? 0.05f : -0.05f);

                            NPC.ai[2] = potChandelier.whoAmI;

                            (Main.projectile[(int)NPC.ai[2]].ModProjectile as GoblinChandelierLight).rotationVelocity = 0;

                            NPC.ai[3] = 270;

                            NPC.velocity.X = (Main.projectile[(int)NPC.ai[2]].Center.X - NPC.Center.X) / 40f;
                            NPC.velocity.Y = -2f;
                        }
                        else
                        {
                            GoblinChandelierLight skrunkle = (Main.projectile[(int)NPC.ai[2]].ModProjectile as GoblinChandelierLight);

                            skrunkle.axisRotation = (float)Math.Sin(((NPC.ai[3] % 310) * MathHelper.TwoPi) / 90f) * 0.5f;

                            NPC.Center = skrunkle.anchorPos16 +
                                ((Vector2.UnitY).RotatedBy(skrunkle.axisRotation)) * (skrunkle.chainLength - 40);

                            NPC.rotation = skrunkle.axisRotation;
                        }
                    }
                    else if (NPC.ai[3] % 310 < 310) //In midair
                    {
                        NPC.velocity.Y += 2 / 20f;
                        NPC.rotation += NPC.velocity.X / 15f;
                        NPC.spriteDirection = (NPC.velocity.X < 0 ? -1 : 1);
                    }
                    #endregion

                    NPC.ai[3]++;
                    NPC.ai[1]++;
                }
            }
        }

        public void PickNewAttack(bool phase1)
        {
            if (phase1)
            {
                myGuard.frameY = 0;
                currentAttack = Main.rand.Next(6);
                myGuard.NPC.velocity = Vector2.Zero;
            }
            else
                currentAttack = Main.rand.Next(5);

            NPC.ai[1] = 0;
        }

        public void Trigger()
        {
            NPC.ai[1] = 0;
            bruteDead = true;
            fightBegun = false;
        }

        public void InitShader(SpriteBatch spriteBatch)
        {
            spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, default, Main.GameViewMatrix.ZoomMatrix);

            EEMod.ShadowWarp.Parameters["noise"].SetValue(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/Noise/noise").Value);
            EEMod.ShadowWarp.Parameters["newColor"].SetValue(new Vector4(Color.Violet.R, Color.Violet.G, Color.Violet.B, Color.Violet.A) / 255f);
            EEMod.ShadowWarp.Parameters["lerpVal"].SetValue(1 - MathHelper.Clamp((bruteDead ? teleportFloat : myGuard.teleportFloat), 0f, 1f));
            EEMod.ShadowWarp.Parameters["baseColor"].SetValue(new Vector4(Color.White.R, Color.White.G, Color.White.B, Color.White.A) / 255f);

            EEMod.ShadowWarp.CurrentTechnique.Passes[0].Apply();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            InitShader(spriteBatch);

            Rectangle rect = new Rectangle(0, 0, 60, 70);

            Texture2D tex = ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/Scrapwizard").Value;

            spriteBatch.Draw(tex, NPC.Center - Main.screenPosition, rect, Color.White, NPC.rotation, tex.TextureCenter(), 1f, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            
            Main.spriteBatch.End(); Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, default, Main.GameViewMatrix.ZoomMatrix);

            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            InitShader(spriteBatch);

            Texture2D tex = ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/ScrapwizardStaff").Value;

            //spriteBatch.Draw(tex, NPC.Center - Main.screenPosition + new Vector2((NPC.spriteDirection == 1 ? -6 : 6), 0).RotatedBy(NPC.rotation), null, Color.White, NPC.rotation, tex.Size() / 2f, 1f, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);


            Texture2D tex2 = ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/ScrapwizardArm").Value;

            //spriteBatch.Draw(tex2, NPC.Center - Main.screenPosition, null, Color.White, NPC.rotation, tex2.Size() / 2f, 1f, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            if (bruteDead && fightBegun)
            {
                if (currentAttack == 0 && (NPC.ai[1] % 100) > 1 && (NPC.ai[1] % 100) <= 40)
                {
                    spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, null, null, null, Main.GameViewMatrix.ZoomMatrix);

                    Texture2D telegraphTex = ModContent.Request<Texture2D>("EEMod/Textures/TelegraphLine").Value;

                    Point pos = (attackVector - Main.screenPosition).ToPoint();

                    spriteBatch.Draw(telegraphTex, new Rectangle(pos.X, pos.Y, 10, (int)Vector2.Distance((attackChandelier.ModProjectile as GoblinChandelierLight).trails[0].startPoint, attackVector)), null, Color.Pink * MathHelper.Clamp(1 + ((20 - (NPC.ai[1] % 100)) / 20f), 0, 1) * 0.75f, ((attackChandelier.ModProjectile as GoblinChandelierLight).trails[0].startPoint - attackVector).ToRotation() - (MathHelper.Pi / 2f), new Vector2(37 / 2f, 0), SpriteEffects.None, 0f);
                    spriteBatch.Draw(telegraphTex, new Rectangle(pos.X, pos.Y, 10, (int)Vector2.Distance((attackChandelier.ModProjectile as GoblinChandelierLight).trails[3].startPoint, attackVector)), null, Color.Pink * MathHelper.Clamp(1 + ((20 - (NPC.ai[1] % 100)) / 20f), 0, 1) * 0.75f, ((attackChandelier.ModProjectile as GoblinChandelierLight).trails[3].startPoint - attackVector).ToRotation() - (MathHelper.Pi / 2f), new Vector2(37 / 2f, 0), SpriteEffects.None, 0f);
                    spriteBatch.Draw(telegraphTex, new Rectangle(pos.X, pos.Y, 10, (int)Vector2.Distance((attackChandelier.ModProjectile as GoblinChandelierLight).trails[6].startPoint, attackVector)), null, Color.Pink * MathHelper.Clamp(1 + ((20 - (NPC.ai[1] % 100)) / 20f), 0, 1) * 0.75f, ((attackChandelier.ModProjectile as GoblinChandelierLight).trails[6].startPoint - attackVector).ToRotation() - (MathHelper.Pi / 2f), new Vector2(37 / 2f, 0), SpriteEffects.None, 0f);
                }
                if (currentAttack == 1 && (NPC.ai[1] % 100) > 48 && (NPC.ai[1] % 100) <= 60)
                {
                    spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, null, null, null, Main.GameViewMatrix.ZoomMatrix);

                    Texture2D godrayTex = ModContent.Request<Texture2D>("EEMod/Textures/GodrayMask").Value;

                    Vector2 pos = (attackChandelier.Center - Main.screenPosition);

                    spriteBatch.Draw(godrayTex, pos, null, Color.Pink, Main.GameUpdateCount / 15f, godrayTex.TextureCenter(), 0.33f * Math.Sin(((NPC.ai[1] % 100) - 48) * MathHelper.Pi / 12f).PositiveSin(), SpriteEffects.None, 0f);
                }
            }

            spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, default, Main.GameViewMatrix.ZoomMatrix);
        }

        public override void OnKill()
        {
            if(bruteDead)
            {
                (Main.projectile[(int)NPC.ai[2]].ModProjectile as GoblinChandelierLight).rotationVelocity = -(Main.projectile[(int)NPC.ai[2]].ModProjectile as GoblinChandelierLight).axisRotation * 0.05f;
            }

            foreach (Projectile table in tables)
            {
                (table.ModProjectile as PhantomTable).dyingTicks++;
            }

            foreach (Projectile chandelier in chandeliers)
            {
                (chandelier.ModProjectile as GoblinChandelierLight).retracting = true;
            }

            base.OnKill();
        }
    }
}