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
using EEMod.Systems.Subworlds.EESubworlds;

namespace EEMod
{
    public class EESubWorlds
    {
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