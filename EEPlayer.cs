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
using EEMod.Seamap.Core;
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
        public bool HasVisitedSpire;

        //Runes
        public byte[] hasGottenRuneBefore = new byte[7];
        public byte[] inPossesion = new byte[7];
        public int bubbleRuneBubble = 0;

        public readonly SubworldManager SM = new SubworldManager();
        public float zipMultiplier = 1;
        public bool isPickingUp;

        public Dictionary<int, int> fishLengths = new Dictionary<int, int>();

        public string NameForJoiningClients = "";
        public Vector2[] arrayPoints = new Vector2[24];
        private float speedOfPan = 1;
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

        public bool isHoldingGlider;
        public Vector2 currentAltarPos;
        public bool isInSubworld;

        private float displacmentX = 0;
        private float displacmentY = 0;
        public bool isCameraFixating;
        public bool isCameraShaking;
        public Vector2 fixatingPoint;
        public float fixatingSpeedInv;
        public int intensity;
        private int runeCooldown = 0;
        public bool playingGame;

        public bool isHangingOnVine;

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

        public override bool CloneNewInstances => false; // just in case something doesn't reset

        public override void PostUpdate()
        {

        }

        // TODO: move some of the logic and stop calling this as it's called during PLAYER SELECTION SCREEN
        public override void Initialize()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                try
                {
                    if (Main.gameMenu)
                        isInSubworld = false;

                    // TODO: Clients need to know when they're in a subworld
                    //else
                    //isInSubworld = Main.ActiveWorldFileData.Path.Contains($@"{Main.SavePath}\Worlds\{Main.LocalPlayer.GetModPlayer<EEPlayer>().baseWorldName}Subworlds");
                }
                catch
                {

                }

                SpireCutscene = 0;

                isPickingUp = false;
                quickOpeningFloat = 20;

                isSaving = false;
                timerForCutscene = 0;
                seamapUpdateCount = 0;
                arrowFlag = false;
                noU = false;

                triggerSeaCutscene = false;
                importantCutscene = false;

                cutSceneTriggerTimer = 0;

                speedOfPan = 0;
                subTextAlpha = 0;

                displacmentX = 0;
                displacmentY = 0;
                isCameraFixating = false;

                EEMod.AscentionHandler = 0;
                EEMod.startingTextHandler = 0;
            }
        }

        public override void ResetEffects()
        {
            isSaving = false;
        }

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
                        Player.bodyFrameCounter += Math.Abs(Player.velocity.X) * 0.5f;
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
            var zonePlayer = Player.GetModPlayer<EEZonePlayer>();

            EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.03f));
            if (zonePlayer.reefMinibiomeID == MinibiomeID.AquamarineCaverns)
                EEMod.MainParticles.SpawnParticleDownUp(-Vector2.UnitY * 3, null, Color.Lerp(new Color(78, 125, 224), new Color(107, 2, 81), Main.rand.NextFloat(0, 1)), GetInstance<EEMod>().Assets.Request<Texture2D>("Textures/RadialGradient").Value, new SimpleBrownianMotion(0.2f), new AfterImageTrail(0.5f), new RotateVelocity(Main.rand.NextFloat(-0.002f, 0.002f)), new SetLightingBlend(true));

            EEMod.MainParticles.SetSpawningModules(new SpawnRandomly(0.08f));
            if (zonePlayer.reefMinibiomeID == MinibiomeID.KelpForest)
            {
                float gottenParalax = Main.rand.NextFloat(1, 1.5f);
                EEMod.MainParticles.SpawnParticleDownUp(-Vector2.UnitY * 3, GetInstance<EEMod>().Assets.Request<Texture2D>("Particles/ForegroundParticles/KelpLeaf").Value, gottenParalax, 1 - (gottenParalax - 1) / 1.2f, new RotateVelocity(Main.rand.NextFloat(-0.002f, 0.002f)), new RotateTexture(0.03f), new SetLightingBlend(true), new SetAnimData(6, 5));
            }

            if (playingGame)
            {
                Player.velocity = Vector2.Zero;
            }

            //UpdateVerletCollisions(1, 3f, 10, 54, 1.6f);

            UpdateRunes();

            // EEMod.isSaving = false;
            if (Main.worldName != KeyID.Sea)
            {
                if (triggerSeaCutscene && cutSceneTriggerTimer <= 500)
                {
                    cutSceneTriggerTimer += 2;
                    Player.position = Player.oldPosition;
                }
            }

            if(Main.worldName == KeyID.BaseWorldName)
            {
                UpdateWorld();
            }

            UpdateCutscenesAndTempShaders();

            base.UpdateEquips();
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

        public override Texture2D GetMapBackgroundImage()
        {
            return null;
        }

        public override void SaveData(TagCompound tag)
        {
            tag["hasGottenRuneBefore"] = hasGottenRuneBefore;
            tag["baseworldname"] = baseWorldName;
            tag["importantCutscene"] = importantCutscene;
            tag["fishLengthsKeys"] = fishLengths.Keys.ToList();
            tag["fishLengthsValues"] = fishLengths.Values.ToList();
            tag["lastPos"] = myLastBoatPos;
        }

        public override void LoadData(TagCompound tag)
        {
            tag.TryGetByteArrayRef("hasGottenRuneBefore", ref hasGottenRuneBefore);
            tag.TryGetRef("baseworldname", ref baseWorldName);
            tag.TryGetRef("importantCutscene", ref importantCutscene);

            var fishLengthsKeys = new List<int>();
            var fishLengthsValues = new List<int>();

            tag.TryGetRef("fishLengthsKeys", ref fishLengthsKeys);
            tag.TryGetRef("fishLengthsValues", ref fishLengthsValues);

            tag.TryGetRef("lastPos", ref myLastBoatPos);

            fishLengths = fishLengthsKeys.Zip(fishLengthsValues, (k, v) => new { fishLengthsKeys = k, fishLengthsValues = v }).ToDictionary(d => d.fishLengthsKeys, d => d.fishLengthsValues);
        }
    }
}
