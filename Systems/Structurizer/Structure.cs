using System;
using System.Collections.Generic;
using System.IO;
using EEMod.Systems.Structurizer.PlacementActions;
using EEMod.Systems.Structurizer.PlacementActions.Actions;
using EEMod.Systems.Structurizer.PlacementActions.Actions.Repetition;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace EEMod.Systems.Structurizer
{
    public class Structure
    {
        public virtual int Width { get; }

        public virtual int Height { get; }

        public virtual Dictionary<ushort, ushort> EntryToTileID { get; }

        public virtual Dictionary<ushort, ushort> EntryToWallID { get; }

        public virtual IPlacementAction[] PlacementActions { get; }

        private Structure(int width, int height, Dictionary<ushort, ushort> tileMap, Dictionary<ushort, ushort> wallMap,
            IPlacementAction[] placementActions)
        {
            Width = width;
            Height = height;
            EntryToTileID = tileMap;
            EntryToWallID = wallMap;
            PlacementActions = placementActions;
        }

        public virtual void PlaceAt(int x, int y)
        {
            int i = x;
            int j = y;
            List<(Point, ushort, ushort, ushort)> deferredMultitiles = new List<(Point, ushort, ushort, ushort)>();

            PrepareAreaForStructure(x, y);

            foreach (IPlacementAction action in PlacementActions)
            {
                action.Place(ref i, ref j, this);

                if (i >= x + Width)
                {
                    i = x;
                    j++;
                }

                if (j >= y + Height)
                {
                    j = y;
                    i = x;
                }
            }

            foreach ((Point pos, ushort _, ushort style, ushort alternate) in deferredMultitiles)
            {
                TileObject tileObject = new TileObject
                {
                    xCoord = pos.X,
                    yCoord = pos.Y,
                    style = style,
                    alternate = alternate
                };

                TileObject.Place(tileObject);
            }

            FinalizeArea(x, y);
        }

        public virtual void PrepareAreaForStructure(int x, int y)
        {
            for (int a = y; a < y + Height; a++)
            {
                for (int b = x; b < x + Width; b++)
                {
                    int chestID = Chest.FindChest(b, a);
                    if (chestID != -1)
                    {
                        Chest chest = Main.chest[chestID];

                        foreach (Item item in chest.item)
                            item.TurnToAir();

                        WorldGen.KillTile(b, a, false, noItem: true);
                    }

                    Framing.GetTileSafely(b, a).liquid = 0;
                }
            }
        }

        public virtual void FinalizeArea(int x, int y)
        {
            for (int a = y; a < y + Height; a++)
            {
                for (int b = x; b < x + Width; b++)
                {
                    WorldGen.SquareTileFrame(b, a);
                    WorldGen.SquareWallFrame(b, a);
                }
            }
        }

        public static void SaveWorldStructureTo(int x, int y, int width, int height, string outputPath)
        {
            if (!Path.HasExtension(outputPath))
                outputPath += ".lcs";

            File.WriteAllBytes(outputPath, SerializeFromWorld(x, y, width, height));
        }

        public static byte[] SerializeFromWorld(int x, int y, int width, int height)
        {
            int endX = x + width;

            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(StructureConstants.StructureFileFormatVersion);
                    writer.Write((ushort) width);
                    writer.Write((ushort) height);

                    (Dictionary<ushort, ushort> vanillaTileEntryMap, Dictionary<ushort, ushort> moddedTileEntryMap) =
                        CreateAreaTileMapData(x, y, width, height);
                    WriteMapData(vanillaTileEntryMap, moddedTileEntryMap, false, writer);

                    for (int j = y; j < y + height; j++)
                    {
                        for (int i = x; i < x + width; i++)
                        {
                            Tile tile = Framing.GetTileSafely(i, j);

                            if (tile.active())
                            {
                                bool vanillaTile = tile.type < TileID.Count;
                                ushort indexInMap = vanillaTile
                                    ? vanillaTileEntryMap[tile.type]
                                    : moddedTileEntryMap[tile.type];

                                TileObjectData tileObjectData = TileObjectData.GetTileData(tile);
                                if (tileObjectData != null)
                                {
                                    Point16 tileObjectDataOrigin = tileObjectData.Origin;
                                    Point tileTopLeft = GetTileTopLeft(i, j);
                                    Point tileOrigin = new Point(tileObjectDataOrigin.X + tileTopLeft.X,
                                        tileObjectDataOrigin.Y + tileTopLeft.Y);

                                    if (tileOrigin == new Point(i, j))
                                    {
                                        Tile originTile = Framing.GetTileSafely(tileOrigin);

                                        int style = 0;
                                        int alternate = 0;
                                        TileObjectData.GetTileInfo(originTile, ref style, ref alternate);

                                        if (style != 0)
                                        {
                                            if (alternate == 0)
                                            {
                                                writer.Write(StructureConstants.PlaceMultitileWithStyleFlag);
                                                writer.Write((byte) style);
                                            }
                                            else
                                            {
                                                writer.Write(StructureConstants.PlaceMultitileWithAlternateStyleFlag);
                                                writer.Write((byte) style);
                                                writer.Write((byte) alternate);
                                            }
                                        }
                                        else
                                            writer.Write(StructureConstants.PlaceMultitileFlag);

                                        writer.Write(indexInMap);
                                    }
                                    else
                                        writer.Write(StructureConstants.AirTileFlag);
                                }
                                else if (tile.halfBrick())
                                {
                                    Tile nextTile = Framing.GetTileSafely(i + 1, j);

                                    if (i + 1 < endX && nextTile.type == tile.type && nextTile.halfBrick())
                                    {
                                        ushort identicalHalfBricks = 0;

                                        while (i < endX && nextTile.type == tile.type && nextTile.halfBrick())
                                        {
                                            identicalHalfBricks++;
                                            nextTile = Framing.GetTileSafely(++i, j);
                                        }

                                        i--;

                                        writer.Write(StructureConstants.RepeatedHalfBrickFlag);
                                        writer.Write(identicalHalfBricks);
                                    }
                                    else
                                        writer.Write(StructureConstants.PlaceHalfBrickFlag);

                                    writer.Write(indexInMap);
                                }
                                else if (tile.slope() != 0)
                                {
                                    Tile nextTile = Framing.GetTileSafely(i + 1, j);
                                    byte tileSlope = tile.slope();

                                    if (i + 1 < endX && nextTile.type == tile.type && nextTile.active() &&
                                        nextTile.slope() == tileSlope)
                                    {
                                        ushort identicalSlopes = 0;

                                        while (i < endX && nextTile.type == tile.type && nextTile.active() &&
                                               nextTile.slope() == tileSlope)
                                        {
                                            identicalSlopes++;
                                            nextTile = Framing.GetTileSafely(++i, j);
                                        }

                                        i--;

                                        writer.Write(StructureConstants.RepeatedTileWithSlopeFlag);
                                        writer.Write(identicalSlopes);
                                    }
                                    else
                                        writer.Write(StructureConstants.PlaceTileWithSlopeFlag);

                                    writer.Write(tileSlope);
                                    writer.Write(indexInMap);
                                }
                                else
                                {
                                    Tile nextTile = Framing.GetTileSafely(i + 1, j);

                                    if (i + 1 < endX && nextTile.type == tile.type && nextTile.slope() == 0 &&
                                        !nextTile.halfBrick())
                                    {
                                        ushort identicalTiles = 0;

                                        while (i < endX && nextTile.type == tile.type && nextTile.slope() == 0 &&
                                               !nextTile.halfBrick())
                                        {
                                            identicalTiles++;
                                            nextTile = Framing.GetTileSafely(++i, j);
                                        }

                                        i--;

                                        writer.Write(StructureConstants.RepeatedTileFlag);
                                        writer.Write(indexInMap);
                                        writer.Write(identicalTiles);
                                        continue;
                                    }

                                    writer.Write(indexInMap);
                                }
                            }
                            else if (tile.liquid > 0)
                            {
                                int liquidType = tile.liquidType();

                                if (i + 1 < endX && Framing.GetTileSafely(i + 1, j).liquid > 0)
                                {
                                    ushort identicalLiquids = 0;

                                    while (i < endX && !Framing.GetTileSafely(i, j).active())
                                    {
                                        identicalLiquids++;
                                        i++;
                                    }

                                    i--;

                                    switch (liquidType)
                                    {
                                        case 0:
                                            writer.Write(StructureConstants.RepeatedWaterFlag);
                                            break;
                                        case 1:

                                            writer.Write(StructureConstants.RepeatedLavaFlag);
                                            break;
                                        case 2:

                                            writer.Write(StructureConstants.RepeatedHoneyFlag);
                                            break;
                                    }

                                    writer.Write(identicalLiquids);
                                }
                                else
                                {
                                    switch (liquidType)
                                    {
                                        case 0:
                                            writer.Write(StructureConstants.PlaceWaterFlag);
                                            break;

                                        case 1:
                                            writer.Write(StructureConstants.PlaceLavaFlag);
                                            break;

                                        case 2:
                                            writer.Write(StructureConstants.PlaceHoneyFlag);
                                            break;
                                    }
                                }

                                writer.Write(tile.liquid);
                            }
                            else
                            {
                                if (i + 1 < endX && !Framing.GetTileSafely(i + 1, j).active())
                                {
                                    ushort skippedTiles = 0;

                                    while (i < endX && !Framing.GetTileSafely(i, j).active())
                                    {
                                        skippedTiles++;
                                        i++;
                                    }

                                    i--;

                                    writer.Write(StructureConstants.RepeatedAirFlag);
                                    writer.Write(skippedTiles);
                                }
                                else
                                    writer.Write(StructureConstants.AirTileFlag);
                            }
                        }
                    }

                    writer.Write(StructureConstants.EndOfTilesDataFlag);

                    (Dictionary<ushort, ushort> vanillaWallEntryMap, Dictionary<ushort, ushort> moddedWallEntryMap) =
                        CreateAreaWallMapData(x, y, width, height);
                    WriteMapData(vanillaWallEntryMap, moddedWallEntryMap, true, writer);

                    for (int j = y; j < y + height; j++)
                    {
                        for (int i = x; i < x + width; i++)
                        {
                            Tile tile = Framing.GetTileSafely(i, j);

                            if (tile.wall == 0)
                            {
                                if (i + 1 < endX && Framing.GetTileSafely(i + 1, j).wall == 0)
                                {
                                    ushort emptyWalls = 0;

                                    while (i < endX && Framing.GetTileSafely(i, j).wall == 0)
                                    {
                                        emptyWalls++;
                                        i++;
                                    }

                                    i--;

                                    writer.Write(StructureConstants.RepeatedEmptyWallFlag);
                                    writer.Write(emptyWalls);
                                }
                                else
                                    writer.Write(StructureConstants.EmptyWallFlag);

                                continue;
                            }

                            bool vanillaWall = tile.wall < WallID.Count;
                            ushort indexInMap = vanillaWall 
                                ? vanillaWallEntryMap[tile.wall] 
                                : moddedWallEntryMap[tile.wall];

                            if (i + 1 < endX && Framing.GetTileSafely(i + 1, j).wall == tile.wall)
                            {
                                ushort identicalWalls = 0;

                                while (i < endX && Framing.GetTileSafely(i, j).wall == tile.wall)
                                {
                                    identicalWalls++;
                                    i++;
                                }

                                i--;

                                writer.Write(StructureConstants.RepeatedWallFlag);
                                writer.Write(indexInMap);
                                writer.Write(identicalWalls);
                                continue;
                            }

                            writer.Write(indexInMap);
                        }
                    }
                }

                return stream.ToArray();
            }
        }

        public static Structure DeserializeFromBytes(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    byte formatVersion = reader.ReadByte();
                    ushort width = reader.ReadUInt16();
                    ushort height = reader.ReadUInt16();

                    List<IPlacementAction> placementActions = new List<IPlacementAction>();
                    Dictionary<ushort, ushort> tileEntryMap = ReadMapData(false, reader);

                    ushort action;
                    while (stream.Position < stream.Length)
                    {
                        action = reader.ReadUInt16();

                        switch (action)
                        {
                            case StructureConstants.RepeatedAirFlag:
                                placementActions.Add(new RepeatedPlaceAirAction(reader.ReadUInt16()));
                                break;

                            case StructureConstants.AirTileFlag:
                                placementActions.Add(new PlaceAirAction());
                                break;

                            case StructureConstants.RepeatedTileFlag:
                                placementActions.Add(new RepeatedPlaceTileAction(reader.ReadUInt16(), reader.ReadUInt16()));
                                break;

                            case StructureConstants.PlaceMultitileFlag:
                                placementActions.Add(new PlaceMultitileAction(reader.ReadUInt16()));
                                break;

                            case StructureConstants.PlaceWaterFlag:
                                placementActions.Add(new PlaceWaterAction(reader.ReadByte()));
                                break;

                            case StructureConstants.PlaceLavaFlag:
                                placementActions.Add(new PlaceLavaAction(reader.ReadByte()));
                                break;

                            case StructureConstants.PlaceHoneyFlag:
                                placementActions.Add(new PlaceHoneyAction(reader.ReadByte()));
                                break;

                            case StructureConstants.RepeatedWaterFlag:
                                placementActions.Add(new RepeatedPlaceWaterAction(reader.ReadUInt16(), reader.ReadByte()));
                                break;

                            case StructureConstants.RepeatedLavaFlag:
                                placementActions.Add(new RepeatedPlaceLavaAction(reader.ReadUInt16(), reader.ReadByte()));
                                break;

                            case StructureConstants.RepeatedHoneyFlag:
                                placementActions.Add(new RepeatedPlaceHoneyAction(reader.ReadUInt16(), reader.ReadByte()));
                                break;

                            case StructureConstants.PlaceMultitileWithStyleFlag:
                            {
                                byte style = reader.ReadByte();
                                ushort entry = reader.ReadUInt16();
                                placementActions.Add(new PlaceMultitileWithStyle(entry, style));
                                break;
                            }

                            case StructureConstants.PlaceMultitileWithAlternateStyleFlag:
                            {
                                byte style = reader.ReadByte();
                                byte alternate = reader.ReadByte();
                                ushort entry = reader.ReadUInt16();
                                placementActions.Add(new PlaceMultitileWithAlternativeStyleAction(entry, style, alternate));
                                break;
                            }

                            case StructureConstants.PlaceTileWithSlopeFlag:
                            {
                                byte slope = reader.ReadByte();
                                ushort entry = reader.ReadUInt16();
                                placementActions.Add(new PlaceSlopeAction(entry, slope));
                                break;
                            }

                            case StructureConstants.RepeatedTileWithSlopeFlag:
                            {
                                ushort count = reader.ReadUInt16();
                                byte slope = reader.ReadByte();
                                ushort entry = reader.ReadUInt16();
                                placementActions.Add(new RepeatedPlaceSlopeAction(count, entry, slope));
                                break;
                            }

                            case StructureConstants.PlaceHalfBrickFlag:
                                placementActions.Add(new PlaceHalfBrickAction(reader.ReadUInt16()));
                                break;

                            case StructureConstants.RepeatedHalfBrickFlag:
                                placementActions.Add(new RepeatedPlaceHalfBrickAction(reader.ReadUInt16(), reader.ReadUInt16()));
                                break;

                            case StructureConstants.EndOfTilesDataFlag:
                                goto BreakOutOfLoop;

                            default:
                                placementActions.Add(new PlaceTileAction(action));
                                break;
                        }
                    }

                    BreakOutOfLoop:

                    Dictionary<ushort, ushort> wallEntryMap = ReadMapData(true, reader);

                    while (stream.Position < stream.Length)
                    {
                        action = reader.ReadUInt16();

                        switch (action)
                        {
                            case StructureConstants.RepeatedWallFlag:
                                ushort entry = reader.ReadUInt16();
                                ushort count = reader.ReadUInt16();
                                placementActions.Add(new RepeatedPlaceWallAction(count, entry));
                                break;

                            case StructureConstants.EmptyWallFlag:
                                placementActions.Add(new PlaceEmptyWallAction());
                                break;

                            case StructureConstants.RepeatedEmptyWallFlag:
                                placementActions.Add(new RepeatedPlaceEmptyWallAction(reader.ReadUInt16()));
                                break;

                            default:
                                placementActions.Add(new PlaceWallAction(action));
                                break;
                        }
                    }

                    return new Structure(width, height, tileEntryMap, wallEntryMap, placementActions.ToArray());
                }
            }
        }

        public static (Dictionary<ushort, ushort>, Dictionary<ushort, ushort>) CreateAreaTileMapData(int x, int y,
            int width, int height)
        {
            Dictionary<ushort, ushort> vanillaEntryMap = new Dictionary<ushort, ushort>();
            Dictionary<ushort, ushort> moddedEntryMap = new Dictionary<ushort, ushort>();

            for (int j = y; j < y + height; j++)
            {
                for (int i = x; i < x + width; i++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (!tile.active())
                        continue;

                    bool vanillaTile = tile.type < TileID.Count;

                    switch (vanillaTile)
                    {
                        case true when !vanillaEntryMap.ContainsKey(tile.type):
                            vanillaEntryMap[tile.type] = (ushort) (vanillaEntryMap.Count + moddedEntryMap.Count);
                            break;

                        case false when !moddedEntryMap.ContainsKey(tile.type):
                            moddedEntryMap[tile.type] = (ushort) (moddedEntryMap.Count + vanillaEntryMap.Count);
                            break;
                    }
                }
            }

            return (vanillaEntryMap, moddedEntryMap);
        }

        public static (Dictionary<ushort, ushort>, Dictionary<ushort, ushort>) CreateAreaWallMapData(int x, int y,
            int width, int height)
        {
            Dictionary<ushort, ushort> vanillaEntryMap = new Dictionary<ushort, ushort>();
            Dictionary<ushort, ushort> moddedEntryMap = new Dictionary<ushort, ushort>();

            for (int j = y; j < y + height; j++)
            {
                for (int i = x; i < x + width; i++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);
                    if (tile.wall == 0)
                        continue;

                    bool vanillaWall = tile.wall < WallID.Count;

                    switch (vanillaWall)
                    {
                        case true when !vanillaEntryMap.ContainsKey(tile.wall):
                            vanillaEntryMap[tile.wall] = (ushort) (vanillaEntryMap.Count + moddedEntryMap.Count);
                            break;

                        case false when !moddedEntryMap.ContainsKey(tile.wall):
                            moddedEntryMap[tile.wall] = (ushort) (moddedEntryMap.Count + vanillaEntryMap.Count);
                            break;
                    }
                }
            }

            return (vanillaEntryMap, moddedEntryMap);
        }

        public static void WriteMapData(Dictionary<ushort, ushort> vanillaEntryMap,
            Dictionary<ushort, ushort> moddedEntryMap, bool isWalls, BinaryWriter writer)
        {
            writer.Write((ushort) vanillaEntryMap.Count);
            foreach (KeyValuePair<ushort, ushort> vanillaEntry in vanillaEntryMap)
            {
                writer.Write(vanillaEntry.Value);
                writer.Write(vanillaEntry.Key);
            }

            writer.Write((ushort) moddedEntryMap.Count);

            if (!isWalls)
            {
                foreach (KeyValuePair<ushort, ushort> moddedEntry in moddedEntryMap)
                {
                    writer.Write(moddedEntry.Value);

                    ModTile modTile = TileLoader.GetTile(moddedEntry.Key);
                    writer.Write(modTile.mod.Name + "." + modTile.Name);
                }
            }
            else
            {
                foreach (KeyValuePair<ushort, ushort> moddedEntry in moddedEntryMap)
                {
                    writer.Write(moddedEntry.Value);

                    ModWall modWall = WallLoader.GetWall(moddedEntry.Key);
                    writer.Write(modWall.mod.Name + "." + modWall.Name);
                }
            }
        }

        public static Dictionary<ushort, ushort> ReadMapData(bool isWalls, BinaryReader reader)
        {
            Dictionary<ushort, ushort> entryMap = new Dictionary<ushort, ushort>();

            ushort vanillaEntryCount = reader.ReadUInt16();
            for (int i = 0; i < vanillaEntryCount; i++)
                entryMap[reader.ReadUInt16()] = reader.ReadUInt16();

            ushort moddedEntryCount = reader.ReadUInt16();

            if (!isWalls)
            {
                for (int i = 0; i < moddedEntryCount; i++)
                {
                    ushort index = reader.ReadUInt16();
                    string tileName = reader.ReadString();
                    string[] parts = tileName.Split('.');

                    ushort? type = ModLoader.GetMod(parts[0])?.GetTile(parts[1]).Type;
                    if (type == null)
                        throw new Exception(
                            $"Attempted to generate structure that depends on modded tile '{tileName}' but it was not loaded");

                    entryMap[index] = type.Value;
                }
            }
            else
            {
                for (int i = 0; i < moddedEntryCount; i++)
                {
                    ushort index = reader.ReadUInt16();
                    string tileName = reader.ReadString();
                    string[] parts = tileName.Split('.');

                    ushort? type = ModLoader.GetMod(parts[0])?.GetWall(parts[1]).Type;
                    if (type == null)
                        throw new Exception(
                            $"Attempted to generate structure that depends on modded wall '{tileName}' but it was not loaded");

                    entryMap[index] = type.Value;
                }
            }

            return entryMap;
        }

        public static Point GetTileTopLeft(int i, int j)
        {
            if (i < 0 || i >= Main.maxTilesX || j < 0 || j >= Main.maxTilesY)
                return new Point(-1, -1);

            Tile tile = Main.tile[i, j];

            int fX = 0;
            int fY = 0;

            if (tile == null)
                return new Point(i, j);

            TileObjectData data = TileObjectData.GetTileData(tile.type, 0);

            if (data == null)
                return new Point(i, j);

            fX = tile.frameX % (18 * data.Width) / 18;
            fY = tile.frameY % (18 * data.Height) / 18;

            return new Point(i - fX, j - fY);
        }
    }
}