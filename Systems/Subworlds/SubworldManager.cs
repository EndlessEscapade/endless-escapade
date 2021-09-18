
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.Graphics.Capture;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Social;
using Terraria.Utilities;
using Terraria.WorldBuilding;
using Terraria.Audio;

namespace EEMod.Systems.Subworlds.EESubworlds
{
    public class SubworldManager
    {
        internal enum EEServerState : byte
        {
            None,
            SinglePlayer,
            MultiPlayer
        }

        private static string EEPath;
        private static WorldGenerator _generator;

        internal static EEServerState serverState = EEServerState.None;

        //public static Process LinuxModServer = new Process();

        public static int lastSeed;

        private static void AddGenerationPass(string name, WorldGenLegacyMethod method) => _generator.Append(new PassLegacy(name, method));

        public static void SettleLiquids()
        {
            /*AddGenerationPass("Settle Liquids", delegate (GenerationProgress progress)
            {
                int repeats = 0;

                progress.Message = "Settling Liquids";

                Liquid.QuickWater(3);
                WorldGen.WaterCheck();

                Liquid.quickSettle = true;

                while (repeats < 10)
                {
                    int liquidPlusBuffer = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
                    float i = 0f;

                    while (Liquid.numLiquid > 0)
                    {
                        float j = (liquidPlusBuffer - (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer)) / (float)liquidPlusBuffer;

                        if (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer > liquidPlusBuffer)
                        {
                            liquidPlusBuffer = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
                        }

                        if (j > i)
                        {
                            i = j;
                        }
                        else
                        {
                            j = i;
                        }

                        if (repeats == 1)
                        {
                            progress.Set(j / 3f + 0.33f);
                        }

                        int maxRepeats = 10;

                        if (repeats > maxRepeats)
                        {
                            maxRepeats = repeats;
                        }

                        Liquid.UpdateLiquid();
                    }

                    WorldGen.WaterCheck();
                    progress.Set(repeats * 0.1f / 3f + 0.66f);

                    repeats++;
                }

                // Liquid.quickSettle = false;
                Main.tileSolid[190] = true;
            });*/
        }

        public static void Reset(int seed)
        {
            Logging.Terraria.InfoFormat("Generating World: {0}", Main.ActiveWorldFileData.Name);

            lastSeed = seed;
            _generator = new WorldGenerator(seed);

            WorldGen.structures = new StructureMap();
            //MicroBiome.ResetAll();

            /*AddGenerationPass("Reset", delegate (GenerationProgress progress)
            {
                progress.Message = "";

                Liquid.ReInit();

                Main.cloudAlpha = 0f;
                Main.maxRaining = 0f;
                // Main.raining = false;

                WorldGen.RandomizeTreeStyle();
                WorldGen.RandomizeCaveBackgrounds();
                WorldGen.RandomizeBackgrounds();
                WorldGen.RandomizeMoonState();

                Main.worldID = WorldGen.genRand.Next(int.MaxValue);
            });*/
        }

        public static void PostReset(GenerationProgress customProgressObject = null)
        {
            _generator.GenerateWorld(customProgressObject);

            Main.WorldFileMetadata = FileMetadata.FromCurrentSettings(FileType.World);
        }

        internal static void PreSaveAndQuit()
        {
            Mod[] mods = ModLoader.Mods;

            for (int i = 0; i < mods.Length; i++)
            {
                //mods[i].PreSaveAndQuit();
            }
        }

        public static void EnterSubworld<T>() where T : Subworld, new()
        {
            SoundEngine.PlaySound(SoundID.MenuClose);
            PreSaveAndQuit();
            ThreadPool.QueueUserWorkItem(SaveAndQuitCallBack, new T());
        }


        public static void SaveAndQuitCallBack(object threadContext)
        {
            try
            {
                SoundEngine.PlaySound(SoundID.Waterfall, -1, -1, 0);
                SoundEngine.PlaySound(SoundID.Lavafall, -1, -1, 0);
            }
            catch
            {
            }

            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                WorldFile.CacheSaveTime();

                Main.menuMode = 10;
                EEMod.Subworlds.IsSaving = true;
            }
            else
            {
                Main.menuMode = 889;
            }

            Main.invasionProgress = 0;
            Main.invasionProgressDisplayLeft = 0;
            Main.invasionProgressAlpha = 0f;
            Main.gameMenu = true;
            //Main.StopTrackedSounds();
            CaptureInterface.ResetFocus();
            Main.ActivePlayerFileData.StopPlayTimer();
            Player.SavePlayer(Main.ActivePlayerFileData);

            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                WorldFile.SaveWorld();
                SoundEngine.PlaySound(SoundID.MenuOpen);

                // Main.fastForwardTime = false;

                //Main.UpdateSundial();

                Main.menuMode = 0;
                serverState = EEServerState.SinglePlayer;
            }
            else
            {
                Netplay.Disconnect = true;
                Main.netMode = NetmodeID.SinglePlayer;
                // Main.fastForwardTime = false;

                //Main.UpdateSundial();

                Main.menuMode = 889;
                serverState = EEServerState.MultiPlayer;
            }
            if (threadContext != null)
            {
                if (threadContext is Subworld sub)
                    EnterSub(sub);

                if (threadContext is string subN)
                    EnterSub(subN);
            }
        }

        public static void Do_worldGenCallBack(object threadContext)
        {
            SoundEngine.PlaySound(SoundID.MenuOpen);
            WorldGen.clearWorld();
            GenerateWorld((Subworld)threadContext, Main.ActiveWorldFileData.Seed, null);
            WorldFile.SaveWorld(Main.ActiveWorldFileData.IsCloudSave, resetTime: true);

            Main.ActiveWorldFileData = WorldFile.GetAllMetadata($@"{EEPath}\{(threadContext as Subworld).Name}.wld", false);

            WorldGen.playWorld();
        }

        public static void GenerateWorld(Subworld subworld, int seed, GenerationProgress customProgressObject = null)
        {
            subworld.Generate(seed, customProgressObject);
        }

        public static void WorldGenCallBack(object threadContext)
        {
            try
            {
                Do_worldGenCallBack(threadContext);
            }
            catch (Exception ex)
            {
                Logging.Terraria.Error(Language.GetTextValue("tModLoader.WorldGenError"), ex);
            }
        }

        public static void CreateNewWorld(Subworld subworld)
        {
            Main.rand = new UnifiedRandom(Main.ActiveWorldFileData.Seed);
            ThreadPool.QueueUserWorkItem(WorldGenCallBack, subworld);
        }

        public static string ConvertToSafeArgument(string arg) => Uri.EscapeDataString(arg);

        public static WorldFileData CreateSubworldMetaData(string name, bool cloudSave, string path)
        {
            WorldFileData existing = new WorldFileData(path, cloudSave)
            {
                Name = name,
                //existing.GameMode = GameMode;
                CreationTime = DateTime.Now,
                Metadata = FileMetadata.FromCurrentSettings(FileType.World)
            };
            existing.SetFavorite(favorite: false);
            existing.WorldGeneratorVersion = 987842478081uL;
            existing.UniqueId = Guid.NewGuid();

            if (Main.DefaultSeed == "")
            {
                existing.SetSeedToRandom();
            }
            else
            {
                existing.SetSeed(Main.DefaultSeed);
            }

            return existing;
        }
        public static string SubworldPath => Path.Combine(Main.SavePath,"Worlds","EESubworlds_Temp");
        private static void OnWorldNamed(object subworld)
        {
            string Name = "";

            if (subworld is Subworld sub) Name = sub.Name;
            if (subworld is string subN) Name = subN;

            SubworldPlayer subworldPlayer = Main.LocalPlayer.GetModPlayer<SubworldPlayer>();
            if (Name != subworldPlayer.PrimaryWorldName && subworld is Subworld Subworld)
            {
                subworldPlayer.InSubworld = true;
                subworldPlayer.CurrentSubworld = Subworld;

                EEPath = Path.Combine(SubworldPath, subworldPlayer.PrimaryWorldName + "Subworlds");

                if (!Directory.Exists(EEPath))
                {
                    Directory.CreateDirectory(EEPath);
                }

                string EESubworldPath = Path.Combine(EEPath, Name + ".wld");

                Main.ActiveWorldFileData = WorldFile.GetAllMetadata(EESubworldPath, false);
                Main.ActivePlayerFileData.SetAsActive();

                if (!File.Exists(EESubworldPath))
                {
                    CreateNewWorld(Subworld);

                    Main.ActiveWorldFileData = CreateSubworldMetaData(Name, SocialAPI.Cloud != null && SocialAPI.Cloud.EnabledByDefault, EESubworldPath);
                    Main.worldName = Name.Trim();

                    return;
                }
            }
            else
            {
                // subworldPlayer.InSubworld = false;
                Main.ActiveWorldFileData = WorldFile.GetAllMetadata(Path.Combine(EEPath, Name + ".wld"), false);
                Main.ActivePlayerFileData.SetAsActive();
            }

            if (serverState == EEServerState.SinglePlayer)
            {
                WorldGen.playWorld();
            }
            else
            {
                Main.clrInput();

                Netplay.ServerPassword = "";

                Main.GetInputText("");

                // Main.autoPass = false;
                Main.menuMode = 30;

                SoundEngine.PlaySound(SoundID.MenuOpen);
            }
        }

        public static void StartClientGameplay()
        {
            Main.menuMode = 10;
            Netplay.StartTcpClient();
        }

        private void ReturnOnName(string text)
        {
            Main.ActiveWorldFileData = WorldFile.GetAllMetadata(Path.Combine(Main.SavePath, "Worlds", text + ".wld"), false);
            WorldGen.playWorld();
        }

        public static void ReturnToBaseWorld()
        {

        }
        public static void EnterSub(string sub)
        {
            OnWorldNamed(sub);
        }
        public static void EnterSub(Subworld subworld)
        {
            OnWorldNamed(subworld);
        }


        public void Return(string baseWorldName) => ReturnOnName(baseWorldName);
    }
}