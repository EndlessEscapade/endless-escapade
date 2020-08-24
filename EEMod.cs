using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.World.Generation;
using ReLogic.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using EEMod.Autoloading;
using EEMod.Items.Materials;
using EEMod.Items.Weapons.Mage;
using EEMod.Tiles;
using EEMod.UI;
using System.Reflection.Emit;
using Mono.Cecil.Cil;
using OpCodes = Mono.Cecil.Cil.OpCodes;
using EEMod.Projectiles.Mage;
using EEMod.Projectiles.OceanMap;
using System.IO;
using System.Threading;
using Terraria.IO;
using EEMod.Projectiles;
using EEMod.Projectiles.CoralReefs;
using Terraria.ModLoader.IO;
using Terraria.GameInput;
using EEMod.NPCs.Bosses.Hydros;
using static Terraria.ModLoader.ModContent;
using EEMod.NPCs;
using EEMod.NPCs.Bosses.Akumo;
using EEMod.NPCs.CoralReefs;
using EEMod.NPCs.Bosses.Kraken;
using EEMod.NPCs.Friendly;
using EEMod.Items;
using EEMod.ID;
using EEMod.Net;
using EEMod.Extensions;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace EEMod
{
    public partial class EEMod : Mod
    {
        public static EEMod instance;

        public override void PostSetupContent()
        {

        }
        public static Texture2D ScTex;
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
        public static double worldSurface;

        public static double worldSurfaceLow;

        public static double worldSurfaceHigh;

        public static double rockLayer;

        public static double rockLayerLow;

        public static double rockLayerHigh;

        public static int _lastSeed;

        public UserInterface customResources;

		public UserInterface SpeedrunnTimer;
		internal RunninUI RunUI;

        public static void GenerateWorld(string key, int seed, GenerationProgress customProgressObject = null)
        {
            typeof(EESubWorlds).GetMethod(key).Invoke(null, new object[] { seed, customProgressObject });
        }
        public static Effect NoiseSurfacing;
        public void DrawZipline()
        {
            Vector2 PylonBegin = Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonBegin;
            Vector2 PylonEnd = Main.LocalPlayer.GetModPlayer<EEPlayer>().PylonEnd;
            Main.spriteBatch.Begin();
            Main.spriteBatch.Draw(ModContent.GetTexture("EEMod/Items/ZipCarrier2"), Main.LocalPlayer.position.ForDraw() + new Vector2(0, 6), new Rectangle(0, 0, 2, 16), Color.White, 0, new Rectangle(0, 0, 2, 16).Size() / 2, Vector2.One, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(ModContent.GetTexture("EEMod/Items/ZipCarrier"), Main.LocalPlayer.position.ForDraw(), new Rectangle(0, 0, 18, 8), Color.White, (PylonEnd - PylonBegin).ToRotation(), new Rectangle(0, 0, 18, 8).Size() / 2, Vector2.One, SpriteEffects.None, 0);
            Main.spriteBatch.End();
        }

        public override void Unload()
        {
            //IL.Terraria.IO.WorldFile.SaveWorldTiles -= ILSaveWorldTiles;
            RuneActivator = null;
            ActivateGame = null;
            RuneSpecial = null;
            simpleGame = null;
            ActivateVerletEngine = null;
            NoiseSurfacing = null;
            UnloadIL();
            AutoloadingManager.UnloadManager(this);
            instance = null;
        }
        internal EEUI eeui;
        public UserInterface EEInterface;
        private GameTime lastGameTime;
        int delay;
        float pauseShaderTImer;
        public IceHockey simpleGame;
        public ModPacket GetPacket(EEMessageType type, int capacity)
        {
            ModPacket packet = GetPacket(capacity + 1);
            packet.Write((byte)type);
            return packet;
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            EENet.ReceievePacket(reader, whoAmI);
        }
        int lerps;
        float alphas;
        int delays;
        Verlet verlet = new Verlet();
        bool mode;
        public void UpdateVerlet()
        {
          //  System.Drawing.Bitmap ScreenTexture = CaptureFromScreen(new System.Drawing.Rectangle(0, 0, 1980, 1080));
           // ScTex = GetTextureSc(Main.graphics.GraphicsDevice, ScreenTexture);
           // Main.spriteBatch.Draw(ScTex, Main.LocalPlayer.position, Color.White);
            if (ActivateVerletEngine.JustPressed)
            {
                mode = !mode;
            }
            if(mode)
            verlet.Update();

            verlet.GlobalRenderPoints();
            if (Main.LocalPlayer.controlUp && delays == 0)
            {
                /* int a = verlet.CreateVerletPoint(Main.MouseWorld);
                 int b = verlet.CreateVerletPoint(Main.MouseWorld + new Vector2(0,60));
                 int c = verlet.CreateVerletPoint(Main.MouseWorld + new Vector2(100, 100));
                 int d = verlet.CreateVerletPoint(Main.MouseWorld + new Vector2(60, 0));
                 int e = verlet.CreateVerletPoint(Main.MouseWorld + new Vector2(-100, -100), true);
                 int e1 = verlet.CreateVerletPoint(Main.MouseWorld + new Vector2(-100, -80));
                 int e2 = verlet.CreateVerletPoint(Main.MouseWorld + new Vector2(-100, -60));
                 int e3 = verlet.CreateVerletPoint(Main.MouseWorld + new Vector2(-100, -40));
                 int e4 = verlet.CreateVerletPoint(Main.MouseWorld + new Vector2(-100, -20));
                 verlet.BindPoints(a, b);
                 verlet.BindPoints(b, c);
                 verlet.BindPoints(c, d);
                 verlet.BindPoints(d, a);
                 verlet.BindPoints(a, c);
                 verlet.BindPoints(e, e1);
                 verlet.BindPoints(e1, e2);
                 verlet.BindPoints(e2,e3);
                 verlet.BindPoints(e3,e4);
                 verlet.BindPoints(e4, a);
                 verlet.BindPoints(e4, a);
                 int[] square = verlet.CreateVerletSquare(Main.MouseWorld, 100);
                verlet.BindPoints(c, square[0]);*/
                if (Verlet.points.Count == 0)
                {
                   // verlet.CreateVerletPoint(Main.MouseWorld);
                }
                else
                {
                  //  int a = verlet.CreateVerletPoint(Main.MouseWorld);
                   // verlet.BindPoints(a - 1, a);
                }
                verlet.CreateStickMan(Main.MouseWorld);
                delays = 20;
            }
            if (Main.LocalPlayer.controlUseItem && delays == 0)
            {
                if (Verlet.points.Count == 0)
                {
                    verlet.CreateVerletPoint(Main.MouseWorld,true);
                }
                else
                {
                    int a = verlet.CreateVerletPoint(Main.MouseWorld, true);
                    verlet.BindPoints(a - 1, a);
                }
                delays = 20;
            }
            if (Main.LocalPlayer.controlHook && delays == 0)
            {
                verlet.ClearPoints();
                delays = 20;
            }
        }
        public void UpdateIslands()
        {
            EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            for (int i = 0; i < modPlayer.SeaObject.Count; i++)
            {
                Color drawColour = Lighting.GetColor((int)(modPlayer.SeaObject[i].posToScreen.X / 16f), (int)(modPlayer.SeaObject[i].posToScreen.Y / 16f));
                Main.spriteBatch.Draw(modPlayer.SeaObject[i].texture, modPlayer.SeaObject[i].posToScreen.ForDraw(), drawColour);
            }
        }
        public void UpdateGame(GameTime gameTime)
        {
            lerps++;
            if(delays > 0)
            {
                delays--;
            }
            float lerpLol = Math.Abs((float)Math.Sin(lerps/50f));
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].type == ModContent.NPCType<OrbCollection>() && Main.npc[i].active)
                {
                    float Dist = Vector2.Distance(Main.npc[i].Center, Main.LocalPlayer.Center);
                    if (Dist < 1000)
                    {
                        Vector2 p1 = Main.npc[i].Center;
                        Vector2 p2 = Main.LocalPlayer.Center;
                        if (!Main.LocalPlayer.GetModPlayer<EEPlayer>().isPickingUp)
                        {
                            for (float j = 0; j < 1; j += 1 / Dist)
                            {
                                Vector2 Lerped = p1 + j * (p2 - p1);
                                Main.spriteBatch.Draw(Main.magicPixel, Lerped - Main.screenPosition, new Rectangle(0, 0, 1, 1), Color.AliceBlue * Math.Abs(lerpLol - j), 0f, new Rectangle(0, 0, 1, 1).Size(), 1f, SpriteEffects.None, 0f);
                            }
                            UIText("Pick Up?", Color.White * alphas, new Vector2(Main.screenWidth / 2, Main.screenHeight / 2 - 50), 1);
                        }
                        if(Dist < 100)
                        {
                            if(alphas < 1)
                                alphas += 0.01f;
                            if (Main.LocalPlayer.controlUp && delays == 0)
                            {
                                switch (Main.LocalPlayer.GetModPlayer<EEPlayer>().isPickingUp)
                                {
                                    case true:
                                        {
                                            Main.LocalPlayer.GetModPlayer<EEPlayer>().isPickingUp = false;
                                            break;
                                        }
                                    case false:
                                        {
                                            Main.npc[i].ai[1] = Main.myPlayer;
                                            Main.LocalPlayer.GetModPlayer<EEPlayer>().isPickingUp = true;
                                            break;
                                        }
                                }
                                delays = 120;
                            }
                        }
                        else
                        {
                            if (alphas > 0)
                                alphas -= 0.01f;
                        }
                       
                    }
                }
            }
            simpleGame = simpleGame ?? new IceHockey();
            simpleGame.Update(gameTime);
            for (int i = 0; i < Main.player.Length; i++)
            {
                if (Main.player[i].active && !Main.player[i].dead)
                {
                    Player player = Main.player[i];
                    if (ActivateGame.JustPressed)
                    {
                        simpleGame = new IceHockey();
                        simpleGame.StartGame(i);
                    }
                    if (player.controlHook)
                    {
                        simpleGame.EndGame();
                    }
                }
            }
            
        }
        public override void UpdateUI(GameTime gameTime)
        {

            lastGameTime = gameTime;
            if (EEInterface?.CurrentState != null)
            {
                EEInterface.Update(gameTime);
            }
            base.UpdateUI(gameTime);

            if (RuneActivator.JustPressed && delay == 0)
            {
                if (EEUIVisible)
                {
                    EEUIVisible = false;
                    if (Main.netMode != NetmodeID.Server && Filters.Scene["EEMod:Pause"].IsActive())
                    {
                        Filters.Scene.Deactivate("EEMod:Pause");
                    }
                }
                else
                {
                    EEUIVisible = true;
                    if (Main.netMode != NetmodeID.Server && !Filters.Scene["EEMod:Pause"].IsActive())
                    {
                        Filters.Scene.Activate("EEMod:Pause").GetShader().UseOpacity(pauseShaderTImer);
                    }
                }
                delay++;
            }
            if (EEUIVisible)
            {
                Filters.Scene["EEMod:Pause"].GetShader().UseOpacity(pauseShaderTImer);
                pauseShaderTImer += 50;
                if (pauseShaderTImer > 1000)
                {
                    pauseShaderTImer = 1000;
                }
            }
            else
            {
                pauseShaderTImer = 0;
            }
            if (delay > 0)
            {
                delay++;
                if (delay == 60)
                    delay = 0;
            }

            //_lastUpdateUiGameTime = gameTime;
			if (SpeedrunnTimer?.CurrentState != null)
			{
				RunUI.Update(gameTime);
			}
        }

        //internal void ShowMyUI()
        //{
        //    SpeedrunnTimer?.SetState(RunUI);
        //}

        //internal void HideMyUI()
        //{
        //    SpeedrunnTimer?.SetState(null);
        //}


        public override void Load()
        {
            instance = this;
            RuneActivator = RegisterHotKey("Rune UI", "Z");
            RuneSpecial = RegisterHotKey("Activate Runes", "V");
            ActivateGame = RegisterHotKey("Activate Games", "[");
            ActivateVerletEngine = RegisterHotKey("Activate VerletEngine", "N");
            AutoloadingManager.LoadManager(this);
            //IL.Terraria.IO.WorldFile.SaveWorldTiles += ILSaveWorldTiles;
            if (!Main.dedServ)
            {
                eeui = new EEUI();
                eeui.Activate();
                EEInterface = new UserInterface();
                Ref<Effect> screenRef3 = new Ref<Effect>(GetEffect("Effects/Ripple"));
                Ref<Effect> screenRef2 = new Ref<Effect>(GetEffect("Effects/SeaTrans"));
                Ref<Effect> screenRef = new Ref<Effect>(GetEffect("Effects/SunThroughWalls"));
                Filters.Scene["EEMod:Ripple"] = new Filter(new ScreenShaderData(screenRef3, "Ripple"), EffectPriority.High);
                Filters.Scene["EEMod:Ripple"].Load();
                Filters.Scene["EEMod:SeaTrans"] = new Filter(new ScreenShaderData(screenRef2, "SeaTrans"), EffectPriority.High);
                Filters.Scene["EEMod:SeaTrans"].Load();
                Filters.Scene["EEMod:SunThroughWalls"] = new Filter(new ScreenShaderData(screenRef, "SunThroughWalls"), EffectPriority.High);
                Filters.Scene["EEMod:SunThroughWalls"].Load();
                Filters.Scene["EEMod:SavingCutscene"] = new Filter(new SavingSkyData("FilterMiniTower").UseColor(0f, 0.20f, 1f).UseOpacity(0.3f), EffectPriority.High);
                SkyManager.Instance["EEMod:SavingCutscene"] = new SavingSky();
                NoiseSurfacing = GetEffect("Effects/NoiseSurfacing");
                /*
		  SpeedrunnTimer = new UserInterface();
		  //RunUI.Activate();
		  RunUI = new RunninUI();
		  SpeedrunnTimer.SetState(RunUI);
                */
            }
            LoadIL();
        }

        public static bool isSaving = false;
        public static int loadingChoose;
        public static int loadingChooseImage;
        public static bool loadingFlag = true;

        public static ModHotKey RuneActivator;
        public static ModHotKey RuneSpecial;
        public static ModHotKey ActivateGame;
        public static ModHotKey ActivateVerletEngine;
        internal bool EEUIVisible
        {
            get => EEInterface?.CurrentState != null;
            set => EEInterface?.SetState(value ? eeui : null);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                LegacyGameInterfaceLayer EEInterfaceLayer = new LegacyGameInterfaceLayer("EEMod: EEInterface",
                delegate
                {
                    if (lastGameTime != null)
                    {
                        if (EEInterface?.CurrentState != null)
                        {
                            EEInterface.Draw(Main.spriteBatch, lastGameTime);
                        }
                        UpdateGame(lastGameTime);
                       // UpdateVerlet();
                    }
                
                    return true;
                }, InterfaceScaleType.UI);
                layers.Insert(mouseTextIndex, EEInterfaceLayer);
            }

            if (Main.LocalPlayer.GetModPlayer<EEPlayer>().ridingZipline)
                DrawZipline();

            if (Main.worldName == KeyID.Sea)
            {
                for (int i = 0; i < layers.Count; i++)
                {
                    var layer = layers[i];
                    //Remove Resource bars
                    if (layer.Name.Contains("Vanilla: Resource Bars") || layer.Name.Contains("Vanilla: Info Accessories Bar"))
                    {
                        layers.RemoveAt(i);
                    }
                }
            }

            // EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            var textLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            var computerState = new LegacyGameInterfaceLayer("EE: UI",
            delegate
            {
                Ascension();
                if (Main.worldName == KeyID.Sea)
                {
                    UpdateIslands();
                    DrawSubText();
                    DrawShip();
                }
                if (Main.worldName == KeyID.Pyramids || Main.worldName == KeyID.Sea || Main.worldName == KeyID.CoralReefs)
                    DrawText();
                return true;
            },
            InterfaceScaleType.UI);
            layers.Insert(textLayer, computerState);
		    /*if (mouseTextIndex != -1)
		    {
		        layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
		        "SpeedrunTimer: SpeedrunnTimer",
		        delegate
		        {
		            if (_lastUpdateUiGameTime != null && SpeedrunnTimer?.CurrentState != null)
		            {
			            SpeedrunnTimer.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
		            }
		            return true;
		        },
		        InterfaceScaleType.UI));
		    }*/
        }
        public string text;
        public static int AscentionHandler;
        public static int startingTextHandler;
        public static bool isAscending;
        void UIText(string text, Color colour, Vector2 position, int style)
        {
            DynamicSpriteFont font = style == 0 ? Main.fontDeathText : Main.fontMouseText;
            Vector2 textSize = font.MeasureString(text);
            float textPositionLeft = position.X - textSize.X / 2;
            float textPositionRight = position.X + textSize.X / 2;
            Main.spriteBatch.DrawString(font, text, new Vector2(textPositionLeft, position.Y), colour, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
        }
        private void Ascension()
        {
            float seperation = 400;
            // EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            if (EEPlayer.startingText)
            {
                float alpha;
                startingTextHandler++;
                if (startingTextHandler % seperation <= (seperation / 2) && startingTextHandler > seperation)
                {
                    alpha = (float)Math.Sin(startingTextHandler % seperation / (seperation / 2) * Math.PI);
                }
                else
                {
                    alpha = 0;
                }
                Color color = Color.White;
                if (startingTextHandler < seperation * 2)
                {
                    text = "Im too weak";
                }
                else if (startingTextHandler < seperation * 3)
                {
                    text = "Haha Funny Sans Go Burr";
                }
                else if (startingTextHandler < seperation * 4)
                {
                    text = "Sans Slime was too much";
                }
                else
                {
                    text = "Go to the world and avenge me ples ok? Thx bye";
                }
                color *= alpha;
                Vector2 textSize = Main.fontDeathText.MeasureString(text);
                float textPositionLeft = Main.screenWidth / 2 - textSize.X / 2;
                //float textPositionRight = Main.screenWidth / 2 + textSize.X / 2;
                Main.spriteBatch.DrawString(Main.fontDeathText, text, new Vector2(textPositionLeft, Main.screenHeight / 2 - 300), color, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
            }
            // EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            if (isAscending)
            {
                float alpha;
                AscentionHandler++;
                if (AscentionHandler % seperation <= (seperation / 2) && AscentionHandler > seperation)
                {
                    alpha = (float)Math.Sin(AscentionHandler % seperation / (seperation / 2) * Math.PI);
                }
                else
                {
                    alpha = 0;
                }
                Color color = Color.Black;
                if (AscentionHandler < seperation * 2)
                {
                    text = "You have discovered your first rune";
                }
                else if (AscentionHandler < seperation * 3)
                {
                    text = "Be wary, many more remain";
                }
                else if (AscentionHandler < seperation * 4)
                {
                    text = "Once you collect them all, you will be able to make synergies"; //"Collect runes for synergies" This is technically wrong since you can only get synergies after collecting them all
                }
                else
                {
                    text = "Good luck.";
                }
                color *= alpha;
                Vector2 textSize = Main.fontDeathText.MeasureString(text);
                float textPositionLeft = Main.screenWidth / 2 - textSize.X / 2;
                //float textPositionRight = Main.screenWidth / 2 + textSize.X / 2;
                Main.spriteBatch.DrawString(Main.fontDeathText, text, new Vector2(textPositionLeft, Main.screenHeight / 2 - 300), color, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
            }
        }
        private void DrawText()
        {
            EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            float alpha = modPlayer.titleText;
            Color color = Color.White * alpha;
            if (Main.worldName == KeyID.Sea)
            {
                text = "The Ocean";
                color = new Color((1 - alpha), (1 - alpha), 1) * alpha;
            }
            if (Main.ActiveWorldFileData.Name == KeyID.Pyramids)
            {
                text = "The Pyramids";
                color = Color.Yellow * alpha;
            }
            if (Main.ActiveWorldFileData.Name == KeyID.CoralReefs)
            {
                text = "The Coral Reefs";
                color = Color.Blue * alpha;
            }
            if (Main.ActiveWorldFileData.Name == KeyID.VolcanoIsland)
            {
                text = "The Volcano";
                color = Color.OrangeRed * alpha;
            }
            if (Main.ActiveWorldFileData.Name == KeyID.VolcanoInside)
            {
                text = "The Volcano's Core";
                color = Color.Red * alpha;
            }
            if (Main.ActiveWorldFileData.Name == KeyID.Island)
            {
                text = "Tropical Island";
                color = Color.GreenYellow * alpha;
            }
            Texture2D Outline = TextureCache.Outline;
            Vector2 textSize = Main.fontDeathText.MeasureString(text);
            float textPositionLeft = Main.screenWidth / 2 - textSize.X / 2;
            float textPositionRight = Main.screenWidth / 2 + textSize.X / 2;

            Main.spriteBatch.DrawString(Main.fontDeathText, text, new Vector2(textPositionLeft, Main.screenHeight / 2 - 300), color, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(Outline, new Vector2(textPositionLeft - 25, Main.screenHeight / 2 - 270), new Rectangle(0, 0, Outline.Width, Outline.Height), Color.White * alpha, 0, new Rectangle(0, 0, Outline.Width, Outline.Height).Size() / 2, 1, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(Outline, new Vector2(textPositionRight + 25, Main.screenHeight / 2 - 270), new Rectangle(0, 0, Outline.Width, Outline.Height), Color.White * alpha, 0, new Rectangle(0, 0, Outline.Width, Outline.Height).Size() / 2, 1, SpriteEffects.FlipHorizontally, 0);
        }
        Texture2D texture;
        Rectangle frame;
        int frames;
        public static float ShipHelthMax = 7;
        public static float ShipHelth = 7;
        public Vector2 position;
        public Vector2 velocity;
        public static readonly Vector2 start = new Vector2(1700, 900);
        private void DrawSubText()
        {
            EEPlayer modPlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            float alpha = modPlayer.subTextAlpha;
            Color color = Color.White;
            if (Main.worldName == KeyID.Sea)
            {
                text = "Disembark?";
                color *= alpha;
            }
            if (text != null)
            {
                Vector2 textSize = Main.fontMouseText.MeasureString(text);
                float textPositionLeft = position.X - textSize.X / 2;
                Main.spriteBatch.DrawString(Main.fontMouseText, text, new Vector2(textPositionLeft, position.Y + 20), color, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
            }
        }
        float flash = 0;
        float markerPlacer = 0;
        public static bool IsPlayerLocalServerOwner(int whoAmI)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return Netplay.Connection.Socket.GetRemoteAddress().IsLocalHost();
            }

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                RemoteClient client = Netplay.Clients[i];
                if (client.State == 10 && i == whoAmI && client.Socket.GetRemoteAddress().IsLocalHost())
                {
                    return true;
                }
            }
            return false;
        }
        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            priority = MusicPriority.BossHigh;
            music = GetSoundSlot(SoundType.Music, "Sounds/Music/TransitionMusoc");
            return;
        }
        private int cannonDelay = 60;
        public Vector2 otherBoatPos;
        private void DrawShip()
        {
            markerPlacer++;
            Player player = Main.LocalPlayer;
            EEPlayer eePlayer = Main.LocalPlayer.GetModPlayer<EEPlayer>();
            if (!Main.gamePaused)
            {
                position += velocity;
                if (player.controlJump)
                {
                    velocity.Y -= 0.1f * eePlayer.boatSpeed;
                }
                if (player.controlDown)
                {
                    velocity.Y += 0.1f * eePlayer.boatSpeed;
                }
                if (player.controlRight)
                {
                    velocity.X += 0.1f * eePlayer.boatSpeed;
                }
                if (player.controlLeft)
                {
                    velocity.X -= 0.1f * eePlayer.boatSpeed;
                }
                if (player.controlUseItem && cannonDelay <= 0 && eePlayer.cannonballType != 0)
                {
                    switch (eePlayer.cannonballType)
                    {
                        case 1:
                            Projectile.NewProjectile(position + Main.screenPosition, -Vector2.Normalize((position + Main.screenPosition) - Main.MouseWorld) * 4, ModContent.ProjectileType<FriendlyCannonball>(), 0, 0);
                            break;
                        case 2:
                            Projectile.NewProjectile(position + Main.screenPosition, -Vector2.Normalize((position + Main.screenPosition) - Main.MouseWorld) * 4, ModContent.ProjectileType<FriendlyExplosiveCannonball>(), 0, 0);
                            break;
                        case 3:
                            Projectile.NewProjectile(position + Main.screenPosition, -Vector2.Normalize((position + Main.screenPosition) - Main.MouseWorld) * 4, ModContent.ProjectileType<FriendlyHallowedCannonball>(), 0, 0);
                            break;
                        case 4:
                            Projectile.NewProjectile(position + Main.screenPosition, -Vector2.Normalize((position + Main.screenPosition) - Main.MouseWorld) * 4, ModContent.ProjectileType<FriendlyChlorophyteCannonball>(), 0, 0);
                            break;
                        case 5:
                            Projectile.NewProjectile(position + Main.screenPosition, -Vector2.Normalize((position + Main.screenPosition) - Main.MouseWorld) * 4, ModContent.ProjectileType<FriendlyLuminiteCannonball>(), 0, 0);
                            break;
                    }
                    Main.PlaySound(SoundID.Item61);
                    cannonDelay = 60;
                }
                cannonDelay--;
            }
            velocity.X = Helpers.Clamp(velocity.X, -1 * eePlayer.boatSpeed, 1 * eePlayer.boatSpeed);
            velocity.Y = Helpers.Clamp(velocity.Y, -1 * eePlayer.boatSpeed, 1 * eePlayer.boatSpeed);
            texture = TextureCache.ShipMount;

            frames = 12;
            int frameNum = 0;
            if (Main.netMode == NetmodeID.SinglePlayer || ((Main.netMode == NetmodeID.MultiplayerClient || Main.netMode == NetmodeID.Server) && player.team == 0))
            {
                if (eePlayer.boatSpeed == 3)
                    frameNum = 1;
                if (eePlayer.boatSpeed == 1)
                    frameNum = 0;
            }
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                switch (player.team)
                {
                    case 1:
                        if (eePlayer.boatSpeed == 3)
                            frameNum = 3;
                        if (eePlayer.boatSpeed == 1)
                            frameNum = 2;
                        break;
                    case 2:
                        if (eePlayer.boatSpeed == 3)
                            frameNum = 9;
                        if (eePlayer.boatSpeed == 1)
                            frameNum = 8;
                        break;
                    case 3:
                        if (eePlayer.boatSpeed == 3)
                            frameNum = 5;
                        if (eePlayer.boatSpeed == 1)
                            frameNum = 4;
                        break;
                    case 4:
                        if (eePlayer.boatSpeed == 3)
                            frameNum = 7;
                        if (eePlayer.boatSpeed == 1)
                            frameNum = 6;
                        break;
                    case 5:
                        if (eePlayer.boatSpeed == 3)
                            frameNum = 11;
                        if (eePlayer.boatSpeed == 1)
                            frameNum = 10;
                        break;
                }
            }


            if (!Main.gamePaused)
            {
                velocity *= 0.98f;
            }
            for (int i = 0; i < eePlayer.objectPos.Count; i++)
            {
                if (i != 5 && i != 4 && i != 6 && i != 7 && i != 0 && i != 2 && i != 1 && i != 7 && i != 8)
                    Lighting.AddLight(eePlayer.objectPos[i], .4f, .4f, .4f);
                if (i == 1)
                    Lighting.AddLight(eePlayer.objectPos[i], .15f, .15f, .15f);
                if (i == 2)
                    Lighting.AddLight(eePlayer.objectPos[i], .4f, .4f, .4f);
                if (i == 4)
                    Lighting.AddLight(eePlayer.objectPos[i], .15f, .15f, .15f);
                if (i == 7)
                    Lighting.AddLight(eePlayer.objectPos[i], .4f, .4f, .4f);
                if (i == 0)
                    Lighting.AddLight(eePlayer.objectPos[i], .4f, .4f, .4f);
            }
            //Lighting.AddLight(eePlayer.objectPos[1], 0.9f, 0.9f, 0.9f);

            Texture2D texture3 = TextureCache.ShipHelth;
            Lighting.AddLight(Main.screenPosition + position, .1f, .1f, .1f);
            float quotient = ShipHelth / ShipHelthMax;
            Main.spriteBatch.Draw(texture3, new Vector2(Main.screenWidth - 175, 50), new Rectangle(0, (int)((texture3.Height / 8) * ShipHelth), texture3.Width, texture3.Height / 8), Color.White, 0, new Rectangle(0, (int)((texture3.Height / 8) * ShipHelth), texture3.Width, texture3.Height / 8).Size() / 2, 1, SpriteEffects.None, 0);
            for (int i = 0; i < Main.ActivePlayersCount; i++)
            {
                if (i == 0)
                {
                    Main.spriteBatch.Draw(texture, position, new Rectangle(0, frameNum * 52, texture.Width, texture.Height / frames), Color.White, velocity.X / 10, new Rectangle(0, frame.Y, texture.Width, texture.Height / frames).Size() / 2, 1, velocity.X < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                }
                else
                {
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        EEServerVariableCache.SyncBoatPos(position, velocity.X);
                    }
                    for (int j = 0; j < 255; j++)
                    {
                        if (Main.player[j].active && j != Main.myPlayer)
                        {
                            Main.spriteBatch.Draw(texture, EEServerVariableCache.OtherBoatPos[j], new Rectangle(0, frameNum * 52, texture.Width, texture.Height / frames), Color.White, EEServerVariableCache.OtherRot[j]/10f, new Rectangle(0, frame.Y, texture.Width, texture.Height / frames).Size() / 2, 1, EEServerVariableCache.OtherRot[j] < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                        }
                    }
                    
                }
            }
            flash += 0.01f;
            if (flash == 2)
            {
                flash = 10;
            }
        }


        public override void AddRecipeGroups()
        {
            RecipeGroup group0 = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " Gemstones", new int[]
            {
                ItemID.Amber,
                ItemID.Amethyst,
                ItemID.Diamond,
                ItemID.Emerald,
                ItemID.Ruby,
                ItemID.Sapphire,
                ItemID.Topaz
            });
            // Registers the new recipe group with the specified name
            RecipeGroup.RegisterGroup("EEMod:Gemstones", group0);
        }

        /*public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(this);
            recipe.AddIngredient(ModContent.ItemType<SaharaSceptoid>(), 1);
            recipe.AddIngredient(ItemID.CrystalShard, 8);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(ItemID.CrystalSerpent, 1);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ModContent.ItemType<QuartzicLifeFragment>(), 1);
            recipe.AddIngredient(ItemID.Gel, 25);
            recipe.AddIngredient(ItemID.Wood, 10);
            recipe.AddTile(TileID.Solidifier);
            recipe.SetResult(ItemID.SlimeStaff, 1);
            recipe.AddRecipe();
        }*/
        }
    }
