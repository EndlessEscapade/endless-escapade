using System;
using System.Collections.Generic;
using System.IO;
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

        public virtual PlacementAction[] PlacementActions { get; }

        private Structure(int width, int height, Dictionary<ushort, ushort> tileMap, Dictionary<ushort, ushort> wallMap,
            PlacementAction[] placementActions)
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

            foreach (PlacementAction action in PlacementActions)
            {
                switch (action.Type)
                {
                    case PlacementActionType.PlaceAirRepeated:
                    {
                        for (int z = i; z < i + action.RepetitionData; z++)
                            WorldGen.KillTile(z, j, false, noItem: true);

                        i += action.RepetitionData;
                        break;
                    }

                    case PlacementActionType.PlaceAir:
                        WorldGen.KillTile(i, j, false, noItem: true);
                        i++;
                        break;

                    case PlacementActionType.PlaceTile:
                        WorldGen.PlaceTile(i, j, EntryToTileID[action.EntryData], true, true);
                        i++;
                        break;

                    case PlacementActionType.PlaceTileRepeated:
                    {
                        for (int z = i; z < i + action.RepetitionData; z++)
                            WorldGen.PlaceTile(z, j, EntryToTileID[action.EntryData], true, true);

                        i += action.RepetitionData;
                        break;
                    }

                    case PlacementActionType.PlaceMultitile:
                        deferredMultitiles.Add((new Point(i, j), EntryToTileID[action.EntryData], 0, 0));
                        i++;
                        break;

                    case PlacementActionType.PlaceWater:
                    {
                        Tile tile = Framing.GetTileSafely(i, j);
                        tile.liquidType(0);
                        tile.liquid = action.LiquidData;
                        break;
                    }

                    case PlacementActionType.PlaceLava:
                    {
                        Tile tile = Framing.GetTileSafely(i, j);
                        tile.liquidType(1);
                        tile.liquid = action.LiquidData;
                        break;
                    }

                    case PlacementActionType.PlaceHoney:
                    {
                        Tile tile = Framing.GetTileSafely(i, j);
                        tile.liquidType(2);
                        tile.liquid = action.LiquidData;
                        break;
                    }

                    case PlacementActionType.PlaceWaterRepeated:
                    {
                        for (int z = i; z < i + action.RepetitionData; z++)
                        {
                            Tile tile = Framing.GetTileSafely(z, j);
                            tile.liquidType(0);
                            tile.liquid = action.LiquidData;
                        }

                        i += action.RepetitionData;
                        break;
                    }

                    case PlacementActionType.PlaceLavaRepeated:
                    {
                        for (int z = i; z < i + action.RepetitionData; z++)
                        {
                            Tile tile = Framing.GetTileSafely(z, j);
                            tile.liquidType(1);
                            tile.liquid = action.LiquidData;
                        }

                        i += action.RepetitionData;
                        break;
                    }

                    case PlacementActionType.PlaceHoneyRepeated:
                    {
                        for (int z = i; z < i + action.RepetitionData; z++)
                        {
                            Tile tile = Framing.GetTileSafely(z, j);
                            tile.liquidType(2);
                            tile.liquid = action.LiquidData;
                        }

                        i += action.RepetitionData;
                        break;
                    }

                    case PlacementActionType.PlaceMultitileWithStyle:
                        deferredMultitiles.Add((new Point(i, j), EntryToTileID[action.EntryData], action.StyleData, 0));
                        i++;
                        break;

                    case PlacementActionType.PlaceMultitileWithAlternateStyle:
                        deferredMultitiles.Add((new Point(i, j), EntryToTileID[action.EntryData], action.StyleData,
                            action.AlternateStyleData));
                        i++;
                        break;

                    case PlacementActionType.PlaceWall:
                        WorldGen.PlaceWall(i, j, EntryToWallID[action.EntryData], true);
                        i++;
                        break;

                    case PlacementActionType.PlaceWallRepeated:
                    {
                        for (int z = i; z < i + action.RepetitionData; z++)
                            WorldGen.PlaceWall(z, j, EntryToWallID[action.EntryData], true);

                        i += action.RepetitionData;
                        break;
                    }

                    case PlacementActionType.PlaceEmptyWall:
                        WorldGen.KillWall(i, j, false);
                        i++;
                        break;

                    case PlacementActionType.PlaceEmptyWallRepeated:
                    {
                        for (int z = i; z < i + action.RepetitionData; z++) WorldGen.KillWall(z, j, false);

                        i += action.RepetitionData;
                        break;
                    }

                    case PlacementActionType.PlaceSlope:
                    {
                        if (WorldGen.PlaceTile(i, j, EntryToTileID[action.EntryData], true))
                            Main.tile[i, j].slope(action.StyleData);

                        i++;
                        break;
                    }

                    case PlacementActionType.PlaceSlopeRepeated:
                    {
                        for (int z = i; z < i + action.RepetitionData; z++)
                            if (WorldGen.PlaceTile(z, j, EntryToTileID[action.EntryData], true))
                                Main.tile[z, j].slope(action.StyleData);

                        i += action.RepetitionData;
                        break;
                    }

                    case PlacementActionType.PlaceHalfBrick:
                    {
                        if (WorldGen.PlaceTile(i, j, EntryToTileID[action.EntryData]))
                            Main.tile[i, j].halfBrick(true);

                        i++;
                        break;
                    }

                    case PlacementActionType.PlaceHalfBrickRepeated:
                    {
                        for (int z = i; z < i + action.RepetitionData; z++)
                            if (WorldGen.PlaceTile(z, j, EntryToTileID[action.EntryData], true))
                                Main.tile[z, j].halfBrick(true);

                        i += action.RepetitionData;
                        break;
                    }

                    default:
                        throw new ArgumentOutOfRangeException();
                }

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
                        for (int z = 0; z < chest.item.Length; z++)
                            chest.item[z].TurnToAir();

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
                                        writer.Write(StructureConstants.AirTile);
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
                                    writer.Write(StructureConstants.AirTile);
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
                            ushort indexInMap =
                                vanillaWall ? vanillaWallEntryMap[tile.wall] : moddedWallEntryMap[tile.wall];

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

                    List<PlacementAction> placementActions = new List<PlacementAction>();
                    Dictionary<ushort, ushort> tileEntryMap = ReadMapData(false, reader);

                    ushort action = 0;
                    while (stream.Position < stream.Length)
                    {
                        action = reader.ReadUInt16();

                        if (action == StructureConstants.RepeatedAirFlag)
                            placementActions.Add(PlacementAction.PlaceAirRepeated(reader.ReadUInt16()));
                        else if (action == StructureConstants.AirTile)
                            placementActions.Add(PlacementAction.AirTile);
                        else if (action == StructureConstants.RepeatedTileFlag)
                            placementActions.Add(
                                PlacementAction.PlaceTileRepeated(reader.ReadUInt16(), reader.ReadUInt16()));
                        else if (action == StructureConstants.PlaceMultitileFlag)
                            placementActions.Add(PlacementAction.PlaceMultitile(reader.ReadUInt16()));
                        else if (action == StructureConstants.PlaceWaterFlag)
                            placementActions.Add(PlacementAction.PlaceWater(reader.ReadByte()));
                        else if (action == StructureConstants.PlaceLavaFlag)
                            placementActions.Add(PlacementAction.PlaceLava(reader.ReadByte()));
                        else if (action == StructureConstants.PlaceHoneyFlag)
                            placementActions.Add(PlacementAction.PlaceHoney(reader.ReadByte()));
                        else if (action == StructureConstants.RepeatedWaterFlag)
                            placementActions.Add(
                                PlacementAction.PlaceWaterRepeated(reader.ReadUInt16(), reader.ReadByte()));
                        else if (action == StructureConstants.RepeatedLavaFlag)
                            placementActions.Add(
                                PlacementAction.PlaceLavaRepeated(reader.ReadUInt16(), reader.ReadByte()));
                        else if (action == StructureConstants.RepeatedHoneyFlag)
                            placementActions.Add(
                                PlacementAction.PlaceHoneyRepeated(reader.ReadUInt16(), reader.ReadByte()));
                        else if (action == StructureConstants.PlaceMultitileWithStyleFlag)
                            placementActions.Add(
                                PlacementAction.PlaceMultitileWithStyle(reader.ReadByte(), reader.ReadUInt16()));
                        else if (action == StructureConstants.PlaceMultitileWithAlternateStyleFlag)
                            placementActions.Add(PlacementAction.PlaceMultitileWithAlternateStyle(reader.ReadByte(),
                                reader.ReadByte(), reader.ReadUInt16()));
                        else if (action == StructureConstants.PlaceTileWithSlopeFlag)
                            placementActions.Add(PlacementAction.PlaceSlope(reader.ReadByte(), reader.ReadUInt16()));
                        else if (action == StructureConstants.RepeatedTileWithSlopeFlag)
                            placementActions.Add(PlacementAction.PlaceSlopeRepeated(reader.ReadUInt16(),
                                reader.ReadByte(), reader.ReadUInt16()));
                        else if (action == StructureConstants.PlaceHalfBrickFlag)
                            placementActions.Add(PlacementAction.PlaceHalfBrick(reader.ReadUInt16()));
                        else if (action == StructureConstants.RepeatedHalfBrickFlag)
                            placementActions.Add(
                                PlacementAction.PlaceHalfBrickRepeated(reader.ReadUInt16(), reader.ReadUInt16()));
                        else if (action == StructureConstants.EndOfTilesDataFlag)
                            break;
                        else
                            placementActions.Add(PlacementAction.PlaceTile(action));
                    }

                    Dictionary<ushort, ushort> wallEntryMap = ReadMapData(true, reader);

                    while (stream.Position < stream.Length)
                    {
                        action = reader.ReadUInt16();

                        switch (action)
                        {
                            case StructureConstants.RepeatedWallFlag:
                                placementActions.Add(
                                    PlacementAction.PlaceWallRepeated(reader.ReadUInt16(), reader.ReadUInt16()));
                                break;

                            case StructureConstants.EmptyWallFlag:
                                placementActions.Add(PlacementAction.EmptyWall);
                                break;

                            case StructureConstants.RepeatedEmptyWallFlag:
                                placementActions.Add(PlacementAction.PlaceEmptyWallRepeated(reader.ReadUInt16()));
                                break;

                            default:
                                placementActions.Add(PlacementAction.PlaceWall(action));
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
                        throw new System.Exception(
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
                        throw new System.Exception(
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