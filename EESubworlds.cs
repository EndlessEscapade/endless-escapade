using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.World.Generation;
using Terraria.ModLoader;
using EEMod.Walls;
using EEMod.Tiles;
using EEMod.EEWorld;
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
           SubworldManager.Reset(seed, customProgressObject);
           EEWorld.EEWorld.FillRegion(400, 400, new Vector2(0, 0), TileID.SandstoneBrick);
           EEWorld.EEWorld.Pyramid(63, 42);
      }
        public static void Sea(int seed, GenerationProgress customProgressObject = null)
        {
            Main.maxTilesX = 400;
            Main.maxTilesY = 400;
            Main.spawnTileX = 234;
            Main.spawnTileY = 92;
            SubworldManager.Reset(seed, customProgressObject);
            EEWorld.EEWorld.FillWall(400, 400, new Vector2(0, 0), WallID.Waterfall);
        }
        public static void CoralReefs(int seed, GenerationProgress customProgressObject = null)
        {
            Main.maxTilesX = 1000;
            Main.maxTilesY = 2000;
            Main.spawnTileX = 234;
            Main.spawnTileY = 92;
            int boatPos = Main.maxTilesX / 2;
            SubworldManager.Reset(seed, customProgressObject);
            EEWorld.EEWorld.FillRegion(1000, 2000, new Vector2(0, 0), ModContent.TileType<HardenedGemsandTile>());
            EEWorld.EEWorld.CoralReef();
            EEWorld.EEWorld.ClearRegion(1000, 80, Vector2.Zero);
            EEWorld.EEWorld.fillRegionWithWater(1000, 1930, new Vector2(0,70));
            EEWorld.EEWorld.PlaceShip(boatPos, EEWorld.EEWorld.TileCheck(boatPos) - 22, EEWorld.EEWorld.ShipTiles);
            EEWorld.EEWorld.PlaceShipWalls(boatPos, EEWorld.EEWorld.TileCheck(boatPos) - 22, EEWorld.EEWorld.ShipWalls);
        }
    }
}
