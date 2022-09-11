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
using EEMod.Systems;
using EEMod.NPCs.Goblins.Shaman;

namespace EEMod.NPCs.Goblins.Scrapwizard
{
    [AutoloadBossHead]
    public class Scrapwizard : EENPC
    {
        public Rectangle myRoom;

        public GuardBrute myGuard;
        public float guardShield;

        public int fightPhase;

        public List<Projectile> chandeliers = new List<Projectile>();
        public int resetVal;

        public List<Projectile> tables;
        public int tableTicks;

        public List<Projectile> scrapbits;
        public List<Projectile> activeScrap;

        public List<int> slingshotBolts = new List<int>();

        public float teleportFloat;

        public bool threeSwing;
        public Projectile potChandelier;
        public float swingDir;
        public bool nextToTheLeft;

        public Projectile attackChandelier;
        public Vector2 attackVector;

        public Vector2 swordOrig;
        public float swordRot;

        public Vector2 staffPos;
        public Vector2 targetOldPos;
        public Vector2 targetOldPosOld;
        public Vector2 targetOldPosNew;

        public ref float currentAttack => ref NPC.ai[0];
        public ref float attackTimer => ref NPC.ai[1];
        public ref float activeChandelierID => ref NPC.ai[2];
        public ref float chandelierSwingTimer => ref NPC.ai[3];


        public Vector2 staffVelocity;
        public Vector2 oldSwordOrig;
        public bool staffReturned;


        Easing.CurveSegment anticipation = new Easing.CurveSegment(Easing.EasingType.ExpOut, 0f, 0f, -0.15f);
        Easing.CurveSegment swingback = new Easing.CurveSegment(Easing.EasingType.ExpIn, 0.15f, -0.15f, 0.15f);
        Easing.CurveSegment slash = new Easing.CurveSegment(Easing.EasingType.ExpOut, 0.3f, 0f, 1.3f);

        //Easing.CurveSegment anticipation2 = new Easing.CurveSegment(Easing.EasingType.ExpOut, 0f, 0f, -0.1f);
        Easing.CurveSegment swingback2 = new Easing.CurveSegment(Easing.EasingType.ExpIn, 0f, 1.3f, -0.1f);
        Easing.CurveSegment slash2 = new Easing.CurveSegment(Easing.EasingType.ExpOut, 0.15f, 1f, -1.1f);

        //Easing.CurveSegment anticipation3 = new Easing.CurveSegment(Easing.EasingType.ExpOut, 0f, 0f, -0.1f);
        Easing.CurveSegment slash3 = new Easing.CurveSegment(Easing.EasingType.PolyIn, 0f, -0.1f, 3.3f, 2);


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

        public override void AI()
        {
            NPC.TargetClosest();
            Player target = Main.player[NPC.target];

            if (target.Center.X < NPC.Center.X) NPC.direction = -1;
            else NPC.direction = 1;
            NPC.spriteDirection = NPC.direction;

            if (fightPhase == 0) //jumping onto brute cutscene
            {
                NPC.dontTakeDamage = true;

                guardShield = (float)myGuard.NPC.life / (float)myGuard.NPC.lifeMax;

                NPC.velocity.Y += 0.48f; //Force of gravity

                if (scrapbits == null)
                {
                    scrapbits = new List<Projectile>();

                    for (int i = 0; i < 5; i++)
                    {
                        scrapbits.Add(Projectile.NewProjectileDirect(new Terraria.DataStructures.EntitySource_Parent(NPC), new Vector2(myRoom.Center.X, myRoom.Bottom - 24) + new Vector2(104 - (10 * Math.Abs(i - 2)), 0).RotatedBy(-MathHelper.PiOver2 + ((MathHelper.Pi / 4f) * ((i - 2) / 4f))), Vector2.Zero, ModContent.ProjectileType<LargeScrap>(), 0, 0, default, -1, 0));
                        scrapbits[i].rotation = -MathHelper.PiOver2 + ((MathHelper.Pi / 6f) * ((i - 2) / 4f));
                    }

                    scrapbits.Add(Projectile.NewProjectileDirect(new Terraria.DataStructures.EntitySource_Parent(NPC), new Vector2(myRoom.Center.X, myRoom.Bottom - 46), Vector2.Zero, ModContent.ProjectileType<CenterScrap>(), 0, 0));

                    scrapbits.Add(Projectile.NewProjectileDirect(new Terraria.DataStructures.EntitySource_Parent(NPC), new Vector2(myRoom.Center.X - 34, myRoom.Bottom - 48), Vector2.Zero, ModContent.ProjectileType<ArmrestScrap>(), 0, 0, default, 1, 0));
                    scrapbits.Add(Projectile.NewProjectileDirect(new Terraria.DataStructures.EntitySource_Parent(NPC), new Vector2(myRoom.Center.X + 34, myRoom.Bottom - 48), Vector2.Zero, ModContent.ProjectileType<ArmrestScrap>(), 0, 0, default, 0, 0));

                    scrapbits.Add(Projectile.NewProjectileDirect(new Terraria.DataStructures.EntitySource_Parent(NPC), new Vector2(myRoom.Center.X, myRoom.Bottom - 28), Vector2.Zero, ModContent.ProjectileType<SmallScrap>(), 0, 0));

                    scrapbits.Add(Projectile.NewProjectileDirect(new Terraria.DataStructures.EntitySource_Parent(NPC), new Vector2(myRoom.Center.X - 30, myRoom.Bottom - 12), Vector2.Zero, ModContent.ProjectileType<SmallScrap>(), 0, 0));
                    scrapbits[9].rotation = MathHelper.PiOver2;

                    scrapbits.Add(Projectile.NewProjectileDirect(new Terraria.DataStructures.EntitySource_Parent(NPC), new Vector2(myRoom.Center.X + 30, myRoom.Bottom - 12), Vector2.Zero, ModContent.ProjectileType<SmallScrap>(), 0, 0));
                    scrapbits[10].rotation = MathHelper.PiOver2;

                    foreach (Projectile scrapbit in scrapbits)
                    {
                        if (scrapbit.ModProjectile is LargeScrap)
                        {
                            (scrapbit.ModProjectile as LargeScrap).offset = scrapbit.Center - new Vector2(myRoom.Center.X, myRoom.Bottom - 46);
                        }
                        else if (scrapbit.ModProjectile is SmallScrap)
                            (scrapbit.ModProjectile as SmallScrap).offset = scrapbit.Center - new Vector2(myRoom.Center.X, myRoom.Bottom - 46);
                        else if (scrapbit.ModProjectile is CenterScrap)
                            (scrapbit.ModProjectile as CenterScrap).offset = scrapbit.Center - new Vector2(myRoom.Center.X, myRoom.Bottom - 46);
                        else if (scrapbit.ModProjectile is ArmrestScrap)
                            (scrapbit.ModProjectile as ArmrestScrap).offset = scrapbit.Center - new Vector2(myRoom.Center.X, myRoom.Bottom - 46);
                    }

                    foreach (Projectile scrapbit in scrapbits)
                    {
                        if (scrapbit.ModProjectile is LargeScrap)
                            (scrapbit.ModProjectile as LargeScrap).initRotation = scrapbit.rotation;
                        else if (scrapbit.ModProjectile is SmallScrap)
                            (scrapbit.ModProjectile as SmallScrap).initRotation = scrapbit.rotation;
                        else if (scrapbit.ModProjectile is CenterScrap)
                            (scrapbit.ModProjectile as CenterScrap).initRotation = scrapbit.rotation;
                        else if (scrapbit.ModProjectile is ArmrestScrap)
                            (scrapbit.ModProjectile as ArmrestScrap).initRotation = scrapbit.rotation;
                    }

                    activeScrap = new List<Projectile>();
                }

                if ((Vector2.DistanceSquared(target.Center, NPC.Center) <= 320 * 320 || myGuard.NPC.life < myGuard.NPC.lifeMax) && attackTimer == 0)
                {
                    NPC.velocity.X = (myGuard.NPC.Center.X - (NPC.Center.X - (-40 * myGuard.NPC.direction))) / 60f;

                    NPC.velocity.Y = -15f;

                    attackTimer++;
                }

                if (attackTimer >= 1)
                {
                    NPC.rotation += NPC.velocity.X / 4f;

                    attackTimer++;
                }
                if (attackTimer >= 60)
                {
                    attackTimer = 0;
                    fightPhase = 1;
                    NPC.velocity = Vector2.Zero;
                    currentAttack = 0;
                    myGuard.fightBegun = true;

                    chandeliers = new List<Projectile>();

                    for (int i = 0; i < Main.maxProjectiles; i++)
                    {
                        if (Main.projectile[i].type == ModContent.ProjectileType<GoblinChandelierLight>())
                        {
                            chandeliers.Add(Main.projectile[i]);
                        }
                    }
                }
            }
            else if (fightPhase == 1) //phase 1
            {
                switch (currentAttack)
                {
                    case 0:
                        if (attackTimer < 40)
                        {
                            teleportFloat += 1 / 40f;
                        }

                        else if (attackTimer == 40)
                        {
                            myGuard.NPC.Center = new Vector2(target.Center.X - (target.direction * 80), myRoom.Bottom - myGuard.NPC.height  / 2f);
                        }

                        //less than a second to telegraph
                        else if (attackTimer < 80)
                        {
                            teleportFloat -= 1 / 40f;

                            if (attackTimer >= 60)
                            {
                                if (attackTimer == 60) myGuard.frameY = 0;

                                if (attackTimer % 4 == 0)
                                {
                                    myGuard.frameY++;
                                }
                            }

                        }

                        else if (attackTimer == 80)
                        {
                            teleportFloat = 0f;

                            //Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_Parent(NPC), myGuard.NPC.Bottom, Vector2.Zero, ModContent.ProjectileType<FlameSpiral>(), 0, 0);

                            if (target.Center.X < NPC.Center.X)
                            {
                                Projectile spike1 = Projectile.NewProjectileDirect(new Terraria.DataStructures.EntitySource_Parent(NPC), new Vector2(myGuard.NPC.Center.X - 124 + 62 - 30, myRoom.Bottom - (168 / 2)), Vector2.Zero, ModContent.ProjectileType<FlameSpiral>(), 0, 0);
                                spike1.spriteDirection = 1;
                            }
                            else
                            {
                                Projectile spike1 = Projectile.NewProjectileDirect(new Terraria.DataStructures.EntitySource_Parent(NPC), new Vector2(myGuard.NPC.Center.X + 62 + 30, myRoom.Bottom - (168 / 2)), Vector2.Zero, ModContent.ProjectileType<FlameSpiral>(), 0, 0);
                                spike1.spriteDirection = -1;
                            }

                            //SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot("EEMod/Assets/Sounds/goblingrunt2"));
                        }

                        else if (attackTimer < 100)
                        {

                        }

                        //next attack
                        else
                        {
                            PickNewAttack(true);
                        }
                        break;
                    case 1:
                        if (attackTimer < 40)
                        {
                            if (attackTimer >= 20)
                            {
                                if (attackTimer % 4 == 0)
                                {
                                    myGuard.frameY++;
                                }
                            }

                            //telegraph
                        }
                        else if (attackTimer == 40)
                        {
                            myGuard.NPC.velocity.Y -= 22f;
                            myGuard.NPC.velocity.X =  MathHelper.Clamp((target.Center.X - myGuard.NPC.Center.X) / 25f, -6f, 6f);

                            chandelierSwingTimer = 0;
                        }
                        else
                        {
                            if (chandelierSwingTimer <= 0)
                            {
                                if (attackTimer % 15 == 0)
                                {
                                    int newBolt = NPC.NewNPC(new Terraria.DataStructures.EntitySource_SpawnNPC(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<SlingshotBolt>(), 20, 0, Main.myPlayer);

                                    Main.npc[newBolt].velocity = Vector2.Zero;

                                    PrimitiveSystem.primitives.CreateTrail(new ShadowflamePrimTrail(Main.npc[newBolt], Color.Violet, 20, 10, true));
                                    PrimitiveSystem.primitives.CreateTrail(new ShadowflamePrimTrail(Main.npc[newBolt], Color.Violet * 0.5f, 16, 10));

                                    SoundEngine.PlaySound(SoundID.Item8, NPC.Center);

                                    slingshotBolts.Add(newBolt);
                                } //Make it so the bolts "slingshot" when brute hits the ground

                                if (attackTimer % 3 == 0 && (myGuard.NPC.velocity.X == 0 || Math.Abs(myGuard.NPC.Center.Y - myRoom.Bottom) < 250) && myGuard.frameY < 5)
                                {
                                    myGuard.frameY++;
                                }

                                if (myGuard.NPC.velocity.X == 0)
                                {
                                    myGuard.NPC.velocity.Y += 0.2f;
                                }
                                else
                                {
                                    if (attackTimer % 4 == 0 && myGuard.frameY > 0)
                                    {
                                        myGuard.frameY--;
                                    }

                                    if (Math.Abs(myGuard.NPC.Center.X - target.Center.X) <= 4)
                                        myGuard.NPC.velocity.X = 0;
                                }

                                if (((myGuard.NPC.velocity.Y <= 0 && myGuard.NPC.oldVelocity.Y > 0) || (myGuard.NPC.position.Y == myGuard.NPC.oldPosition.Y)))
                                {
                                    //Send out shadowflame spirals to left and right of brute's fist

                                    foreach(int slingshotBolt in slingshotBolts)
                                    {
                                        Main.npc[slingshotBolt].ai[0] = 1;
                                    }

                                    slingshotBolts.Clear();

                                    Projectile spike1 = Projectile.NewProjectileDirect(new Terraria.DataStructures.EntitySource_Parent(NPC), new Vector2(myGuard.NPC.Center.X - 124 + 62 - 30, myRoom.Bottom - (168 / 2)), Vector2.Zero, ModContent.ProjectileType<FlameSpiral>(), 0, 0);
                                    spike1.spriteDirection = 1;

                                    Projectile spike2 = Projectile.NewProjectileDirect(new Terraria.DataStructures.EntitySource_Parent(NPC), new Vector2(myGuard.NPC.Center.X + 62 + 30, myRoom.Bottom - (168 / 2)), Vector2.Zero, ModContent.ProjectileType<FlameSpiral>(), 0, 0);
                                    spike2.spriteDirection = -1;

                                    myGuard.NPC.velocity.X = 0;

                                    chandelierSwingTimer++;
                                }
                            }
                            else
                            {
                                chandelierSwingTimer++;

                                myGuard.NPC.velocity.X = 0;

                                if (chandelierSwingTimer > 20)
                                {
                                    chandelierSwingTimer = 0;

                                    PickNewAttack(true);
                                }
                            }
                        }

                        break;
                    case 2: //teleports to one corner of the arena, punches rapidly sending out shadowflame shockwaves, and gets stuck at the end
                        if (attackTimer < 40)
                        {
                            if (attackTimer >= 20)
                            {
                                if (attackTimer % 4 == 0)
                                {
                                    myGuard.frameY++;
                                }
                            }

                            //teleportFloat += 1 / 40f;
                        }
                        else if (attackTimer == 40)
                        {  
                            /*float minDist = 1000000;
                            int minVal = -2;

                            for(int minVal2 = -2; minVal2 < 3; minVal2++)
                            {
                                if(Vector2.Distance(new Vector2(myRoom.Center.X + (minVal2 * 240), myRoom.Bottom - (myGuard.NPC.height / 2)), target.Center) < minDist)
                                {
                                    minDist = Vector2.Distance(new Vector2(myRoom.Center.X + (minVal2 * 240), myRoom.Bottom - (myGuard.NPC.height / 2)), target.Center);

                                    minVal = minVal2;
                                }
                            }*/

                            myGuard.NPC.velocity.Y = -24f;
                            myGuard.NPC.velocity.X = (target.Center.X - myGuard.NPC.Center.X) / 80f;

                            //myGuard.NPC.Center = new Vector2(myRoom.Center.X + (minVal * 240), myRoom.Bottom - (myGuard.NPC.height / 2));
                        }
                        else if(attackTimer < 120)
                        {
                            if (myGuard.NPC.oldVelocity.Y > 0 && myGuard.NPC.oldVelocity.Y == 0) myGuard.NPC.velocity.X = 0;

                            //teleportFloat -= 1 / 40f;
                        }
                        else if(attackTimer < 240)
                        {
                            myGuard.NPC.velocity.X = 0;

                            teleportFloat = 0f;

                            //slamming animation

                            if (attackTimer % 40 < 20)
                            {
                                if (attackTimer % 40 == 0) myGuard.frameY = 0;

                                if (attackTimer % 4 == 0)
                                {
                                    myGuard.frameY++;
                                }
                            }
                            else
                            {
                                if (attackTimer % 4 == 0)
                                {
                                    Projectile spike1 = Projectile.NewProjectileDirect(new Terraria.DataStructures.EntitySource_Parent(NPC), new Vector2(myGuard.NPC.Center.X - 124 - (((attackTimer % 20)) * 15) + 62, myRoom.Bottom - (168 / 2)), Vector2.Zero, ModContent.ProjectileType<FlameSpiral>(), 0, 0);
                                    spike1.spriteDirection = 1;

                                    Projectile spike2 = Projectile.NewProjectileDirect(new Terraria.DataStructures.EntitySource_Parent(NPC), new Vector2(myGuard.NPC.Center.X + (((attackTimer % 20)) * 15) + 62, myRoom.Bottom - (168 / 2)), Vector2.Zero, ModContent.ProjectileType<FlameSpiral>(), 0, 0);
                                    spike2.spriteDirection = -1;
                                }

                                myGuard.frameY = 6;
                            }
                        }
                        else 
                        {
                            PickNewAttack(true);
                        }

                        break;
                    case 3:
                        if (attackTimer < 40)
                        {
                            if (attackTimer >= 20)
                            {
                                if (attackTimer % 4 == 0)
                                {
                                    myGuard.frameY++;
                                }
                            }

                            //teleportFloat += 1 / 40f;
                        }
                        else if (attackTimer == 40)
                        {
                            myGuard.NPC.velocity.Y = -24f;
                            myGuard.NPC.velocity.X = (target.Center.X - myGuard.NPC.Center.X) / 80f;
                        }
                        else if (attackTimer < 120)
                        {
                            if (myGuard.NPC.oldVelocity.Y > 0 && myGuard.NPC.oldVelocity.Y == 0) myGuard.NPC.velocity.X = 0;

                            //teleportFloat -= 1 / 40f;
                        }
                        else if (attackTimer < 240)
                        {
                            myGuard.NPC.velocity.X = 0;

                            teleportFloat = 0f;

                            //slamming animation

                            if (attackTimer % 40 < 20)
                            {
                                if (attackTimer % 40 == 0) myGuard.frameY = 0;

                                if (attackTimer % 4 == 0)
                                {
                                    myGuard.frameY++;
                                }
                            }
                            else
                            {
                                if (attackTimer % 2 == 0)
                                {
                                    Projectile spike1 = Projectile.NewProjectileDirect(new Terraria.DataStructures.EntitySource_Parent(NPC), new Vector2(myGuard.NPC.Center.X + (30 * myGuard.NPC.spriteDirection) + (((attackTimer % 20)) * 30 * myGuard.NPC.spriteDirection), myRoom.Bottom - (168 / 2)), Vector2.Zero, ModContent.ProjectileType<FlameSpiral>(), 0, 0);
                                    spike1.spriteDirection = -myGuard.NPC.spriteDirection;
                                }

                                myGuard.frameY = 6;
                            }
                        }
                        else
                        {
                            PickNewAttack(true);
                        }

                        break;
                }

                NPC.dontTakeDamage = true;

                guardShield = (float)myGuard.NPC.life / (float)myGuard.NPC.lifeMax;

                NPC.Center = myGuard.NPC.Center + new Vector2(-40 * myGuard.NPC.spriteDirection, -32);
                NPC.rotation = 0f;

                attackTimer++;
            }
            else if (fightPhase == 2) //slamming throne cutscene and hopping on chandeliers
            {
                tableTicks++;

                guardShield = 0f;

                NPC.dontTakeDamage = true;

                NPC.knockBackResist = 0f;

                NPC.velocity.Y += 0.48f;

                if (attackTimer < 40)
                {
                    if (teleportFloat < 1) teleportFloat += 1 / 40f;
                }
                else if (attackTimer == 40)
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
                }
                else if (attackTimer < 80)
                {
                    if (teleportFloat > 0) teleportFloat -= 1 / 40f;
                }
                else if (attackTimer == 80)
                {
                    MakeTendrilTrail(scrapbits[5]);
                }
                else if (attackTimer < 700)
                {

                }
                else if (attackTimer == 700)
                {
                    //TODO
                    //Jump up to chandeliers instead of just starting the fight

                    float dist = 100000000;
                    Projectile myProj = null;
                    for (int i = 0; i < chandeliers.Count; i++)
                    {
                        if (Vector2.Distance(NPC.Center, chandeliers[i].Center) < dist)
                        {
                            dist = Vector2.Distance(NPC.Center, chandeliers[i].Center);
                            myProj = chandeliers[i];
                        }
                    }

                    activeChandelierID = myProj.whoAmI;

                    NPC.velocity = new Vector2((myProj.Center.X - NPC.Center.X) / 60f, -22.2f);

                    //SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot("EEMod/Assets/Sounds/goblincry1"));
                }
                else if (attackTimer < 760)
                {
                    NPC.rotation -= 0.25f;
                }
                else if (attackTimer >= 760)
                {
                    fightPhase = 3;
                    attackTimer = 0;
                    chandelierSwingTimer = 0;
                    currentAttack = 0;

                    NPC.velocity.Y = 0;
                    NPC.velocity.X = 0;

                    NPC.rotation = 0f;

                    for (int i = 1; i < 80; i++)
                    {
                        Vector2 flamePos = Vector2.Lerp(myRoom.BottomLeft(), myRoom.BottomRight(), i / 80f) + new Vector2(0, -8);

                        Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_Parent(NPC), flamePos, Vector2.Zero, ModContent.ProjectileType<Shadowfire>(), 0, 0);
                    }

                    PrimitiveSystem.primitives.ClearTrailsOn(scrapbits[5]);

                    return;
                }

                if (attackTimer >= 100)
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


                foreach (Projectile scrapbit in scrapbits)
                {
                    if (scrapbit.ModProjectile is LargeScrap)
                        (scrapbit.ModProjectile as LargeScrap).desiredPosition = new Vector2(myRoom.Center.X, myRoom.Center.Y + (float)(Math.Sin(attackTimer / 60f) * 150f));
                    else if (scrapbit.ModProjectile is SmallScrap)
                        (scrapbit.ModProjectile as SmallScrap).desiredPosition = new Vector2(myRoom.Center.X, myRoom.Center.Y + (float)(Math.Sin(attackTimer / 60f) * 150f));
                    else if (scrapbit.ModProjectile is CenterScrap)
                        (scrapbit.ModProjectile as CenterScrap).desiredPosition = new Vector2(myRoom.Center.X, myRoom.Center.Y + (float)(Math.Sin(attackTimer / 60f) * 150f));
                    else if (scrapbit.ModProjectile is ArmrestScrap)
                        (scrapbit.ModProjectile as ArmrestScrap).desiredPosition = new Vector2(myRoom.Center.X, myRoom.Center.Y + (float)(Math.Sin(attackTimer / 60f) * 150f));
                }

                foreach (Projectile scrapbit in scrapbits)
                {
                    if (scrapbit.ModProjectile is LargeScrap)
                        (scrapbit.ModProjectile as LargeScrap).desiredRotation = (attackTimer / 120f);
                    else if (scrapbit.ModProjectile is SmallScrap)
                        (scrapbit.ModProjectile as SmallScrap).desiredRotation = (attackTimer / 120f);
                    else if (scrapbit.ModProjectile is CenterScrap)
                        (scrapbit.ModProjectile as CenterScrap).desiredRotation = (attackTimer / 120f);
                    else if (scrapbit.ModProjectile is ArmrestScrap)
                        (scrapbit.ModProjectile as ArmrestScrap).desiredRotation = (attackTimer / 120f);
                }

                attackTimer++;
            }
            else if (fightPhase == 3) //phase 2
            {
                tableTicks++;

                guardShield = 0f;

                NPC.dontTakeDamage = false;

                NPC.knockBackResist = 0f;

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
                        if (attackTimer % 100 == 1)
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
                        else if (attackTimer % 100 < 40)
                        {
                            attackVector = target.Center;
                        }
                        else if (attackTimer % 100 == 40)
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
                        else if (attackTimer % 100 < 99)
                        {
                            (attackChandelier.ModProjectile as GoblinChandelierLight).hideFlames = true;
                        }
                        else
                        {
                            (attackChandelier.ModProjectile as GoblinChandelierLight).hideFlames = false;
                        }

                        if (attackTimer >= 300)
                        {
                            PickNewAttack(false);
                        }

                        break;
                    case 1: //lengthens a chandelier and leans down to throw more shadowflame molotovs at the player
                        if (attackTimer % 40 == 0)
                        {
                            Projectile.NewProjectile(new Terraria.DataStructures.EntitySource_Parent(NPC), NPC.Center, (Vector2.Normalize(target.Center - NPC.Center) * 6f), ModContent.ProjectileType<ShadowflameJarBounce>(), 0, 0);
                        }

                        if (attackTimer == 0)
                        {
                            if (nextToTheLeft)
                            {
                                //resetVal = (int)(chandelierSwingTimer / 310) + 90;
                                resetVal = (int)(chandelierSwingTimer / 310) + 0;
                            }
                            else
                            {
                                resetVal = (int)(chandelierSwingTimer / 310) + 45;
                            }

                            (Main.projectile[(int)activeChandelierID].ModProjectile as GoblinChandelierLight).chainLength++;

                            (Main.projectile[(int)activeChandelierID].ModProjectile as GoblinChandelierLight).disabled = true;
                        }
                        if (attackTimer < 180)
                        {
                            (Main.projectile[(int)activeChandelierID].ModProjectile as GoblinChandelierLight).chainLength++;

                            (Main.projectile[(int)activeChandelierID].ModProjectile as GoblinChandelierLight).disabled = true;
                        }
                        else if (attackTimer < 360)
                        {
                            (Main.projectile[(int)activeChandelierID].ModProjectile as GoblinChandelierLight).disabled = true;
                        }
                        else if (attackTimer < 540)
                        {
                            (Main.projectile[(int)activeChandelierID].ModProjectile as GoblinChandelierLight).chainLength--;

                            (Main.projectile[(int)activeChandelierID].ModProjectile as GoblinChandelierLight).disabled = true;
                        }
                        else
                        {
                            (Main.projectile[(int)activeChandelierID].ModProjectile as GoblinChandelierLight).chainLength = 80;

                            (Main.projectile[(int)activeChandelierID].ModProjectile as GoblinChandelierLight).disabled = false;

                            PickNewAttack(false);
                        }

                        break;
                    case 2: // drops a chandelier down to you rapidly and the flames explode, pulls the chandelier back up afterward, repeat 5 times - come back to this
                        if (attackTimer % 80 == 0)
                        {
                            attackTimer++;
                        }

                        if (attackTimer % 80 == 0)
                        {
                            float currDist = 1000000;
                            foreach (Projectile chandelier in chandeliers)
                            {
                                if (Vector2.Distance(chandelier.Center, target.Center) < currDist &&
                                    (Main.projectile[(int)activeChandelierID] != chandelier) &&
                                    chandelier != potChandelier)
                                {
                                    currDist = Vector2.Distance(chandelier.Center, target.Center);

                                    attackChandelier = chandelier;
                                }
                            }

                            //pick chandelier and start drop
                        }
                        else if (attackTimer % 80 < 60)
                        {
                            (attackChandelier.ModProjectile as GoblinChandelierLight).disabled = true;

                            if ((attackChandelier.ModProjectile as GoblinChandelierLight).chainLength < 16 * 33)
                            {
                                (attackChandelier.ModProjectile as GoblinChandelierLight).chainLength += (int)(((attackTimer + 10) % 80) * 0.25f);
                            }

                            if (attackChandelier.Center.Y >= target.Center.Y)
                            {
                                //yes rico, kaboom.

                                attackTimer += (80 - (attackTimer % 80));

                                (attackChandelier.ModProjectile as GoblinChandelierLight).disabled = false;
                            }

                            //drop chandelier
                        }

                        if (attackTimer >= (80 * 5) - 1)
                        {
                            PickNewAttack(false);
                        }


                        break;
                    case 3: //railgun shoots bits of scrap at yoy that stick to tables and explode as mines        SCRAAAAPA

                        if (attackTimer == 0)
                        {
                            activeScrap.Clear();

                            while (activeScrap.Count < 4)
                            {
                                foreach (Projectile bit in scrapbits)
                                {
                                    if (activeScrap.Count < 4 && bit.ModProjectile is LargeScrap) activeScrap.Add(bit);
                                }
                            }

                            MakeTendrilTrail(activeScrap[0]);
                            MakeTendrilTrail(activeScrap[1]);

                            (activeScrap[0].ModProjectile as LargeScrap).movementDuration = 30 + 30 + 65;
                            (activeScrap[1].ModProjectile as LargeScrap).movementDuration = 30 + 30 + 65;
                            (activeScrap[2].ModProjectile as LargeScrap).movementDuration = 30 + 30 + 65;
                            (activeScrap[3].ModProjectile as LargeScrap).movementDuration = 30 + 30 + 65;

                            (activeScrap[0].ModProjectile as LargeScrap).controlPoint = new Vector2(myRoom.Center.X, myRoom.Top) + new Vector2(0, 170);
                            (activeScrap[1].ModProjectile as LargeScrap).controlPoint = new Vector2(myRoom.Center.X, myRoom.Top) + new Vector2(0, 170);
                            (activeScrap[2].ModProjectile as LargeScrap).controlPoint = new Vector2(myRoom.Center.X, myRoom.Top) + new Vector2(0, 170);
                            (activeScrap[3].ModProjectile as LargeScrap).controlPoint = new Vector2(myRoom.Center.X, myRoom.Top) + new Vector2(0, 170);

                            if ((activeScrap[0].ModProjectile as LargeScrap).AttackPhase == -1) (activeScrap[0].ModProjectile as LargeScrap).AttackPhase = 0;
                            if ((activeScrap[1].ModProjectile as LargeScrap).AttackPhase == -1) (activeScrap[1].ModProjectile as LargeScrap).AttackPhase = 0;
                            if ((activeScrap[2].ModProjectile as LargeScrap).AttackPhase == -1) (activeScrap[2].ModProjectile as LargeScrap).AttackPhase = 0;
                            if ((activeScrap[3].ModProjectile as LargeScrap).AttackPhase == -1) (activeScrap[3].ModProjectile as LargeScrap).AttackPhase = 0;
                        }

                        if (attackTimer >= (181 * 2) + 60)
                        {
                            PickNewAttack(false);
                        }

                        if (attackTimer < 30)
                        {
                            if ((activeScrap[0].ModProjectile as LargeScrap).lastPosition != Vector2.Zero) (activeScrap[0].ModProjectile as LargeScrap).controlPoint = new Vector2((activeScrap[0].ModProjectile as LargeScrap).lastPosition.X, myRoom.Top + 50);
                            if ((activeScrap[1].ModProjectile as LargeScrap).lastPosition != Vector2.Zero) (activeScrap[1].ModProjectile as LargeScrap).controlPoint = new Vector2((activeScrap[1].ModProjectile as LargeScrap).lastPosition.X, myRoom.Top + 50);

                            (activeScrap[0].ModProjectile as LargeScrap).AttackPhase = 1;
                            (activeScrap[1].ModProjectile as LargeScrap).AttackPhase = 1;
                        }
                        else if (attackTimer < 60)
                        {

                        }
                        else if ((attackTimer - 60) % 181 < 65)
                        {
                            (activeScrap[0].ModProjectile as LargeScrap).AttackPhase = 1;
                            (activeScrap[1].ModProjectile as LargeScrap).AttackPhase = 1;

                            if ((attackTimer - 60) % 181 == 30)
                            {
                                if ((activeScrap[2].ModProjectile as LargeScrap).lastPosition != Vector2.Zero) (activeScrap[2].ModProjectile as LargeScrap).controlPoint = new Vector2((activeScrap[2].ModProjectile as LargeScrap).lastPosition.X, myRoom.Top + 50);
                                if ((activeScrap[3].ModProjectile as LargeScrap).lastPosition != Vector2.Zero) (activeScrap[3].ModProjectile as LargeScrap).controlPoint = new Vector2((activeScrap[3].ModProjectile as LargeScrap).lastPosition.X, myRoom.Top + 50);

                                (activeScrap[2].ModProjectile as LargeScrap).AttackPhase = 1;
                                (activeScrap[3].ModProjectile as LargeScrap).AttackPhase = 1;

                                MakeTendrilTrail(activeScrap[2]);
                                MakeTendrilTrail(activeScrap[3]);

                                float randFloat = 0;
                                float randFloat2 = 0;

                                while (Math.Abs(randFloat - randFloat2) < 0.2f)
                                {
                                    randFloat = Main.rand.NextFloat(0, MathHelper.Pi);
                                    randFloat2 = Main.rand.NextFloat(0, MathHelper.Pi);
                                }

                                Vector2 pos1 = new Vector2(myRoom.Center.X, myRoom.Top) + new Vector2(0, 170) + new Vector2(myRoom.Width * 0.4f * (float)Math.Cos(randFloat), -60 * (float)Math.Sin(randFloat));
                                Vector2 pos2 = new Vector2(myRoom.Center.X, myRoom.Top) + new Vector2(0, 170) + new Vector2(myRoom.Width * 0.4f * (float)Math.Cos(randFloat2), -60 * (float)Math.Sin(randFloat2));

                                if (Vector2.DistanceSquared(activeScrap[2].Center, pos1) < Vector2.DistanceSquared(activeScrap[3].Center, pos1))
                                {
                                    (activeScrap[2].ModProjectile as LargeScrap).desiredPosition = pos1;
                                    (activeScrap[3].ModProjectile as LargeScrap).desiredPosition = pos2;
                                }
                                else
                                {
                                    (activeScrap[2].ModProjectile as LargeScrap).desiredPosition = pos2;
                                    (activeScrap[3].ModProjectile as LargeScrap).desiredPosition = pos1;
                                }
                            }

                            (activeScrap[0].ModProjectile as LargeScrap).desiredRotation = (target.Center - (activeScrap[0].ModProjectile as LargeScrap).desiredPosition).ToRotation();
                            (activeScrap[1].ModProjectile as LargeScrap).desiredRotation = (target.Center - (activeScrap[1].ModProjectile as LargeScrap).desiredPosition).ToRotation();
                        }
                        else if ((attackTimer - 60) % 181 < 90)
                        {

                        }
                        else if ((attackTimer - 60) % 181 < 145)
                        {
                            (activeScrap[2].ModProjectile as LargeScrap).AttackPhase = 1;
                            (activeScrap[3].ModProjectile as LargeScrap).AttackPhase = 1;

                            if ((attackTimer - 60) % 181 == 120 && (attackTimer - 60) < 181)
                            {
                                if ((activeScrap[0].ModProjectile as LargeScrap).lastPosition != Vector2.Zero) (activeScrap[0].ModProjectile as LargeScrap).controlPoint = new Vector2((activeScrap[0].ModProjectile as LargeScrap).lastPosition.X, myRoom.Top + 50);
                                if ((activeScrap[1].ModProjectile as LargeScrap).lastPosition != Vector2.Zero) (activeScrap[1].ModProjectile as LargeScrap).controlPoint = new Vector2((activeScrap[1].ModProjectile as LargeScrap).lastPosition.X, myRoom.Top + 50);

                                (activeScrap[0].ModProjectile as LargeScrap).AttackPhase = 1;
                                (activeScrap[1].ModProjectile as LargeScrap).AttackPhase = 1;

                                MakeTendrilTrail(activeScrap[0]);
                                MakeTendrilTrail(activeScrap[1]);

                                float randFloat = 0;
                                float randFloat2 = 0;

                                while (Math.Abs(randFloat - randFloat2) < 0.2f)
                                {
                                    randFloat = Main.rand.NextFloat(0, MathHelper.Pi);
                                    randFloat2 = Main.rand.NextFloat(0, MathHelper.Pi);
                                }

                                Vector2 pos1 = new Vector2(myRoom.Center.X, myRoom.Top) + new Vector2(0, 170) + new Vector2(myRoom.Width * 0.4f * (float)Math.Cos(randFloat), -60 * (float)Math.Sin(randFloat));
                                Vector2 pos2 = new Vector2(myRoom.Center.X, myRoom.Top) + new Vector2(0, 170) + new Vector2(myRoom.Width * 0.4f * (float)Math.Cos(randFloat2), -60 * (float)Math.Sin(randFloat2));

                                if (Vector2.DistanceSquared(activeScrap[0].Center, pos1) < Vector2.DistanceSquared(activeScrap[1].Center, pos1))
                                {
                                    (activeScrap[0].ModProjectile as LargeScrap).desiredPosition = pos1;
                                    (activeScrap[1].ModProjectile as LargeScrap).desiredPosition = pos2;
                                }
                                else
                                {
                                    (activeScrap[0].ModProjectile as LargeScrap).desiredPosition = pos2;
                                    (activeScrap[1].ModProjectile as LargeScrap).desiredPosition = pos1;
                                }
                            }

                            (activeScrap[2].ModProjectile as LargeScrap).desiredRotation = (target.Center - (activeScrap[2].ModProjectile as LargeScrap).desiredPosition).ToRotation();
                            (activeScrap[3].ModProjectile as LargeScrap).desiredRotation = (target.Center - (activeScrap[3].ModProjectile as LargeScrap).desiredPosition).ToRotation();
                        }
                        else if ((attackTimer - 60) % 181 < 180)
                        {

                        }

                        break;
                    case 5: // scrap wall attack where you must get in a good position or you get torn up        SCRAAAAPA

                        break;
                    case 4: //morphs the scrap into a sword and starts swinging, up cut, down cut, spin attack, and final up cut and the shards fly up        SCRAAAAPA
                        Vector2 centerScrapOffset = new Vector2(0, -60) + new Vector2(0, -60);

                        Vector2 armrest1Offset = new Vector2(46, -48) + new Vector2(0, -60);
                        Vector2 armrest2Offset = new Vector2(-46, -48) + new Vector2(0, -60);

                        Vector2 centerLong1Offset = new Vector2(8, -186) + new Vector2(0, -60);
                        Vector2 centerLong2Offset = new Vector2(-8, -186) + new Vector2(0, -60);

                        Vector2 sideLong1Offset = new Vector2(24, -156) + new Vector2(0, -60);
                        Vector2 sideLong2Offset = new Vector2(-24, -156) + new Vector2(0, -60);

                        void UpdateScrapPositions()
                        {
                            activeScrap[0].Center = swordOrig + sideLong1Offset.RotatedBy(swordRot) + new Vector2(0, 2).RotatedBy((Main.GameUpdateCount / 10f) + activeScrap[0].whoAmI);
                            activeScrap[1].Center = swordOrig + centerLong1Offset.RotatedBy(swordRot) + new Vector2(0, 2).RotatedBy((Main.GameUpdateCount / 10f) + activeScrap[1].whoAmI);
                            activeScrap[2].Center = swordOrig + centerLong2Offset.RotatedBy(swordRot) + new Vector2(0, 2).RotatedBy((Main.GameUpdateCount / 10f) + activeScrap[2].whoAmI);
                            activeScrap[3].Center = swordOrig + sideLong2Offset.RotatedBy(swordRot) + new Vector2(0, 2).RotatedBy((Main.GameUpdateCount / 10f) + activeScrap[3].whoAmI);
                            activeScrap[4].Center = swordOrig + armrest1Offset.RotatedBy(swordRot) + new Vector2(0, 2).RotatedBy((Main.GameUpdateCount / 10f) + activeScrap[4].whoAmI);
                            activeScrap[5].Center = swordOrig + armrest2Offset.RotatedBy(swordRot) + new Vector2(0, 2).RotatedBy((Main.GameUpdateCount / 10f) + activeScrap[5].whoAmI);
                            activeScrap[6].Center = swordOrig + centerScrapOffset.RotatedBy(swordRot) + new Vector2(0, 2).RotatedBy((Main.GameUpdateCount / 10f) + activeScrap[6].whoAmI);

                            activeScrap[0].rotation = swordRot - MathHelper.PiOver2;
                            activeScrap[1].rotation = swordRot - MathHelper.PiOver2;
                            activeScrap[2].rotation = swordRot - MathHelper.PiOver2;
                            activeScrap[3].rotation = swordRot - MathHelper.PiOver2;
                            activeScrap[4].rotation = swordRot;
                            activeScrap[5].rotation = swordRot;
                            activeScrap[6].rotation = swordRot;
                        }

                        if (attackTimer == 0)
                        {
                            activeScrap.Clear();

                            while (activeScrap.Count < 7)
                            {
                                foreach (Projectile scrapbit in scrapbits)
                                {
                                    if (scrapbit.ModProjectile is CenterScrap && activeScrap.Count == 6)
                                        activeScrap.Add(scrapbit);
                                    if (scrapbit.ModProjectile is ArmrestScrap && activeScrap.Count >= 4 && activeScrap.Count < 6)
                                        activeScrap.Add(scrapbit);
                                    if (scrapbit.ModProjectile is LargeScrap && activeScrap.Count < 4)
                                        activeScrap.Add(scrapbit);
                                }
                            }

                            staffPos = new Vector2(myRoom.Center.X, myRoom.Center.Y);

                            targetOldPos = target.Center - staffPos;

                            swordRot = targetOldPos.ToRotation() + MathHelper.PiOver2 - 1.25f;

                            (activeScrap[0].ModProjectile as LargeScrap).AttackPhase = -1;
                            (activeScrap[1].ModProjectile as LargeScrap).AttackPhase = -1;
                            (activeScrap[2].ModProjectile as LargeScrap).AttackPhase = -1;
                            (activeScrap[3].ModProjectile as LargeScrap).AttackPhase = -1;
                            (activeScrap[4].ModProjectile as ArmrestScrap).AttackPhase = -1;
                            (activeScrap[5].ModProjectile as ArmrestScrap).AttackPhase = -1;
                            (activeScrap[6].ModProjectile as CenterScrap).AttackPhase = -1;

                            (activeScrap[0].ModProjectile as LargeScrap).desiredPosition = Vector2.Zero;
                            (activeScrap[1].ModProjectile as LargeScrap).desiredPosition = Vector2.Zero;
                            (activeScrap[2].ModProjectile as LargeScrap).desiredPosition = Vector2.Zero;
                            (activeScrap[3].ModProjectile as LargeScrap).desiredPosition = Vector2.Zero;
                            (activeScrap[4].ModProjectile as ArmrestScrap).desiredPosition = Vector2.Zero;
                            (activeScrap[5].ModProjectile as ArmrestScrap).desiredPosition = Vector2.Zero;
                            (activeScrap[6].ModProjectile as CenterScrap).desiredPosition = Vector2.Zero;

                            (activeScrap[0].ModProjectile as LargeScrap).lastPosition = activeScrap[0].Center;
                            (activeScrap[1].ModProjectile as LargeScrap).lastPosition = activeScrap[1].Center;
                            (activeScrap[2].ModProjectile as LargeScrap).lastPosition = activeScrap[2].Center;
                            (activeScrap[3].ModProjectile as LargeScrap).lastPosition = activeScrap[3].Center;
                            (activeScrap[4].ModProjectile as ArmrestScrap).lastPosition = activeScrap[4].Center;
                            (activeScrap[5].ModProjectile as ArmrestScrap).lastPosition = activeScrap[5].Center;
                            (activeScrap[6].ModProjectile as CenterScrap).lastPosition = activeScrap[6].Center;

                            (activeScrap[0].ModProjectile as LargeScrap).lastRotation = activeScrap[0].rotation;
                            (activeScrap[1].ModProjectile as LargeScrap).lastRotation = activeScrap[1].rotation;
                            (activeScrap[2].ModProjectile as LargeScrap).lastRotation = activeScrap[2].rotation;
                            (activeScrap[3].ModProjectile as LargeScrap).lastRotation = activeScrap[3].rotation;
                            (activeScrap[4].ModProjectile as ArmrestScrap).lastRotation = activeScrap[4].rotation;
                            (activeScrap[5].ModProjectile as ArmrestScrap).lastRotation = activeScrap[5].rotation;
                            (activeScrap[6].ModProjectile as CenterScrap).lastRotation = activeScrap[6].rotation;
                        }

                        if (attackTimer < 60)
                        {
                            if (attackTimer == 0) targetOldPosOld = targetOldPos;

                            swordRot = targetOldPos.ToRotation() + MathHelper.PiOver2 - 1.25f;

                            staffPos = new Vector2(myRoom.Center.X, myRoom.Center.Y);

                            swordOrig = staffPos + new Vector2((float)Math.Cos(swordRot - MathHelper.PiOver2 - targetOldPos.ToRotation()) * 150f, (float)Math.Sin(swordRot - MathHelper.PiOver2 - targetOldPos.ToRotation()) * 40f)
                                .RotatedBy((targetOldPos).ToRotation());

                            activeScrap[0].Center = Vector2.SmoothStep((activeScrap[0].ModProjectile as LargeScrap).lastPosition, swordOrig + sideLong1Offset.RotatedBy(swordRot) + new Vector2(0, 2).RotatedBy((Main.GameUpdateCount / 10f) + activeScrap[0].whoAmI), attackTimer / 60f);
                            activeScrap[1].Center = Vector2.SmoothStep((activeScrap[1].ModProjectile as LargeScrap).lastPosition, swordOrig + centerLong1Offset.RotatedBy(swordRot) + new Vector2(0, 2).RotatedBy((Main.GameUpdateCount / 10f) + activeScrap[1].whoAmI), attackTimer / 60f);
                            activeScrap[2].Center = Vector2.SmoothStep((activeScrap[2].ModProjectile as LargeScrap).lastPosition, swordOrig + centerLong2Offset.RotatedBy(swordRot) + new Vector2(0, 2).RotatedBy((Main.GameUpdateCount / 10f) + activeScrap[2].whoAmI), attackTimer / 60f);
                            activeScrap[3].Center = Vector2.SmoothStep((activeScrap[3].ModProjectile as LargeScrap).lastPosition, swordOrig + sideLong2Offset.RotatedBy(swordRot) + new Vector2(0, 2).RotatedBy((Main.GameUpdateCount / 10f) + activeScrap[3].whoAmI), attackTimer / 60f);
                            activeScrap[4].Center = Vector2.SmoothStep((activeScrap[4].ModProjectile as ArmrestScrap).lastPosition, swordOrig + armrest1Offset.RotatedBy(swordRot) + new Vector2(0, 2).RotatedBy((Main.GameUpdateCount / 10f) + activeScrap[4].whoAmI), attackTimer / 60f);
                            activeScrap[5].Center = Vector2.SmoothStep((activeScrap[5].ModProjectile as ArmrestScrap).lastPosition, swordOrig + armrest2Offset.RotatedBy(swordRot) + new Vector2(0, 2).RotatedBy((Main.GameUpdateCount / 10f) + activeScrap[5].whoAmI), attackTimer / 60f);
                            activeScrap[6].Center = Vector2.SmoothStep((activeScrap[6].ModProjectile as CenterScrap).lastPosition, swordOrig + centerScrapOffset.RotatedBy(swordRot) + new Vector2(0, 2).RotatedBy((Main.GameUpdateCount / 10f) + activeScrap[6].whoAmI), attackTimer / 60f);

                            activeScrap[0].rotation = MathHelper.SmoothStep((activeScrap[0].ModProjectile as LargeScrap).lastRotation, swordRot - MathHelper.PiOver2, attackTimer / 60f);
                            activeScrap[1].rotation = MathHelper.SmoothStep((activeScrap[1].ModProjectile as LargeScrap).lastRotation, swordRot - MathHelper.PiOver2, attackTimer / 60f);
                            activeScrap[2].rotation = MathHelper.SmoothStep((activeScrap[2].ModProjectile as LargeScrap).lastRotation, swordRot - MathHelper.PiOver2, attackTimer / 60f);
                            activeScrap[3].rotation = MathHelper.SmoothStep((activeScrap[3].ModProjectile as LargeScrap).lastRotation, swordRot - MathHelper.PiOver2, attackTimer / 60f);
                            activeScrap[4].rotation = MathHelper.SmoothStep((activeScrap[4].ModProjectile as ArmrestScrap).lastRotation, swordRot, attackTimer / 60f);
                            activeScrap[5].rotation = MathHelper.SmoothStep((activeScrap[5].ModProjectile as ArmrestScrap).lastRotation, swordRot, attackTimer / 60f);
                            activeScrap[6].rotation = MathHelper.SmoothStep((activeScrap[6].ModProjectile as CenterScrap).lastRotation, swordRot, attackTimer / 60f);
                        }
                        else if (attackTimer < 120)
                        {
                            if (attackTimer < 65)
                            {
                                targetOldPosNew = target.Center - staffPos;

                                targetOldPos = Vector2.SmoothStep(targetOldPosOld, targetOldPosNew, (attackTimer - 60) / 5f);

                                if (attackTimer == 65 - 1) targetOldPosOld = targetOldPos;
                            }

                            swordRot = targetOldPos.ToRotation() + MathHelper.PiOver2 + 
                                (Easing.PiecewiseAnimation((attackTimer - 60) / 60f, new Easing.CurveSegment[] { anticipation, swingback, slash }) * 2.5f) - 1.25f;

                            staffPos += Vector2.Normalize(targetOldPos) *
                                Math.Abs(Easing.PiecewiseAnimation((attackTimer - 60 + 1) / 60f, new Easing.CurveSegment[] { anticipation, swingback, slash }) -
                                 Easing.PiecewiseAnimation((attackTimer - 60) / 60f, new Easing.CurveSegment[] { anticipation, swingback, slash }))
                                * 40f;

                            swordOrig = staffPos + new Vector2((float)Math.Cos(swordRot - MathHelper.PiOver2 - targetOldPos.ToRotation()) * 150f, (float)Math.Sin(swordRot - MathHelper.PiOver2 - targetOldPos.ToRotation()) * 40f)
                                .RotatedBy((targetOldPos).ToRotation());

                            UpdateScrapPositions();
                        }
                        else if (attackTimer < 180)
                        {
                            if (attackTimer < 125)
                            {
                                targetOldPosNew = target.Center - staffPos;

                                targetOldPos = Vector2.SmoothStep(targetOldPosOld, targetOldPosNew, (attackTimer - 120) / 5f);

                                if (attackTimer == 125 - 1) targetOldPosOld = targetOldPos;
                            }

                            swordRot = targetOldPos.ToRotation() + MathHelper.PiOver2 + 
                                (Easing.PiecewiseAnimation(((attackTimer - 120) / 60f), new Easing.CurveSegment[] { swingback2, slash2 }) * 2.5f) - 1.25f;

                            staffPos += Vector2.Normalize(targetOldPos) * 
                                Math.Abs(Easing.PiecewiseAnimation(((attackTimer - 120 + 1) / 60f), new Easing.CurveSegment[] { swingback2, slash2 }) -
                                 Easing.PiecewiseAnimation(((attackTimer - 120) / 60f), new Easing.CurveSegment[] { swingback2, slash2 }))
                                * 40f;

                            swordOrig = staffPos + new Vector2((float)Math.Cos(swordRot - MathHelper.PiOver2 - targetOldPos.ToRotation()) * 150f, (float)Math.Sin(swordRot - MathHelper.PiOver2 - targetOldPos.ToRotation()) * 40f)
                                .RotatedBy((targetOldPos).ToRotation());

                            UpdateScrapPositions();
                        }
                        else if (attackTimer < 240)
                        {
                            if (attackTimer < 185)
                            {
                                targetOldPosNew = target.Center - staffPos;

                                targetOldPos = Vector2.SmoothStep(targetOldPosOld, targetOldPosNew, (attackTimer - 180) / 5f);

                                if (attackTimer == 180 - 1) targetOldPosOld = targetOldPos;
                            }

                            swordRot = targetOldPos.ToRotation() + MathHelper.PiOver2 + 
                                (Easing.PiecewiseAnimation(((attackTimer - 180) / 60f), new Easing.CurveSegment[] { slash3 }) * 2.5f) - 1.25f;

                            staffPos += Vector2.Normalize(targetOldPos) *
                                Math.Abs(Easing.PiecewiseAnimation(((attackTimer - 180 + 1) / 60f), new Easing.CurveSegment[] { slash3 }) -
                                Easing.PiecewiseAnimation(((attackTimer - 180) / 60f), new Easing.CurveSegment[] { slash3 }) )
                                * 60f;

                            swordOrig = staffPos + Vector2.SmoothStep(new Vector2((float)Math.Cos(swordRot - MathHelper.PiOver2 - targetOldPos.ToRotation()) * 150f, (float)Math.Sin(swordRot - MathHelper.PiOver2 - targetOldPos.ToRotation()) * 40f)
                                .RotatedBy((targetOldPos).ToRotation()), Vector2.Zero, MathHelper.Clamp((attackTimer - 180) / 10f, 0f, 1f));

                            (activeScrap[0].ModProjectile as LargeScrap).lastPosition = activeScrap[0].Center;
                            (activeScrap[1].ModProjectile as LargeScrap).lastPosition = activeScrap[1].Center;
                            (activeScrap[2].ModProjectile as LargeScrap).lastPosition = activeScrap[2].Center;
                            (activeScrap[3].ModProjectile as LargeScrap).lastPosition = activeScrap[3].Center;
                            (activeScrap[4].ModProjectile as ArmrestScrap).lastPosition = activeScrap[4].Center;
                            (activeScrap[5].ModProjectile as ArmrestScrap).lastPosition = activeScrap[5].Center;
                            (activeScrap[6].ModProjectile as CenterScrap).lastPosition = activeScrap[6].Center;

                            (activeScrap[0].ModProjectile as LargeScrap).lastRotation = activeScrap[0].rotation;
                            (activeScrap[1].ModProjectile as LargeScrap).lastRotation = activeScrap[1].rotation;
                            (activeScrap[2].ModProjectile as LargeScrap).lastRotation = activeScrap[2].rotation;
                            (activeScrap[3].ModProjectile as LargeScrap).lastRotation = activeScrap[3].rotation;
                            (activeScrap[4].ModProjectile as ArmrestScrap).lastRotation = activeScrap[4].rotation;
                            (activeScrap[5].ModProjectile as ArmrestScrap).lastRotation = activeScrap[5].rotation;
                            (activeScrap[6].ModProjectile as CenterScrap).lastRotation = activeScrap[6].rotation;

                            UpdateScrapPositions();
                        }
                        else if (attackTimer == 240)
                        {
                            (activeScrap[0].ModProjectile as LargeScrap).AttackPhase = 3;
                            (activeScrap[1].ModProjectile as LargeScrap).AttackPhase = 3;
                            (activeScrap[2].ModProjectile as LargeScrap).AttackPhase = 3;
                            (activeScrap[3].ModProjectile as LargeScrap).AttackPhase = 3;
                            (activeScrap[4].ModProjectile as ArmrestScrap).AttackPhase = 3;
                            (activeScrap[5].ModProjectile as ArmrestScrap).AttackPhase = 3;
                            (activeScrap[6].ModProjectile as CenterScrap).AttackPhase = 3;

                            activeScrap[0].velocity = (activeScrap[0].Center - (activeScrap[0].ModProjectile as LargeScrap).lastPosition);
                            activeScrap[1].velocity = (activeScrap[1].Center - (activeScrap[1].ModProjectile as LargeScrap).lastPosition);
                            activeScrap[2].velocity = (activeScrap[2].Center - (activeScrap[2].ModProjectile as LargeScrap).lastPosition);
                            activeScrap[3].velocity = (activeScrap[3].Center - (activeScrap[3].ModProjectile as LargeScrap).lastPosition);
                            activeScrap[4].velocity = (activeScrap[4].Center - (activeScrap[4].ModProjectile as ArmrestScrap).lastPosition);
                            activeScrap[5].velocity = (activeScrap[5].Center - (activeScrap[5].ModProjectile as ArmrestScrap).lastPosition);
                            activeScrap[6].velocity = (activeScrap[6].Center - (activeScrap[6].ModProjectile as CenterScrap).lastPosition);
                        }
                        else if (attackTimer < 300)
                        {
                            activeScrap[0].rotation = MathHelper.Lerp((activeScrap[0].ModProjectile as LargeScrap).lastRotation, activeScrap[0].velocity.ToRotation(), MathHelper.Clamp((attackTimer - 240) / 5f, 0f, 1f));
                            activeScrap[1].rotation = MathHelper.Lerp((activeScrap[1].ModProjectile as LargeScrap).lastRotation, activeScrap[1].velocity.ToRotation(), MathHelper.Clamp((attackTimer - 240) / 5f, 0f, 1f));
                            activeScrap[2].rotation = MathHelper.Lerp((activeScrap[2].ModProjectile as LargeScrap).lastRotation, activeScrap[2].velocity.ToRotation(), MathHelper.Clamp((attackTimer - 240) / 5f, 0f, 1f));
                            activeScrap[3].rotation = MathHelper.Lerp((activeScrap[3].ModProjectile as LargeScrap).lastRotation, activeScrap[3].velocity.ToRotation(), MathHelper.Clamp((attackTimer - 240) / 5f, 0f, 1f));
                            activeScrap[4].rotation = MathHelper.Lerp((activeScrap[4].ModProjectile as ArmrestScrap).lastRotation, activeScrap[4].velocity.ToRotation(), MathHelper.Clamp((attackTimer - 240) / 5f, 0f, 1f));
                            activeScrap[5].rotation = MathHelper.Lerp((activeScrap[5].ModProjectile as ArmrestScrap).lastRotation, activeScrap[5].velocity.ToRotation(), MathHelper.Clamp((attackTimer - 240) / 5f, 0f, 1f));
                            activeScrap[6].rotation = MathHelper.Lerp((activeScrap[6].ModProjectile as CenterScrap).lastRotation, activeScrap[6].velocity.ToRotation(), MathHelper.Clamp((attackTimer - 240) / 5f, 0f, 1f));
                        }
                        else
                        {
                            PickNewAttack(false);
                        }

                        foreach(Projectile scrap in activeScrap)
                        {
                            while (scrap.rotation > MathHelper.Pi)
                                scrap.rotation -= MathHelper.TwoPi;
                            while (scrap.rotation < -MathHelper.Pi)
                                scrap.rotation += MathHelper.TwoPi;
                        }

                        break;
                    case 6: //railgun scrap knives

                        break;
                }
                #endregion

                #region Chandelier swinging logic
                if (chandelierSwingTimer % 310 == 0)
                {
                    List<Projectile> potentialChandeliers = new List<Projectile>();

                    potChandelier = null;

                    foreach (Projectile chandelier in chandeliers)
                    {
                        if (Vector2.Distance(chandelier.Center, NPC.Center) < 20 * 16 && chandelier != Main.projectile[(int)activeChandelierID] && !(chandelier.ModProjectile as GoblinChandelierLight).disabled)
                        {
                            potentialChandeliers.Add(chandelier);
                        }
                    }

                    potChandelier = potentialChandeliers[Main.rand.Next(0, potentialChandeliers.Count)];

                    if (nextToTheLeft)
                    {
                        resetVal = (int)(chandelierSwingTimer / 310) + 0;
                        chandelierSwingTimer = (int)(chandelierSwingTimer / 310) + 0;
                    }
                    else
                    {
                        resetVal = (int)(chandelierSwingTimer / 310) + 45;
                        chandelierSwingTimer = (int)(chandelierSwingTimer / 310) + 45;
                    }

                    if ((potChandelier.ModProjectile as GoblinChandelierLight).anchorPos16.X < (Main.projectile[(int)activeChandelierID].ModProjectile as GoblinChandelierLight).anchorPos16.X)
                    {
                        nextToTheLeft = true;
                    }
                    else
                    {
                        nextToTheLeft = false;
                    }

                    GoblinChandelierLight skrunkle = (Main.projectile[(int)activeChandelierID].ModProjectile as GoblinChandelierLight);

                    skrunkle.axisRotation = (float)Math.Sin(((chandelierSwingTimer % 310) * MathHelper.TwoPi) / 90f) * 0.5f;

                    NPC.Center = skrunkle.anchorPos16 +
                        ((Vector2.UnitY).RotatedBy(skrunkle.axisRotation)) * (skrunkle.chainLength - 40);

                    NPC.rotation = skrunkle.axisRotation;
                }
                else if (chandelierSwingTimer % 310 < 180) //Actively swinging on a chandelier
                {
                    GoblinChandelierLight skrunkle = (Main.projectile[(int)activeChandelierID].ModProjectile as GoblinChandelierLight);

                    skrunkle.axisRotation = (float)Math.Sin(((chandelierSwingTimer % 310) * MathHelper.TwoPi) / 90f) * 0.5f;

                    NPC.Center = skrunkle.anchorPos16 +
                        ((Vector2.UnitY).RotatedBy(skrunkle.axisRotation)) * (skrunkle.chainLength - 40);

                    NPC.rotation = skrunkle.axisRotation;
                }
                else if (chandelierSwingTimer % 310 < 270) //Picking the next chandelier to swing to, and starting the jump
                {
                    if ((nextToTheLeft && chandelierSwingTimer == 180) || (!nextToTheLeft && chandelierSwingTimer == 225))
                    {
                        if (potChandelier == null || (Main.projectile[(int)activeChandelierID].ModProjectile as GoblinChandelierLight).disabled)
                        {
                            chandelierSwingTimer = resetVal;

                            GoblinChandelierLight skrunkle = (Main.projectile[(int)activeChandelierID].ModProjectile as GoblinChandelierLight);

                            skrunkle.axisRotation = (float)Math.Sin(((chandelierSwingTimer % 310) * MathHelper.TwoPi) / 90f) * 0.5f;

                            NPC.Center = skrunkle.anchorPos16 +
                                ((Vector2.UnitY).RotatedBy(skrunkle.axisRotation)) * (skrunkle.chainLength - 40);

                            NPC.rotation = skrunkle.axisRotation;

                            skrunkle.axisRotation = (float)Math.Sin(((chandelierSwingTimer % 310) * MathHelper.TwoPi) / 90f) * 0.5f;

                            chandelierSwingTimer++;

                            return;
                        }

                        //Ready to swing

                        (Main.projectile[(int)activeChandelierID].ModProjectile as GoblinChandelierLight).rotationVelocity = (chandelierSwingTimer == 180 ? 0.05f : -0.05f);

                        activeChandelierID = potChandelier.whoAmI;

                        (Main.projectile[(int)activeChandelierID].ModProjectile as GoblinChandelierLight).rotationVelocity = 0;

                        chandelierSwingTimer = 270;

                        NPC.velocity.X = (Main.projectile[(int)activeChandelierID].Center.X - NPC.Center.X) / 40f;
                        NPC.velocity.Y = -2f;
                    }
                    else
                    {
                        GoblinChandelierLight skrunkle = (Main.projectile[(int)activeChandelierID].ModProjectile as GoblinChandelierLight);

                        skrunkle.axisRotation = (float)Math.Sin(((chandelierSwingTimer % 310) * MathHelper.TwoPi) / 90f) * 0.5f;

                        NPC.Center = skrunkle.anchorPos16 +
                            ((Vector2.UnitY).RotatedBy(skrunkle.axisRotation)) * (skrunkle.chainLength - 40);

                        NPC.rotation = skrunkle.axisRotation;
                    }
                }
                else if (chandelierSwingTimer % 310 < 310) //In midair
                {
                    NPC.velocity.Y += 2 / 20f;
                    NPC.rotation += NPC.velocity.X / 15f;
                    NPC.spriteDirection = (NPC.velocity.X < 0 ? -1 : 1);
                }
                #endregion

                chandelierSwingTimer++;
                attackTimer++;
            }
            else if (fightPhase == 4)//20% hp phase / hand phase
            {

            }
            else if (fightPhase == 5) //death cutscene (W)
            {

            }
        }

        public void MakeTendrilTrail(Projectile tendrilTarget)
        {
            PrimitiveSystem.primitives.CreateTrail(new ScrapwizardTendrilPrimTrail2(tendrilTarget, NPC, 48f));
            PrimitiveSystem.primitives.CreateTrail(new ScrapwizardTendrilPrimTrail(tendrilTarget, NPC, 24f, 0.65f));
            PrimitiveSystem.primitives.CreateTrail(new ScrapwizardTendrilPrimTrail(tendrilTarget, NPC, 16f, 0.95f));
        }

        public void PickNewAttack(bool phase1)
        {
            if (phase1)
            {
                myGuard.frameY = 0;
                currentAttack = Main.rand.Next(4);
                myGuard.NPC.velocity = Vector2.Zero;
            }
            else
            {
                //currentAttack = Main.rand.Next(5);
                currentAttack = 3;
            }

            attackTimer = -1;
        }

        public override void OnKill()
        {
            if (fightPhase >= 2)
            {
                (Main.projectile[(int)activeChandelierID].ModProjectile as GoblinChandelierLight).rotationVelocity = -(Main.projectile[(int)activeChandelierID].ModProjectile as GoblinChandelierLight).axisRotation * 0.05f;
            }

            foreach (Projectile table in tables)
            {
                (table.ModProjectile as PhantomTable).dyingTicks++;
            }

            foreach (Projectile chandelier in chandeliers)
            {
                (chandelier.ModProjectile as GoblinChandelierLight).retracting = true;
            }
        }

        public override bool CheckActive()
        {
            return false;
        }

        public void InitShader(SpriteBatch spriteBatch)
        {
            spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, default, Main.GameViewMatrix.ZoomMatrix);

            EEMod.ShadowWarp.Parameters["noise"].SetValue(ModContent.GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/Noise/noise").Value);
            EEMod.ShadowWarp.Parameters["newColor"].SetValue(new Vector4(Color.Violet.R, Color.Violet.G, Color.Violet.B, Color.Violet.A) / 255f);
            EEMod.ShadowWarp.Parameters["lerpVal"].SetValue(1 - MathHelper.Clamp((teleportFloat), 0f, 1f));
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

        public Vector2 staffCastPos;

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/ScrapwizardStaff").Value;

            InitShader(spriteBatch);

            if (fightPhase == 3 && currentAttack == 4)
            {
                if (attackTimer < 60)
                {
                    staffReturned = false;

                    spriteBatch.Draw(tex, Vector2.SmoothStep(NPC.Center, swordOrig + new Vector2(0, -56).RotatedBy(swordRot), attackTimer / 60f) - Main.screenPosition, null, Color.White, MathHelper.Lerp(0f, swordRot, attackTimer / 60f), tex.Size() / 2f, 1f, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
                }
                else if (attackTimer < 240)
                {
                    spriteBatch.Draw(tex, swordOrig - Main.screenPosition + new Vector2(0, -56).RotatedBy(swordRot), null, Color.White, swordRot, tex.Size() / 2f, 1f, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

                    if (attackTimer == 239) oldSwordOrig = swordOrig + new Vector2(0, -56).RotatedBy(swordRot);
                }
                else
                {
                    if (!staffReturned)
                    {
                        if (attackTimer == 240)
                        {
                            swordOrig += new Vector2(0, -56).RotatedBy(swordRot);

                            staffVelocity = (swordOrig - oldSwordOrig);
                        }

                        float staffVelMag = staffVelocity.Length();

                        staffVelocity += Vector2.Normalize(NPC.Center - swordOrig)  * 2f;

                        staffVelocity = Vector2.Normalize(staffVelocity) * staffVelMag;

                        swordOrig += staffVelocity;

                        swordRot += ((targetOldPos.ToRotation() + MathHelper.PiOver2 +
                                    (Easing.PiecewiseAnimation(((240 - 180) / 60f), new Easing.CurveSegment[] { slash3 }) * 2.5f) - 1.25f) -
                                    (targetOldPos.ToRotation() + MathHelper.PiOver2 +
                                    (Easing.PiecewiseAnimation(((240 - 180 - 1) / 60f), new Easing.CurveSegment[] { slash3 }) * 2.5f) - 1.25f));

                        if (Vector2.Distance(NPC.Center, swordOrig) <= staffVelocity.Length())
                        {
                            swordOrig = NPC.Center;
                            staffReturned = true;
                            swordRot = 0f;
                        }

                        spriteBatch.Draw(tex, swordOrig - Main.screenPosition, null, Color.White, swordRot, tex.Size() / 2f, 1f, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
                    }
                    else
                    {
                        spriteBatch.Draw(tex, NPC.Center - Main.screenPosition, null, Color.White, swordRot, tex.Size() / 2f, 1f, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
                    }
                }
            }
            else
            {
                staffPos = NPC.Center;

                staffCastPos = staffPos + new Vector2(0, -28).RotatedBy(NPC.rotation);

                spriteBatch.Draw(tex, NPC.Center - Main.screenPosition, null, Color.White, NPC.rotation, tex.Size() / 2f, 1f, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            }


            //Texture2D tex2 = ModContent.Request<Texture2D>("EEMod/NPCs/Goblins/Scrapwizard/ScrapwizardArm").Value;

            //spriteBatch.Draw(tex2, NPC.Center - Main.screenPosition, null, Color.White, NPC.rotation, tex2.Size() / 2f, 1f, NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            InitShader(spriteBatch);


            if (fightPhase == 3)
            {
                if (currentAttack == 0 && (attackTimer % 100) > 1 && (attackTimer % 100) <= 40)
                {
                    spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, null, null, null, Main.GameViewMatrix.ZoomMatrix);

                    Texture2D telegraphTex = ModContent.Request<Texture2D>("EEMod/Textures/TelegraphLine").Value;

                    Point pos = (attackVector - Main.screenPosition).ToPoint();

                    spriteBatch.Draw(telegraphTex, new Rectangle(pos.X, pos.Y, 10, (int)Vector2.Distance((attackChandelier.ModProjectile as GoblinChandelierLight).trails[0].startPoint, attackVector)), null, Color.Pink * MathHelper.Clamp(1 + ((20 - (attackTimer % 100)) / 20f), 0, 1) * 0.75f, ((attackChandelier.ModProjectile as GoblinChandelierLight).trails[0].startPoint - attackVector).ToRotation() - (MathHelper.Pi / 2f), new Vector2(37 / 2f, 0), SpriteEffects.None, 0f);
                    spriteBatch.Draw(telegraphTex, new Rectangle(pos.X, pos.Y, 10, (int)Vector2.Distance((attackChandelier.ModProjectile as GoblinChandelierLight).trails[3].startPoint, attackVector)), null, Color.Pink * MathHelper.Clamp(1 + ((20 - (attackTimer % 100)) / 20f), 0, 1) * 0.75f, ((attackChandelier.ModProjectile as GoblinChandelierLight).trails[3].startPoint - attackVector).ToRotation() - (MathHelper.Pi / 2f), new Vector2(37 / 2f, 0), SpriteEffects.None, 0f);
                    spriteBatch.Draw(telegraphTex, new Rectangle(pos.X, pos.Y, 10, (int)Vector2.Distance((attackChandelier.ModProjectile as GoblinChandelierLight).trails[6].startPoint, attackVector)), null, Color.Pink * MathHelper.Clamp(1 + ((20 - (attackTimer % 100)) / 20f), 0, 1) * 0.75f, ((attackChandelier.ModProjectile as GoblinChandelierLight).trails[6].startPoint - attackVector).ToRotation() - (MathHelper.Pi / 2f), new Vector2(37 / 2f, 0), SpriteEffects.None, 0f);
                }
                /*if (currentAttack == 1 && (attackTimer % 100) > 48 && (attackTimer % 100) <= 60)
                {
                    spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, null, null, null, Main.GameViewMatrix.ZoomMatrix);

                    Texture2D godrayTex = ModContent.Request<Texture2D>("EEMod/Textures/GodrayMask").Value;

                    Vector2 pos = (attackChandelier.Center - Main.screenPosition);

                    spriteBatch.Draw(godrayTex, pos, null, Color.Pink, Main.GameUpdateCount / 15f, godrayTex.TextureCenter(), 0.33f * Math.Sin(((attackTimer % 100) - 48) * MathHelper.Pi / 12f).PositiveSin(), SpriteEffects.None, 0f);
                }*/
            }

            EEMod.UIText("" + currentAttack, Color.White, NPC.Center - new Vector2(0, 100) - Main.screenPosition, 0);

            spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, default, Main.GameViewMatrix.ZoomMatrix);
        }
    }
}