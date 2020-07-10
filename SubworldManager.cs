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
using Microsoft.Xna.Framework;
using EEMod.Tiles;

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

        public static void SettleLiquids()
        {
            AddGenerationPass("Settle Liquids", delegate (GenerationProgress progress)
            {
                progress.Message = "Settling Liquids";
                Liquid.QuickWater(3);
                WorldGen.WaterCheck();
                int num362 = 0;
                Liquid.quickSettle = true;
                while (num362 < 10)
                {
                    int num363 = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
                    num362++;
                    float num364 = 0f;
                    while (Liquid.numLiquid > 0)
                    {
                        float num365 = (float)(num363 - (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer)) / (float)num363;
                        if (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer > num363)
                        {
                            num363 = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
                        }
                        if (num365 > num364)
                        {
                            num364 = num365;
                        }
                        else
                        {
                            num365 = num364;
                        }
                        if (num362 == 1)
                        {
                            progress.Set(num365 / 3f + 0.33f);
                        }
                        int num366 = 10;
                        if (num362 > num366)
                        {
                            num366 = num362;
                        }
                        Liquid.UpdateLiquid();
                    }
                    WorldGen.WaterCheck();
                    progress.Set((float)num362 * 0.1f / 3f + 0.66f);
                }
                Liquid.quickSettle = false;
                Main.tileSolid[190] = true;
            });
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
            EEWorld.EEWorld.FillRegion(Main.maxTilesX, Main.maxTilesY, new Vector2(0, 0), ModContent.TileType<HardenedGemsandTile>());
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
            FileUtilities.Copy($@"{Main.SavePath}\Worlds\{threadContext as string}.wld", $@"{Main.SavePath}\EEWorlds" + ".wld", false);
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
                Logging.Terraria.Error(Language.GetTextValue("tModLoader.WorldGenError"), ex);
            }
        }
        public static void CreateNewWorld(string text)
        {
                Main.rand = new UnifiedRandom(Main.ActiveWorldFileData.Seed);
            ThreadPool.QueueUserWorkItem(WorldGenCallBack, text);
        }
        private static void OnWorldNamed(string text)
        {
            string EEpath = $@"{Main.SavePath}\EEWorlds";
            if (!Directory.Exists(EEpath))
            {
                DirectoryInfo di = Directory.CreateDirectory(EEpath);
            }

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
