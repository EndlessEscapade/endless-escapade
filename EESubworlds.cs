using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.World.Generation;

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
            Main.maxTilesY = 1000;
            Main.spawnTileX = 234;
            Main.spawnTileY = 92;
            SubworldManager.Reset(seed, customProgressObject);
            EEWorld.EEWorld.CoralReef();
        }
    }
}
