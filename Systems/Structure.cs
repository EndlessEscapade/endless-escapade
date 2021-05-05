using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.WorldGen;

namespace EEMod.Systems
{
	public class Structure
	{
		private enum PlacementActionType : byte
		{
			PlaceAirRepeated,
			PlaceAir,
			PlaceTileRepeated,
			PlaceTile,
			PlaceMultitile,
			PlaceWater,
			PlaceLava,
			PlaceHoney,
			PlaceWaterRepeated,
			PlaceLavaRepeated,
			PlaceHoneyRepeated,
			PlaceMultitileWithStyle,
			PlaceMultitileWithAlternateStyle,
			PlaceWall,
			PlaceWallRepeated,
			PlaceEmptyWall,
			PlaceEmptyWallRepeated,
		}

		private struct PlacementAction
		{
			public PlacementActionType Type;
			public ushort EntryData;
			public ushort RepetitionData;
			public byte LiquidData;
			public byte StyleData;
			public byte AlternateStyleData;

			public PlacementAction(PlacementActionType actionType, ushort entryData, ushort repetitionData, byte liquidData, byte styleData, byte altData)
			{
				Type = actionType;
				LiquidData = liquidData;
				EntryData = entryData;
				RepetitionData = repetitionData;
				StyleData = styleData;
				AlternateStyleData = altData;
			}

			public static PlacementAction AirTile => new PlacementAction(PlacementActionType.PlaceAir, 0, 0, 0, 0, 0);

			public static PlacementAction EmptyWall => new PlacementAction(PlacementActionType.PlaceEmptyWall, 0, 0, 0, 0, 0);

			public static PlacementAction PlaceAirRepeated(ushort count) => new PlacementAction(PlacementActionType.PlaceAirRepeated, 0, count, 0, 0, 0);

			public static PlacementAction PlaceTile(ushort entry) => new PlacementAction(PlacementActionType.PlaceTile, entry, 0, 0, 0, 0);

			public static PlacementAction PlaceTileRepeated(ushort entry, ushort count) => new PlacementAction(PlacementActionType.PlaceTileRepeated, entry, count, 0, 0, 0);

			public static PlacementAction PlaceMultitile(ushort entry) => new PlacementAction(PlacementActionType.PlaceMultitile, entry, 0, 0, 0, 0);

			public static PlacementAction PlaceWater(byte amount) => new PlacementAction(PlacementActionType.PlaceWater, 0, 0, amount, 0, 0);

			public static PlacementAction PlaceLava(byte amount) => new PlacementAction(PlacementActionType.PlaceLava, 0, 0, amount, 0, 0);

			public static PlacementAction PlaceHoney(byte amount) => new PlacementAction(PlacementActionType.PlaceHoney, 0, 0, amount, 0, 0);

			public static PlacementAction PlaceWaterRepeated(ushort count, byte amount) => new PlacementAction(PlacementActionType.PlaceWaterRepeated, 0, count, amount, 0, 0);

			public static PlacementAction PlaceLavaRepeated(ushort count, byte amount) => new PlacementAction(PlacementActionType.PlaceLavaRepeated, 0, count, amount, 0, 0);

			public static PlacementAction PlaceHoneyRepeated(ushort count, byte amount) => new PlacementAction(PlacementActionType.PlaceHoneyRepeated, 0, count, amount, 0, 0);

			public static PlacementAction PlaceMultitileWithStyle(byte style, ushort entry) => new PlacementAction(PlacementActionType.PlaceMultitileWithStyle, entry, 0, 0, style, 0);

			public static PlacementAction PlaceMultitileWithAlternateStyle(byte style, byte alternate, ushort entry) => new PlacementAction(PlacementActionType.PlaceMultitileWithAlternateStyle, entry, 0, 0, style, alternate);

			public static PlacementAction PlaceWall(ushort entry) => new PlacementAction(PlacementActionType.PlaceWall, entry, 0, 0, 0, 0);

			public static PlacementAction PlaceWallRepeated(ushort entry, ushort count) => new PlacementAction(PlacementActionType.PlaceWallRepeated, entry, count, 0, 0, 0);

			public static PlacementAction PlaceEmptyWallRepeated(ushort count) => new PlacementAction(PlacementActionType.PlaceEmptyWallRepeated, 0, count, 0, 0, 0);
		}

		public readonly int Width;
		public readonly int Height;

		private Dictionary<ushort, ushort> EntryToTileID { get; set; }

		private Dictionary<ushort, ushort> EntryToWallID { get; set; }

		private PlacementAction[] PlacementActions { get; set; }

		private const ushort RepeatedAirFlag = 0xFFFF;
		private const ushort AirTile = 0xFFFE;
		private const ushort RepeatedTileFlag = 0xFFFD;
		private const ushort PlaceMultitileFlag = 0xFFFC;
		private const ushort PlaceWaterFlag = 0xFFFB;
		private const ushort PlaceLavaFlag = 0xFFFA;
		private const ushort PlaceHoneyFlag = 0xFFF9;
		private const ushort RepeatedWaterFlag = 0xFFF8;
		private const ushort RepeatedLavaFlag = 0xFFF7;
		private const ushort RepeatedHoneyFlag = 0xFFF6;
		private const ushort PlaceMultitileWithStyleFlag = 0xFFF5;
		private const ushort PlaceMultitileWithAlternateStyleFlag = 0xFFF4;
		private const ushort RepeatedWallFlag = 0xFFF3;
		private const ushort EmptyWallFlag = 0xFFF2;
		private const ushort RepeatedEmptyWallFlag = 0xFFF1;
		private const ushort EndOfTilesDataFlag = 0xFFF0;

		private const byte StructureFileFormatVersion = 0;

		private Structure(int width, int height, Dictionary<ushort, ushort> tileMap, Dictionary<ushort, ushort> wallMap, PlacementAction[] placementActions)
		{
			Width = width;
			Height = height;
			EntryToTileID = tileMap;
			EntryToWallID = wallMap;
			PlacementActions = placementActions;
		}

		public void PlaceAt(int x, int y)
		{
			int i = x;
			int j = y;
			List<(Point, ushort, ushort, ushort)> deferredMultitiles = new List<(Point, ushort, ushort, ushort)>();

			PrepareAreaForStructure(x, y);

			foreach (PlacementAction action in PlacementActions)
			{
				if (action.Type == PlacementActionType.PlaceAirRepeated)
				{
					for (int z = i; z < i + action.RepetitionData; z++)
						KillTile(z, j, false, noItem: true);

					i += action.RepetitionData;
				}
				else if (action.Type == PlacementActionType.PlaceAir)
				{
					KillTile(i, j, false, noItem: true);
					i++;
				}
				else if (action.Type == PlacementActionType.PlaceTile)
				{
					PlaceTile(i, j, EntryToTileID[action.EntryData], true, true);
					i++;
				}
				else if (action.Type == PlacementActionType.PlaceTileRepeated)
				{
					for (int z = i; z < i + action.RepetitionData; z++)
						PlaceTile(z, j, EntryToTileID[action.EntryData], true, true);

					i += action.RepetitionData;
				}
				else if (action.Type == PlacementActionType.PlaceMultitile)
				{
					deferredMultitiles.Add((new Point(i, j), EntryToTileID[action.EntryData], 0, 0));
					i++;
				}
				else if (action.Type == PlacementActionType.PlaceWater)
				{
					Tile tile = Framing.GetTileSafely(i, j);
					tile.liquidType(0);
					tile.liquid = action.LiquidData;
				}
				else if (action.Type == PlacementActionType.PlaceLava)
				{
					Tile tile = Framing.GetTileSafely(i, j);
					tile.liquidType(1);
					tile.liquid = action.LiquidData;
				}
				else if (action.Type == PlacementActionType.PlaceHoney)
				{
					Tile tile = Framing.GetTileSafely(i, j);
					tile.liquidType(2);
					tile.liquid = action.LiquidData;
				}
				else if (action.Type == PlacementActionType.PlaceWaterRepeated)
				{
					for (int z = i; z < i + action.RepetitionData; z++)
					{
						Tile tile = Framing.GetTileSafely(z, j);
						tile.liquidType(0);
						tile.liquid = action.LiquidData;
					}

					i += action.RepetitionData;
				}
				else if (action.Type == PlacementActionType.PlaceLavaRepeated)
				{
					for (int z = i; z < i + action.RepetitionData; z++)
					{
						Tile tile = Framing.GetTileSafely(z, j);
						tile.liquidType(1);
						tile.liquid = action.LiquidData;
					}

					i += action.RepetitionData;
				}
				else if (action.Type == PlacementActionType.PlaceHoneyRepeated)
				{
					for (int z = i; z < i + action.RepetitionData; z++)
					{
						Tile tile = Framing.GetTileSafely(z, j);
						tile.liquidType(2);
						tile.liquid = action.LiquidData;
					}

					i += action.RepetitionData;
				}
				else if (action.Type == PlacementActionType.PlaceMultitileWithStyle)
				{
					deferredMultitiles.Add((new Point(i, j), EntryToTileID[action.EntryData], action.StyleData, 0));
					i++;
				}
				else if (action.Type == PlacementActionType.PlaceMultitileWithAlternateStyle)
				{
					deferredMultitiles.Add((new Point(i, j), EntryToTileID[action.EntryData], action.StyleData, action.AlternateStyleData));
					i++;
				}
				else if (action.Type == PlacementActionType.PlaceWall)
				{
					PlaceWall(i, j, EntryToWallID[action.EntryData], true);
					i++;
				}
				else if (action.Type == PlacementActionType.PlaceWallRepeated)
				{
					for (int z = i; z < i + action.RepetitionData; z++)
						PlaceWall(z, j, EntryToWallID[action.EntryData], true);

					i += action.RepetitionData;
				}
				else if (action.Type == PlacementActionType.PlaceEmptyWall)
				{
					KillWall(i, j, false);
					i++;
				}
				else if (action.Type == PlacementActionType.PlaceEmptyWallRepeated)
				{
					for (int z = i; z < i + action.RepetitionData; z++)
						KillWall(z, j, false);

					i += action.RepetitionData;
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

			foreach ((Point pos, ushort type, ushort style, ushort alternate) in deferredMultitiles)
			{
				TileObject tileObject = new TileObject();
				tileObject.xCoord = pos.X;
				tileObject.yCoord = pos.Y;
				tileObject.style = style;
				tileObject.alternate = alternate;

				TileObject.Place(tileObject);
			}

			FinalizeArea(x, y);
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
					writer.Write(StructureFileFormatVersion);
					writer.Write((ushort)width);
					writer.Write((ushort)height);

					(var vanillaTileEntryMap, var moddedTileEntryMap) = CreateAreaTileMapData(x, y, width, height);
					WriteMapData(vanillaTileEntryMap, moddedTileEntryMap, false, writer);

					for (int j = y; j < y + height; j++)
					{
						for (int i = x; i < x + width; i++)
						{
							Tile tile = Framing.GetTileSafely(i, j);

							if (tile.active())
							{
								bool vanillaTile = tile.type < TileID.Count;
								ushort indexInMap = vanillaTile ? vanillaTileEntryMap[tile.type] : moddedTileEntryMap[tile.type];

								TileObjectData tileObjectData = TileObjectData.GetTileData(tile);
								if (tileObjectData != null)
								{
									Point16 tileObjectDataOrigin = tileObjectData.Origin;
									Point tileTopLeft = GetTileTopLeft(i, j);
									Point tileOrigin = new Point(tileObjectDataOrigin.X + tileTopLeft.X, tileObjectDataOrigin.Y + tileTopLeft.Y);

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
												writer.Write(PlaceMultitileWithStyleFlag);
												writer.Write((byte)style);
											}
											else
											{
												writer.Write(PlaceMultitileWithAlternateStyleFlag);
												writer.Write((byte)style);
												writer.Write((byte)alternate);
											}
										}
										else
											writer.Write(PlaceMultitileFlag);

										writer.Write(indexInMap);
									}
									else
										writer.Write(AirTile);
								}
								else
								{
									if (i + 1 < endX && Framing.GetTileSafely(i + 1, j).type == tile.type)
									{
										ushort identicalTiles = 0;

										while (i < endX && Framing.GetTileSafely(i, j).type == tile.type)
										{
											identicalTiles++;
											i++;
										}

										i--;

										writer.Write(RepeatedTileFlag);
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

									if (liquidType == 0)
										writer.Write(RepeatedWaterFlag);
									else if (liquidType == 1)
										writer.Write(RepeatedLavaFlag);
									else if (liquidType == 2)
										writer.Write(RepeatedHoneyFlag);

									writer.Write(identicalLiquids);
								}
								else
								{
									if (liquidType == 0)
										writer.Write(PlaceWaterFlag);
									else if (liquidType == 1)
										writer.Write(PlaceLavaFlag);
									else if (liquidType == 2)
										writer.Write(PlaceHoneyFlag);
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

									writer.Write(RepeatedAirFlag);
									writer.Write(skippedTiles);
								}
								else
									writer.Write(AirTile);
							}
						}
					}

					writer.Write(EndOfTilesDataFlag);

					(var vanillaWallEntryMap, var moddedWallEntryMap) = CreateAreaWallMapData(x, y, width, height);
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

									writer.Write(RepeatedEmptyWallFlag);
									writer.Write(emptyWalls);
								}
								else
									writer.Write(EmptyWallFlag);

								continue;
							}

							bool vanillaWall = tile.wall < WallID.Count;
							ushort indexInMap = vanillaWall ? vanillaWallEntryMap[tile.wall] : moddedWallEntryMap[tile.wall];

							if (i + 1 < endX && Framing.GetTileSafely(i + 1, j).wall == tile.wall)
							{
								ushort identicalWalls = 0;

								while (i < endX && Framing.GetTileSafely(i, j).wall == tile.wall)
								{
									identicalWalls++;
									i++;
								}

								i--;

								writer.Write(RepeatedWallFlag);
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

						if (action == RepeatedAirFlag)
							placementActions.Add(PlacementAction.PlaceAirRepeated(reader.ReadUInt16()));
						else if (action == AirTile)
							placementActions.Add(PlacementAction.AirTile);
						else if (action == RepeatedTileFlag)
							placementActions.Add(PlacementAction.PlaceTileRepeated(reader.ReadUInt16(), reader.ReadUInt16()));
						else if (action == PlaceMultitileFlag)
							placementActions.Add(PlacementAction.PlaceMultitile(reader.ReadUInt16()));
						else if (action == PlaceWaterFlag)
							placementActions.Add(PlacementAction.PlaceWater(reader.ReadByte()));
						else if (action == PlaceLavaFlag)
							placementActions.Add(PlacementAction.PlaceLava(reader.ReadByte()));
						else if (action == PlaceHoneyFlag)
							placementActions.Add(PlacementAction.PlaceHoney(reader.ReadByte()));
						else if (action == RepeatedWaterFlag)
							placementActions.Add(PlacementAction.PlaceWaterRepeated(reader.ReadUInt16(), reader.ReadByte()));
						else if (action == RepeatedLavaFlag)
							placementActions.Add(PlacementAction.PlaceLavaRepeated(reader.ReadUInt16(), reader.ReadByte()));
						else if (action == RepeatedHoneyFlag)
							placementActions.Add(PlacementAction.PlaceHoneyRepeated(reader.ReadUInt16(), reader.ReadByte()));
						else if (action == PlaceMultitileWithStyleFlag)
							placementActions.Add(PlacementAction.PlaceMultitileWithStyle(reader.ReadByte(), reader.ReadUInt16()));
						else if (action == PlaceMultitileWithAlternateStyleFlag)
							placementActions.Add(PlacementAction.PlaceMultitileWithAlternateStyle(reader.ReadByte(), reader.ReadByte(), reader.ReadUInt16()));
						else if (action == EndOfTilesDataFlag)
							break;
						else
							placementActions.Add(PlacementAction.PlaceTile(action));
					}

					Dictionary<ushort, ushort> wallEntryMap = ReadMapData(true, reader);

					while (stream.Position < stream.Length)
					{
						action = reader.ReadUInt16();

						if (action == RepeatedWallFlag)
							placementActions.Add(PlacementAction.PlaceWallRepeated(reader.ReadUInt16(), reader.ReadUInt16()));
						else if (action == EmptyWallFlag)
							placementActions.Add(PlacementAction.EmptyWall);
						else if (action == RepeatedEmptyWallFlag)
							placementActions.Add(PlacementAction.PlaceEmptyWallRepeated(reader.ReadUInt16()));
						else
							placementActions.Add(PlacementAction.PlaceWall(action));
					}

					return new Structure(width, height, tileEntryMap, wallEntryMap, placementActions.ToArray());
				}
			}
		}

		private static (Dictionary<ushort, ushort>, Dictionary<ushort, ushort>) CreateAreaTileMapData(int x, int y, int width, int height)
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

					if (vanillaTile && !vanillaEntryMap.ContainsKey(tile.type))
						vanillaEntryMap[tile.type] = (ushort)(vanillaEntryMap.Count + moddedEntryMap.Count);
					else if (!vanillaTile && !moddedEntryMap.ContainsKey(tile.type))
						moddedEntryMap[tile.type] = (ushort)(moddedEntryMap.Count + vanillaEntryMap.Count);
				}
			}

			return (vanillaEntryMap, moddedEntryMap);
		}

		private static (Dictionary<ushort, ushort>, Dictionary<ushort, ushort>) CreateAreaWallMapData(int x, int y, int width, int height)
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

					if (vanillaWall && !vanillaEntryMap.ContainsKey(tile.wall))
						vanillaEntryMap[tile.wall] = (ushort)(vanillaEntryMap.Count + moddedEntryMap.Count);
					else if (!vanillaWall && !moddedEntryMap.ContainsKey(tile.wall))
						moddedEntryMap[tile.wall] = (ushort)(moddedEntryMap.Count + vanillaEntryMap.Count);
				}
			}

			return (vanillaEntryMap, moddedEntryMap);
		}

		private static void WriteMapData(Dictionary<ushort, ushort> vanillaEntryMap, Dictionary<ushort, ushort> moddedEntryMap, bool isWalls, BinaryWriter writer)
		{
			writer.Write((ushort)vanillaEntryMap.Count);
			foreach (KeyValuePair<ushort, ushort> vanillaEntry in vanillaEntryMap)
			{
				writer.Write(vanillaEntry.Value);
				writer.Write(vanillaEntry.Key);
			}

			writer.Write((ushort)moddedEntryMap.Count);

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

		private static Dictionary<ushort, ushort> ReadMapData(bool isWalls, BinaryReader reader)
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
						throw new System.Exception($"Attempted to generate structure that depends on modded tile '{tileName}' but it was not loaded");

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
						throw new System.Exception($"Attempted to generate structure that depends on modded wall '{tileName}' but it was not loaded");

					entryMap[index] = type.Value;
				}
			}

			return entryMap;
		}

		private void PrepareAreaForStructure(int x, int y)
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

						KillTile(b, a, false, noItem: true);
					}

					Framing.GetTileSafely(b, a).liquid = 0;
				}
			}
		}

		private void FinalizeArea(int x, int y)
		{
			for (int a = y; a < y + Height; a++)
			{
				for (int b = x; b < x + Width; b++)
				{
					SquareTileFrame(b, a);
					SquareWallFrame(b, a);
				}
			}
		}

		private static Point GetTileTopLeft(int i, int j)
		{
			if (SafeCoordinates(i, j))
			{
				Tile tile = Main.tile[i, j];

				int fX = 0;
				int fY = 0;

				if (tile != null)
				{
					TileObjectData data = TileObjectData.GetTileData(tile.type, 0);

					if (data != null)
					{
						fX = tile.frameX % (18 * data.Width) / 18;
						fY = tile.frameY % (18 * data.Height) / 18;
					}
				}

				return new Point(i - fX, j - fY);
			}

			return new Point(-1, -1);
		}
		private static bool SafeCoordinates(int i, int j) => i >= 0 && i < Main.maxTilesX && j >= 0 && j < Main.maxTilesY;
	}
}
