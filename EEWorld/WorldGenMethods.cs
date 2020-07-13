using System;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.World.Generation;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Tiles;
using EEMod.Tiles.Furniture;
using EEMod.Tiles.Ores;
using EEMod.Tiles.Walls;
using System.Collections.Generic;

namespace EEMod.EEWorld
{
    public partial class EEWorld
    {
        private static void StartSandstorm()
        {
            Sandstorm.Happening = true;
            Sandstorm.TimeLeft = (int)(3600f * (8f + Main.rand.NextFloat() * 16f));
            ChangeSeverityIntentions();
        }
        private static void ChangeSeverityIntentions()
        {
            if (Sandstorm.Happening)
            {
                Sandstorm.IntendedSeverity = 0.4f + Main.rand.NextFloat();
            }
            else if (Main.rand.NextBool(3))
            {
                Sandstorm.IntendedSeverity = 0f;
            }
            else
            {
                Sandstorm.IntendedSeverity = Main.rand.NextFloat() * 0.3f;
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NetMessage.SendData(msgType: MessageID.WorldData, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
            }
        }

        private static void EEModOres(GenerationProgress progress)
        {
            progress.Message = "Endless Escapade Ores";
            int maxTiles = Main.maxTilesX * Main.maxTilesY;
            int rockLayerLow = (int)WorldGen.rockLayerLow;
            int OreAmount;

            OreAmount = (int)(maxTiles * 0.00008);
            for (int k = 0; k < OreAmount; k++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next(rockLayerLow, Main.maxTilesY);
                WorldGen.TileRunner(x, y, WorldGen.genRand.Next(3, 7), WorldGen.genRand.Next(5, 7), ModContent.TileType<HydroFluorideOreTile>());

                x = WorldGen.genRand.Next(0, Main.maxTilesX);
                y = WorldGen.genRand.Next(rockLayerLow, Main.maxTilesY);
                WorldGen.TileRunner(x, y, WorldGen.genRand.Next(3, 7), WorldGen.genRand.Next(5, 7), ModContent.TileType<DalantiniumOreTile>());
            }
        }


        public static void FillRegionNoEdit(int width, int height, Vector2 startingPoint, int type)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    WorldGen.TileRunner(i + (int)startingPoint.X, j + (int)startingPoint.Y, 30, 20, type, false, 0, 0, false, true);
                }
            }
        }

        public static void FillWall(int width, int height, Vector2 startingPoint, int type)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    WorldGen.PlaceWall(i + (int)startingPoint.X, j + (int)startingPoint.Y, type);
                }
            }
        }
        private static void FillRegionDiag(int width, int height, Vector2 startingPoint, int type, int leftOrRight)
        {
            if (leftOrRight == 0)
            {
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height - i; j++)
                    {
                        WorldGen.PlaceTile(i + (int)startingPoint.X, j + (int)startingPoint.Y, type);
                    }
                }
            }
            if (leftOrRight == 1)
            {
                for (int i = width; i > -1; i--)
                {
                    for (int j = 0; j < i; j++)
                    {
                        WorldGen.PlaceTile(i + (int)startingPoint.X - width, j + (int)startingPoint.Y, type);
                    }
                }
            }
        }


        private static void ClearPathWay(int width, int height, float gradient, Vector2 startingPoint, bool withPillars)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    WorldGen.KillTile(i + (int)startingPoint.X, j + (int)startingPoint.Y + (int)(i * gradient));
                }
            }
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (withPillars)
                    {
                        if (i % 10 == 0)
                            MakePillarWalls(new Vector2(i + (int)startingPoint.X, +(int)startingPoint.Y + (int)(i * gradient) - 1), 11);
                        //WorldGen.PlaceWall(i + (int)startingPoint.X, j + (int)startingPoint.Y + +(int)Math.Round(i * gradient), WallID.SandstoneBrick);
                    }
                }
            }

        }
        private static void Hole(int height, int width, Vector2 startingPoint)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    WorldGen.KillTile((int)startingPoint.X + j, (int)startingPoint.Y + i);
                    WorldGen.KillWall((int)startingPoint.X + j, (int)startingPoint.Y + i);
                }
            }
        }
        private static void MakePathWay(Vector2 firstRoom, Vector2 secondRoom, Vector2 firstRoomSize, Vector2 secondRoomSize, int heightOfConnection, bool withPillars)
        {
            Vector2 secondRoomDoorPos = new Vector2(secondRoom.X, secondRoomSize.Y / 2 + secondRoom.Y - heightOfConnection);
            Vector2 firstRoomDoorPos = new Vector2(firstRoom.X, firstRoomSize.Y / 2 + firstRoom.Y - heightOfConnection);
            if (firstRoom.X > secondRoom.X)
            {
                float gradient = (firstRoomDoorPos.Y - secondRoomDoorPos.Y) / (firstRoomDoorPos.X - firstRoomSize.X / 2 - (secondRoomDoorPos.X + secondRoomSize.X / 2));
                ClearPathWay((int)(firstRoomDoorPos.X - firstRoomSize.X / 2) - (int)(secondRoomDoorPos.X + secondRoomSize.X / 2) + 1, heightOfConnection, gradient, secondRoomDoorPos + new Vector2(secondRoomSize.X / 2, 0), withPillars);
                if (firstRoomDoorPos.X - firstRoomSize.X / 2 - (int)(secondRoomDoorPos.X + secondRoomSize.X / 2) <= 4)
                {
                    if (secondRoomDoorPos.Y < firstRoomDoorPos.Y)
                        Hole((int)(firstRoomDoorPos.Y - secondRoomDoorPos.Y), 5, new Vector2(firstRoomDoorPos.X - firstRoomSize.X / 2, secondRoomDoorPos.Y));
                    else
                        Hole((int)(secondRoomDoorPos.Y - firstRoomDoorPos.Y), 5, new Vector2(secondRoomDoorPos.X - secondRoomSize.X / 2, firstRoomDoorPos.Y));
                }
            }
            else
            {
                float gradient = (secondRoomDoorPos.Y - firstRoomDoorPos.Y) / (secondRoomDoorPos.X - secondRoomSize.X / 2 - (firstRoomDoorPos.X + firstRoomSize.X / 2));
                ClearPathWay((int)(secondRoomDoorPos.X - secondRoomSize.X / 2) - (int)(firstRoomDoorPos.X + firstRoomSize.X / 2) + 1, heightOfConnection, gradient, firstRoomDoorPos + new Vector2(firstRoomSize.X / 2, 0), withPillars);
                if (secondRoomDoorPos.X - secondRoomSize.X / 2 - (int)(firstRoomDoorPos.X + firstRoomSize.X / 2) <= 4)
                {
                    if (secondRoomDoorPos.Y < firstRoomDoorPos.Y)
                        Hole((int)(firstRoomDoorPos.Y - secondRoomDoorPos.Y), 5, new Vector2(firstRoomDoorPos.X + firstRoomSize.X / 2, secondRoomDoorPos.Y));
                    else
                        Hole((int)(secondRoomDoorPos.Y - firstRoomDoorPos.Y), 5, new Vector2(secondRoomDoorPos.X + secondRoomSize.X / 2, firstRoomDoorPos.Y));
                }
            }
        }

        public static void PlaceEntrance(int i, int j, int[,] shape)
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
                            case 0:
                                WorldGen.KillTile(k, l, false, false, true);
                                break;
                            case 1:
                                tile.type = TileID.SandstoneBrick;
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.RubyGemspark;
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        public static void PlaceWalls(int i, int j, int[,] shape)
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
                        switch (shape[y, x])
                        {
                            case 0:
                                WorldGen.KillWall(y, x, false);
                                break;
                            case 1:
                                tile.wall = WallID.SandFall;
                                break;
                            case 2:
                                tile.wall = WallID.SandstoneBrick;
                                break;
                            case 3:
                                tile.wall = WallID.DesertFossil;
                                tile.wallColor(29);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        public static void LowerResident1(int i, int j, int[,] shape)
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
                        switch (shape[y, x])
                        {
                            case 1:
                                tile.type = (ushort)ModContent.TileType<GemsandTile>();
                                tile.active(true);
                                break;

                            case 2:
                                tile.type = TileID.Platforms;
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        public static void PlaceShip(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                //WorldGen.KillTile(k, l, false, false, true);
                                break;
                            case 1:
                                tile.type = TileID.WoodBlock;
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.RichMahogany;
                                tile.color(28);
                                tile.active(true);
                                break;
                            case 3:
                                tile.type = TileID.GoldCoinPile;
                                tile.active(true);
                                break;
                            case 4:
                                tile.type = TileID.Platforms;
                                tile.active(true);
                                break;
                            case 5:
                                tile.type = TileID.WoodenBeam;
                                tile.active(true);
                                break;
                            case 6:
                                tile.type = TileID.SilkRope;
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        public static void PlaceLootRoom(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                //WorldGen.KillTile(k, l, false, false, true);
                                break;
                            case 1:
                                tile.type = (ushort)ModContent.TileType<AtlanteanBrickTile>();
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.Platforms;
                                tile.active(true);
                                break;
                            case 3:
                                tile.type = TileID.PalladiumColumn;
                                tile.active(true);
                                break;
                            case 4:
                                tile.type = TileID.Platforms;
                                tile.active(true);
                                break;
                            case 5:
                                tile.type = TileID.BlueDungeonBrick;
                                tile.active(true);
                                break;
                            case 6:
                                tile.type = TileID.SapphireGemspark;
                                tile.active(true);
                                break;
                            case 7:
                                tile.type = TileID.BlueDynastyShingles;
                                tile.active(true);
                                break;
                            case 8:
                                tile.type = TileID.LeafBlock;
                                tile.active(true);
                                break;
                            case 9:
                                tile.type = TileID.LunarBlockStardust;
                                tile.active(true);
                                break;
                            case 10:
                                tile.type = TileID.Books;
                                tile.active(true);
                                break;
                            case 11:
                                tile.type = TileID.TeamBlockBluePlatform;
                                tile.active(true);
                                break;
                            case 12:
                                tile.type = TileID.MeteoriteBrick;
                                tile.active(true);
                                break;
                            case 13:
                                tile.type = TileID.CobaltBrick;
                                tile.active(true);
                                break;
                            case 14:
                                tile.type = TileID.Spikes;
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            PlaceAtlantisWalls(i, j, LootRoomWalls);
        }
        public static void PlaceFountain(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                //WorldGen.KillTile(k, l, false, false, true);
                                break;
                            case 1:
                                tile.type = (ushort)ModContent.TileType<GemsandTile>();
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.Sand;
                                tile.color(8);
                                tile.active(true);
                                break;
                            case 3:
                                tile.type = TileID.Stone;
                                tile.color(8);
                                tile.active(true);
                                break;
                            case 4:
                                tile.type = TileID.Platforms;
                                tile.active(true);
                                break;
                            case 5:
                                tile.type = TileID.LivingFire;
                                tile.color(4);
                                tile.active(true);
                                break;
                            case 6:
                                tile.type = TileID.SilkRope;
                                tile.active(true);
                                break;
                            case 7:
                                tile.type = TileID.LeafBlock;
                                tile.color(8);
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            PlaceAtlantisWalls(i, j, FountainWalls);
        }
        public static void PlaceM1(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                //WorldGen.KillTile(k, l, false, false, true);
                                break;
                            case 1:
                                tile.type = (ushort)ModContent.TileType<GemsandTile>();
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.LunarBlockStardust;
                                tile.active(true);
                                break;
                            case 3:
                                tile.type = TileID.LeafBlock;
                                tile.color(8);
                                tile.active(true);
                                break;
                            case 4:
                                tile.type = TileID.Platforms;
                                tile.active(true);
                                break;
                            case 5:
                                tile.type = TileID.Books;
                                tile.active(true);
                                break;
                            case 6:
                                tile.type = TileID.VineRope;
                                tile.color(8);
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            PlaceAtlantisWalls(i, j, M1Walls);
        }
        public static void PlaceBlackSmith(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                //WorldGen.KillTile(k, l, false, false, true);
                                break;
                            case 1:
                                tile.type = (ushort)ModContent.TileType<GemsandTile>();
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.Coralstone;
                                tile.active(true);
                                break;
                            case 3:
                                tile.type = TileID.MetalBars;
                                tile.color(8);
                                tile.active(true);
                                break;
                            case 4:
                                tile.type = TileID.Platforms;
                                tile.active(true);
                                break;
                            case 5:
                                tile.type = TileID.Chain;
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            PlaceAtlantisWalls(i, j, BlacksmithWalls);
        }
        public static void PlaceM2(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                //WorldGen.KillTile(k, l, false, false, true);
                                break;
                            case 1:
                                tile.type = (ushort)ModContent.TileType<GemsandTile>();
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.Coralstone;
                                tile.active(true);
                                break;
                            case 3:
                                tile.type = TileID.Books;
                                tile.active(true);
                                break;
                            case 4:
                                tile.type = TileID.Platforms;
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        public static void PlaceM3(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                //WorldGen.KillTile(k, l, false, false, true);
                                break;
                            case 1:
                                tile.type = (ushort)ModContent.TileType<GemsandTile>();
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.Books;
                                tile.active(true);
                                break;
                            case 3:
                                tile.type = TileID.Platforms;
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            PlaceAtlantisWalls(i, j, M3Walls);
        }
        public static void PlaceBrewery(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                //WorldGen.KillTile(k, l, false, false, true);
                                break;
                            case 1:
                                tile.type = (ushort)ModContent.TileType<GemsandTile>();
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.Books;
                                tile.active(true);
                                break;
                            case 3:
                                tile.type = TileID.Platforms;
                                tile.active(true);
                                break;
                            case 4:
                                tile.type = TileID.Glass;
                                tile.active(true);
                                break;
                            case 5:
                                tile.type = TileID.LeafBlock;
                                tile.color(8);
                                tile.active(true);
                                break;
                            case 6:
                                tile.type = TileID.BlueDungeonBrick;
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            PlaceAtlantisWalls(i, j, BreweryWalls);
        }
        public static void PlaceM4Temple(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                //WorldGen.KillTile(k, l, false, false, true);
                                break;
                            case 1:
                                tile.type = (ushort)ModContent.TileType<GemsandTile>();
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.Books;
                                tile.active(true);
                                break;
                            case 3:
                                tile.type = TileID.Platforms;
                                tile.active(true);
                                break;
                            case 4:
                                tile.type = TileID.CobaltBrick;
                                tile.active(true);
                                break;
                            case 5:
                                tile.type = TileID.Coralstone;
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            PlaceAtlantisWalls(i, j, M4TempleWalls);
        }
        public static void PlaceH2(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                //WorldGen.KillTile(k, l, false, false, true);
                                break;
                            case 1:
                                tile.type = (ushort)ModContent.TileType<GemsandTile>();
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.Glass;
                                tile.active(true);
                                break;
                            case 3:
                                tile.type = TileID.BlueDungeonBrick;
                                tile.active(true);
                                break;
                            case 4:
                                tile.type = TileID.PalladiumColumn;
                                tile.active(true);
                                break;
                            case 5:
                                tile.type = TileID.MeteoriteBrick;
                                tile.color(8);
                                tile.active(true);
                                break;
                            case 6:
                                tile.type = TileID.BlueDynastyShingles;
                                tile.active(true);
                                break;
                            case 7:
                                tile.type = TileID.Chain;
                                tile.active(true);
                                break;
                            case 8:
                                tile.type = TileID.Gold;
                                tile.active(true);
                                break;
                            case 9:
                                tile.type = TileID.Cog;
                                tile.active(true);
                                break;
                            case 10:
                                tile.type = TileID.LeafBlock;
                                tile.color(8);
                                tile.active(true);
                                break;
                            case 11:
                                tile.type = TileID.LunarBlockStardust;
                                tile.active(true);
                                break;
                            case 12:
                                tile.type = TileID.CrystalBlock;
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            PlaceAtlantisWalls(i, j, H2Walls);
        }
        public static void MidTemp2(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                //WorldGen.KillTile(k, l, false, false, true);
                                break;
                            case 1:
                                tile.type = (ushort)ModContent.TileType<AtlanteanBrickTile>();
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.PalladiumColumn;
                                tile.color(8);
                                tile.active(true);
                                break;
                            case 3:
                                tile.type = TileID.LunarBlockStardust;
                                tile.active(true);
                                break;
                            case 4:
                                tile.type = TileID.Books;
                                tile.active(true);
                                break;
                            case 5:
                                tile.type = TileID.TeamBlockBluePlatform;
                                tile.active(true);
                                break;
                            case 6:
                                tile.type = TileID.RainCloud;
                                tile.active(true);
                                break;
                            case 7:
                                tile.type = TileID.Pots;
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        public static void PlaceFiller1(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                //WorldGen.KillTile(k, l, false, false, true);
                                break;
                            case 1:
                                tile.type = (ushort)ModContent.TileType<AtlanteanBrickTile>();
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        public static void PlaceFiller2(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                //WorldGen.KillTile(k, l, false, false, true);
                                break;
                            case 1:
                                tile.type = (ushort)ModContent.TileType<AtlanteanBrickTile>();
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.PalladiumColumn;
                                tile.color(8);
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        public static void PlaceFiller3(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                //WorldGen.KillTile(k, l, false, false, true);
                                break;
                            case 1:
                                tile.type = (ushort)ModContent.TileType<AtlanteanBrickTile>();
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.StoneSlab;
                                tile.color(8);
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        public static void PlaceFiller4(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                //WorldGen.KillTile(k, l, false, false, true);
                                break;
                            case 1:
                                tile.type = (ushort)ModContent.TileType<AtlanteanBrickTile>();
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        public static void PlaceFiller5(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                //WorldGen.KillTile(k, l, false, false, true);
                                break;
                            case 1:
                                tile.type = (ushort)ModContent.TileType<AtlanteanBrickTile>();
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        public static void PlaceFiller6(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                //WorldGen.KillTile(k, l, false, false, true);
                                break;
                            case 1:
                                tile.type = (ushort)ModContent.TileType<AtlanteanBrickTile>();
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.StoneSlab;
                                tile.color(8);
                                tile.active(true);
                                break;
                            case 3:
                                tile.type = TileID.Cloud;
                                tile.active(true);
                                break;
                            case 4:
                                tile.type = TileID.LunarBlockStardust;
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        public static void PlaceShrine(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                //WorldGen.KillTile(k, l, false, false, true);
                                break;
                            case 1:
                                tile.type = (ushort)ModContent.TileType<GemsandstoneTile>();
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.LeafBlock;
                                tile.active(true);
                                break;
                            case 3:
                                tile.type = TileID.PalladiumColumn;
                                tile.active(true);
                                break;
                            case 4:
                                tile.type = TileID.LunarBlockStardust;
                                tile.active(true);
                                break;
                            case 5:
                                tile.type = TileID.LivingCursedFire;
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        public static void PlaceObservatory(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                //WorldGen.KillTile(k, l, false, false, true);
                                break;
                            case 1:
                                tile.type = (ushort)ModContent.TileType<GemsandstoneTile>();
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.LeafBlock;
                                tile.color(8);
                                tile.active(true);
                                break;
                            case 3:
                                tile.type = TileID.Glass;
                                tile.active(true);
                                break;
                            case 4:
                                tile.type = TileID.BlueDungeonBrick;
                                tile.active(true);
                                break;
                            case 5:
                                tile.type = TileID.LunarBlockStardust;
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        public static void PlaceDome(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                //WorldGen.KillTile(k, l, false, false, true);
                                break;
                            case 1:
                                tile.type = (ushort)ModContent.TileType<GemsandstoneTile>();
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.BlueDungeonBrick;
                                tile.active(true);
                                break;
                            case 3:
                                tile.type = TileID.Glass;
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            PlaceAtlantisWalls(i, j, DomeWalls);
        }
        public static void PlaceMisc(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                //WorldGen.KillTile(k, l, false, false, true);
                                break;
                            case 1:
                                tile.type = (ushort)ModContent.TileType<GemsandstoneTile>();
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.Glass;
                                tile.active(true);
                                break;
                            case 3:
                                tile.type = TileID.LeafBlock;
                                tile.color(8);
                                tile.active(true);
                                break;
                            case 4:
                                tile.type = TileID.LunarBlockStardust;
                                tile.active(true);
                                break;
                            case 5:
                                tile.type = TileID.LivingCursedFire;
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        public static void PlaceMisc1(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                //WorldGen.KillTile(k, l, false, false, true);
                                break;
                            case 1:
                                tile.type = (ushort)ModContent.TileType<GemsandstoneTile>();
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.LunarBlockStardust;
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            PlaceAtlantisWalls(i, j, M1Walls);
        }
        public static void PlaceMisc2(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                //WorldGen.KillTile(k, l, false, false, true);
                                break;
                            case 1:
                                tile.type = (ushort)ModContent.TileType<GemsandstoneTile>();
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.LunarBlockStardust;
                                tile.active(true);
                                break;
                            case 3:
                                tile.type = TileID.Marble;
                                tile.color(8);
                                tile.active(true);
                                break;
                            case 4:
                                tile.type = TileID.MarbleBlock;
                                tile.color(8);
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        public static void PlaceMisc3(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                //WorldGen.KillTile(k, l, false, false, true);
                                break;
                            case 1:
                                tile.type = (ushort)ModContent.TileType<GemsandstoneTile>();
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.LunarBlockStardust;
                                tile.active(true);
                                break;
                            case 3:
                                tile.type = TileID.PalladiumColumn;
                                tile.color(8);
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            PlaceAtlantisWalls(i, j, M3Walls);
        }
        public static void PlaceMisc4(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                //WorldGen.KillTile(k, l, false, false, true);
                                break;
                            case 1:
                                tile.type = (ushort)ModContent.TileType<GemsandstoneTile>();
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.LunarBlockStardust;
                                tile.active(true);
                                break;
                            case 3:
                                tile.type = TileID.Platforms;
                                tile.active(true);
                                break;
                            case 4:
                                tile.type = TileID.Glass;
                                tile.active(true);
                                break;
                            case 5:
                                tile.type = TileID.LunarBlockStardust;
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        public static void PlaceH1(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                //WorldGen.KillTile(k, l, false, false, true);
                                break;
                            case 1:
                                tile.type = (ushort)ModContent.TileType<GemsandTile>();
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.LunarBlockStardust;
                                tile.active(true);
                                break;
                            case 3:
                                tile.type = TileID.PalladiumColumn;
                                tile.color(8);
                                tile.active(true);
                                break;
                            case 4:
                                tile.type = TileID.Glass;
                                tile.active(true);
                                break;
                            case 5:
                                tile.type = TileID.DynastyWood;
                                tile.color(8);
                                tile.active(true);
                                break;
                            case 6:
                                tile.type = TileID.TeamBlockBluePlatform;
                                tile.active(true);
                                break;
                            case 7:
                                tile.type = TileID.PalladiumColumn;
                                tile.color(8);
                                tile.active(true);
                                break;
                            case 8:
                                tile.type = TileID.LunarBlockStardust;
                                tile.active(true);
                                break;
                            case 9:
                                tile.type = TileID.RainCloud;
                                tile.active(true);
                                break;
                            case 10:
                                tile.type = TileID.AdamantiteBeam;
                                tile.color(8);
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        public static void PlaceHeadQ(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                //WorldGen.KillTile(k, l, false, false, true);
                                break;
                            case 1:
                                tile.type = (ushort)ModContent.TileType<GemsandTile>();
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.Platforms;
                                tile.active(true);
                                break;
                            case 3:
                                tile.type = TileID.BlueDungeonBrick;
                                tile.active(true);
                                break;
                            case 4:
                                tile.type = TileID.Glass;
                                tile.color(8);
                                tile.active(true);
                                break;
                            case 5:
                                tile.type = TileID.Marble;
                                tile.color(8);
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        public static void PlaceAtlantisWalls(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                break;
                            case 1:
                                tile.wall = WallID.SandstoneBrick;
                                tile.wallColor(21);
                                break;
                            case 2:
                                tile.wall = WallID.SapphireGemspark;
                                break;
                            case 3:
                                tile.wall = WallID.Dirt;
                                break;
                            case 4:
                                tile.wall = WallID.ShroomitePlating;
                                tile.wallColor(17);
                                break;
                            case 5:
                                tile.wall = WallID.BlueDungeonTile;
                                tile.wallColor(21);
                                break;
                            case 6:
                                tile.wall = WallID.StoneSlab;
                                tile.wallColor(21);
                                break;
                            case 7:
                                tile.wall = WallID.Dirt;
                                break;
                            case 8:
                                tile.wall = WallID.Stone;
                                tile.wallColor(21);
                                break;
                            case 9:
                                tile.wall = WallID.BlueDungeon;
                                tile.wallColor(21);
                                break;
                            case 10:
                                tile.wall = WallID.Grass;
                                break;
                            case 11:
                                tile.wall = WallID.Dirt;
                                break;
                            case 12:
                                tile.wall = WallID.Dirt;
                                break;
                            case 13:
                                tile.wall = WallID.AdamantiteBeam;
                                break;
                            case 14:
                                tile.wall = WallID.BlueDungeonTile;
                                tile.wallColor(27);
                                break;
                            case 15:
                                tile.wall = WallID.Waterfall;
                                break;
                            case 16:
                                tile.wall = WallID.CobaltBrick;
                                break;
                            case 17:
                                tile.wall = WallID.Dirt;
                                break;
                            case 18:
                                tile.wall = WallID.Grass;
                                tile.wallColor(1);
                                break;
                            case 19:
                                tile.wall = WallID.Grass;
                                tile.wallColor(10);
                                break;
                            case 20:
                                tile.wall = WallID.Dirt;
                                break;
                            case 21:
                                tile.wall = WallID.Dirt;
                                break;
                            case 22:
                                tile.wall = WallID.SilverBrick;
                                break;
                            case 23:
                                tile.wall = WallID.SapphireGemspark;
                                break;
                            case 24:
                                tile.wall = WallID.GreenDungeonSlab;
                                break;
                            case 25:
                                tile.wall = WallID.DiscWall;
                                tile.wallColor(10);
                                break;
                            case 26:
                                tile.wall = WallID.RichMahoganyFence;
                                tile.wallColor(21);
                                break;
                            case 27:
                                tile.wall = WallID.Waterfall;
                                break;
                            case 28:
                                tile.wall = WallID.Dirt;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        public static void PlaceShipWalls(int i, int j, int[,] shape)
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
                        switch (shape[y, x])
                        {
                            case 0:
                                WorldGen.KillWall(y, x, false);
                                break;
                            case 1:
                                tile.wall = WallID.Cloud;
                                break;
                            case 2:
                                tile.wall = WallID.RichMahoganyFence;
                                tile.wallColor(28);
                                break;
                            case 3:
                                tile.wall = WallID.Cloud;
                                tile.wallColor(29);
                                break;
                            case 4:
                                tile.wall = WallID.Wood;
                                break;
                            case 5:
                                tile.type = TileID.WoodenBeam;
                                tile.active(true);
                                break;
                            case 6:
                                tile.wall = WallID.Sail;
                                tile.wallColor(29);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        public static void ClearOval(int width, int height, Vector2 startingPoint)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (OvalCheck((int)(startingPoint.X + width / 2), (int)(startingPoint.Y + height / 2), i + (int)startingPoint.X, j + (int)startingPoint.Y, (int)(width * .5f), (int)(height * .5f)))
                        WorldGen.KillTile(i + (int)startingPoint.X, j + (int)startingPoint.Y);

                    if (i == width / 2 && j == height / 2)
                    {
                        WorldGen.TileRunner(i + (int)startingPoint.X, j + (int)startingPoint.Y + 2, WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(10, 20), TileID.StoneSlab, true, 0f, 0f, true, true);
                    }
                }
            }
        }

        public static void MakeLavaPit(int width, int height, Vector2 startingPoint, float lavaLevel)
        {
            ClearOval(width, height, startingPoint);
            FillRegionWithLava(width, (int)(height * lavaLevel), new Vector2(startingPoint.X, startingPoint.Y + (int)(height - (height * lavaLevel))));
        }

        public static void GenerateStructure(int i, int j, int[,] shape, int[] blocks, int[] paints = null, int[,] wallShape = null, int[] walls = null, int[] wallPaints = null)
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
                        tile.type = (ushort)blocks[shape[y, x]];
                        if (paints[blocks[shape[y, x]]] != default)
                            tile.color((byte)paints[blocks[shape[y, x]]]);
                        tile.active(true);
                    }
                }
            }
            if (wallShape != default && walls != default)
            {
                for (int y = 0; y < wallShape.GetLength(0); y++)
                {
                    for (int x = 0; x < wallShape.GetLength(1); x++)
                    {
                        int k = i - 3 + x;
                        int l = j - 6 + y;
                        if (WorldGen.InWorld(k, l, 30))
                        {
                            Tile tile = Framing.GetTileSafely(k, l);
                            tile.wall = (ushort)walls[wallShape[y, x]];
                            if (wallPaints[walls[wallShape[y, x]]] != default)
                                tile.color((byte)wallPaints[walls[wallShape[y, x]]]);
                        }
                    }
                }
            }
        }

        private static void MakePillar(Vector2 startingPos, int height, bool water, bool fire)
        {

            if (water)
            {
                Main.tile[(int)startingPos.X - 1, (int)startingPos.Y - 3].liquidType(0); // set liquid type 0 is water 1 lava 2 honey 3+ water iirc
                Main.tile[(int)startingPos.X - 1, (int)startingPos.Y - 3].liquid = 255; // set liquid ammount
                Main.tile[(int)startingPos.X - 1, (int)startingPos.Y - 4].liquid = 255;
                WorldGen.SquareTileFrame((int)startingPos.X - 1, (int)startingPos.Y - 3, true); // soemthing for astatic voiding the liquid from being static
                if (Main.netMode == NetmodeID.MultiplayerClient) // sync
                    NetMessage.sendWater((int)startingPos.X - 1, (int)startingPos.Y - 3);
            }
            if (fire)
            {
                WorldGen.PlaceTile((int)startingPos.X - 1, (int)startingPos.Y - 3, TileID.LivingFire);
                WorldGen.PlaceTile((int)startingPos.X, (int)startingPos.Y - 3, TileID.LivingFire);
                WorldGen.PlaceTile((int)startingPos.X + 1, (int)startingPos.Y - 3, TileID.LivingFire);
                WorldGen.PlaceTile((int)startingPos.X, (int)startingPos.Y - 3, TileID.LivingFire);
            }
            WorldGen.PlaceTile((int)startingPos.X - 2, (int)startingPos.Y - 3, TileID.SandStoneSlab);
            FillRegion(5, 1, new Vector2(startingPos.X - 2, startingPos.Y + 1 - 3), TileID.SandStoneSlab);
            FillRegion(3, 1, new Vector2(startingPos.X + 1 - 2, startingPos.Y + 2 - 3), TileID.SandStoneSlab);
            WorldGen.PlaceTile((int)startingPos.X + 4 - 2, (int)startingPos.Y - 3, TileID.SandStoneSlab);
            WorldGen.PlaceTile((int)startingPos.X + 2 - 2, (int)startingPos.Y + 3 - 3, TileID.SandStoneSlab);
            if (water)
            {
                var tile = Framing.GetTileSafely((int)startingPos.X - 2, (int)startingPos.Y - 3);
                var tile1 = Framing.GetTileSafely((int)startingPos.X - 2 + 4, (int)startingPos.Y - 3);
                tile.halfBrick(true);
                tile1.halfBrick(true);
            }
            MakePillarWalls(new Vector2(startingPos.X + 2 - 2, startingPos.Y + 2 - 4), height);
        }

        private static void MakePillarWalls(Vector2 startingPos, int height)
        {
            var tile1 = Framing.GetTileSafely((int)startingPos.X + 1, (int)startingPos.Y);
            var tile2 = Framing.GetTileSafely((int)startingPos.X + 0, (int)startingPos.Y);
            var tile3 = Framing.GetTileSafely((int)startingPos.X + -1, (int)startingPos.Y);
            var tile4 = Framing.GetTileSafely((int)startingPos.X + 1, (int)startingPos.Y + height + 1);
            var tile5 = Framing.GetTileSafely((int)startingPos.X + 0, (int)startingPos.Y + height + 1);
            var tile6 = Framing.GetTileSafely((int)startingPos.X + -1, (int)startingPos.Y + height + 1);
            if (tile1.active() && tile2.active() && tile3.active() && tile4.active() && tile5.active() && tile6.active())
            {
                for (int i = -1; i < 2; i++)
                {

                    for (int j = 0; j < height; j++)
                    {
                        var tile = Framing.GetTileSafely((int)startingPos.X + i, (int)startingPos.Y + j);
                        WorldGen.PlaceWall((int)startingPos.X + i, (int)startingPos.Y + j, WallID.StoneSlab);
                        tile.wallColor(28);
                    }
                }
            }
        }
        private static void MakeGoldPile(Vector2 startingPos, int type)
        {
            if (type == 0)
            {
                WorldGen.PlaceTile((int)startingPos.X, (int)startingPos.Y, TileID.GoldCoinPile);
                WorldGen.PlaceTile((int)startingPos.X + 1, (int)startingPos.Y, TileID.GoldCoinPile);
                WorldGen.PlaceTile((int)startingPos.X + 2, (int)startingPos.Y, TileID.GoldCoinPile);
                WorldGen.PlaceTile((int)startingPos.X + 3, (int)startingPos.Y, TileID.GoldCoinPile);
                WorldGen.PlaceTile((int)startingPos.X + 4, (int)startingPos.Y, TileID.GoldCoinPile);
                WorldGen.PlaceTile((int)startingPos.X + 1, (int)startingPos.Y - 1, TileID.GoldCoinPile);
                WorldGen.PlaceTile((int)startingPos.X + 2, (int)startingPos.Y - 1, TileID.GoldCoinPile);
                WorldGen.PlaceTile((int)startingPos.X + 3, (int)startingPos.Y - 1, TileID.GoldCoinPile);
            }
            if (type == 1)
            {
                WorldGen.PlaceTile((int)startingPos.X, (int)startingPos.Y, TileID.GoldCoinPile);
                WorldGen.PlaceTile((int)startingPos.X + 1, (int)startingPos.Y, TileID.GoldCoinPile);
                WorldGen.PlaceTile((int)startingPos.X + 2, (int)startingPos.Y, TileID.GoldCoinPile);
            }
        }

        private static void MakeShelf(Vector2 startingPos, int leftOrRight, int length)
        {
            if (leftOrRight == 0)
            {
                var tile = Framing.GetTileSafely((int)startingPos.X - 1, (int)startingPos.Y);
                var tile1 = Framing.GetTileSafely((int)startingPos.X, (int)startingPos.Y);
                if (tile.active() && !tile1.active())
                {
                    for (int i = 0; i < length; i++)
                    {
                        WorldGen.PlaceTile((int)startingPos.X + i, (int)startingPos.Y, TileID.Platforms, true, true, -1, 31);
                        WorldGen.PlaceTile((int)startingPos.X + i, (int)startingPos.Y - 1, TileID.Books);
                    }
                }
            }
            if (leftOrRight == 1)
            {
                var tile = Framing.GetTileSafely((int)startingPos.X + 1, (int)startingPos.Y);
                var tile1 = Framing.GetTileSafely((int)startingPos.X, (int)startingPos.Y);
                if (tile.active() && !tile1.active())
                {
                    for (int i = 0; i < length; i++)
                    {
                        WorldGen.PlaceTile((int)startingPos.X - i, (int)startingPos.Y, TileID.Platforms, true, true, -1, 31);
                        WorldGen.PlaceTile((int)startingPos.X - i, (int)startingPos.Y - 1, TileID.Books);
                    }
                }
            }
        }

        private static void FirstRoomFirstVariation(Vector2 startingPos)
        {
            int RoomPosX = (int)startingPos.X;
            int RoomPosY = (int)startingPos.Y;

            for (var x = 0; x < TowerTiles.GetLength(1); x++)
            {
                for (var y = 0; y < TowerTiles.GetLength(0); y++)
                {
                    var tile = Framing.GetTileSafely(RoomPosX + x, RoomPosY - y);
                    switch (TowerTiles[TowerTiles.GetLength(0) - 1 - y, x])
                    {
                        case 0:
                            if (tile.type == TileID.Trees)
                            {
                                WorldGen.KillTile(RoomPosX + x, RoomPosY - y, false, false, true);
                            }

                            break;
                        case 1:
                            tile.type = TileID.SandstoneBrick;
                            tile.active(true);
                            break;
                        case 2:
                            tile.type = TileID.GoldCoinPile;
                            tile.active(true);
                            break;
                        case 3:
                            tile.type = TileID.SandStoneSlab;
                            tile.active(true);
                            tile.color(28);
                            break;
                        case 4:
                            tile.type = TileID.MarbleBlock;
                            tile.active(true);
                            tile.color(28);
                            break;
                        case 5:
                            tile.type = TileID.RubyGemspark;
                            tile.active(true);
                            break;
                        case 6:
                            tile.type = TileID.PalladiumColumn;
                            tile.active(true);
                            tile.inActive(true);
                            tile.color(28);
                            break;
                        case 7:
                            tile.type = TileID.Lamps;
                            tile.active(true);
                            tile.inActive(true);
                            tile.color(28);
                            break;
                        case 8:
                            tile.type = TileID.Banners;
                            tile.active(true);
                            tile.inActive(true);
                            tile.color(28);
                            break;
                        case 9:
                            WorldGen.PlaceObject(RoomPosX + x, RoomPosY - y, TileID.TallGateClosed);
                            tile.active(true);
                            break;
                    }
                    /*if (TowerSlopes[y, x] == 5)
                    {
                        tile.halfBrick(true);
                    }
                    else
                    {
                        tile.halfBrick(false);
                        tile.slope(TowerSlopes[y, x]);
                    }*/
                    switch (TowerWalls[TowerWalls.GetLength(0) - 1 - y, x])
                    {
                        case 1:
                            tile.wall = WallID.SandstoneBrick;
                            break;
                        case 2:
                            tile.wall = WallID.StoneSlab;
                            tile.wallColor(28);
                            break;
                        case 3:
                            tile.wall = WallID.DesertFossil;
                            break;
                        case 4:
                            tile.wall = WallID.DesertFossil;
                            tile.wallColor(29);
                            break;
                        case 5:
                            tile.wall = WallID.SandFall;
                            break;
                    }
                }
            }

        }


        public static void DoAndAssignShrineValues()
        {
            int posX = WorldGen.genRand.Next(0, Main.maxTilesX);
            int posY = WorldGen.genRand.Next((int)WorldGen.worldSurfaceHigh, Main.maxTilesY);
            PlaceEntrance(posX, posY, pyroEntrance);
            PlaceWalls(posX, posY, pyroEntranceWalls);
            EntracesPosses.Add(new Vector2(posX, posY));
            yes = new Vector2(posX, posY);
        }
        public static void DoAndAssignShipValues()
        {
            PlaceShip(100, TileCheckWater(100) - 22, ShipTiles);
            PlaceShipWalls(100, TileCheckWater(100) - 22, ShipWalls);
            //GenerateStructure(100, TileCheck(100) - 22, ShipTiles, new int[]{ TileID.WoodBlock, TileID.RichMahogany, TileID.GoldCoinPile, TileID.Platforms, TileID.WoodenBeam, TileID.SilkRope}, new int[] { 0, 28, 0, 0, 0, 26 }, ShipWalls, new int[] { WallID.Cloud, WallID.RichMahoganyFence, WallID.Cloud, WallID.Wood }, new int[] { 0, 28, 29, 0 });
            ree = new Vector2(100, TileCheckWater(100) - 22);
        }
        public static int TileCheckWater(int positionX)
        {
            for (int i = 0; i < Main.maxTilesY; i++)
            {
                Tile tile = Framing.GetTileSafely(positionX, i);
                if (tile.liquid > 64)
                {
                    return i;
                }
            }
            return 0;
        }
        public static void Pyramid(float startX, float startY)
        {
            int noOfRooms = 12;
            int waterBoltRoom = WorldGen.genRand.Next(2, noOfRooms - 2);
            int fireRoom = 0;
            while (fireRoom == waterBoltRoom || fireRoom == 0)
            {
                fireRoom = WorldGen.genRand.Next(2, noOfRooms - 2);
                if (fireRoom != waterBoltRoom)
                    break;
            }
            float startingX = startX;
            float startingY = startY;
            int width = 200;
            int height = 250;
            Vector2[] Rooms = new Vector2[noOfRooms];
            Vector2[] RoomSizes = new Vector2[noOfRooms];
            bool[] isRoom = new bool[noOfRooms];
            ClearRegion(width, height, new Vector2(startingX, startingY));
            FillRegion(width, height, new Vector2(startingX, startingY), TileID.SandstoneBrick);
            for (int i = 0; i < noOfRooms; i++)
            {
                if (i == 0)
                {
                    int chosenWidth = 50;
                    int chosenHeight = 50;
                    Rooms[i] = new Vector2(startingX + 145 + chosenWidth / 2, startingY + 5 + chosenHeight / 2);
                    RoomSizes[i] = new Vector2(chosenWidth, chosenHeight);
                    ClearRegion(50, 50, new Vector2(startingX + 145, startingY + 5));
                    isRoom[i] = true;
                }
                if (i != 0 && i != noOfRooms - 1)
                {

                    for (int j = 0; j < 500; j++)
                    {
                        int counter = 0;
                        int chosenWidth = WorldGen.genRand.Next(20, 35);
                        int chosenHeight = WorldGen.genRand.Next(15, 25);
                        float chosenX = startingX + WorldGen.genRand.Next(width - chosenWidth);
                        float chosenY = startingY + WorldGen.genRand.Next(height - chosenHeight - 55);
                        Rooms[i] = new Vector2(chosenX + chosenWidth / 2, chosenY + chosenHeight / 2);
                        RoomSizes[i] = new Vector2(chosenWidth, chosenHeight);
                        for (int k = 1; k <= i; k++)
                        {
                            if (
                               (Math.Abs(Rooms[i].X - Rooms[i - k].X) > RoomSizes[i].X / 2 + RoomSizes[i - k].X / 2 + 20 ||
                               Math.Abs(Rooms[i].Y - Rooms[i - k].Y) > RoomSizes[i].Y / 2 + RoomSizes[i - 1].Y / 2) &&
                               Math.Abs(Rooms[i].X - Rooms[i - 1].X) < RoomSizes[i].X / 2 + RoomSizes[i - 1].X / 2 + 50 &&
                               Math.Abs(Rooms[i].Y - Rooms[i - 1].Y) < RoomSizes[i].Y / 2 + RoomSizes[i - 1].Y / 2 + 2 &&
                               Rooms[i].Y - Rooms[i - 1].Y > 0)
                            {
                                counter++;
                            }
                        }
                        if (counter == i)
                        {
                            isRoom[i] = true;
                            ClearRegion(chosenWidth, chosenHeight, new Vector2(chosenX, chosenY));
                            break;
                        }
                        else
                        {
                            isRoom[i] = false;
                        }
                    }
                }
                if (i == noOfRooms - 1)
                {
                    int chosenWidth1 = 150;
                    int chosenHeight1 = 50;
                    Rooms[i] = new Vector2(startingX + 25 + chosenWidth1 / 2, startingY + height - 55 + chosenHeight1 / 2);
                    RoomSizes[i] = new Vector2(chosenWidth1, chosenHeight1);
                    ClearRegion(chosenWidth1, chosenHeight1, new Vector2(startingX + 25, startingY + height - 55));
                }
            }
            for (int j = 0; j < 2; j++)
            {
                for (int i = 1; i < noOfRooms - 1; i++)
                {
                    if (isRoom[i] && isRoom[i - 1])
                    {
                        if (j == 0)
                            MakePathWay(Rooms[i], Rooms[i - 1], RoomSizes[i], RoomSizes[i - 1], 9, false);
                        if (j == 1)
                            MakePathWay(Rooms[i], Rooms[i - 1], RoomSizes[i], RoomSizes[i - 1], 9, true);
                    }
                }
            }
            Hole((int)(Rooms[noOfRooms - 1].Y - RoomSizes[noOfRooms - 1].Y / 2 - (Rooms[noOfRooms - 2].Y + RoomSizes[noOfRooms - 2].Y / 2)) + 5, 10, new Vector2(Rooms[noOfRooms - 2].X, Rooms[noOfRooms - 2].Y + RoomSizes[noOfRooms - 2].Y / 2));

            FillWall(width, height, new Vector2(startingX, startingY), WallID.SandstoneBrick);

            // var TowerChestIndex = Chest.FindChest((int)Rooms[1].X, (int)Rooms[1].Y + (int)RoomSizes[1].Y / 2 - 1);
            //WorldGen.PlaceObject((int)Rooms[1].X, (int)Rooms[1].Y + (int)RoomSizes[1].Y/2-32, TileID);
            //  WorldGen.PlaceObject((int)Rooms[1].X, (int)Rooms[1].Y + (int)RoomSizes[1].Y / 2 - 0, TileID.Pianos);
            for (int i = 0; i < noOfRooms - 1; i++)
            {
                if (WorldGen.genRand.Next(3) == 0 && i != waterBoltRoom && i != fireRoom)
                    MakePillar(Rooms[i], (int)RoomSizes[i].Y / 2, false, false);

                if (i == waterBoltRoom && i != fireRoom)
                {
                    WorldGen.PlaceObject((int)Rooms[i].X, (int)Rooms[i].Y - (int)RoomSizes[i].Y / 2, TileID.Chandeliers, false, 2);
                    MakePillar(Rooms[i] + new Vector2(-RoomSizes[i].X / 4, 0), (int)RoomSizes[i].Y / 2 + 3, true, false);
                    MakePillar(Rooms[i] + new Vector2(RoomSizes[i].X / 4, 0), (int)RoomSizes[i].Y / 2 + 3, true, false);
                    FillRegion((int)RoomSizes[i].X / 2, 1, Rooms[i] + new Vector2(-RoomSizes[i].X / 4, 0), TileID.Platforms);
                    FillRegion((int)RoomSizes[i].X / 2 - 2, 1, Rooms[i] + new Vector2(-RoomSizes[i].X / 4 + 1, -1), TileID.Books);
                    WorldGen.PlaceObject((int)Rooms[i].X, (int)Rooms[i].Y + (int)RoomSizes[i].Y / 2, TileID.WaterFountain, true, 1);
                    WorldGen.PlaceObject(WorldGen.genRand.Next((int)Rooms[i].X - (int)RoomSizes[i].X / 4 + 1, (int)Rooms[i].X - (int)RoomSizes[i].X / 4 + 1 + (int)RoomSizes[i].X / 2 - 2), (int)Rooms[i].Y - 1, TileID.Books, false, 1);
                }
                if (i == fireRoom)
                {
                    MakePillar(Rooms[i], (int)RoomSizes[i].Y / 2 + 3, false, true);
                }
                int slit1 = WorldGen.genRand.Next(2, 4);
                int slit2 = WorldGen.genRand.Next(2, 4);
                int tablePos = (int)Rooms[i].X + WorldGen.genRand.Next(-(int)RoomSizes[i].X / 2, (int)RoomSizes[i].X / 2);
                if (i < 9)
                {
                    FillRegionDiag(slit1, slit1, Rooms[i] - new Vector2(RoomSizes[i].X / 2, RoomSizes[i].Y / 2), TileID.SandstoneBrick, 0);
                    FillRegionDiag(slit2, slit2, Rooms[i] - new Vector2(-RoomSizes[i].X / 2, RoomSizes[i].Y / 2), TileID.SandstoneBrick, 1);
                }
                if (i != waterBoltRoom)
                    MakeGoldPile(new Vector2((int)Rooms[i].X + WorldGen.genRand.Next(-(int)RoomSizes[i].X / 2, (int)RoomSizes[i].X / 2), (int)Rooms[i].Y + (int)RoomSizes[i].Y / 2), WorldGen.genRand.Next(2));
                MakeShelf(new Vector2(Rooms[i].X - RoomSizes[i].X / 2, Rooms[i].Y - WorldGen.genRand.Next((int)RoomSizes[i].Y / 2)), 0, Main.rand.Next(2, 6));
                MakeShelf(new Vector2(Rooms[i].X + RoomSizes[i].X / 2, Rooms[i].Y - WorldGen.genRand.Next((int)RoomSizes[i].Y / 2)), 1, Main.rand.Next(2, 6));
                //WorldGen.PlaceObject((int)Rooms[i].X + WorldGen.genRand.Next(-(int)RoomSizes[i].X / 2, (int)RoomSizes[i].X / 2), (int)Rooms[i].Y + (int)RoomSizes[i].Y / 2, TileID.Bathtubs, false, 26);
                if (i != waterBoltRoom && i != fireRoom)
                {
                    if (WorldGen.genRand.Next(1) == 0)
                        WorldGen.PlaceObject((int)Rooms[i].X + WorldGen.genRand.Next(-(int)RoomSizes[i].X / 2, (int)RoomSizes[i].X / 2), (int)Rooms[i].Y - (int)RoomSizes[i].Y / 2, TileID.Chandeliers, false, 2);
                    WorldGen.PlaceObject((int)Rooms[i].X + WorldGen.genRand.Next(-(int)RoomSizes[i].X / 2, (int)RoomSizes[i].X / 2), (int)Rooms[i].Y + (int)RoomSizes[i].Y / 2, TileID.Beds, false, 10);
                    WorldGen.PlaceObject(tablePos, (int)Rooms[i].Y + (int)RoomSizes[i].Y / 2, TileID.Tables, false, 18);
                    WorldGen.PlaceObject(tablePos + 2, (int)Rooms[i].Y + (int)RoomSizes[i].Y / 2, TileID.Chairs, false, 19);
                    WorldGen.PlaceObject(tablePos - 2, (int)Rooms[i].Y + (int)RoomSizes[i].Y / 2, TileID.Chairs, false, 19, 0, -1, 1);
                    WorldGen.PlaceObject((int)Rooms[i].X + WorldGen.genRand.Next(-(int)RoomSizes[i].X / 2, (int)RoomSizes[i].X / 2), (int)Rooms[i].Y + (int)RoomSizes[i].Y / 2 - 1, TileID.Containers, false, 2);
                    WorldGen.PlaceObject((int)Rooms[i].X + WorldGen.genRand.Next(-(int)RoomSizes[i].X / 2, (int)RoomSizes[i].X / 2), (int)Rooms[i].Y + (int)RoomSizes[i].Y / 2, TileID.GrandfatherClocks, false, 2);
                }
            }
            FirstRoomFirstVariation(new Vector2(Rooms[0].X - RoomSizes[0].X / 2 - 2, Rooms[0].Y + RoomSizes[0].Y / 2));
        }




        public static void ClearRegion(int width, int height, Vector2 startingPoint)
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
        public static void FillRegionWithWater(int width, int height, Vector2 startingPoint)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Main.tile[i + (int)startingPoint.X, j + (int)startingPoint.Y].liquidType(0); // set liquid type 0 is water 1 lava 2 honey 3+ water iirc
                    Main.tile[i + (int)startingPoint.X, j + (int)startingPoint.Y].liquid = 255; // set liquid ammount
                    WorldGen.SquareTileFrame(i + (int)startingPoint.X, j + (int)startingPoint.Y, true); // soemthing for astatic voiding the liquid from being static
                    if (Main.netMode == NetmodeID.MultiplayerClient) // sync
                        NetMessage.sendWater(i + (int)startingPoint.X, j + (int)startingPoint.Y);
                }
            }
        }
        public static void FillRegionWithLava(int width, int height, Vector2 startingPoint)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (WorldGen.InWorld(i + (int)startingPoint.X, j + (int)startingPoint.Y))
                    {
                        Main.tile[i + (int)startingPoint.X, j + (int)startingPoint.Y].liquidType(1); // set liquid type 0 is water 1 lava 2 honey 3+ water iirc
                        Main.tile[i + (int)startingPoint.X, j + (int)startingPoint.Y].liquid = 255; // set liquid ammount
                        WorldGen.SquareTileFrame(i + (int)startingPoint.X, j + (int)startingPoint.Y, true); // soemthing for astatic voiding the liquid from being static
                        if (Main.netMode == NetmodeID.MultiplayerClient) // sync
                            NetMessage.sendWater(i + (int)startingPoint.X, j + (int)startingPoint.Y);
                    }
                }
            }
        }
        public static void MakeVolcanoEntrance(int i, int j, int[,] shape)
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
                        //tile.ClearTile();
                        switch (shape[y, x])
                        {
                            case 0:
                                break;
                            case 1:
                                WorldGen.PlaceTile(k, l, TileID.Ash);
                                break;
                            case 2:
                                WorldGen.PlaceWall(k, l, WallID.Cloud);
                                tile.wallColor(29);
                                break;
                        }
                    }
                }
            }
        }
        public static void GenerateLuminite()
        {
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesX; j++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if(tile.type == TileID.Stone)
                    {
                        if(Main.rand.NextBool(2000))
                        {
                            WorldGen.TileRunner(i, j, 10, 10, TileID.LunarOre);
                        }
                    }
                }
            }
        }
        public static void RemoveWaterFromRegion(int width, int height, Vector2 startingPoint)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (Main.tile[i + (int)startingPoint.X, j + (int)startingPoint.Y].liquidType() == 0 && Main.tile[i + (int)startingPoint.X, j + (int)startingPoint.Y].liquid > 64)
                    {
                        Main.tile[i + (int)startingPoint.X, j + (int)startingPoint.Y].ClearEverything();
                        if (Main.netMode == NetmodeID.MultiplayerClient) // sync
                            NetMessage.sendWater(i + (int)startingPoint.X, j + (int)startingPoint.Y);
                    }
                }
            }
        }
        public static int TileCheck2(int i, int j)
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
        public static void MakeOvalJaggedTop(int width, int height, Vector2 startingPoint, int type, int lowRand = 10, int highRand = 20)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (OvalCheck((int)(startingPoint.X + width / 2), (int)(startingPoint.Y + height / 2), i + (int)startingPoint.X, j + (int)startingPoint.Y, (int)(width * .5f), (int)(height * .5f)))
                        WorldGen.PlaceTile(i + (int)startingPoint.X, j + (int)startingPoint.Y, type);

                    if (i == width / 2 && j == height / 2)
                    {
                        WorldGen.TileRunner(i + (int)startingPoint.X, j + (int)startingPoint.Y + 2, WorldGen.genRand.Next(lowRand, highRand), WorldGen.genRand.Next(lowRand, highRand), type, true, 0f, 0f, true, true);
                    }
                }
            }
            int steps = 0;
            for (int i = 0; i < width; i++)
            {
                if (Main.rand.NextBool(2))
                    steps += Main.rand.Next(-1, 2);
                for (int j = -6; j < height / 2 - 2 + steps; j++)
                {
                    Tile tile = Framing.GetTileSafely(i + (int)startingPoint.X, j + (int)startingPoint.Y);
                    if (tile.type == type)
                        WorldGen.KillTile(i + (int)startingPoint.X, j + (int)startingPoint.Y);
                }
            }
        }
        public static void MakeOvalJaggedBottom(int width, int height, Vector2 startingPoint, int type, bool overwrite = false)
        {
            int steps = 0;
            for (int i = 0; i < width; i++)
            {
                if (Main.rand.NextBool(2))
                    steps += Main.rand.Next(-1, 2);
                for (int j = 0; j < height; j++)
                {
                    if (OvalCheck((int)(startingPoint.X + width / 2), (int)(startingPoint.Y + height / 2) + steps, i + (int)startingPoint.X, j + (int)startingPoint.Y, (int)(width * .5f), (int)(height * .5f)))
                        WorldGen.PlaceTile(i + (int)startingPoint.X, j + (int)startingPoint.Y, type);
                }
            }
            int steps2 = 0;
            for (int i = 0; i < width; i++)
            {
                if (Main.rand.NextBool(2))
                    steps2 += Main.rand.Next(-1, 2);
                for (int j = height / 2 - 2 + steps2; j < height + 12 + steps2; j++)
                {
                    Tile tile = Framing.GetTileSafely(i + (int)startingPoint.X, j + (int)startingPoint.Y);
                    if (tile.type == type)
                        WorldGen.KillTile(i + (int)startingPoint.X, j + (int)startingPoint.Y);
                }
            }
        }
        public static void MakeOvalFlatTop(int width, int height, Vector2 startingPoint, int type)
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
        public static void MakeOval(int width, int height, Vector2 startingPoint, int type, bool forced)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (OvalCheck((int)(startingPoint.X + width / 2), (int)(startingPoint.Y + height / 2), i + (int)startingPoint.X, j + (int)startingPoint.Y, (int)(width * .5f), (int)(height * .5f)))
                        WorldGen.PlaceTile(i + (int)startingPoint.X, j + (int)startingPoint.Y, type,false, forced);

                    if (i == width / 2 && j == height / 2)
                    {
                        WorldGen.TileRunner(i + (int)startingPoint.X, j + (int)startingPoint.Y + 2, WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(10, 20), type, true, 0f, 0f, true, true);
                    }
                }
            }
        }
        public static void MakeTriangle(Vector2 startingPoint, int width, int height, int slope, int type, bool isFlat = false, bool hasChasm = false, int wallType = 0)
        {
            int initialStartingPosX = (int)startingPoint.X;
            int initialWidth = width;
            int initialSlope = slope;
            for (int j = 0; j < height; j++)
            {
                slope = Main.rand.Next(-1, 2) + initialSlope;
                for (int k = 0; k < slope; k++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        WorldGen.PlaceTile(i + (int)startingPoint.X, (int)startingPoint.Y - (j + k), type);
                    }
                }
                startingPoint.X += 1;
                width-=2;
                j += slope - 1;
            }
            int topRight = (int)startingPoint.Y - height;
            if (hasChasm)
            {
                MakeChasm((int)(startingPoint.X + width / 2), (int)(topRight + (height / (slope * 10))), height - 30, TileID.StoneSlab, 0, 10, 20);

                for (int i = 0; i < Main.maxTilesX; i++)
                {
                    for (int j = 0; j < Main.maxTilesY; j++)
                    {
                        if (wallType != 0)
                        {
                            WorldGen.PlaceWall(i, j, wallType);
                        }
                        Tile tile = Framing.GetTileSafely(i, j);
                        if (tile.type == TileID.StoneSlab)
                        {
                            WorldGen.KillTile(i, j);
                        }
                    }
                }
            }
            if (isFlat)
            {
                ClearRegion(initialWidth, height/5, new Vector2(initialStartingPosX, topRight - 5));
                KillWall(initialWidth, height / 5, new Vector2(initialStartingPosX, topRight - 5));
            }
        }
        public static void FillRegion(int width, int height, Vector2 startingPoint, int type)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    WorldGen.PlaceTile(i + (int)startingPoint.X, j + (int)startingPoint.Y, type);
                }
            }
        }
        private static void MakeCoral(Vector2 startingPoint, int type, int strength)
        {
            for (int j = 0; j < 5; j++)
            {
                int displacement = 0;
                for (int i = 0; i < strength; i++)
                {
                    if (Main.rand.NextBool(1))
                    {
                        displacement += Main.rand.Next(-1, 2);
                    }
                    WorldGen.PlaceTile(displacement + (int)startingPoint.X, (int)startingPoint.Y - i, type, false, true);
                }
            }
        }

        private static void FillRegion(int width, int height, Vector2 startingPoint, int type1, int type2)
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
        public static void MakeChasm(int positionX, int positionY, int height, int type, float slant, int sizeAddon, int stepAddon)
        {
            for (int i = 0; i < height; i++)
            {
                // Tile tile = Framing.GetTileSafely(positionX + (int)(i * slant), positionY + i);
                WorldGen.TileRunner(positionX + (int)(i * slant), positionY + i, WorldGen.genRand.Next(5 + sizeAddon / 2, 10 + sizeAddon), WorldGen.genRand.Next(5, 10) + stepAddon, type, true, 0f, 0f, true, true);
            }
        }
        public static void MakeWavyChasm(int positionX, int positionY, int height, int type, float slant, int sizeAddon)
        {
            for (int i = 0; i < height; i++)
            {
                // Tile tile = Framing.GetTileSafely(positionX + (int)(i * slant), positionY + i);
                WorldGen.TileRunner(positionX + (int)(i * slant) + (int)(Math.Sin(i / (float)50) * (20 * (1 + (i * 1.5f / (float)height)))), positionY + i, WorldGen.genRand.Next(5 + sizeAddon / 2, 10 + sizeAddon), WorldGen.genRand.Next(5, 10), type, true, 0f, 0f, true, true);
            }
        }
        public static void MakeWavyChasm2(int positionX, int positionY, int height, int type, float slant, int sizeAddon, bool Override)
        {
            for (int i = 0; i < height; i++)
            {
                // Tile tile = Framing.GetTileSafely(positionX + (int)(i * slant), positionY + i);
                WorldGen.TileRunner(positionX + (int)(i * slant) + (int)(Math.Sin(i / (float)50) * (20 * (1 + (i * 1.5f / (float)height)))), positionY + i, WorldGen.genRand.Next(5 + sizeAddon / 2, 10 + sizeAddon), WorldGen.genRand.Next(20, 40), type, true, 0f, 0f, true, Override);
            }
        }
        public static int TileCheck(int positionX, int type)
        {
            for (int i = 0; i < Main.maxTilesY; i++)
            {
                Tile tile = Framing.GetTileSafely(positionX, i);
                if (tile.type == type)
                {
                    return i;
                }
            }
            return 0;
        }
        public static void KillWall(int width, int height, Vector2 startingPoint)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    WorldGen.KillWall(i + (int)startingPoint.X, j + (int)startingPoint.Y);
                }
            }
        }

        public static bool OvalCheck(int h, int k, int x, int y, int a, int b)
        {
            double p = Math.Pow(x - h, 2) / Math.Pow(a, 2)
                    + Math.Pow(y - k, 2) / Math.Pow(b, 2);

            return p < 1 ? true : false;
        }
        public static void MakeAtlantis(Vector2 startingPoint, Vector2 size)
        {
            List<Vector2> islandPositions = new List<Vector2>();
            int sizeX = 120;
            int sizeY = 60;
            int numberOfBuildingsInMidClass = 7;
            int numberOfBuildingsInHighClass = 6;
            int numberOfFillers = 9;
            int numberMisc = 4;
            int number;
            List<int> listNumbers = new List<int>();
            List<int> listNumbersHighClass = new List<int>();
            List<int> listNumbersFillers = new List<int>();
            List<int> listNumbersMisc = new List<int>();
            List<Vector2> fillers = new List<Vector2>();
            int yPos = (int)startingPoint.Y + 80;
            for (int i = 0; i < numberOfBuildingsInMidClass; i++)
            {
                do
                {
                    number = Main.rand.Next(0, numberOfBuildingsInMidClass);
                } while (listNumbers.Contains(number));
                listNumbers.Add(number);
            }
            for (int i = 0; i < numberMisc; i++)
            {
                do
                {
                    number = Main.rand.Next(0, numberMisc);
                } while (listNumbersMisc.Contains(number));
                listNumbersMisc.Add(number);
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
            Vector2 midpoint = (startingPoint + size) / 2;
            for (int i = 0; i < numberOfBuildingsInMidClass; i++)
            {
                float randomPosMiddleClass = midpoint.X - sizeX + (i * ((sizeX * 2) / (numberOfBuildingsInMidClass - 1)));
                float whereTheYShouldBe = yPos + sizeY - (float)(Math.Pow(randomPosMiddleClass - midpoint.X, 2) / (Math.Pow(sizeX, 2) / (float)sizeY));
                Vector2 actualPlace = new Vector2(randomPosMiddleClass, whereTheYShouldBe);
                islandPositions.Add(actualPlace);
            }
            float displacement = 220;
            float startingHeightOfUpperClass = sizeY + yPos + 30;
            KillWall((int)(size.X), (int)(size.Y), startingPoint);
            for (int j = 0; j < islandPositions.Count; j++)
            {
                // MakeOvalFlatTop(40, 13, new Vector2(islandPositions[j].X - 15, islandPositions[j].Y), ModContent.TileType<HardenedGemsandTile>());
                switch (listNumbers[j])
                {
                    case 0:
                        {
                            PlaceM1((int)islandPositions[j].X - M1.GetLength(0) / 2, (int)islandPositions[j].Y -  M1.GetLength(1) / 2,  M1);
                            break;
                        }
                    case 1:
                        {
                             PlaceM2((int)islandPositions[j].X -  M2.GetLength(0) / 2, (int)islandPositions[j].Y -  M2.GetLength(1) / 2,  M2);
                            break;
                        }
                    case 2:
                        {
                             PlaceM3((int)islandPositions[j].X -  M3.GetLength(0) / 2, (int)islandPositions[j].Y -  M3.GetLength(1) / 2,  M3);
                            break;
                        }
                    case 3:
                        {
                             PlaceBlackSmith((int)islandPositions[j].X -  Blacksmith.GetLength(0) / 2, (int)islandPositions[j].Y -  Blacksmith.GetLength(1) / 2,  Blacksmith);
                            break;
                        }
                    case 4:
                        {
                             PlaceM4Temple((int)islandPositions[j].X -  M4Temple.GetLength(0) / 2, (int)islandPositions[j].Y -  M4Temple.GetLength(1) / 2,  M4Temple);
                            break;
                        }
                    case 5:
                        {
                             PlaceBrewery((int)islandPositions[j].X -  Brewery.GetLength(0) / 2, (int)islandPositions[j].Y -  Brewery.GetLength(1) / 2,  Brewery);
                            break;
                        }
                    case 6:
                        {
                             PlaceHeadQ((int)islandPositions[j].X -  HeadQ.GetLength(0) / 2, (int)islandPositions[j].Y -  HeadQ.GetLength(1) / 2,  HeadQ);
                            break;
                        }
                }
            }
            fillers.Add(new Vector2(midpoint.X - (int)displacement + 90, (int)startingHeightOfUpperClass + 20));
            fillers.Add(new Vector2(midpoint.X + (int)displacement - 90, (int)startingHeightOfUpperClass + 20));
            fillers.Add(new Vector2(midpoint.X - (int)displacement, (int)startingHeightOfUpperClass - 60));
            fillers.Add(new Vector2(midpoint.X - (int)displacement + 90 - 40, (int)startingHeightOfUpperClass - 30));
            fillers.Add(new Vector2(midpoint.X + (int)displacement - 90 + 30, (int)startingHeightOfUpperClass + 20 - 40));
            fillers.Add(new Vector2(midpoint.X + (int)displacement - 30, (int)startingHeightOfUpperClass + 50 + 130));
            fillers.Add(new Vector2(midpoint.X - (int)displacement + 30, (int)startingHeightOfUpperClass + 50 + 130));
             MakeLayerWithOutline((int)midpoint.X, 70, 20, 1, ModContent.TileType<HardenedGemsandTile>(), 10);
            for (int j = 0; j < 3; j++)
            {
                switch (listNumbersHighClass[j])
                {
                    case 0:
                        {
                             PlaceH2((int)midpoint.X - (int)displacement -  H2.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 -  H2.GetLength(1) / 2,  H2);
                            break;
                        }
                    case 1:
                        {
                             PlaceLootRoom((int)midpoint.X - (int)displacement -  LootRoom.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 -  LootRoom.GetLength(1) / 2,  LootRoom);
                            break;
                        }
                    case 2:
                        {
                             PlaceH1((int)midpoint.X - (int)displacement -  H1.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 -  H1.GetLength(1) / 2,  H1);
                            break;
                        }
                    case 3:
                        {
                             MidTemp2((int)midpoint.X - (int)displacement -  WorshipPlaceAtlantis.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 -  WorshipPlaceAtlantis.GetLength(1) / 2,  WorshipPlaceAtlantis);
                            break;
                        }
                    case 4:
                        {
                             PlaceFountain((int)midpoint.X - (int)displacement -  Fountain.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 -  Fountain.GetLength(1) / 2,  Fountain);
                            break;
                        }
                    case 5:
                        {
                             PlaceH2((int)midpoint.X - (int)displacement -  H2.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 -  H2.GetLength(1) / 2,  H2);
                            break;
                        }
                    case 6:
                        {
                             PlaceObservatory((int)midpoint.X - (int)displacement -  Observatory.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 -  Observatory.GetLength(1) / 2,  Observatory);
                            break;
                        }
                }
            }
            for (int j = 0; j < 3; j++)
            {
                switch (listNumbersHighClass[j + 3])
                {
                    case 0:
                        {
                             PlaceH2((int)midpoint.X + (int)displacement -  H2.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 -  H2.GetLength(1) / 2,  H2);
                            break;
                        }
                    case 1:
                        {
                             PlaceLootRoom((int)midpoint.X + (int)displacement -  LootRoom.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 -  LootRoom.GetLength(1) / 2,  LootRoom);
                            break;
                        }
                    case 2:
                        {
                             PlaceH1((int)midpoint.X + (int)displacement -  H1.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 -  H1.GetLength(1) / 2,  H1);
                            break;
                        }
                    case 3:
                        {
                             MidTemp2((int)midpoint.X + (int)displacement -  WorshipPlaceAtlantis.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 -  WorshipPlaceAtlantis.GetLength(1) / 2,  WorshipPlaceAtlantis);
                            break;
                        }
                    case 4:
                        {
                             PlaceFountain((int)midpoint.X + (int)displacement -  Fountain.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 -  Fountain.GetLength(1) / 2,  Fountain);
                            break;
                        }
                    case 5:
                        {
                             PlaceH2((int)midpoint.X + (int)displacement -  H2.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 -  H2.GetLength(1) / 2,  H2);
                            break;
                        }
                    case 6:
                        {
                             PlaceObservatory((int)midpoint.X - (int)displacement -  Observatory.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 -  Observatory.GetLength(1) / 2,  Observatory);
                            break;
                        }
                }
            }
            int distanceFromEdge = 100 + (int)startingPoint.X;
            for (int j = 0; j < 2; j++)
            {
                for (int i = 2; i > 0; i--)
                {
                    if ((j == 0 && i == 2) || (j == 1 && i == 1))
                    {
                        switch (listNumbersMisc[j])
                        {
                            case 0:
                                {
                                     PlaceMisc1(distanceFromEdge + (j * 50) -  Misc1.GetLength(0) / 2, distanceFromEdge + (i * 40) - 50 -  Misc1.GetLength(1) / 2,  Misc1);
                                    break;
                                }
                            case 1:
                                {
                                     PlaceMisc2(distanceFromEdge + (j * 50) -  Misc2.GetLength(0) / 2, distanceFromEdge + (i * 40) - 50 -  Misc2.GetLength(1) / 2,  Misc2);
                                    break;
                                }
                            case 2:
                                {
                                     PlaceMisc3(distanceFromEdge + (j * 50) -  Misc3.GetLength(0) / 2, distanceFromEdge + (i * 40) - 50 -  Misc3.GetLength(1) / 2,  Misc3);
                                    break;
                                }
                            case 3:
                                {
                                     PlaceMisc4(distanceFromEdge + (j * 50) -  Misc4.GetLength(0) / 2, distanceFromEdge + (i * 40) - 50 -  Misc4.GetLength(1) / 2,  Misc4);
                                    break;
                                }
                        }
                    }
                }
            }
             MakeChasm(distanceFromEdge - 20, distanceFromEdge + 60, 170, ModContent.TileType<GemsandstoneTile>(), 0, 10, 10);
             MakeChasm(distanceFromEdge + 70, distanceFromEdge + 60, 170, ModContent.TileType<GemsandstoneTile>(), 0, 10, 10);
             MakeOvalJaggedTop(25, 40, new Vector2(distanceFromEdge + 12, distanceFromEdge + 120), ModContent.TileType<GemsandstoneTile>());
            for (int j = 0; j < 2; j++)
            {
                for (int i = 2; i > 0; i--)
                {
                    if ((j == 0 && i == 2) || (j == 1 && i == 1))
                    {
                        switch (listNumbersMisc[j + 2])
                        {
                            case 0:
                                {
                                     PlaceMisc1((int)(startingPoint.X + size.X) - distanceFromEdge - (j * 50) - 44 -  Misc1.GetLength(0) / 2, distanceFromEdge + (i * 40) - 50 -  Misc1.GetLength(1) / 2,  Misc1);
                                    break;
                                }
                            case 1:
                                {
                                     PlaceMisc2((int)(startingPoint.X + size.X) - distanceFromEdge - (j * 50) - 44 -  Misc2.GetLength(0) / 2, distanceFromEdge + (i * 40) - 50 -  Misc2.GetLength(1) / 2,  Misc2);
                                    break;
                                }
                            case 2:
                                {
                                     PlaceMisc3((int)(startingPoint.X + size.X) - distanceFromEdge - (j * 50) - 44 -  Misc3.GetLength(0) / 2, distanceFromEdge + (i * 40) - 50 -  Misc3.GetLength(1) / 2,  Misc3);
                                    break;
                                }
                            case 3:
                                {
                                     PlaceMisc4((int)(startingPoint.X + size.X) - distanceFromEdge - (j * 50) - 44 -  Misc4.GetLength(0) / 2, distanceFromEdge + (i * 40) - 50 -  Misc4.GetLength(1) / 2,  Misc4);
                                    break;
                                }
                        }
                    }
                }
            }
            fillers.Add(new Vector2((int)(startingPoint.X + size.X) - distanceFromEdge - 44, distanceFromEdge + 120));
            fillers.Add(new Vector2(60, 60));
            Main.spawnTileX = 500;
            Main.spawnTileY = 300;
            SubworldManager.SettleLiquids();
            EEMod.isSaving = false;
             MakeOval(350, 190, new Vector2(midpoint.X - 160, (int)startingHeightOfUpperClass + 25), ModContent.TileType<GemsandstoneTile>(), false);
             MakeOval(335, 160, new Vector2(midpoint.X - 165, (int)startingHeightOfUpperClass + 40), TileID.StoneSlab, true);
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (tile.type == TileID.StoneSlab)
                        WorldGen.KillTile(i, j);
                }
            }
            // MakeChasm(Main.maxTilesX / 2 - 120, (int)startingHeightOfUpperClass + 75, 70, ModContent.TileType<GemsandstoneTile>(), 0, 1,1);
             MakeAtlantisCastle((int)midpoint.X - 146, (int)startingHeightOfUpperClass + 65);
            //imagine coding...
            for (int j = 0; j < fillers.Count; j++)
            {
                switch (listNumbersFillers[j])
                {
                    case 0:
                        {
                             PlaceFiller1((int)fillers[j].X -  Filler1.GetLength(0) / 2, (int)fillers[j].Y -  Filler1.GetLength(1) / 2,  Filler1);
                            break;
                        }
                    case 1:
                        {
                             PlaceFiller2((int)fillers[j].X -  Filler2.GetLength(0) / 2, (int)fillers[j].Y -  Filler2.GetLength(1) / 2,  Filler2);
                            break;
                        }
                    case 2:
                        {
                             PlaceFiller3((int)fillers[j].X -  Filler3.GetLength(0) / 2, (int)fillers[j].Y -  Filler3.GetLength(1) / 2,  Filler3);
                            break;
                        }
                    case 3:
                        {
                             PlaceFiller4((int)fillers[j].X -  Filler4.GetLength(0) / 2, (int)fillers[j].Y -  Filler4.GetLength(1) / 2,  Filler4);
                            break;
                        }
                    case 4:
                        {
                             PlaceFiller5((int)fillers[j].X -  Filler5.GetLength(0) / 2, (int)fillers[j].Y -  Filler5.GetLength(1) / 2,  Filler5);
                            break;
                        }
                    case 5:
                        {
                             PlaceFiller6((int)fillers[j].X -  Filler6.GetLength(0) / 2, (int)fillers[j].Y -  Filler6.GetLength(1) / 2,  Filler6);
                            break;
                        }
                    case 6:
                        {
                             PlaceShrine((int)fillers[j].X -  Shrine.GetLength(0) / 2, (int)fillers[j].Y -  Shrine.GetLength(1) / 2,  Shrine);
                            break;
                        }
                    case 7:
                        {
                             PlaceDome((int)fillers[j].X -  Dome.GetLength(0) / 2, (int)fillers[j].Y -  Dome.GetLength(1) / 2,  Dome);
                            break;
                        }
                    case 8:
                        {
                             PlaceMisc((int)fillers[j].X -  Misc.GetLength(0) / 2, (int)fillers[j].Y -  Misc.GetLength(1) / 2,  Misc);
                            break;
                        }
                }
            }
            WorldGen.TileRunner(80, 80, 30, 10, ModContent.TileType<GemsandstoneTile>());
            WorldGen.TileRunner(100, 60, 30, 10, ModContent.TileType<GemsandstoneTile>());
             FillRegionWithWater((int)(size.X), (int)(size.Y), startingPoint);
             FillRegionWithWater((int)(size.X), (int)(size.Y), startingPoint);
             FillRegionWithWater((int)(size.X), (int)(size.Y), startingPoint);
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

             FillRegionWithWater(Main.maxTilesX, Main.maxTilesY, Vector2.Zero);
             RemoveWaterFromRegion(Main.maxTilesX, 210, Vector2.Zero);
             MakeOvalJaggedTop(Main.maxTilesX, Main.maxTilesY - 300, new Vector2(0, 300), ModContent.TileType<GemsandTile>(), 15, 15);


             RemoveWaterFromRegion(40, 40, new Vector2(180, 170));
             MakeOvalJaggedBottom(islandWidth, islandHeight, new Vector2(50, 210), ModContent.TileType<VolcanicAshTile>());
             MakeTriangle(new Vector2(100, 230), 200, 160, 2, ModContent.TileType<VolcanicAshTile>(), true, true, ModContent.WallType<VolcanicAshWallTile>());
             FillRegionWithLava(40, 50, new Vector2(180, 190));
             KillWall(Main.maxTilesX, Main.maxTilesY, Vector2.Zero);
             MakeVolcanoEntrance(198, 192,  VolcanoEntrance);

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

             FillRegion(Main.maxTilesX, Main.maxTilesY, Vector2.Zero, ModContent.TileType<MagmastoneTile>());
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    if (Main.rand.NextBool(3000))
                         MakeLavaPit(Main.rand.Next(20, 30), Main.rand.Next(7, 20), new Vector2(i, j), Main.rand.NextFloat(0.1f, 0.5f));
                }
            }
             MakeChasm(200, 10, 100, TileID.StoneSlab, 0, 10, 20);
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
             MakeOvalJaggedTop(80, 60, new Vector2(160, 170), ModContent.TileType<MagmastoneTile>());
             KillWall(Main.maxTilesX, Main.maxTilesY, Vector2.Zero);
             FillWall(Main.maxTilesX, Main.maxTilesY, Vector2.Zero, ModContent.WallType<MagmastoneWallTile>());
        }
        public static void MakeLayer(int X, int midY, int size, int layer, int type)
        {

            int maxTiles = (int)(Main.maxTilesX * Main.maxTilesY * 9E-04);
            for (int k = 0; k < maxTiles * (size/8); k++)
            {
                int x = WorldGen.genRand.Next(X - 80, X + 80);
                int y = WorldGen.genRand.Next(midY - 80, midY + 80);
                // Tile tile = Framing.GetTileSafely(x, y);
                if (layer == 1)
                {
                    if (Vector2.DistanceSquared(new Vector2(x, y), new Vector2(X, midY)) < size * size)
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
                for (int j = 0; j < 2000; j++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (tile.type == TileID.StoneSlab)
                        WorldGen.KillTile(i, j);
                }
            }
            /*  for (int k = 0; k < density; k++)
              {
                  int x = WorldGen.genRand.Next(X - 80, X + 80);
                  int y = WorldGen.genRand.Next(midY - 100, midY + 100);
                  Tile tile = Framing.GetTileSafely(x, y);
                  if (layer == 1)
                  {
                      int sizeSQ = size * size + 50 * 50;
                     // if (Vector2.DistanceSquared(new Vector2(x, y), new Vector2(X, midY)) < (sizeSQ) && tile.active())
                        //  WorldGen.TileRunner(x, y, WorldGen.genRand.Next(4, 10), WorldGen.genRand.Next(5, 10), ModContent.TileType<HardenedGemsandTile>(), true, 0f, 0f, true, true);
                  }
                  if (layer == 2)
                  {
                     // if (OvalCheck(X, midY, x, y, (size * 3) + 10, (size) + 10) && tile.active())
                        //  WorldGen.TileRunner(x, y, WorldGen.genRand.Next(4, 10), WorldGen.genRand.Next(5, 10), ModContent.TileType<GemsandstoneTile>(), true, 1, 1, true, true);
                  }
              }*/
            if (layer == 1)
                WorldGen.TileRunner(X, midY, WorldGen.genRand.Next(5, 10), WorldGen.genRand.Next(5, 10), type, true, 1f, 1f, false, true);
        }
        public static void MakeLayerWithOutline(int X, int midY, int size, int layer, int type, int thickness)
        {

            int maxTiles = (int)(Main.maxTilesX * Main.maxTilesY * 9E-04);
            for (int k = 0; k < maxTiles * (size / 8); k++)
            {
                int x = WorldGen.genRand.Next(X - (size*2), X + (size * 2));
                int y = WorldGen.genRand.Next(midY - (size * 2), midY + (size * 2));
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
            
              for (int k = 0; k < maxTiles * 3; k++)
              {
                  int x = WorldGen.genRand.Next(X - (size * 2), X + (size * 2));
                  int y = WorldGen.genRand.Next(midY - (size * 2), midY + (size * 2));
                  Tile tile = Framing.GetTileSafely(x, y);
                  if (layer == 1)
                  {
                      int sizeSQ = size + thickness;
                      if (Vector2.Distance(new Vector2(x, y), new Vector2(X, midY)) < (sizeSQ))
                          WorldGen.TileRunner(x, y, WorldGen.genRand.Next(4, 10), WorldGen.genRand.Next(5, 10), ModContent.TileType<HardenedGemsandTile>(), true, 0f, 0f, false, false);
                  }
                  if (layer == 2)
                  {
                      if (OvalCheck(X, midY, x, y, (size * 3) + 10, (size) + 10) && tile.active())
                          WorldGen.TileRunner(x, y, WorldGen.genRand.Next(4, 10), WorldGen.genRand.Next(5, 10), ModContent.TileType<GemsandstoneTile>(), true, 1, 1, true, true);
                  }
              }
            for (int i = 0; i < 800; i++)
            {
                for (int j = 0; j < 2000; j++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (tile.type == TileID.StoneSlab)
                        WorldGen.KillTile(i, j);
                }
            }
            if (layer == 1)
            {
                //MakeOvalJaggedTop(20, 10, new Vector2(X - 12, midY), ModContent.TileType<GemsandstoneTile>());
            }
        }

        public static void CoralReef()
        {
            int maxTiles = (int)(Main.maxTilesX * Main.maxTilesY * 9E-04);
            int chasmX = 100;
            int chasmY = 100;
            MakeWavyChasm(chasmX, chasmY, 1000, TileID.StoneSlab, 0.3f, WorldGen.genRand.Next(50, 60));
            MakeWavyChasm2(chasmX - 50, chasmY, 1000, ModContent.TileType<HardenedGemsandTile>(), 0.3f, WorldGen.genRand.Next(10, 20), true);
            MakeWavyChasm2(chasmX + 50, chasmY, 1000, ModContent.TileType<HardenedGemsandTile>(), 0.3f, WorldGen.genRand.Next(10, 20), true);
            for (int i = 0; i < 5; i++)
            {
                MakeChasm(chasmX + Main.rand.Next(-50, 50) + i * 20, chasmY + (i * 200) + Main.rand.Next(-50, 50), Main.rand.Next(5, 30), TileID.StoneSlab, Main.rand.Next(5, 10), WorldGen.genRand.Next(20, 60), Main.rand.Next(10, 20));

            }
            for (int i = 0; i < 20; i++)
            {
                MakeOvalFlatTop(Main.rand.Next(10, 20), Main.rand.Next(5, 10), new Vector2(chasmX + Main.rand.Next(-10, 10) + i * 15, chasmY + (i * 50) + Main.rand.Next(-20, 20)), ModContent.TileType<HardenedGemsandTile>());
                if (i % 5 == 0)
                {
                    MakeLayer(chasmX + Main.rand.Next(-10, 10) + i * 15, chasmY + Main.rand.Next(-20, 20) + (i * 50), 25, 2, ModContent.TileType<HardenedGemsandTile>());
                    MakeLayer(chasmX + Main.rand.Next(-10, 10) + i * 5, chasmY + Main.rand.Next(-20, 20) + (i * 50), 20, 1, ModContent.TileType<HardenedGemsandTile>());
                    MakeCoral(new Vector2(chasmX + Main.rand.Next(-10, 10) + i * 5, chasmY + Main.rand.Next(-20, 20) + (i * 50)), TileID.Coralstone, Main.rand.Next(4, 8));
                    for (int j = 0; j < 7; j++)
                        MakeOvalFlatTop(WorldGen.genRand.Next(20, 30), WorldGen.genRand.Next(5, 10), new Vector2(chasmX + Main.rand.Next(-10, 10) + i * 15 + (j * 35) - 50, chasmY + Main.rand.Next(-20, 20) + (i * 50)), ModContent.TileType<GemsandstoneTile>());
                }
            }
            for (int k = 0; k < maxTiles * 9; k++)
            {
                int xPos = 500;
                int yPos = 1200;
                int size = 80;
                int x = WorldGen.genRand.Next(xPos - (size*3), xPos + (size * 3));
                int y = WorldGen.genRand.Next(yPos - (size * 3), yPos + (size * 3));
                if (OvalCheck(xPos, yPos, x, y, size * 3, size))
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
            }
            for (int i = 0; i < 500; i++)
            {
                for (int j = 0; j < 1500; j++)
                {
                    Tile tile = Framing.GetTileSafely(i, chasmY + j);
                    int yes = WorldGen.genRand.Next(5, 10);
                    if (TileCheck2(i, chasmY + j) == 1 && j % yes == 0)
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
                    if (TileCheck2(i, chasmY + j) == 2 && j % yes <= 4)
                    {
                        int selection = WorldGen.genRand.Next(10);
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
                            case 6:
                                WorldGen.PlaceTile(i, chasmY + j - 7, ModContent.TileType<BigCoral>());
                                break;
                            case 7:
                                WorldGen.PlaceTile(i, chasmY + j - 7, ModContent.TileType<WavyBigCoral>());
                                break;
                            case 8:
                                WorldGen.PlaceTile(i, chasmY + j - 3, ModContent.TileType<Brain1BigCoral>());
                                break;
                            case 9:
                                WorldGen.PlaceTile(i, chasmY + j - 3, ModContent.TileType<Brain2BigCoral>());
                                break;
                        }
                        if (selection == 5 && j < 300 && Main.rand.NextBool(4))
                            MakeCoral(new Vector2(i, chasmY + j), TileID.Coralstone, Main.rand.Next(4, 8));
                    }
                }
            }


            int barrier = 1000;

            for (int j = 0; j < barrier; j++)
            {
                for (int i = 0; i < Main.maxTilesX; i++)
                {
                    Tile tile = Main.tile[i, j];
                    if (tile.type == ModContent.TileType<HardenedGemsandTile>() || tile.type == ModContent.TileType<GemsandstoneTile>() || tile.type == ModContent.TileType<GemsandTile>())
                    {
                        if (Main.rand.NextBool(2000))
                        {
                            WorldGen.TileRunner(i, j, WorldGen.genRand.Next(4, 8), WorldGen.genRand.Next(5, 7), ModContent.TileType<LythenOreTile>());
                        }
                    }
                }
            }
            for (int j = 0; j < 2; j++)
                MakeOvalJaggedTop(WorldGen.genRand.Next(50, 60), WorldGen.genRand.Next(25, 30), new Vector2(375 + (j * 250) - 25, 1225), ModContent.TileType<GemsandstoneTile>());
            for (int j = 0; j < 2; j++)
                MakeOvalJaggedTop(WorldGen.genRand.Next(50, 60), WorldGen.genRand.Next(25, 30), new Vector2(375 + (j * 250) - 25, 1150), ModContent.TileType<GemsandstoneTile>());

            for (int j = 0; j < 2; j++)
                MakeOvalJaggedTop(WorldGen.genRand.Next(40, 50), WorldGen.genRand.Next(25, 30), new Vector2(450 + (j * 100) - 22, 1180), ModContent.TileType<GemsandstoneTile>());

            for (int j = barrier; j < Main.maxTilesY; j++)
            {
                for (int i = 0; i < Main.maxTilesX; i++)
                {
                    Tile tile = Main.tile[i, j];
                    if (tile.type == ModContent.TileType<HardenedGemsandTile>() || tile.type == ModContent.TileType<GemsandstoneTile>() || tile.type == ModContent.TileType<GemsandTile>())
                    {
                        if (Main.rand.NextBool(2000))
                        {
                            WorldGen.TileRunner(i, j, WorldGen.genRand.Next(4, 8), WorldGen.genRand.Next(5, 7), ModContent.TileType<HydriteOreTile>());
                        }
                    }
                }
            }
        }

        public static void PlaceRuins(int i, int j, int[,] shape)
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
        public static void Island(int islandWidth, int islandHeight)
        {
            /*MakeOvalJaggedBottom(islandWidth, islandHeight, new Vector2((Main.maxTilesX / 2) - islandWidth / 2, 164), ModContent.TileType<CoralSand>());
            MakeOvalJaggedBottom((int)(islandWidth * 0.6), (int)(islandHeight * 0.6), new Vector2((int)((Main.maxTilesX / 2) * 0.66), TileCheck((int)(Main.maxTilesX / 2), ModContent.TileType<CoralSand>()) - 5), TileID.Dirt);
            KillWall(Main.maxTilesX, Main.maxTilesY, Vector2.Zero);

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    WorldGen.SpreadGrass(i, j);
                }
            }
            for (int j = 0; j < Main.maxTilesX; j++)
            {
                if ((Main.rand.NextBool(5)) && (TileCheck(j, ModContent.TileType<CoralSand>()) < TileCheck(j, TileID.Dirt)) && (TileCheck(j, ModContent.TileType<CoralSand>()) < TileCheck(j, TileID.Grass)))
                    WorldGen.PlaceTile(j, TileCheck(j, ModContent.TileType<CoralSand>()) - 1, 324);
            }*/
            MakeAtlantisCastle(75, 100);
        }
        public static void PlaceAtlantisCastleRoom(int i, int j, int[,] shape)
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
                        switch (shape[y, x])
                        {
                            case 1:
                                tile.type = (ushort)ModContent.TileType<AtlanteanBrickTile>();
                                tile.active(true);
                                break;

                            case 2:
                                tile.type = TileID.PalladiumColumn;
                                tile.color(27);
                                Main.tile[k, l].inActive(true);
                                tile.active(true);
                                break;

                            case 3:
                                tile.type = TileID.Platforms;
                                tile.active(true);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        public static void PlaceAtlantisCastleRoomWalls(int i, int j, int[,] shape)
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
                        switch (shape[y, x])
                        {
                            case 1:
                                tile.wall = (ushort)ModContent.WallType<AtlanteanBrickWallTile>();
                                break;

                            case 2:
                                tile.wall = WallID.BlueDungeonTile;
                                tile.wallColor(20);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        public static void MakeAtlantisCastle(int i, int j)
        {
            int x = i;
            int y = j;
            int corriDiff = -24;
            int pillarDiff = -6 + corriDiff;

            int lengthL = Main.rand.Next(10, 15);

            for (int k = 0; k < lengthL; k++)
            {
                if (k == (int)(lengthL / 2))
                {
                    GenerateSubfloors(x, y, corriDiff);
                }
                else
                {
                    PlaceAtlantisCastleRoom(x, y - corriDiff, EEWorld.CastleCorridor);
                    PlaceAtlantisCastleRoomWalls(x, y - corriDiff, EEWorld.CastleCorridorWalls);
                }
                x += 9;
                PlaceAtlantisCastleRoom(x, y - pillarDiff, EEWorld.CastlePillar);
                PlaceAtlantisCastleRoomWalls(x, y - pillarDiff, EEWorld.CastlePillarWalls);
                x += 2;
            }
            PlaceAtlantisCastleRoom(x, y, EEWorld.MainCastleRoom);
            PlaceAtlantisCastleRoomWalls(x, y, EEWorld.MainCastleRoomWalls);
            x += 31;
            PlaceAtlantisCastleRoom(x, y - pillarDiff, EEWorld.CastlePillar);
            PlaceAtlantisCastleRoomWalls(x, y - pillarDiff, EEWorld.CastlePillarWalls);
            x += 2;

            int lengthR = Main.rand.Next(10, 14);

            for (int k = 0; k < lengthR; k++)
            {
                if (k == (int)(lengthL / 2))
                {
                    GenerateSubfloors(x, y, corriDiff);
                }
                else
                {
                    PlaceAtlantisCastleRoom(x, y - corriDiff, EEWorld.CastleCorridor);
                    PlaceAtlantisCastleRoomWalls(x, y - corriDiff, EEWorld.CastleCorridorWalls);
                }
                x += 9;
                PlaceAtlantisCastleRoom(x, y - pillarDiff, EEWorld.CastlePillar);
                PlaceAtlantisCastleRoomWalls(x, y - pillarDiff, EEWorld.CastlePillarWalls);
                x += 2;
            }
        }

        private static void GenerateSubfloors(int x, int y, int corriDiff)
        {
            PlaceAtlantisCastleRoom(x, y - corriDiff + 1, EEWorld.CastleStaircaseCorridor);
            PlaceAtlantisCastleRoomWalls(x, y - corriDiff + 1, EEWorld.CastleStaircaseCorridorWalls);
            int tempY = y + 73;
            PlaceAtlantisCastleRoom(x, tempY, EEWorld.CastleStaircaseUnderground);
            PlaceAtlantisCastleRoomWalls(x, tempY, EEWorld.CastleStaircaseUndergroundWalls);
            int undLength = Main.rand.Next(2, 4);
            int tempX = x - undLength * 11;

            PlaceAtlantisCastleRoom(tempX, tempY, EEWorld.CastleBorderLUnderground);
            PlaceAtlantisCastleRoomWalls(tempX, tempY, EEWorld.CastleBorderLUndergroundWalls);
            tempX += 9;
            PlaceAtlantisCastleRoom(tempX, tempY, EEWorld.CastlePillarUnderground);
            PlaceAtlantisCastleRoomWalls(tempX, tempY, EEWorld.CastlePillarUndergroundWalls);
            tempX += 2;
            for (int m = 0; m < undLength; m++)
            {
                if (m == undLength / 2)
                {
                    PlaceAtlantisCastleRoom(tempX, tempY, EEWorld.CastleStaircaseUndergrounder);
                    PlaceAtlantisCastleRoomWalls(tempX, tempY, EEWorld.CastleStaircaseUndergrounderWalls);
                    int tempY2 = tempY + 22;
                    PlaceAtlantisCastleRoom(tempX, tempY2, EEWorld.CastleStaircaseUndergrounderBottom);
                    PlaceAtlantisCastleRoomWalls(tempX, tempY2, EEWorld.CastleStaircaseUndergrounderBottomWalls);
                    int undLength2 = Main.rand.Next(1, 3);
                    int tempX2 = tempX - undLength2 * 11;

                    PlaceAtlantisCastleRoom(tempX2, tempY2, EEWorld.CastleBorderLUnderground);
                    PlaceAtlantisCastleRoomWalls(tempX2, tempY2, EEWorld.CastleBorderLUndergroundWalls);
                    tempX2 += 9;
                    PlaceAtlantisCastleRoom(tempX2, tempY2, EEWorld.CastlePillarUnderground);
                    PlaceAtlantisCastleRoomWalls(tempX2, tempY2, EEWorld.CastlePillarUndergroundWalls);
                    tempX2 += 2;

                    for (int w = 0; w < undLength2; w++)
                    {
                        PlaceAtlantisCastleRoom(tempX2, tempY2, EEWorld.CastleCorridorUnderground);
                        PlaceAtlantisCastleRoomWalls(tempX2, tempY2, EEWorld.CastleCorridorUndergroundWalls);
                        tempX2 += 9;
                        PlaceAtlantisCastleRoom(tempX2, tempY2, EEWorld.CastlePillarUnderground);
                        PlaceAtlantisCastleRoomWalls(tempX2, tempY2, EEWorld.CastlePillarUndergroundWalls);
                        tempX2 += 2;
                    }

                    for (int z = 0; z < undLength2 - 1; z++)
                    {
                        PlaceAtlantisCastleRoom(tempX2, tempY2, EEWorld.CastleCorridorUnderground);
                        PlaceAtlantisCastleRoomWalls(tempX2, tempY2, EEWorld.CastleCorridorUndergroundWalls);
                        tempX2 += 9;
                        PlaceAtlantisCastleRoom(tempX2, tempY2, EEWorld.CastlePillarUnderground);
                        PlaceAtlantisCastleRoomWalls(tempX2, tempY2, EEWorld.CastlePillarUndergroundWalls);
                        tempX2 += 2;
                    }

                    PlaceAtlantisCastleRoom(tempX2, tempY2, EEWorld.CastleBorderRUnderground);
                    PlaceAtlantisCastleRoomWalls(tempX2, tempY2, EEWorld.CastleBorderRUndergroundWalls);

                    tempX += 9;
                    PlaceAtlantisCastleRoom(tempX, tempY, EEWorld.CastlePillarUnderground);
                    PlaceAtlantisCastleRoomWalls(tempX, tempY, EEWorld.CastlePillarUndergroundWalls);
                    tempX += 2;
                }
                else
                {
                    PlaceAtlantisCastleRoom(tempX, tempY, EEWorld.CastleCorridorUnderground);
                    PlaceAtlantisCastleRoomWalls(tempX, tempY, EEWorld.CastleCorridorUndergroundWalls);
                    tempX += 9;
                    PlaceAtlantisCastleRoom(tempX, tempY, EEWorld.CastlePillarUnderground);
                    PlaceAtlantisCastleRoomWalls(tempX, tempY, EEWorld.CastlePillarUndergroundWalls);
                    tempX += 2;
                }
            }

            tempX = x + 9;
            PlaceAtlantisCastleRoom(tempX, tempY, EEWorld.CastlePillarUnderground);
            PlaceAtlantisCastleRoomWalls(tempX, tempY, EEWorld.CastlePillarUndergroundWalls);
            tempX += 2;

            for (int n = 0; n < undLength - 1; n++)
            {
                PlaceAtlantisCastleRoom(tempX, tempY, EEWorld.CastleCorridorUnderground);
                PlaceAtlantisCastleRoomWalls(tempX, tempY, EEWorld.CastleCorridorUndergroundWalls);
                tempX += 9;
                PlaceAtlantisCastleRoom(tempX, tempY, EEWorld.CastlePillarUnderground);
                PlaceAtlantisCastleRoomWalls(tempX, tempY, EEWorld.CastlePillarUndergroundWalls);
                tempX += 2;
            }

            PlaceAtlantisCastleRoom(tempX, tempY, EEWorld.CastleBorderRUnderground);
            PlaceAtlantisCastleRoomWalls(tempX, tempY, EEWorld.CastleBorderRUndergroundWalls);
        }
    }
}
