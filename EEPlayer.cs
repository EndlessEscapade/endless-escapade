using EEMod.Autoloading;
using EEMod.Buffs.Buffs;
using EEMod.Config;
using EEMod.Extensions;
using EEMod.ID;
using EEMod.Items.Fish;
using EEMod.Projectiles;
using EEMod.Projectiles.Armor;
using EEMod.Items.Weapons.Mage;
using EEMod.Projectiles.Runes;
using EEMod.VerletIntegration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static EEMod.EEWorld.EEWorld;
using static Terraria.ModLoader.ModContent;
using EEMod.Seamap.SeamapContent;
using Terraria.DataStructures;
using System.Linq;
using EEMod.Systems.Subworlds.EESubworlds;
using EEMod.EEWorld;
using EEMod.Players;
using EEMod.Items.Accessories;

namespace EEMod
{
    public partial class EEPlayer : ModPlayer
    {
        /// <summary>Screen shake</summary>
        public int Shake = 0;
        public bool importantCutscene;
        public static bool startingText;
        public bool godMode = false;



        public bool HasVisitedSpire;

        //Equipment booleans
        //public bool lythenSet;
        //public int lythenSetTimer;
        //public bool hydriteSet;
        //public bool hydrofluoricSet;
        //public int hydrofluoricSetTimer;
        //public bool hydriteVisage;
        public bool quartzCrystal = false;
        public bool isQuartzRangedOn = false;
        public bool isQuartzSummonOn = false;
        public bool isQuartzMeleeOn = false;
        public bool isQuartzChestOn = false;
        //public bool FlameSpirit; // unused

        //Runes
        public byte[] hasGottenRuneBefore = new byte[7];
        public byte[] inPossesion = new byte[7];
        public int bubbleRuneBubble = 0;

        //Whips
        public int summonTagDamage;
        public int summonTagCrit;

        public readonly SubworldManager SM = new SubworldManager();
        public int rippleCount = 3;
        public int rippleSize = 5;
        public int rippleSpeed = 15;
        public int distortStrength = 100;
        public float powerLevel = 0;
        public int maxPowerLevel = 11;
        public float zipMultiplier = 1;
        public int thermalHealingTimer = 30;
        public int cannonballType = 0;
        public bool isPickingUp;
        private float propagation;

        public Dictionary<int, int> fishLengths = new Dictionary<int, int>();

        private readonly int displaceX = 2;
        private readonly int displaceY = 4;
        private readonly float[] dis = new float[51];
        public string NameForJoiningClients = "";
        public Vector2[] arrayPoints = new Vector2[24];
        //public static EEPlayer instance => Main.LocalPlayer.GetModPlayer<EEPlayer>(); // unused
        //private int Arrow; // unused
        //public int Arrow2; // unused
        private float speedOfPan = 1;
        //public int offSea = 1000;// unused
        //private int opac; 
        public int boatSpeed = 1;
        private readonly string RippleShader = "EEMod:Ripple";
        private readonly string SunThroughWallsShader = "EEMod:SunThroughWalls";
        private readonly string SeaTransShader = "EEMod:SeaTrans";
        public bool firstFrameVolcano;
        public Vector2 PylonBegin;
        public Vector2 PylonEnd;
        public bool holdingPylon;
        public bool ridingZipline;
        public Vector2 secondPlayerMouse;
        public int PlayerX;
        public int PlayerY;
        public Vector2 velHolder;

        // public bool currentlyRotated, currentlyRotatedByToRotation, wasAirborn, lerpingToRotation = false; // unused
        // public int timeAirborne = 0; // unused
        public override void PostUpdate()
        {

        }

        private int bubbleTimer = 6;
        private int bubbleLen = 0;
        private int dur = 0;
        private int bubbleColumn;
        public bool isHoldingGlider;
        public Vector2 currentAltarPos;
        public bool isInSubworld;

        /*public override void UpdateVanityAccessories()
        {
            if (hydroGear || dragonScale)
            {
                Player.accFlipper = true;
            }

            if (hydroGear)
            {
                Player.accDivingHelm = true;
            }

            if (dragonScale && Player.wet && PlayerInput.Triggers.JustPressed.Jump)
            {
                if (dur <= 0)
                {
                    bubbleColumn = 0;
                    dur = 36;
                }
            }
        }*/

        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            /*if (junk)
            {
                return;
            }*/

            /*if (ZoneCoralReefs)
            {
                if (Main.rand.NextFloat() < 0.01f && questFish == ItemType<BlueTang>())
                {
                    caughtType = ItemType<BlueTang>();
                }

                if (Main.rand.NextFloat() < 0.01f && questFish == ItemType<Spiritfish>() && Main.hardMode)
                {
                    caughtType = ItemType<Spiritfish>();
                }

                /*if (Main.rand.NextFloat() < 0.01f && questFish == ItemType<GlitteringPearlfish>() && downedCoralGolem)
                {
                    caughtType = ItemType<GlitteringPearlfish>();
                }

                if (Main.rand.NextFloat() < 0.01f && questFish == ItemType<Ironfin>() && downedTalos)
                {
                    caughtType = ItemType<Ironfin>();
                }

                if (Main.rand.NextFloat() < 0.01f)
                {
                    caughtType = ItemType<LunaJellyItem>();
                }

                if (Main.rand.NextFloat() < 0.1f)
                {
                    caughtType = ItemType<Barracuda>();
                }

                if (Main.rand.NextFloat() < 0.4f)
                {
                    caughtType = ItemType<ReeftailMinnow>();
                }

                if (Main.rand.NextFloat() < 0.4f)
                {
                    caughtType = ItemType<Coralfin>();
                }
            }*/
        }

        /*public override bool CustomBiomesMatch(Player other)
        {
            EEPlayer modOther = other.GetModPlayer<EEPlayer>();
            return ZoneCoralReefs == modOther.ZoneCoralReefs;
        }

        public override void CopyCustomBiomesTo(Player other)
        {
            EEPlayer modOther = other.GetModPlayer<EEPlayer>();
            modOther.ZoneCoralReefs = ZoneCoralReefs;
        }

        public override void SendCustomBiomes(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = ZoneCoralReefs;
            writer.Write(flags);
        }

        public override void ReceiveCustomBiomes(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            ZoneCoralReefs = flags[0];
        }*/

        public bool isHangingOnVine;

        public override bool CloneNewInstances => false; // just in case something doesn't reset

        // TODO: move some of the logic and stop calling this as it's called during PLAYER SELECTION SCREEN
        public override void Initialize()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                SpireCutscene = 0;
                try
                {
                    if (Main.gameMenu)
                        isInSubworld = false;
                    else
                        isInSubworld = Main.ActiveWorldFileData.Path.Contains($@"{Main.SavePath}\Worlds\{Main.LocalPlayer.GetModPlayer<EEPlayer>().baseWorldName}Subworlds");
                }
                catch
                {

                }
                for (int i = 0; i < arrayPoints.Length; i++)
                {
                    arrayPoints[i] = new Vector2(mainPoint.X + (i * displaceX), mainPoint.Y + (i * displaceY));
                }
                isPickingUp = false;
                quickOpeningFloat = 20;
                EEMod.AscentionHandler = 0;
                EEMod.startingTextHandler = 0;
                // EEMod.isAscending = false;
                EEMod.AscentionHandler = 0;
                isSaving = false;
                godMode = false;
                timerForCutscene = 0;
                seamapUpdateCount = 0;
                arrowFlag = false;
                noU = false;
                triggerSeaCutscene = false;
                cutSceneTriggerTimer = 0;
                position = Player.Center;
                importantCutscene = false;
                speedOfPan = 0;
                subTextAlpha = 0;

                if (SeamapObjects.localship != null)
                {
                    SeamapObjects.localship.position = new Vector2(1700, 900);
                    SeamapObjects.localship.shipHelth = SeamapObjects.localship.ShipHelthMax;
                }

                displacmentX = 0;
                displacmentY = 0;
                startingText = false;
                isCameraFixating = false;
            }
        }

        public override void ResetEffects()
        {
            isQuartzChestOn = false;
            isQuartzRangedOn = false;
            isQuartzMeleeOn = false;
            isQuartzSummonOn = false;
            ResetMinionEffect();
            isSaving = false;
            //lythenSet = false;

            //hydriteSet = false;
            //hydrofluoricSet = false;
        }

        private float displacmentX = 0;
        private float displacmentY = 0;
        public bool isCameraFixating;
        public bool isCameraShaking;
        public Vector2 fixatingPoint;
        public float fixatingSpeedInv;
        public int intensity;
        private int runeCooldown = 0;

        private readonly Dictionary<int, bool[]> RuneData = new Dictionary<int, bool[]>()
        {
            {0,new []{false,false }},
            {1,new []{false,false }},
            {2,new []{false,false }},
            {3,new []{false,false }},
            {4,new []{false,false }},
            {5,new []{false,false }},
            {6,new []{false,false }},
        };

        public void FixateCameraOn(Vector2 fixatingPointCamera, float fixatingSpeed, bool isCameraShakings, bool CameraMove, int intensity)
        {
            fixatingPoint = fixatingPointCamera;
            isCameraFixating = CameraMove;
            fixatingSpeedInv = fixatingSpeed;
            isCameraShaking = isCameraShakings;
            this.intensity = intensity;
        }

        public void TurnCameraFixationsOff()
        {
            isCameraFixating = false;
            isCameraShaking = false;
            fixatingPoint.X = Player.Center.X;
            fixatingPoint.Y = Player.Center.Y;
        }

        public override void ModifyScreenPosition()
        {
            if (Shake > 0)
            {
                Main.screenPosition.Y += Main.rand.Next(-Shake, Shake);
                Main.screenPosition.X += Main.rand.Next(-Shake, Shake);
                Shake--;
            }

            int clamp = 80;
            float disSpeed = .4f;
            base.ModifyScreenPosition();

            EEMod.UpdateAmbience();

            if (Main.ActiveWorldFileData.Name == KeyID.Cutscene1)
            {
                if (seamapUpdateCount < 120 * 8)
                {
                    displacmentX -= (displacmentX - (200 * 16)) / 32f;
                    displacmentY -= (displacmentY - (110 * 16)) / 32f;
                    Main.screenPosition += new Vector2(displacmentX - Player.Center.X, displacmentY - Player.Center.Y);
                    Player.position = Player.oldPosition;
                }
                else
                {
                    startingText = true;
                    Filters.Scene[RippleShader].GetShader().UseOpacity(timerForCutscene);
                    if (Main.netMode != NetmodeID.Server && !Filters.Scene[RippleShader].IsActive())
                    {
                        Filters.Scene.Activate(RippleShader, Player.Center).GetShader().UseOpacity(timerForCutscene);
                    }
                    Main.screenPosition += new Vector2(displacmentX - Player.Center.X, displacmentY - Player.Center.Y);
                    displacmentX -= (displacmentX - Player.Center.X) / 16f;
                    displacmentY -= (displacmentY - Player.Center.Y) / 16f;
                    timerForCutscene += 10;
                    if (timerForCutscene > 1000)
                    {
                        timerForCutscene = 1000;
                    }

                    if (seamapUpdateCount >= (120 * 8) + 1400)
                    {
                        if (Main.netMode != NetmodeID.Server && Filters.Scene[RippleShader].IsActive())
                        {
                            Filters.Scene[RippleShader].Deactivate();
                        }
                        if (Main.netMode != NetmodeID.Server && !Filters.Scene["EEMod:WhiteFlash"].IsActive())
                        {
                            //  Filters.Scene.Activate("EEMod:WhiteFlash", player.Center).GetShader().UseOpacity(markerPlacer - ((120 * 8) + 1400));
                        }

                        /*Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
                        Main.spriteBatch.Draw(GetTexture("EEMod/Projectiles/Nice"), player.Center.ForDraw(), new Rectangle(0, 0, 174, 174), Color.White * (markerPlacer - ((120 * 8) + 1400)) * 0.05f, (markerPlacer - ((120 * 8) + 1400)) / 10, new Rectangle(0, 0, 174, 174).Size() / 2, markerPlacer - ((120 * 8) + 1400), SpriteEffects.None, 0);
                        Main.spriteBatch.End();*/

                        //  Filters.Scene["EEMod:WhiteFlash"].GetShader().UseOpacity(markerPlacer - ((120 * 8) + 1400));
                    }
                    if (seamapUpdateCount >= (120 * 8) + 1800)
                    {
                        startingText = false;
                        if (Main.netMode != NetmodeID.Server && Filters.Scene["EEMod:WhiteFlash"].IsActive())
                        {
                            //    Filters.Scene["EEMod:WhiteFlash"].Deactivate();
                        }

                        Initialize();
                        SM.Return(KeyID.BaseWorldName);
                    }
                }
            }

            if (Main.worldName != KeyID.Sea && Main.ActiveWorldFileData.Name != KeyID.Cutscene1 && EEModConfigClient.Instance.CamMoveBool)
            {
                if (Player.velocity.X > 1)
                {
                    displacmentX += disSpeed;
                }
                else if (Player.velocity.X < -1)
                {
                    displacmentX -= disSpeed;
                }
                else
                {
                    displacmentX -= displacmentX / 16f;
                }
                if (Player.velocity.Y > 1)
                {
                    displacmentY += disSpeed / 2;
                }
                else if (Player.velocity.Y < -1)
                {
                    displacmentY -= disSpeed / 2;
                }
                else
                {
                    displacmentY -= displacmentY / 16f;
                }
                displacmentX = Helpers.Clamp(displacmentX, -clamp, clamp);
                displacmentY = Helpers.Clamp(displacmentY, -clamp, clamp);
                Main.screenPosition += new Vector2(displacmentX, displacmentY);
            }

            if (Main.worldName == KeyID.Sea)
            {
                Player.position = Player.oldPosition;
                if (seamapUpdateCount > 1)
                {
                    //Main.screenPosition += new Vector2(0, offSea);
                    //SeamapObjects.localship.ModifyScreenPosition(ref Main.screenPosition);
                }
            }
            if (cutSceneTriggerTimer > 0 && triggerSeaCutscene)
            {
                if (!Main.gamePaused)
                {
                    speedOfPan += 0.01f;
                }

                Main.screenPosition.X += cutSceneTriggerTimer * speedOfPan;
            }
            if (isCameraFixating)
            {
                displacmentX += (fixatingPoint.X - Player.Center.X - displacmentX) / fixatingSpeedInv;
                displacmentY += (fixatingPoint.Y - Player.Center.Y - displacmentY) / fixatingSpeedInv;
                Main.screenPosition += new Vector2(displacmentX, displacmentY);
            }
            else if (Main.ActiveWorldFileData.Name != KeyID.Cutscene1 && Math.Abs(displacmentX + displacmentY) > 0.01f)
            {
                displacmentX *= 0.95f;
                displacmentY *= 0.95f;
                Main.screenPosition += new Vector2(displacmentX, displacmentY);
            }
            else
            {
                displacmentX = 0;
                displacmentY = 0;
                Main.screenPosition += new Vector2(displacmentX, displacmentY);
            }
            if (isCameraShaking)
            {
                Main.screenPosition += new Vector2(Main.rand.Next(-intensity, intensity), Main.rand.Next(-intensity, intensity));
            }
        }

        Vector2 mainPoint => new Vector2(Player.Center.X, Player.position.Y);

        private float inspectTimer = 0;

        public void InspectObject()
        {
            Main.spriteBatch.Draw(EEMod.Instance.Assets.Request<Texture2D>("UI/InspectIcon").Value, (Player.Center + new Vector2(0, (float)Math.Sin(inspectTimer) * 32)).ForDraw(), Color.White);
            inspectTimer += 0.5f;
        }
        public void UpdateVerletCollisions(int pRP, float velDamp, int fakeElevation, int newFeetPos, float gradientFunction)
        {
            foreach (Verlet.Stick stick in Verlet.stickPoints)
            {
                Rectangle pRect = new Rectangle((int)Player.position.X - pRP, (int)Player.position.Y - pRP, Player.width + pRP, Player.height + pRP);
                Vector2 Vec1 = Verlet.Points[stick.a].point;
                Vector2 Vec2 = Verlet.Points[stick.b].point;
                int Y = Vec1.Y < Vec2.Y ? (int)Vec1.Y : (int)Vec2.Y;
                int X = Vec1.X < Vec2.X ? (int)Vec1.X : (int)Vec2.X;
                int Y1 = Y == (int)Vec1.Y ? (int)Vec2.Y : (int)Vec1.Y;
                int X1 = X == (int)Vec1.X ? (int)Vec2.X : (int)Vec1.X;

                Rectangle vRect = new Rectangle(X - pRP, Y - pRP, X1 - X + pRP, Y1 - Y + pRP);
                if (pRect.Intersects(vRect))
                {
                    float perc = (Player.Center.X - Vec1.X) / (Vec2.X - Vec1.X);
                    float yTarget = Vec1.Y + (Vec2.Y - Vec1.Y) * perc + fakeElevation;
                    float feetPos = Player.position.Y + Player.height;
                    float grad = (Vec2.Y - Vec1.Y) / (Vec2.X - Vec1.X);
                    grad *= gradientFunction;
                    if (feetPos - 5 - Player.velocity.Y < yTarget && feetPos > yTarget)
                    {
                        Player.velocity.Y = 0;
                        Player.gravity = 0f;
                        Player.position.Y = yTarget - (newFeetPos - grad * Player.direction * Math.Abs(Player.velocity.X / velDamp));
                        Player.bodyFrameCounter += Math.Abs(velocity.X) * 0.5f;
                        while (Player.bodyFrameCounter > 8.0)
                        {
                            Player.bodyFrameCounter -= 8.0;
                            Player.bodyFrame.Y += Player.bodyFrame.Height;
                        }
                        if (Player.bodyFrame.Y < Player.bodyFrame.Height * 7)
                        {
                            Player.bodyFrame.Y = Player.bodyFrame.Height * 19;
                        }
                        else if (Player.bodyFrame.Y > Player.bodyFrame.Height * 19)
                        {
                            Player.bodyFrame.Y = Player.bodyFrame.Height * 7;
                        }
                    }
                }
            }
        }
        public override void UpdateEquips()
        {


            seamapLightColor = MathHelper.Clamp((Seamap.SeamapContent.Seamap.isStorming ? 1 : 2 / 3f) + Seamap.SeamapContent.Seamap.brightness, 0.333f, 2f);
            /*
            int minibiome = 0;
            for (int k = 0; k < EESubWorlds.MinibiomeLocations.Count; k++)
            {
                if (Vector2.DistanceSquared(new Vector2(EESubWorlds.MinibiomeLocations[k].X, EESubWorlds.MinibiomeLocations[k].Y), new Vector2(player.Center.X / 16, player.Center.Y / 16)) < (220 * 220) && EESubWorlds.MinibiomeLocations[k].Z != 0)
                {
                    minibiome = (int)EESubWorlds.MinibiomeLocations[k].Z;
                    break;
                }
            }*/
            // Main.NewText(minibiome);
            var zonePlayer = Player.GetModPlayer<EEZonePlayer>();

            EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.03f));
            if (zonePlayer.reefMinibiomeID == MinibiomeID.AquamarineCaverns)
                EEMod.MainParticles.SpawnParticleDownUp(Main.LocalPlayer, -Vector2.UnitY * 3, null, Color.Lerp(new Color(78, 125, 224), new Color(107, 2, 81), Main.rand.NextFloat(0, 1)), GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/RadialGradient").Value, new SimpleBrownianMotion(0.2f), new AfterImageTrail(0.5f), new RotateVelocity(Main.rand.NextFloat(-0.002f, 0.002f)), new SetLightingBlend(true));

            EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.08f));
            if (zonePlayer.reefMinibiomeID == MinibiomeID.KelpForest)
            {
                float gottenParalax = Main.rand.NextFloat(1, 1.5f);
                EEMod.MainParticles.SpawnParticleDownUp(Main.LocalPlayer, -Vector2.UnitY * 3, GetInstance<EEMod>().Assets.Request<Texture2D>("Particles/ForegroundParticles/KelpLeaf").Value, gottenParalax, 1 - (gottenParalax - 1) / 1.2f, new RotateVelocity(Main.rand.NextFloat(-0.002f, 0.002f)), new RotateTexture(0.03f), new SetLightingBlend(true), new SetAnimData(6, 5));
            }
            if (playingGame)
            {
                Player.velocity = Vector2.Zero;
            }
            //UpdateVerletCollisions(1, 3f, 10, 54, 1.6f);
            if (Player.GetModPlayer<RedVelvetCapePlayer>().isWearingCape)
            {
                UpdateArrayPoints();
            }
            thermalHealingTimer--;
            if (Player.HasBuff(BuffType<ThermalHealing>()) && thermalHealingTimer <= 0)
            {
                Player.statLife++;
                thermalHealingTimer = 30;
            }
            UpdateRunes();
            UpdateSets();
            UpdatePowerLevel();

            if (dur > 0)
            {
                bubbleTimer--;
                if (bubbleTimer <= 0)
                {
                    bubbleTimer = 6;
                    if (Player.wet)
                    {
                        Projectile.NewProjectile(new ProjectileSource_BySourceId(ProjectileType<WaterDragonsBubble>()), new Vector2(Player.Center.X + bubbleLen - 16, Player.Center.Y - bubbleColumn), new Vector2(0, -1), ProjectileType<WaterDragonsBubble>(), 5, 0, Owner: Player.whoAmI);
                    }

                    bubbleLen = Main.rand.Next(-16, 17);
                    bubbleColumn += 2;
                }
                dur--;
            }

            // EEMod.isSaving = false;
            if (Main.worldName != KeyID.Sea)
            {
                if (triggerSeaCutscene && cutSceneTriggerTimer <= 500)
                {
                    cutSceneTriggerTimer += 2;
                    Player.position = Player.oldPosition;
                }
                if (godMode)
                {
                    timerForCutscene += 20;
                }
            }
            switch (Main.worldName)
            {
                case KeyID.CoralReefs:
                    break;
                case KeyID.Sea:
                    break;
                default:
                    UpdateWorld();
                    break;
            }

            UpdateCutscenesAndTempShaders();

            base.UpdateEquips();
        }

        public bool playingGame;
        public float seamapLightColor;

        public void UpdateArrayPoints()
        {
            float acc = arrayPoints.Length;
            float upwardDrag = 0.2f;
            float smoothStepSpeed = 8;
            float yDis = 15;
            float propagtionSpeedWTRdisX = 15;
            float propagtionSpeedWTRvelY = 4;
            float basePosFluncStatic = 5f;
            float basePosFlunc = 3f;
            propagation += (Math.Abs(Player.velocity.X / 2f) * 0.015f) + 0.1f;
            for (int i = 0; i < acc; i++)
            {
                float prop = (float)Math.Sin(propagation + (i * propagtionSpeedWTRdisX / acc));
                Vector2 basePos = new Vector2(mainPoint.X + (i * displaceX) + (Math.Abs(Player.velocity.X / basePosFluncStatic) * i), mainPoint.Y + (i * displaceY) + 20);
                float dist = Player.position.Y + yDis - basePos.Y + prop / acc * Math.Abs(-Math.Abs(Player.velocity.X) - (i / acc));
                float amp = Math.Abs(Player.velocity.X * basePosFlunc) * (i * basePosFlunc / acc) + 1f;
                float goTo = Math.Abs(dist * (Math.Abs(Player.velocity.X) * upwardDrag)) + (Player.velocity.Y / propagtionSpeedWTRvelY * i);
                float disClamp = (goTo - dis[i]) / smoothStepSpeed;
                disClamp = MathHelper.Clamp(disClamp, -1.7f, 15);
                dis[i] += disClamp;
                if (i == 0)
                {
                    arrayPoints[i] = basePos;
                }
                else
                {
                    arrayPoints[i] = new Vector2(basePos.X, basePos.Y + prop / acc * amp - dis[i] + i * 2);
                }

                if (Player.direction == 1)
                {
                    float distX = arrayPoints[i].X - Player.Center.X;
                    arrayPoints[i].X = Player.Center.X - distX;
                }
                int tracker = 0;
                if (i != 0)
                {
                    Tile tile = Framing.GetTileSafely((int)arrayPoints[i].X / 16, (int)arrayPoints[i].Y / 16);
                    while (tile.IsActive &&
                            Main.tileSolid[tile.type]
                           || !Collision.CanHit(new Vector2(arrayPoints[i].X, arrayPoints[i].Y), 1, 1, new Vector2(arrayPoints[i - 1].X, arrayPoints[i - 1].Y), 1, 1))
                    {
                        arrayPoints[i].Y--;
                        tracker++;
                        if (tracker >= displaceY * acc)
                        {
                            break;
                        }

                        if (arrayPoints[i].Y <= arrayPoints[i - 1].Y - 4)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public void UpdateRunes()
        {
            if (runeCooldown > 0) runeCooldown--;

            bool[][] states = new bool[][] { new bool[] { false, false }, new bool[] { true, false }, new bool[] { true, true } };
            for (int i = 0; i < hasGottenRuneBefore.Length; i++)
            {
                if (hasGottenRuneBefore[i] == 1)
                {
                    RuneData.TryGetValue(i, out states[(int)StateID.RetrievedButNotEquiped]);
                    if (inPossesion[i] == 1)
                    {
                        RuneData.TryGetValue(i, out states[(int)StateID.Equiped]);
                    }
                }
                else
                {
                    RuneData.TryGetValue(i, out states[(int)StateID.Nothing]);
                }
                if (RuneData[i] == states[(int)StateID.Equiped])
                {
                    switch ((RuneID)i)
                    {
                        case RuneID.SandRune:
                        {
                            if (EEMod.RuneSpecial.JustPressed && runeCooldown == 0)
                            {
                                runeCooldown = 180;
                            }
                            else
                            {

                            }
                            break;
                        }
                        case RuneID.WaterRune:
                        {
                            if (EEMod.RuneSpecial.JustPressed && runeCooldown == 0)
                            {
                                if (bubbleRuneBubble == 0)
                                {
                                    bubbleRuneBubble = Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_BySourceId(ProjectileType<BubblingWatersBubble>()), Player.Center, Vector2.Zero, ProjectileType<BubblingWatersBubble>(), 0, 0, Main.myPlayer);
                                }
                                else
                                {
                                    Main.projectile[bubbleRuneBubble].Kill();
                                }
                                runeCooldown = 600;
                            }
                            else
                            {
                                if (Player.wet)
                                {
                                    Player.gravity = 0;
                                    if (Player.controlUp)
                                        Player.gravity = -0.2f;
                                    if (Player.controlDown)
                                        Player.gravity = 0.1f;
                                    if (!Player.controlUp && !Player.controlDown)
                                        Player.gravity = -0.1f;
                                }
                            }
                            break;
                        }
                        case RuneID.LeafRune:
                        {
                            if (EEMod.RuneSpecial.JustPressed && runeCooldown == 0)
                            {

                            }
                            else
                            {

                            }
                            break;
                        }
                        case RuneID.FireRune:
                        {
                            if (EEMod.RuneSpecial.JustPressed && runeCooldown == 0)
                            {
                                runeCooldown = 180;
                            }
                            else
                            {

                            }

                            break;
                        }
                        case RuneID.IceRune:
                        {
                            for (int j = 0; j < Main.npc.Length - 1; j++)
                            {
                                NPC npc = Main.npc[j];
                                if (!npc.active)
                                    continue;
                                if (Vector2.Distance(npc.Center, Player.Center) <= 256)
                                {
                                    npc.AddBuff(BuffID.Slow, 30);

                                    Texture2D tex = Mod.Assets.Request<Texture2D>("EEMod/Projectiles/Runes/PermafrostSnowflake").Value;
                                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
                                    Main.spriteBatch.Draw(tex, new Vector2(npc.Center.X, npc.Center.Y - npc.height / 2 - 32), tex.Bounds, Color.White, 0, tex.Bounds.Size() / 2, 1, SpriteEffects.None, 0);
                                    Main.spriteBatch.End();
                                }
                            }

                            if (EEMod.RuneSpecial.JustPressed && runeCooldown == 0)
                            {
                                runeCooldown = 180;
                            }
                            else
                            {

                            }
                            break;
                        }
                        case RuneID.SkyRune:
                        {
                            if (EEMod.RuneSpecial.JustPressed && runeCooldown == 0)
                            {
                                Player.velocity -= new Vector2((Player.Center.X - Main.MouseWorld.X) / 32, 16 * (Main.MouseWorld.Y > Player.Center.Y ? -1 : 1));
                                runeCooldown = 300;
                            }
                            else
                            {
                                Player.dash = 3;
                            }

                            break;
                        }
                    }
                }
            }
            //synergies
            if (RuneData[(int)RuneID.SandRune] == states[(int)StateID.Equiped])
            {

            }
        }

        public void UpdateSets()
        {
            /*if (hydrofluoricSet)
            {
                hydrofluoricSetTimer++;
                if (hydrofluoricSetTimer >= 30 && Player.velocity != Vector2.Zero)
                {
                    Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_BySourceId(ProjectileType<CorrosiveBubble>()), Player.Center, Player.velocity / 2, ProjectileType<CorrosiveBubble>(), 20, 0f);
                    hydrofluoricSetTimer = 0;
                }
            }*/

            /*if (lythenSet)
            {
                lythenSetTimer++;
                if (lythenSetTimer >= 480)
                {
                    NPC closest = null;
                    float closestDistance = 9999999;
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.active && npc.Distance(position) < closestDistance)
                        {
                            closest = npc;
                            closestDistance = npc.Distance(position);
                        }
                    }
                    if (closest != null)
                    {
                        Projectile.NewProjectile(new Terraria.DataStructures.ProjectileSource_BySourceId(ProjectileType<CyanoburstTomeKelp>()), closest.Center, Vector2.Zero, ProjectileType<CyanoburstTomeKelp>(), 10, 0f, Owner: Player.whoAmI);
                    }

                    lythenSetTimer = 0;
                }
            }*/

            /*if (hydriteSet)
            {
                Player.gills = true;
            }*/
        }

        public void UpdateZipLines()
        {
            EEPlayer eEPlayer = this; //Main.LocalPlayer.GetModPlayer<EEPlayer>();
            if (eEPlayer.ridingZipline)
            {
                Vector2 begin = eEPlayer.PylonBegin;
                Vector2 end = eEPlayer.PylonEnd;
                Main.LocalPlayer.velocity = Vector2.Normalize(end - begin) * zipMultiplier;
                Main.LocalPlayer.gravity = 0;
                Main.LocalPlayer.AddBuff(BuffID.Cursed, 2, true);
                if (zipMultiplier <= 30)
                {
                    zipMultiplier *= 1.02f;
                }
            }
            if (Vector2.DistanceSquared(Main.LocalPlayer.position, eEPlayer.PylonEnd) <= 32 * 32 && eEPlayer.ridingZipline)
            {
                int i;
                for (i = 0; i <= 100; i++)
                {
                    if (i < 99 && EEWorld.EEWorld.PylonEnd[i] == EEWorld.EEWorld.PylonBegin[i + 1] && EEWorld.EEWorld.PylonEnd[i + 1] != default && eEPlayer.PylonBegin == EEWorld.EEWorld.PylonBegin[i] && eEPlayer.PylonEnd == EEWorld.EEWorld.PylonEnd[i])
                    {
                        break;
                    }
                }

                if (i >= 99)
                {
                    //Leaving zipline
                    eEPlayer.PylonBegin = default;
                    eEPlayer.PylonEnd = default;
                    // eEPlayer.ridingZipline = false;
                    zipMultiplier = 1;
                }
                else
                {
                    //Continue on zipline
                    eEPlayer.PylonBegin = EEWorld.EEWorld.PylonEnd[i];
                    eEPlayer.PylonEnd = EEWorld.EEWorld.PylonEnd[i + 1];
                    eEPlayer.ridingZipline = true;
                }
            }
        }

        public void UpdatePowerLevel()
        {
            if (Player.controlUseItem)
            {
                powerLevel += 0.2f;
                if (powerLevel > maxPowerLevel)
                {
                    powerLevel = maxPowerLevel;
                }
            }
            else
            {
                powerLevel = 0;
            }
        }

        public override Texture2D GetMapBackgroundImage()
        {
            if (Player.GetModPlayer<EEZonePlayer>().ZoneCoralReefs)
            {
                return EEMod.Instance.Assets.Request<Texture2D>("Backgrounds/CoralReefsSurfaceClose").Value;
            }
            return null;
        }

        public override void SaveData(TagCompound tag)
        {
            tag = new TagCompound {
                ["hasGottenRuneBefore"] = hasGottenRuneBefore,
                ["baseworldname"] = baseWorldName,
                ["importantCutscene"] = importantCutscene,
                ["swiftSail"] = boatSpeed,
                ["cannonball"] = cannonballType,
                ["fishLengthsKeys"] = fishLengths.Keys.ToList(),
                ["fishLengthsValues"] = fishLengths.Values.ToList(),
                ["firstLoad"] = firstLoad
                /*
             {"Hours", Hours},
		     {"Minutes", Minutes},
		     {"Seconds", Seconds},
		     {"Milliseconds", Milliseconds},
             */
            };
        }

        public override void LoadData(TagCompound tag)
        {
            tag.TryGetByteArrayRef("hasGottenRuneBefore", ref hasGottenRuneBefore);
            tag.TryGetRef("baseworldname", ref baseWorldName);
            tag.TryGetRef("importantCutscene", ref importantCutscene);
            tag.TryGetRef("swiftSail", ref boatSpeed);
            tag.TryGetRef("cannonball", ref cannonballType);
            var fishLengthsKeys = new List<int>();
            var fishLengthsValues = new List<int>();
            tag.TryGetRef("fishLengthsKeys", ref fishLengthsKeys);
            tag.TryGetRef("fishLengthsValues", ref fishLengthsValues);
            tag.TryGetRef("firstLoad", ref firstLoad);
            fishLengths = fishLengthsKeys.Zip(fishLengthsValues, (k, v) => new { fishLengthsKeys = k, fishLengthsValues = v }).ToDictionary(d => d.fishLengthsKeys, d => d.fishLengthsValues);
            /*
                if (tag.ContainsKey("Hours"))
		           Hours = tag.GetInt("Hours");
		       if (tag.ContainsKey("Minutes"))
		            Minutes = tag.GetInt("Minutes");
		       if (tag.ContainsKey("Seconds"))
		           Seconds = tag.GetInt("Seconds");
		      if (tag.ContainsKey("Milliseconds"))
		          Milliseconds = tag.GetInt("Milliseconds");
                  */
        }

        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            /*if (dalantiniumSet)
            {
                for (int i = 0; i < 3; i++)
                {
                    Projectile.NewProjectile(player.Center, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2)), ProjectileType<DalantiniumFang>(), 12, 2f);
                }
            }*/
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (godMode)
            {
                int getRand = Main.rand.Next(5);
                int healSet = Helpers.Clamp(damage / 9, 1, 5);

                if (getRand == 1)
                {
                    Player.statLife += healSet;
                    Player.HealEffect(healSet);
                }
            }
            if (isQuartzRangedOn && item.DamageType == DamageClass.Ranged)
            {
                if (crit)
                {
                    target.AddBuff(BuffID.CursedInferno, 120);
                }
            }
            if (isQuartzSummonOn && item.DamageType == DamageClass.Summon)
            {
                if (Main.rand.Next(10) < 3)
                {
                    target.AddBuff(BuffID.OnFire, 180);
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (isQuartzRangedOn && proj.DamageType == DamageClass.Ranged)
            {
                if (crit)
                {
                    target.AddBuff(BuffID.CursedInferno, 120);
                }
            }
            if (isQuartzSummonOn && proj.minion)
            {
                if (Main.rand.Next(10) < 3)
                {
                    target.AddBuff(BuffID.OnFire, 180);
                }
            }
        }

        private void ResetMinionEffect()
        {
            quartzCrystal = false;
        }

        public int hours;
        public int minutes;
        public int seconds;
        public int milliseconds;

        public override void PreUpdate()
        {
            if (Main.frameRate != 0)
            {
                milliseconds += 1000 / Main.frameRate;
            }

            if (milliseconds >= 1000)
            {
                milliseconds = 0;
                seconds++;
            }
            if (seconds >= 60)
            {
                seconds = 0;
                minutes++;
            }
            if (minutes >= 60)
            {
                minutes = 0;
                hours++;
            }
        }

        /*public override void ModifyDrawInfo(ref PlayerDrawInfo drawInfo)
        {
            if (!Main.gameMenu)
            {
                if (player.wet)
                {
                    if (drawInfo.drawPlayer.fullRotation < MathHelper.ToRadians(90) && drawInfo.drawPlayer.fullRotation > MathHelper.ToRadians(-90))
                    {
                        if (drawInfo.drawPlayer.direction == 1 && Main.MouseWorld.X > drawInfo.drawPlayer.position.X)
                        {
                            drawInfo.drawPlayer.headRotation = Utils.Clamp((Main.MouseWorld - drawInfo.drawPlayer.Center).ToRotation(), -0.5f, 0.5f);
                        }
                        else if (drawInfo.drawPlayer.direction == -1 && Main.MouseWorld.X < drawInfo.drawPlayer.position.X)
                        {
                            drawInfo.drawPlayer.headRotation = Utils.Clamp((drawInfo.drawPlayer.Center - Main.MouseWorld).ToRotation(), -0.5f, 0.5f);
                        }
                    }
                }
            }
        }*/

        /*private Vector2 leftClickPos;
        private Vector2 rightClickPos;
        public static readonly PlayerLayer CubicBezier = new PlayerLayer("EEMod", "CubicBezier", PlayerLayer.MiscEffectsBack, delegate (PlayerDrawInfo drawInfo) 
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }

            Player player = drawInfo.drawPlayer;

            Vector2 leftClickPos = player.GetModPlayer<EEPlayer>().leftClickPos;
            Vector2 rightClickPos = player.GetModPlayer<EEPlayer>().rightClickPos;

            if (leftClickPos != Vector2.Zero && rightClickPos != Vector2.Zero)
            {
                DrawData clown1 = new DrawData(GetTexture("EEMod/Particles/Clownfish"), leftClickPos - Main.screenPosition, new Rectangle(0, 0, 48, 32), Color.White, 0, new Rectangle(0, 0, 48, 32).Size() / 2f, 1f, SpriteEffects.None, default);
                Main.playerDrawData.Add(clown1);
                DrawData clown2 = new DrawData(GetTexture("EEMod/Particles/Clownfish"), rightClickPos - Main.screenPosition, new Rectangle(0, 0, 48, 32), Color.White, 0, new Rectangle(0, 0, 48, 32).Size() / 2f, 1f, SpriteEffects.None, default);
                Main.playerDrawData.Add(clown2);

                for (float lerpVal = 0; lerpVal < 1; lerpVal += 0.05f)
                {
                    Vector2 origin = player.Center;
                    Vector2 cp1 = leftClickPos;
                    Vector2 cp2 = rightClickPos;
                    Vector2 end = Main.MouseWorld;

                    Vector2 p1 = Vector2.Lerp(origin, cp1, lerpVal);
                    Vector2 p2 = Vector2.Lerp(cp1, cp2, lerpVal);
                    Vector2 p3 = Vector2.Lerp(cp2, end, lerpVal);

                    Vector2 s1 = Vector2.Lerp(p1, p2, lerpVal);
                    Vector2 s2 = Vector2.Lerp(p2, p3, lerpVal);

                    Vector2 finalPos = Vector2.Lerp(s1, s2, lerpVal);

                    DrawData data = new DrawData(GetTexture("EEMod/Particles/Fish"), finalPos - Main.screenPosition, new Rectangle(0, 0, 36, 22), Color.White, 0, new Rectangle(0, 0, 36, 22).Size() / 2f, 1f, SpriteEffects.None, default);
                    Main.playerDrawData.Add(data);
                }
            }
        });*/


        public override void ModifyDrawLayerOrdering(IDictionary<PlayerDrawLayer, PlayerDrawLayer.Position> positions)
        {


            /*CubicBezier.visible = true;
            layers.Insert(0, CubicBezier);

            if (player.controlUseItem)
            {
                leftClickPos = Main.MouseWorld;
                Main.NewText("Left click");
            }
            if (player.controlUseTile)
            {
                rightClickPos = Main.MouseWorld;
                Main.NewText("Right click");
            }*/
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {

        }
    }
}
