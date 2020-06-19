using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using EEMod.Tiles;

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
            EEWorld.EEWorld.FillWall(400, 400, new Vector2(0, 0), WallID.Waterfall);
            EEMod.isSaving = false;
            SubworldManager.PostReset(customProgressObject);
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
            EEMod.isSaving = false;
            Main.spawnTileX = boatPos;
            Main.spawnTileY = EEWorld.EEWorld.TileCheckWater(boatPos) - 22;
        }
        public static void Island(int seed, GenerationProgress customProgressObject = null)
        {
            Main.maxTilesX = 400;
            Main.maxTilesY = 405;
            Main.spawnTileX = 234;
            Main.spawnTileY = 92;
            Main.worldSurface = Main.maxTilesY;
            Main.rockLayer = Main.maxTilesY;
            SubworldManager.Reset(seed);
            SubworldManager.PostReset(customProgressObject);


            int islandWidth = 274;
            int islandHeight = 120;


            //This is going to move later, just here for now for simplicity, don't @ me
            //Not the island
            EEWorld.EEWorld.FillRegionWithWater(Main.maxTilesX, Main.maxTilesY, new Vector2(0, 0));
            EEWorld.EEWorld.RemoveWaterFromRegion(Main.maxTilesX, 180, new Vector2(0, 0));
            EEWorld.EEWorld.MakeOvalJaggedTop(Main.maxTilesX, Main.maxTilesY - 300, new Vector2(0, 300), ModContent.TileType<GemsandTile>(), 15, 15);

            
            //The island
            EEWorld.EEWorld.MakeOvalJaggedBottom(islandWidth, islandHeight, new Vector2((Main.maxTilesX / 2) - islandWidth / 2, 164), ModContent.TileType<CoralSand>());
            EEWorld.EEWorld.MakeOvalJaggedBottom(islandWidth - 60, islandHeight - 40, new Vector2((Main.maxTilesX / 2) - (islandWidth - 60) / 2, 160), TileID.Dirt);
            EEWorld.EEWorld.KillWall(Main.maxTilesX, Main.maxTilesY, Vector2.Zero);
            for(int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    WorldGen.SpreadGrass(i, j);
                }
            }
            for (int j = 0; j < Main.maxTilesX; j++)
            {
                if ((Main.rand.Next(5) == 0) && (EEWorld.EEWorld.TileCheck(j, ModContent.TileType<CoralSand>()) < EEWorld.EEWorld.TileCheck(j, TileID.Dirt)) && (EEWorld.EEWorld.TileCheck(j, ModContent.TileType<CoralSand>()) < EEWorld.EEWorld.TileCheck(j, TileID.Grass)))
                    WorldGen.PlaceTile(j, EEWorld.EEWorld.TileCheck(j, ModContent.TileType<CoralSand>()) - 1, 324);
            }

            for (int i = 42; i < Main.maxTilesX-42; i++)
            {
                for (int j = 42; j < Main.maxTilesY-42; j++)
                {
                    int yes = WorldGen.genRand.Next(0, 5);
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (EEWorld.EEWorld.TileCheck2(i, j) == 2 && yes < 4 && tile.type == ModContent.TileType<CoralSand>())
                    {
                        int selection = WorldGen.genRand.Next(6);
                        switch (selection)
                        {
                            case 0:
                                WorldGen.PlaceTile(i, j - 1, TileID.Coral);
                                break;
                            case 1:
                                WorldGen.PlaceTile(i, j - 1, TileID.Coral);
                                break;
                            case 2:
                                WorldGen.PlaceTile(i, j - 1, TileID.Coral);
                                break;
                            case 3:
                                WorldGen.PlaceTile(i, j - 1, TileID.Coral);
                                break;
                            case 4:
                                WorldGen.PlaceTile(i, j - 1, TileID.Coral);
                                break;
                            case 5:
                                WorldGen.PlaceTile(i, j - 1, TileID.Coral);
                                break;
                        }
                    }
                }
            }
            EEWorld.EEWorld.PlaceShip(50, 158, EEWorld.EEWorld.ShipTiles);
            EEWorld.EEWorld.PlaceShipWalls(50, 158, EEWorld.EEWorld.ShipWalls);

            SubworldManager.SettleLiquids();




            /*EEWorld.EEWorld.MakeOvalJaggedTop(Main.maxTilesX, 20, new Vector2(0, 380), ModContent.TileType<CoralSand>());

            //The island
            EEWorld.EEWorld.MakeOvalJaggedBottom(islandWidth, islandHeight, new Vector2((Main.maxTilesX / 2) - islandWidth / 2, 170), ModContent.TileType<CoralSand>());
            EEWorld.EEWorld.MakeOvalJaggedBottom(216, 150, new Vector2((Main.maxTilesX / 2) - 216 / 2, 165), TileID.Dirt);
            EEMod.isSaving = false;
            EEWorld.EEWorld.FillRegionWithWater(Main.maxTilesX, (Main.maxTilesY - 190), new Vector2(0, 190));
            SubworldManager.SettleLiquids();*/
        }
    }
}