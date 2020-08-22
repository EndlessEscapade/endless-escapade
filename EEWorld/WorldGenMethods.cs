using System;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.World.Generation;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EEMod.Tiles;
using EEMod.Tiles.Furniture;
using EEMod.Tiles.Furniture.Coral;
using EEMod.Tiles.Ores;
using EEMod.Tiles.Walls;
using System.Collections.Generic;
using EEMod.Arrays;
using Terraria.ModLoader.IO;
using EEMod.ID;

namespace EEMod.EEWorld
{
    public partial class EEWorld
    {
        public static IList<Vector2> Vines = new List<Vector2>();
        public override void Load(TagCompound tag)
        {
            if (tag.ContainsKey("EntracesPosses"))
            {
                EntracesPosses = tag.GetList<Vector2>("EntracesPosses");
            }
            if (tag.ContainsKey("CoralBoatPos"))
            {
                EESubWorlds.CoralBoatPos = tag.Get<Vector2>("CoralBoatPos");
            }
            if (tag.ContainsKey("SubWorldSpecificVolcanoInsidePos"))
            {
                SubWorldSpecificVolcanoInsidePos = tag.Get<Vector2>("SubWorldSpecificVolcanoInsidePos");
            }
            if (tag.ContainsKey("yes"))
            {
                yes = tag.Get<Vector2>("yes");
            }
            if (tag.ContainsKey("ree"))
            {
                ree = tag.Get<Vector2>("ree");
            }
            if (tag.ContainsKey("ChainConnections"))
            {
                EESubWorlds.ChainConnections = tag.GetList<Vector2>("ChainConnections");
            }
            if (tag.ContainsKey("LightStates"))
            {
                LightStates = tag.GetByteArray("LightStates");
            }
            var downed = new List<string>();
            if (eocFlag) downed.Add("eocFlag");

            IList<string> flags = tag.GetList<string>("boolFlags");

            // Game modes

            // Downed bosses
            downedAkumo = flags.Contains("downedAkumo");
            downedHydros = flags.Contains("downedHydros");
            downedKraken = flags.Contains("downedKraken");
        }
        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound();
            if (Main.ActiveWorldFileData.Name == KeyID.CoralReefs)
            {
                tag["CoralBoatPos"] = EESubWorlds.CoralBoatPos;
                tag["ChainConnections"] = EESubWorlds.ChainConnections;
                tag["LightStates"] = LightStates;
            }
            if (Main.ActiveWorldFileData.Name == KeyID.VolcanoInside)
            {
                tag["SubWorldSpecificVolcanoInsidePos"] = SubWorldSpecificVolcanoInsidePos;
            }
            tag["EntracesPosses"] = EntracesPosses;
            tag["yes"] = yes;
            tag["ree"] = ree;
            return tag;
        }
        /*List<string> boolflags = new List<string>();

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
        };*/
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
                WorldGen.TileRunner(x, y, WorldGen.genRand.Next(3, 7), WorldGen.genRand.Next(5, 7), ModContent.TileType<HydrofluoricOreTile>());

                x = WorldGen.genRand.Next(0, Main.maxTilesX);
                y = WorldGen.genRand.Next(rockLayerLow, Main.maxTilesY);
                WorldGen.TileRunner(x, y, WorldGen.genRand.Next(3, 7), WorldGen.genRand.Next(5, 7), ModContent.TileType<DalantiniumOreTile>());
            }
        }


        public static void FillRegionNoEdit(int width, int height, Vector2 startingPoint, int type)
        {
            string messageBefore = EEMod.progressMessage;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Tile tile = Framing.GetTileSafely(i + (int)startingPoint.X, j + (int)startingPoint.Y);
                    tile.type = (ushort)type;
                    tile.active(true);
                    EEMod.progressMessage = messageBefore;
                    EEMod.progressMessage += $" {(int)((j + (i * height)) / (float)(width * height) * 100)}% done";
                }
            }
            EEMod.progressMessage = messageBefore;
        }
        static PerlinNoiseFunction PNF;
        public static float[] PerlinArray(int width, int seedVar, float amplitude, Vector2 res)
        {
            PNF = new PerlinNoiseFunction(width, seedVar, (int)res.X, (int)res.Y, 0.5f);
            int rand = Main.rand.Next(0, seedVar);
            float[] PerlinStrip = new float[width];
            for(int i = 0; i<width; i++)
            {
                PerlinStrip[i] = PNF.Perlin2[i, rand] * amplitude;
            }
            return PerlinStrip;
        }
        public static float[] PerlinArrayNoZero(int width,float amplitude, Vector2 res, int seedVar = 1000)
        {
            PNF = new PerlinNoiseFunction(width, seedVar, (int)res.X, (int)res.Y, 0.5f);
            int rand = Main.rand.Next(0, seedVar);
            float[] PerlinStrip = new float[width];
            for (int i = 0; i < width; i++)
            {
                PerlinStrip[i] = PNF.Perlin[i, rand] * amplitude;
            }
            return PerlinStrip;
        }
        public static void FillRegionNoEditWithNoise(int width, int height, Vector2 startingPoint, int type)
        {
            string messageBefore = EEMod.progressMessage;
            float[] PerlinStrip = PerlinArray(width,1000,15, new Vector2(60,200));
            for (int i = 0; i < width; i++)
            {
                for (int j = (int)PerlinStrip[i]; j < height; j++)
                {
                    Tile tile = Framing.GetTileSafely(i + (int)startingPoint.X, j + (int)startingPoint.Y);
                    tile.type = (ushort)type;
                    tile.active(true);
                    EEMod.progressMessage = messageBefore;
                    EEMod.progressMessage += $" {(int)((j + (i * height)) / (float)(width * height) * 100)}% done";
                }
            }
            EEMod.progressMessage = messageBefore;
        }
        public static void FillWall(int width, int height, Vector2 startingPoint, int type)
        {
            string messageBefore = EEMod.progressMessage;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    WorldGen.PlaceWall(i + (int)startingPoint.X, j + (int)startingPoint.Y, type);
                    if (EEMod.isSaving)
                    {
                        EEMod.progressMessage = messageBefore;
                        EEMod.progressMessage += $" {(int)((j + (i * height)) / (float)(width * height) * 100)}% done";
                    }
                }
            }
        }
        private static void FillRegionDiag(int width, int height, Vector2 startingPoint, int type, int leftOrRight)
        {
            string messageBefore = EEMod.progressMessage;
            if (leftOrRight == 0)
            {
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height - i; j++)
                    {
                        WorldGen.PlaceTile(i + (int)startingPoint.X, j + (int)startingPoint.Y, type);
                        EEMod.progressMessage = messageBefore;
                        EEMod.progressMessage += $" {(int)((j + (i * height)) / (float)(width * height) * 100)}% done";
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
                        EEMod.progressMessage = messageBefore;
                        EEMod.progressMessage += $" {(int)((j + (i * height)) / (float)(width * height) * 100)}% done";
                    }
                }
            }
            EEMod.progressMessage = messageBefore;
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
                                tile.type = (ushort)ModContent.TileType<LightGemsandTile>();
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
                        switch (shape[y, x])
                        {
                            case 0:
                                if (Main.netMode == NetmodeID.MultiplayerClient) // sync
                                    NetMessage.sendWater(k, l);
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
                                Main.tile[x, y].inActive(true);
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
                                tile.type = (ushort)ModContent.TileType<LightGemsandTile>();
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
                                tile.type = (ushort)ModContent.TileType<LightGemsandTile>();
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
                                tile.type = (ushort)ModContent.TileType<LightGemsandTile>();
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
                                tile.type = (ushort)ModContent.TileType<LightGemsandTile>();
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
                                tile.type = (ushort)ModContent.TileType<LightGemsandTile>();
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
                                tile.type = (ushort)ModContent.TileType<LightGemsandTile>();
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
                                tile.type = (ushort)ModContent.TileType<LightGemsandTile>();
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
                                tile.type = (ushort)ModContent.TileType<LightGemsandTile>();
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
                                Main.tile[x, y].inActive(true);
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
                                Main.tile[x, y].inActive(true);
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
                                Main.tile[x, y].inActive(true);
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
                                tile.type = (ushort)ModContent.TileType<DarkGemsandTile>();
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.LeafBlock;
                                tile.active(true);
                                break;
                            case 3:
                                tile.type = TileID.PalladiumColumn;
                                Main.tile[x, y].inActive(true);
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
                                tile.type = (ushort)ModContent.TileType<DarkGemsandTile>();
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
                                tile.type = (ushort)ModContent.TileType<DarkGemsandTile>();
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
                                tile.type = (ushort)ModContent.TileType<DarkGemsandTile>();
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
                                tile.type = (ushort)ModContent.TileType<DarkGemsandTile>();
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
                                tile.type = (ushort)ModContent.TileType<DarkGemsandTile>();
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
                                tile.type = (ushort)ModContent.TileType<DarkGemsandTile>();
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.LunarBlockStardust;
                                tile.active(true);
                                break;
                            case 3:
                                tile.type = TileID.PalladiumColumn;
                                Main.tile[x, y].inActive(true);
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
                                tile.type = (ushort)ModContent.TileType<DarkGemsandTile>();
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
                                tile.type = (ushort)ModContent.TileType<LightGemsandTile>();
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.LunarBlockStardust;
                                tile.active(true);
                                break;
                            case 3:
                                tile.type = TileID.PalladiumColumn;
                                Main.tile[x, y].inActive(true);
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
                                Main.tile[x, y].inActive(true);
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
                                tile.type = (ushort)ModContent.TileType<LightGemsandTile>();
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
                                WorldGen.PlaceWall(k, l, WallID.SandstoneBrick);
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
                                WorldGen.PlaceWall(k, l, WallID.Dirt);
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
            if (wallShape != null && walls != null)
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
            PlaceShipWalls(100, TileCheckWater(100) - 22, ShipWalls);
            PlaceShip(100, TileCheckWater(100) - 22, ShipTiles);
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
            string messageBefore = EEMod.progressMessage;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Tile tile = Framing.GetTileSafely(i + (int)startingPoint.X, j + (int)startingPoint.Y);
                    tile.ClearTile();
                    WorldGen.KillWall(i + (int)startingPoint.X, j + (int)startingPoint.Y);
                    EEMod.progressMessage = messageBefore;
                    EEMod.progressMessage += $" {(int)((j + (i * height)) / (float)(width * height) * 100)}% done";
                }
            }
        }
        public static void ClearRegionSafely(int width, int height, Vector2 startingPoint, int type)
        {
            string messageBefore = EEMod.progressMessage;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (Framing.GetTileSafely(i + (int)startingPoint.X, j + (int)startingPoint.Y).type == type)
                    {
                        WorldGen.KillTile(i + (int)startingPoint.X, j + (int)startingPoint.Y);
                        //WorldGen.KillWall(i + (int)startingPoint.X, j + (int)startingPoint.Y);
                        EEMod.progressMessage = messageBefore;
                        EEMod.progressMessage += $" {(int)((j + (i * height)) / (float)(width * height) * 100)}% done";
                    }
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
                                WorldGen.PlaceTile(k, l, ModContent.TileType<VolcanicAshTile>());
                                break;
                            case 2:
                                WorldGen.PlaceWall(k, l, WallID.DiamondGemspark);
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
                    if (tile.type == TileID.Stone)
                    {
                        if (Main.rand.NextBool(2000))
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
            Tile tile6 = Framing.GetTileSafely(i - 1, j);
            Tile tile7 = Framing.GetTileSafely(i - 2, j);
            Tile tile8 = Framing.GetTileSafely(i + 1, j);
            Tile tile9 = Framing.GetTileSafely(i + 2, j);
            if (tile1.active() && tile2.active() && tile3.active() && !tile4.active() && !tile5.active())
            {
                return 1;
            }
            if (tile1.active() && !tile2.active() && !tile3.active() && tile4.active() && tile5.active())
            {
                return 2;
            }
            if (tile1.active() && tile6.active() && tile7.active() && !tile8.active() && !tile9.active())
            {
                return 3;
            }
            if (tile1.active() && !tile6.active() && !tile7.active() && tile8.active() && tile9.active())
            {
                return 4;
            }
            else
            {
                return 0;
            }
        }
        public static int WaterCheck(int i, int j)
        {
            Tile tile1 = Framing.GetTileSafely(i, j);
            Tile tile2 = Framing.GetTileSafely(i, j - 1);
            Tile tile3 = Framing.GetTileSafely(i, j - 2);
            Tile tile4 = Framing.GetTileSafely(i, j + 1);
            Tile tile5 = Framing.GetTileSafely(i, j + 2);
            Tile tile6 = Framing.GetTileSafely(i - 1, j);
            Tile tile7 = Framing.GetTileSafely(i - 2, j);
            Tile tile8 = Framing.GetTileSafely(i + 1, j);
            Tile tile9 = Framing.GetTileSafely(i + 2, j);
            if (tile1.active() && tile2.active() && tile3.active() && !tile4.active() && !tile5.active())
            {
                return 1;
            }
            if (tile1.active() && !tile2.active() && !tile3.active() && tile4.active() && tile5.active())
            {
                return 2;
            }
            if (tile1.active() && tile6.active() && tile7.active() && !tile8.active() && !tile9.active())
            {
                return 3;
            }
            if (tile1.active() && !tile6.active() && !tile7.active() && tile8.active() && tile9.active())
            {
                return 4;
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
                    if (j > height / 2)
                    {
                        if (OvalCheck((int)(startingPoint.X + width / 2), (int)(startingPoint.Y + height / 2), i + (int)startingPoint.X, j + (int)startingPoint.Y, (int)(width * .5f), (int)(height * .5f)))
                            WorldGen.PlaceTile(i + (int)startingPoint.X, j + (int)startingPoint.Y, type);
                    }
                    if (i == width / 2 && j == height / 2)
                    {
                        WorldGen.TileRunner(i + (int)startingPoint.X, j + (int)startingPoint.Y + 2, WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(10, 20), type, true, 0f, 0f, true, true);
                    }
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
                        WorldGen.PlaceTile(i + (int)startingPoint.X, j + (int)startingPoint.Y, type, false, forced);

                    if (i == width / 2 && j == height / 2)
                    {
                        WorldGen.TileRunner(i + (int)startingPoint.X, j + (int)startingPoint.Y + 2, WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(10, 20), type, true, 0f, 0f, true, true);
                    }
                }
            }
        }
        public static void MakeCircle(int size, Vector2 startingPoint, int type, bool forced)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    float f = size * 1.5f;
                    if (Vector2.DistanceSquared(new Vector2(i + (int)startingPoint.X, j + (int)startingPoint.Y), startingPoint + new Vector2(size * 0.5f, size * 0.5f)) < f * f)
                        WorldGen.PlaceTile(i + (int)startingPoint.X, j + (int)startingPoint.Y, type, false, forced);
                }
            }
        }
        public static void MakeJaggedOval(int width, int height, Vector2 startingPoint, int type, bool forced)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (OvalCheck((int)(startingPoint.X + width / 2), (int)(startingPoint.Y + height / 2), i + (int)startingPoint.X, j + (int)startingPoint.Y, (int)(width * .5f), (int)(height * .5f)))
                        WorldGen.TileRunner(i + (int)startingPoint.X, j + (int)startingPoint.Y, WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(5, 10), type, true, 0f, 0f, true, true);

                    if (i == width / 2 && j == height / 2)
                    {
                        WorldGen.TileRunner(i + (int)startingPoint.X, j + (int)startingPoint.Y + 2, WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(10, 20), type, true, 0f, 0f, true, true);
                    }
                }
            }
        }
        public static void MakeKramkenArena(int xPos, int yPos, int size)
        {
            int maxTiles = (int)(Main.maxTilesX * Main.maxTilesY * 9E-04);
            for (int k = 0; k < maxTiles * 60; k++)
            {
                int x = WorldGen.genRand.Next(xPos - (size * 2), xPos + (size * 2));
                int y = WorldGen.genRand.Next(yPos - (size * 2), yPos + (size * 2));
                if (OvalCheck(xPos, yPos, x, y, size * 2, size))
                    WorldGen.TileRunner(x, y, WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(5, 10), TileID.StoneSlab, true, 0f, 0f, true, true);
            }
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (tile.type == TileID.StoneSlab)
                        WorldGen.KillTile(i, j);
                }
            }
        }
        public static void RemoveStoneSlabs()
        {
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (tile.type == TileID.StoneSlab)
                        WorldGen.KillTile(i, j);
                }
            }
        }
        public static PerlinNoiseFunction perlinNoise;
        public static void PlaceKelp(int height, Vector2 startingPoint)
        {
            for (int i = 0; i < height; i++)
            {
                Tile tile = Framing.GetTileSafely((int)startingPoint.X, (int)startingPoint.Y - i);
                if (!tile.active())
                    tile.type = (ushort)ModContent.TileType<KelpTile>();
                tile.active(true);
            }
        }
        public delegate bool NoiseConditions(Vector2 point);
        public static void NoiseGen(Vector2 topLeft, Vector2 size, Vector2 dimensions, float thresh, ushort type, NoiseConditions noiseFilter = null)
        {
               perlinNoise = new PerlinNoiseFunction((int)size.X, (int)size.Y, (int)dimensions.X, (int)dimensions.Y, thresh);
               int[,] perlinNoiseFunction = perlinNoise.PerlinBinary;
               for (int i = (int)topLeft.X; i < (int)topLeft.X + (int)size.X; i++)
               {
                 for (int j = (int)topLeft.Y; j < (int)topLeft.Y + (int)size.Y; j++)
                 {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (perlinNoiseFunction[i - (int)topLeft.X, j - (int)topLeft.Y] == 1)
                    {
                        WorldGen.PlaceTile(i, j, type);
                    }
                 }
               }
        }
        public static void NoiseGenWave(Vector2 topLeft, Vector2 size, Vector2 dimensions, ushort type, float thresh, NoiseConditions noiseFilter = null)
        {
            PerlinNoiseFunction perlinNoise = new PerlinNoiseFunction((int)size.X, (int)size.Y, (int)dimensions.X, (int)dimensions.Y, thresh);
            int[,] perlinNoiseFunction = perlinNoise.PerlinBinary;
            float[] disp = PerlinArrayNoZero((int)size.X, size.Y*0.5f,new Vector2(50,100));
            for (int i = (int)topLeft.X; i < (int)topLeft.X + (int)size.X; i++)
            {
                for (int j = (int)topLeft.Y + (int)disp[i - (int)topLeft.X]; j < (int)topLeft.Y + (int)size.Y; j++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (perlinNoiseFunction[i - (int)topLeft.X, j - (int)topLeft.Y] == 1)
                    {
                        WorldGen.PlaceTile(i, j, type);
                        if (j < Main.maxTilesY * 0.33f)
                        {
                            WorldGen.PlaceTile(i, j, (ushort)ModContent.TileType<LightGemsandTile>());
                        }
                        else if (j < Main.maxTilesY * 0.66f)
                        {
                            WorldGen.PlaceTile(i, j, (ushort)ModContent.TileType<GemsandTile>());
                        }
                        else if (j > Main.maxTilesY * 0.66f)
                        {
                            WorldGen.PlaceTile(i, j, (ushort)ModContent.TileType<DarkGemsandTile>());
                        }
                        if (j < Main.maxTilesY / 10)
                        {
                            WorldGen.PlaceTile(i, j, (ushort)ModContent.TileType<CoralSand>());
                        }
                    }
                }
            }
        }
        public static void MakeCoralRoom(int xPos, int yPos, int size, int type, int foliage, bool ensureNoise = false)
        {
            int sizeX = size;
            int sizeY = size / 2;
            Vector2 TL = new Vector2(xPos - sizeX / 2f, yPos - sizeY / 2f);
            Vector2 BR = new Vector2(xPos + sizeX / 2f, yPos + sizeY / 2f);

            Vector2 startingPoint = new Vector2(xPos - sizeX, yPos - sizeY);
            int tile2;
            if (TL.Y < Main.maxTilesY * 0.33f)
            {
                tile2 = (ushort)ModContent.TileType<LightGemsandTile>();
            }
            else if (TL.Y < Main.maxTilesY * 0.66f)
            {
                tile2 = (ushort)ModContent.TileType<GemsandTile>();
            }
            else
            {
                tile2 = (ushort)ModContent.TileType<DarkGemsandTile>();
            }
            if (TL.Y < Main.maxTilesY / 10)
            {
                tile2 = (ushort)ModContent.TileType<CoralSand>();
            }
            void CreateNoise(bool ensureN, int width, int height, float thresh)
            {
                perlinNoise = new PerlinNoiseFunction(1000, 1000, width, height, thresh);
                int[,] perlinNoiseFunction = perlinNoise.PerlinBinary;
                if (ensureN)
                {
                    for (int i = (int)startingPoint.X; i < (int)startingPoint.X + sizeX * 2; i++)
                    {
                        for (int j = (int)startingPoint.Y; j < (int)startingPoint.Y + sizeY * 2; j++)
                        {
                            if (perlinNoiseFunction[i - (int)startingPoint.X, j - (int)startingPoint.Y] == 1 && OvalCheck(xPos, yPos, i, j, sizeX, sizeY) && WorldGen.InWorld(i, j))
                            {
                                Tile tile = Framing.GetTileSafely(i, j);
                                if (j < Main.maxTilesY * 0.33f)
                                {
                                    tile.type = (ushort)ModContent.TileType<LightGemsandTile>();
                                }
                                else if (j < Main.maxTilesY * 0.66f)
                                {
                                    tile.type = (ushort)ModContent.TileType<GemsandTile>();
                                }
                                else if (j > Main.maxTilesY * 0.66f)
                                {
                                    tile.type = (ushort)ModContent.TileType<DarkGemsandTile>();
                                }

                                if (j < Main.maxTilesY / 10)
                                {
                                    tile.type = (ushort)ModContent.TileType<CoralSand>();
                                }
                            }
                        }
                    }
                }
            }
            RemoveStoneSlabs();
            switch (type)
            {
                case -1:
                    MakeJaggedOval(sizeX, sizeY, new Vector2(TL.X, TL.Y), TileID.StoneSlab, true);
                    MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos, yPos) + new Vector2(0, 0), tile2);
                    MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos, yPos) + new Vector2(-sizeX / 5 - sizeX / 6, -sizeY / 5 - sizeY / 6), tile2);
                    MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos, yPos) + new Vector2(sizeX / 5 - sizeX / 6, -sizeY / 5 - sizeY / 6), tile2);
                    MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos, yPos) + new Vector2(sizeX / 5 - sizeX / 6, sizeY / 5 - sizeY / 6), tile2);
                    MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos, yPos) + new Vector2(-sizeX / 5 - sizeX / 6, sizeY / 5 - sizeY / 6), tile2);
                    break;
                case 0:
                    MakeJaggedOval(sizeX, sizeY, new Vector2(TL.X, TL.Y), TileID.StoneSlab, true);
                    MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos, yPos) + new Vector2(0, 0), tile2);
                    MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos, yPos) + new Vector2(-sizeX / 5, -sizeY / 5), tile2);
                    MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos, yPos) + new Vector2(sizeX / 5, -sizeY / 5), tile2);
                    MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos, yPos) + new Vector2(sizeX / 5, sizeY / 5), tile2);
                    MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos, yPos) + new Vector2(-sizeX / 5, sizeY / 5), tile2);
                    CreateNoise(!ensureNoise, 100, 10, 0.45f);
                    break;
                case 1:
                    MakeJaggedOval(sizeX, sizeY, new Vector2(TL.X, TL.Y), TileID.StoneSlab, true);
                    CreateNoise(!ensureNoise, 20, 200, 0.5f);
                    break;
                case 2:
                    MakeJaggedOval(sizeX, sizeY, new Vector2(TL.X, TL.Y), TileID.StoneSlab, true);
                    CreateNoise(!ensureNoise, 200, 20, 0.5f);
                    break;
                case 3:
                    MakeJaggedOval(sizeX, sizeY, new Vector2(TL.X, TL.Y), TileID.StoneSlab, true);
                    MakeWavyChasm3(new Vector2(TL.X - 50 + Main.rand.Next(-30, 30), TL.Y - 10), new Vector2(BR.X - 50 + Main.rand.Next(-30, 30), BR.Y - 10), tile2, 100, 4, true, new Vector2(10, 13), 50, 20);
                    MakeWavyChasm3(new Vector2(TL.X + 50 + Main.rand.Next(-30, 30), yPos + 10), new Vector2(BR.X + 50 + Main.rand.Next(-30, 30), yPos + 10), tile2, 100, 4, true, new Vector2(10, 13), 50, 20);
                    MakeWavyChasm3(new Vector2(TL.X + Main.rand.Next(-30, 30), TL.Y - 10), new Vector2(BR.X + Main.rand.Next(-30, 30), BR.Y - 10), tile2, 100, 4, true, new Vector2(10, 13), 50, 20);
                    MakeWavyChasm3(new Vector2(TL.X + Main.rand.Next(-100, 100), TL.Y - 10), new Vector2(BR.X + Main.rand.Next(-30, 30), BR.Y - 10), tile2, 100, 4, true, new Vector2(10, 13), 50, 20);
                    MakeWavyChasm3(new Vector2(TL.X + Main.rand.Next(-100, 100), yPos - 10), new Vector2(BR.X + Main.rand.Next(-30, 30), BR.Y - 10), tile2, 100, 4, true, new Vector2(10, 13), 50, 20);
                    break;
                case 4:
                    MakeJaggedOval(sizeX, sizeY, new Vector2(TL.X, TL.Y), TileID.StoneSlab, true);
                    for (int i = 0; i < 20; i++)
                        MakeCircle(WorldGen.genRand.Next(5, 20), new Vector2(TL.X + WorldGen.genRand.Next(sizeX), TL.Y + WorldGen.genRand.Next(sizeY)), tile2, true);
                    break;
                case 5:
                    MakeJaggedOval(sizeX, sizeY * 2, new Vector2(TL.X, yPos - sizeY), TileID.StoneSlab, true);
                    MakeJaggedOval((int)(sizeX * 0.8f), (int)(sizeY * 1.6f), new Vector2(xPos - sizeX * 0.4f, yPos - sizeY * 0.8f), tile2, true);
                    MakeJaggedOval(sizeX / 10, sizeY / 5, new Vector2(xPos - sizeX / 20, yPos - sizeY / 10), TileID.StoneSlab, true);
                    for (int i = 0; i < 30; i++)
                        MakeCircle(WorldGen.genRand.Next(5, 20), new Vector2(TL.X + WorldGen.genRand.Next(sizeX), yPos - sizeY + WorldGen.genRand.Next(sizeY * 2)), TileID.StoneSlab, true);
                    break;
                case 6:
                    MakeJaggedOval((int)(sizeX * 1.3f), sizeY, new Vector2(TL.X, TL.Y), TileID.StoneSlab, true);
                    MakeWavyChasm3(new Vector2(TL.X - 50 + Main.rand.Next(-30, 30), TL.Y - 10), new Vector2(BR.X - 50 + Main.rand.Next(-30, 30), BR.Y - 10), tile2, 100, 4, true, new Vector2(10, 13), 50, 20);
                    MakeWavyChasm3(new Vector2(TL.X + 50 + Main.rand.Next(-30, 30), yPos + 10), new Vector2(BR.X + 50 + Main.rand.Next(-30, 30), yPos + 10), tile2, 100, 4, true, new Vector2(10, 13), 50, 20);
                    MakeWavyChasm3(new Vector2(TL.X + Main.rand.Next(-30, 30), TL.Y - 10), new Vector2(BR.X + Main.rand.Next(-30, 30), BR.Y - 10), tile2, 100, 4, true, new Vector2(10, 13), 50, 20);
                    MakeWavyChasm3(new Vector2(TL.X + Main.rand.Next(-100, 100), TL.Y - 10), new Vector2(BR.X + Main.rand.Next(-30, 30), BR.Y - 10), tile2, 100, 4, true, new Vector2(10, 13), 50, 20);
                    MakeWavyChasm3(new Vector2(TL.X + Main.rand.Next(-100, 100), yPos - 10), new Vector2(BR.X + Main.rand.Next(-30, 30), BR.Y - 10), tile2, 100, 4, true, new Vector2(10, 13), 50, 20);
                    CreateNoise(true, 100, 20, 0.2f);
                    break;
            }
            CreateNoise(ensureNoise, 50, 20, 0.5f);
            for (int i = -20; i < sizeX + 20; i++)
            {
                for (int j = -20; j < sizeY + 20; j++)
                {
                    Vector2 basePos = new Vector2(i + xPos - size / 2f, j + yPos - size / 4f);
                    if (WorldGen.InWorld((int)basePos.X, (int)basePos.Y,20))
                    {
                        switch (foliage)
                        {
                            case 0:
                                if (TileCheck2((int)basePos.X, (int)basePos.Y) == 1 && !WorldGen.genRand.NextBool(5))
                                {
                                    int selection = WorldGen.genRand.Next(6);
                                    switch (selection)
                                    {
                                        case 0:
                                            WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y + 1, ModContent.TileType<HangingCoral1>());
                                            break;
                                        case 1:
                                            WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y + 1, ModContent.TileType<HangingCoral2>());
                                            break;
                                        case 2:
                                            WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y + 1, ModContent.TileType<HangingCoral3>());
                                            break;
                                        case 3:
                                            WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y + 1, ModContent.TileType<HangingCoral4>());
                                            break;
                                        case 4:
                                            WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y + 1, ModContent.TileType<HangingCoral5>());
                                            break;
                                        case 5:
                                            WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y + 1, ModContent.TileType<HangingCoral6>());
                                            break;
                                    }
                                }
                                if (TileCheck2((int)basePos.X, (int)basePos.Y) == 2 && !WorldGen.genRand.NextBool(6))
                                {
                                    int selection = WorldGen.genRand.Next(13);
                                    switch (selection)
                                    {
                                        case 0:
                                            WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y - 8, ModContent.TileType<CoralStack1>());
                                            break;
                                        case 1:
                                            WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y - 8, ModContent.TileType<CoralStack2>());
                                            break;
                                        case 2:
                                            WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y - 3, ModContent.TileType<MediumCoral>(), style: WorldGen.genRand.Next(0, 2));
                                            break;
                                        case 3:
                                            WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y - 2, ModContent.TileType<ShortCoral>(), style: WorldGen.genRand.Next(0, 3));
                                            break;
                                        case 4:
                                            WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y - 1, ModContent.TileType<SingleCoral>(), style: WorldGen.genRand.Next(0, 3));
                                            break;
                                        case 5:
                                            WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y - 2, ModContent.TileType<SquareCoral>(), style: WorldGen.genRand.Next(0, 5));
                                            break;
                                        case 6:
                                            WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y - 7, ModContent.TileType<BigCoral1>());
                                            break;
                                        case 7:
                                            WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y - 7, ModContent.TileType<BigCoral2>());
                                            break;
                                        case 8:
                                            WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y - 6, ModContent.TileType<TallCoral>());
                                            break;
                                        case 9:
                                            WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y - 3, ModContent.TileType<Brain1BigCoral>());
                                            break;
                                        case 10:
                                            WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y - 3, ModContent.TileType<Brain2BigCoral>());
                                            break;
                                        case 11:
                                            PlaceKelp(WorldGen.genRand.Next(sizeY / 30, sizeY / 20), new Vector2((int)basePos.X, (int)basePos.Y - 1));
                                            break;
                                        case 12:
                                            switch (WorldGen.genRand.Next(3))
                                            {
                                                case 0:
                                                    WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y - 2, ModContent.TileType<GlowCoral1>());
                                                    break;
                                                case 1:
                                                    WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y - 2, ModContent.TileType<GlowCoral2>());
                                                    break;
                                                case 2:
                                                    WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y - 2, ModContent.TileType<GlowCoral3>());
                                                    break;
                                            }
                                            break;
                                    }
                                }
                                break;
                            case 1:
                                if (TileCheck2((int)basePos.X, (int)basePos.Y) == 2 && !WorldGen.genRand.NextBool(6))
                                {
                                    if (WorldGen.genRand.NextBool())
                                    {
                                        PlaceKelp(WorldGen.genRand.Next(sizeY / 10, sizeY / 3), new Vector2((int)basePos.X, (int)basePos.Y - 1));
                                    }
                                    else
                                    {
                                        int selection = WorldGen.genRand.Next(2);
                                        switch (selection)
                                        {
                                            case 0:
                                                WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y - 2, ModContent.TileType<ShortCoral>(), style: WorldGen.genRand.Next(0, 3));
                                                break;
                                            case 1:
                                                WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y - 1, ModContent.TileType<SingleCoral>(), style: WorldGen.genRand.Next(0, 3));
                                                break;
                                        }
                                    }
                                }
                                break;
                            case 2:
                                if (TileCheck2((int)basePos.X, (int)basePos.Y) == 2 && WorldGen.genRand.NextBool() && !WorldGen.genRand.NextBool(6))
                                {
                                    if (WorldGen.genRand.NextBool())
                                    {
                                        WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y - 3, ModContent.TileType<ThermalVent>());
                                    }
                                    else
                                    {
                                        int selection = WorldGen.genRand.Next(6);
                                        switch (selection)
                                        {
                                            case 0:
                                                WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y - 2, ModContent.TileType<SquareCoral>(), style: 3);
                                                break;
                                            case 1:
                                                WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y - 2, ModContent.TileType<GlowCoral1>());
                                                break;
                                            case 2:
                                                WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y - 2, ModContent.TileType<GlowCoral2>());
                                                break;
                                            case 3:
                                                WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y - 1, ModContent.TileType<SingleCoral>(), style: 0);
                                                break;
                                            case 4:
                                                WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y - 1, ModContent.TileType<Brain1BigCoral>(), style: 0);
                                                break;
                                            case 5:
                                                WorldGen.PlaceTile((int)basePos.X, (int)basePos.Y - 1, ModContent.TileType<Brain2BigCoral>(), style: 0);
                                                break;
                                        }
                                    }
                                }
                                break;
                            case 3:
                                //Currents
                                break;
                            case 4:
                                //Nothing
                                break;
                        }
                    }
                }
            }

        }
        public static void PlaceCoral()
        {
            for (int i = 42; i < Main.maxTilesX - 42; i++)
            {
                for (int j = 42; j < Main.maxTilesY - 42; j++)
                {
                    if (TileCheck2(i, j) == 1 && !WorldGen.genRand.NextBool(5) && WorldGen.InWorld(i, j))
                    {
                        int selection = WorldGen.genRand.Next(8);
                        switch (selection)
                        {
                            case 0:
                                WorldGen.PlaceTile(i, j + 1, ModContent.TileType<HangingCoral1>());
                                break;
                            case 1:
                                WorldGen.PlaceTile(i, j + 1, ModContent.TileType<HangingCoral2>());
                                break;
                            case 2:
                                WorldGen.PlaceTile(i, j + 1, ModContent.TileType<HangingCoral3>());
                                break;
                            case 3:
                                WorldGen.PlaceTile(i, j + 1, ModContent.TileType<HangingCoral4>());
                                break;
                            case 4:
                                WorldGen.PlaceTile(i, j + 1, ModContent.TileType<HangingCoral5>());
                                break;
                            case 5:
                                WorldGen.PlaceTile(i, j + 1, ModContent.TileType<HangingCoral6>());
                                break;
                            case 6:
                                ModContent.GetInstance<GlowHangCoral1TE>().Place(i, j + 1);
                                WorldGen.PlaceTile(i, j + 1, ModContent.TileType<GlowHangCoral1>());
                                break;
                            case 7:
                                ModContent.GetInstance<GlowHangCoral2TE>().Place(i, j + 1);
                                WorldGen.PlaceTile(i, j + 1, ModContent.TileType<GlowHangCoral2>());
                                break;
                        }
                    }
                    if (TileCheck2(i, j) == 2 && !WorldGen.genRand.NextBool(6) && WorldGen.InWorld(i, j))
                    {
                        int selection = WorldGen.genRand.Next(13);
                        switch (selection)
                        {
                            case 0:
                                WorldGen.PlaceTile(i, j - 8, ModContent.TileType<CoralStack1>());
                                break;
                            case 1:
                                WorldGen.PlaceTile(i, j - 8, ModContent.TileType<CoralStack2>());
                                break;
                            case 2:
                                WorldGen.PlaceTile(i, j - 3, ModContent.TileType<MediumCoral>(), style: WorldGen.genRand.Next(0, 2));
                                break;
                            case 3:
                                WorldGen.PlaceTile(i, j - 2, ModContent.TileType<ShortCoral>(), style: WorldGen.genRand.Next(0, 3));
                                break;
                            case 4:
                                WorldGen.PlaceTile(i, j - 1, ModContent.TileType<SingleCoral>(), style: WorldGen.genRand.Next(0, 3));
                                break;
                            case 5:
                                WorldGen.PlaceTile(i, j - 2, ModContent.TileType<SquareCoral>(), style: WorldGen.genRand.Next(0, 5));
                                break;
                            case 6:
                                WorldGen.PlaceTile(i, j - 7, ModContent.TileType<BigCoral1>());
                                break;
                            case 7:
                                WorldGen.PlaceTile(i, j - 7, ModContent.TileType<BigCoral2>());
                                break;
                            case 8:
                                WorldGen.PlaceTile(i, j - 6, ModContent.TileType<TallCoral>());
                                break;
                            case 9:
                                WorldGen.PlaceTile(i, j - 3, ModContent.TileType<Brain1BigCoral>());
                                break;
                            case 10:
                                WorldGen.PlaceTile(i, j - 3, ModContent.TileType<Brain2BigCoral>());
                                break;
                            case 11:
                                PlaceKelp(WorldGen.genRand.Next(3, 9), new Vector2(i, j - 1));
                                break;
                            case 12:
                                switch (WorldGen.genRand.Next(3))
                                {
                                    case 0:
                                        WorldGen.PlaceTile(i, j - 2, ModContent.TileType<GlowCoral1>());
                                        break;
                                    case 1:
                                        WorldGen.PlaceTile(i, j - 2, ModContent.TileType<GlowCoral2>());
                                        break;
                                    case 2:
                                        WorldGen.PlaceTile(i, j - 2, ModContent.TileType<GlowCoral3>());
                                        break;
                                }
                                break;
                        }
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
                width -= 2;
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
                        Tile tile = Framing.GetTileSafely(i, j);
                        if (tile.type == TileID.StoneSlab)
                        {
                            WorldGen.KillTile(i, j);
                            if (wallType != 0)
                                tile.wall = (ushort)wallType;
                        }
                    }
                }
            }
            if (isFlat)
            {
                ClearRegion(initialWidth, height / 5, new Vector2(initialStartingPosX, topRight - 5));
                KillWall(initialWidth, height / 5, new Vector2(initialStartingPosX, topRight - 5));
            }
        }
        public static void FillRegion(int width, int height, Vector2 startingPoint, int type)
        {
            string messageBefore = EEMod.progressMessage;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Tile tile = Framing.GetTileSafely(i + (int)startingPoint.X, j + (int)startingPoint.Y);
                    tile.type = (ushort)type;
                    tile.active(true);
                    EEMod.progressMessage = messageBefore;
                    EEMod.progressMessage += $" {(int)((j + (i * height)) / (float)(width * height) * 100)}% done";
                }
            }
            EEMod.progressMessage = messageBefore;
        }
        public static void MakeCoral(Vector2 startingPoint, int type, int strength)
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
                WorldGen.TileRunner(positionX + (int)(i * slant) + (int)(Math.Sin(i / (float)50) * (20 * (1 + (i * 1.5f / (float)height)))), positionY + i, WorldGen.genRand.Next(5 + sizeAddon / 2, 10 + sizeAddon), WorldGen.genRand.Next(10, 12), type, true, 0f, 0f, true, Override);
            }
        }
        public static void MakeWavyChasm3(Vector2 position1, Vector2 position2, int type, int accuracy, int sizeAddon, bool Override, Vector2 stepBounds, int waveInvolvment = 0, float frequency = 5, bool withBranches = false, int branchFrequency = 0, int lengthOfBranches = 0)
        {
            for (int i = 0; i < accuracy; i++)
            {
                // Tile tile = Framing.GetTileSafely(positionX + (int)(i * slant), positionY + i);
                float perc = i / (float)accuracy;
                Vector2 currentPos = new Vector2(position1.X + (perc * (position2.X - position1.X)), position1.Y + (perc * (position2.Y - position1.Y)));
                WorldGen.TileRunner((int)currentPos.X + (int)(Math.Sin(i / frequency) * waveInvolvment),
                    (int)currentPos.Y,
                    WorldGen.genRand.Next(5 + sizeAddon / 2, 10 + sizeAddon),
                    WorldGen.genRand.Next((int)stepBounds.X, (int)stepBounds.Y),
                    type,
                    true,
                    0f,
                    0f,
                    true,
                    Override)
                    ;
                if (withBranches)
                {
                    if (i % branchFrequency == 0 && WorldGen.genRand.Next(2) == 0)
                    {

                        int Side = Main.rand.Next(0, 2);
                        if (Side == 0)
                        {
                            Vector2 NormalizedGradVec = Vector2.Normalize(position2 - position1).RotatedBy((float)Math.PI / 2 + Main.rand.NextFloat(-0.3f, 0.3f));
                            //int ChanceForRecursion = Main.rand.Next(0, 4);
                            MakeWavyChasm3(currentPos, currentPos + NormalizedGradVec * lengthOfBranches, type, 100, 10, true, new Vector2(0, 5), 2, 5, true, 50, (int)(lengthOfBranches * 0.5f));
                        }
                        if (Side == 1)
                        {
                            Vector2 NormalizedGradVec = Vector2.Normalize(position2 - position1).RotatedBy(-(float)Math.PI / 2);
                            //int ChanceForRecursion = Main.rand.Next(0, 4);
                            MakeWavyChasm3(currentPos, currentPos + NormalizedGradVec * lengthOfBranches, type, 100, 10, true, new Vector2(0, 5), 7, 5, true, 50, (int)(lengthOfBranches * 0.5f));
                        }
                    }
                }
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

        public static bool OvalCheck(int midX, int midY, int x, int y, int sizeX, int sizeY)
        {
            double p = Math.Pow(x - midX, 2) / Math.Pow(sizeX, 2)
                    + Math.Pow(y - midY, 2) / Math.Pow(sizeY, 2);

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
            for (int j = 0; j < islandPositions.Count; j++)
            {
                // MakeOvalFlatTop(40, 13, new Vector2(islandPositions[j].X - 15, islandPositions[j].Y), ModContent.TileType<HardenedGemsandTile>());
                switch (listNumbers[j])
                {
                    case 0:
                        {
                            PlaceM1((int)islandPositions[j].X - M1.GetLength(0) / 2, (int)islandPositions[j].Y - M1.GetLength(1) / 2, M1);
                            break;
                        }
                    case 1:
                        {
                            PlaceM2((int)islandPositions[j].X - M2.GetLength(0) / 2, (int)islandPositions[j].Y - M2.GetLength(1) / 2, M2);
                            break;
                        }
                    case 2:
                        {
                            PlaceM3((int)islandPositions[j].X - M3.GetLength(0) / 2, (int)islandPositions[j].Y - M3.GetLength(1) / 2, M3);
                            break;
                        }
                    case 3:
                        {
                            PlaceBlackSmith((int)islandPositions[j].X - Blacksmith.GetLength(0) / 2, (int)islandPositions[j].Y - Blacksmith.GetLength(1) / 2, Blacksmith);
                            break;
                        }
                    case 4:
                        {
                            PlaceM4Temple((int)islandPositions[j].X - M4Temple.GetLength(0) / 2, (int)islandPositions[j].Y - M4Temple.GetLength(1) / 2, M4Temple);
                            break;
                        }
                    case 5:
                        {
                            PlaceBrewery((int)islandPositions[j].X - Brewery.GetLength(0) / 2, (int)islandPositions[j].Y - Brewery.GetLength(1) / 2, Brewery);
                            break;
                        }
                    case 6:
                        {
                            PlaceHeadQ((int)islandPositions[j].X - HeadQ.GetLength(0) / 2, (int)islandPositions[j].Y - HeadQ.GetLength(1) / 2, HeadQ);
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
            MakeLayerWithOutline((int)midpoint.X, 70 + (int)startingPoint.Y, 20, 1, ModContent.TileType<LightGemsandTile>(), 10);
            for (int j = 0; j < 3; j++)
            {
                switch (listNumbersHighClass[j])
                {
                    case 0:
                        {
                            PlaceH2((int)midpoint.X - (int)displacement - H2.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 - H2.GetLength(1) / 2, H2);
                            break;
                        }
                    case 1:
                        {
                            PlaceLootRoom((int)midpoint.X - (int)displacement - LootRoom.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 - LootRoom.GetLength(1) / 2, LootRoom);
                            break;
                        }
                    case 2:
                        {
                            PlaceH1((int)midpoint.X - (int)displacement - H1.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 - H1.GetLength(1) / 2, H1);
                            break;
                        }
                    case 3:
                        {
                            MidTemp2((int)midpoint.X - (int)displacement - WorshipPlaceAtlantis.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 - WorshipPlaceAtlantis.GetLength(1) / 2, WorshipPlaceAtlantis);
                            break;
                        }
                    case 4:
                        {
                            PlaceFountain((int)midpoint.X - (int)displacement - Fountain.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 - Fountain.GetLength(1) / 2, Fountain);
                            break;
                        }
                    case 5:
                        {
                            PlaceH2((int)midpoint.X - (int)displacement - H2.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 - H2.GetLength(1) / 2, H2);
                            break;
                        }
                    case 6:
                        {
                            PlaceObservatory((int)midpoint.X - (int)displacement - Observatory.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 - Observatory.GetLength(1) / 2, Observatory);
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
                            PlaceH2((int)midpoint.X + (int)displacement - H2.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 - H2.GetLength(1) / 2, H2);
                            break;
                        }
                    case 1:
                        {
                            PlaceLootRoom((int)midpoint.X + (int)displacement - LootRoom.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 - LootRoom.GetLength(1) / 2, LootRoom);
                            break;
                        }
                    case 2:
                        {
                            PlaceH1((int)midpoint.X + (int)displacement - H1.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 - H1.GetLength(1) / 2, H1);
                            break;
                        }
                    case 3:
                        {
                            MidTemp2((int)midpoint.X + (int)displacement - WorshipPlaceAtlantis.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 - WorshipPlaceAtlantis.GetLength(1) / 2, WorshipPlaceAtlantis);
                            break;
                        }
                    case 4:
                        {
                            PlaceFountain((int)midpoint.X + (int)displacement - Fountain.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 - Fountain.GetLength(1) / 2, Fountain);
                            break;
                        }
                    case 5:
                        {
                            PlaceH2((int)midpoint.X + (int)displacement - H2.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 - H2.GetLength(1) / 2, H2);
                            break;
                        }
                    case 6:
                        {
                            PlaceObservatory((int)midpoint.X - (int)displacement - Observatory.GetLength(0) / 2, (int)startingHeightOfUpperClass + (j * 50) + 10 - Observatory.GetLength(1) / 2, Observatory);
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
                                    PlaceMisc1(distanceFromEdge + (j * 50) - Misc1.GetLength(0) / 2, 100 + (int)startingPoint.Y + (i * 40) - 50 - Misc1.GetLength(1) / 2, Misc1);
                                    break;
                                }
                            case 1:
                                {
                                    PlaceMisc2(distanceFromEdge + (j * 50) - Misc2.GetLength(0) / 2, 100 + (int)startingPoint.Y + (i * 40) - 50 - Misc2.GetLength(1) / 2, Misc2);
                                    break;
                                }
                            case 2:
                                {
                                    PlaceMisc3(distanceFromEdge + (j * 50) - Misc3.GetLength(0) / 2, 100 + (int)startingPoint.Y + (i * 40) - 50 - Misc3.GetLength(1) / 2, Misc3);
                                    break;
                                }
                            case 3:
                                {
                                    PlaceMisc4(distanceFromEdge + (j * 50) - Misc4.GetLength(0) / 2, 100 + (int)startingPoint.Y + (i * 40) - 50 - Misc4.GetLength(1) / 2, Misc4);
                                    break;
                                }
                        }
                    }
                }
            }
            MakeChasm(distanceFromEdge - 20, (int)startingPoint.Y + 100 + 60, 170, ModContent.TileType<DarkGemsandTile>(), 0, 10, 10);
            MakeChasm(distanceFromEdge + 70, (int)startingPoint.Y + 100 + 60, 170, ModContent.TileType<DarkGemsandTile>(), 0, 10, 10);
            MakeOvalJaggedTop(25, 40, new Vector2(distanceFromEdge + 12, 100 + (int)startingPoint.Y + 120), ModContent.TileType<DarkGemsandTile>());
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
                                    PlaceMisc1((int)(startingPoint.X + size.X) - distanceFromEdge - (j * 50) - 44 - Misc1.GetLength(0) / 2, 100 + (int)startingPoint.Y + (i * 40) - 50 - Misc1.GetLength(1) / 2, Misc1);
                                    break;
                                }
                            case 1:
                                {
                                    PlaceMisc2((int)(startingPoint.X + size.X) - distanceFromEdge - (j * 50) - 44 - Misc2.GetLength(0) / 2, 100 + (int)startingPoint.Y + (i * 40) - 50 - Misc2.GetLength(1) / 2, Misc2);
                                    break;
                                }
                            case 2:
                                {
                                    PlaceMisc3((int)(startingPoint.X + size.X) - distanceFromEdge - (j * 50) - 44 - Misc3.GetLength(0) / 2, 100 + (int)startingPoint.Y + (i * 40) - 50 - Misc3.GetLength(1) / 2, Misc3);
                                    break;
                                }
                            case 3:
                                {
                                    PlaceMisc4((int)(startingPoint.X + size.X) - distanceFromEdge - (j * 50) - 44 - Misc4.GetLength(0) / 2, 100 + (int)startingPoint.Y + (i * 40) - 50 - Misc4.GetLength(1) / 2, Misc4);
                                    break;
                                }
                        }
                    }
                }
            }
            fillers.Add(new Vector2((int)(startingPoint.X + size.X) - distanceFromEdge - 44, startingPoint.Y + distanceFromEdge + 120));
            fillers.Add(new Vector2(startingPoint.X + 60, startingPoint.Y + 60));
            KillWall((int)size.X, (int)size.Y, startingPoint);
            Main.spawnTileX = 500;
            Main.spawnTileY = 300;
            SubworldManager.SettleLiquids();
            EEMod.isSaving = false;
            MakeOval(350, 190, new Vector2(midpoint.X - 160, (int)startingHeightOfUpperClass + 25), ModContent.TileType<DarkGemsandTile>(), false);
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

            for (int j = 0; j < fillers.Count; j++)
            {
                switch (listNumbersFillers[j])
                {
                    case 0:
                        {
                            PlaceFiller1((int)fillers[j].X - Filler1.GetLength(0) / 2, (int)fillers[j].Y - Filler1.GetLength(1) / 2, Filler1);
                            break;
                        }
                    case 1:
                        {
                            PlaceFiller2((int)fillers[j].X - Filler2.GetLength(0) / 2, (int)fillers[j].Y - Filler2.GetLength(1) / 2, Filler2);
                            break;
                        }
                    case 2:
                        {
                            PlaceFiller3((int)fillers[j].X - Filler3.GetLength(0) / 2, (int)fillers[j].Y - Filler3.GetLength(1) / 2, Filler3);
                            break;
                        }
                    case 3:
                        {
                            PlaceFiller4((int)fillers[j].X - Filler4.GetLength(0) / 2, (int)fillers[j].Y - Filler4.GetLength(1) / 2, Filler4);
                            break;
                        }
                    case 4:
                        {
                            PlaceFiller5((int)fillers[j].X - Filler5.GetLength(0) / 2, (int)fillers[j].Y - Filler5.GetLength(1) / 2, Filler5);
                            break;
                        }
                    case 5:
                        {
                            PlaceFiller6((int)fillers[j].X - Filler6.GetLength(0) / 2, (int)fillers[j].Y - Filler6.GetLength(1) / 2, Filler6);
                            break;
                        }
                    case 6:
                        {
                            PlaceShrine((int)fillers[j].X - Shrine.GetLength(0) / 2, (int)fillers[j].Y - Shrine.GetLength(1) / 2, Shrine);
                            break;
                        }
                    case 7:
                        {
                            PlaceDome((int)fillers[j].X - Dome.GetLength(0) / 2, (int)fillers[j].Y - Dome.GetLength(1) / 2, Dome);
                            break;
                        }
                    case 8:
                        {
                            PlaceMisc((int)fillers[j].X - Misc.GetLength(0) / 2, (int)fillers[j].Y - Misc.GetLength(1) / 2, Misc);
                            break;
                        }
                }
            }
            WorldGen.TileRunner(80, 80, 30, 10, ModContent.TileType<DarkGemsandTile>());
            WorldGen.TileRunner(100, 60, 30, 10, ModContent.TileType<DarkGemsandTile>());
            FillRegionWithWater((int)size.X, (int)size.Y, startingPoint);
            FillRegionWithWater((int)size.X, (int)size.Y, startingPoint);
            FillRegionWithWater((int)size.X, (int)size.Y, startingPoint);
        }
        public static void MakeLayer(int X, int midY, int size, int layer, int type)
        {

            int maxTiles = (int)(Main.maxTilesX * Main.maxTilesY * 9E-04);
            for (int k = 0; k < maxTiles * (size / 8); k++)
            {
                int x = WorldGen.genRand.Next(X - 160, X + 160);
                int y = WorldGen.genRand.Next(midY - 160, midY + 160);
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
            RemoveStoneSlabs();
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
                WorldGen.TileRunner(X, midY, WorldGen.genRand.Next(size / 3 - 10, size / 3 + 10), WorldGen.genRand.Next(5, 10), type, true, 1f, 1f, false, true);
        }
        public static void MakeLayerWithOutline(int X, int midY, int size, int layer, int type, int thickness)
        {

            int maxTiles = (int)(Main.maxTilesX * Main.maxTilesY * 9E-04);
            for (int k = 0; k < maxTiles * (size / 8); k++)
            {
                int x = WorldGen.genRand.Next(X - (size * 2), X + (size * 2));
                int y = WorldGen.genRand.Next(midY - (size * 2), midY + (size * 2));
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

            for (int k = 0; k < maxTiles * 3; k++)
            {
                int x = WorldGen.genRand.Next(X - (size * 2), X + (size * 2));
                int y = WorldGen.genRand.Next(midY - (size * 2), midY + (size * 2));
                Tile tile = Framing.GetTileSafely(x, y);
                if (layer == 1)
                {
                    int sizeSQ = size + thickness;
                    if (Vector2.DistanceSquared(new Vector2(x, y), new Vector2(X, midY)) < sizeSQ * sizeSQ)
                        WorldGen.TileRunner(x, y, WorldGen.genRand.Next(4, 10), WorldGen.genRand.Next(5, 10), ModContent.TileType<GemsandTile>(), true, 0f, 0f, false, false);
                }
                if (layer == 2)
                {
                    if (OvalCheck(X, midY, x, y, (size * 3) + 10, (size) + 10) && tile.active())
                        WorldGen.TileRunner(x, y, WorldGen.genRand.Next(4, 10), WorldGen.genRand.Next(5, 10), ModContent.TileType<DarkGemsandTile>(), true, 1, 1, true, true);
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
        public static void PlaceAnyBuilding(int i, int j, int[,,] shape)
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

                        if (shape[y, x, 0] != 0)
                        {
                            if(tile.type != ModContent.TileType<GemsandChestTile>())
                                tile.ClearTile();
                            if (shape[y, x, 0] == ModContent.TileType<GemsandChestTile>())
                                WorldGen.PlaceChest(k, l, (ushort)ModContent.TileType<GemsandChestTile>());
                            else
                                tile.type = (ushort)shape[y, x, 0];
                            tile.active(true);
                        }
                        tile.wall = (ushort)shape[y, x, 1];
                        tile.color((byte)shape[y, x, 2]);
                        tile.slope((byte)shape[y, x, 3]);
                        tile.wallColor((byte)shape[y, x, 4]);
                        if ((byte)shape[y, x, 5] == 1)
                        {
                            tile.inActive(true);
                        }
                        else
                        {
                            tile.inActive(false);
                        }
                        if ((byte)shape[y, x, 6] > 0)
                        {
                            tile.liquid = ((byte)shape[y, x, 6]);
                            tile.liquidType((byte)shape[y, x, 7]);
                        }
                        tile.frameX = ((byte)shape[y, x, 8]);
                        tile.frameY = ((byte)shape[y, x, 9]);
                    }
                }
            }
        }
        public static void Island(int islandWidth, int islandHeight, int posY)
        {
            MakeOvalJaggedBottom(islandWidth, islandHeight, new Vector2((Main.maxTilesX / 2) - islandWidth / 2, posY), ModContent.TileType<CoralSand>());
            MakeOvalJaggedBottom((int)(islandWidth * 0.6), (int)(islandHeight * 0.6), new Vector2((int)((Main.maxTilesX / 2) - (Main.maxTilesX / 4)), TileCheck((int)(Main.maxTilesX / 2), ModContent.TileType<CoralSand>()) - 10), TileID.Dirt);
            //KillWall(Main.maxTilesX, Main.maxTilesY, Vector2.Zero);

            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    WorldGen.SpreadGrass(i, j);
                }
            }
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
