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

namespace EEMod
{
    public class EESubWorlds
    {
        [FieldInit] public static IList<Vector2> ChainConnections = new List<Vector2>();
        [FieldInit] public static IList<Vector3> MinibiomeLocations = new List<Vector3>();
        [FieldInit] public static IList<Vector2> OrbPositions = new List<Vector2>();
        [FieldInit] public static IList<Vector2> BulbousTreePosition = new List<Vector2>();
        [FieldInit] public static IList<Vector2> CoralCrystalPosition = new List<Vector2>();
        [FieldInit] public static IList<Vector2> AquamarineZiplineLocations = new List<Vector2>();
        [FieldInit] public static IList<Vector2> GiantKelpRoots = new List<Vector2>();
        [FieldInit] public static IList<Vector2> WebPositions = new List<Vector2>();
        public static Vector2 CoralBoatPos;
        public static Vector2 SpirePosition;
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

        public static void Sea(int seed, GenerationProgress customProgressObject = null)
        {
            Main.maxTilesX = 400;
            Main.maxTilesY = 405;
            Main.spawnTileX = 234;
            Main.spawnTileY = 92;
            SubworldManager.Reset(seed);
            SubworldManager.PostReset(customProgressObject);
            EEMod.isSaving = false;
        }

        public static void CoralReefs(int seed, GenerationProgress customProgressObject = null)
        {
            EEMod.progressMessage = "Generating Coral Reefs";
            //Variables and Initialization stuff
            int roomCount = 20;
            Vector2[] roomsUp = new Vector2[roomCount];
            Vector2[] roomsDown = new Vector2[roomCount];
            Main.maxTilesX = 1500;
            Main.maxTilesY = 2400;

            int depth = Main.maxTilesY / 20;
            int boatPos = 300;

            SubworldManager.Reset(seed);
            SubworldManager.PostReset(customProgressObject);


            //Placing initial blocks
            #region Initial block placement
            EEMod.progressMessage = "Initial generation";
            FillRegion(Main.maxTilesX, (Main.maxTilesY / 10) * 4, Vector2.Zero, ModContent.TileType<LightGemsandTile>());

            FillRegion(Main.maxTilesX, (Main.maxTilesY / 10) * 3, new Vector2(0, (Main.maxTilesY / 10) * 4), ModContent.TileType<GemsandTile>());

            FillRegion(Main.maxTilesX, (Main.maxTilesY / 10) * 3, new Vector2(0, (Main.maxTilesY / 10) * 7), ModContent.TileType<DarkGemsandTile>());

            ClearRegion(Main.maxTilesX, (Main.maxTilesY / 20) + (Main.maxTilesY / 60) + (Main.maxTilesY / 60), Vector2.Zero);

            FillRegionNoEditWithNoise(Main.maxTilesX, Main.maxTilesY / 60, new Vector2(0, (Main.maxTilesY / 60) + (Main.maxTilesY / 20)), ModContent.TileType<CoralSandTile>(), 20);
            FillRegionEditWithNoise(Main.maxTilesX, (Main.maxTilesY / 60) - (Main.maxTilesY / 120), new Vector2(0, (Main.maxTilesY / 60) + (Main.maxTilesY / 20) + 20), ModContent.TileType<CoralsandstoneTile>(), 20);

            FillRegionEditWithNoise(Main.maxTilesX, (Main.maxTilesY / 60) - (Main.maxTilesY / 120), new Vector2(0, (Main.maxTilesY / 60) + (Main.maxTilesY / 20) + 40), ModContent.TileType<LightGemsandTile>(), 20);

            /*List<Vector3> vecs = new List<Vector3>();

            for (int i = 50; i < Main.maxTilesX - 50; i++)
            {
                if (WorldGen.genRand.NextBool(50))
                {
                    int height = TileCheck(i, ModContent.TileType<CoralSandTile>());

                    if((height > TileCheck(i - 5, ModContent.TileType<CoralSandTile>()) && height > TileCheck(i + 5, ModContent.TileType<CoralSandTile>())) || (height < TileCheck(i - 5, ModContent.TileType<CoralSandTile>()) && height < TileCheck(i + 5, ModContent.TileType<CoralSandTile>())))
                    {
                        i += 6;
                        continue;
                    }

                    int polarity = TileCheck(i - 5, ModContent.TileType<CoralSandTile>()) < TileCheck(i + 5, ModContent.TileType<CoralSandTile>()) ? -1 : 1;

                    Vector2 slope = new Vector2(Main.rand.Next(1, 4) * polarity, Main.rand.NextFloat(0.5f, 1.25f));
                    int length = Main.rand.Next(6, 11);
                    int scale = Main.rand.Next(6, 11);

                    for(int k = -3; k < length; k++)
                    {
                        Vector2 vec = (slope * k * 1.5f);
                        MakeCircle(scale, new Vector2(i + (int)vec.X, height + (int)vec.Y - (scale / 2)), TileID.StoneSlab, true);

                        if (Main.rand.NextBool(3)) {
                            scale++;
                            slope.Y *= 0.75f;
                        }
                    }

                    vecs.Add(new Vector3(i, height, scale));
                    i += 55;
                }
            }

            Vector3[] finalVecs = vecs.ToArray();

            for(int i = 0; i < finalVecs.Length - 1; i++)
            {
                float dist = finalVecs[i + 1].X - finalVecs[i].X;
                if (dist < 40 && dist > 15)
                {
                    for (float k = 0; k < 1; k += 0.05f)
                    {
                        MakeCircle((int)(finalVecs[i].Z + k * (finalVecs[i + 1].Z - finalVecs[i].Z)), Vector2.Lerp(new Vector2(finalVecs[i].X, finalVecs[i].Y), new Vector2(finalVecs[i + 1].X, finalVecs[i + 1].Y), k), TileID.StoneSlab, true);
                    }
                }
            }*/

            #endregion

            #region Finding suitable chasm positions and room positions
            int maxTiles = (int)(Main.maxTilesX * Main.maxTilesY * 9E-04);
            EEMod.progressMessage = "Finding Suitable Chasm Positions";


            Vector2 size = new Vector2(Main.maxTilesX - 300, Main.maxTilesY / 20);
            //NoiseGenWave(new Vector2(300, 80), size, new Vector2(20, 100), (ushort)ModContent.TileType<CoralSandTile>(), 0.5f);
            //NoiseGenWave(new Vector2(300, 60), size, new Vector2(50, 50), TileID.StoneSlab, 0.6f);
            int[] roomGen = Helpers.FillPseudoRandomUniform<int>(4);
            int[] roomGen2 = Helpers.FillPseudoRandomUniform<int>(4);

            //Placing water and etc
            Vector2[] Rooms = MakeDistantLocations(roomCount, 150, new Rectangle(200, 300, Main.maxTilesX - 400, Main.maxTilesY / 2 - 100), 1000);
            List<Vector2> Buffer = new List<Vector2>();

            for (int i = 0; i < roomsUp.Length - 1; i++)
            {
                if (i == 0)
                {
                    roomsUp[i] = new Vector2(300, 500);
                }
                else
                {
                    roomsUp[i] = Rooms[i];
                    int sizeOfChasm = WorldGen.genRand.Next(100, 200);
                    void PlaceRoom(int biome)
                    {
                        MakeCoralRoom((int)roomsUp[i].X + sizeOfChasm / 2, (int)roomsUp[i].Y + sizeOfChasm / 4, sizeOfChasm, biome);
                        MinibiomeLocations.Add(new Vector3((int)roomsUp[i].X + sizeOfChasm / 2, (int)roomsUp[i].Y + sizeOfChasm / 4, biome));
                        
                    }
                    if (i > 3)
                    {
                        PlaceRoom(roomGen2[i % 4]);
                    }
                    else
                    {
                        PlaceRoom(roomGen[i]);
                    }
                }
                Buffer.Add(roomsUp[i]);
            }
            for (int i = 0; i < roomsUp.Length - 1; i++)
            {
                if (i != 0)
                {
                    Vector2 closest = FindClosest(roomsUp[i], Buffer.ToArray());
                    MakeWavyChasm3(roomsUp[i], closest, TileID.StoneSlab, 100, WorldGen.genRand.Next(10, 20), true, new Vector2(10, 20), WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(5, 10), true, 51, WorldGen.genRand.Next(80, 120));
                }
            }
            
            try
            {
                //Making chasms
                for (int i = 0; i < roomsUp.Length; i++)
                {
                    /*int sizeOfChasm = WorldGen.genRand.Next(100, 200);

                    if (i == 0)
                    {
                        roomsUp[i] = new Vector2(200, 500);
                    }
                    else
                    {
                        int score;
                        int breakLoop = 0;
                        float randPosX;
                        float randPosY;
                        int distance = 250;
                        do
                        {
                            breakLoop++;
                            score = 0;
                            int Left = Helpers.Clamp((int)roomsUp[i - 1].X - distance, 200, Main.maxTilesX - 200);
                            int Right = Helpers.Clamp((int)roomsUp[i - 1].X + distance, 200, Main.maxTilesX - 200);
                            randPosX = WorldGen.genRand.Next(Left, Right);
                            randPosY = MathHelper.Clamp(WorldGen.genRand.Next((int)roomsUp[i - 1].Y - distance, (int)roomsUp[i - 1].Y + distance), Main.maxTilesY / 10, Main.maxTilesY);
                            float f = sizeOfChasm * 1.5f;
                            float ff = f * f;
                            for (int k = 0; k < i; k++)
                            {
                                if (Vector2.DistanceSquared(new Vector2(randPosX, randPosY), roomsUp[k]) < ff)
                                {
                                    score++;
                                }
                            }
                            EEMod.progressMessage = "Finding Suitable Chasm Positions " + breakLoop + " try";
                            if (breakLoop > 6000)
                            {
                                break;
                            }
                        } while (score != 0
                        || randPosX < sizeOfChasm * 2f
                        || randPosX > Main.maxTilesX - (sizeOfChasm * 1.2f)
                        || randPosY < sizeOfChasm
                        || randPosY > Main.maxTilesY / 2
                        || Vector2.DistanceSquared(new Vector2(randPosX, randPosY), new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2)) < 100 * 100
                        || Vector2.DistanceSquared(new Vector2(randPosX, randPosY), new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 + 400)) < 100 * 100
                        || Vector2.DistanceSquared(new Vector2(randPosX, randPosY), new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 - 400)) < 100 * 100);
                        roomsUp[i] = new Vector2(Helpers.Clamp(randPosX, 200, Main.maxTilesX - 200), randPosY);

<<<<<<< Updated upstream
                        int biome = i > 3 ? roomGen2[i % 4] : roomGen[i];

                        // Place room

                        MakeCoralRoom((int)roomsUp[i].X + sizeOfChasm / 2, (int)roomsUp[i].Y + sizeOfChasm / 4, sizeOfChasm, biome);
                        MinibiomeLocations.Add(new Vector3((int)roomsUp[i].X + sizeOfChasm / 2, (int)roomsUp[i].Y + sizeOfChasm / 4, biome));
                        if (i != 0)
=======
                        void PlaceRoom(int biome)
                        {
                            MakeCoralRoom((int)roomsUp[i].X + sizeOfChasm / 2, (int)roomsUp[i].Y + sizeOfChasm / 4, sizeOfChasm, biome);
                            MinibiomeLocations.Add(new Vector3((int)roomsUp[i].X + sizeOfChasm / 2, (int)roomsUp[i].Y + sizeOfChasm / 4, biome));
                            if (i != 0)
                            {
                                MakeWavyChasm3(roomsUp[i], roomsUp[i - 1], TileID.StoneSlab, 100, WorldGen.genRand.Next(10, 20), true, new Vector2(10, 20), WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(5, 10), true, 51, WorldGen.genRand.Next(80, 120));
                            }
                        }
                        if (i > 3)
                        {
                            PlaceRoom(roomGen2[i % 4]);
                        }
                        else
                        {
                            MakeWavyChasm3(roomsUp[i], roomsUp[i - 1], TileID.StoneSlab, 100, WorldGen.genRand.Next(10, 20), true, new Vector2(20, 40), WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(5, 10), true, 51, WorldGen.genRand.Next(80, 120));
                        }

                    }
                    }*/
                }
                

                for (int i = 0; i < roomsDown.Length; i++)
                {
                    int sizeOfChasm = WorldGen.genRand.Next(100, 200);
                    if (i == 0)
                    {
                        roomsDown[i] = new Vector2(400, 1300);
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
                            randPosX = WorldGen.genRand.Next(Helpers.Clamp((int)roomsDown[i - 1].X - distance, 200, Main.maxTilesX - 200), Helpers.Clamp((int)roomsDown[i - 1].X + distance, 200, Main.maxTilesX - 200));
                            randPosY = MathHelper.Clamp(WorldGen.genRand.Next((int)roomsDown[i - 1].Y - distance, (int)roomsDown[i - 1].Y + distance), Main.maxTilesY / 10, Main.maxTilesY);
                            float f = sizeOfChasm * 1.4f;
                            float ff = f * f;
                            for (int k = 0; k < i; k++)
                            {
                                if (Vector2.DistanceSquared(new Vector2(randPosX, randPosY), roomsDown[k]) < ff)
                                {
                                    score++;
                                }
                            }
                            EEMod.progressMessage = "Finding Suitable Chasm Positions " + breakLoop + " try";
                            if (breakLoop > 6000)
                            {
                                break;
                            }
                        } while (score != 0
                        || randPosX > Main.maxTilesX - (sizeOfChasm * 1.2f)
                        || randPosX < sizeOfChasm * 1.2f
                        || randPosY < Main.maxTilesY * 0.5f
                        || randPosY > Main.maxTilesY * 0.83f
                        || Vector2.DistanceSquared(new Vector2(randPosX, randPosY), new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2)) < 300 * 300
                        || Vector2.DistanceSquared(new Vector2(randPosX, randPosY), new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 + 400)) < 300 * 300
                        || Vector2.DistanceSquared(new Vector2(randPosX, randPosY), new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 - 400)) < 300 * 300);
                        roomsDown[i] = new Vector2(Helpers.Clamp(randPosX, 200, Main.maxTilesX - 200), randPosY);

                        void PlaceRoom(int biome)
                        {
                            MakeCoralRoom((int)roomsDown[i].X + sizeOfChasm / 2, (int)roomsDown[i].Y + sizeOfChasm / 4, sizeOfChasm, biome);
                            MinibiomeLocations.Add(new Vector3((int)roomsDown[i].X + sizeOfChasm / 2, (int)roomsDown[i].Y + sizeOfChasm / 4, biome));
                            if (i != 0)
                            {
                                MakeWavyChasm3(roomsDown[i], roomsDown[i - 1], TileID.StoneSlab, 100, WorldGen.genRand.Next(10, 20), true, new Vector2(10, 20), WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(5, 10), true, 51, WorldGen.genRand.Next(80, 120));
                            }
                        }
                        if (i > 3)
                        {
                            int legoYodaTheSequel = roomGen2[i % 4];
                            if (legoYodaTheSequel != 0) legoYodaTheSequel += 3;
                            PlaceRoom(legoYodaTheSequel);
                        }
                        else
                        {
                            int legoYodaTheSequel = roomGen[i];
                            if (legoYodaTheSequel != 0) legoYodaTheSequel += 3;
                            PlaceRoom(legoYodaTheSequel);
                        }
                    }

                }

                EEMod.progressMessage = "Genning Rooms";


                MakeCoralRoom(Main.maxTilesX / 2, Main.maxTilesY / 2, 400, WorldGen.genRand.Next(4), true);
                MinibiomeLocations.Add(new Vector3(Main.maxTilesX / 2, Main.maxTilesY / 2, 0));
                MakeCoralRoom(Main.maxTilesX / 2, Main.maxTilesY / 2 + 400, 400, 0, false);
                MinibiomeLocations.Add(new Vector3(Main.maxTilesX / 2, Main.maxTilesY / 2 + 400, 0));


                Vector2[] chosen = { Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero };
                for (int i = 0; i < roomsUp.Length; i++)
                {
                    if (chosen[0] == Vector2.Zero || Vector2.DistanceSquared(roomsUp[i], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2)) <
                        Vector2.DistanceSquared(chosen[0], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2)))
                    {
                        chosen[0] = roomsUp[i];
                    }
                    if (chosen[1] == Vector2.Zero || Vector2.DistanceSquared(roomsDown[i], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2)) <
                        Vector2.DistanceSquared(chosen[1], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2)))
                    {
                        chosen[1] = roomsDown[i];
                    }
                    if (chosen[2] == Vector2.Zero || Vector2.DistanceSquared(roomsUp[i], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 - 400)) <
                        Vector2.DistanceSquared(chosen[2], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 - 400)))
                    {
                        chosen[2] = roomsUp[i];
                    }
                    if (chosen[3] == Vector2.Zero || Vector2.DistanceSquared(roomsDown[i], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 - 400)) <
                        Vector2.DistanceSquared(chosen[3], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 - 400)))
                    {
                        chosen[3] = roomsDown[i];
                    }
                    if (chosen[4] == Vector2.Zero || Vector2.DistanceSquared(roomsUp[i], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 + 400)) <
                       Vector2.DistanceSquared(chosen[4], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 + 400)))
                    {
                        chosen[4] = roomsUp[i];
                    }
                    if (chosen[5] == Vector2.Zero || Vector2.DistanceSquared(roomsDown[i], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 + 400)) <
                        Vector2.DistanceSquared(chosen[5], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 + 400)))
                    {
                        chosen[5] = roomsDown[i];
                    }
                }

                #endregion

                #region Making chasms
                EEMod.progressMessage = "Making Wavy Chasms"; //I sense OPTIMIZATION
                for (int i = 0; i < 2; i++)
                {
                    MakeWavyChasm3(chosen[i], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2), TileID.StoneSlab, 100, 10, true, new Vector2(20, 40), WorldGen.genRand.Next(10, 50), WorldGen.genRand.Next(2, 5));
                }

                for (int i = 2; i < 4; i++)
                {
                    MakeWavyChasm3(chosen[i], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 - 400), TileID.StoneSlab, 100, 10, true, new Vector2(20, 40), WorldGen.genRand.Next(10, 50), WorldGen.genRand.Next(2, 5));
                }

                for (int i = 4; i < 6; i++)
                {
                    MakeWavyChasm3(chosen[i], new Vector2(Main.maxTilesX / 2, Main.maxTilesY / 2 + 400), TileID.StoneSlab, 100, 10, true, new Vector2(20, 40), WorldGen.genRand.Next(10, 50), WorldGen.genRand.Next(2, 5));
                }

                Vector2 highestRoom = new Vector2(0, 3000);
                foreach (Vector2 legoYoda in roomsUp)
                {
                    if (legoYoda.Y < highestRoom.Y)
                    {
                        highestRoom = legoYoda;
                    }
                }

                MakeWavyChasm3(highestRoom, new Vector2(highestRoom.X + WorldGen.genRand.Next(-100, 101), 100), TileID.StoneSlab, 100, 10, true, new Vector2(20, 40));
                #endregion

                MakeLayer(Main.maxTilesX / 2, Main.maxTilesY / 2 - 400, 100, 1, ModContent.TileType<GemsandTile>());

                RemoveStoneSlabs();

                #region Placing ores
                EEMod.progressMessage = "Generating Ores";
                //Generating ores
                /*int barrier = 800;
                for (int j = Main.maxTilesY / 10; j < barrier; j++)
                {
                    for (int i = 0; i < Main.maxTilesX; i++)
                    {
                        if (GemsandCheck(i, j))
                        {
                            if (WorldGen.genRand.NextBool(2000))
                            {
                                WorldGen.TileRunner(i, j, WorldGen.genRand.Next(4, 8), WorldGen.genRand.Next(5, 7), ModContent.TileType<LythenOreTile>());
                            }
                        }
                    }
                }*/
                #endregion

                #region Shipwrecks
                EEMod.progressMessage = "Generating Shipwrecks";
                int mlem = 0;
                while (mlem < 3)
                {
                    int tileX = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
                    int tileY = TileCheck(tileX, ModContent.TileType<CoralSandTile>());
                    if (Math.Abs(TileCheck(tileX + Ship1.GetLength(1), ModContent.TileType<CoralSandTile>()) - tileY) <= 2)
                    {
                        PlaceStructure(tileX, tileY - Ship1.GetLength(0), Ship1);
                        mlem++;
                    }
                    else
                    {
                        continue;
                    }
                }
                #endregion

                #region Remaining generation




                perlinNoise = new PerlinNoiseFunction(Main.maxTilesX, (int)(Main.maxTilesY * 0.9f), 50, 50, 0.4f);
                int[,] perlinNoiseFunction = perlinNoise.perlinBinary;
                for (int i = 42; i < Main.maxTilesX - 42; i++)
                {
                    for (int j = (Main.maxTilesY / 10); j < Main.maxTilesY - 42; j++)
                    {
                        if (perlinNoiseFunction[i, j - (Main.maxTilesY / 10)] == 1)
                        {
                            Tile tile = Framing.GetTileSafely(i, j - (Main.maxTilesY / 10));
                            if (tile.type == ModContent.TileType<LightGemsandTile>())
                                tile.type = (ushort)ModContent.TileType<LightGemsandstoneTile>();
                            else if (tile.type == ModContent.TileType<GemsandTile>())
                                tile.type = (ushort)ModContent.TileType<GemsandstoneTile>();
                            else if (tile.type == ModContent.TileType<DarkGemsandTile>())
                                tile.type = (ushort)ModContent.TileType<DarkGemsandstoneTile>();
                        }
                    }
                }

                //BLOODY DREAD AY MATE? BISCUITS AND CRUMPETS AND BLOODY TEA!! (hi os)

                perlinNoise = new PerlinNoiseFunction(Main.maxTilesX, (int)(Main.maxTilesY * 0.9f), 50, 50, 0.8f);
                int[,] perlinNoiseFunction2 = perlinNoise.perlinBinary;
                for (int i = 42; i < Main.maxTilesX - 42; i++)
                {
                    for (int j = (Main.maxTilesY / 10); j < Main.maxTilesY - 42; j++)
                    {
                        if (perlinNoiseFunction2[i, j - (Main.maxTilesY / 10)] == 1)
                        {
                            int minibiome = 0;
                            List<float> BufferLengths = new List<float>();
                            List<int> BufferMinibiome = new List<int>();
                            for (int k = 0; k < MinibiomeLocations.Count; k++)
                            {
                                if (Vector2.DistanceSquared(new Vector2(MinibiomeLocations[k].X, MinibiomeLocations[k].Y), new Vector2(i, j)) < (180 * 180) && MinibiomeLocations[k].Z != 0)
                                {
                                    BufferLengths.Add(Vector2.DistanceSquared(new Vector2(MinibiomeLocations[k].X, MinibiomeLocations[k].Y), new Vector2(i, j)));
                                    BufferMinibiome.Add((int)MinibiomeLocations[k].Z);
                                }
                            }
                            float MakingMyWayDownTown = -1;
                            int WalkingFast = -1;
                            for (int a = 0; a < BufferLengths.Count; a++)
                            {
                                if (BufferLengths[a] < MakingMyWayDownTown || MakingMyWayDownTown == -1)
                                {
                                    MakingMyWayDownTown = BufferLengths[a];
                                    WalkingFast = BufferMinibiome[a];
                                }
                            }
                            if (WalkingFast != -1) minibiome = WalkingFast;


                           /* if ((Framing.GetTileSafely(i, j).type == ModContent.TileType<LightGemsandstoneTile>()
                                || Framing.GetTileSafely(i, j).type == ModContent.TileType<KelpLeafTile>()) &&
                                (MinibiomeID)minibiome == MinibiomeID.KelpForest && WorldGen.genRand.Next(4) == 0)
                                Framing.GetTileSafely(i, j).type = (ushort)ModContent.TileType<KelpMossTile>();*/
                            if (Framing.GetTileSafely(i, j).type == ModContent.TileType<GemsandTile>() && (MinibiomeID)minibiome == MinibiomeID.ThermalVents)
                                Framing.GetTileSafely(i, j).type = (ushort)ModContent.TileType<ThermalMossTile>();
                        }
                    }
                }
                //Final polishing
                /*EEMod.progressMessage = "Tidying the world";
                for (int i = 42; i < Main.maxTilesX - 42; i++)
                {
                    for (int j = 42; j < Main.maxTilesY - 42; j++)
                    {
                        if(!Framing.GetTileSafely(i, j + 1).active() && !Framing.GetTileSafely(i, j - 1).active() && !Framing.GetTileSafely(i + 1, j).active() && !Framing.GetTileSafely(i - 1, j).active())
                        {
                            WorldGen.KillTile(i, j);
                        }
                    }
                }*/
                #endregion

                #region Implementing dynamic objects
                EEMod.progressMessage = "Adding Dynamics";
                for (int j = 42; j < Main.maxTilesY - 42; j += 5)
                {
                    for (int i = 42; i < Main.maxTilesX - 42; i += 5)
                    {
                        int noOfTiles = 0;
                        if (j > 200)
                        {
                            for (int k = -11; k < 11; k++)
                            {
                                for (int l = -11; l < 11; l++)
                                {
                                    if (Framing.GetTileSafely(i + k, j + l).active())
                                    {
                                        noOfTiles++;
                                    }
                                }
                            }
                            for (int m = 0; m < OrbPositions.Count; m++)
                            {
                                if (Vector2.DistanceSquared(new Vector2(i, j), OrbPositions[m]) < 200 * 200)
                                {
                                    noOfTiles++;
                                }
                            }
                            if (noOfTiles <= 2)
                            {
                                OrbPositions.Add(new Vector2(i, j));
                            }
                            if (noOfTiles <= 30)
                            {
                                int dist = 0;
                                for (int m = 0; m < OrbPositions.Count; m++)
                                {
                                    if (Vector2.DistanceSquared(new Vector2(i, j), OrbPositions[m]) < 200 * 200)
                                    {
                                        dist++;
                                    }
                                }
                                for (int m = 0; m < WebPositions.Count; m++)
                                {
                                    if (Vector2.DistanceSquared(new Vector2(i, j), WebPositions[m]) < 200 * 200)
                                    {
                                        dist++;
                                    }
                                }
                                if (dist == 0)
                                {
                                    //WebPositions.Add(new Vector2(i, j));
                                }
                            }
                        }

                        int ifa = 0;
                        for (int m = 0; m < BulbousTreePosition.Count; m++)
                        {
                            if (Vector2.DistanceSquared(new Vector2(i, j), BulbousTreePosition[m]) < 20 * 20)
                            {
                                ifa++;
                            }
                        }
                        for (int m = 0; m < AquamarineZiplineLocations.Count; m++)
                        {
                            if (Vector2.DistanceSquared(new Vector2(i, j), AquamarineZiplineLocations[m]) < 20 * 20)
                            {
                                ifa++;
                            }
                        }
                        
                            if ((TileCheck2(i, j) == 3 || TileCheck2(i, j) == 4) && WorldGen.genRand.Next(2) == 0 /*&& GemsandCheck(i, j)*/ && j > Main.maxTilesY / 10)
                            {
                                if (ChainConnections.Count == 0)
                                {
                                    ChainConnections.Add(new Vector2(i, j));
                                }
                                else
                                {
                                    Vector2 lastPos = ChainConnections[ChainConnections.Count - 1];
                                    if (Vector2.DistanceSquared(lastPos, new Vector2(i, j)) > 5 * 5 && 
                                        Vector2.DistanceSquared(lastPos, new Vector2(i, j)) < 55 * 55 || 
                                        Vector2.DistanceSquared(lastPos, new Vector2(i, j)) > 150 * 150)
                                    {
                                        ChainConnections.Add(new Vector2(i, j));
                                    }
                                }
                            }
                        
                    }
                }
                #endregion

                EEMod.progressMessage = "Inserting foes";
                Vector2 pos1 = new Vector2(SpirePosition.X + 10, SpirePosition.Y - 150 / 2);
                Vector2 pos2 = new Vector2(SpirePosition.X + 10, SpirePosition.Y + 150 / 2);
                int tile2 = 0;
                tile2 = GetGemsandType((int)pos1.Y);
                MakeExpandingChasm(pos1, pos2, tile2, 100, -2, true, new Vector2(20, 30), .5f);
                MakeExpandingChasm(pos2, pos1, tile2, 100, -2, true, new Vector2(20, 30), .5f);
                ClearRegion(46, 26, new Vector2(SpirePosition.X + 10 - 24, SpirePosition.Y - 26));
                MakeWavyChasm3(new Vector2(SpirePosition.X - 5, SpirePosition.Y - 26), new Vector2(SpirePosition.X + 25, SpirePosition.Y - 26), tile2, 20, -2, true, new Vector2(1, 5));
                MakeWavyChasm3(new Vector2(SpirePosition.X - 5, SpirePosition.Y), new Vector2(SpirePosition.X + 25, SpirePosition.Y), tile2, 20, -2, true, new Vector2(1, 5));

                EEMod.progressMessage = "Placing Corals";
                PlaceCoral();

                #region Smoothing
                EEMod.progressMessage = "Smoothing";
                for (int i = 2; i < Main.maxTilesX - 2; i++)
                {
                    for (int j = 2; j < Main.maxTilesY - 2; j++)
                    {
                        if (WorldGen.genRand.NextBool(4) && Framing.GetTileSafely(i, j).type != ModContent.TileType<BulbousBlockTile>())
                        {
                            Tile.SmoothSlope(i, j);
                        }
                        if (!Framing.GetTileSafely(i, j + 1).active() && !Framing.GetTileSafely(i, j - 1).active() && !Framing.GetTileSafely(i + 1, j).active() && !Framing.GetTileSafely(i - 1, j).active())
                        {
                            WorldGen.KillTile(i, j);
                        }
                    }
                }
                #endregion

                EEMod.progressMessage = "Final touches";
                FillRegionWithWater(Main.maxTilesX, Main.maxTilesY - depth, new Vector2(0, depth));
                PlaceWallGrass();

                for (int i = 2; i < Main.maxTilesX - 2; i++)
                {
                    for (int j = 2; j < Main.maxTilesY - 2; j++)
                    {
                        if (Framing.GetTileSafely(i, j).wall == WallID.Dirt || Framing.GetTileSafely(i, j).wall == WallID.DirtUnsafe || Framing.GetTileSafely(i, j).wall == WallID.DirtUnsafe1 || Framing.GetTileSafely(i, j).wall == WallID.DirtUnsafe2 || Framing.GetTileSafely(i, j).wall == WallID.DirtUnsafe3 || Framing.GetTileSafely(i, j).wall == WallID.DirtUnsafe4)
                        {
                            WorldGen.KillWall(i, j);
                        }
                    }
                }

                #region Placing the boat

                PlaceShipWalls(boatPos, TileCheckWater(boatPos) - 22, ShipWalls);
                PlaceShip(boatPos, TileCheckWater(boatPos) - 22, ShipTiles);
                CoralBoatPos = new Vector2(boatPos, TileCheckWater(boatPos) - 22);

                RemoveWaterFromRegion(ShipTiles.GetLength(1), ShipTiles.GetLength(0), new Vector2(boatPos, TileCheckWater(boatPos) - 22));

                #endregion
            }
            catch (Exception e)
            {
                EEMod.progressMessage = "Unsuccessful!";
                EEMod.progressMessage = e.ToString();
                SubworldManager.PreSaveAndQuit();
                return;
            }
            //Finishing initialization stuff
            EEMod.progressMessage = "Successful!";
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