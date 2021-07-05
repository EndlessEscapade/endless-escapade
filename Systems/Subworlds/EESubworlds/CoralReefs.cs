using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Input;
using EEMod.ID;
using EEMod.Tiles;
using EEMod.VerletIntegration;
using static EEMod.EEWorld.EEWorld;
using EEMod.Tiles.Foliage.Coral.HangingCoral;
using EEMod.Tiles.Foliage.Coral.WallCoral;
using EEMod.Tiles.Foliage.Coral;
using EEMod.Tiles.Foliage;
using System;
using EEMod.Systems.Noise;
using System.Collections.Generic;
using EEMod.Autoloading;
using Terraria.World.Generation;

namespace EEMod.Systems.Subworlds.EESubworlds
{
    public class CoralReefs : Subworld
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

        public override Point Dimensions => new Point(500,500);

        public override Point SpawnTile => new Point(10,200);

        public override string Name => "CoralReefs";

        internal override void WorldGeneration(int seed, GenerationProgress customProgressObject = null)
        {
            var rand = WorldGen.genRand;
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

            for (int i = 50; i < Main.maxTilesX - 100; i++)
            {
                if (i >= boatPos - 60 && i <= boatPos + ShipTiles.GetLength(1)) i += 50;

                if (WorldGen.genRand.NextBool(200))
                {
                    int width = WorldGen.genRand.Next(40, 60);
                    int height = WorldGen.genRand.Next(20, 25);

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

            MakeWavyChasm3(upperRoomPositions[highestUpperRoom], new Vector2(upperRoomPositions[highestUpperRoom].X + WorldGen.genRand.Next(-100, 100), 0), TileID.StoneSlab, 100, WorldGen.genRand.Next(10, 20), true, new Vector2(10, 20), WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(5, 10), true, 51, WorldGen.genRand.Next(80, 120));
            
            MakeWavyChasm3(upperRoomPositions[highestUpperRoom], new Vector2(upperRoomPositions[highestUpperRoom].X + rand.Next(-100, 100), 0), TileID.StoneSlab, 100, WorldGen.genRand.Next(10, 20), true, new Vector2(10, 20), WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(5, 10), true, 51, WorldGen.genRand.Next(80, 120));

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
                EEMod.progressMessage = "Generating rooms pt. 1 " + (i * 100f / upperRoomPositions.Length) + "%";
                MakeCoralRoom((int)upperRoomPositions[i].X, (int)upperRoomPositions[i].Y, 100, 50, biomes[i]);
            }

            for (int j = 0; j < lowerRoomPositions.Length; j++)
            {
                int biome = biomes[j + (upperRoomPositions.Length - 1)];
                if (biome > 0) biome += 2;

                EEMod.progressMessage = "Generating rooms pt. 2 " + (j * 100f / lowerRoomPositions.Length) + "%";
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

                #region Placing moss and seagrass
                EEMod.progressMessage = "Mossifying";

                perlinNoise = new PerlinNoiseFunction(Main.maxTilesX, (int)(Main.maxTilesY * 0.9f), 50, 50, 0.8f, rand);
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
                
                FillRegionWithWater(Main.maxTilesX, Main.maxTilesY - depth, new Vector2(0, depth));

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

                int watercheck = depth - 22;

                PlaceShipWalls(boatPos, watercheck, ShipWalls);
                PlaceShip(boatPos, watercheck, ShipTiles);
                CoralBoatPos = new Vector2(boatPos, watercheck);

                for (int i = 42; i < Main.maxTilesX - 42; i++)
                {
                    if (WorldGen.genRand.NextBool(4))
                    {
                        if (TileCheck(i, ModContent.TileType<CoralSandTile>()) > depth)
                        {
                            int ballfart = TileCheck(i, ModContent.TileType<CoralSandTile>());

                            int random = WorldGen.genRand.Next(4, 15);

                            for (int j = 1; j < random; j++)
                            {
                                if (Framing.GetTileSafely(i, ballfart - j).active() || Framing.GetTileSafely(i, ballfart - j).liquid < 64) break;

                                WorldGen.PlaceTile(i, ballfart - j, ModContent.TileType<SeagrassTile>());
                            }
                        }
                    }
                }

                for (int i = 42; i < Main.maxTilesX - 42; i++)
                {
                    if (WorldGen.genRand.NextBool(6))
                    {
                        if (!Framing.GetTileSafely(i, TileCheckWater(i) - 1).active() && !Framing.GetTileSafely(i, TileCheckWater(i)).active() && !Framing.GetTileSafely(i, TileCheckWater(i + 1)).active())
                        {
                            int ballfart = TileCheckWater(i);

                            switch (WorldGen.genRand.Next(2))
                            {
                                case 0:
                                    WorldGen.PlaceTile(i, ballfart - 1, ModContent.TileType<LilyPadSmol>());
                                    break;
                                case 1:
                                    WorldGen.PlaceTile(i, ballfart - 1, ModContent.TileType<LilyPadMedium>());
                                    break;
                            }
                        }
                    }
                }

                RemoveWaterFromRegion(ShipTiles.GetLength(1), ShipTiles.GetLength(0), new Vector2(boatPos, watercheck));

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
            Main.spawnTileY = depth - 22;

            EEMod.progressMessage = null;
        }
        internal override void PlayerUpdate(Player player)
        {
           
        }
    }
}