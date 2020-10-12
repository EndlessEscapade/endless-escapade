using EEMod.Tiles;
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
using Terraria.World.Generation;

namespace EEMod
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

        public static Process EEServer = new Process();
        public static int lastSeed;

        private static void AddGenerationPass(string name, WorldGenLegacyMethod method) => _generator.Append(new PassLegacy(name, method));

        public static void SettleLiquids()
        {
            AddGenerationPass("Settle Liquids", delegate (GenerationProgress progress)
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

                Liquid.quickSettle = false;
                Main.tileSolid[190] = true;
            });
        }

        public static void Reset(int seed)
        {
            EEMod.progressMessage = "Resetting";
            Logging.Terraria.InfoFormat("Generating World: {0}", Main.ActiveWorldFileData.Name);

            lastSeed = seed;
            _generator = new WorldGenerator(seed);

            WorldGen.structures = new StructureMap();
            MicroBiome.ResetAll();

            AddGenerationPass("Reset", delegate (GenerationProgress progress)
            {
                progress.Message = "Resetting";

                Liquid.ReInit();

                Main.cloudAlpha = 0f;
                Main.maxRaining = 0f;
                Main.raining = false;

                WorldGen.RandomizeTreeStyle();
                WorldGen.RandomizeCaveBackgrounds();
                WorldGen.RandomizeBackgrounds();
                WorldGen.RandomizeMoonState();

                Main.worldID = WorldGen.genRand.Next(int.MaxValue);
            });
        }

        public static void PostReset(GenerationProgress customProgressObject = null)
        {
            EEMod.progressMessage = "Post Resetting";

            _generator.GenerateWorld(customProgressObject);

            Main.WorldFileMetadata = FileMetadata.FromCurrentSettings(FileType.World);

            EEWorld.EEWorld.FillRegion(Main.maxTilesX, Main.maxTilesY, Vector2.Zero, ModContent.TileType<GemsandTile>());
            EEWorld.EEWorld.ClearRegion(Main.maxTilesX, Main.maxTilesY, Vector2.Zero);
        }

        internal static void PreSaveAndQuit()
        {
            Mod[] mods = ModLoader.Mods;

            for (int i = 0; i < mods.Length; i++)
            {
                mods[i].PreSaveAndQuit();
            }
        }

        public void SaveAndQuit(string key)
        {
            Main.PlaySound(SoundID.MenuClose);
            PreSaveAndQuit();
            ThreadPool.QueueUserWorkItem(SaveAndQuitCallBack, key);
        }

        public static void SaveAndQuitCallBack(object threadContext)
        {
            try
            {
                Main.PlaySound(SoundID.Waterfall, -1, -1, 0);
                Main.PlaySound(SoundID.Lavafall, -1, -1, 0);
            }
            catch
            {
            }

            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                WorldFile.CacheSaveTime();

                Main.menuMode = 10;
                EEMod.isSaving = true;
            }
            else
            {
                Main.menuMode = 889;
            }

            Main.invasionProgress = 0;
            Main.invasionProgressDisplayLeft = 0;
            Main.invasionProgressAlpha = 0f;
            Main.gameMenu = true;
            Main.StopTrackedSounds();
            CaptureInterface.ResetFocus();
            Main.ActivePlayerFileData.StopPlayTimer();
            Player.SavePlayer(Main.ActivePlayerFileData);

            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                WorldFile.saveWorld();
                Main.PlaySound(SoundID.MenuOpen);

                Main.fastForwardTime = false;

                Main.UpdateSundial();

                Main.menuMode = 0;
                serverState = EEServerState.SinglePlayer;
            }
            else
            {
                Netplay.disconnect = true;
                Main.netMode = NetmodeID.SinglePlayer;
                Main.fastForwardTime = false;

                Main.UpdateSundial();

                Main.menuMode = 889;
                serverState = EEServerState.MultiPlayer;
            }
            if (threadContext != null)
            {
                EnterSub((string)threadContext);
            }
        }

        public static void Do_worldGenCallBack(object threadContext)
        {
            Main.PlaySound(SoundID.MenuOpen);
            WorldGen.clearWorld();
            EEMod.GenerateWorld((string)threadContext, Main.ActiveWorldFileData.Seed, null);
            WorldFile.saveWorld(Main.ActiveWorldFileData.IsCloudSave, resetTime: true);

            Main.ActiveWorldFileData = WorldFile.GetAllMetadata($@"{EEPath}\{threadContext as string}.wld", false);

            WorldGen.playWorld();
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

        public static void CreateNewWorld(string text)
        {
            Main.rand = new UnifiedRandom(Main.ActiveWorldFileData.Seed);
            ThreadPool.QueueUserWorkItem(WorldGenCallBack, text);
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

        private static void OnWorldNamed(string text)
        {
            if (text != Main.LocalPlayer.GetModPlayer<EEPlayer>().baseWorldName)
            {
                EEPath = $@"{Main.SavePath}\Worlds\{Main.LocalPlayer.GetModPlayer<EEPlayer>().baseWorldName}Subworlds";

                if (!Directory.Exists(EEPath))
                {
                    Directory.CreateDirectory(EEPath);
                }

                string EESubworldPath = $@"{EEPath}\{text}.wld";

                Main.ActiveWorldFileData = WorldFile.GetAllMetadata(EESubworldPath, false);
                Main.ActivePlayerFileData.SetAsActive();

                if (!File.Exists(EESubworldPath))
                {
                    CreateNewWorld(text);

                    Main.ActiveWorldFileData = CreateSubworldMetaData(text, SocialAPI.Cloud != null && SocialAPI.Cloud.EnabledByDefault, EESubworldPath);
                    Main.worldName = text.Trim();

                    return;
                }
            }
            else
            {
                Main.spawnTileX = 100;
                Main.spawnTileY = EEWorld.EEWorld.TileCheckWater(100) - 22;
                Main.LocalPlayer.position = new Vector2(100, EEWorld.EEWorld.TileCheckWater(100) - 22);
                Main.ActiveWorldFileData = WorldFile.GetAllMetadata($@"{Main.SavePath}\Worlds\{text}.wld", false);
                Main.ActivePlayerFileData.SetAsActive();
            }

            /*string path = $@"{Main.SavePath}\Worlds\{text}.wld";

            if (!File.Exists(path))
            {
                Main.ActiveWorldFileData = WorldFile.CreateMetadata(text, SocialAPI.Cloud != null && SocialAPI.Cloud.EnabledByDefault, Main.expertMode);
                Main.worldName = text.Trim();
                CreateNewWorld(text);
                return;
            }*/

            if (serverState == EEServerState.SinglePlayer)
            {
                WorldGen.playWorld();
            }
            else
            {
                /*EEServer.StartInfo.FileName = "tModLoaderServer.exe";
                if (Main.libPath != "")
                {
                    ProcessStartInfo startInfo = EEServer.StartInfo;
                    startInfo.Arguments = startInfo.Arguments + " -loadlib " + Main.libPath;
                }
                EEServer.StartInfo.UseShellExecute = false;
                EEServer.StartInfo.CreateNoWindow = !Main.showServerConsole;
                if (SocialAPI.Network != null)
                {
                    SocialAPI.Network.LaunchLocalServer(EEServer, Main.MenuServerMode);
                }
                else
                {
                    EEServer.Start();
                }*/

                //Netplay.SetRemoteIP("127.0.0.1");

                //Main.autoPass = true;

                //Netplay.StartTcpClient();
                Main.clrInput();

                Netplay.ServerPassword = "";

                Main.GetInputText("");

                Main.autoPass = false;
                Main.menuMode = 30;

                Main.PlaySound(SoundID.MenuOpen);
            }
        }

        public static void StartClientGameplay()
        {
            Main.menuMode = 10;
            Netplay.StartTcpClient();
        }

        private void ReturnOnName(string text)
        {
            Main.ActiveWorldFileData = WorldFile.GetAllMetadata($@"{Main.SavePath}\Worlds\{text}.wld", false);
            WorldGen.playWorld();
        }

        public static void EnterSub(string key) => OnWorldNamed(key);

        public void Return(string baseWorldName) => ReturnOnName(baseWorldName);
    }
}