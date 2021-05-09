using EEMod.ID;
using EEMod.Tiles;
using EEMod.Tiles.EmptyTileArrays;
using EEMod.Tiles.Foliage;
using EEMod.Tiles.Foliage.Coral;
using EEMod.Tiles.Foliage.Coral.HangingCoral;
using EEMod.Tiles.Foliage.Coral.WallCoral;
using EEMod.Tiles.Ores;
using EEMod.Tiles.Walls;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Tiles.Furniture;
using EEMod.Tiles.Foliage.ThermalVents;
using EEMod.Tiles.Foliage.KelpForest;
using EEMod.Tiles.Foliage.Aquamarine;

namespace EEMod.EEWorld
{
    public partial class EEWorld
    {
        public static PerlinNoiseFunction perlinNoise;

        internal static void PlaceWallGrass()
        {
            for (int i = 10; i < Main.maxTilesX - 10; i++)
            {
                for (int j = 10; j < Main.maxTilesY - 10; j++)
                {
                    int X = i;
                    int Y = j;
                    switch (TileCheck2(X, Y))
                    {
                        case (int)TileSpacing.Top:
                        {
                            for (int a = 0; a < WorldGen.genRand.Next(11); a++)
                                WorldGen.PlaceWall(X, Y - a, ModContent.WallType<KelpForestLeafyWall>());
                            break;
                        }
                        case (int)TileSpacing.Bottom:
                        {
                            for (int a = 0; a < WorldGen.genRand.Next(11); a++)
                                WorldGen.PlaceWall(X, Y + a, ModContent.WallType<KelpForestLeafyWall>());
                            break;
                        }
                        case (int)TileSpacing.Left:
                        {
                            for (int a = 0; a < WorldGen.genRand.Next(11); a++)
                                WorldGen.PlaceWall(X - a, Y, ModContent.WallType<KelpForestLeafyWall>());
                            break;
                        }
                        case (int)TileSpacing.Right:
                        {
                            for (int a = 0; a < WorldGen.genRand.Next(11); a++)
                                WorldGen.PlaceWall(X + a, Y, ModContent.WallType<KelpForestLeafyWall>());
                            break;
                        }
                    }
                }
            }
        }

        public static void MakeCoralRoom(int xPos, int yPos, int sizeX, int sizeY, int type, bool ensureNoise = false)
        {
            Vector2 TL = new Vector2(xPos - sizeX / 2f, yPos - sizeY / 2f);
            Vector2 BR = new Vector2(xPos + sizeX / 2f, yPos + sizeY / 2f);

            Vector2 startingPoint = new Vector2(xPos - sizeX, yPos - sizeY);
            int tile2;
            tile2 = (ushort)GetGemsandType((int)TL.Y);
            void CreateNoise(bool ensureN, int width, int height, float thresh)
            {
                perlinNoise = new PerlinNoiseFunction(1000, 1000, width, height, thresh);
                int[,] perlinNoiseFunction = perlinNoise.perlinBinary;
                if (ensureN)
                {
                    for (int i = (int)startingPoint.X; i < (int)startingPoint.X + sizeX * 2; i++)
                    {
                        for (int j = (int)startingPoint.Y; j < (int)startingPoint.Y + sizeY * 2; j++)
                        {
                            if (i > 0 && i < Main.maxTilesX && j > 0 && j < Main.maxTilesY)
                            {
                                if (i - (int)startingPoint.X < 1000 && j - (int)startingPoint.Y < 1000)
                                {
                                    if (perlinNoiseFunction[i - (int)startingPoint.X, j - (int)startingPoint.Y] == 1 && OvalCheck(xPos, yPos, i, j, sizeX, sizeY) && WorldGen.InWorld(i, j))
                                    {
                                        Tile tile = Framing.GetTileSafely(i, j);
                                        tile.type = (ushort)GetGemsandType(j);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            RemoveStoneSlabs();

            switch (type) //Creating the formation of the room(the shape)
            {
                case -1:
                    MakeJaggedOval(sizeX, sizeY, new Vector2(TL.X, TL.Y), TileID.StoneSlab, true, 100);
                    MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos + 0, yPos + 0), tile2);
                    MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos + (-sizeX / 5 - sizeX / 6), yPos + (-sizeY / 5 - sizeY / 6)), tile2);
                    MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos + (sizeX / 5 - sizeX / 6), yPos + (-sizeY / 5 - sizeY / 6)), tile2);
                    MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos + (sizeX / 5 - sizeX / 6), yPos + (sizeY / 5 - sizeY / 6)), tile2);
                    MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos + (-sizeX / 5 - sizeX / 6), yPos + (sizeY / 5 - sizeY / 6)), tile2);
                    for (int i = (int)startingPoint.X + 20; i < (int)startingPoint.X + sizeX * 2 - 20; i++)
                    {
                        for (int j = (int)startingPoint.Y + 20; j < (int)startingPoint.Y + sizeY * 2 - 20; j++)
                        {
                            int buffer = 0;
                            for (int a = 0; a < 20; a++)
                            {
                                if (Framing.GetTileSafely(i, j - a).active())
                                {
                                    buffer++;
                                }
                            }
                            if (buffer < 17 && buffer > 3)
                            {
                                if (TileCheck2(i, j) == 1 && TileCheckVertical(i, j + 1, 1) - (j + 1) <= 50)
                                {
                                    for (int a = 0; a < TileCheckVertical(i, j + 1, 1) - (j + 1); a++)
                                    {
                                        if (Main.rand.Next(2) == 1)
                                        {
                                            WorldGen.PlaceWall(i, j + a, ModContent.WallType<GemsandstoneWallTile>());
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;

                case 0:
                    MakeJaggedOval(sizeX, sizeY, new Vector2(TL.X, TL.Y), TileID.StoneSlab, true, 100);
                    MakeJaggedOval(sizeX + 50, sizeY - 50, new Vector2(TL.X, TL.Y), TileID.StoneSlab, true, 100);
                    MakeJaggedOval(sizeX - 50, sizeY + 50, new Vector2(TL.X, TL.Y), TileID.StoneSlab, true, 100);
                    MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos + 0, yPos + 0), tile2);
                    MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos + (-sizeX / 5), yPos + (-sizeY / 5)), tile2);
                    MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos + (sizeX / 5), yPos + (-sizeY / 5)), tile2);
                    MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos + (sizeX / 5), yPos + (sizeY / 5)), tile2);
                    MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos + (-sizeX / 5), yPos + (sizeY / 5)), tile2);
                    CreateNoise(!ensureNoise, Main.rand.Next(20, 50), Main.rand.Next(20, 50), 0.3f);
                    CreateNoise(!ensureNoise, Main.rand.Next(20, 50), Main.rand.Next(20, 50), 0.3f);
                    for (int i = (int)startingPoint.X + 20; i < (int)startingPoint.X + sizeX * 2 - 20; i++)
                    {
                        for (int j = (int)startingPoint.Y + 20; j < (int)startingPoint.Y + sizeY * 2 - 20; j++)
                        {
                            int buffer = 0;
                            for (int a = 0; a < 14; a++)
                            {
                                if (WorldGen.InWorld(i, j - a, 10))
                                    if (Framing.GetTileSafely(i, j - a).active())
                                    {
                                        buffer++;
                                    }
                            }
                            if (buffer < 7)
                            {
                                if (TileCheck2(i, j) == 1 && TileCheckVertical(i, j + 1, 1) - (j + 1) <= 50)
                                {
                                    for (int a = 0; a < 50; a++)
                                    {
                                        if (Main.rand.Next(4) == 1)
                                        {
                                            WorldGen.PlaceWall(i, j + a, ModContent.WallType<GemsandstoneWallTile>());
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;

                case (int)MinibiomeID.KelpForest: //A normally shaped room cut out with noise
                    KelpForest kelpForest = new KelpForest
                    {
                        Position = TL.ToPoint(),
                        Size = new Point(sizeX * 2, sizeY * 2),
                        EnsureNoise = ensureNoise
                    };
                    kelpForest.StructureStep();
                    break;


                case (int)MinibiomeID.GlowshroomGrotto: //One medium-sized open room completely covered in bulbous blocks
                    GlowshroomGrotto GlowshroomGrotto = new GlowshroomGrotto
                    {
                        Position = TL.ToPoint(),
                        Size = new Point(sizeX * 2, sizeY * 2),
                        EnsureNoise = ensureNoise
                    };
                    GlowshroomGrotto.StructureStep();
                    break;

                case (int)MinibiomeID.ThermalVents: //A wide-open room with floating platforms that hold abandoned ashen houses with huge chasms in between
                    MakeJaggedOval(sizeX, (int)(sizeY * 1.5f), new Vector2(TL.X, yPos - sizeY), TileID.StoneSlab, true, 50);

                    for (int i = 0; i < 30; i++)
                    {
                        MakeCircle(WorldGen.genRand.Next(5, 20), new Vector2(TL.X + WorldGen.genRand.Next(sizeX), yPos - sizeY + WorldGen.genRand.Next(sizeY * 2)), TileID.StoneSlab, true);
                    }

                    for (int i = 0; i < 5; i++)
                    {
                        MakeOvalJaggedTop(WorldGen.genRand.Next(25, 50), WorldGen.genRand.Next(10, 30), new Vector2(TL.X + WorldGen.genRand.Next((int)(sizeX * 0.25f), (int)(sizeX * 0.75f)), TL.X + WorldGen.genRand.Next((int)(sizeY * 0.25f * 1.5f), (int)(sizeY * 0.75f * 1.5f))), ModContent.TileType<GemsandTile>());
                    }
                    break;

                case (int)MinibiomeID.CrystallineCaves: //Massive caves made with noise surrounding a central large room(where the spire is, if there's a spire)
                    MakeJaggedOval((int)(sizeX * 1.3f), sizeY, new Vector2(TL.X, TL.Y), TileID.StoneSlab, true, 50);
                    CreateNoise(true, 100, 20, 0.3f);
                    CreateNoise(true, 20, 100, 0.4f);
                    RemoveStoneSlabs();
                    for (int j = (int)startingPoint.Y; j < (int)startingPoint.Y + sizeY * 2; j++)
                    {
                        for (int i = (int)startingPoint.X; i < (int)startingPoint.X + sizeX * 2; i++)
                        {
                            //if ((TileCheck2(i, j) == 3 || TileCheck2(i, j) == 4) && Main.rand.Next(8) == 1)
                            if ((TileCheck2(i, j) != 0) && Main.rand.NextBool(10))
                            {
                                if (EESubWorlds.AquamarineZiplineLocations.Count == 0)
                                {
                                    EESubWorlds.AquamarineZiplineLocations.Add(new Vector2(i, j));
                                }
                                else
                                {
                                    Vector2 lastPos = EESubWorlds.AquamarineZiplineLocations[EESubWorlds.AquamarineZiplineLocations.Count - 1];
                                    if ((Vector2.DistanceSquared(lastPos, new Vector2(i, j)) > 10 * 10 && Vector2.DistanceSquared(lastPos, new Vector2(i, j)) < 210 * 210) || Vector2.DistanceSquared(lastPos, new Vector2(i, j)) > 200 * 200)
                                    {
                                        EESubWorlds.AquamarineZiplineLocations.Add(new Vector2(i, j));
                                    }
                                }
                            }
                        }
                    }
                    for (int i = (int)startingPoint.X; i < (int)startingPoint.X + sizeX * 2; i++)
                    {
                        for (int j = (int)startingPoint.Y; j < (int)startingPoint.Y + sizeY * 2; j++)
                           {
                            //if ((TileCheck2(i, j) == 3 || TileCheck2(i, j) == 4) && Main.rand.Next(8) == 1)
                            if ((TileCheck2(i, j) != 0) && Main.rand.NextBool(10))
                            {
                                if (EESubWorlds.AquamarineZiplineLocations.Count == 0)
                                {
                                    EESubWorlds.AquamarineZiplineLocations.Add(new Vector2(i, j));
                                }
                                else
                                {
                                    Vector2 lastPos = EESubWorlds.AquamarineZiplineLocations[EESubWorlds.AquamarineZiplineLocations.Count - 1];
                                    if ((Vector2.DistanceSquared(lastPos, new Vector2(i, j)) > 10 * 10 && Vector2.DistanceSquared(lastPos, new Vector2(i, j)) < 210 * 210) || Vector2.DistanceSquared(lastPos, new Vector2(i, j)) > 200 * 200)
                                    {
                                        EESubWorlds.AquamarineZiplineLocations.Add(new Vector2(i, j));
                                    }
                                }
                            }
                        }
                    }
                    EESubWorlds.SpirePosition = new Vector2(xPos, yPos);
                    break;
            }
            CreateNoise(ensureNoise, Main.rand.Next(30, 50), Main.rand.Next(20, 40), Main.rand.NextFloat(0.3f, 0.6f));
        }

        public static void MakeCrystal(int xPos, int yPos, int length, int width, int vertDir, int horDir, int type)
        {
            for (int a = 0; a < length; a++)
            {
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (!Framing.GetTileSafely(i, j).active())
                        {
                            WorldGen.TileRunner(i + xPos + (a * horDir), j + yPos + (a * vertDir), Main.rand.Next(2, 3), Main.rand.Next(1, 2), type, true, 0, 0, false, false);
                        }
                    }
                }
            }
        }

        public static int GetGemsandType(int height)
        {
            if (height < Main.maxTilesY * 0.4f)
                return ModContent.TileType<LightGemsandTile>();
            else if (height < Main.maxTilesY * 0.8f)
                return ModContent.TileType<GemsandTile>();
            else if (height > Main.maxTilesY * 0.8f)
                return ModContent.TileType<DarkGemsandTile>();
            if (height < Main.maxTilesY / 20)
                return ModContent.TileType<CoralSandTile>();
            else
                return 0;
        }

        public static void MakeCoralStack()
        {

        }

        public static void PlaceCoral()
        {
            #region Surface Reefs

            TilePopulate(
            new int[] { ModContent.TileType<Hanging1x2Coral>(),
            ModContent.TileType<Hanging1x3Coral>(),
            ModContent.TileType<Hanging2x3Coral>(),
            ModContent.TileType<Hanging2x4Coral>(),
            ModContent.TileType<Hanging1x4Coral>(),

            ModContent.TileType<Floor1x1Coral>(),
            ModContent.TileType<Floor1x2Coral>(),
            ModContent.TileType<Floor2x1Coral>(),
            ModContent.TileType<Floor2x2Coral>(),
            ModContent.TileType<FloorGlow2x2Coral>(),
            ModContent.TileType<Floor2x6Coral>(),
            ModContent.TileType<Floor3x2Coral>(),
            ModContent.TileType<Floor3x3Coral>(),
            ModContent.TileType<Floor4x3Coral>(),
            ModContent.TileType<Floor7x7Coral>(),
            ModContent.TileType<Floor8x7Coral>(),
            ModContent.TileType<Floor8x3Coral>(),
            ModContent.TileType<FloorGlow9x4Coral>(),
            ModContent.TileType<Floor9x9Coral>(),
            ModContent.TileType<Floor11x11Coral>(),

            ModContent.TileType<Wall2x2CoralL>(),
            ModContent.TileType<Wall3x2CoralL>(),
            ModContent.TileType<Wall3x2NonsolidCoralL>(),
            ModContent.TileType<Wall5x2NonsolidCoralL>(),
            ModContent.TileType<Wall6x3CoralL>(),

            ModContent.TileType<Wall2x2CoralR>(),
            ModContent.TileType<Wall3x2CoralR>(),
            ModContent.TileType<Wall3x2NonsolidCoralR>(),
            ModContent.TileType<Wall5x2NonsolidCoralR>(),
            ModContent.TileType<Wall6x3CoralR>() },
            new Rectangle(42, 42, Main.maxTilesX - 84, 240));

            #endregion Surface Reefs
        }
    }
}
