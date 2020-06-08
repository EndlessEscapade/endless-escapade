using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using System.Reflection;
using Microsoft.Xna.Framework;
using Terraria.Utilities;
using Terraria.World.Generation;
using Terraria.GameContent.Generation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using ReLogic.Graphics;
using Terraria.UI;
using Terraria.GameContent.UI.States;
using Microsoft.Xna.Framework.Graphics;
using EEMod.UI;
using MonoMod.Cil;
using Terraria.DataStructures;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using ReLogic.OS;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.UI;

namespace EEMod
{
    public static class Logging
    {
        public static readonly string LogDir = Path.Combine(Program.SavePath, "Logs");

        public static readonly string LogArchiveDir = Path.Combine(LogDir, "Old");

        internal const string side = "client";

        private static List<string> initWarnings = new List<string>();

        private static HashSet<string> pastExceptions = new HashSet<string>();

        private static HashSet<string> ignoreSources = new HashSet<string>
    {
        "MP3Sharp"
    };

        private static List<string> ignoreContents = new List<string>
    {
        "System.Console.set_OutputEncoding",
        "Terraria.ModLoader.Core.ModCompile",
        "Delegate.CreateDelegateNoSecurityCheck",
        "MethodBase.GetMethodBody",
        "Terraria.Net.Sockets.TcpSocket.Terraria.Net.Sockets.ISocket.AsyncSend",
        "System.Diagnostics.Process.Kill",
        "Terraria.ModLoader.Core.AssemblyManager.CecilAssemblyResolver.Resolve",
        "Terraria.ModLoader.Engine.TMLContentManager.OpenStream"
    };

        private static List<string> ignoreMessages = new List<string>
    {
        "A blocking operation was interrupted by a call to WSACancelBlockingCall",
        "The request was aborted: The request was canceled.",
        "Object name: 'System.Net.Sockets.Socket'.",
        "Object name: 'System.Net.Sockets.NetworkStream'",
        "This operation cannot be performed on a completed asynchronous result object.",
        "Object name: 'SslStream'.",
        "Unable to load DLL 'Microsoft.DiaSymReader.Native.x86.dll'"
    };

        private static List<string> ignoreThrowingMethods = new List<string>
    {
        "at Terraria.Lighting.doColors_Mode",
        "System.Threading.CancellationToken.Throw"
    };

        private static ThreadLocal<bool> handlerActive = new ThreadLocal<bool>(() => false);

        private static Exception previousException;

        private static Regex statusRegex = new Regex("(.+?)[: \\d]*%$");

        internal static readonly FieldInfo f_fileName = typeof(StackFrame).GetField("strFileName", BindingFlags.Instance | BindingFlags.NonPublic) ?? typeof(StackFrame).GetField("fileName", BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly Assembly TerrariaAssembly = Assembly.GetExecutingAssembly();

        public static string LogPath
        {
            get;
            private set;
        }

        internal static ILog Terraria
        {
            get;
        } = LogManager.GetLogger("Terraria");


        internal static ILog tML
        {
            get;
        } = LogManager.GetLogger("tML");






        private static string GetNewLogFile(string baseName)
        {
            Regex pattern = new Regex(baseName + "(\\d*)\\.log$");
            List<string> source = (from s in Directory.GetFiles(LogDir)
                                   where pattern.IsMatch(Path.GetFileName(s))
                                   select s).ToList();
            if (!source.All(CanOpen))
            {
                int num = source.Select(delegate (string s)
                {
                    string value = pattern.Match(Path.GetFileName(s)).Groups[1].Value;
                    return (value.Length == 0) ? 1 : int.Parse(value);
                }).Max();
                return $"{baseName}{num + 1}.log";
            }
            foreach (string item in source.OrderBy(File.GetCreationTime))
            {
                string text = ".old";
                int num2 = 0;
                while (File.Exists(item + text))
                {
                    text = $".old{++num2}";
                }
                try
                {
                    File.Move(item, item + text);
                }
                catch (IOException ex)
                {
                    initWarnings.Add($"Move failed during log initialization: {item} -> {Path.GetFileName(item)}{text}\n{ex}");
                }
            }
            return baseName + ".log";
        }

        private static bool CanOpen(string fileName)
        {
            try
            {
                using (new FileStream(fileName, FileMode.Append))
                {
                }
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }



        internal static void ResetPastExceptions()
        {
            pastExceptions.Clear();
        }

        public static void IgnoreExceptionSource(string source)
        {
            ignoreSources.Add(source);
        }

        public static void IgnoreExceptionContents(string source)
        {
            if (!ignoreContents.Contains(source))
            {
                ignoreContents.Add(source);
            }
        }



        private static void AddChatMessage(string msg, Color color)
        {
            if (!Main.gameMenu)
            {
                float soundVolume = Main.soundVolume;
                Main.soundVolume = 0f;
                Main.NewText(msg, color);
                Main.soundVolume = soundVolume;
            }
        }

        internal static void LogStatusChange(string oldStatusText, string newStatusText)
        {
            string value = statusRegex.Match(oldStatusText).Groups[1].Value;
            string value2 = statusRegex.Match(newStatusText).Groups[1].Value;
            if (value2 != value && value2.Length > 0)
            {
                LogManager.GetLogger("StatusText").Info((object)value2);
            }
        }






        private static void EnablePortablePDBTraces()
        {
            if (FrameworkVersion.Framework == Framework.NetFramework && FrameworkVersion.Version >= new Version(4, 7, 2))
            {
                Type.GetType("System.AppContextSwitches").GetField("_ignorePortablePDBsInStackTraces", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, -1);
            }
        }
    }

    public class WorldGenerator
    {
        internal List<GenPass> _passes = new List<GenPass>();

        internal float _totalLoadWeight;

        internal int _seed;

        public WorldGenerator(int seed)
        {
            _seed = seed;
        }

        public void Append(GenPass pass)
        {
            _passes.Add(pass);
            _totalLoadWeight += pass.Weight;
        }

        public void GenerateWorld(GenerationProgress progress = null)
        {
            Stopwatch stopwatch = new Stopwatch();
            float num = 0f;
            foreach (GenPass pass in _passes)
            {
                num += pass.Weight;
            }
            if (progress == null)
            {
                progress = new GenerationProgress();
            }
            progress.TotalWeight = num;
            Main.menuMode = 888;
            Main.MenuUI.SetState(new UIWorldLoad(progress));
            foreach (GenPass pass2 in _passes)
            {
                WorldGen._genRand = new UnifiedRandom(_seed);
                Main.rand = new UnifiedRandom(_seed);
                stopwatch.Start();
                progress.Start(pass2.Weight);
                try
                {
                    pass2.Apply(progress);
                }
                catch (Exception ex)
                {
                    string text = string.Join("\n", Language.GetTextValue("tModLoader.WorldGenError"), pass2.Name, ex);
                    throw ex;
                }
                progress.End();
                stopwatch.Reset();
            }
        }
    }

    public partial class EEMod : Mod
    {
        public static EEMod instance;

        public override void PostSetupContent()
        {

        }
        public static double worldSurface;

        public static double worldSurfaceLow;

        public static double worldSurfaceHigh;

        public static double rockLayer;

        public static double rockLayerLow;

        public static double rockLayerHigh;

        public static int _lastSeed;

        private static WorldGenerator _generator;
        public UserInterface customResources;

        private static void AddGenerationPass(string name, WorldGenLegacyMethod method)
        {
            _generator.Append(new PassLegacy(name, method));
        }

        public static void GenerateWorld(int seed, GenerationProgress customProgressObject = null)
        {
            Main.maxTilesX = 400;
            Main.maxTilesY = 400;
            Main.spawnTileX = 234;
            Main.spawnTileY = 92;
            Logging.Terraria.InfoFormat("Generating World: {0}", (object)Main.ActiveWorldFileData.Name);
            _lastSeed = seed;
            _generator = new WorldGenerator(seed);
            MicroBiome.ResetAll();

            //WorldHooks.PreWorldGen();
            AddGenerationPass("Reset", delegate (GenerationProgress progress)
            {
                Liquid.ReInit();
                progress.Message = "";
                Main.cloudAlpha = 0f;
                Main.maxRaining = 0f;
                Main.raining = false;
                WorldGen.RandomizeTreeStyle();
                WorldGen.RandomizeCaveBackgrounds();
                WorldGen.RandomizeBackgrounds();
                WorldGen.RandomizeMoonState();

            });
            Main.worldID = WorldGen.genRand.Next(int.MaxValue);
            //WorldHooks.ModifyWorldGenTasks(_generator._passes, ref _generator._totalLoadWeight);
            _generator.GenerateWorld(customProgressObject);
            EEWorld pW = new EEWorld();
            pW.FillRegion(400, 400, new Vector2(0, 0), TileID.SandstoneBrick);
            pW.Pyramid(63, 42);
            //Main.WorldFileMetadata = FileMetadata.FromCurrentSettings(FileType.World);
        }
        public static void GenerateWorld2(int seed, GenerationProgress customProgressObject = null)
        {
            Main.maxTilesX = 400;
            Main.maxTilesY = 400;
            Main.spawnTileX = 234;
            Main.spawnTileY = 92;
            Logging.Terraria.InfoFormat("Generating World: {0}", (object)Main.ActiveWorldFileData.Name);
            _lastSeed = seed;
            _generator = new WorldGenerator(seed);
            MicroBiome.ResetAll();

            //WorldHooks.PreWorldGen();
            AddGenerationPass("Reset", delegate (GenerationProgress progress)
            {
                Liquid.ReInit();
                progress.Message = "";
                Main.cloudAlpha = 0f;
                Main.maxRaining = 0f;
                Main.raining = false;
                WorldGen.RandomizeTreeStyle();
                WorldGen.RandomizeCaveBackgrounds();
                WorldGen.RandomizeBackgrounds();
                WorldGen.RandomizeMoonState();

            });
            Main.worldID = WorldGen.genRand.Next(int.MaxValue);
            //WorldHooks.ModifyWorldGenTasks(_generator._passes, ref _generator._totalLoadWeight);
            _generator.GenerateWorld(customProgressObject);
            EEWorld pW = new EEWorld();
            pW.FillWall(400, 400, new Vector2(0, 0), WallID.Waterfall);
            //Main.WorldFileMetadata = FileMetadata.FromCurrentSettings(FileType.World);
        }



        public override void Unload()
        {
            instance = null;
        }
        internal EEUI eeui;
        public UserInterface EEInterface;

        public override void Load()
        {
            instance = this;
            IL.Terraria.IO.WorldFile.SaveWorldTiles += ILSaveWorldTiles;
            if (!Main.dedServ)
            {

                eeui = new EEUI();
                eeui.Initialize();
                EEInterface = new UserInterface();
                EEInterface.SetState(eeui);
                Ref<Effect> screenRef3 = new Ref<Effect>(GetEffect("Effects/Ripple"));
                Ref<Effect> screenRef2 = new Ref<Effect>(GetEffect("Effects/SeaTrans"));
                Ref<Effect> screenRef = new Ref<Effect>(GetEffect("Effects/SunThroughWalls"));
                Filters.Scene["Prophecy:Ripple"] = new Filter(new ScreenShaderData(screenRef3, "Ripple"), EffectPriority.High);
                Filters.Scene["Prophecy:Ripple"].Load();
                Filters.Scene["Prophecy:SeaTrans"] = new Filter(new ScreenShaderData(screenRef2, "SeaTrans"), EffectPriority.High);
                Filters.Scene["Prophecy:SeaTrans"].Load();
                Filters.Scene["Prophecy:SunThroughWalls"] = new Filter(new ScreenShaderData(screenRef, "SunThroughWalls"), EffectPriority.High);
                Filters.Scene["Prophecy:SunThroughWalls"].Load();
                Filters.Scene["Prophecy:SavingCutscene"] = new Filter(new SavingSkyData("FilterMiniTower").UseColor(0f, 0.20f, 1f).UseOpacity(0.3f), EffectPriority.High);
                SkyManager.Instance["Prophecy:SavingCutscene"] = new SavingSky();
            }
            if (!Main.gameMenu)
                On.Terraria.Main.DoUpdate += OnUpdate;
            On.Terraria.Main.DrawMenu += OnDrawMenu;
            On.Terraria.WorldGen.SaveAndQuitCallBack += OnSave;
        }

        public static bool isSaving = false;
        public static int loadingChoose;
        public static int loadingChooseImage;
        public static bool loadingFlag = true;
        public void OnSave(On.Terraria.WorldGen.orig_SaveAndQuitCallBack orig, object threadcontext)
        {
            isSaving = true;
            orig(threadcontext);
            isSaving = false;

            //saveInterface?.SetState(null);

        }
        public void OnUpdate(On.Terraria.Main.orig_DoUpdate orig, Main self, GameTime gameTime)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (!isSaving)
                {
                    loadingChoose = Main.rand.Next(61);
                    loadingChooseImage = Main.rand.Next(5);
                    Main.numClouds = 10;
                    if (SkyManager.Instance["Prophecy:SavingCutscene"].IsActive()) SkyManager.Instance.Deactivate("Prophecy:SavingCutscene", new object[0]);
                    Main.logo2Texture = ModContent.GetTexture("Terraria/Logo2");
                    Main.logoTexture = ModContent.GetTexture("Terraria/Logo");
                    Main.sun2Texture = ModContent.GetTexture("Terraria/Sun2");
                    Main.sun3Texture = ModContent.GetTexture("Terraria/Sun3");
                    Main.sunTexture = ModContent.GetTexture("Terraria/Sun");
                    for (int i = 0; i < Main.backgroundTexture.Length - 1; i++)
                        Main.backgroundTexture[i] = ModContent.GetTexture("Terraria/Background_" + i);
                }
                else
                {

                }
            }
            orig(self, gameTime);
        }
        private void OnDrawMenu(On.Terraria.Main.orig_DrawMenu orig, Main self, GameTime gameTime)
        {
            position = start;
            velocity = Vector2.Zero;
            if (isSaving)
            {
                Mod mod = ModLoader.GetMod("EEMod");
                //Main.spriteBatch.Draw(texture, new Vector2(Main.screenWidth / 2, 100), new Rectangle(0, 0, 100, 100), Color.Black, 0, origin, 5, SpriteEffects.None, 0);
                for (int i = 0; i < Main.backgroundTexture.Length; i++)
                    Main.backgroundTexture[i] = Main.magicPixel;
                Main.numClouds = 0;
                Main.logo2Texture = mod.GetTexture("Empty");
                Main.logoTexture = mod.GetTexture("Empty");
                Main.sun2Texture = mod.GetTexture("Empty");
                Main.sun3Texture = mod.GetTexture("Empty");
                Main.sunTexture = mod.GetTexture("Empty");

                if (SkyManager.Instance["Prophecy:SavingCutscene"] != null) SkyManager.Instance.Activate("Prophecy:SavingCutscene", default, new object[0]);
                if (loadingFlag)
                {
                    loadingChoose = Main.rand.Next(61);
                    loadingChooseImage = Main.rand.Next(5);
                    loadingFlag = false;
                }
                switch (loadingChoose)
                {
                    case 0:
                        Main.statusText = "Watch Out! Dune Shamblers Pop from the ground from time to time!";
                        break;
                    case 1:
                        Main.statusText = "Gallagar is Satan!";
                        break;
                    case 2:
                        Main.statusText = "Tip of the Day! Loading screens are useless";
                        break;
                    case 3:
                        Main.statusText = "OH YES LOAD ME HARDER";
                        break;
                    case 4:
                        Main.statusText = "Terraria sprites need outlines... except when I make them";
                        break;
                    case 5:
                        Main.statusText = "Remove the Banding";
                        break;
                    case 6:
                        Main.statusText = Main.player[Main.myPlayer].name + " ....huh...what a shitty name";
                        break;
                    case 7:
                        Main.statusText = "Dont ping everyone you big dumb stupid";
                        break;
                    case 8:
                        Main.statusText = "Im nothing without attention";
                        break;
                    case 9:
                        Main.statusText = "Why are you even reading this?";
                        break;
                    case 10:
                        Main.statusText = "We actually think we are funny";
                        break;
                    case 11:
                        Main.statusText = "Interitos...whats that?";
                        break;
                    case 12:
                        Main.statusText = "its my style";
                        break;
                    case 13:
                        Main.statusText = "Now featuring 50% more monkey per chimp!";
                        break;
                    case 14:
                        Main.statusText = "im angy";
                        break;
                    case 15:
                        Main.statusText = "Send help, please this is not a joke im actually being held hostage";
                        break;
                    case 16:
                        Main.statusText = "Mod is not edgy I swear!";
                        break;
                    case 17:
                        Main.statusText = "Help help help please god jesus fucking chirst help me it hurts so fucking bad please help me please god why";
                        break;
                    case 18:
                        Main.statusText = "Im gonna have to mute you for that";
                        break;
                    case 19:
                        Main.statusText = "Gamers rise up!";
                        break;
                    case 20:
                        Main.statusText = "THATS NOT THE CONCEPT";
                        break;
                    case 21:
                        Main.statusText = "Help help help please god jesus fucking chirst help me it hurts so fucking bad please help me please god why";
                        break;
                    case 22:
                        Main.statusText = "D D D A G# G F D F G";
                        break;
                    case 23:
                        Main.statusText = "We live in a society";
                        break;
                    case 24:
                        Main.statusText = "Dont mine at night!";
                        break;
                    case 25:
                        Main.statusText = "deleting system32...";
                        break;
                    case 26:
                        Main.statusText = "Sans in real!";
                        break;
                    case 27:
                        Main.statusText = "daimgamer craves penis";
                        break;
                    case 28:
                        Main.statusText = "If you liked endless escapade you will love endless escapade premium!";
                        break;
                    case 29:
                        Main.statusText = "daddy oh yes";
                        break;
                    case 30:
                        Main.statusText = "mario in real life";
                        break;
                    case 31:
                        Main.statusText = "I love it when you load me like that";
                        break;
                    case 32:
                        Main.statusText = "Prophecy Foretold? More like doesn't exist";
                        break;
                    case 33:
                        Main.statusText = "Endless Escapade is the new PornHub";
                        break;
                    case 34:
                        Main.statusText = "Respect the drip Karen";
                        break;
                    case 35:
                        Main.statusText = "Its not Animal abuse he is just a chonker!";
                        break;
                    case 36:
                        Main.statusText = "All good sprites made by daimgamer!";
                        break;
                    case 37:
                        Main.statusText = "All (good) music made by Universe";
                        break;
                    case 38:
                        Main.statusText = "All good sprites made by vadim";
                        break;
                    case 39:
                        Main.statusText = "All good sprites made by zarn";
                        break;
                    case 40:
                        Main.statusText = "ni-𝕌ℕ𝕀𝕍𝔼ℝ𝕊𝔼";
                        break;
                    case 41:
                        Main.statusText = "Totally not copying Starlight River";
                        break;
                    case 42:
                        Main.statusText = "Do a Barrel Roll";
                        break;
                    case 43:
                        Main.statusText = "The man behind the laughter";
                        break;
                    case 44:
                        Main.statusText = "We all eventually die!";
                        break;
                    case 45:
                        Main.statusText = "twinkle twinkle little dick";
                        break;
                    case 46:
                        Main.statusText = "Poggers? Poggers.";
                        break;
                    case 47:
                        Main.statusText = "Totally not sentient AI";
                        break;
                    case 48:
                        Main.statusText = "Ḯ̴͝t̶͐̈́ ̶̄͆ȁ̷͠l̶̄̑l̵̇͝ ̴̀̎e̶͌̌n̶̍̓d̵͋̂s̶̑̃ ̵͊̉ẻ̶̛v̶̍̍ë̴́́n̶͗͠t̷́͘u̵͒̆à̶̎l̷̊͗l̶͊̍y̴̌̋!̴͂̑";
                        break;
                    case 49:
                        Main.statusText = "Illegal in 5 countries!";
                        break;
                    case 50:
                        Main.statusText = "Inside jokes you wont understand!";
                        break;
                    case 51:
                        Main.statusText = "Big content mod bad!";
                        break;
                    case 52:
                        Main.statusText = "Loading the random chimp event...";
                        break;
                    case 53:
                        Main.statusText = "Sending you to the Aether...";
                        break;
                    case 54:
                        Main.statusText = "When";
                        break;
                    case 55:
                        Main.statusText = "Yoda cbt hard style remix 2019";
                        break;
                    case 56:
                        Main.statusText = "Enabling large chode mode";
                        break;
                    case 57:
                        Main.statusText = "owo";
                        break;
                    case 58:
                        Main.statusText = "*nuzzles you";
                        break;
                    case 59:
                        Main.statusText = "Hacking the mainframe...";
                        break;
                    case 60:
                        Main.statusText = "Not Proud";
                        break;
                }
            }
            else
            {
                loadingChoose = Main.rand.Next(61);
                loadingChooseImage = Main.rand.Next(5);
                Main.numClouds = 10;
                if (SkyManager.Instance["Prophecy:SavingCutscene"].IsActive()) SkyManager.Instance.Deactivate("Prophecy:SavingCutscene", new object[0]);
                Main.logo2Texture = ModContent.GetTexture("Terraria/Logo2");
                Main.logoTexture = ModContent.GetTexture("Terraria/Logo");
                Main.sun2Texture = ModContent.GetTexture("Terraria/Sun2");
                Main.sun3Texture = ModContent.GetTexture("Terraria/Sun3");
                Main.sunTexture = ModContent.GetTexture("Terraria/Sun");
                for (int i = 0; i < Main.backgroundTexture.Length; i++)
                    Main.backgroundTexture[i] = ModContent.GetTexture("Terraria/Background_" + i);
            }
            orig(self, gameTime);

        }

        public override void UpdateUI(GameTime gameTime)
        {
            // it will only draw if the player is not on the main menu
            /*if (!Main.gameMenu
				&& EEUI.visible)
			{
				EEInterface?.Update(gameTime);
			}
			if(Main.gameMenu)
			{
				EEInterface?.Update(gameTime);
			}*/
        }

        // Load

        // Load

        private static void ILSaveWorldTiles(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            PropertyInfo statusText = typeof(Main).GetProperty(nameof(Main.statusText));
            MethodInfo set = statusText.GetSetMethod();

            if (!c.TryGotoNext(i => i.MatchCall(set)))
                throw new Exception();

            c.EmitDelegate<Func<string, string>>((originalText) =>
            {
                return originalText;
            });
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            if (Main.ActiveWorldFileData.Name == EEPlayer.key2)
            {
                for (int i = 0; i < layers.Count; i++)
                {
                    //Remove Resource bars
                    if (layers[i].Name.Contains("Vanilla: Resource Bars"))
                    {
                        layers.RemoveAt(i);
                    }
                }
                for (int i = 0; i < layers.Count; i++)
                {
                    //Remove Resource bars
                    if (layers[i].Name.Contains("Vanilla: Info Accessories Bar"))
                    {
                        layers.RemoveAt(i);
                    }
                }
            }
            EEPlayer modPlayer = Main.player[Main.myPlayer].GetModPlayer<EEPlayer>();
            var textLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            var computerState = new LegacyGameInterfaceLayer("EE: UI",
                delegate
                {
                    if (Main.ActiveWorldFileData.Name == EEPlayer.key2)
                    {
                        DrawShip();
                        DrawSubText();
                    }
                    if (Main.ActiveWorldFileData.Name == EEPlayer.key1 || Main.ActiveWorldFileData.Name == EEPlayer.key2)
                        DrawText();
                    return true;
                },
                InterfaceScaleType.UI);
            layers.Insert(textLayer, computerState);
        }
        public string text;
        private void DrawText()
        {
            Mod mod = ModLoader.GetMod("EEMod");
            EEPlayer modPlayer = Main.player[Main.myPlayer].GetModPlayer<EEPlayer>();
            float alpha = modPlayer.titleText;
            Color color = Color.White * alpha;
            if (Main.ActiveWorldFileData.Name == EEPlayer.key2)
            {
                text = "The Ocean";
                color = new Color((1 - alpha), (1 - alpha), 1) * alpha;
            }
            if (Main.ActiveWorldFileData.Name == EEPlayer.key1)
                text = "The Pyramids";
            Texture2D Outline = mod.GetTexture("Outline");
            Vector2 textSize = Main.fontDeathText.MeasureString(text);
            float textPositionLeft = Main.screenWidth / 2 - textSize.X / 2;
            float textPositionRight = Main.screenWidth / 2 + textSize.X / 2;
            Main.spriteBatch.DrawString(Main.fontDeathText, text, new Vector2(textPositionLeft, Main.screenHeight / 2 - 300), color, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(Outline, new Vector2(textPositionLeft - 25, Main.screenHeight / 2 - 270), new Rectangle(0, 0, Outline.Width, Outline.Height), Color.White * alpha, 0, new Rectangle(0, 0, Outline.Width, Outline.Height).Size() / 2, 1, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(Outline, new Vector2(textPositionRight + 25, Main.screenHeight / 2 - 270), new Rectangle(0, 0, Outline.Width, Outline.Height), Color.White * alpha, 0, new Rectangle(0, 0, Outline.Width, Outline.Height).Size() / 2, 1, SpriteEffects.FlipHorizontally, 0);
        }
        Texture2D texture;
        Rectangle frame;
        int Countur;
        int frames;
        int frameSpeed;
        public static float ShipHelthMax = 100;
        public static float ShipHelth = 100;
        public static Vector2 position;
        public static Vector2 velocity;
        public static readonly Vector2 start = new Vector2(1700, 900);
        private void DrawSubText()
        {
            EEPlayer modPlayer = Main.player[Main.myPlayer].GetModPlayer<EEPlayer>();
            float alpha = modPlayer.subText;
            Color color = Color.White;
            if (Main.ActiveWorldFileData.Name == EEPlayer.key2)
            {
                text = "Disembark?";
                color *= alpha;
            }
            Vector2 textSize = Main.fontMouseText.MeasureString(text);
            float textPositionLeft = position.X - textSize.X / 2;
            float textPositionRight = position.X + textSize.X / 2;
            Main.spriteBatch.DrawString(Main.fontMouseText, text, new Vector2(textPositionLeft, position.Y + 20), color, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
        }
        private void DrawShip()
        {
            Player modPlayer = Main.player[Main.myPlayer];
            if (!Main.gamePaused)
            {
                position += velocity;
                if (modPlayer.controlJump)
                {
                    velocity.Y -= 0.1f;
                }
                if (modPlayer.controlDown)
                {
                    velocity.Y += 0.1f;
                }
                if (modPlayer.controlRight)
                {
                    velocity.X += 0.1f;
                }
                if (modPlayer.controlLeft)
                {
                    velocity.X -= 0.1f;
                }
            }
            if (velocity.X > 1)
                velocity.X = 1;
            if (velocity.X < -1)
                velocity.X = -1;
            if (velocity.Y > 1)
                velocity.Y = 1;
            if (velocity.Y < -1)
                velocity.Y = -1;
            Mod mod = ModLoader.GetMod("EEMod");
            texture = mod.GetTexture("ShipMount");
            frames = 1;
            frameSpeed = 15;
            if (Countur++ > frameSpeed)
            {
                Countur = 0;
                frame.Y += texture.Height / frames;
            }
            if (frame.Y >= (texture.Height / frames) * (frames - 1))
            {
                frame.Y = 0;
            }
            if (!Main.gamePaused)
            {
                velocity *= 0.98f;
            }
            //Dust.NewDust(Main.screenPosition + position, 1, 1, DustID.BlueCrystalShard);
            Lighting.AddLight(new Vector2(Main.screenPosition.X + Main.screenWidth, Main.screenPosition.Y + Main.screenHeight), 1f, 1f, 1f);
            for (int i = 0; i < 200; i++)
            {
                for (int j = 0; j < 100; j++)
                {

                }
            }
            for (int i = 0; i < EEPlayer.objectPos.Count; i++)
            {
                if (i != 5 && i != 4 && i != 6 && i != 7 && i != 0 && i != 2 && i != 1)
                    Lighting.AddLight(EEPlayer.objectPos[i], .4f, .4f, .4f);
                if (i == 1)
                    Lighting.AddLight(EEPlayer.objectPos[i], .15f, .15f, .15f);
            }

            Texture2D texture3 = mod.GetTexture("ShipHelth");
            Lighting.AddLight(Main.screenPosition + position, .1f, .1f, .1f);
            float quotient = ShipHelth / ShipHelthMax;
            Main.spriteBatch.Draw(texture3, new Vector2(Main.screenWidth - 100, 100), new Rectangle(0, 0, (int)(texture3.Width * quotient), texture3.Height), Color.White, 0, new Rectangle(0, 0, texture3.Width, texture3.Height).Size() / 2, 1, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height / frames), Color.White, velocity.X / 10, new Rectangle(0, frame.Y, texture.Width, texture.Height / frames).Size() / 2, 1, velocity.X < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
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

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(this);
            recipe.AddIngredient(null, "SaharaSceptoid", 1);
            recipe.AddIngredient(ItemID.CrystalShard, 8);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(ItemID.CrystalSerpent, 1);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(null, "QuartzicLifeFragment", 1);
            recipe.AddIngredient(ItemID.Gel, 25);
            recipe.AddIngredient(ItemID.Wood, 10);
            recipe.AddTile(TileID.Solidifier);
            recipe.SetResult(ItemID.SlimeStaff, 1);
            recipe.AddRecipe();

        }
    }
}
