using EEMod.NPCs.CoralReefs;
using EEMod.Tiles;
using EEMod.Tiles.EmptyTileArrays;
using EEMod.Tiles.Ores;
using EEMod.Tiles.Walls;
using IL.Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;
using static EEMod.EEWorld.EEWorld;
using EEMod.Autoloading;
using EEMod.ID;
using EEMod.Tiles.Foliage.Coral;
using EEMod.Tiles.Foliage.Coral.HangingCoral;
using EEMod.Tiles.Foliage.Coral.WallCoral;
using EEMod.Systems.Noise;
using EEMod.VerletIntegration;
using EEMod.Tiles.Foliage;

namespace EEMod
{
    public class EESubWorlds
    {
        [FieldInit] public static IList<Vector2> CoralReefVineLocations = new List<Vector2>();
        [FieldInit] public static IList<Vector3> MinibiomeLocations = new List<Vector3>();
        [FieldInit] public static IList<Vector2> OrbPositions = new List<Vector2>();
        [FieldInit] public static IList<Vector2> BulbousTreePosition = new List<Vector2>();

        [FieldInit] public static IList<Vector2> CoralCrystalPosition = new List<Vector2>();
        [FieldInit] public static IList<Vector2> AquamarineZiplineLocations = new List<Vector2>();
        [FieldInit] public static IList<Vector2> ThinCrystalBambooLocations = new List<Vector2>();

        [FieldInit] public static IList<Vector2> GiantKelpRoots = new List<Vector2>();
        [FieldInit] public static IList<Vector2> WebPositions = new List<Vector2>();

        public static Vector2 CoralBoatPos;
        public static Vector2 SpirePosition = Vector2.Zero;

        public static void Pyramids(int seed, GenerationProgress customProgressObject = null)
        {
            Main.maxTilesX = 400;
            Main.maxTilesY = 400;
            Main.spawnTileX = 234;
            Main.spawnTileY = 92;
            SubworldManager.Reset(seed);
            SubworldManager.PostReset(customProgressObject);
            FillRegion(400, 400, Vector2.Zero, TileID.SandstoneBrick);
            Pyramid(63, 42);
            EEMod.isSaving = false;
        }

        public static void Sea(int seed, GenerationProgress customProgressObject = null)
        {
            Main.maxTilesX = 400;
            Main.maxTilesY = 405;
            Main.spawnTileX = 234;
            Main.spawnTileY = 92;
            SubworldManager.Reset(seed);
            SubworldManager.PostReset(customProgressObject);
            EEMod.isSaving = false;
        }

        public static void CoralReefs(int seed, GenerationProgress customProgressObject = null)
        {
            EEMod.progressMessage = "Generating Coral Reefs";

            //Variables and Initialization stuff
            Main.maxTilesX = 1500;
            Main.maxTilesY = 2400;

            int roomsPerLayer = 10;
            int depth = Main.maxTilesY / 20;
            int boatPos = 300;

            SubworldManager.Reset(seed);
            SubworldManager.PostReset(customProgressObject);

            VerletHelpers.SwingableVines.Clear();

            //Placing initial blocks
            #region Initial block placement
            EEMod.progressMessage = "Initial generation";

            FillRegion(Main.maxTilesX, (Main.maxTilesY / 10) * 4, Vector2.Zero, ModContent.TileType<LightGemsandTile>());

            FillRegionNoEditWithNoise(Main.maxTilesX, Main.maxTilesY / 60, new Vector2(0, (Main.maxTilesY / 10) * 4 - (Main.maxTilesY / 60)), ModContent.TileType<GemsandTile>(), 20);
            FillRegionNoEdit(Main.maxTilesX, (Main.maxTilesY / 10) * 3, new Vector2(0, (Main.maxTilesY / 10) * 4), ModContent.TileType<GemsandTile>());

            FillRegionNoEditWithNoise(Main.maxTilesX, Main.maxTilesY / 60, new Vector2(0, (Main.maxTilesY / 10) * 7 - (Main.maxTilesY / 60)), ModContent.TileType<DarkGemsandTile>(), 20);
            FillRegionNoEdit(Main.maxTilesX, (Main.maxTilesY / 10) * 3, new Vector2(0, (Main.maxTilesY / 10) * 7), ModContent.TileType<DarkGemsandTile>());

            ClearRegion(Main.maxTilesX, (Main.maxTilesY / 20) + (Main.maxTilesY / 60) + (Main.maxTilesY / 60), Vector2.Zero);

            #endregion

            #region Surface reefs
            EEMod.progressMessage = "Making the surface";

            NoiseGenWave(new Vector2(0, 130), new Vector2(Main.maxTilesX, Main.maxTilesY / 20), new Vector2(20, 100), (ushort)ModContent.TileType<CoralsandstoneTile>(), 0.5f);
            NoiseGenWave(new Vector2(0, 110), new Vector2(Main.maxTilesX, Main.maxTilesY / 20), new Vector2(50, 50), TileID.StoneSlab, 0.6f);

            RemoveStoneSlabs();

            //Plateaus
            FillRegionEditWithNoise(Main.maxTilesX, Main.maxTilesY / 40, new Vector2(0, 120), ModContent.TileType<CoralSandTile>(), 8);
            FillRegionEditWithNoise(Main.maxTilesX, Main.maxTilesY / 120, new Vector2(0, 155), ModContent.TileType<CoralsandstoneTile>(), 8);

            //Ground
            FillRegionNoEditWithNoise(Main.maxTilesX, Main.maxTilesY / 80, new Vector2(0, 170), ModContent.TileType<CoralsandstoneTile>(), 5);
            FillRegionNoChangeWithNoise(Main.maxTilesX, Main.maxTilesY / 80, new Vector2(0, 165), ModContent.TileType<CoralSandTile>(), 8);

            FillRegionEditWithNoise(Main.maxTilesX, Main.maxTilesY / 40, new Vector2(0, 190), ModContent.TileType<LightGemsandTile>(), 10);

            for(int i = 50; i < Main.maxTilesX - 100; i++)
            {
                if (i >= boatPos - 60 && i <= boatPos + ShipTiles.GetLength(1)) i += 50;

                if(WorldGen.genRand.NextBool(200))
                {
                    int width = Main.rand.Next(40, 60);
                    int height = Main.rand.Next(20, 25);
                    MakeOvalJaggedTop(width, height, new Vector2(i, depth - 10), ModContent.TileType<CoralSandTile>());

                    MakeOval(width - 10, 10, new Vector2(i + 5, depth - 5), TileID.Dirt, true);

                    for (int k = i; k < i + 25; k++)
                    {
                        for (int l = depth - 15; l < depth + 10; l++)
                        {
                            WorldGen.SpreadGrass(k, l);
                        }
                    }

                    i += 50;
                }
            }

            #endregion

            #region Finding suitable room positions
            EEMod.progressMessage = "Finding suitable room positions";

            int[] biomes = Helpers.FillUniformArray(roomsPerLayer * 2, 0, 3);

            Vector2[] upperRoomPositions = MakeDistantLocations(roomsPerLayer, 150, new Rectangle(200, 265, Main.maxTilesX - 400, ((Main.maxTilesY / 10) * 4) - (265 + 100)), 5000);
            Vector2[] lowerRoomPositions = MakeDistantLocations(roomsPerLayer, 150, new Rectangle(200, (Main.maxTilesY / 10) * 4, Main.maxTilesX - 400, (Main.maxTilesY / 10) * 3), 5000);
            Vector2[] depthsRoomPositions = MakeDistantLocations(roomsPerLayer / 2, 200, new Rectangle(300, (Main.maxTilesY / 10) * 7, Main.maxTilesX - 600, ((Main.maxTilesY / 10) * 3) - 300), 5000);

            #endregion

            #region Generating chasms
            EEMod.progressMessage = "Generating chasms";

            /*int highestUpperRoom = 0;
            int lowestUpperRoom = 0;
            for (int i = 0; i < upperRoomPositions.Length; i++)
            {
                if (upperRoomPositions[i].Y < upperRoomPositions[highestUpperRoom].Y) highestUpperRoom = i;
                if (upperRoomPositions[i].Y > upperRoomPositions[lowestUpperRoom].Y) lowestUpperRoom = i;
            }

            MakeWavyChasm3(upperRoomPositions[highestUpperRoom], new Vector2(upperRoomPositions[highestUpperRoom].X + Main.rand.Next(-100, 100), 0), TileID.StoneSlab, 100, WorldGen.genRand.Next(10, 20), true, new Vector2(10, 20), WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(5, 10), true, 51, WorldGen.genRand.Next(80, 120));

            float dist = 1000000;
            int closestLowerRoom = 0;

            for(int i = 0; i < lowerRoomPositions.Length; i++)
            {
                if(Vector2.DistanceSquared(upperRoomPositions[lowestUpperRoom], lowerRoomPositions[i]) < dist)
                {
                    dist = Vector2.DistanceSquared(upperRoomPositions[lowestUpperRoom], lowerRoomPositions[i]);
                    closestLowerRoom = i;
                }
            }

            MakeWavyChasm3(upperRoomPositions[lowestUpperRoom], lowerRoomPositions[closestLowerRoom], TileID.StoneSlab, 100, WorldGen.genRand.Next(10, 20), true, new Vector2(10, 20), WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(5, 10), true, 51, WorldGen.genRand.Next(80, 120));

            for (int i = 0; i < upperRoomPositions.Length; i++)
            {
                for (int j = i; j < upperRoomPositions.Length; j++)
                {
                    if (Vector2.DistanceSquared(upperRoomPositions[i], upperRoomPositions[j]) <= 90000)
                    {
                        MakeWavyChasm3(upperRoomPositions[i], upperRoomPositions[j], TileID.StoneSlab, 100, WorldGen.genRand.Next(10, 20), true, new Vector2(10, 20), WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(5, 10), true, 51, WorldGen.genRand.Next(80, 120));
                    }
                }
            }*/

            RemoveStoneSlabs();

            EEMod.progressMessage = "Placing chasm coral";
            TilePopulate(
                new int[] { ModContent.TileType<Hanging1x2Coral>(),
                ModContent.TileType<Hanging1x3Coral>(),
                ModContent.TileType<Hanging2x3Coral>(),
                ModContent.TileType<Hanging2x4Coral>(),
                ModContent.TileType<Hanging1x4Coral>(),

                ModContent.TileType<Floor1x1Coral>(),
                ModContent.TileType<Floor1x2Coral>(),
                ModContent.TileType<Floor2x1Coral>(),
                ModContent.TileType<Floor2x2Coral>(),
                ModContent.TileType<FloorGlow2x2Coral>(),
                ModContent.TileType<Floor2x6Coral>(),
                ModContent.TileType<Floor3x2Coral>(),
                ModContent.TileType<Floor3x3Coral>(),
                ModContent.TileType<Floor4x3Coral>(),
                ModContent.TileType<Floor7x7Coral>(),
                ModContent.TileType<Floor8x7Coral>(),
                ModContent.TileType<Floor8x3Coral>(),
                ModContent.TileType<FloorGlow9x4Coral>(),
                ModContent.TileType<Floor9x9Coral>(),
                ModContent.TileType<Floor11x11Coral>(),

                ModContent.TileType<Wall2x2CoralL>(),
                ModContent.TileType<Wall3x2CoralL>(),
                ModContent.TileType<Wall3x2NonsolidCoralL>(),
                ModContent.TileType<Wall5x2NonsolidCoralL>(),
                ModContent.TileType<Wall6x3CoralL>(),

                ModContent.TileType<Wall2x2CoralR>(),
                ModContent.TileType<Wall3x2CoralR>(),
                ModContent.TileType<Wall3x2NonsolidCoralR>(),
                ModContent.TileType<Wall5x2NonsolidCoralR>(),
                ModContent.TileType<Wall6x3CoralR>() },
            new Rectangle(42, depth, Main.maxTilesX - 42, Main.maxTilesY - depth - 42));

            TilePopulate(
                new int[] { ModContent.TileType<BigTropicalTree>(),
                ModContent.TileType<TropicalTree>(), },
            new Rectangle(42, 42, Main.maxTilesX - 42, depth), 3);

            #endregion

            #region Spawning rooms

            for (int i = 0; i < upperRoomPositions.Length; i++)
            {
                EEMod.progressMessage = "Generating rooms pt. 1 " + (i * 100 / upperRoomPositions.Length) + "%";
                MakeCoralRoom((int)upperRoomPositions[i].X, (int)upperRoomPositions[i].Y, 100, 50, biomes[i]);
            }

            for (int j = 0; j < lowerRoomPositions.Length; j++)
            {
                int biome = biomes[j + (upperRoomPositions.Length - 1)];
                if (biome > 0) biome += 2;

                EEMod.progressMessage = "Generating rooms pt. 2 " + (j * 100 / lowerRoomPositions.Length) + "%";
                MakeCoralRoom((int)lowerRoomPositions[j].X, (int)lowerRoomPositions[j].Y, 100, 50, biome);
            }

            /*for (int k = 0; k < depthsRoomPositions.Length; k++)
            {
                EEMod.progressMessage = "Generating rooms pt. 3 " + (k * 100 / depthsRoomPositions.Length) + "%";
                MakeCoralRoom((int)depthsRoomPositions[k].X, (int)depthsRoomPositions[k].Y, 150, 75, 0);
            }*/
            
            #endregion

            RemoveStoneSlabs();

            try
            {
                #region Shipwrecks
                EEMod.progressMessage = "Wrecking ships";

                int maxIt = 0;
                int mlem = 0;
                while (mlem < 2 && maxIt < 300)
                {
                    int tileX = WorldGen.genRand.Next(50, Main.maxTilesX - 50);
                    int tileY = TileCheck(tileX, ModContent.TileType<CoralSandTile>());
                    if (Math.Abs(TileCheck(tileX + Ship1.GetLength(1), ModContent.TileType<CoralSandTile>()) - tileY) <= 2 && Math.Abs(TileCheck(tileX + (Ship1.GetLength(1) / 2), ModContent.TileType<CoralSandTile>()) - tileY) <= 3 && tileY > depth + 10)
                    {
                        PlaceStructure(tileX, tileY - Ship1.GetLength(0) + 7, Ship1);
                        mlem++;
                    }
                    else
                    {
                        continue;
                    }
                }

                maxIt = 0;
                mlem = 0;
                while (mlem < 2 && maxIt < 300)
                {
                    int tileX = WorldGen.genRand.Next(50, Main.maxTilesX - 50);
                    int tileY = TileCheck(tileX, ModContent.TileType<CoralSandTile>());
                    if (Math.Abs(TileCheck(tileX + Ship2.GetLength(1), ModContent.TileType<CoralSandTile>()) - tileY) <= 2 && Math.Abs(TileCheck(tileX + (Ship2.GetLength(1) / 2), ModContent.TileType<CoralSandTile>()) - tileY) <= 3 && tileY > depth + 10)
                    {
                        PlaceStructure(tileX, tileY - Ship2.GetLength(0) + 7, Ship2);
                        mlem++;
                    }
                    else
                    {
                        continue;
                    }
                }

                maxIt = 0;
                mlem = 0;
                while (mlem < 2 && maxIt < 300)
                {
                    int tileX = WorldGen.genRand.Next(50, Main.maxTilesX - 100);
                    int tileY = TileCheck(tileX, ModContent.TileType<CoralSandTile>());
                    if (Math.Abs(TileCheck(tileX + Ship3.GetLength(1), ModContent.TileType<CoralSandTile>()) - tileY) <= 2 && Math.Abs(TileCheck(tileX + (Ship3.GetLength(1) / 2), ModContent.TileType<CoralSandTile>()) - tileY) <= 3 && tileY > depth + 10)
                    {
                        PlaceStructure(tileX, tileY - Ship3.GetLength(0) + 7, Ship3);
                        mlem++;
                    }
                    else
                    {
                        continue;
                    }
                }
                #endregion

                #region Placing moss
                EEMod.progressMessage = "Mossifying";

                perlinNoise = new PerlinNoiseFunction(Main.maxTilesX, (int)(Main.maxTilesY * 0.9f), 50, 50, 0.8f);
                int[,] perlinNoiseFunction2 = perlinNoise.perlinBinary;
                for (int i = 42; i < Main.maxTilesX - 42; i++)
                {
                    for (int j = (Main.maxTilesY / 10); j < Main.maxTilesY - 42; j++)
                    {
                        if (perlinNoiseFunction2[i, j - (Main.maxTilesY / 10)] == 1)
                        {
                            int minibiome = 0;
                            List<float> BufferLengths = new List<float>();
                            List<int> BufferMinibiome = new List<int>();
                            for (int k = 0; k < MinibiomeLocations.Count; k++)
                            {
                                if (Vector2.DistanceSquared(new Vector2(MinibiomeLocations[k].X, MinibiomeLocations[k].Y), new Vector2(i, j)) < (180 * 180) && MinibiomeLocations[k].Z != 0)
                                {
                                    BufferLengths.Add(Vector2.DistanceSquared(new Vector2(MinibiomeLocations[k].X, MinibiomeLocations[k].Y), new Vector2(i, j)));
                                    BufferMinibiome.Add((int)MinibiomeLocations[k].Z);
                                }
                            }
                            float MakingMyWayDownTown = -1;
                            int WalkingFast = -1;
                            for (int a = 0; a < BufferLengths.Count; a++)
                            {
                                if (BufferLengths[a] < MakingMyWayDownTown || MakingMyWayDownTown == -1)
                                {
                                    MakingMyWayDownTown = BufferLengths[a];
                                    WalkingFast = BufferMinibiome[a];
                                }
                            }
                            if (WalkingFast != -1) minibiome = WalkingFast;


                            if ((Framing.GetTileSafely(i, j).type == ModContent.TileType<KelpLeafTile>()) &&
                                (MinibiomeID)minibiome == MinibiomeID.KelpForest)
                                Framing.GetTileSafely(i, j).type = (ushort)ModContent.TileType<KelpMossTile>();

                            if (Framing.GetTileSafely(i, j).type == ModContent.TileType<ScorchedGemsandTile>() && 
                                (MinibiomeID)minibiome == MinibiomeID.ThermalVents)
                                Framing.GetTileSafely(i, j).type = (ushort)ModContent.TileType<ThermalMossTile>();
                        }
                    }
                }
                #endregion

                #region Implementing dynamic objects
                EEMod.progressMessage = "Adding Dynamics";

                for (int j = 42; j < ((Main.maxTilesY / 10f) * 4f); j += 2)
                {
                    for (int i = 42; i < Main.maxTilesX - 42; i += 2)
                    {
                        /*int noOfTiles = 0;
                        if (j > 200)
                        {
                            /*for (int m = 0; m < OrbPositions.Count; m++)
                            {
                                if (Vector2.DistanceSquared(new Vector2(i, j), OrbPositions[m]) < 200 * 200)
                                {
                                    noOfTiles++;
                                }
                            }
                            if (noOfTiles <= 2)
                            {
                                OrbPositions.Add(new Vector2(i, j));
                            }
                            int funnyDist = 0;
                            /*for (int m = 0; m < OrbPositions.Count; m++)
                            {
                                if (Vector2.DistanceSquared(new Vector2(i, j), OrbPositions[m]) < 200 * 200)
                                {
                                    funnyDist++;
                                }
                            }
                        }*/

                        /*for (int m = 0; m < BulbousTreePosition.Count; m++)
                        {
                            if (Vector2.DistanceSquared(new Vector2(i, j), BulbousTreePosition[m]) < 20 * 20)
                            {
                                ifa++;
                            }
                        }*/

                        if ((TileCheck2(i, j) == 3 || TileCheck2(i, j) == 4) && WorldGen.genRand.NextBool(3))
                        {
                            if (CoralReefVineLocations.Count == 0)
                            {
                                CoralReefVineLocations.Add(new Vector2(i, j));
                            }
                            else
                            {
                                Vector2 lastPos = CoralReefVineLocations[CoralReefVineLocations.Count - 1];
                                if (Vector2.DistanceSquared(lastPos, new Vector2(i, j)) > 5 * 5 &&
                                    Vector2.DistanceSquared(lastPos, new Vector2(i, j)) < 55 * 55 ||
                                    Vector2.DistanceSquared(lastPos, new Vector2(i, j)) > 150 * 150)
                                {
                                    CoralReefVineLocations.Add(new Vector2(i, j));
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Smoothing
                EEMod.progressMessage = "Making the world look nice";

                for (int i = 2; i < Main.maxTilesX - 2; i++)
                {
                    for (int j = 2; j < Main.maxTilesY - 2; j++)
                    {
                        if (WorldGen.genRand.NextBool(3))
                        {
                            Tile.SmoothSlope(i, j);
                        }
                        if (!Framing.GetTileSafely(i, j + 1).active() && !Framing.GetTileSafely(i, j - 1).active() && !Framing.GetTileSafely(i + 1, j).active() && !Framing.GetTileSafely(i - 1, j).active())
                        {
                            WorldGen.KillTile(i, j);
                        }
                    }
                }
                #endregion

                FillRegionWithWater(Main.maxTilesX, Main.maxTilesY - depth, new Vector2(0, depth));

                #region Removing dirt walls
                for (int i = 2; i < Main.maxTilesX - 2; i++)
                {
                    for (int j = 2; j < Main.maxTilesY - 2; j++)
                    {
                        if (Framing.GetTileSafely(i, j).wall == WallID.Dirt || Framing.GetTileSafely(i, j).wall == WallID.DirtUnsafe || Framing.GetTileSafely(i, j).wall == WallID.DirtUnsafe1 || Framing.GetTileSafely(i, j).wall == WallID.DirtUnsafe2 || Framing.GetTileSafely(i, j).wall == WallID.DirtUnsafe3 || Framing.GetTileSafely(i, j).wall == WallID.DirtUnsafe4)
                        {
                            WorldGen.KillWall(i, j);
                        }

                        WorldGen.SquareTileFrame(i, j);
                    }
                }
                #endregion

                #region Placing the boat
                EEMod.progressMessage = "Placing boat";

                int watercheck = TileCheckWater(boatPos) - 22;
                RemoveWaterFromRegion(ShipTiles.GetLength(1), ShipTiles.GetLength(0), new Vector2(boatPos, watercheck));

                PlaceShipWalls(boatPos, watercheck, ShipWalls);
                PlaceShip(boatPos, watercheck, ShipTiles);
                CoralBoatPos = new Vector2(boatPos, watercheck);

                #endregion
            }
            catch (Exception e)
            {
                EEMod.progressMessage = "Unsuccessful!";
                EEMod.progressMessage = e.ToString();
                SubworldManager.PreSaveAndQuit();
                return;
            }

            //Finishing initialization stuff
            EEMod.progressMessage = "Successful!";
            EEMod.isSaving = false;

            Main.spawnTileX = boatPos;
            Main.spawnTileY = TileCheckWater(boatPos) - 22;

            EEMod.progressMessage = null;
        }












        public static void Island(int seed, GenerationProgress customProgressObject = null)
        {
            Main.maxTilesX = 1000;
            Main.maxTilesY = 500;
            SubworldManager.Reset(seed);
            SubworldManager.PostReset(customProgressObject);

            FillRegionWithWater(Main.maxTilesX, Main.maxTilesY, Vector2.Zero);
            RemoveWaterFromRegion(Main.maxTilesX, 170, Vector2.Zero);

            MakeOvalJaggedTop(Main.maxTilesX, 50, new Vector2(0, 165), ModContent.TileType<CoralSandTile>(), 15, 15);

            EEWorld.EEWorld.Island(800, 250, 140);

            FillRegion(Main.maxTilesX, Main.maxTilesY - 190, new Vector2(0, 190), ModContent.TileType<CoralSandTile>());

            for (int i = 42; i < Main.maxTilesX - 42; i++)
            {
                for (int j = 42; j < Main.maxTilesY - 42; j++)
                {
                    int yes = WorldGen.genRand.Next(0, 5);
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (TileCheck2(i, j) == 2 && yes < 3 && tile.type == ModContent.TileType<CoralSandTile>())
                    {
                        int selection = WorldGen.genRand.Next(3);
                        switch (selection)
                        {
                            case 0:
                                WorldGen.PlaceTile(i, j - 1, 324);
                                break;

                            case 1:
                                WorldGen.PlaceTile(i, j - 1, 324, style: 2);
                                break;

                            case 2:
                                WorldGen.PlaceTile(i, j - 1, TileID.Coral);
                                break;
                        }
                    }
                    yes = WorldGen.genRand.Next(0, 10);
                    if (TileCheck2(i, j) == 2 && yes == 0 && tile.type == TileID.Grass)
                    {
                        WorldGen.GrowTree(i, j - 1);
                    }
                }
            }

            for (int i = 2; i < Main.maxTilesX - 2; i++)
            {
                for (int j = 2; j < Main.maxTilesY - 2; j++)
                {
                    Tile.SmoothSlope(i, j);
                }
            }

            PlaceShip(50, 150, ShipTiles);
            PlaceShipWalls(50, 145, ShipWalls);
            WorldGen.AddTrees();

            /*PlaceAnyBuilding(100, 100, IceShrine);
            PlaceAnyBuilding(200, 100, FireShrine);
            PlaceAnyBuilding(300, 100, DesertShrine);
            PlaceAnyBuilding(400, 100, WaterShrine);
            PlaceAnyBuilding(500, 100, LeafShrine);*/
            SubworldManager.SettleLiquids();
            EEMod.isSaving = false;
            Main.spawnTileX = 200;
            Main.spawnTileY = 100;
        }

        public static void Island2(int seed, GenerationProgress customProgressObject = null)
        {
            Main.maxTilesX = 1000;
            Main.maxTilesY = 500;
            SubworldManager.Reset(seed);
            SubworldManager.PostReset(customProgressObject);

            FillRegionWithWater(Main.maxTilesX, Main.maxTilesY, Vector2.Zero);
            RemoveWaterFromRegion(Main.maxTilesX, 170, Vector2.Zero);

            MakeOvalJaggedTop(Main.maxTilesX, 50, new Vector2(0, 165), ModContent.TileType<CoralSandTile>(), 15, 15);

            EEWorld.EEWorld.Island(600, 250, 140);

            FillRegion(Main.maxTilesX, Main.maxTilesY - 190, new Vector2(0, 190), ModContent.TileType<CoralSandTile>());

            for (int i = 42; i < Main.maxTilesX - 42; i++)
            {
                for (int j = 42; j < Main.maxTilesY - 42; j++)
                {
                    int yes = WorldGen.genRand.Next(0, 5);
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (TileCheck2(i, j) == 2 && yes < 3 && tile.type == ModContent.TileType<CoralSandTile>())
                    {
                        int selection = WorldGen.genRand.Next(3);
                        switch (selection)
                        {
                            case 0:
                                WorldGen.PlaceTile(i, j - 1, 324);
                                break;

                            case 1:
                                WorldGen.PlaceTile(i, j - 1, 324, style: 2);
                                break;

                            case 2:
                                WorldGen.PlaceTile(i, j - 1, TileID.Coral);
                                break;
                        }
                    }
                    yes = WorldGen.genRand.Next(0, 10);
                    if (TileCheck2(i, j) == 2 && yes == 0 && tile.type == TileID.Grass)
                    {
                        WorldGen.GrowTree(i, j - 1);
                    }
                }
            }

            for (int i = 2; i < Main.maxTilesX - 2; i++)
            {
                for (int j = 2; j < Main.maxTilesY - 2; j++)
                {
                    Tile.SmoothSlope(i, j);
                }
            }

            PlaceShip(50, 150, ShipTiles);
            PlaceShipWalls(50, 145, ShipWalls);

            WorldGen.AddTrees();

            SubworldManager.SettleLiquids();
            EEMod.isSaving = false;
            Main.spawnTileX = 200;
            Main.spawnTileY = 100;
        }

        public static void Cutscene1(int seed, GenerationProgress customProgressObject = null)
        {
            Main.maxTilesX = 400;
            Main.maxTilesY = 400;
            SubworldManager.Reset(seed);
            SubworldManager.PostReset(customProgressObject);
            /*FillRegion(Main.maxTilesX, Main.maxTilesY, Vector2.Zero, ModContent.TileType<VolcanicAshTile>());
            MakeLayer(200, 100, 90, 1, ModContent.TileType<VolcanicAshTile>());
            MakeOvalFlatTop(40, 10, new Vector2(200 - 20, 100), ModContent.TileType<VolcanicAshTile>());
            MakeChasm(200, 140, 110, ModContent.TileType<GemsandTile>(), 0, 5, 0);
            for (int i = 0; i < 400; i++)
            {
                for (int j = 0; j < 400; j++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (tile.type == ModContent.TileType<GemsandTile>())
                    {
                        WorldGen.KillTile(i, j);
                    }
                }
            }
            KillWall(Main.maxTilesX, Main.maxTilesY, Vector2.Zero);
            FillWall(Main.maxTilesX, Main.maxTilesY, Vector2.Zero, ModContent.WallType<MagmastoneWallTile>());*/
            SubworldManager.SettleLiquids();
            EEMod.isSaving = false;
            Main.spawnTileX = 200;
            Main.spawnTileY = 140;
        }

        public static void VolcanoIsland(int seed, GenerationProgress customProgressObject = null)
        {
            Main.maxTilesX = 1200;
            Main.maxTilesY = 800;
            //Main.worldSurface = Main.maxTilesY;
            //Main.rockLayer = Main.maxTilesY;
            SubworldManager.Reset(seed);
            SubworldManager.PostReset(customProgressObject);

            FillRegionWithWater(Main.maxTilesX, Main.maxTilesY, Vector2.Zero);
            RemoveWaterFromRegion(Main.maxTilesX, 360, Vector2.Zero);

            RemoveWaterFromRegion(60, 630, new Vector2(570, 170));
            KillWall(Main.maxTilesX, Main.maxTilesY, Vector2.Zero);
            //MakeTriangle(new Vector2(300, 895), 600, 1000, 3, ModContent.TileType<VolcanicAshTile>(), ModContent.WallType<VolcanicAshWallTile>());
            EEWorld.EEWorld.Island(800, 400, 290);
            FillRegion(Main.maxTilesX, Main.maxTilesY - 190, new Vector2(0, 400), ModContent.TileType<CoralSandTile>());

            ClearRegionSafely(60, 630, new Vector2(570, 170), ModContent.TileType<CoralSandTile>());
            ClearRegionSafely(60, 630, new Vector2(570, 170), TileID.Dirt);
            ClearRegionSafely(60, 630, new Vector2(570, 170), TileID.Grass);
            FillRegionWithLava(40, 206, new Vector2(580, 594));
            MakeVolcanoEntrance(598, 596, VolcanoEntrance);

            SubworldManager.SettleLiquids();
            EEMod.isSaving = false;
            Main.spawnTileX = 200;
            Main.spawnTileY = 100;
        }

        public static void VolcanoInside(int seed, GenerationProgress customProgressObject = null)
        {
            Main.maxTilesX = 400;
            Main.maxTilesY = 600;
            //Main.worldSurface = Main.maxTilesY;
            //Main.rockLayer = Main.maxTilesY;
            SubworldManager.Reset(seed);
            SubworldManager.PostReset(customProgressObject);

            /*FillRegion(Main.maxTilesX, Main.maxTilesY, Vector2.Zero, ModContent.TileType<MagmastoneTile>());
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    if (WorldGen.genRand.NextBool(3000))
                    {
                        MakeLavaPit(WorldGen.genRand.Next(20, 30), WorldGen.genRand.Next(7, 20), new Vector2(i, j), WorldGen.genRand.NextFloat(0.1f, 0.5f));
                    }
                }
            }
            MakeChasm(200, 10, 100, TileID.StoneSlab, 0, 10, 20);
            WorldGen.TileRunner(200, 190, 200, 100, TileID.StoneSlab);
            for (int k = 0; k < Main.maxTilesX; k++)
            {
                for (int l = 0; l < Main.maxTilesY; l++)
                {
                    if (Framing.GetTileSafely(k, l).type == TileID.StoneSlab)
                    {
                        WorldGen.KillTile(k, l);
                    }
                }
            }
            MakeOvalJaggedTop(80, 60, new Vector2(160, 170), ModContent.TileType<MagmastoneTile>());
            KillWall(Main.maxTilesX, Main.maxTilesY, Vector2.Zero);
            FillWall(Main.maxTilesX, Main.maxTilesY, Vector2.Zero, ModContent.WallType<MagmastoneWallTile>());*/
        }
    }
}