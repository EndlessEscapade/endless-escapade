using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using EEMod.Tiles;
using EEMod.Tiles.Walls;
using System.Collections.Generic;
using System;
using EEMod.Tiles.Furniture;
using EEMod.Tiles.Ores;
using static EEMod.EEWorld.EEWorld;
namespace EEMod
{
    public class EESubWorlds
    {
        public static IList<Vector2> ChainConnections = new List<Vector2>();
        public static IList<Vector2> OrbPositions = new List<Vector2>();
        public static Vector2 CoralBoatPos;
        public static void Pyramids(int seed, GenerationProgress customProgressObject = null)
        {
            Main.maxTilesX = 400;
            Main.maxTilesY = 400;
            Main.spawnTileX = 234;
            Main.spawnTileY = 92;
            SubworldManager.Reset(seed);
            SubworldManager.PostReset(customProgressObject);
            FillRegion(400, 400, new Vector2(0, 0), TileID.SandstoneBrick);
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
            FillWall(400, 400, new Vector2(0, 0), WallID.Waterfall);
            EEMod.isSaving = false;
        }

        public static void CoralReefs(int seed, GenerationProgress customProgressObject = null)
        {
            EEMod.progressMessage = "Generating CoralReefs";
            //Variables and Initialization stuff
            int depth = 70;
            int boatPos = Main.maxTilesX / 2;
            int roomCount = 8;
            Vector2[] roomsLeft = new Vector2[roomCount];
            Vector2[] roomsRight = new Vector2[roomCount];
            Main.maxTilesX = 1500;
            Main.maxTilesY = 2400;
            SubworldManager.Reset(seed);
            SubworldManager.PostReset(customProgressObject);

            try
            {
                //Placing initial blocks
                EEMod.progressMessage = "Generating Upper layer base";
                FillRegion(Main.maxTilesX, (Main.maxTilesY / 3), new Vector2(0, 0), ModContent.TileType<LightGemsandTile>());
                EEMod.progressMessage = "Generating Mid layer base";
                FillRegion(Main.maxTilesX, (Main.maxTilesY / 3), new Vector2(0, Main.maxTilesY / 3), ModContent.TileType<GemsandTile>());
                EEMod.progressMessage = "Generating Lower layer base";
                FillRegion(Main.maxTilesX, (Main.maxTilesY / 3), new Vector2(0, (Main.maxTilesY / 3) * 2), ModContent.TileType<DarkGemsandTile>());
                EEMod.progressMessage = "Clearing Upper Region";
                ClearRegion(Main.maxTilesX, (Main.maxTilesY / 10), Vector2.Zero);
                EEMod.progressMessage = "Generating Coral Sand";
                FillRegionNoEditWithNoise(Main.maxTilesX, (Main.maxTilesY / 20), new Vector2(0, Main.maxTilesY / 20), ModContent.TileType<CoralSand>());
                int maxTiles = (int)(Main.maxTilesX * Main.maxTilesY * 9E-04);
                EEMod.progressMessage = "Finding Suitable Chasm Positions";
                NoiseGenWave(new Vector2(300, 80), new Vector2(Main.maxTilesX - 300, Main.maxTilesY / 20), new Vector2(20, 100), (ushort)ModContent.TileType<CoralSand>(), 0.5f);
                NoiseGenWave(new Vector2(300, 60), new Vector2(Main.maxTilesX - 300, Main.maxTilesY / 20), new Vector2(50, 50), TileID.StoneSlab, 0.6f);


                //Making chasms
                for (int i = 0; i < roomsLeft.Length; i++)
                {
                    int sizeOfChasm = Main.rand.Next(100, 200);
                    if (i == 0)
                    {
                        roomsLeft[i] = new Vector2(200, 500);
                    }
                    else
                    {
                        int score;
                        int breakLoop = 0;
                        float randPosX;
                        float randPosY;
                        int distance = 400;
                        do
                        {
                            breakLoop++;
                            score = 0;
                            randPosX = Main.rand.Next((int)roomsLeft[i - 1].X - distance, (int)roomsLeft[i - 1].X + distance);
                            randPosY = Main.rand.Next((int)roomsLeft[i - 1].Y - distance, (int)roomsLeft[i - 1].Y + distance);
                            float f = sizeOfChasm * 1.6f;
                            float ff = f * f;
                            for (int k = 0; k < i; k++)
                            {
                                if (Vector2.DistanceSquared(new Vector2(randPosX, randPosY), roomsLeft[k]) > ff)
                                {
                                    score++;
                                }
                            }
                            if (breakLoop > 2000)
                            {
                                break;
                            }
                        } while (score != i || randPosX < sizeOfChasm * 1.2f || randPosY < sizeOfChasm || randPosX > Main.maxTilesX / 2 - 50 || randPosY > Main.maxTilesY * 0.66f || Vector2.DistanceSquared(new Vector2(randPosX, randPosY), new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2)) < 220 * 220
                         || Vector2.DistanceSquared(new Vector2(randPosX, randPosY), new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 - 400)) < 220 * 220);
                        roomsLeft[i] = new Vector2(randPosX, randPosY);
                    }
                    MakeCoralRoom((int)roomsLeft[i].X, (int)roomsLeft[i].Y, sizeOfChasm, Main.rand.Next(0, 7), Main.rand.Next(0, 3));
                    if (i != 0)
                    {
                        MakeWavyChasm3(roomsLeft[i], roomsLeft[i - 1], TileID.StoneSlab, 100, Main.rand.Next(10, 20), true, new Vector2(20, 40), 0, 5, true, 51, Main.rand.Next(80, 120));
                    }
                }

                for (int i = 0; i < roomsRight.Length; i++)
                {
                    int sizeOfChasm = Main.rand.Next(100, 200);
                    if (i == 0)
                    {
                        roomsRight[i] = new Vector2(1000, 500);
                    }
                    else
                    {
                        int score;
                        int breakLoop = 0;
                        float randPosX;
                        float randPosY;
                        int distance = 400;
                        do
                        {
                            breakLoop++;
                            score = 0;
                            randPosX = Main.rand.Next((int)roomsRight[i - 1].X - distance, (int)roomsRight[i - 1].X + distance);
                            randPosY = Main.rand.Next((int)roomsRight[i - 1].Y - distance, (int)roomsRight[i - 1].Y + distance);
                            float f = sizeOfChasm * 1.6f;
                            float ff = f * f;
                            for (int k = 0; k < i; k++)
                            {
                                if (Vector2.DistanceSquared(new Vector2(randPosX, randPosY), roomsRight[k]) > ff)
                                {
                                    score++;
                                }
                            }
                            if (breakLoop > 2000)
                            {
                                break;
                            }
                        } while (score != i || randPosX > Main.maxTilesX - (sizeOfChasm * 1.2f)
                        || randPosY < (sizeOfChasm * 1) || randPosX < Main.maxTilesX / 2 + 50
                        || randPosY > Main.maxTilesY * 0.66f
                        || Vector2.DistanceSquared(new Vector2(randPosX, randPosY), new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2)) < 220 * 220
                         || Vector2.DistanceSquared(new Vector2(randPosX, randPosY), new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 - 400)) < 220 * 220);
                        roomsRight[i] = new Vector2(randPosX, randPosY);
                    }
                    MakeCoralRoom((int)roomsRight[i].X, (int)roomsRight[i].Y, sizeOfChasm, WorldGen.genRand.Next(0, 7), WorldGen.genRand.Next(0, 3));
                    if (i != 0)
                    {
                        MakeWavyChasm3(roomsRight[i], roomsRight[i - 1], TileID.StoneSlab, 100, 10, true, new Vector2(20, 40), 0, 5, true, 51, Main.rand.Next(80, 120));
                    }
                }
                EEMod.progressMessage = "Genning Rooms";
                MakeCoralRoom(Main.maxTilesX / 2, Main.maxTilesY / 2, 400, WorldGen.genRand.Next(0, 3), WorldGen.genRand.Next(0, 3), true);
                MakeCoralRoom(Main.maxTilesX / 2, Main.maxTilesY / 2 + 400, 400, -1, WorldGen.genRand.Next(0, 3), false);
                Vector2[] chosen = { Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero };
                for (int i = 0; i < roomsLeft.Length; i++)
                {
                    if (chosen[0] == Vector2.Zero || Vector2.DistanceSquared(roomsLeft[i], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2)) <
                        Vector2.DistanceSquared(chosen[0], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2)))
                    {
                        chosen[0] = roomsLeft[i];
                    }
                    if (chosen[1] == Vector2.Zero || Vector2.DistanceSquared(roomsRight[i], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2)) <
                        Vector2.DistanceSquared(chosen[1], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2)))
                    {
                        chosen[1] = roomsRight[i];
                    }
                    if (chosen[2] == Vector2.Zero || Vector2.DistanceSquared(roomsLeft[i], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 - 400)) <
                        Vector2.DistanceSquared(chosen[2], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 - 400)))
                    {
                        chosen[2] = roomsLeft[i];
                    }
                    if (chosen[3] == Vector2.Zero || Vector2.DistanceSquared(roomsRight[i], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 - 400)) <
                        Vector2.DistanceSquared(chosen[3], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 - 400)))
                    {
                        chosen[3] = roomsRight[i];
                    }
                    if (chosen[4] == Vector2.Zero || Vector2.DistanceSquared(roomsLeft[i], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 + 400)) <
                       Vector2.DistanceSquared(chosen[4], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 + 400)))
                    {
                        chosen[4] = roomsLeft[i];
                    }
                    if (chosen[5] == Vector2.Zero || Vector2.DistanceSquared(roomsRight[i], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 + 400)) <
                        Vector2.DistanceSquared(chosen[5], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 + 400)))
                    {
                        chosen[5] = roomsRight[i];
                    }
                }
                EEMod.progressMessage = "Making Wavy Chasms";
                for (int i = 0; i < 2; i++)
                    MakeWavyChasm3(chosen[i], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2), TileID.StoneSlab, 100, 10, true, new Vector2(20, 40));
                for (int i = 2; i < 4; i++)
                    MakeWavyChasm3(chosen[i], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 - 400), TileID.StoneSlab, 100, 10, true, new Vector2(20, 40));
                for (int i = 4; i < 6; i++)
                    MakeWavyChasm3(chosen[i], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 + 400), TileID.StoneSlab, 100, 10, true, new Vector2(20, 40));

                if (Main.rand.NextBool())
                {
                    Vector2 highestRoom = new Vector2(0, 3000);
                    foreach (Vector2 legoYoda in roomsLeft)
                        if (legoYoda.Y < highestRoom.Y)
                            highestRoom = legoYoda;
                    MakeWavyChasm3(highestRoom, new Vector2(highestRoom.X + Main.rand.Next(-100, 101), 100), TileID.StoneSlab, 100, 10, true, new Vector2(20, 40));
                }
                else
                {
                    Vector2 highestRoom = new Vector2(0, 3000);
                    foreach (Vector2 legoYoda in roomsRight)
                        if (legoYoda.Y < highestRoom.Y)
                            highestRoom = legoYoda;
                    MakeWavyChasm3(highestRoom, new Vector2(highestRoom.X + Main.rand.Next(-100, 101), 100), TileID.StoneSlab, 100, 10, true, new Vector2(20, 40));
                }


                RemoveStoneSlabs();

                MakeLayer(Main.maxTilesX / 2, Main.maxTilesY / 2 - 400, 100, 1, ModContent.TileType<GemsandTile>());

                EEMod.progressMessage = "Generating Ores";
                //Generating ores
                int barrier = 800;
                for (int j = 0; j < barrier; j++)
                {
                    for (int i = 0; i < Main.maxTilesX; i++)
                    {
                        Tile tile = Main.tile[i, j];
                        if (tile.type == ModContent.TileType<DarkGemsandTile>() || tile.type == ModContent.TileType<GemsandTile>() || tile.type == ModContent.TileType<LightGemsandTile>())
                        {
                            if (Main.rand.NextBool(2000))
                            {
                                WorldGen.TileRunner(i, j, WorldGen.genRand.Next(4, 8), WorldGen.genRand.Next(5, 7), ModContent.TileType<LythenOreTile>());
                            }
                        }
                    }
                }
                for (int j = barrier; j < Main.maxTilesY; j++)
                {
                    for (int i = 0; i < Main.maxTilesX; i++)
                    {
                        Tile tile = Main.tile[i, j];
                        if (tile.type == ModContent.TileType<GemsandTile>() || tile.type == ModContent.TileType<DarkGemsandTile>() || tile.type == ModContent.TileType<LightGemsandTile>())
                            if (Main.rand.NextBool(2000))
                                WorldGen.TileRunner(i, j, WorldGen.genRand.Next(4, 8), WorldGen.genRand.Next(5, 7), ModContent.TileType<HydriteOreTile>());
                    }
                }

                EEMod.progressMessage = "Generating Ruins";
                int mlem = 0;
                while(mlem < 5)
                {
                    int tileX = Main.rand.Next(100, 1400);
                    int tileY = Main.rand.Next(200, 700);
                    if (!Main.tile[tileX, tileY].active())
                    {
                        if(Main.rand.NextBool())
                            PlaceAnyBuilding(tileX, tileY, ReefRuins1);
                        else
                            PlaceAnyBuilding(tileX, tileY, ReefRuins2);
                        mlem++;
                    }
                }

                //Placing water and etc
                KillWall(1000, 1000, Vector2.Zero);
                FillRegionWithWater(Main.maxTilesX, Main.maxTilesY - depth, new Vector2(0, depth));

                //Lower reefs stuffs

                /*MakeKramkenArena(670, 1600, 190);
                MakeAtlantis(new Vector2(0,1900), new Vector2(900, 500));*/

                //Final polishing
                PlaceCoral();
                for (int i = 2; i < Main.maxTilesX - 2; i++)
                {
                    for (int j = 2; j < Main.maxTilesY - 2; j++)
                    {
                        if (WorldGen.genRand.NextBool(2))
                        {
                            Tile.SmoothSlope(i, j);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                EEMod.progressMessage = e.ToString();
                SubworldManager.PreSaveAndQuit();
            }
            
            //Placing boat
            PlaceShipWalls(boatPos, TileCheckWater(boatPos) - 22, ShipWalls);
            PlaceShip(boatPos, TileCheckWater(boatPos) - 22, ShipTiles);
            CoralBoatPos = new Vector2(boatPos, TileCheckWater(boatPos) - 22);
            EEMod.progressMessage = "Successful!";
            for (int j = 42; j < Main.maxTilesY - 42; j++)
            {
                for (int i = 42; i < Main.maxTilesX - 42; i++)
                {
                    int noOfTiles = 0;
                    if (j > 200)
                    {
                        for (int k = -10; k < 10; k++)
                        {
                            for (int l = -10; l < 10; l++)
                            {
                                if (Main.tile[i + k, j + l].active())
                                {
                                    noOfTiles++;
                                }
                            }
                        }
                    
                    if(noOfTiles < 2 && WorldGen.genRand.NextBool(5))
                    {
                        OrbPositions.Add(new Vector2(i, j));
                    }
                    }
                    if ((TileCheck2(i, j) == 3 || TileCheck2(i, j) == 4) && WorldGen.genRand.NextBool(4) && Main.tileSolid[Main.tile[i, j].type])
                    {
                        if (ChainConnections.Count == 0)
                        {
                            ChainConnections.Add(new Vector2(i, j));
                        }
                        else
                        {
                            Vector2 lastPos = ChainConnections[ChainConnections.Count - 1];
                            if (Vector2.DistanceSquared(lastPos, new Vector2(i, j)) > 5 * 5 && Vector2.DistanceSquared(lastPos, new Vector2(i, j)) < 35 * 35 || Vector2.DistanceSquared(lastPos, new Vector2(i, j)) > 500 * 500 && Math.Abs(lastPos.X - i) > 3)
                            {
                                ChainConnections.Add(new Vector2(i, j));
                            }
                        }
                    }
                }
            }
            //Finishing initialization stuff
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


            FillRegionWithWater(Main.maxTilesX, Main.maxTilesY, new Vector2(0, 0));
            RemoveWaterFromRegion(Main.maxTilesX, 170, new Vector2(0, 0));

            MakeOvalJaggedTop(Main.maxTilesX, 50, new Vector2(0, 165), ModContent.TileType<CoralSand>(), 15, 15);

            EEWorld.EEWorld.Island(800, 250, 140);

            FillRegion(Main.maxTilesX, Main.maxTilesY - 190, new Vector2(0, 190), ModContent.TileType<CoralSand>());


            for (int i = 42; i < Main.maxTilesX - 42; i++)
            {
                for (int j = 42; j < Main.maxTilesY - 42; j++)
                {

                    int yes = WorldGen.genRand.Next(0, 5);
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (TileCheck2(i, j) == 2 && yes < 3 && tile.type == ModContent.TileType<CoralSand>())
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

            PlaceAnyBuilding(100, 100, IceShrine);
            PlaceAnyBuilding(200, 100, FireShrine);
            PlaceAnyBuilding(300, 100, DesertShrine);
            PlaceAnyBuilding(400, 100, WaterShrine);
            PlaceAnyBuilding(500, 100, LeafShrine);
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


            FillRegionWithWater(Main.maxTilesX, Main.maxTilesY, new Vector2(0, 0));
            RemoveWaterFromRegion(Main.maxTilesX, 170, new Vector2(0, 0));

            MakeOvalJaggedTop(Main.maxTilesX, 50, new Vector2(0, 165), ModContent.TileType<CoralSand>(), 15, 15);

            EEWorld.EEWorld.Island(600, 250, 140);

            FillRegion(Main.maxTilesX, Main.maxTilesY - 190, new Vector2(0, 190), ModContent.TileType<CoralSand>());


            for (int i = 42; i < Main.maxTilesX - 42; i++)
            {
                for (int j = 42; j < Main.maxTilesY - 42; j++)
                {

                    int yes = WorldGen.genRand.Next(0, 5);
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (TileCheck2(i, j) == 2 && yes < 3 && tile.type == ModContent.TileType<CoralSand>())
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
            FillRegion(Main.maxTilesX, Main.maxTilesY, new Vector2(0, 0), ModContent.TileType<VolcanicAshTile>());
            MakeLayer(200, 100, 90, 1, ModContent.TileType<VolcanicAshTile>());
            MakeOvalFlatTop(40, 10, new Vector2(200 - 20, 100), ModContent.TileType<VolcanicAshTile>());
            MakeChasm(200, 140, 110, ModContent.TileType<GemsandTile>(), 0, 5, 0);
            for (int i = 0; i < 400; i++)
            {
                for (int j = 0; j < 400; j++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (tile.type == ModContent.TileType<GemsandTile>())
                        WorldGen.KillTile(i, j);
                }
            }
            KillWall(Main.maxTilesX, Main.maxTilesY, Vector2.Zero);
            FillWall(Main.maxTilesX, Main.maxTilesY, Vector2.Zero, ModContent.WallType<MagmastoneWallTile>());
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
            MakeTriangle(new Vector2(300, 895), 600, 1000, 3, ModContent.TileType<VolcanicAshTile>(), true, true, ModContent.WallType<VolcanicAshWallTile>());
            EEWorld.EEWorld.Island(800, 400, 290);
            FillRegion(Main.maxTilesX, Main.maxTilesY - 190, new Vector2(0, 400), ModContent.TileType<CoralSand>());

            ClearRegionSafely(60, 630, new Vector2(570, 170), ModContent.TileType<CoralSand>());
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

            FillRegion(Main.maxTilesX, Main.maxTilesY, Vector2.Zero, ModContent.TileType<MagmastoneTile>());
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    if (Main.rand.NextBool(3000))
                        MakeLavaPit(Main.rand.Next(20, 30), Main.rand.Next(7, 20), new Vector2(i, j), Main.rand.NextFloat(0.1f, 0.5f));
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
            FillWall(Main.maxTilesX, Main.maxTilesY, Vector2.Zero, ModContent.WallType<MagmastoneWallTile>());
        }
    }
}