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
            Main.maxTilesX = 600;
            Main.maxTilesY = 500;
            SubworldManager.Reset(seed);
            SubworldManager.PostReset(customProgressObject);


            EEWorld.EEWorld.FillRegionWithWater(Main.maxTilesX, Main.maxTilesY, new Vector2(0, 0));
            EEWorld.EEWorld.RemoveWaterFromRegion(Main.maxTilesX, 180, new Vector2(0, 0));

            EEWorld.EEWorld.Island(400, 225);

            EEWorld.EEWorld.MakeOvalJaggedTop(Main.maxTilesX, 50, new Vector2(0, 200), ModContent.TileType<CoralSand>(), 15, 15);
            EEWorld.EEWorld.FillRegion(Main.maxTilesX, Main.maxTilesY - 200, new Vector2(0, 225), ModContent.TileType<CoralSand>());

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
        public static void VolcanoIsland(int seed, GenerationProgress customProgressObject = null)
        {
            Main.maxTilesX = 800;
            Main.maxTilesY = 500;
            //Main.worldSurface = Main.maxTilesY;
            //Main.rockLayer = Main.maxTilesY;
            SubworldManager.Reset(seed);
            SubworldManager.PostReset(customProgressObject);

            int islandWidth = 300;
            int islandHeight = 90;

            EEWorld.EEWorld.FillRegionWithWater(Main.maxTilesX, Main.maxTilesY, Vector2.Zero);
            EEWorld.EEWorld.RemoveWaterFromRegion(Main.maxTilesX, 260, Vector2.Zero);
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