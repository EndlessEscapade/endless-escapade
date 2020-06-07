using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Generation;
using Terraria.World.Generation;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using InteritosMod.Autoloading;
using InteritosMod.Tiles.Ores;
using InteritosMod.Tiles;
using InteritosMod.Tiles.Furniture;

namespace InteritosMod.IntWorld
{
    public partial class InteritosWorld : ModWorld
    {
        public static bool GenkaiMode;

        public static bool downedGallagar;
        public static bool downedForerunner;
        public static bool downedSoS;
        public static bool downedFlare;
        public static bool downedAssimilator;
        public static bool downedAkumo;
        public static bool downedHydros;
        public static bool downedStagrel;
        public static bool downedBeheader;

        [Unloading]
        private static List<Point> BiomeCenters;
        public static int CoralReefsTiles = 0;

        public override void ResetNearbyTileEffects()
        {
            CoralReefsTiles = 0;
        }

		public override void TileCountsAvailable(int[] tileCounts)
		{
			CoralReefsTiles = tileCounts[mod.TileType("GemsandstoneTile")];
		}

        public override TagCompound Save()
        {
            List<string> boolflags = new List<string>();

            // Game modes
            if (GenkaiMode)
                boolflags.Add("GenkaiMode");

            // Downed bosses
            if (downedGallagar)
                boolflags.Add("downedGallagar");
            if (downedForerunner)
                boolflags.Add("downedForerunner");
            if (downedSoS)
                boolflags.Add("downedSoS");
            if (downedFlare)
                boolflags.Add("downedFlare");
            if (downedAssimilator)
                boolflags.Add("downedAssimilator");
            if (downedAkumo)
                boolflags.Add("downedAkumo");
            if (downedHydros)
                boolflags.Add("downedHydros");
            if (downedStagrel)
                boolflags.Add("downedStagrel");
            if (downedBeheader)
                boolflags.Add("downedBeheader");


            return new TagCompound
            {
                ["SaveVersion"] = new Version(0, 3, 0, 0).ToString(),
                ["boolFlags"] = boolflags
            };
        }

        public override void Load(TagCompound tag)
        {
            IList<string> flags = tag.GetList<string>("boolFlags");

            // Game modes
            GenkaiMode = flags.Contains("GenkaiMode");

            // Downed bosses
            downedGallagar = flags.Contains("downedGallagar");
            downedForerunner = flags.Contains("downedForerunner");
            downedSoS = flags.Contains("downedSoS");
            downedFlare = flags.Contains("downedFlare");
            downedAssimilator = flags.Contains("downedAssimilator");
            downedAkumo = flags.Contains("downedAkumo");
            downedHydros = flags.Contains("downedHydros");
            downedStagrel = flags.Contains("downedStagrel");
            downedBeheader = flags.Contains("downedBeheader");
        }


        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            if (ShiniesIndex != -1)
            {
                tasks.Insert(ShiniesIndex + 1, new PassLegacy("Interitos Mod Ores", InteritosModOres));
            }
            int MicroBiomes = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
            int LivingTreesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Living Trees"));
            if (LivingTreesIndex != -1)
            {
                tasks.Insert(LivingTreesIndex + 1, new PassLegacy("Post Terrain", delegate (GenerationProgress progress)
                {
                    progress.Message = "Generating structures";
                    for (int l = 0; l < 30; l++)
                    {
                        int posX = WorldGen.genRand.Next(0, Main.maxTilesX);
                        int posY = WorldGen.genRand.Next((int)WorldGen.rockLayerLow, Main.maxTilesY);
                        PlaceRuins(posX, posY, ruinsShape);
                    }
                }));
            }
            if (MicroBiomes != -1)
            {
                tasks.Insert(MicroBiomes, new PassLegacy("Coral Reef", delegate (GenerationProgress progress)
                {
                    CoralReef();
                }));
            }

        }
        private void clearRegion(int width, int height, Vector2 startingPoint)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    WorldGen.KillTile(i + (int)startingPoint.X, j + (int)startingPoint.Y);
                    WorldGen.KillWall(i + (int)startingPoint.X, j + (int)startingPoint.Y);
                }
            }
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    WorldGen.PlaceWall(i + (int)startingPoint.X, j + (int)startingPoint.Y + 1060, WallID.CorruptionUnsafe1);
                }
            }

        }
        private void fillRegionWithWater(int width, int height, Vector2 startingPoint)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Main.tile[i + (int)startingPoint.X, j + (int)startingPoint.Y].liquidType(0); // set liquid type 0 is water 1 lava 2 honey 3+ water iirc
                    Main.tile[i + (int)startingPoint.X, j + (int)startingPoint.Y].liquid = 255; // set liquid ammount
                    WorldGen.SquareTileFrame(i + (int)startingPoint.X, j + (int)startingPoint.Y, true); // soemthing for avoiding the liquid from being static
                    if (Main.netMode == NetmodeID.MultiplayerClient) // sync
                        NetMessage.sendWater(i + (int)startingPoint.X, j + (int)startingPoint.Y);
                }
            }
        }
        private int tileCheck2(int i, int j)
        {
            Tile tile1 = Framing.GetTileSafely(i, j);
            Tile tile2 = Framing.GetTileSafely(i, j - 1);
            Tile tile3 = Framing.GetTileSafely(i, j - 2);
            Tile tile4 = Framing.GetTileSafely(i, j + 1);
            Tile tile5 = Framing.GetTileSafely(i, j + 2);
            if (tile1.active() && tile2.active() && tile3.active() && !tile4.active() && !tile5.active())
            {
                return 1;
            }
            if (tile1.active() && !tile2.active() && !tile3.active() && tile4.active() && tile5.active())
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }
        private void makeOvalFlatTop(int width, int height, Vector2 startingPoint, int type)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (OvalCheck((int)(startingPoint.X + width / 2), (int)(startingPoint.Y + height / 2), i + (int)startingPoint.X, j + (int)startingPoint.Y, (int)(width * .5f), (int)(height * .5f)))
                        WorldGen.PlaceTile(i + (int)startingPoint.X, j + (int)startingPoint.Y, type);

                    if (i == width / 2 && j == height / 2)
                    {
                        WorldGen.TileRunner(i + (int)startingPoint.X, j + (int)startingPoint.Y + 2, WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(10, 20), type, true, 0f, 0f, true, true);
                    }
                }
            }
            for (int i = 0; i < width; i++)
            {
                for (int j = -6; j < height / 2 - 2; j++)
                {
                    Tile tile = Framing.GetTileSafely(i + (int)startingPoint.X, j + (int)startingPoint.Y);
                    if (tile.type == type)
                        WorldGen.KillTile(i + (int)startingPoint.X, j + (int)startingPoint.Y);
                }
            }
        }
        private void fillRegion(int width, int height, Vector2 startingPoint, int type)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    WorldGen.PlaceTile(i + (int)startingPoint.X, j + (int)startingPoint.Y, type);
                }
            }
        }
        private void fillWall(int width, int height, Vector2 startingPoint, int type)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    WorldGen.PlaceWall(i + (int)startingPoint.X, j + (int)startingPoint.Y, type);
                }
            }
        }
        // ---- overload
        private void fillRegion(int width, int height, Vector2 startingPoint, int type1, int type2)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int tileType;
                    if (Main.rand.Next(100) < 95)
                        tileType = type1;
                    else
                        tileType = type2;

                    WorldGen.PlaceTile(i + (int)startingPoint.X, j + (int)startingPoint.Y, tileType);
                }
            }
        }
        private void makeChasm(int positionX, int positionY, int height, int type, float slant, int sizeAddon)
        {
            for (int i = 0; i < height; i++)
            {
                // Tile tile = Framing.GetTileSafely(positionX + (int)(i * slant), positionY + i);
                WorldGen.TileRunner(positionX + (int)(i * slant), positionY + i, WorldGen.genRand.Next(5 + sizeAddon / 2, 10 + sizeAddon), WorldGen.genRand.Next(5, 10), type, true, 0f, 0f, true, true);
            }
        }
        private void makeWavyChasm(int positionX, int positionY, int height, int type, float slant, int sizeAddon)
        {
            for (int i = 0; i < height; i++)
            {
                // Tile tile = Framing.GetTileSafely(positionX + (int)(i * slant), positionY + i);
                WorldGen.TileRunner(positionX + (int)(i * slant) + (int)(Math.Sin(i/(float)30)*60), positionY + i, WorldGen.genRand.Next(5 + sizeAddon / 2, 10 + sizeAddon), WorldGen.genRand.Next(5, 10), type, true, 0f, 0f, true, true);
            }
        }
        private int tileCheck(int positionX)
        {
            for (int i = 0; i < Main.maxTilesY; i++)
            {
                Tile tile = Framing.GetTileSafely(positionX, i);
                if (tile.type == TileID.Sand)
                {
                    return i;
                }
            }
            return 0;
        }
        private void killWall(int width, int height, Vector2 startingPoint)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    WorldGen.KillWall(i + (int)startingPoint.X, j + (int)startingPoint.Y);
                }
            }
        }

        private bool OvalCheck(int h, int k, int x, int y, int a, int b)
        {
            double p = (Math.Pow((x - h), 2) / Math.Pow(a, 2))
                    + (Math.Pow((y - k), 2) / Math.Pow(b, 2));

            return p < 1 ? true : false;
        }

        private void makeLayer(int X, int midY, int size, int layer)
        {
            double density = 9000;
            if (layer == 2)
                density = 30000;
            int maxTiles = (int)(Main.maxTilesX * Main.maxTilesY * 9E-04);
            for (int k = 0; k < maxTiles; k++)
            {
                int x = WorldGen.genRand.Next(X - 150, X + 150);
                int y = WorldGen.genRand.Next(midY - 100, midY + 100);
                // Tile tile = Framing.GetTileSafely(x, y);
                if (layer == 1)
                {
                    if (Vector2.Distance(new Vector2(x, y), new Vector2(X, midY)) < size)
                        WorldGen.TileRunner(x, y, WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(5, 10), TileID.StoneSlab, true, 0f, 0f, true, true);
                }
                if (layer == 2)
                {
                    if (OvalCheck(X, midY, x, y, size * 3, size))
                        WorldGen.TileRunner(x, y, WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(5, 10), TileID.StoneSlab, true, 0f, 0f, true, true);
                }
            }
            for (int i = 0; i < 800; i++)
            {
                for (int j = 0; j < 1000; j++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (tile.type == TileID.StoneSlab)
                        WorldGen.KillTile(i, j);
                }
            }
            for (int k = 0; k < density; k++)
            {
                int x = WorldGen.genRand.Next(X - 150, X + 150);
                int y = WorldGen.genRand.Next(midY - 100, midY + 100);
                Tile tile = Framing.GetTileSafely(x, y);
                if (layer == 1)
                {
                    int sizeSQ = size * size + 50 * 50;
                    if (Vector2.DistanceSquared(new Vector2(x, y), new Vector2(X, midY)) < (sizeSQ) && tile.active())
                        WorldGen.TileRunner(x, y, WorldGen.genRand.Next(4, 10), WorldGen.genRand.Next(5, 10), ModContent.TileType<HardenedGemsandTile>(), true, 0f, 0f, true, true);
                }
                if (layer == 2)
                {
                    if (OvalCheck(X, midY, x, y, (size * 3) + 10, (size) + 10) && tile.active())
                        WorldGen.TileRunner(x, y, WorldGen.genRand.Next(4, 10), WorldGen.genRand.Next(5, 10), ModContent.TileType<GemsandstoneTile>(), true, 0f, 0f, true, true);
                }
            }
            if (layer == 1)
                WorldGen.TileRunner(X, midY, WorldGen.genRand.Next(30, 50), WorldGen.genRand.Next(10, 20), ModContent.TileType<HardenedGemsandTile>(), true, 1f, 1f, false, true);
        }
        public void CoralReef()
        {
            int chasmX = WorldGen.genRand.Next(150, 200);
            int chasmY = tileCheck(chasmX);
            int sizeOfLayer1 = 80;
            int sizeOfLayer2 = 50;
            int firstLayerPosY = 220;
            int secondLayerPosY = 350;
            //clearRegion(chasmX, 1000, new Vector2(0, tileCheck(chasmX)));
            makeChasm(chasmX, chasmY + 50, 160, TileID.StoneSlab, WorldGen.genRand.NextFloat(-0.1f, 0.1f), WorldGen.genRand.Next(30, 50));

            for (int i = 0; i < 800; i++)
            {
                for (int j = 0; j < 1000; j++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (tile.type == TileID.StoneSlab || tile.type == TileID.Granite || tile.type == TileID.Marble)
                        WorldGen.KillTile(i, j);
                }
            }
            for (int k = 0; k < 50; k++)
            {
                int x = WorldGen.genRand.Next(chasmX - 25, chasmX - 25);
                int y = WorldGen.genRand.Next(chasmY + 50, chasmY + 50 + 160);
                // Tile tile = Framing.GetTileSafely(x, y);
                WorldGen.TileRunner(x, y, WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(10, 20), ModContent.TileType<HardenedGemsandTile>(), true, 0f, 0f, true, true);
            }
            //-right border
            for (int k = 0; k < 50; k++)
            {
                int x = WorldGen.genRand.Next(chasmX + 25, chasmX + 25);
                int y = WorldGen.genRand.Next(chasmY + 50, chasmY + 50 + 160);
                // Tile tile = Framing.GetTileSafely(x, y);
                WorldGen.TileRunner(x, y, WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(10, 20), ModContent.TileType<HardenedGemsandTile>(), true, 0f, 0f, true, true);
            }
            float grad = WorldGen.genRand.NextFloat(0, 1);
            killWall(1000, 500, new Vector2(0, 0));
            makeLayer(chasmX, chasmY + firstLayerPosY, sizeOfLayer1, 1);
            makeLayer(chasmX + 30, chasmY + secondLayerPosY + sizeOfLayer1, sizeOfLayer2, 2);
            //---------------------------1
            makeChasm(chasmX, chasmY + firstLayerPosY + sizeOfLayer1, 100, TileID.StoneSlab, grad, WorldGen.genRand.Next(10, 20));
            makeChasm(chasmX - 15, chasmY + firstLayerPosY + sizeOfLayer1, 80 * (int)(1 + grad / 2), ModContent.TileType<HardenedGemsandTile>(), grad, WorldGen.genRand.Next(5, 10));
            makeChasm(chasmX + 15, chasmY + firstLayerPosY + sizeOfLayer1, 80 * (int)(1 + grad / 2), ModContent.TileType<HardenedGemsandTile>(), grad, WorldGen.genRand.Next(5, 10));
            //---------------------------2
            makeWavyChasm(chasmX, chasmY + secondLayerPosY + sizeOfLayer2+60, 150, TileID.StoneSlab, grad, WorldGen.genRand.Next(20, 30));
            makeWavyChasm(chasmX - 25, chasmY + secondLayerPosY + sizeOfLayer2+60, 130 * (int)(1 + grad / 2), ModContent.TileType<GemsandstoneTile>(), grad, WorldGen.genRand.Next(5, 10));
            makeWavyChasm(chasmX + 25, chasmY + secondLayerPosY + sizeOfLayer2+60, 130 * (int)(1 + grad / 2), ModContent.TileType<GemsandstoneTile>(), grad, WorldGen.genRand.Next(5, 10));
            for (int i = 0; i < 7; i++)
                makeOvalFlatTop(WorldGen.genRand.Next(20, 30), WorldGen.genRand.Next(5, 10), new Vector2(chasmX - 100 + (i * 35), chasmY + sizeOfLayer1 + 350 + WorldGen.genRand.Next(-10, 0)), ModContent.TileType<GemsandstoneTile>());
            for (int i = 0; i < 800; i++)
            {
                for (int j = 0; j < 1000; j++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (tile.type == TileID.StoneSlab)
                        WorldGen.KillTile(i, j);
                }
            }
            for (int i = 0; i < 300; i++)
            {
                for (int j = 0; j < 700; j++)
                {
                    int yes = WorldGen.genRand.Next(5, 10);
                    if (tileCheck2(i, chasmY + j) == 1 && j % yes == 0)
                    {
                        int selection = WorldGen.genRand.Next(2);
                        switch (selection)
                        {
                            case 0:
                                WorldGen.PlaceTile(i, chasmY + j + 1, ModContent.TileType<CoralLanternTile>());
                                break;
                            case 1:
                                WorldGen.PlaceTile(i, chasmY + j + 1, ModContent.TileType<HangingCoralTile>());
                                break;
                        }
                    }
                    if (tileCheck2(i, chasmY + j) == 2 && j % yes <= 4)
                    {
                        int selection = WorldGen.genRand.Next(6);
                        switch (selection)
                        {
                            case 0:
                                WorldGen.PlaceTile(i, chasmY + j - 1, ModContent.TileType<Coral1Tile>());
                                break;
                            case 1:
                                WorldGen.PlaceTile(i, chasmY + j - 1, ModContent.TileType<Coral2Tile>());
                                break;
                            case 2:
                                WorldGen.PlaceTile(i, chasmY + j - 1, ModContent.TileType<Coral3Tile>());
                                break;
                            case 3:
                                WorldGen.PlaceTile(i, chasmY + j - 1, ModContent.TileType<EyeTile>());
                                break;
                            case 4:
                                WorldGen.PlaceTile(i, chasmY + j - 1, ModContent.TileType<CoralLanternLamp>());
                                break;
                            case 5:
                                WorldGen.PlaceTile(i, chasmY + j - 1, ModContent.TileType<BrainTile>());
                                break;
                        }

                    }
                }
            }
            fillRegionWithWater(300, 700, new Vector2(0, chasmY));
            fillWall(300, 700, new Vector2(0, chasmY), WallID.GraniteBlock);
        }
       
        public override void PostWorldGen()
        {
            CoralReef();
        }


        public void PlaceRuins(int i, int j, int[,] shape)
        {
            for (int y = 0; y < shape.GetLength(0); y++)
            {
                for (int x = 0; x < shape.GetLength(1); x++)
                {
                    int k = i - 3 + x;
                    int l = j - 6 + y;
                    if (WorldGen.InWorld(k, l, 30))
                    {
                        Tile tile = Framing.GetTileSafely(k, l);
                        tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 1:
                                WorldGen.PlaceTile(k, l, TileID.GrayBrick);
                                tile.active(true);
                                break;
                            case 2:
                                WorldGen.PlaceTile(k, l, TileID.Stone);
                                tile.active(true);
                                break;
                            case 5:
                                WorldGen.PlaceTile(k, l, TileID.HangingLanterns);
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        private void InteritosModOres(GenerationProgress progress)
        {
            progress.Message = "Interitos Mod Ores";
            int maxtiles = Main.maxTilesX * Main.maxTilesY;
            int rockLayerLow = (int)WorldGen.rockLayerLow;
            int OreAmmount;

            OreAmmount = (int)(maxtiles * 0.00007); // 1/1,250
            for (int k = 0; k < OreAmmount; k++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next(rockLayerLow, Main.maxTilesY);
                WorldGen.TileRunner(x, y, WorldGen.genRand.Next(5, 6), WorldGen.genRand.Next(5, 7), ModContent.TileType<DalantiniumOreTile>());
            }

            OreAmmount = (int)(maxtiles * 0.00008); // 1/12,500
            for (int k = 0; k < OreAmmount; k++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next(rockLayerLow, Main.maxTilesY);
                WorldGen.TileRunner(x, y, WorldGen.genRand.Next(5, 6), WorldGen.genRand.Next(5, 7), ModContent.TileType<HydriteOreTile>());

                x = WorldGen.genRand.Next(0, Main.maxTilesX);
                y = WorldGen.genRand.Next(rockLayerLow, Main.maxTilesY);
                WorldGen.TileRunner(x, y, WorldGen.genRand.Next(5, 6), WorldGen.genRand.Next(5, 7), ModContent.TileType<LythenOreTile>());

                x = WorldGen.genRand.Next(0, Main.maxTilesX);
                y = WorldGen.genRand.Next(rockLayerLow, Main.maxTilesY);
                WorldGen.TileRunner(x, y, WorldGen.genRand.Next(5, 6), WorldGen.genRand.Next(5, 7), ModContent.TileType<HydroFluorideOreTile>());
            }
        }
    }
}
