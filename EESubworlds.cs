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

namespace EEMod
{
    public class EESubWorlds
    {
        
        public static Vector2 CoralBoatPos;
        public static void Pyramids(int seed, GenerationProgress customProgressObject = null)
        {
            Main.maxTilesX = 400;
            Main.maxTilesY = 400;
            Main.spawnTileX = 234;
            Main.spawnTileY = 92;
            SubworldManager.Reset(seed);
            SubworldManager.PostReset(customProgressObject);
            EEWorld.EEWorld.FillRegion(400, 400, new Vector2(0, 0), TileID.SandstoneBrick);
            EEWorld.EEWorld.Pyramid(63, 42);
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
            EEWorld.EEWorld.FillWall(400, 400, new Vector2(0, 0), WallID.Waterfall);
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
                EEWorld.EEWorld.FillRegion(Main.maxTilesX, (Main.maxTilesY / 3), new Vector2(0, 0), ModContent.TileType<LightGemsandTile>());
                EEMod.progressMessage = "Generating Mid layer base";
                EEWorld.EEWorld.FillRegion(Main.maxTilesX, (Main.maxTilesY / 3), new Vector2(0, Main.maxTilesY / 3), ModContent.TileType<GemsandTile>());
                EEMod.progressMessage = "Generating Lower layer base";
                EEWorld.EEWorld.FillRegion(Main.maxTilesX, (Main.maxTilesY / 3), new Vector2(0, (Main.maxTilesY / 3) * 2), ModContent.TileType<DarkGemsandTile>());
                EEMod.progressMessage = "Clearing Upper Region";
                EEWorld.EEWorld.ClearRegion(Main.maxTilesX, (Main.maxTilesY / 10), Vector2.Zero);
                for (int i = 0; i < 10; i++)
                    for (int j = -5; j < 5; j++)
                        WorldGen.TileRunner(300 + (i * 170) + (j * 10), Main.maxTilesY / 20, 10, 10, ModContent.TileType<GemsandTile>(), true, 0, 0, true, true);
                for (int i = 0; i < 100; i++)
                    for (int j = -5; j < 5; j++)
                        WorldGen.TileRunner(300 + (i * 17) + (j * 10), Main.maxTilesY / 20, 4, 10, ModContent.TileType<GemsandTile>(), true, 0, 0, true, true);
                EEMod.progressMessage = "Generating Coral Sand";
                EEWorld.EEWorld.FillRegionNoEdit(Main.maxTilesX, (Main.maxTilesY / 20), new Vector2(0, Main.maxTilesY / 20), ModContent.TileType<CoralSand>());
                int maxTiles = (int)(Main.maxTilesX * Main.maxTilesY * 9E-04);
                EEMod.progressMessage = "Finding Suitable Chasm Positions";
                
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
                        int distance = 430;
                        do
                        {
                            breakLoop++;
                            score = 0;
                            randPosX = Main.rand.Next((int)roomsLeft[i - 1].X - distance, (int)roomsLeft[i - 1].X + distance);
                            randPosY = Main.rand.Next((int)roomsLeft[i - 1].Y - distance, (int)roomsLeft[i - 1].Y + distance);
                            for (int k = 0; k < i; k++)
                            {
                                if (Vector2.Distance(new Vector2(randPosX, randPosY), roomsLeft[k]) > sizeOfChasm * 1.2f)
                                {
                                    score++;
                                }
                            }
                            if (breakLoop > 5000)
                            {
                                break;
                            }
                        } while (score != i || randPosX < sizeOfChasm * 1 || randPosY < sizeOfChasm * 1 || randPosX > Main.maxTilesX / 2 - 100 || randPosY > Main.maxTilesY * 0.66f);
                        roomsLeft[i] = new Vector2(randPosX, randPosY);
                    }
                    EEWorld.EEWorld.MakeCoralRoom((int)roomsLeft[i].X, (int)roomsLeft[i].Y, sizeOfChasm, Main.rand.Next(0, 3), Main.rand.Next(0, 3));
                    if (i != 0)
                    {
                        EEWorld.EEWorld.MakeWavyChasm3(roomsLeft[i], roomsLeft[i - 1], TileID.StoneSlab, 100, 10, true);
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
                        int distance = 430;
                        do
                        {
                            breakLoop++;
                            score = 0;
                            randPosX = Main.rand.Next((int)roomsRight[i - 1].X - distance, (int)roomsRight[i - 1].X + distance);
                            randPosY = Main.rand.Next((int)roomsRight[i - 1].Y - distance, (int)roomsRight[i - 1].Y + distance);
                            for (int k = 0; k < i; k++)
                            {
                                if (Vector2.Distance(new Vector2(randPosX, randPosY), roomsRight[k]) > sizeOfChasm * 1.2f)
                                {
                                    score++;
                                }
                            }
                            if (breakLoop > 5000)
                            {
                                break;
                            }
                        } while (score != i || randPosX > Main.maxTilesX - (sizeOfChasm * 1) || randPosY < (sizeOfChasm * 1) || randPosX < Main.maxTilesX / 2 + 100 || randPosY > Main.maxTilesY * 0.66f);
                        roomsRight[i] = new Vector2(randPosX, randPosY);
                    }
                    EEWorld.EEWorld.MakeCoralRoom((int)roomsRight[i].X, (int)roomsRight[i].Y, sizeOfChasm, WorldGen.genRand.Next(0, 3), WorldGen.genRand.Next(0, 3));
                    EEWorld.EEWorld.MakeCoralRoom((int)roomsRight[i].X, (int)roomsRight[i].Y, sizeOfChasm, WorldGen.genRand.Next(0, 3), WorldGen.genRand.Next(0, 3));
                    if (i != 0)
                    {
                        EEWorld.EEWorld.MakeWavyChasm3(roomsRight[i], roomsRight[i - 1], TileID.StoneSlab, 100, 10, true);
                    }
                }
                EEMod.progressMessage = "Genning Rooms";
                EEWorld.EEWorld.MakeCoralRoom(Main.maxTilesX / 2, Main.maxTilesY / 2, 400, WorldGen.genRand.Next(0, 3), WorldGen.genRand.Next(0, 3), true);
                Vector2[] chosen = { Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero };
                for (int i = 0; i < roomsLeft.Length; i++)
                {
                    if (chosen[0] == Vector2.Zero || Vector2.Distance(roomsLeft[i], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2)) <
                        Vector2.Distance(chosen[0], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2)))
                    {
                        chosen[0] = roomsLeft[i];
                    }
                    if (chosen[1] == Vector2.Zero || Vector2.Distance(roomsRight[i], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2)) <
                        Vector2.Distance(chosen[1], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2)))
                    {
                        chosen[1] = roomsRight[i];
                    }
                    if (chosen[2] == Vector2.Zero || Vector2.Distance(roomsLeft[i], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 - 400)) <
                        Vector2.Distance(chosen[2], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 - 400)))
                    {
                        chosen[2] = roomsLeft[i];
                    }
                    if (chosen[3] == Vector2.Zero || Vector2.Distance(roomsRight[i], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 - 400)) <
                        Vector2.Distance(chosen[3], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 - 400)))
                    {
                        chosen[3] = roomsRight[i];
                    }
                }
                EEMod.progressMessage = "Making Wavy Chasms";
                for (int i = 0; i < 2; i++)
                    EEWorld.EEWorld.MakeWavyChasm3(chosen[i], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2), TileID.StoneSlab, 100, 10, true);
                for (int i = 2; i < 4; i++)
                    EEWorld.EEWorld.MakeWavyChasm3(chosen[i], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 - 400), TileID.StoneSlab, 100, 10, true);

                if (Main.rand.NextBool())
                {
                    Vector2 highestRoom = new Vector2(0, 3000);
                    foreach(Vector2 legoYoda in roomsLeft)
                        if(legoYoda.Y < highestRoom.Y)
                            highestRoom = legoYoda;
                    EEWorld.EEWorld.MakeWavyChasm3(highestRoom, new Vector2(highestRoom.X + Main.rand.Next(-100, 101), 100), TileID.StoneSlab, 100, 10, true);
                }
                else
                {
                    Vector2 highestRoom = new Vector2(0, 3000);
                    foreach (Vector2 legoYoda in roomsRight)
                        if (legoYoda.Y < highestRoom.Y)
                            highestRoom = legoYoda;
                    EEWorld.EEWorld.MakeWavyChasm3(highestRoom, new Vector2(highestRoom.X + Main.rand.Next(-100, 101), 100), TileID.StoneSlab, 100, 10, true);
                }


                EEWorld.EEWorld.RemoveStoneSlabs();

                EEWorld.EEWorld.MakeLayer(Main.maxTilesX / 2, Main.maxTilesY / 2 - 400, 100, 1, ModContent.TileType<GemsandTile>());
                
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
                
                //Placing water and etc
                EEWorld.EEWorld.KillWall(1000, 1000, Vector2.Zero);
                EEWorld.EEWorld.FillRegionWithWater(Main.maxTilesX, Main.maxTilesY - depth, new Vector2(0, depth));

                //Lower reefs stuffs

                /*EEWorld.EEWorld.MakeKramkenArena(670, 1600, 190);
                EEWorld.EEWorld.MakeAtlantis(new Vector2(0,1900), new Vector2(900, 500));*/

                //Final polishing
                EEWorld.EEWorld.PlaceCoral();
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
            catch(Exception e)
            {
                EEMod.progressMessage= e.ToString();
            }
            //Placing boat
            EEWorld.EEWorld.PlaceShip(boatPos, EEWorld.EEWorld.TileCheckWater(boatPos) - 22, EEWorld.EEWorld.ShipTiles);
            EEWorld.EEWorld.PlaceShipWalls(boatPos, EEWorld.EEWorld.TileCheckWater(boatPos) - 27, EEWorld.EEWorld.ShipWalls);
            CoralBoatPos = new Vector2(boatPos, EEWorld.EEWorld.TileCheckWater(boatPos) - 22);
            EEMod.progressMessage = "Successful!";
            WorldGen.UpdateWorld();
            //Finishing initialization stuff
            EEMod.isSaving = false;
            Main.spawnTileX = boatPos;
            Main.spawnTileY = EEWorld.EEWorld.TileCheckWater(boatPos) - 22;
            EEMod.progressMessage = null;
        }

        public static void Island(int seed, GenerationProgress customProgressObject = null)
        {
            Main.maxTilesX = 1000;
            Main.maxTilesY = 500;
            SubworldManager.Reset(seed);
            SubworldManager.PostReset(customProgressObject);


            EEWorld.EEWorld.FillRegionWithWater(Main.maxTilesX, Main.maxTilesY, new Vector2(0, 0));
            EEWorld.EEWorld.RemoveWaterFromRegion(Main.maxTilesX, 170, new Vector2(0, 0));

            EEWorld.EEWorld.MakeOvalJaggedTop(Main.maxTilesX, 50, new Vector2(0, 165), ModContent.TileType<CoralSand>(), 15, 15);

            EEWorld.EEWorld.Island(800, 250, 140);

            EEWorld.EEWorld.FillRegion(Main.maxTilesX, Main.maxTilesY - 190, new Vector2(0, 190), ModContent.TileType<CoralSand>());


            for (int i = 42; i < Main.maxTilesX - 42; i++)
            {
                for (int j = 42; j < Main.maxTilesY - 42; j++)
                {
                   
                    int yes = WorldGen.genRand.Next(0, 5);
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (EEWorld.EEWorld.TileCheck2(i, j) == 2 && yes < 3 && tile.type == ModContent.TileType<CoralSand>())
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
                    if (EEWorld.EEWorld.TileCheck2(i, j) == 2 && yes == 0 && tile.type == TileID.Grass)
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

            EEWorld.EEWorld.PlaceShip(50, 150, EEWorld.EEWorld.ShipTiles);
            EEWorld.EEWorld.PlaceShipWalls(50, 150, EEWorld.EEWorld.ShipWalls);

            WorldGen.AddTrees();

            EEWorld.EEWorld.PlaceAnyBuilding(100, 100, EEWorld.EEWorld.IceShrine);
            EEWorld.EEWorld.PlaceAnyBuilding(200, 100, EEWorld.EEWorld.FireShrine);
            EEWorld.EEWorld.PlaceAnyBuilding(300, 100, EEWorld.EEWorld.DesertShrine);
            EEWorld.EEWorld.PlaceAnyBuilding(400, 100, EEWorld.EEWorld.WaterShrine);
            EEWorld.EEWorld.PlaceAnyBuilding(500, 100, EEWorld.EEWorld.LeafShrine);
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


            EEWorld.EEWorld.FillRegionWithWater(Main.maxTilesX, Main.maxTilesY, new Vector2(0, 0));
            EEWorld.EEWorld.RemoveWaterFromRegion(Main.maxTilesX, 170, new Vector2(0, 0));

            EEWorld.EEWorld.MakeOvalJaggedTop(Main.maxTilesX, 50, new Vector2(0, 165), ModContent.TileType<CoralSand>(), 15, 15);

            EEWorld.EEWorld.Island(600, 250, 140);

            EEWorld.EEWorld.FillRegion(Main.maxTilesX, Main.maxTilesY - 190, new Vector2(0, 190), ModContent.TileType<CoralSand>());


            for (int i = 42; i < Main.maxTilesX - 42; i++)
            {
                for (int j = 42; j < Main.maxTilesY - 42; j++)
                {

                    int yes = WorldGen.genRand.Next(0, 5);
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (EEWorld.EEWorld.TileCheck2(i, j) == 2 && yes < 3 && tile.type == ModContent.TileType<CoralSand>())
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
                    if (EEWorld.EEWorld.TileCheck2(i, j) == 2 && yes == 0 && tile.type == TileID.Grass)
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

            EEWorld.EEWorld.PlaceShip(50, 150, EEWorld.EEWorld.ShipTiles);
            EEWorld.EEWorld.PlaceShipWalls(50, 150, EEWorld.EEWorld.ShipWalls);

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
            EEWorld.EEWorld.FillRegion(Main.maxTilesX, Main.maxTilesY, new Vector2(0, 0), ModContent.TileType<VolcanicAshTile>());
            EEWorld.EEWorld.MakeLayer(200, 100, 90, 1, ModContent.TileType<VolcanicAshTile>());
            EEWorld.EEWorld.MakeOvalFlatTop(40, 10, new Vector2(200 - 20, 100), ModContent.TileType<VolcanicAshTile>());
            EEWorld.EEWorld.MakeChasm(200, 140, 110, ModContent.TileType<GemsandTile>(), 0, 5, 0);
            for (int i = 0; i < 400; i++)
            {
                for (int j = 0; j < 400; j++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (tile.type == ModContent.TileType<GemsandTile>())
                        WorldGen.KillTile(i, j);
                }
            }
            EEWorld.EEWorld.KillWall(Main.maxTilesX, Main.maxTilesY, Vector2.Zero);
            EEWorld.EEWorld.FillWall(Main.maxTilesX, Main.maxTilesY, Vector2.Zero, ModContent.WallType<MagmastoneWallTile>());
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


            EEWorld.EEWorld.FillRegionWithWater(Main.maxTilesX, Main.maxTilesY, Vector2.Zero);
            EEWorld.EEWorld.RemoveWaterFromRegion(Main.maxTilesX, 360, Vector2.Zero);

            EEWorld.EEWorld.RemoveWaterFromRegion(60, 630, new Vector2(570, 170));
            EEWorld.EEWorld.KillWall(Main.maxTilesX, Main.maxTilesY, Vector2.Zero);
            EEWorld.EEWorld.MakeTriangle(new Vector2(300, 895), 600, 1000, 3, ModContent.TileType<VolcanicAshTile>(), true, true, ModContent.WallType<VolcanicAshWallTile>());
            EEWorld.EEWorld.Island(800, 400, 290);
            EEWorld.EEWorld.FillRegion(Main.maxTilesX, Main.maxTilesY - 190, new Vector2(0, 400), ModContent.TileType<CoralSand>());

            EEWorld.EEWorld.ClearRegionSafely(60, 630, new Vector2(570, 170), ModContent.TileType<CoralSand>());
            EEWorld.EEWorld.ClearRegionSafely(60, 630, new Vector2(570, 170), TileID.Dirt);
            EEWorld.EEWorld.ClearRegionSafely(60, 630, new Vector2(570, 170), TileID.Grass);
            EEWorld.EEWorld.FillRegionWithLava(40, 206, new Vector2(580, 594));
            EEWorld.EEWorld.MakeVolcanoEntrance(598, 596, EEWorld.EEWorld.VolcanoEntrance);

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

            EEWorld.EEWorld.FillRegion(Main.maxTilesX, Main.maxTilesY, Vector2.Zero, ModContent.TileType<MagmastoneTile>());
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    if (Main.rand.NextBool(3000))
                        EEWorld.EEWorld.MakeLavaPit(Main.rand.Next(20, 30), Main.rand.Next(7, 20), new Vector2(i, j), Main.rand.NextFloat(0.1f, 0.5f));
                }
            }
            EEWorld.EEWorld.MakeChasm(200, 10, 100, TileID.StoneSlab, 0, 10, 20);
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
            EEWorld.EEWorld.MakeOvalJaggedTop(80, 60, new Vector2(160, 170), ModContent.TileType<MagmastoneTile>());
            EEWorld.EEWorld.KillWall(Main.maxTilesX, Main.maxTilesY, Vector2.Zero);
            EEWorld.EEWorld.FillWall(Main.maxTilesX, Main.maxTilesY, Vector2.Zero, ModContent.WallType<MagmastoneWallTile>());
        }
    }
}
