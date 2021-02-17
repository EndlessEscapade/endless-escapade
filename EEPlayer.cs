using EEMod.Autoloading;
using EEMod.Buffs.Buffs;
using EEMod.Config;
using EEMod.Extensions;
using EEMod.ID;
using EEMod.Items.Fish;
using EEMod.Projectiles;
using EEMod.Projectiles.Armor;
using EEMod.Projectiles.Mage;
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

namespace EEMod
{
    public partial class EEPlayer : ModPlayer
    {
        public int Shake = 0;
        public bool importantCutscene;
        public static bool startingText;
        public bool godMode = false;

        //Biome checks
        public bool ZoneCoralReefs;
        public bool HasVisitedSpire;
        public bool[] reefMinibiome = new bool[7];
        public bool aquamarineSetBonus = true;
        public int aquamarineCooldown;
        public bool isLight;
        public Vector2 aquamarineVel;
        public bool ZoneTropicalIsland;

        //Equipment booleans
        public bool hydroGear;
        public bool dragonScale;
        public bool lythenSet;
        public int lythenSetTimer;
        public bool dalantiniumSet;
        public bool hydriteSet;
        public bool hydrofluoricSet;
        public int hydrofluoricSetTimer;
        public bool dalantiniumHood;
        public bool hydriteVisage;
        public bool quartzCrystal = false;
        public bool isQuartzRangedOn = false;
        public bool isQuartzSummonOn = false;
        public bool isQuartzMeleeOn = false;
        public bool isQuartzChestOn = false;
        public bool FlameSpirit;

        //Runes
        public byte[] hasGottenRuneBefore = new byte[7];
        public byte[] inPossesion = new byte[7];
        public int bubbleRuneBubble = 0;

        //Morality
        public static int moralScore;
        public int initialMoralScore;

        public readonly SubworldManager SM = new SubworldManager();
        public int rippleCount = 3;
        public int rippleSize = 5;
        public int rippleSpeed = 15;
        public int distortStrength = 100;
        public List<ParticlesClass> Particles = new List<ParticlesClass>();
        public List<Vector2> Velocity;
        private static string prevKey = "Main";
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
        public bool isWearingCape = false;
        public string NameForJoiningClients = "";
        public Vector2[] arrayPoints = new Vector2[24];
        public static EEPlayer instance => Main.LocalPlayer.GetModPlayer<EEPlayer>();
        private int Arrow;
        public int Arrow2;
        private float speedOfPan = 1;
        public int offSea = 1000;
        private int opac;
        public int boatSpeed = 1;
        private readonly string shad1 = "EEMod:Ripple";
        private readonly string shad2 = "EEMod:SunThroughWalls";
        private readonly string shad3 = "EEMod:SeaTrans";
        public bool firstFrameVolcano;
        public Vector2 PylonBegin;
        public Vector2 PylonEnd;
        public bool holdingPylon;
        public bool ridingZipline;
        public Vector2 secondPlayerMouse;
        public int PlayerX;
        public int PlayerY;
        public Vector2 velHolder;

        public bool currentlyRotated, currentlyRotatedByToRotation, wasAirborn, lerpingToRotation = false;
        public int timeAirborne = 0;

        public override void PostUpdate()
        {
            /*if (player.wet)
            {
                if (player.fullRotation % MathHelper.ToRadians(-360f) < 1 && player.fullRotation % MathHelper.ToRadians(-360f) > -1 && !lerpingToRotation)
                {
                    player.fullRotation = 0;
                    wasAirborn = false;
                }

                if (player.mount.Type == -1)
                {
                    player.fullRotationOrigin = new Vector2(player.width / 2, player.height / 2);

                    if (player.fullRotation != 0)
                    {
                        currentlyRotated = true;
                    }

                    if ((player.velocity.X != 0 && player.velocity.Y != 0) || (player.velocity.Y != 0 && timeAirborne > 60))
                    {
                        timeAirborne++;

                        if (timeAirborne > 60)
                        {
                            lerpingToRotation = true;
                            player.fullRotation = player.fullRotation.AngleLerp(player.velocity.ToRotation() + (float)Math.PI / 2f, 0.05f);
                            wasAirborn = true;
                        }
                        else
                        {
                            lerpingToRotation = false;
                            wasAirborn = false;
                        }
                    }
                    else
                    {
                        lerpingToRotation = false;

                        if (player.direction == -1)
                        {
                            if (wasAirborn)
                            {
                                player.fullRotation = MathHelper.Lerp(player.fullRotation, 0f, -0.085f);
                            }
                            else
                            {
                                player.fullRotation = 0;
                                timeAirborne = 0;
                            }
                        }
                        else
                        {
                            if (wasAirborn)
                            {
                                player.fullRotation = MathHelper.Lerp(player.fullRotation, 0f, -0.085f);
                            }
                            else
                            {
                                player.fullRotation = 0;
                                timeAirborne = 0;
                            }
                        }

                        if (player.fullRotation == 0)
                        {
                            player.fullRotation += player.velocity.X / 7f;

                            if (player.fullRotation > MathHelper.ToRadians(player.velocity.X))
                            {
                                player.fullRotation = MathHelper.ToRadians(player.velocity.X);
                            }

                            if (player.fullRotation < MathHelper.ToRadians(-player.velocity.X))
                            {
                                player.fullRotation = -MathHelper.ToRadians(-player.velocity.X);
                            }
                        }
                    }
                }
                else
                {
                    if (currentlyRotated)
                    {
                        player.fullRotation = 0f;
                        currentlyRotated = false;
                        wasAirborn = false;
                        lerpingToRotation = false;
                    }
                }
            }*/
        }

        public override void UpdateBiomes()
        {
            ZoneCoralReefs = Main.ActiveWorldFileData.Name == KeyID.CoralReefs;
            if (ZoneCoralReefs)
            {
                opac++;
                if (opac > 100)
                {
                    opac = 100;
                }
                //Filters.Scene.Activate("EEMod:CR").GetShader().UseOpacity(opac);

                int minibiome = 0;
                for (int k = 0; k < EESubWorlds.MinibiomeLocations.Count; k++)
                {
                    if (Vector2.DistanceSquared(new Vector2(EESubWorlds.MinibiomeLocations[k].X, EESubWorlds.MinibiomeLocations[k].Y), player.Center / 16) < (160 * 160) && EESubWorlds.MinibiomeLocations[k].Z != 0)
                    {
                        minibiome = (int)EESubWorlds.MinibiomeLocations[k].Z;
                        break;
                    }
                }

                for (int i = 0; i < reefMinibiome.Length; i++)
                    reefMinibiome[i] = false;

                reefMinibiome[minibiome] = true;
            }
            else
            {
                opac--;
                if (opac < 0)
                {
                    opac = 0;
                }

                for (int i = 0; i < reefMinibiome.Length; i++)
                    reefMinibiome[i] = false;

                //	Filters.Scene.Deactivate("EEMod:CR");
            }
        }

        private int bubbleTimer = 6;
        private int bubbleLen = 0;
        private int dur = 0;
        private int bubbleColumn;
        public bool isHoldingGlider;
        public Vector2 currentAltarPos;
        public bool isInSubworld;
        public override void UpdateVanityAccessories()
        {
            if (hydroGear || dragonScale)
            {
                player.accFlipper = true;
            }

            if (hydroGear)
            {
                player.accDivingHelm = true;
            }

            if (dragonScale && player.wet && PlayerInput.Triggers.JustPressed.Jump)
            {
                if (dur <= 0)
                {
                    bubbleColumn = 0;
                    dur = 36;
                }
            }
        }

        public override void CatchFish(Item fishingRod, Item bait, int power, int liquidType, int poolSize, int worldLayer, int questFish, ref int caughtType, ref bool junk)
        {
            if (junk)
            {
                return;
            }

            if (ZoneCoralReefs)
            {
                if (Main.rand.NextFloat() < 0.01f && questFish == ItemType<BlueTang>())
                {
                    caughtType = ItemType<BlueTang>();
                }

                if (Main.rand.NextFloat() < 0.01f && questFish == ItemType<Spiritfish>() && Main.hardMode)
                {
                    caughtType = ItemType<Spiritfish>();
                }

                if (Main.rand.NextFloat() < 0.01f && questFish == ItemType<GlitteringPearlfish>() && downedCoralGolem)
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
            }
        }

        public override bool CustomBiomesMatch(Player other)
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
        }

        private void MoralFirstFrame()
        {
            switch (player.name)
            {
                case "OS":
                case "EpicCrownKing":
                case "Coolo109":
                case "Pyxis":
                case "phanta":
                case "cynik":
                case "daimgamer":
                case "Thecherrynuke":
                case "Vadim":
                case "Exitium":
                case "Chkylis":
                case "LolXD87":
                case "Nomis":
                case "A44":
                case "Stevie":
                    initialMoralScore += 1000;
                    break;
            }
        }
        public bool isHangingOnVine;
        private void Moral()
        {
            moralScore = 0;
            moralScore += initialMoralScore;
            moralScore -= WorldGen.totalEvil * 30;
            if (WorldGen.totalEvil == 0)
            {
                moralScore += 1000;
            }
            //Main.NewText(moralScore);
        }

        public override void Initialize()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
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
                EEMod.isAscending = false;
                EEMod.AscentionHandler = 0;
                isSaving = false;
                godMode = false;
                timerForCutscene = 0;
                markerPlacer = 0;
                arrowFlag = false;
                noU = false;
                triggerSeaCutscene = false;
                cutSceneTriggerTimer = 0;
                position = player.Center;
                importantCutscene = false;
                speedOfPan = 0;
                subTextAlpha = 0;
                if (SeamapPlayerShip.localship != null)
                {
                    SeamapPlayerShip.localship.position = new Vector2(1700, 900);
                    SeamapPlayerShip.localship.shipHelth = SeamapPlayerShip.ShipHelthMax;
                }
                SeaObject.Clear();
                MoralFirstFrame();
                displacmentX = 0;
                displacmentY = 0;
                startingText = false;
                Particles.Clear();
                OceanMapElements.Clear();
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
            dragonScale = false;
            hydroGear = false;
            lythenSet = false;
            dalantiniumSet = false;
            hydriteSet = false;
            hydrofluoricSet = false;
            isWearingCape = false;
            aquamarineSetBonus = false;
        }

        public void ReturnHome()
        {
            Initialize();
            SM.SaveAndQuit(KeyID.BaseWorldName);
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
            fixatingPoint.X = player.Center.X;
            fixatingPoint.Y = player.Center.Y;
        }

        public override void ModifyScreenPosition()
        {
            Main.screenPosition.Y += Main.rand.Next(-Shake, Shake);
            Main.screenPosition.X += Main.rand.Next(-Shake, Shake);
            if (Shake > 0) { Shake--; }
            
            int clamp = 80;
            float disSpeed = .4f;
            base.ModifyScreenPosition();

            if (Main.ActiveWorldFileData.Name == KeyID.Cutscene1)
            {
                if (markerPlacer < 120 * 8)
                {
                    displacmentX -= (displacmentX - (200 * 16)) / 32f;
                    displacmentY -= (displacmentY - (110 * 16)) / 32f;
                    Main.screenPosition += new Vector2(displacmentX - player.Center.X, displacmentY - player.Center.Y);
                    player.position = player.oldPosition;
                }
                else
                {
                    startingText = true;
                    Filters.Scene[shad1].GetShader().UseOpacity(timerForCutscene);
                    if (Main.netMode != NetmodeID.Server && !Filters.Scene[shad1].IsActive())
                    {
                        Filters.Scene.Activate(shad1, player.Center).GetShader().UseOpacity(timerForCutscene);
                    }
                    Main.screenPosition += new Vector2(displacmentX - player.Center.X, displacmentY - player.Center.Y);
                    displacmentX -= (displacmentX - player.Center.X) / 16f;
                    displacmentY -= (displacmentY - player.Center.Y) / 16f;
                    timerForCutscene += 10;
                    if (timerForCutscene > 1000)
                    {
                        timerForCutscene = 1000;
                    }

                    if (markerPlacer >= (120 * 8) + 1400)
                    {
                        if (Main.netMode != NetmodeID.Server && Filters.Scene[shad1].IsActive())
                        {
                            Filters.Scene[shad1].Deactivate();
                        }
                        if (Main.netMode != NetmodeID.Server && !Filters.Scene["EEMod:WhiteFlash"].IsActive())
                        {
                            //  Filters.Scene.Activate("EEMod:WhiteFlash", player.Center).GetShader().UseOpacity(markerPlacer - ((120 * 8) + 1400));
                        }

                        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
                        Main.spriteBatch.Draw(GetTexture("EEMod/Projectiles/Nice"), player.Center.ForDraw(), new Rectangle(0, 0, 174, 174), Color.White * (markerPlacer - ((120 * 8) + 1400)) * 0.05f, (markerPlacer - ((120 * 8) + 1400)) / 10, new Rectangle(0, 0, 174, 174).Size() / 2, markerPlacer - ((120 * 8) + 1400), SpriteEffects.None, 0);
                        Main.spriteBatch.End();
                        //  Filters.Scene["EEMod:WhiteFlash"].GetShader().UseOpacity(markerPlacer - ((120 * 8) + 1400));
                    }
                    if (markerPlacer >= (120 * 8) + 1800)
                    {
                        startingText = false;
                        if (Main.netMode != NetmodeID.Server && Filters.Scene["EEMod:WhiteFlash"].IsActive())
                        {
                            //    Filters.Scene["EEMod:WhiteFlash"].Deactivate();
                        }

                        Initialize();
                        SM.SaveAndQuit(KeyID.BaseWorldName);
                    }
                }
            }
            if (Main.worldName != KeyID.Sea && Main.ActiveWorldFileData.Name != KeyID.Cutscene1 && EEModConfigClient.Instance.CamMoveBool)
            {
                if (player.velocity.X > 1)
                {
                    displacmentX += disSpeed;
                }
                else if (player.velocity.X < -1)
                {
                    displacmentX -= disSpeed;
                }
                else
                {
                    displacmentX -= displacmentX / 16f;
                }
                if (player.velocity.Y > 1)
                {
                    displacmentY += disSpeed / 2;
                }
                else if (player.velocity.Y < -1)
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
                player.position = player.oldPosition;
                if (markerPlacer > 1)
                {
                    Main.screenPosition += new Vector2(0, offSea);
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
                displacmentX += (fixatingPoint.X - player.Center.X - displacmentX) / fixatingSpeedInv;
                displacmentY += (fixatingPoint.Y - player.Center.Y - displacmentY) / fixatingSpeedInv;
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

        Vector2 mainPoint => new Vector2(player.Center.X, player.position.Y);

        private float inspectTimer = 0;

        public void InspectObject()
        {
            Main.spriteBatch.Draw(ModContent.GetInstance<EEMod>().GetTexture("UI/InspectIcon"), (player.Center + new Vector2(0, (float)Math.Sin(inspectTimer) * 32)).ForDraw(), Color.White);
            inspectTimer += 0.5f;
        }
        public void UpdateVerletCollisions(int pRP, float velDamp, int fakeElevation, int newFeetPos, float gradientFunction)
        {
            foreach (Verlet.Stick stick in Verlet.stickPoints)
            {
                Rectangle pRect = new Rectangle((int)player.position.X - pRP, (int)player.position.Y - pRP, player.width + pRP, player.height + pRP);
                Vector2 Vec1 = Verlet.Points[stick.a].point;
                Vector2 Vec2 = Verlet.Points[stick.b].point;
                int Y = Vec1.Y < Vec2.Y ? (int)Vec1.Y : (int)Vec2.Y;
                int X = Vec1.X < Vec2.X ? (int)Vec1.X : (int)Vec2.X;
                int Y1 = Y == (int)Vec1.Y ? (int)Vec2.Y : (int)Vec1.Y;
                int X1 = X == (int)Vec1.X ? (int)Vec2.X : (int)Vec1.X;

                Rectangle vRect = new Rectangle(X - pRP, Y - pRP, X1 - X + pRP, Y1 - Y + pRP);
                if (pRect.Intersects(vRect))
                {
                    float perc = (player.Center.X - Vec1.X) / (Vec2.X - Vec1.X);
                    float yTarget = Vec1.Y + (Vec2.Y - Vec1.Y) * perc + fakeElevation;
                    float feetPos = player.position.Y + player.height;
                    float grad = (Vec2.Y - Vec1.Y) / (Vec2.X - Vec1.X);
                    grad *= gradientFunction;
                    if (feetPos - 5 - player.velocity.Y < yTarget && feetPos > yTarget)
                    {
                        player.velocity.Y = 0;
                        player.gravity = 0f;
                        player.position.Y = yTarget - (newFeetPos - grad * player.direction * Math.Abs(player.velocity.X / velDamp));
                        player.bodyFrameCounter += Math.Abs(velocity.X) * 0.5f;
                        while (player.bodyFrameCounter > 8.0)
                        {
                            player.bodyFrameCounter -= 8.0;
                            player.bodyFrame.Y += player.bodyFrame.Height;
                        }
                        if (player.bodyFrame.Y < player.bodyFrame.Height * 7)
                        {
                            player.bodyFrame.Y = player.bodyFrame.Height * 19;
                        }
                        else if (player.bodyFrame.Y > player.bodyFrame.Height * 19)
                        {
                            player.bodyFrame.Y = player.bodyFrame.Height * 7;
                        }
                    }
                }
            }
        }

        public bool playingGame;
        public float seamapLightColor;
        public override void UpdateBiomeVisuals()
        {
            if (aquamarineSetBonus)
            {
                if (isLight)
                {
                    player.gravity = 0;

                    if (Math.Abs(player.velocity.X) <= 0.01) aquamarineVel.X = -aquamarineVel.X * 1.25f;

                    if (Math.Abs(player.velocity.Y) <= 0.01) aquamarineVel.Y = -aquamarineVel.Y * 1.25f;

                    player.velocity = aquamarineVel;

                    aquamarineCooldown++;
                    if (aquamarineCooldown >= 600 || (player.controlUp && aquamarineCooldown >= 30))
                    {
                        isLight = false;
                        aquamarineCooldown = 30;
                        player.gravity = 1;
                    }
                }
                else
                {
                    aquamarineCooldown--;
                    if(player.controlUp && aquamarineCooldown <= 0)
                    {
                        isLight = true;

                        aquamarineVel = Vector2.Normalize(Main.MouseWorld - player.Center) * 24;
                    }
                }
            }


            seamapLightColor = MathHelper.Clamp((isStorming ? 1 : 2 / 3f) + brightness, 0.333f, 2f);
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
            EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.03f));
            EEPlayer eeplayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            if (eeplayer.reefMinibiome[(int)MinibiomeID.CrystallineCaves])
                EEMod.Particles.Get("Main").SpawnParticleDownUp(Main.LocalPlayer, -Vector2.UnitY * 3, null, Color.Lerp(new Color(78, 125, 224), new Color(107, 2, 81), Main.rand.NextFloat(0, 1)), GetInstance<EEMod>().GetTexture("Masks/RadialGradient"), new SimpleBrownianMotion(0.2f), new AfterImageTrail(0.5f), new RotateVelocity(Main.rand.NextFloat(-0.002f, 0.002f)), new SetLightingBlend(true));
            EEMod.Particles.Get("Main").SetSpawningModules(new SpawnRandomly(0.08f));
            if (eeplayer.reefMinibiome[(int)MinibiomeID.KelpForest])
            {
                float gottenParalax = Main.rand.NextFloat(1, 1.5f);
                EEMod.Particles.Get("Main").SpawnParticleDownUp(Main.LocalPlayer, -Vector2.UnitY * 3, GetInstance<EEMod>().GetTexture("ForegroundParticles/KelpLeaf"), gottenParalax, 1 - (gottenParalax - 1)/1.2f, new RotateVelocity(Main.rand.NextFloat(-0.002f, 0.002f)), new RotateTexture(0.03f), new SetLightingBlend(true), new SetAnimData(6, 5));
            }
            if (playingGame)
            {
                player.velocity = Vector2.Zero;
            }
            //UpdateVerletCollisions(1, 3f, 10, 54, 1.6f);
            if (isWearingCape)
            {
                UpdateArrayPoints();
            }
            thermalHealingTimer--;
            if (player.HasBuff(BuffType<ThermalHealing>()) && thermalHealingTimer <= 0)
            {
                player.statLife++;
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
                    if (player.wet)
                    {
                        Projectile.NewProjectile(new Vector2(player.Center.X + bubbleLen - 16, player.Center.Y - bubbleColumn), new Vector2(0, -1), ProjectileType<WaterDragonsBubble>(), 5, 0, Owner: player.whoAmI);
                    }

                    bubbleLen = Main.rand.Next(-16, 17);
                    bubbleColumn += 2;
                }
                dur--;
            }
            Moral();

            EEMod.isSaving = false;
            if (Main.worldName != KeyID.Sea)
            {
                if (triggerSeaCutscene && cutSceneTriggerTimer <= 500)
                {
                    cutSceneTriggerTimer += 2;
                    player.position = player.oldPosition;
                }
                if (godMode)
                {
                    timerForCutscene += 20;
                }
            }
            switch (Main.worldName)
            {
                case KeyID.Pyramids:
                {
                    UpdatePyramids();
                    break;
                }
                case KeyID.Sea:
                {
                    UpdateSea();
                    break;
                }
                case KeyID.CoralReefs:
                {
                    UpdateCR();
                    break;
                }
                case KeyID.Island:
                {
                    UpdateIsland();
                    break;
                }
                case KeyID.VolcanoIsland:
                {
                    UpdateVolcano();
                    break;
                }
                case KeyID.VolcanoInside:
                {
                    UpdateInnerVolcano();
                    break;
                }
                case KeyID.Cutscene1:
                {
                    UpdateCutscene();
                    break;
                }
                default:
                {
                    UpdateWorld();
                    break;
                }
            }
            UpdateCutscenesAndTempShaders();
        }

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
            propagation += (Math.Abs(player.velocity.X / 2f) * 0.015f) + 0.1f;
            for (int i = 0; i < acc; i++)
            {
                float prop = (float)Math.Sin(propagation + (i * propagtionSpeedWTRdisX / acc));
                Vector2 basePos = new Vector2(mainPoint.X + (i * displaceX) + (Math.Abs(player.velocity.X / basePosFluncStatic) * i), mainPoint.Y + (i * displaceY) + 20);
                float dist = player.position.Y + yDis - basePos.Y + prop / acc * Math.Abs(-Math.Abs(player.velocity.X) - (i / acc));
                float amp = Math.Abs(player.velocity.X * basePosFlunc) * (i * basePosFlunc / acc) + 1f;
                float goTo = Math.Abs(dist * (Math.Abs(player.velocity.X) * upwardDrag)) + (player.velocity.Y / propagtionSpeedWTRvelY * i);
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

                if (player.direction == 1)
                {
                    float distX = arrayPoints[i].X - player.Center.X;
                    arrayPoints[i].X = player.Center.X - distX;
                }
                int tracker = 0;
                if (i != 0)
                {
                    while ((Main.tile[(int)arrayPoints[i].X / 16, (int)arrayPoints[i].Y / 16].active() &&
                            Main.tileSolid[Main.tile[(int)arrayPoints[i].X / 16, (int)arrayPoints[i].Y / 16].type])
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
                                    bubbleRuneBubble = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<BubblingWatersBubble>(), 0, 0, Main.myPlayer);
                                }
                                else
                                {
                                    Main.projectile[bubbleRuneBubble].Kill();
                                }
                                runeCooldown = 600;
                            }
                            else
                            {
                                if (player.wet)
                                {
                                    player.gravity = 0;
                                    if (player.controlUp)
                                        player.gravity = -0.2f;
                                    if (player.controlDown)
                                        player.gravity = 0.1f;
                                    if (!player.controlUp && !player.controlDown)
                                        player.gravity = -0.1f;
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
                            Main.NewText("rune equipped");
                            for (int j = 0; j < Main.npc.Length - 1; j++)
                            {
                                NPC npc = Main.npc[j];
                                if (!npc.active)
                                    continue;
                                if (Vector2.Distance(npc.Center, player.Center) <= 256)
                                {
                                    npc.AddBuff(BuffID.Slow, 30);

                                    Texture2D tex = mod.GetTexture("EEMod/Projectiles/Runes/PermafrostSnowflake");
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
                                player.velocity -= new Vector2((player.Center.X - Main.MouseWorld.X) / 32, 16 * (Main.MouseWorld.Y > player.Center.Y ? -1 : 1));
                                runeCooldown = 300;
                            }
                            else
                            {
                                player.dash = 3;
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
            if (hydrofluoricSet)
            {
                hydrofluoricSetTimer++;
                if (hydrofluoricSetTimer >= 30 && player.velocity != Vector2.Zero)
                {
                    Projectile.NewProjectile(player.Center, player.velocity / 2, ProjectileType<CorrosiveBubble>(), 20, 0f);
                    hydrofluoricSetTimer = 0;
                }
            }

            if (lythenSet)
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
                        Projectile.NewProjectile(closest.Center, Vector2.Zero, ProjectileType<CyanoburstTomeKelp>(), 10, 0f, Owner: player.whoAmI);
                    }

                    lythenSetTimer = 0;
                }
            }

            if (hydriteSet)
            {
                player.gills = true;
            }
        }

        public void UpdateZipLines()
        {
            if (Main.LocalPlayer.GetModPlayer<EEPlayer>().ridingZipline)
            {
                Vector2 begin = Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonBegin;
                Vector2 end = Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonEnd;
                Main.LocalPlayer.velocity = Vector2.Normalize(end - begin) * zipMultiplier;
                Main.LocalPlayer.gravity = 0;
                Main.LocalPlayer.AddBuff(BuffID.Cursed, 2, true);
                if (zipMultiplier <= 30)
                {
                    zipMultiplier *= 1.02f;
                }
            }
            if (Vector2.DistanceSquared(Main.LocalPlayer.position, Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonEnd) <= 32 * 32 && Main.LocalPlayer.GetModPlayer<EEPlayer>().ridingZipline)
            {
                int i;
                for (i = 0; i <= 100; i++)
                {
                    if (i < 99 && EEWorld.EEWorld.PylonEnd[i] == EEWorld.EEWorld.PylonBegin[i + 1] && EEWorld.EEWorld.PylonEnd[i + 1] != default && Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonBegin == EEWorld.EEWorld.PylonBegin[i] && Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonEnd == EEWorld.EEWorld.PylonEnd[i])
                    {
                        break;
                    }
                }

                if (i >= 99)
                {
                    //Leaving zipline
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonBegin = default;
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonEnd = default;
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().ridingZipline = false;
                    zipMultiplier = 1;
                }
                else
                {
                    //Continue on zipline
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonBegin = EEWorld.EEWorld.PylonEnd[i];
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonEnd = EEWorld.EEWorld.PylonEnd[i + 1];
                    Main.LocalPlayer.GetModPlayer<EEPlayer>().ridingZipline = true;
                }
            }
        }

        public void UpdatePowerLevel()
        {
            if (player.controlUseItem)
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
            if (ZoneCoralReefs)
            {
                return ModContent.GetInstance<EEMod>().GetTexture("Backgrounds/CoralReefsSurfaceClose");
            }
            return null;
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                ["hasGottenRuneBefore"] = hasGottenRuneBefore,
                ["moral"] = moralScore,
                ["baseworldname"] = baseWorldName,
                ["importantCutscene"] = importantCutscene,
                ["swiftSail"] = boatSpeed,
                ["cannonball"] = cannonballType//,
                //["fishLengths"] = fishLengths
                /*
             {"Hours", Hours},
		     {"Minutes", Minutes},
		     {"Seconds", Seconds},
		     {"Milliseconds", Milliseconds},
             */
            };
        }

        public override void Load(TagCompound tag)
        {
            tag.TryGetByteArrayRef("hasGottenRuneBefore", ref hasGottenRuneBefore);
            tag.TryGetRef("moral", ref moralScore);
            tag.TryGetRef("baseworldname", ref baseWorldName);
            tag.TryGetRef("importantCutscene", ref importantCutscene);
            tag.TryGetRef("swiftSail", ref boatSpeed);
            tag.TryGetRef("cannonball", ref cannonballType);
            /*if (tag.ContainsKey("fishLengths"))
            {
                fishLengths = tag.GetList("fishLengths");
            }*/
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
                    player.statLife += healSet;
                    player.HealEffect(healSet);
                }
            }
            if (isQuartzRangedOn && item.ranged)
            {
                if (crit)
                {
                    target.AddBuff(BuffID.CursedInferno, 120);
                }
            }
            if (isQuartzSummonOn && item.summon)
            {
                if (Main.rand.Next(10) < 3)
                {
                    target.AddBuff(BuffID.OnFire, 180);
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (isQuartzRangedOn && proj.ranged)
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

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            if (isLight)
            {
                for (int i = 0; i < layers.Count; i++)
                {
                    layers[i].visible = false;
                }
            }
        }

        public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (isLight)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
                Main.spriteBatch.Draw(GetTexture("EEMod/Projectiles/Nice"), player.Center.ForDraw(), new Rectangle(0, 0, 174, 174), Color.White * 0.75f, Main.GameUpdateCount / 300f, new Rectangle(0, 0, 174, 174).Size() / 2, 0.5f, SpriteEffects.None, default);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            }
        }
    }
}
