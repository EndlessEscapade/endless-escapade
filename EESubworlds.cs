using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using EEMod.Tiles;
using EEMod.Tiles.Walls;
using System.Collections.Generic;
using System;

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
            int depth = 70;
            int boatPos = Main.maxTilesX / 2;
            Main.maxTilesX = 1000;
            Main.maxTilesY = 2000;
            SubworldManager.Reset(seed);
            SubworldManager.PostReset(customProgressObject);
            EEWorld.EEWorld.FillRegion(Main.maxTilesX, Main.maxTilesY, new Vector2(0, 0), ModContent.TileType<HardenedGemsandTile>());
            EEWorld.EEWorld.ClearRegion(Main.maxTilesX, Main.maxTilesY / 20, Vector2.Zero);
            for (int i = 0; i < 10; i++)
            {
                for (int j = -5; j < 5; j++)
                    WorldGen.TileRunner(300 + (i * 170) + (j * 10), Main.maxTilesY / 20, 10, 10, ModContent.TileType<HardenedGemsandTile>(), true, 0, 0, true, true);
            }
            for (int i = 0; i < 100; i++)
            {
                for (int j = -5; j < 5; j++)
                    WorldGen.TileRunner(300 + (i * 17) + (j * 10), Main.maxTilesY / 20, 4, 10, ModContent.TileType<HardenedGemsandTile>(), true, 0, 0, true, true);
            }
            EEWorld.EEWorld.FillRegionNoEdit(Main.maxTilesX, Main.maxTilesY / 20, new Vector2(0, Main.maxTilesY / 40), ModContent.TileType<CoralSand>());
            EEWorld.EEWorld.CoralReef();
            for (int i = 2; i < Main.maxTilesX - 2; i++)
            {
                for (int j = 2; j < Main.maxTilesY - 2; j++)
                {
                    Tile.SmoothSlope(i, j);
                }
            }
            EEWorld.EEWorld.KillWall(1000, 1000, Vector2.Zero);
            EEWorld.EEWorld.FillRegionWithWater(Main.maxTilesX, Main.maxTilesY - depth, new Vector2(0, depth));
            EEWorld.EEWorld.PlaceShip(boatPos, EEWorld.EEWorld.TileCheckWater(boatPos) - 22, EEWorld.EEWorld.ShipTiles);
            EEWorld.EEWorld.PlaceShipWalls(boatPos, EEWorld.EEWorld.TileCheckWater(boatPos) - 22, EEWorld.EEWorld.ShipWalls);
            CoralBoatPos = new Vector2(boatPos, EEWorld.EEWorld.TileCheckWater(boatPos) - 22);
            EEMod.isSaving = false;
            Main.spawnTileX = boatPos;
            Main.spawnTileY = EEWorld.EEWorld.TileCheckWater(boatPos) - 22;
        }
        public static void Island(int seed, GenerationProgress customProgressObject = null)
        {
            Main.maxTilesX = 900;
            Main.maxTilesY = 500;
            SubworldManager.Reset(seed);
            SubworldManager.PostReset(customProgressObject);
            List<Vector2> islandPositions = new List<Vector2>();
            /*
            //Not the island
            EEWorld.EEWorld.FillRegionWithWater(Main.maxTilesX, Main.maxTilesY, new Vector2(0, 0));
            EEWorld.EEWorld.RemoveWaterFromRegion(Main.maxTilesX, 180, new Vector2(0, 0));
            EEWorld.EEWorld.MakeOvalJaggedTop(Main.maxTilesX, Main.maxTilesY - 300, new Vector2(0, 300), ModContent.TileType<GemsandTile>(), 15, 15);

            //The island
            EEWorld.EEWorld.Island(274, 120);

            for (int i = 42; i < Main.maxTilesX-42; i++)
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
            EEWorld.EEWorld.PlaceShip(50, 158, EEWorld.EEWorld.ShipTiles);
            EEWorld.EEWorld.PlaceShipWalls(50, 158, EEWorld.EEWorld.ShipWalls);

            WorldGen.AddTrees();
            */
            int sizeX = 120;
            int sizeY = 60;
            int yPos = 80;
            int numberOfBuildingsInMidClass = 7;
            int numberOfBuildingsInHighClass = 6;
            int numberOfFillers = 6;
            int number;
            List<int> listNumbers = new List<int>();
            List<int> listNumbersHighClass = new List<int>();
            List<int> listNumbersFillers = new List<int>();
            List<Vector2> fillers = new List<Vector2>();
            for (int i = 0; i < numberOfBuildingsInMidClass; i++)
            {
                do
                {
                    number = Main.rand.Next(0, numberOfBuildingsInMidClass);
                } while (listNumbers.Contains(number));
                listNumbers.Add(number);
            }
            for (int i = 0; i < numberOfFillers; i++)
            {
                do
                {
                    number = Main.rand.Next(0, numberOfFillers);
                } while (listNumbersFillers.Contains(number));
                listNumbersFillers.Add(number);
            }
            for (int i = 0; i < numberOfBuildingsInHighClass; i++)
            {
                do
                {
                    number = Main.rand.Next(0, numberOfBuildingsInHighClass);
                } while (listNumbersHighClass.Contains(number));
                listNumbersHighClass.Add(number);
            }
            for (int i = 0; i < numberOfBuildingsInMidClass; i++)
            {
                float randomPosMiddleClass = Main.maxTilesX / 2 - sizeX + (i * ((sizeX * 2) / (numberOfBuildingsInMidClass - 1)));
                float whereTheYShouldBe = yPos + sizeY - (float)(Math.Pow(randomPosMiddleClass - (Main.maxTilesX / 2), 2) / (Math.Pow(sizeX, 2) / (float)sizeY));
                Vector2 actualPlace = new Vector2(randomPosMiddleClass, whereTheYShouldBe);
                islandPositions.Add(actualPlace);
            }
            float displacement = 220;
            float startingHeightOfUpperClass = sizeY + yPos + 10;
            for (int j = 0; j < islandPositions.Count; j++)
            {
                //EEWorld.EEWorld.MakeOvalFlatTop(40, 13, new Vector2(islandPositions[j].X - 15, islandPositions[j].Y), ModContent.TileType<HardenedGemsandTile>());
                switch(listNumbers[j])
                {
                    case 0:
                        {
                            EEWorld.EEWorld.PlaceM1((int)islandPositions[j].X - EEWorld.EEWorld.M1.GetLength(0)/2, (int)islandPositions[j].Y - EEWorld.EEWorld.M1.GetLength(1)/2, EEWorld.EEWorld.M1);
                            break;
                        }
                    case 1:
                        {
                            EEWorld.EEWorld.PlaceM2((int)islandPositions[j].X - EEWorld.EEWorld.M2.GetLength(0) / 2, (int)islandPositions[j].Y - EEWorld.EEWorld.M2.GetLength(1)/2, EEWorld.EEWorld.M2);
                            break;
                        }
                    case 2:
                        {
                            EEWorld.EEWorld.PlaceM3((int)islandPositions[j].X - EEWorld.EEWorld.M3.GetLength(0) / 2, (int)islandPositions[j].Y - EEWorld.EEWorld.M3.GetLength(1) / 2, EEWorld.EEWorld.M3);
                            break;
                        }
                    case 3:
                        {
                            EEWorld.EEWorld.PlaceBlackSmith((int)islandPositions[j].X - EEWorld.EEWorld.Blacksmith.GetLength(0) / 2, (int)islandPositions[j].Y - EEWorld.EEWorld.Blacksmith.GetLength(1) / 2, EEWorld.EEWorld.Blacksmith);
                            break;
                        }
                    case 4:
                        {
                            EEWorld.EEWorld.PlaceM4Temple((int)islandPositions[j].X - EEWorld.EEWorld.M4Temple.GetLength(0) / 2, (int)islandPositions[j].Y - EEWorld.EEWorld.M4Temple.GetLength(1) / 2, EEWorld.EEWorld.M4Temple);
                            break;
                        }
                    case 5:
                        {
                            EEWorld.EEWorld.PlaceBrewery((int)islandPositions[j].X - EEWorld.EEWorld.Brewery.GetLength(0) / 2, (int)islandPositions[j].Y - EEWorld.EEWorld.Brewery.GetLength(1) / 2, EEWorld.EEWorld.Brewery);
                            break;
                        }
                    case 6:
                        {
                            EEWorld.EEWorld.PlaceHeadQ((int)islandPositions[j].X - EEWorld.EEWorld.HeadQ.GetLength(0) / 2, (int)islandPositions[j].Y - EEWorld.EEWorld.HeadQ.GetLength(1) / 2, EEWorld.EEWorld.HeadQ);
                            break;
                        }
                }
            }
            fillers.Add(new Vector2(Main.maxTilesX / 2 - (int)displacement + 90, (int)startingHeightOfUpperClass + 40));
            fillers.Add(new Vector2(Main.maxTilesX / 2 + (int)displacement - 90, (int)startingHeightOfUpperClass + 20));
            fillers.Add(new Vector2(Main.maxTilesX / 2 + (int)displacement + 50, (int)startingHeightOfUpperClass + 50 + 130));
            fillers.Add(new Vector2(Main.maxTilesX / 2 - (int)displacement + 20, (int)startingHeightOfUpperClass + 40 + 130));
            for (int j = 0; j < 3; j++)
            {
                switch (listNumbersHighClass[j])
                {
                    case 0:
                        {
                            EEWorld.EEWorld.PlaceH2(Main.maxTilesX / 2 - (int)displacement - EEWorld.EEWorld.H2.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 - EEWorld.EEWorld.H2.GetLength(1) / 2, EEWorld.EEWorld.H2);
                            break;
                        }
                    case 1:
                        {
                            EEWorld.EEWorld.PlaceLootRoom(Main.maxTilesX / 2 - (int)displacement - EEWorld.EEWorld.LootRoom.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 - EEWorld.EEWorld.LootRoom.GetLength(1) / 2, EEWorld.EEWorld.LootRoom);
                            break;
                        }
                    case 2:
                        {
                            EEWorld.EEWorld.PlaceH1(Main.maxTilesX / 2 - (int)displacement - EEWorld.EEWorld.H1.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 - EEWorld.EEWorld.H1.GetLength(1) / 2, EEWorld.EEWorld.H1);
                            break;
                        }
                    case 3:
                        {
                            EEWorld.EEWorld.MidTemp2(Main.maxTilesX / 2 - (int)displacement - EEWorld.EEWorld.WorshipPlaceAtlantis.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 - EEWorld.EEWorld.WorshipPlaceAtlantis.GetLength(1) / 2, EEWorld.EEWorld.WorshipPlaceAtlantis);
                            break;
                        }
                    case 4:
                        {
                            EEWorld.EEWorld.PlaceFountain(Main.maxTilesX / 2 - (int)displacement - EEWorld.EEWorld.Fountain.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 - EEWorld.EEWorld.Fountain.GetLength(1) / 2, EEWorld.EEWorld.Fountain);
                            break;
                        }
                    case 5:
                        {
                            EEWorld.EEWorld.PlaceH2(Main.maxTilesX / 2 - (int)displacement - EEWorld.EEWorld.H2.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 - EEWorld.EEWorld.H2.GetLength(1) / 2, EEWorld.EEWorld.H2);
                            break;
                        }
                }
            }
            for (int j = 0; j < 3; j++)
            {
                switch (listNumbersHighClass[j +3])
                {
                    case 0:
                        {
                            EEWorld.EEWorld.PlaceH2(Main.maxTilesX / 2 + (int)displacement - EEWorld.EEWorld.H2.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 - EEWorld.EEWorld.H2.GetLength(1) / 2, EEWorld.EEWorld.H2);
                            break;
                        }
                    case 1:
                        {
                            EEWorld.EEWorld.PlaceLootRoom(Main.maxTilesX / 2 + (int)displacement - EEWorld.EEWorld.LootRoom.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 - EEWorld.EEWorld.LootRoom.GetLength(1) / 2, EEWorld.EEWorld.LootRoom);
                            break;
                        }
                    case 2:
                        {
                            EEWorld.EEWorld.PlaceH1(Main.maxTilesX / 2 + (int)displacement - EEWorld.EEWorld.H1.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 - EEWorld.EEWorld.H1.GetLength(1) / 2, EEWorld.EEWorld.H1);
                            break;
                        }
                    case 3:
                        {
                            EEWorld.EEWorld.MidTemp2(Main.maxTilesX / 2 + (int)displacement - EEWorld.EEWorld.WorshipPlaceAtlantis.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 - EEWorld.EEWorld.WorshipPlaceAtlantis.GetLength(1) / 2, EEWorld.EEWorld.WorshipPlaceAtlantis);
                            break;
                        }
                    case 4:
                        {
                            EEWorld.EEWorld.PlaceFountain(Main.maxTilesX / 2 + (int)displacement - EEWorld.EEWorld.Fountain.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 - EEWorld.EEWorld.Fountain.GetLength(1) / 2, EEWorld.EEWorld.Fountain);
                            break;
                        }
                    case 5:
                        {
                            EEWorld.EEWorld.PlaceH2(Main.maxTilesX / 2 + (int)displacement - EEWorld.EEWorld.H2.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 - EEWorld.EEWorld.H2.GetLength(1) / 2, EEWorld.EEWorld.H2);
                            break;
                        }
                }
            }
            int distanceFromEdge = 100;
            for (int j = 0; j < 2; j++)
            { 
                for (int i = 2; i > 0; i--)
                {
                    if((j == 0 && i == 2) || (j == 1 && i == 1))
                    EEWorld.EEWorld.MakeOvalFlatTop(44, 12, new Vector2(distanceFromEdge + (j * 50), distanceFromEdge + (i * 40) - 50), ModContent.TileType<GemsandstoneTile>());
                }
            }
            EEWorld.EEWorld.MakeChasm(distanceFromEdge - 20, distanceFromEdge + 60, 170, ModContent.TileType<GemsandstoneTile>(), 0, 10, 10);
            EEWorld.EEWorld.MakeChasm(distanceFromEdge + 70, distanceFromEdge + 60, 170, ModContent.TileType<GemsandstoneTile>(), 0, 10, 10);
            EEWorld.EEWorld.MakeOvalJaggedTop(25, 40, new Vector2(distanceFromEdge + 12, distanceFromEdge + 120), ModContent.TileType<GemsandstoneTile>());
            for (int j = 0; j < 2; j++)
            {
                for (int i = 2; i > 0; i--)
                {
                    if ((j == 0 && i == 2) || (j == 1 && i == 1))
                    EEWorld.EEWorld.MakeOvalFlatTop(44, 12, new Vector2(Main.maxTilesX - distanceFromEdge - (j * 50) - 44, distanceFromEdge + (i * 40) - 50), ModContent.TileType<GemsandstoneTile>());
                }
            }
            fillers.Add(new Vector2(Main.maxTilesX - distanceFromEdge - 44, distanceFromEdge + 120));
            fillers.Add(new Vector2(60, 60));
            EEWorld.EEWorld.MakeLayerWithOutline(Main.maxTilesX / 2, 70, 30, 1, ModContent.TileType<HardenedGemsandTile>(),40);
            EEWorld.EEWorld.KillWall(Main.maxTilesX, Main.maxTilesY, Vector2.Zero);
            Main.spawnTileX = 500;
            Main.spawnTileY = 300;
            SubworldManager.SettleLiquids();
            EEMod.isSaving = false;
            EEWorld.EEWorld.MakeOval(350, 190, new Vector2(Main.maxTilesX / 2 - 160, (int)startingHeightOfUpperClass + 25), ModContent.TileType<GemsandstoneTile>(), false);
            EEWorld.EEWorld.MakeOval(335, 160, new Vector2(Main.maxTilesX / 2 - 165, (int)startingHeightOfUpperClass + 40), TileID.StoneSlab, true);
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (tile.type == TileID.StoneSlab)
                        WorldGen.KillTile(i, j);
                }
            }
            //EEWorld.EEWorld.MakeChasm(Main.maxTilesX / 2 - 120, (int)startingHeightOfUpperClass + 75, 70, ModContent.TileType<GemsandstoneTile>(), 0, 1,1);
            EEWorld.EEWorld.MakeAtlantisCastle(Main.maxTilesX / 2 - 146, (int)startingHeightOfUpperClass + 65);
            //imagine coding...
            for (int j = 0; j < fillers.Count; j++)
            {
                switch (listNumbersFillers[j])
                {
                    case 0:
                        {
                            EEWorld.EEWorld.PlaceFiller1((int)fillers[j].X - EEWorld.EEWorld.Filler1.GetLength(0) / 2, (int)fillers[j].Y - EEWorld.EEWorld.Filler1.GetLength(1) / 2, EEWorld.EEWorld.Filler1);
                            break;
                        }
                    case 1:
                        {
                            EEWorld.EEWorld.PlaceFiller2((int)fillers[j].X - EEWorld.EEWorld.Filler2.GetLength(0) / 2, (int)fillers[j].Y - EEWorld.EEWorld.Filler2.GetLength(1) / 2, EEWorld.EEWorld.Filler2);
                            break;
                        }
                    case 2:
                        {
                            EEWorld.EEWorld.PlaceFiller3((int)fillers[j].X - EEWorld.EEWorld.Filler3.GetLength(0) / 2, (int)fillers[j].Y - EEWorld.EEWorld.Filler3.GetLength(1) / 2, EEWorld.EEWorld.Filler3);
                            break;
                        }
                    case 3:
                        {
                            EEWorld.EEWorld.PlaceFiller4((int)fillers[j].X - EEWorld.EEWorld.Filler4.GetLength(0) / 2, (int)fillers[j].Y - EEWorld.EEWorld.Filler4.GetLength(1) / 2, EEWorld.EEWorld.Filler4);
                            break;
                        }
                    case 4:
                        {
                            EEWorld.EEWorld.PlaceFiller5((int)fillers[j].X - EEWorld.EEWorld.Filler5.GetLength(0) / 2, (int)fillers[j].Y - EEWorld.EEWorld.Filler5.GetLength(1) / 2, EEWorld.EEWorld.Filler5);
                            break;
                        }
                    case 5:
                        {
                            EEWorld.EEWorld.PlaceFiller6((int)fillers[j].X - EEWorld.EEWorld.Filler6.GetLength(0) / 2, (int)fillers[j].Y - EEWorld.EEWorld.Filler6.GetLength(1) / 2, EEWorld.EEWorld.Filler6);
                            break;
                        }
                }
            }
            WorldGen.TileRunner(80, 80, 30, 10, ModContent.TileType<GemsandstoneTile>());
            WorldGen.TileRunner(100, 60, 30, 10, ModContent.TileType<GemsandstoneTile>());
        }
        public static void VolcanoIsland(int seed, GenerationProgress customProgressObject = null)
        {
            Main.maxTilesX = 400;
            Main.maxTilesY = 405;
            //Main.worldSurface = Main.maxTilesY;
            //Main.rockLayer = Main.maxTilesY;
            SubworldManager.Reset(seed);
            SubworldManager.PostReset(customProgressObject);

            int islandWidth = 300;
            int islandHeight = 90;

            EEWorld.EEWorld.FillRegionWithWater(Main.maxTilesX, Main.maxTilesY, Vector2.Zero);
            EEWorld.EEWorld.RemoveWaterFromRegion(Main.maxTilesX, 210, Vector2.Zero);
            EEWorld.EEWorld.MakeOvalJaggedTop(Main.maxTilesX, Main.maxTilesY - 300, new Vector2(0, 300), ModContent.TileType<GemsandTile>(), 15, 15);


            EEWorld.EEWorld.RemoveWaterFromRegion(40, 40, new Vector2(180, 170));
            EEWorld.EEWorld.MakeOvalJaggedBottom(islandWidth, islandHeight, new Vector2(50, 210), ModContent.TileType<VolcanicAshTile>());
            EEWorld.EEWorld.MakeTriangle(new Vector2(100, 230), 200, 160, 2, ModContent.TileType<VolcanicAshTile>(), true, true, ModContent.WallType<VolcanicAshWallTile>());
            EEWorld.EEWorld.FillRegionWithLava(40, 50, new Vector2(180, 190));
            EEWorld.EEWorld.KillWall(Main.maxTilesX, Main.maxTilesY, Vector2.Zero);
            EEWorld.EEWorld.MakeVolcanoEntrance(198, 192, EEWorld.EEWorld.VolcanoEntrance);

            SubworldManager.SettleLiquids();
            EEMod.isSaving = false;
            Main.spawnTileX = 200;
            Main.spawnTileY = 100;
        }
        public static void VolcanoInside(int seed, GenerationProgress customProgressObject = null)
        {
            Main.maxTilesX = 400;
            Main.maxTilesY = 405;
            //Main.worldSurface = Main.maxTilesY;
            //Main.rockLayer = Main.maxTilesY;
            SubworldManager.Reset(seed);
            SubworldManager.PostReset(customProgressObject);

            EEWorld.EEWorld.FillRegion(Main.maxTilesX, Main.maxTilesY, Vector2.Zero, ModContent.TileType<MagmastoneTile>());
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    if(Main.rand.NextBool(3000))
                        EEWorld.EEWorld.MakeLavaPit(Main.rand.Next(20, 30), Main.rand.Next(7, 20), new Vector2(i, j), Main.rand.NextFloat(0.1f, 0.5f));
                }
            }
            EEWorld.EEWorld.MakeChasm(200, 10, 100, TileID.StoneSlab, 0, 10, 20);
            for (int i = 0; i < 5; i++)
            {
                WorldGen.TileRunner(200, 190, 200, 1, TileID.StoneSlab);
            }
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

            SubworldManager.SettleLiquids();
            EEMod.isSaving = false;
            Main.spawnTileX = 200;
            Main.spawnTileY = 20;
        }
        public static void Cutscene1(int seed, GenerationProgress customProgressObject = null)
        {
            Main.maxTilesX = 400;
            Main.maxTilesY = 400;
            SubworldManager.Reset(seed);
            SubworldManager.PostReset(customProgressObject);
            EEWorld.EEWorld.FillRegion(Main.maxTilesX, Main.maxTilesY, new Vector2(0, 0), ModContent.TileType<VolcanicAshTile>());
            EEWorld.EEWorld.MakeLayer(200, 100, 90, 1, ModContent.TileType<VolcanicAshTile>());
            EEWorld.EEWorld.MakeOvalFlatTop(40,10,new Vector2(200-20,100), ModContent.TileType<VolcanicAshTile>());
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
            EEWorld.EEWorld.FillWall(Main.maxTilesX, Main.maxTilesY, Vector2.Zero,ModContent.WallType<VolcanoBG>());
            SubworldManager.SettleLiquids();
            EEMod.isSaving = false;
            Main.spawnTileX = 200;
            Main.spawnTileY = 140;
        }
    }
}