using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.IO;
using Terraria.World.Generation;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Projectiles;
using EEMod.Projectiles.Mage;
using EEMod.Projectiles.OceanMap;
using EEMod.Projectiles.CoralReefs;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader.IO;
using Terraria.GameInput;
using EEMod.NPCs.Bosses.Hydros;
using static Terraria.ModLoader.ModContent;
using EEMod.NPCs;
using EEMod.Tiles;
using EEMod.NPCs.Bosses.Akumo;
using EEMod.NPCs.CoralReefs;
using EEMod.NPCs.Bosses.Kraken;
using EEMod.NPCs.Friendly;
using EEMod.Items;
using EEMod.Buffs.Debuffs;
using EEMod.Buffs.Buffs;
using System.Windows.Forms;
using System.Drawing.Imaging;
using EEMod.Extensions;
using EEMod.Items.Fish;
using EEMod.ID;
using EEMod.Projectiles.Armor;
using static EEMod.EEWorld.EEWorld;
using EEMod.Tiles.Walls;

namespace EEMod
{
    public class EEPlayer : ModPlayer
    {
        public bool importantCutscene;
        public static bool startingText;
        public bool FlameSpirit;
        public bool magmaRune;
        public bool duneRune;
        public bool dalantiniumHood;
        public bool hydriteVisage;
        public bool ZoneCoralReefs;
        public bool ZoneTropicalIsland;
        public bool hydroGear;
        public bool dragonScale;
        public bool lythenSet;
        public int lythenSetTimer;
        public bool dalantiniumSet;
        public bool hydriteSet;
        public bool hydrofluoricSet;
        public int hydrofluoricSetTimer;

        private int opac;
        //public bool Cheese1;
        //public bool Cheese2;
        public int boatSpeed = 1;
        string shad1 = "EEMod:Ripple";
        string shad2 = "EEMod:SunThroughWalls";
        string shad3 = "EEMod:SeaTrans";
        public bool firstFrameVolcano;

        public Vector2 PylonBegin;
        public Vector2 PylonEnd;
        public bool holdingPylon;
        public bool ridingZipline;
        public Vector2 secondPlayerMouse;
        public int PlayerX;
        public int PlayerY;
        public Vector2 velHolder;

        public override void PostUpdate()
        {
            int PlayerX = (int)(player.position.X / 16);
            int PlayerY = (int)(player.position.Y / 16);

            if (Main.tile[PlayerX, PlayerY + 3].type == TileID.Sand)

                if (player.velocity.X >= 3 || player.velocity.X <= -3)

                    Dust.NewDust(player.position + new Vector2(0, 35), 1, 1, 32, 0f, 0f, default, Color.White);


            if (Main.tile[PlayerX, PlayerY + 3].type == TileID.Ebonsand)

                if (player.velocity.X >= 3 || player.velocity.X <= -3)

                    Dust.NewDust(player.position + new Vector2(0, 35), 1, 1, 32, 0f, 0f, default, Color.MediumPurple);


            if (Main.tile[PlayerX, PlayerY + 3].type == TileID.Crimsand)

                if (player.velocity.X >= 3 || player.velocity.X <= -3)

                    Dust.NewDust(player.position + new Vector2(0, 35), 1, 1, 32, 0f, 0f, default, Color.Gray);


            if (Main.tile[PlayerX, PlayerY + 3].type == TileID.Pearlsand)

                if (player.velocity.X >= 3 || player.velocity.X <= -3)

                    Dust.NewDust(player.position + new Vector2(0, 35), 1, 1, 1, 0f, 0f, default, Color.White);


            if (Main.tile[PlayerX, PlayerY + 3].type == TileID.SnowBlock)

                if (player.velocity.X >= 3 || player.velocity.X <= -3)

                    Dust.NewDust(player.position + new Vector2(0, 35), 1, 1, 1, 0f, 0f, default, Color.White);


            if (Main.tile[PlayerX, PlayerY + 3].type == TileID.Ash)

                if (player.velocity.X >= 3 || player.velocity.X <= -3)

                    Dust.NewDust(player.position + new Vector2(0, 35), 1, 1, 1, 0f, 0f, default, Color.Gray);


            if (Main.tile[PlayerX, PlayerY + 3].type == TileType<GemsandTile>())

                if (player.velocity.X >= 3 || player.velocity.X <= -3)

                    Dust.NewDust(player.position + new Vector2(0, 35), 1, 1, 1, 0f, 0f, default, Color.Cyan);


            if (Main.tile[PlayerX, PlayerY + 3].type == TileType<LightGemsandTile>())

                if (player.velocity.X >= 3 || player.velocity.X <= -3)

                    Dust.NewDust(player.position + new Vector2(0, 35), 1, 1, 1, 0f, 0f, default, Color.LightSkyBlue);


            if (Main.tile[PlayerX, PlayerY + 3].type == TileType<DarkGemsandTile>())

                if (player.velocity.X >= 3 || player.velocity.X <= -3)

                    Dust.NewDust(player.position + new Vector2(0, 35), 1, 1, 32, 0f, 0f, default, Color.Blue);


        }

        public override void UpdateBiomes()
        {
            ZoneCoralReefs = Main.ActiveWorldFileData.Name == KeyID.CoralReefs;
            if (ZoneCoralReefs)
            {
                opac++;
                if (opac > 100)
                    opac = 100;
                //Filters.Scene.Activate("EEMod:CR").GetShader().UseOpacity(opac);
            }
            else
            {
                opac--;
                if (opac < 0)
                    opac = 0;
                //	Filters.Scene.Deactivate("EEMod:CR");
            }
        }

        private int bubbleTimer = 6;
        private int bubbleLen = 0;
        private int dur = 0;
        private int bubbleColumn;

        public override void UpdateVanityAccessories()
        {
            if (hydroGear || dragonScale)
                player.accFlipper = true;
            if (hydroGear)
                player.accDivingHelm = true;
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
                return;
            if (ZoneCoralReefs)
            {
                if (Main.rand.NextFloat() < 0.01f && questFish == ItemType<BlueTang>())
                    caughtType = ItemType<BlueTang>();
                if (Main.rand.NextFloat() < 0.01f && questFish == ItemType<Spiritfish>() && Main.hardMode)
                    caughtType = ItemType<Spiritfish>();
                if (Main.rand.NextFloat() < 0.01f && questFish == ItemType<GlitteringPearlfish>() && downedCoralGolem)
                    caughtType = ItemType<GlitteringPearlfish>();
                if (Main.rand.NextFloat() < 0.01f && questFish == ItemType<Ironfin>() && downedTalos)
                    caughtType = ItemType<Ironfin>();
                if (Main.rand.NextFloat() < 0.01f)
                    caughtType = ItemType<LunaJellyItem>();
                if (Main.rand.NextFloat() < 0.1f)
                    caughtType = ItemType<Barracuda>();
                if (Main.rand.NextFloat() < 0.4f)
                    caughtType = ItemType<ReeftailMinnow>();
                if (Main.rand.NextFloat() < 0.4f)
                    caughtType = ItemType<Coralfin>();
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

        public bool godMode = false;
        public bool quartzCrystal = false;
        public bool isQuartzRangedOn = false;
        public bool isQuartzSummonOn = false;
        public bool isQuartzMeleeOn = false;
        public bool isQuartzChestOn = false;
        public int timerForCutscene;
        public bool arrowFlag = false;
        public static bool isSaving;
        public float titleText;
        public float titleText2;
        public float subTextAlpha;
        public bool noU;
        public bool triggerSeaCutscene;
        public int cutSceneTriggerTimer;
        public int cutSceneTriggerTimer2;
        public float cutSceneTriggerTimer3;
        public int coralReefTrans;
        public int markerPlacer;
        public Vector2 position;
        public Vector2 velocity;
        public List<Vector2> objectPos = new List<Vector2>();
        public List<Island> Islands = new List<Island>();
        public bool isNearIsland;
        public bool isNearIsland2;
        public bool isNearVolcano;
        public bool isNearMainIsland;
        public bool isNearCoralReefs;
        public string baseWorldName;
        public byte[] hasGottenRuneBefore = new byte[5];
        public byte[] inPossesion = new byte[5];
        public static int moralScore;
        public int initialMoralScore;

        private void UpdateRuneCollection()
        {

        }

        private void MoralFirstFrame()
        {
            switch (player.name)
            {
                case "OS":
                case "EpicCrownKing":
                case "Coolo109":
                case "Pyxis":
                case "Adarian Virell":
                case "phanta":
                case "cynik":
                case "daimgamer":
                case "Thecherrynuke":
                case "Vadim":
                case "CrackJackery":
                case "Exitium":
                case "Franswal":
                case "Chakylis":
                case "LolXD87":
                case "naka":
                    initialMoralScore += 1000;
                    break;
            }
        }
        private void Moral()
        {
            moralScore = 0;
            moralScore += initialMoralScore;
            moralScore -= (int)WorldGen.totalEvil * 30;
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
                for (int i = 0; i < arrayPoints.Length; i++)
                {
                    arrayPoints[i] = new Vector2(mainPoint.X + (i * displaceX), mainPoint.Y + (i * displaceY));
                }
                isPickingUp = false;
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
                cutSceneTriggerTimer2 = 500;
                position = player.Center;
                speedOfPan = 0;
                subTextAlpha = 0;
                EEMod.instance.position = new Vector2(1700, 900);
                objectPos.Clear();
                Islands.Clear();
                EEMod.ShipHelth = EEMod.ShipHelthMax;
                MoralFirstFrame();
                displacmentX = 0;
                displacmentY = 0;
                startingText = false;
                Particles.Clear();
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
        }
        public static EEPlayer instance => Main.LocalPlayer.GetModPlayer<EEPlayer>();
        int Arrow;
        int Arrow2;
        int Anchors;
        float positionOfGlacX;
        float positionOfGlacY;
        float speedOfPan = 1;
        int AnchorsVolc;
        int AnchorsMain;
        int AnchorsCoral;
        public int offSea = 1000;
        public void ReturnHome()
        {
            Initialize();
            SM.SaveAndQuit(baseWorldName);
        }
        float displacmentX = 0;
        float displacmentY = 0;
        public bool isCameraFixating;
        public bool isCameraShaking;
        public Vector2 fixatingPoint;
        public float fixatingSpeedInv;
        public int intensity;
        int runeCooldown = 0;
        Dictionary<int, bool[]> RuneData = new Dictionary<int, bool[]>()
                                            {
                                                {0,new []{false,false }},
                                                {1,new []{false,false }},
                                                {2,new []{false,false }},
                                                {3,new []{false,false }},
                                                {4,new []{false,false }},
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
                        timerForCutscene = 1000;
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
                        SM.SaveAndQuit(baseWorldName);
                    }
                }
            }
            if (Main.ActiveWorldFileData.Name != KeyID.Sea && Main.ActiveWorldFileData.Name != KeyID.Cutscene1 && EEModConfigClient.Instance.CamMoveBool)
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



            if (Main.ActiveWorldFileData.Name == KeyID.Sea)
            {
                if (markerPlacer > 1)
                    Main.screenPosition += new Vector2(0, offSea);
            }

            if (triggerSeaCutscene && cutSceneTriggerTimer <= 500)
            {
                Main.screenPosition.X -= cutSceneTriggerTimer;
            }
            if (cutSceneTriggerTimer >= 500)
            {
                speedOfPan += 0.005f;
                Main.screenPosition.X -= cutSceneTriggerTimer2 * speedOfPan;
            }
            if (isCameraFixating)
            {
                displacmentX += ((fixatingPoint.X - player.Center.X) - displacmentX) / fixatingSpeedInv;
                displacmentY += ((fixatingPoint.Y - player.Center.Y) - displacmentY) / fixatingSpeedInv;
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
                Main.screenPosition += new Vector2(Main.rand.Next(-intensity, intensity), Main.rand.Next(-intensity, intensity));
        }
        readonly SubworldManager SM = new SubworldManager();
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

        /*   public System.Drawing.Bitmap CaptureFromScreen(System.Drawing.Rectangle rect)
           {
               System.Drawing.Bitmap bmpScreenCapture = null;

               if (rect == System.Drawing.Rectangle.Empty)//capture the whole screen
               {
                   rect = Screen.PrimaryScreen.Bounds;
               }

               bmpScreenCapture = new System.Drawing.Bitmap(rect.Width, rect.Height);

               System.Drawing.Graphics p = System.Drawing.Graphics.FromImage(bmpScreenCapture);


               p.CopyFromScreen(rect.X,
                        rect.Y,
                        0, 0,
                        rect.Size,
                       System.Drawing.CopyPixelOperation.SourceCopy);


               p.Dispose();

               return bmpScreenCapture;
           }
           private Texture2D GetTextureSc(GraphicsDevice dev, System.Drawing.Bitmap bmp)
           {
               int[] imgData = new int[bmp.Width * bmp.Height];
               Texture2D texture = new Texture2D(dev, bmp.Width, bmp.Height);

               unsafe
               {
                   // lock bitmap
                   BitmapData origdata =
                       bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);

                   uint* byteData = (uint*)origdata.Scan0;

                   // Switch bgra -> rgba
                   for (int i = 0; i < imgData.Length; i++)
                   {
                       byteData[i] = (byteData[i] & 0x000000ff) << 16 | (byteData[i] & 0x0000FF00) | (byteData[i] & 0x00FF0000) >> 16 | (byteData[i] & 0xFF000000);
                   }

                   // copy data
                   System.Runtime.InteropServices.Marshal.Copy(origdata.Scan0, imgData, 0, bmp.Width * bmp.Height);

                   byteData = null;

                   // unlock bitmap
                   bmp.UnlockBits(origdata);
               }

               texture.SetData(imgData);

               return texture;
           }*/
        public static Texture2D ScTex;
        public Vector2[] arrayPoints = new Vector2[24];
        Vector2 mainPoint => new Vector2(player.Center.X, player.position.Y);
        public struct Island
        {
            public Island(Vector2 pos, Texture2D tex, bool canCollide = false)
            {
                posX = (int)pos.X;
                posY = (int)pos.Y;
                texture = tex;
                this.canCollide = canCollide;
            }
            int posX;
            int posY;
            public bool canCollide;
            public int posXToScreen
            {
                get => posX + (int)Main.screenPosition.X + Main.screenWidth;
            }
            public int posYToScreen
            {
                get => posY + (int)Main.screenPosition.Y + Main.screenHeight;
            }
            public Texture2D texture;
            public Vector2 posToScreen => new Vector2(posXToScreen - texture.Width / 2, posYToScreen - texture.Height / 2);
            public Rectangle hitBox => new Rectangle((int)posToScreen.X - texture.Width / 2, (int)posToScreen.Y - texture.Height / 2 + 1000, texture.Width, texture.Height);
        }
        float propagation;
        int displaceX = 2;
        int displaceY = 4;
        float[] dis = new float[51];
        public bool isWearingCape = false;

        public override void UpdateBiomeVisuals()
        {
            
            if (isWearingCape)
            {
                float acc = arrayPoints.Length;
                float upwardDrag = 0.2f;
                propagation += (Math.Abs(player.velocity.X / 2f) * 0.015f) + 0.1f;
                for (int i = 0; i < acc; i++)
                {
                    float prop = (float)Math.Sin(propagation + (i * 10 / acc));
                    Vector2 basePos = new Vector2(mainPoint.X + (i * displaceX) + (Math.Abs(player.velocity.X / 5f) * i), mainPoint.Y + (i * displaceY) + 20);
                    float dist = (player.position.Y + 15) - basePos.Y + (prop / acc) * Math.Abs(-Math.Abs(player.velocity.X) - (i / acc));
                    float amp = Math.Abs(player.velocity.X * 3) * ((i * 3) / acc) + 1f;
                    float goTo = Math.Abs(dist * (Math.Abs(player.velocity.X) * upwardDrag)) + (player.velocity.Y / 4f * i);
                    float disClamp = (goTo - dis[i]) / 8f;
                    disClamp = MathHelper.Clamp(disClamp, -1.7f, 15);
                    dis[i] += disClamp;
                    if (i == 0)
                        arrayPoints[i] = basePos;
                    else
                        arrayPoints[i] = new Vector2(basePos.X, basePos.Y + (prop / acc) * amp - dis[i] + i * 2);
                    if (player.direction == 1)
                    {
                        float distX = arrayPoints[i].X - player.Center.X;
                        arrayPoints[i].X = player.Center.X - distX;
                    }
                    int tracker = 0;
                    if (i != 0)
                    {
                        while ((Main.tile[(int)arrayPoints[i].X / 16, ((int)arrayPoints[i].Y / 16)].active() &&
                                Main.tileSolid[Main.tile[(int)arrayPoints[i].X / 16, ((int)arrayPoints[i].Y / 16)].type])
                               || !Collision.CanHit(new Vector2(arrayPoints[i].X, arrayPoints[i].Y), 1, 1, new Vector2(arrayPoints[i - 1].X, arrayPoints[i - 1].Y), 1, 1))
                        {
                            arrayPoints[i].Y--;
                            tracker++;
                            if (tracker >= displaceY * acc)
                                break;
                            if (arrayPoints[i].Y <= arrayPoints[i - 1].Y - 4)
                                break;
                        }
                    }
                }
            }
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
                        Projectile.NewProjectile(closest.Center, Vector2.Zero, ProjectileType<CyanoburstTomeKelp>(), 10, 0f, Owner: player.whoAmI);
                    lythenSetTimer = 0;
                }
            }

            if (hydriteSet)
            {
                player.gills = true;
            }
            //System.Drawing.Bitmap ScreenTexture = CaptureFromScreen(new System.Drawing.Rectangle(0, 0, 1980, 1080));
            // ScTex = GetTextureSc(Main.graphics.GraphicsDevice, ScreenTexture);
            thermalHealingTimer--;
            if (player.HasBuff(BuffType<ThermalHealing>()) && thermalHealingTimer <= 0)
            {
                player.statLife += 1;
                thermalHealingTimer = 30;
            }

            bool[][] states = new bool[][] { new bool[] { false, false }, new bool[] { true, false }, new bool[] { true, true } };
            for (int i = 0; i < hasGottenRuneBefore.Length; i++)
            {
                if (hasGottenRuneBefore[i] == 1)
                {
                    RuneData.TryGetValue(i, out states[StateID.RetrievedButNotEquiped]);
                    if (inPossesion[i] == 1)
                    {
                        RuneData.TryGetValue(i, out states[StateID.Equiped]);
                    }
                }
                else
                {
                    RuneData.TryGetValue(i, out states[StateID.Nothing]);
                }
                if (RuneData[i] == states[StateID.Equiped])
                {
                    switch (i)
                    {
                        case RuneID.SandRune:
                            {
                                if (EEMod.RuneSpecial.JustPressed && runeCooldown == 0)
                                {
                                    runeCooldown = 180;
                                }
                                else
                                {
                                    player.moveSpeed *= 1.15f;
                                    player.jumpSpeedBoost *= 1.6f;
                                    player.noFallDmg = true;
                                    if (player.wet)
                                    {
                                        player.meleeSpeed *= 1.07f;
                                        player.noKnockback = false;
                                    }
                                }
                                break;
                            }
                        case RuneID.ShroomRune:
                            {
                                if (EEMod.RuneSpecial.JustPressed && runeCooldown == 0)
                                {
                                    runeCooldown = 600;
                                }
                                else
                                {
                                    player.statDefense = (int)(player.statDefense * 1.1f);
                                    player.statDefense += 5;
                                }
                                break;
                            }
                        case RuneID.WaterRune:
                            {
                                if (EEMod.RuneSpecial.JustPressed && runeCooldown == 0)
                                {
                                    runeCooldown = 600;
                                }
                                else
                                {

                                }
                                break;
                            }
                        case RuneID.LeafRune:
                            {
                                if (EEMod.RuneSpecial.JustPressed && runeCooldown == 0)
                                {
                                    runeCooldown = 180;
                                }
                                else
                                {
                                    player.meleeSpeed *= 1.08f;
                                }
                                player.moveSpeed *= 1.06f;
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
                                    player.dash = 3;
                                }
                                player.moveSpeed *= 1.06f;
                                player.statDefense = (int)(player.statDefense * 0.93f);
                                break;
                            }
                    }
                }
            }
            //synergies
            if (RuneData[RuneID.SandRune] == states[StateID.Equiped])
            {

            }
            if (Main.LocalPlayer.GetModPlayer<EEPlayer>().ridingZipline)
            {
                Vector2 begin = Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonBegin;
                Vector2 end = Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonEnd;
                Main.LocalPlayer.velocity = Vector2.Normalize(end - begin) * zipMultiplier;
                Main.LocalPlayer.gravity = 0;
                Main.LocalPlayer.AddBuff(BuffID.Cursed, 2, true);
                if (zipMultiplier <= 30)
                    zipMultiplier *= 1.02f;
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
            if (dur > 0)
            {
                bubbleTimer--;
                if (bubbleTimer <= 0)
                {
                    bubbleTimer = 6;
                    Projectile.NewProjectile(new Vector2(player.Center.X + bubbleLen - 16, player.Center.Y - bubbleColumn), new Vector2(0, -1), ProjectileType<WaterDragonsBubble>(), 5, 0, Owner: player.whoAmI);
                    bubbleLen = Main.rand.Next(-16, 17);
                    bubbleColumn += 2;
                }
                dur--;
            }
            Moral();
            if (player.controlHook)
            {
                for (int i = 0; i < hasGottenRuneBefore.Length; i++)
                    hasGottenRuneBefore[i] = 0;
            }
            EEMod.isSaving = false;
            if (triggerSeaCutscene && cutSceneTriggerTimer <= 1000)
            {
                cutSceneTriggerTimer += 6;
                player.position = player.oldPosition;
            }
            if (cutSceneTriggerTimer >= 1000)
            {
                cutSceneTriggerTimer += 2;
            }
            if (godMode)
            {
                timerForCutscene += 20;
            }

            if (Main.ActiveWorldFileData.Name == KeyID.Pyramids)
            {

                if (!noU)
                    titleText += 0.005f;
                if (titleText >= 1)
                    noU = true;
                if (noU)
                    titleText -= 0.005f;

                if (titleText <= 0)
                    titleText = 0;

                titleText2 = 1;
                if (!arrowFlag)
                {
                    Arrow = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<DesArrowProjectile>(), 0, 0, player.whoAmI);
                    arrowFlag = true;
                }
                DesArrowProjectile desArrowProj = Main.projectile[Arrow].modProjectile as DesArrowProjectile;
                if (player.Center.X / 16 >= Main.spawnTileX - 5 &&
                    player.Center.X / 16 <= Main.spawnTileX + 5 &&
                    player.Center.Y / 16 >= Main.spawnTileY - 5 &&
                    player.Center.Y / 16 <= Main.spawnTileY + 5)
                {
                    if (player.controlUp)
                    {
                        ReturnHome();
                    }
                    desArrowProj.visible = true;
                }
                else
                {
                    desArrowProj.visible = false;
                }
                if (Main.netMode != NetmodeID.Server && Filters.Scene[shad2].IsActive())
                {
                    Filters.Scene.Deactivate(shad2);
                }
            }
            else if (Main.ActiveWorldFileData.Name == KeyID.Sea)
            {

                if (!noU)
                    titleText += 0.005f;
                if (titleText >= 1)
                    noU = true;
                if (noU)
                    titleText -= 0.005f;
                Filters.Scene[shad2].GetShader().UseOpacity(EEMod.instance.position.X);
                if (Main.netMode != NetmodeID.Server && !Filters.Scene[shad2].IsActive())
                {
                    Filters.Scene.Activate(shad2, player.Center).GetShader().UseOpacity(cutSceneTriggerTimer);
                }
                markerPlacer++;
                Islands.Clear();
                Islands.Add(new Island(new Vector2(500, 500), GetTexture("EEMod/Projectiles/OceanMap/Land")));
                Islands.Add(new Island(new Vector2(-400, -400), GetTexture("EEMod/Projectiles/OceanMap/VolcanoIsland"), true));
                Islands.Add(new Island(new Vector2(-700, -300), GetTexture("EEMod/Projectiles/OceanMap/Land"), true));
                Islands.Add(new Island(new Vector2(-500, -200), GetTexture("EEMod/Projectiles/OceanMap/Lighthouse")));
                Islands.Add(new Island(new Vector2(-1000, -400), GetTexture("EEMod/Projectiles/OceanMap/Lighthouse2")));
                Islands.Add(new Island(new Vector2(-300, -100), GetTexture("EEMod/Projectiles/OceanMap/Rock1")));
                Islands.Add(new Island(new Vector2(-800, -150), GetTexture("EEMod/Projectiles/OceanMap/Rock2")));
                Islands.Add(new Island(new Vector2(-200, -300), GetTexture("EEMod/Projectiles/OceanMap/Rock3")));
                Islands.Add(new Island(new Vector2(-100, -40), GetTexture("EEMod/Projectiles/OceanMap/MainIsland"), true));
                Islands.Add(new Island(new Vector2(-300, -600), GetTexture("EEMod/Projectiles/OceanMap/CoralReefsEntrance"), true));
                Islands.Add(new Island(new Vector2(-600, -800), GetTexture("EEMod/Projectiles/OceanMap/Land"), true));

                float pos2X = Main.screenPosition.X + Main.screenWidth - 400;
                float pos2Y = Main.screenPosition.Y + Main.screenHeight - 400 + 1000;
                float pos3X = Main.screenPosition.X + Main.screenWidth - 700;
                float pos3Y = Main.screenPosition.Y + Main.screenHeight - 300 + 1000;
                float pos9X = Main.screenPosition.X + Main.screenWidth - 100;
                float pos9Y = Main.screenPosition.Y + Main.screenHeight - 40 + 1000;
                float pos10X = Main.screenPosition.X + Main.screenWidth - 300;
                float pos10Y = Main.screenPosition.Y + Main.screenHeight - 600 + 1000;
                float pos11X = Main.screenPosition.X + Main.screenWidth - 600;
                float pos11Y = Main.screenPosition.Y + Main.screenHeight - 300 + 500;
                Rectangle rectangle1 = Islands[1].hitBox;
                Rectangle rectangle2 = Islands[2].hitBox;
                Rectangle rectangle3 = Islands[8].hitBox;
                Rectangle rectangle4 = Islands[9].hitBox;
                Rectangle rectangle5 = Islands[10].hitBox;

                Rectangle ShipHitBox = new Rectangle((int)Main.screenPosition.X + (int)EEMod.instance.position.X - 30, (int)Main.screenPosition.Y + (int)EEMod.instance.position.Y - 30 + 1000, 60, 60);
                isNearIsland = false;
                isNearIsland2 = false;
                isNearVolcano = false;
                isNearMainIsland = false;
                isNearCoralReefs = false;


                if (EEMod.ShipHelth <= 0)
                {
                    if (prevKey == baseWorldName || prevKey == "Main")
                    {
                        ReturnHome();
                    }
                    else
                    {
                        Initialize();
                        arrowFlag = false;
                        SM.SaveAndQuit(prevKey);
                    }
                }
                if (rectangle1.Intersects(ShipHitBox))
                {
                    isNearVolcano = true;
                }
                if (rectangle2.Intersects(ShipHitBox))
                {
                    isNearIsland = true;
                }
                if (rectangle3.Intersects(ShipHitBox))
                {
                    isNearMainIsland = true;
                }
                if (rectangle4.Intersects(ShipHitBox))
                {
                    isNearCoralReefs = true;
                }
                if (rectangle5.Intersects(ShipHitBox))
                {
                    isNearIsland2 = true;
                }
                if (!arrowFlag)
                {
                    Anchors = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<Anchor>(), 0, 0, player.whoAmI, pos3X, pos3Y);
                    AnchorsVolc = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<Anchor>(), 0, 0, player.whoAmI, pos2X, pos2Y - 50);
                    AnchorsMain = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<Anchor>(), 0, 0, player.whoAmI, pos9X, pos9Y - 50);
                    AnchorsCoral = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<Anchor>(), 0, 0, player.whoAmI, pos10X, pos10Y - 50);
                    arrowFlag = true;
                }
                if (isNearIsland)
                {
                    subTextAlpha += 0.02f;
                    if (subTextAlpha >= 1)
                        subTextAlpha = 1;
                    (Main.projectile[Anchors].modProjectile as Anchor).visible = true;
                    if (player.controlUp)
                    {
                        Initialize();
                        SM.SaveAndQuit(KeyID.Island);
                        prevKey = KeyID.Island;
                    }
                }
                else
                {
                    (Main.projectile[Anchors].modProjectile as Anchor).visible = false;
                }
                if (isNearIsland2)
                {
                    subTextAlpha += 0.02f;
                    if (subTextAlpha >= 1)
                        subTextAlpha = 1;
                    (Main.projectile[Anchors].modProjectile as Anchor).visible = true;
                    if (player.controlUp)
                    {
                        Initialize();
                        SM.SaveAndQuit(KeyID.Island2);
                        prevKey = KeyID.Island2;
                    }
                }
                else
                {
                    (Main.projectile[Anchors].modProjectile as Anchor).visible = false;
                }
                if (isNearVolcano)
                {
                    subTextAlpha += 0.02f;
                    if (subTextAlpha >= 1)
                        subTextAlpha = 1;
                    (Main.projectile[AnchorsVolc].modProjectile as Anchor).visible = true;
                    if (player.controlUp)
                    {
                        Initialize();
                        SM.SaveAndQuit(KeyID.VolcanoIsland);
                        prevKey = KeyID.VolcanoIsland;
                    }
                }
                else
                {
                    (Main.projectile[AnchorsVolc].modProjectile as Anchor).visible = false;
                }

                if (isNearMainIsland)
                {
                    if (player.controlUp)
                    {
                        ReturnHome();
                        prevKey = baseWorldName;
                    }
                    subTextAlpha += 0.02f;
                    if (subTextAlpha >= 1)
                        subTextAlpha = 1;
                    (Main.projectile[AnchorsMain].modProjectile as Anchor).visible = true;
                }
                else
                {
                    (Main.projectile[AnchorsMain].modProjectile as Anchor).visible = false;
                }
                if (isNearCoralReefs && markerPlacer > 1)
                {
                    subTextAlpha += 0.02f;
                    if (subTextAlpha >= 1)
                        subTextAlpha = 1;
                    (Main.projectile[AnchorsCoral].modProjectile as Anchor).visible = true;
                    if (player.controlUp)
                    {
                        Initialize();
                        SM.SaveAndQuit(KeyID.CoralReefs); // coral reefs
                        prevKey = KeyID.CoralReefs;
                    }
                }
                else
                {
                    (Main.projectile[AnchorsCoral].modProjectile as Anchor).visible = false;
                }
                if (!isNearVolcano && !isNearIsland && !isNearCoralReefs && !isNearMainIsland)
                {
                    subTextAlpha -= 0.02f;
                    if (subTextAlpha <= 0)
                        subTextAlpha = 0;
                }

                for (int j = 0; j < 450; j++)
                {
                    if (Main.projectile[j].type == ProjectileType<PirateShip>() || Main.projectile[j].type == ProjectileType<RedDutchman>() || Main.projectile[j].type == ProjectileType<EnemyCannonball>())
                    {
                        if ((Main.projectile[j].Center - EEMod.instance.position.ForDraw()).Length() < 40 && Main.projectile[j].type != ProjectileType<EnemyCannonball>())
                        {
                            EEMod.ShipHelth -= 1;
                            EEMod.instance.velocity += Main.projectile[j].velocity * 20;
                        }
                        if ((Main.projectile[j].Center - EEMod.instance.position.ForDraw()).Length() < 30 && Main.projectile[j].type == ProjectileType<EnemyCannonball>())
                        {
                            EEMod.ShipHelth -= 1;
                            EEMod.instance.velocity += Main.projectile[j].velocity;
                        }
                    }
                    if (Main.projectile[j].type == ProjectileType<Crate>())
                    {
                        Crate a = Main.projectile[j].modProjectile as Crate;
                        if ((Main.projectile[j].Center - EEMod.instance.position.ForDraw()).Length() < 40 && !a.sinking)
                        {
                            //Crate loot tables go here
                            if (Main.rand.NextBool())
                                player.QuickSpawnItem(ItemID.GoldBar, Main.rand.Next(4, 9));
                            else
                                player.QuickSpawnItem(ItemID.PlatinumBar, Main.rand.Next(4, 9));

                            if (Main.rand.NextBool())
                                player.QuickSpawnItem(ItemID.ApprenticeBait, Main.rand.Next(2, 4));
                            else
                                player.QuickSpawnItem(ItemID.JourneymanBait, 1);

                            player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(0, 2));
                            player.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(0, 100));
                            player.QuickSpawnItem(ItemID.CopperCoin, Main.rand.Next(0, 100));


                            a.sinking = true;
                            a.Sink();
                        }
                    }
                }

                if (markerPlacer == 1)
                {
                    for (int i = 0; i < 400; i++)
                    {
                        int CloudChoose = Main.rand.Next(3);
                        Vector2 CloudPos = new Vector2(Main.rand.NextFloat(Main.screenPosition.X - 200, Main.screenPosition.X + Main.screenWidth), Main.rand.NextFloat(Main.screenPosition.Y + 800, Main.screenPosition.Y + Main.screenHeight + 1000));
                        Vector2 dist = (Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight + 1000)) - CloudPos;
                        if (dist.Length() > 1140)
                        {
                            switch (CloudChoose)
                            {
                                case 0:
                                    {
                                        Projectile.NewProjectile(CloudPos, Vector2.Zero, ProjectileType<DarkCloud1>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                                        Projectile.NewProjectile(CloudPos, Vector2.Zero, ProjectileType<DarkCloud1>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                                        break;
                                    }
                                case 1:
                                    {
                                        Projectile.NewProjectile(CloudPos, Vector2.Zero, ProjectileType<DarkCloud2>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                                        break;
                                    }
                                case 2:
                                    {
                                        Projectile.NewProjectile(CloudPos, Vector2.Zero, ProjectileType<DarkCloud3>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                                        break;
                                    }
                            }
                        }
                    }

                    for (int i = 0; i < Islands.Count; i++)
                        objectPos.Add(Islands[i].posToScreen);

                    //upgrade, pirates, radial
                    for (int i = 0; i < 2; i++)
                    {
                        int GlacierChoose = Main.rand.Next(3);
                        float positionOfGlacXLast = positionOfGlacX;
                        float positionOfGlacYLast = positionOfGlacY;
                        positionOfGlacX = Main.rand.NextFloat(Main.screenPosition.X, Main.screenPosition.X + Main.screenWidth);
                        positionOfGlacY = Main.rand.NextFloat(Main.screenPosition.Y + 1000, Main.screenPosition.Y + Main.screenHeight + 1000);
                        Vector2 dist = new Vector2(positionOfGlacY - positionOfGlacYLast, positionOfGlacXLast - positionOfGlacX);
                        if (dist.Length() > 300)
                        {
                            switch (GlacierChoose)
                            {
                                case 0:
                                    {
                                        // Projectile.NewProjectile(new Vector2(positionOfGlacX, positionOfGlacY), Vector2.Zero, ProjectileType<Glacier>(), 0, 0f, Main.myPlayer, EEMod.velocity.X, EEMod.velocity.Y);
                                        break;
                                    }
                                case 1:
                                    {
                                        // Projectile.NewProjectile(new Vector2(positionOfGlacX, positionOfGlacY), Vector2.Zero, ProjectileType<Glacier2>(), 0, 0f, Main.myPlayer, EEMod.velocity.X, EEMod.velocity.Y);
                                        break;
                                    }
                                case 2:
                                    {
                                        //  Projectile.NewProjectile(new Vector2(positionOfGlacX, positionOfGlacY), Vector2.Zero, ProjectileType<Glacier3>(), 0, 0f, Main.myPlayer, EEMod.velocity.X, EEMod.velocity.Y);
                                        break;
                                    }
                            }
                        }
                    }
                }
                player.position = player.oldPosition;
                player.invis = true;
                player.AddBuff(BuffID.Cursed, 100000);
                if (markerPlacer % 600 == 0)
                {
                    Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<PirateShip>(), 0, 0f, Main.myPlayer, 0, 0);
                    Projectile.NewProjectile(Main.screenPosition + new Vector2(-200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<PirateShip>(), 0, 0f, Main.myPlayer, 0, 0);
                }
                if (markerPlacer % 2400 == 0)
                    NPC.NewNPC((int)Main.screenPosition.X + Main.screenWidth - 200, (int)Main.screenPosition.Y + Main.rand.Next(1000), NPCType<MerchantBoat>());

                if (markerPlacer % 7200 == 0)
                    Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<RedDutchman>(), 0, 0f, Main.myPlayer, 0, 0);

                if (markerPlacer % 800 == 0)
                    Projectile.NewProjectile(Main.screenPosition + new Vector2(-200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<Crate>(), 0, 0f, Main.myPlayer, 0, 0);

                if (markerPlacer % 20 == 0)
                {
                    int CloudChoose = Main.rand.Next(5);
                    switch (CloudChoose)
                    {
                        case 0:
                            {
                                // Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<Cloud1>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                                break;
                            }
                        case 1:
                            {
                                Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<Cloud6>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                                break;
                            }
                        case 2:
                            {
                                // Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<Cloud3>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                                break;
                            }
                        case 3:
                            {
                                Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<Cloud4>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                                break;
                            }
                        case 4:
                            {
                                Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.screenWidth + 200, Main.rand.Next(1000)), Vector2.Zero, ProjectileType<Cloud5>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.6f, 1f), Main.rand.Next(60, 180));
                                break;
                            }
                    }
                }

                if (markerPlacer % 40 == 0)
                {
                    Projectile.NewProjectile(Main.screenPosition + EEMod.instance.position, Vector2.Zero, ProjectileType<RedStrip>(), 0, 0f, Main.myPlayer, EEMod.instance.velocity.X, EEMod.instance.velocity.Y);
                }
            }
            else if (Main.ActiveWorldFileData.Name == KeyID.CoralReefs)
            {
                if (player.position.Y >= 800 * 16 && !player.accDivingHelm)
                {
                    player.AddBuff(BuffType<WaterPressure>(), 60);
                }
                if (HydrosCheck())
                {
                    NPC.NewNPC((int)position.X * 16, (int)position.Y * 16 - 400, NPCType<Hydros>());
                    EEWorld.EEWorld.instance.minionsKilled = 0;
                }
                if (!noU)
                    titleText += 0.005f;
                if (titleText >= 1)
                    noU = true;
                if (noU)
                    titleText -= 0.005f;
                if (titleText <= 0)
                    titleText = 0;
                markerPlacer++;
                if (markerPlacer % 40 == 0)
                {
                    // Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.rand.Next(2000), Main.screenHeight + 200), Vector2.Zero, ProjectileType<CoralBubble>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.2f, 0.5f), Main.rand.Next(100, 180));
                }

                if (!arrowFlag)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                        Arrow2 = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<OceanArrowProjectile>(), 0, 0, Main.myPlayer);
                    player.ClearBuff(BuffID.Cursed);
                    arrowFlag = true;
                }
                if (EESubWorlds.CoralBoatPos == Vector2.Zero)
                {
                    EESubWorlds.CoralBoatPos = new Vector2(200, 48);
                }
                try
                {
                    Projectile oceanarrow = Main.projectile[Arrow2];
                    if (Helpers.PointInRectangle(player.Center / 16, EESubWorlds.CoralBoatPos.X, EESubWorlds.CoralBoatPos.Y + 12, 4, 4))
                    {
                        if (player.controlUp)
                        {
                            Initialize();
                            EEMod.instance.position = new Vector2(Main.screenWidth - 300, Main.screenHeight - 600);
                            SM.SaveAndQuit(KeyID.Sea);
                        }
                        oceanarrow.ai[1] = 1;
                    }
                    else
                    {
                        oceanarrow.ai[1] = 0;
                    }
                }
                catch
                {

                }
            }
            else if (Main.ActiveWorldFileData.Name == KeyID.Island)
            {
                player.ClearBuff(BuffID.Cursed);
                if (!arrowFlag)
                {
                    NPC.NewNPC((Main.maxTilesX / 2) * 16, 75 * 16, NPCType<AtlantisCore>());
                    arrowFlag = true;
                }
            }
            else if (Main.ActiveWorldFileData.Name == KeyID.VolcanoIsland)
            {
                firstFrameVolcano = true;
                player.ClearBuff(BuffID.Cursed);

                if (!arrowFlag)
                {
                    Arrow2 = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<VolcanoArrowProj>(), 0, 0, player.whoAmI);
                    Arrow2 = Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<VolcanoArrowProj>(), 0, 0, player.whoAmI);
                    NPC.NewNPC(600 * 16, 594 * 16, NPCType<VolcanoSmoke>());
                    arrowFlag = true;
                }
                if (SubWorldSpecificVolcanoInsidePos == Vector2.Zero)
                {
                    SubWorldSpecificVolcanoInsidePos = new Vector2(198, 198);
                }
                VolcanoArrowProj voclanoarrow = Main.projectile[Arrow2].modProjectile as VolcanoArrowProj;
                if (Helpers.PointInRectangle(player.Center / 16, SubWorldSpecificVolcanoInsidePos.X - 4, SubWorldSpecificVolcanoInsidePos.Y - 4, 8, 8))
                {
                    if (player.controlUp)
                    {
                        Initialize();
                        SM.SaveAndQuit(KeyID.VolcanoInside);
                    }
                    voclanoarrow.visible = true;
                }
                else
                {
                    voclanoarrow.visible = false;
                }
            }
            else if (Main.ActiveWorldFileData.Name == KeyID.VolcanoInside)
            {
                player.ClearBuff(BuffID.Cursed);
                if (firstFrameVolcano)
                {
                    NPC.NewNPC(200, TileCheck(200, TileType<MagmastoneTile>()), NPCType<Akumo>());
                    firstFrameVolcano = false;
                }
            }
            else if (Main.ActiveWorldFileData.Name == KeyID.Cutscene1)
            {

                markerPlacer++;
                if (markerPlacer == 5)
                {
                    player.AddBuff(BuffID.Cursed, 100000);
                    NPC.NewNPC(193 * 16, (120 - 30) * 16, NPCType<SansSlime>());
                    NPC.NewNPC(207 * 16, (120 - 30) * 16, NPCType<GreenSlimeGoBurr>());
                }
                if (markerPlacer > 120 * 8)
                {
                    if (markerPlacer == 5 && EEModConfigClient.Instance.ParticleEffects)
                    {
                        Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.rand.Next(2000), Main.screenHeight + 200), Vector2.Zero, ProjectileType<Particle>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.2f, 0.5f), Main.rand.Next(100, 180));
                    }
                }
            }
            else
            {
                int lastNoOfShipTiles = missingShipTiles.Count;
                try
                {
                    ShipComplete();
                }
                catch
                {

                }
                if (missingShipTiles.Count != lastNoOfShipTiles)
                {
                    for (int i = 0; i < Main.projectile.Length; i++)
                    {
                        if (Main.projectile[i].type == ProjectileType<WhiteBlock>())
                        {
                            Main.projectile[i].Kill();
                        }
                    }

                    foreach (Vector2 tile in missingShipTiles)
                    {
                        int proj = Projectile.NewProjectile(tile * 16 + new Vector2(8, 8) + new Vector2(-3 * 16, -6 * 16), Vector2.Zero, ProjectileType<WhiteBlock>(), 0, 0);  // here
                        WhiteBlock newProj = Main.projectile[proj].modProjectile as WhiteBlock;
                        newProj.itemTexture = missingShipTilesItems[missingShipTilesRespectedPos.IndexOf(tile)];
                    }
                }
                if (Main.netMode == NetmodeID.Server)
                {
                    var netMessage = mod.GetPacket();
                    netMessage.Write(shipComplete);
                    netMessage.Send();
                }
                if (!importantCutscene)
                {
                    //SM.SaveAndQuit(KeyID.Cutscene1);
                    importantCutscene = true;
                }
                if (EEModConfigClient.Instance.ParticleEffects)
                {
                    markerPlacer++;
                }
                else
                {
                    markerPlacer = 0;
                }
                if (markerPlacer == 10 && EEModConfigClient.Instance.ParticleEffects)
                {
                    Projectile.NewProjectile(Main.screenPosition + new Vector2(Main.rand.Next(2000), Main.screenHeight + 200), Vector2.Zero, ProjectileType<Particle>(), 0, 0f, Main.myPlayer, Main.rand.NextFloat(0.2f, 0.5f), player.whoAmI);
                }
                baseWorldName = Main.ActiveWorldFileData.Name;
                if (Main.netMode != NetmodeID.Server && Filters.Scene[shad2].IsActive())
                {
                    Filters.Scene.Deactivate(shad2);
                }
                EEMod.instance.position = EEMod.start;
                EEMod.instance.velocity = Vector2.Zero;
                titleText2 = 0;
                if (!arrowFlag)
                {
                    if (EEModConfigClient.Instance.BetterLighting)
                        Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<BetterLighting>(), 0, 0f, Main.myPlayer, 0, player.whoAmI);
                    player.ClearBuff(BuffID.Cursed);
                    Arrow = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<DesArrowProjectile>(), 0, 0, player.whoAmI);
                    Arrow2 = Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileType<OceanArrowProjectile>(), 0, 0, player.whoAmI);
                    arrowFlag = true;
                    for (int i = 0; i < 200; i++)
                    {
                        if (Main.projectile[i].type == ProjectileType<WhiteBlock>())
                        {
                            Main.projectile[i].Kill();
                        }
                    }

                    foreach (Vector2 tile in missingShipTiles)
                    {
                        int proj = Projectile.NewProjectile(tile * 16 + new Vector2(8, 8) + new Vector2(-3 * 16, -6 * 16), Vector2.Zero, ProjectileType<WhiteBlock>(), 0, 0);  // here
                        WhiteBlock newProj = Main.projectile[proj].modProjectile as WhiteBlock;
                        newProj.itemTexture = missingShipTilesItems[missingShipTilesRespectedPos.IndexOf(tile)];
                    }
                }
                if (EntracesPosses.Count > 0)
                {
                    if (Main.projectile[Arrow].modProjectile is DesArrowProjectile arrow)
                    {
                        Vector2 entrace = EntracesPosses[0];
                        if (Helpers.PointInRectangle(player.Center / 16, entrace.X + 10, entrace.Y + 5, 4, 4))
                        {
                            if (player.controlUp)
                            {
                                godMode = true;
                            }
                            arrow.visible = true;
                        }
                        else
                        {
                            arrow.visible = false;
                        }
                    }
                }
                Vector2 revisedRee = new Vector2(ree.X == 0 ? 100 : ree.X,
                                                 ree.Y == 0 ? TileCheckWater(100) - 22 : ree.Y);
                if (Helpers.PointInRectangle(player.Center / 16, revisedRee.X, revisedRee.Y + 12, 4, 4) && shipComplete)
                {
                    if (player.controlUp)
                    {
                        triggerSeaCutscene = true;
                        if (Main.netMode == NetmodeID.Server)
                        {
                            var netMessage = mod.GetPacket();
                            netMessage.Write(triggerSeaCutscene);
                            netMessage.Send();
                        }
                    }
                    Main.projectile[Arrow2].ai[1] = 1;

                }
                else
                {
                    Main.projectile[Arrow2].ai[1] = 0;

                }
            }
            Filters.Scene[shad1].GetShader().UseOpacity(timerForCutscene);
            if (Main.netMode != NetmodeID.Server && !Filters.Scene[shad1].IsActive())
            {
                Filters.Scene.Activate(shad1, player.Center).GetShader().UseOpacity(timerForCutscene);
            }
            if (!godMode)
            {
                if (Main.netMode != NetmodeID.Server && Filters.Scene[shad1].IsActive())
                {
                    Filters.Scene.Deactivate(shad1);
                }
            }
            Filters.Scene[shad3].GetShader().UseOpacity(cutSceneTriggerTimer);
            if (Main.netMode != NetmodeID.Server && !Filters.Scene[shad3].IsActive())
            {
                Filters.Scene.Activate(shad3, player.Center).GetShader().UseOpacity(cutSceneTriggerTimer);
            }
            if (!triggerSeaCutscene)
            {
                if (Main.netMode != NetmodeID.Server && Filters.Scene[shad3].IsActive())
                {
                    Filters.Scene.Deactivate(shad3);
                }
            }
            if (timerForCutscene >= 1400)
            {
                Initialize();
                SM.SaveAndQuit(KeyID.Pyramids); //pyramid
            }
            if (cutSceneTriggerTimer >= 500)
            {
                cutSceneTriggerTimer2 -= 5;
                if (cutSceneTriggerTimer >= 1520)
                {
                    Initialize();
                    SM.SaveAndQuit(KeyID.Sea); //sea
                }
            }
        }

        public override Texture2D GetMapBackgroundImage()
        {
            if (ZoneCoralReefs)
            {
                return TextureCache.CoralReefsSurfaceClose;
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
                ["swiftSail"] = boatSpeed
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
            if (tag.ContainsKey("hasGottenRuneBefore"))
            {
                hasGottenRuneBefore = tag.GetByteArray("hasGottenRuneBefore");
            }
            if (tag.ContainsKey("moral"))
            {
                moralScore = tag.GetInt("moral");
            }
            if (tag.ContainsKey("baseworldname"))
            {
                baseWorldName = tag.GetString("baseworldname");
            }
            if (tag.ContainsKey("importantCutscene"))
            {
                importantCutscene = tag.GetBool("importantCutscene");
            }
            if (tag.ContainsKey("swiftSail"))
            {
                boatSpeed = tag.GetInt("swiftSail");
            }
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
            /*if (NPC.AnyNPCs(NPCID.KingSlime))
            {
                Cheese1 = true;
            }
            else
            {
                Cheese1 = false;
            }*/
            if (dalantiniumSet)
                for (int i = 0; i < 3; i++)
                    Projectile.NewProjectile(player.Center, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2)), ProjectileType<DalantiniumFang>(), 12, 2f);
        }
        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {

            if (godMode)
            {
                int getRand = Main.rand.Next(5);
                int healSet = damage / 9;
                if (healSet > 5)
                {
                    healSet = 5;
                }
                if (healSet < 1)
                {
                    healSet = 1;
                }

                if (getRand == 1)
                {
                    player.statLife += healSet;
                    player.HealEffect(healSet);
                }
            }
            if (isQuartzRangedOn && item.ranged)
            {
                if (crit)
                    target.AddBuff(BuffID.CursedInferno, 120);
            }
            if (isQuartzSummonOn && item.summon)
            {
                if (Main.rand.Next(10) < 3)
                    target.AddBuff(BuffID.OnFire, 180);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (isQuartzRangedOn && proj.ranged)
            {
                if (crit)
                    target.AddBuff(BuffID.CursedInferno, 120);
            }
            if (isQuartzSummonOn && proj.minion)
            {
                if (Main.rand.Next(10) < 3)
                    target.AddBuff(BuffID.OnFire, 180);
            }
        }

        public override void clientClone(ModPlayer clientClone)
        {
            EEPlayer clone = clientClone as EEPlayer;
        }

        public Vector2 EEPosition;
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = mod.GetPacket();
            packet.Write(triggerSeaCutscene);
            packet.WriteVector2(EEPosition);
            packet.Send(toWho, fromWho);
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            EEPlayer clone = clientPlayer as EEPlayer;

            if (clone.EEPosition != EEMod.instance.position)
            {
                var packet = mod.GetPacket();
                packet.Write(triggerSeaCutscene);
                packet.WriteVector2(EEPosition);
                packet.Send();
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
            milliseconds += 1000/Main.frameRate;
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
    }
}
