using System;
using System.IO;
using System.Threading;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Social;
using Terraria.Utilities;
using Terraria.World.Generation;
using Terraria.ModLoader;

namespace EEMod
{
    public class SubworldManager
    {
        public static int _lastSeed;

        private static WorldGenerator _generator;

        private static void AddGenerationPass(string name, WorldGenLegacyMethod method)
        {
            _generator.Append(new PassLegacy(name, method));
        }

        public static void Reset(int seed)
        {
            Logging.Terraria.InfoFormat("Generating World: {0}", (object)Main.ActiveWorldFileData.Name);
            _lastSeed = seed;
            _generator = new WorldGenerator(seed);
            WorldGen.structures = new StructureMap();
            MicroBiome.ResetAll();
            AddGenerationPass("Reset", delegate (GenerationProgress progress)
            {
                Liquid.ReInit();
                progress.Message = "Resetting";
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
            _generator.GenerateWorld(customProgressObject);
            Main.WorldFileMetadata = FileMetadata.FromCurrentSettings(FileType.World);
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
            EEMod.isSaving = true;
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
            }
            Main.invasionProgress = 0;
            Main.invasionProgressDisplayLeft = 0;
            Main.invasionProgressAlpha = 0f;
            Main.menuMode = 10;
            Main.gameMenu = true;
            Main.StopTrackedSounds();
            Terraria.Graphics.Capture.CaptureInterface.ResetFocus();
            Main.ActivePlayerFileData.StopPlayTimer();
            Player.SavePlayer(Main.ActivePlayerFileData);
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                WorldFile.saveWorld();
                Main.PlaySound(SoundID.MenuOpen);
            }
            else
            {
                Netplay.disconnect = true;
                Main.netMode = NetmodeID.SinglePlayer;
            }
            Main.fastForwardTime = false;
            Main.UpdateSundial();
            Main.menuMode = 0;
            if (threadContext != null)
            {
                EnterSub(threadContext as string);
            }
        }

        public static void Do_worldGenCallBack(object threadContext)
        {
            Main.PlaySound(SoundID.MenuOpen);
            WorldGen.clearWorld();
            EEMod.GenerateWorld(threadContext as string, Main.ActiveWorldFileData.Seed, null);
            WorldFile.saveWorld(Main.ActiveWorldFileData.IsCloudSave, resetTime: true);
            Main.ActiveWorldFileData = WorldFile.GetAllMetadata($@"{Main.SavePath}\Worlds\{threadContext as string}.wld", false);
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
                Logging.Terraria.Error((object)Language.GetTextValue("tModLoader.WorldGenError"), ex);
            }
        }
        public static void CreateNewWorld(string text)
        {
            Main.rand = new UnifiedRandom(Main.ActiveWorldFileData.Seed);
            ThreadPool.QueueUserWorkItem(WorldGenCallBack, text);
        }
        private static void OnWorldNamed(string text)
        {
            string path = $@"{Main.SavePath}\Worlds\{text}.wld";
            if (!File.Exists(path))
            {
                Main.ActiveWorldFileData = WorldFile.CreateMetadata(text, SocialAPI.Cloud != null && SocialAPI.Cloud.EnabledByDefault, Main.expertMode);
                Main.worldName = text.Trim();
                CreateNewWorld(text);
                return;
            }
            Main.ActiveWorldFileData = WorldFile.GetAllMetadata(path, false);
            WorldGen.playWorld();
        }
        private void ReturnOnName(string text)
        {
            Main.ActiveWorldFileData = WorldFile.GetAllMetadata($@"{Main.SavePath}\Worlds\{text}.wld", false);
            WorldGen.playWorld();
        }
        public static void EnterSub(string key)
        {
            OnWorldNamed(key);
        }
        public void Return(string baseWorldName)
        {
            ReturnOnName(baseWorldName);
        }
    }
}
