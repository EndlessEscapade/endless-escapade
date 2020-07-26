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
            //Variables and Initialization stuff
            int depth = 70;
            int boatPos = Main.maxTilesX / 2;
            int roomCount = 6;
            Vector2[] rooms = new Vector2[roomCount];
            Main.maxTilesX = 1500;
            Main.maxTilesY = 2400;
            SubworldManager.Reset(seed);
            SubworldManager.PostReset(customProgressObject);


            //Placing initial blocks
            EEWorld.EEWorld.FillRegion(Main.maxTilesX, (int)(Main.maxTilesY / 3), new Vector2(0, 0), ModContent.TileType<LightGemsandTile>());
            EEWorld.EEWorld.FillRegion(Main.maxTilesX, (int)(Main.maxTilesY / 3), new Vector2(0, (int)Main.maxTilesY / 3), ModContent.TileType<GemsandTile>());
            EEWorld.EEWorld.FillRegion(Main.maxTilesX, (int)(Main.maxTilesY / 3), new Vector2(0, (int)(Main.maxTilesY / 3) * 2), ModContent.TileType<DarkGemsandTile>());
            EEWorld.EEWorld.ClearRegion(Main.maxTilesX, Main.maxTilesY / 30, Vector2.Zero);

            for (int i = 0; i < 10; i++)
                for (int j = -5; j < 5; j++)
                    WorldGen.TileRunner(300 + (i * 170) + (j * 10), Main.maxTilesY / 20, 10, 10, ModContent.TileType<GemsandTile>(), true, 0, 0, true, true);
            for (int i = 0; i < 100; i++)
                for (int j = -5; j < 5; j++)
                    WorldGen.TileRunner(300 + (i * 17) + (j * 10), Main.maxTilesY / 20, 4, 10, ModContent.TileType<GemsandTile>(), true, 0, 0, true, true);

            EEWorld.EEWorld.FillRegionNoEdit(Main.maxTilesX, Main.maxTilesY / 20, new Vector2(0, Main.maxTilesY / 20), ModContent.TileType<CoralSand>());
            int maxTiles = (int)(Main.maxTilesX * Main.maxTilesY * 9E-04);


            //Making chasms
            int ree = Main.rand.Next(100, 750);
            EEWorld.EEWorld.MakeWavyChasm2(ree, 100, 300, TileID.StoneSlab, 0.3f, WorldGen.genRand.Next(50, 60), true);
            rooms[0] = new Vector2(ree + 125 - 100, 400);

            ree = Main.rand.Next(-400, 401);
            int legoYoda = Main.rand.Next(-400, 401);
            while(legoYoda <= 250 && legoYoda >= -250 && legoYoda + rooms[0].Y < 500 && legoYoda + rooms[0].Y > 100)
                legoYoda = Main.rand.Next(-400, 401);
            while (ree <= 250 && ree >= -250 && ree - 100 + rooms[0].X > 200 && ree - 100 + rooms[0].X < 1299)
                ree = Main.rand.Next(-400, 401);

            rooms[1] = new Vector2(ree - 100 + rooms[0].X, legoYoda + rooms[0].Y);

            ree = Main.rand.Next(-400, 401);
            legoYoda = Main.rand.Next(-400, 401);
            while (legoYoda <= 250 && legoYoda >= -250 && legoYoda + rooms[1].Y < 500 && legoYoda + rooms[1].Y > 100)
                legoYoda = Main.rand.Next(-400, 401);
            while (ree <= 250 && ree >= -250 && ree - 100 + rooms[1].X > 200 && ree - 100 + rooms[1].X < 1299)
                ree = Main.rand.Next(-400, 401);

            rooms[2] = new Vector2(ree - 100 + rooms[1].X, legoYoda + rooms[1].Y);




            EEWorld.EEWorld.MakeCoralRoom((int)rooms[0].X, (int)rooms[0].Y, 200, 0, 0);
            EEWorld.EEWorld.MakeWavyChasm2((int)rooms[0].X, (int)rooms[0].Y, Math.Abs((int)rooms[0].Y - (int)rooms[1].Y), TileID.StoneSlab, ((int)rooms[1].Y - (int)rooms[0].Y)/((int)rooms[1].X - (int)rooms[0].X), WorldGen.genRand.Next(50, 60), true);
            EEWorld.EEWorld.MakeCoralRoom((int)rooms[1].X, (int)rooms[1].Y, 200, 0, 0);
            EEWorld.EEWorld.MakeWavyChasm2((int)rooms[1].X, (int)rooms[1].Y, Math.Abs((int)rooms[1].Y - (int)rooms[2].Y), TileID.StoneSlab, ((int)rooms[2].Y - (int)rooms[1].Y) / ((int)rooms[2].X - (int)rooms[1].X), WorldGen.genRand.Next(50, 60), true);
            EEWorld.EEWorld.MakeCoralRoom((int)rooms[2].X, (int)rooms[2].Y, 200, 0, 0);






            //Making chasms and hollowed-out areas
            /*EEWorld.EEWorld.MakeWavyChasm2(100, 100, 500, TileID.StoneSlab, 0.3f, WorldGen.genRand.Next(50, 60), true);
            EEWorld.EEWorld.MakeWavyChasm2(Main.maxTilesX - 100, 100, 500, TileID.StoneSlab, -0.3f, WorldGen.genRand.Next(50, 60), true);

            EEWorld.EEWorld.MakeWavyChasm(450, 100, 200, TileID.StoneSlab, 0.5f, WorldGen.genRand.Next(50, 60));
            EEWorld.EEWorld.ClearOval(100, 100, new Vector2(650, 250));

            EEWorld.EEWorld.ClearOval(300, 150, new Vector2(250, 500));
            EEWorld.EEWorld.ClearOval(300, 150, new Vector2(1100, 550));

            EEWorld.EEWorld.MakeWavyChasm2(700, 550, 550, TileID.StoneSlab, 0.5f, WorldGen.genRand.Next(50, 60), true);
            EEWorld.EEWorld.MakeWavyChasm2(1300, 600, 500, TileID.StoneSlab, -0.5f, WorldGen.genRand.Next(50, 60), true);

            EEWorld.EEWorld.ClearOval(800, 400, new Vector2(350, 950));*/





            /*for (int i = 0; i < 5; i++)
            {
                EEWorld.EEWorld.MakeChasm(chasmX + Main.rand.Next(-50, 50) + i * 20, chasmY + (i * 200) + Main.rand.Next(-50, 50), Main.rand.Next(5, 30), TileID.StoneSlab, Main.rand.Next(5, 10), WorldGen.genRand.Next(20, 60), Main.rand.Next(10, 20));
            }
            for (int i = 0; i < 20; i++)
            {
                EEWorld.EEWorld.MakeOvalFlatTop(Main.rand.Next(10, 20), Main.rand.Next(5, 10), new Vector2(chasmX + Main.rand.Next(-10, 10) + i * 15, chasmY + (i * 50) + Main.rand.Next(-20, 20)), ModContent.TileType<GemsandTile>());
                if (i % 5 == 0)
                {
                    EEWorld.EEWorld.MakeLayer(chasmX + Main.rand.Next(-10, 10) + i * 15, chasmY + Main.rand.Next(-20, 20) + (i * 50), 25, 2, ModContent.TileType<GemsandTile>());
                    EEWorld.EEWorld.MakeLayer(chasmX + Main.rand.Next(-10, 10) + i * 5, chasmY + Main.rand.Next(-20, 20) + (i * 50), 20, 1, ModContent.TileType<GemsandTile>());
                    EEWorld.EEWorld.MakeCoral(new Vector2(chasmX + Main.rand.Next(-10, 10) + i * 5, chasmY + Main.rand.Next(-20, 20) + (i * 50)), TileID.Coralstone, Main.rand.Next(4, 8));
                    for (int j = 0; j < 7; j++)
                        EEWorld.EEWorld.MakeOvalFlatTop(WorldGen.genRand.Next(20, 30), WorldGen.genRand.Next(5, 10), new Vector2(chasmX + Main.rand.Next(-10, 10) + i * 15 + (j * 35) - 50, chasmY + Main.rand.Next(-20, 20) + (i * 50)), ModContent.TileType<DarkGemsandTile>());
                }
            }*/
            /*for (int k = 0; k < maxTiles * 9; k++)
            {
                int xPos = 500;
                int yPos = 1200;
                int size = 80;
                int x = WorldGen.genRand.Next(xPos - (size * 3), xPos + (size * 3));
                int y = WorldGen.genRand.Next(yPos - (size * 3), yPos + (size * 3));
                if (EEWorld.EEWorld.OvalCheck(xPos, yPos, x, y, size * 3, size))
                    WorldGen.TileRunner(x, y, WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(5, 10), TileID.StoneSlab, true, 0f, 0f, true, true);
            }

            for (int i = 0; i < 800; i++)
            {
                for (int j = 0; j < 2000; j++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (tile.type == TileID.StoneSlab)
                        WorldGen.KillTile(i, j);
                }
            }*/




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
                    Tile.SmoothSlope(i, j);
                }
            }

            //Placing boat
            EEWorld.EEWorld.PlaceShip(boatPos, EEWorld.EEWorld.TileCheckWater(boatPos) - 22, EEWorld.EEWorld.ShipTiles);
            EEWorld.EEWorld.PlaceShipWalls(boatPos, EEWorld.EEWorld.TileCheckWater(boatPos) - 27, EEWorld.EEWorld.ShipWalls);
            CoralBoatPos = new Vector2(boatPos, EEWorld.EEWorld.TileCheckWater(boatPos) - 22);

            //Finishing initialization stuff
            EEMod.isSaving = false;
            Main.spawnTileX = boatPos;
            Main.spawnTileY = EEWorld.EEWorld.TileCheckWater(boatPos) - 22;
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
                        int selection = WorldGen.genRand.Next(4);
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
                            case 3:
                                WorldGen.PlaceTile(i, j - 1, ModContent.TileType<KelpTile>());
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
