using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
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
			PlaceWater,
			PlaceLava,
			PlaceHoney,
			PlaceWaterRepeated,
			PlaceLavaRepeated,
			PlaceHoneyRepeated,
			PlaceWall,
			PlaceWallRepeated,
			PlaceEmptyWall,
			PlaceEmptyWallRepeated,
			PlaceSlope,
			PlaceSlopeRepeated,
			PlaceHalfBrick,
			PlaceHalfBrickRepeated,
			PlaceDirectFramed,
			PaintTile,
			PaintTileRepeated,
			PaintWall,
			PaintWallRepeated
		}

		private struct PlacementAction
		{
			public PlacementActionType Type;
			public ushort EntryData;
			public ushort RepetitionData;
			public int FrameData;
			public byte LiquidData;
			public byte SlopeData;

			public PlacementAction(PlacementActionType actionType, ushort entryData, ushort repetitionData, int frameData, byte liquidData, byte slopeData)
			{
				Type = actionType;
				LiquidData = liquidData;
				EntryData = entryData;
				RepetitionData = repetitionData;
				FrameData = frameData;
				SlopeData = slopeData;
			}

			public static PlacementAction AirTile => new PlacementAction(PlacementActionType.PlaceAir, 0, 0, 0, 0, 0);

			public static PlacementAction EmptyWall => new PlacementAction(PlacementActionType.PlaceEmptyWall, 0, 0, 0, 0, 0);

			public static PlacementAction PlaceAirRepeated(ushort count) => new PlacementAction(PlacementActionType.PlaceAirRepeated, 0, count, 0, 0, 0);

			public static PlacementAction PlaceTile(ushort entry) => new PlacementAction(PlacementActionType.PlaceTile, entry, 0, 0, 0, 0);

			public static PlacementAction PlaceTileRepeated(ushort entry, ushort count) => new PlacementAction(PlacementActionType.PlaceTileRepeated, entry, count, 0, 0, 0);

			public static PlacementAction PlaceWater(byte amount) => new PlacementAction(PlacementActionType.PlaceWater, 0, 0, 0, amount, 0);

			public static PlacementAction PlaceLava(byte amount) => new PlacementAction(PlacementActionType.PlaceLava, 0, 0, 0, amount, 0);

			public static PlacementAction PlaceHoney(byte amount) => new PlacementAction(PlacementActionType.PlaceHoney, 0, 0, 0, amount, 0);

			public static PlacementAction PlaceWaterRepeated(ushort count, byte amount) => new PlacementAction(PlacementActionType.PlaceWaterRepeated, 0, count, 0, amount, 0);

			public static PlacementAction PlaceLavaRepeated(ushort count, byte amount) => new PlacementAction(PlacementActionType.PlaceLavaRepeated, 0, count, 0, amount, 0);

			public static PlacementAction PlaceHoneyRepeated(ushort count, byte amount) => new PlacementAction(PlacementActionType.PlaceHoneyRepeated, 0, count, 0, amount, 0);

			public static PlacementAction PlaceWall(ushort entry) => new PlacementAction(PlacementActionType.PlaceWall, entry, 0, 0, 0, 0);

			public static PlacementAction PlaceWallRepeated(ushort entry, ushort count) => new PlacementAction(PlacementActionType.PlaceWallRepeated, entry, count, 0, 0, 0);

			public static PlacementAction PlaceEmptyWallRepeated(ushort count) => new PlacementAction(PlacementActionType.PlaceEmptyWallRepeated, 0, count, 0, 0, 0);

			public static PlacementAction PlaceSlope(byte slope, ushort entry) => new PlacementAction(PlacementActionType.PlaceSlope, entry, 0, 0, 0, slope);

			public static PlacementAction PlaceSlopeRepeated(ushort count, byte slope, ushort entry) => new PlacementAction(PlacementActionType.PlaceSlopeRepeated, entry, count, 0, 0, slope);

			public static PlacementAction PlaceHalfBrick(ushort entry) => new PlacementAction(PlacementActionType.PlaceHalfBrick, entry, 0, 0, 0, 0);

			public static PlacementAction PlaceHalfBrickRepeated(ushort count, ushort entry) => new PlacementAction(PlacementActionType.PlaceHalfBrickRepeated, entry, count, 0, 0, 0);

			public static PlacementAction PlaceDirectFramed(ushort entry, int frameData) => new PlacementAction(PlacementActionType.PlaceDirectFramed, entry, 0, frameData, 0, 0);

			public static PlacementAction PaintTile(byte color) => new PlacementAction(PlacementActionType.PaintTile, color, 0, 0, 0, 0);

			public static PlacementAction PaintTileRepeated(byte color, ushort count) => new PlacementAction(PlacementActionType.PaintTileRepeated, color, count, 0, 0, 0);

			public static PlacementAction PaintWall(byte color) => new PlacementAction(PlacementActionType.PaintWall, color, 0, 0, 0, 0);

			public static PlacementAction PaintWallRepeated(byte color, ushort count) => new PlacementAction(PlacementActionType.PaintWallRepeated, color, count, 0, 0, 0);
		}

		public readonly int Width;
		public readonly int Height;

		private Dictionary<ushort, ushort> EntryToTileID { get; set; }

		private Dictionary<ushort, ushort> EntryToWallID { get; set; }

		private PlacementAction[] PlacementActions { get; set; }

		private const ushort RepeatedAirFlag = 0xFFFF;
		private const ushort AirTile = 0xFFFE;
		private const ushort RepeatedTileFlag = 0xFFFD;
		private const ushort PlaceWaterFlag = 0xFFFC;
		private const ushort PlaceLavaFlag = 0xFFFB;
		private const ushort PlaceHoneyFlag = 0xFFFA;
		private const ushort RepeatedWaterFlag = 0xFFF9;
		private const ushort RepeatedLavaFlag = 0xFFF8;
		private const ushort RepeatedHoneyFlag = 0xFFF7;
		private const ushort RepeatedWallFlag = 0xFFF6;
		private const ushort EmptyWallFlag = 0xFFF5;
		private const ushort RepeatedEmptyWallFlag = 0xFFF4;
		private const ushort PlaceTileWithSlopeFlag = 0xFFF3;
		private const ushort RepeatedTileWithSlopeFlag = 0xFFF2;
		private const ushort PlaceHalfBrickFlag = 0xFFF1;
		private const ushort RepeatedHalfBrickFlag = 0xFFEE;
		private const ushort PlaceDirectFramedFlag = 0xFFED;
		private const ushort EndOfTilesDataFlag = 0xFFEA;
		private const ushort EndOfWallsDataFlag = 0xFFE9;

		private const byte EndOfTilePaintDataFlag = 0x21;
		private const byte RepeatedTilePaintFlag = 0x20;
		private const byte RepeatedWallPaintFlag = 0x1F;

		private const byte StructureFileFormatVersion = 0;

		private Structure(int width, int height, Dictionary<ushort, ushort> tileMap, Dictionary<ushort, ushort> wallMap, PlacementAction[] placementActions)
		{
			Width = width;
			Height = height;
			EntryToTileID = tileMap;
			EntryToWallID = wallMap;
			PlacementActions = placementActions;
		}

		public void PlaceAt(int x, int y, bool keepWater = false, bool keepTiles = false, bool submerge = false)
		{
			int i = x;
			int j = y;

			PrepareAreaForStructure(x, y);

			foreach (PlacementAction action in PlacementActions)
			{
				if (submerge)
				{
					Tile tile = Framing.GetTileSafely(i, j);
					tile.LiquidType = 0;
					tile.LiquidAmount = 255;
				}

				if (action.Type == PlacementActionType.PlaceAirRepeated && !keepTiles && !keepWater)
				{
					for (int z = i; z < i + action.RepetitionData; z++)
						KillTile(z, j, false, noItem: true);

					i += action.RepetitionData;
                }
                if (action.Type == PlacementActionType.PlaceAirRepeated && keepWater && !keepTiles)
                {
					for (int z = i; z < i + action.RepetitionData; z++)
					{
						byte liquid = Framing.GetTileSafely(z, j).LiquidAmount;

						KillTile(z, j, false, noItem: true);

						Framing.GetTileSafely(z, j).LiquidAmount = liquid;
					}

					i += action.RepetitionData;
                }
				if (action.Type == PlacementActionType.PlaceAirRepeated && keepTiles)
				{
					i += action.RepetitionData;
				}
				else if (action.Type == PlacementActionType.PlaceAir && keepTiles)
				{
					i++;
				}
				else if (action.Type == PlacementActionType.PlaceAir && keepWater && !keepTiles)
				{
					byte liquid = Framing.GetTileSafely(i, j).LiquidAmount;

					KillTile(i, j, false, noItem: true);

					Framing.GetTileSafely(i, j).LiquidAmount = liquid;

					i++;
				}
				else if (action.Type == PlacementActionType.PlaceAir && !keepWater && !keepTiles)
				{
					i++;
				}
				else if (action.Type == PlacementActionType.PlaceTile)
				{
					Tile tile = Framing.GetTileSafely(i, j);
					tile.type = EntryToTileID[action.EntryData];
					tile.IsActive = true;

					if (submerge)
					{
						tile.LiquidType = 0;
						tile.LiquidAmount = 255;
					}

					i++;
				}
				else if (action.Type == PlacementActionType.PlaceTileRepeated)
				{
					for (int z = i; z < i + action.RepetitionData; z++)
					{
						Tile tile = Framing.GetTileSafely(z, j);
						tile.type = EntryToTileID[action.EntryData];
						tile.IsActive = true;

						if (submerge)
						{
							tile.LiquidType = 0;
							tile.LiquidAmount = 255;
						}
					}

					i += action.RepetitionData;
				}
				else if (action.Type == PlacementActionType.PlaceWater)
				{
					Tile tile = Framing.GetTileSafely(i, j);
					tile.LiquidType = 0;
					tile.LiquidAmount = action.LiquidData;
				}
				else if (action.Type == PlacementActionType.PlaceLava)
				{
					Tile tile = Framing.GetTileSafely(i, j);
					tile.LiquidType = 1;
					tile.LiquidAmount = action.LiquidData;
				}
				else if (action.Type == PlacementActionType.PlaceHoney)
				{
					Tile tile = Framing.GetTileSafely(i, j);
					tile.LiquidType = 2;
					tile.LiquidAmount = action.LiquidData;
				}
				else if (action.Type == PlacementActionType.PlaceWaterRepeated)
				{
					for (int z = i; z < i + action.RepetitionData; z++)
					{
						Tile tile = Framing.GetTileSafely(z, j);
						tile.LiquidType = 0;
						tile.LiquidAmount = action.LiquidData;
					}

					i += action.RepetitionData;
				}
				else if (action.Type == PlacementActionType.PlaceLavaRepeated)
				{
					for (int z = i; z < i + action.RepetitionData; z++)
					{
						Tile tile = Framing.GetTileSafely(z, j);
						tile.LiquidType = 1;
						tile.LiquidAmount = action.LiquidData;
					}

					i += action.RepetitionData;
				}
				else if (action.Type == PlacementActionType.PlaceHoneyRepeated)
				{
					for (int z = i; z < i + action.RepetitionData; z++)
					{
						Tile tile = Framing.GetTileSafely(z, j);
						tile.LiquidType = 2;
						tile.LiquidAmount = action.LiquidData;
					}

					i += action.RepetitionData;
				}
				else if (action.Type == PlacementActionType.PlaceWall)
				{
					PlaceWall(i, j, EntryToWallID[action.EntryData], true);

					if (submerge)
					{
						Main.tile[i, j].LiquidType = 0;
						Main.tile[i, j].LiquidAmount = 255;
					}

					i++;
				}
				else if (action.Type == PlacementActionType.PlaceWallRepeated)
				{
					for (int z = i; z < i + action.RepetitionData; z++)
					{
						PlaceWall(z, j, EntryToWallID[action.EntryData], true);

						if (submerge)
						{
							Main.tile[z, j].LiquidType = 0;
							Main.tile[z, j].LiquidAmount = 255;
						}
					}

					i += action.RepetitionData;
				}
				else if (action.Type == PlacementActionType.PlaceEmptyWall)
				{
					KillWall(i, j, false);

					if (submerge)
					{
						Main.tile[i, j].LiquidType = 0;
						Main.tile[i, j].LiquidAmount = 255;
					}

					i++;
				}
				else if (action.Type == PlacementActionType.PlaceEmptyWallRepeated)
				{
					for (int z = i; z < i + action.RepetitionData; z++)
					{
						KillWall(z, j, false);

						if (submerge)
						{
							Main.tile[z, j].LiquidType = 0;
							Main.tile[z, j].LiquidAmount = 255;
						}
					}

					i += action.RepetitionData;
				}
				else if (action.Type == PlacementActionType.PlaceSlope)
				{
					if (InWorld(i, j))
					{
						Tile tile = Main.tile[i, j];
						tile.type = EntryToTileID[action.EntryData];
						tile.Slope = (SlopeType)action.SlopeData;
						tile.IsActive = true;

						if (submerge)
						{
							tile.LiquidType = 0;
							tile.LiquidAmount = 255;
						}
					}

					i++;
				}
				else if (action.Type == PlacementActionType.PlaceSlopeRepeated)
				{
					for (int z = i; z < i + action.RepetitionData; z++)
					{
						if (InWorld(z, j))
						{
							Tile tile = Main.tile[z, j];
							tile.type = EntryToTileID[action.EntryData];
							tile.Slope = (SlopeType)action.SlopeData;
							tile.IsActive = true;

							if (submerge)
							{
								tile.LiquidType = 0;
								tile.LiquidAmount = 255;
							}
						}
					}

					i += action.RepetitionData;
				}
				else if (action.Type == PlacementActionType.PlaceHalfBrick)
				{
					if (InWorld(i, j))
					{
						Tile tile = Main.tile[i, j];
						tile.type = EntryToTileID[action.EntryData];
						tile.IsHalfBlock = true;
						tile.IsActive = true;

						if (submerge)
						{
							tile.LiquidType = 0;
							tile.LiquidAmount = 255;
						}
					}

					i++;
				}
				else if (action.Type == PlacementActionType.PlaceHalfBrickRepeated)
				{
					for (int z = i; z < i + action.RepetitionData; z++)
					{
						if (InWorld(z, j))
						{
							Tile tile = Main.tile[z, j];
							tile.type = EntryToTileID[action.EntryData];
							tile.IsHalfBlock = true;
							tile.IsActive = true;

							if (submerge)
							{
								tile.LiquidType = 0;
								tile.LiquidAmount = 255;
							}
						}
					}

					i += action.RepetitionData;
				}
				else if (action.Type == PlacementActionType.PlaceDirectFramed)
				{
					byte[] data = BitConverter.GetBytes(action.FrameData);
					short frameX = BitConverter.ToInt16(data, 0);
					short frameY = BitConverter.ToInt16(data, 2);

					if (InWorld(i, j))
					{
						Tile tile = Main.tile[i, j];
						tile.type = EntryToTileID[action.EntryData];
						tile.frameX = frameX;
						tile.frameY = frameY;
						tile.IsActive = true;

						if (submerge)
						{
							tile.LiquidType = 0;
							tile.LiquidAmount = 255;
						}
					}

					i++;
				}
				else if (action.Type == PlacementActionType.PaintTile)
				{
					if (InWorld(i, j))
					{
						Main.tile[i, j].Color = (byte)action.EntryData;

						if (submerge)
						{
							Main.tile[i, j].LiquidType = 0;
							Main.tile[i, j].LiquidAmount = 255;
						}
					}

					i++;
				}
				else if (action.Type == PlacementActionType.PaintTileRepeated)
				{
					for (int z = i; z < i + action.RepetitionData; z++)
						if (InWorld(z, j))
						{
							Main.tile[z, j].Color = (byte)action.EntryData;

							if (submerge)
							{
								Main.tile[z, j].LiquidType = 0;
								Main.tile[z, j].LiquidAmount = 255;
							}
						}

					i += action.RepetitionData;
				}
				else if (action.Type == PlacementActionType.PaintWall)
				{
					if (InWorld(i, j))
					{
						Main.tile[i, j].WallColor = (byte)action.EntryData;

						if (submerge)
						{
							Main.tile[i, j].LiquidType = 0;
							Main.tile[i, j].LiquidAmount = 255;
						}
					}

					i++;
				}
				else if (action.Type == PlacementActionType.PaintWallRepeated)
				{
					for (int z = i; z < i + action.RepetitionData; z++)
						if (InWorld(z, j))
						{
							Main.tile[z, j].WallColor = (byte)action.EntryData;

							if (submerge)
							{
								Main.tile[z, j].LiquidType = 0;
								Main.tile[z, j].LiquidAmount = 255;
							}
						}

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
			bool needsTilePaint = false;
			bool needsWallPaint = false;

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

							if (tile.Color != 0)
								needsTilePaint = true;

							if (tile.IsActive)
							{
								bool vanillaTile = tile.type < TileID.Count;
								ushort indexInMap = vanillaTile ? vanillaTileEntryMap[tile.type] : moddedTileEntryMap[tile.type];

								if (Main.tileFrameImportant[tile.type])
								{
									byte[] firstHalf = BitConverter.GetBytes(tile.frameX);
									byte[] secondHalf = BitConverter.GetBytes(tile.frameY);
									int frameData = BitConverter.ToInt32(new[] { firstHalf[0], firstHalf[1], secondHalf[0], secondHalf[1] }, 0);

									writer.Write(PlaceDirectFramedFlag);
									writer.Write(indexInMap);
									writer.Write(frameData);
								}
								else if (tile.IsHalfBlock)
								{
									Tile nextTile = Framing.GetTileSafely(i + 1, j);

									if (i + 1 < endX && nextTile.type == tile.type && nextTile.IsHalfBlock)
									{
										ushort identicalHalfBricks = 0;

										while (i < endX && nextTile.type == tile.type && nextTile.IsHalfBlock)
										{
											identicalHalfBricks++;
											nextTile = Framing.GetTileSafely(++i, j);
										}

										i--;

										writer.Write(RepeatedHalfBrickFlag);
										writer.Write(identicalHalfBricks);
									}
									else
										writer.Write(PlaceHalfBrickFlag);

									writer.Write(indexInMap);
								}
								else if (tile.Slope != 0)
								{
									Tile nextTile = Framing.GetTileSafely(i + 1, j);
									byte tileSlope = (byte)tile.Slope;

									if (i + 1 < endX && nextTile.type == tile.type && nextTile.IsActive && (byte)nextTile.Slope == tileSlope)
									{
										ushort identicalSlopes = 0;

										while (i < endX && nextTile.type == tile.type && nextTile.IsActive && (byte)nextTile.Slope == tileSlope)
										{
											identicalSlopes++;
											nextTile = Framing.GetTileSafely(++i, j);
										}

										i--;

										writer.Write(RepeatedTileWithSlopeFlag);
										writer.Write(identicalSlopes);
									}
									else
										writer.Write(PlaceTileWithSlopeFlag);

									writer.Write(tileSlope);
									writer.Write(indexInMap);
								}
								else
								{
									Tile nextTile = Framing.GetTileSafely(i + 1, j);

									if (i + 1 < endX && nextTile.type == tile.type && nextTile.Slope == 0 && !nextTile.IsHalfBlock)
									{
										ushort identicalTiles = 0;

										while (i < endX && nextTile.type == tile.type && nextTile.Slope == 0 && !nextTile.IsHalfBlock)
										{
											identicalTiles++;
											nextTile = Framing.GetTileSafely(++i, j);
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
							else if (tile.LiquidAmount > 0)
							{
								int liquidType = tile.LiquidType;

								if (i + 1 < endX && Framing.GetTileSafely(i + 1, j).LiquidAmount > 0)
								{
									ushort identicalLiquids = 0;

									while (i < endX && !Framing.GetTileSafely(i, j).IsActive)
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

								writer.Write(tile.LiquidAmount);
							}
							else
							{
								if (i + 1 < endX && !Framing.GetTileSafely(i + 1, j).IsActive)
								{
									ushort skippedTiles = 0;

									while (i < endX && !Framing.GetTileSafely(i, j).IsActive)
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

							if (tile.WallColor != 0)
								needsWallPaint = true;

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

					writer.Write(EndOfWallsDataFlag);

					if (!needsTilePaint)
						return stream.ToArray();

					for (int j = y; j < y + height; j++)
					{
						for (int i = x; i < x + width; i++)
						{
							Tile tile = Framing.GetTileSafely(i, j);
							byte color = tile.Color;

							if (i + 1 < endX && Framing.GetTileSafely(i + 1, j).Color == color)
							{
								ushort identicalColors = 0;

								while (i < endX && Framing.GetTileSafely(i, j).Color == color)
								{
									identicalColors++;
									i++;
								}

								i--;

								writer.Write(RepeatedTilePaintFlag);
								writer.Write(color);
								writer.Write(identicalColors);
							}
							else
								writer.Write(color);
						}
					}

					writer.Write(EndOfTilePaintDataFlag);

					if (!needsWallPaint)
						return stream.ToArray();

					for (int j = y; j < y + height; j++)
					{
						for (int i = x; i < x + width; i++)
						{
							Tile tile = Framing.GetTileSafely(i, j);
							byte color = tile.WallColor;

							if (i + 1 < endX && Framing.GetTileSafely(i + 1, j).WallColor == color)
							{
								ushort identicalColors = 0;

								while (i < endX && Framing.GetTileSafely(i, j).WallColor == color)
								{
									identicalColors++;
									i++;
								}

								i--;

								writer.Write(RepeatedWallPaintFlag);
								writer.Write(color);
								writer.Write(identicalColors);
							}
							else
								writer.Write(color);
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
						else if (action == PlaceTileWithSlopeFlag)
							placementActions.Add(PlacementAction.PlaceSlope(reader.ReadByte(), reader.ReadUInt16()));
						else if (action == RepeatedTileWithSlopeFlag)
							placementActions.Add(PlacementAction.PlaceSlopeRepeated(reader.ReadUInt16(), reader.ReadByte(), reader.ReadUInt16()));
						else if (action == PlaceHalfBrickFlag)
							placementActions.Add(PlacementAction.PlaceHalfBrick(reader.ReadUInt16()));
						else if (action == RepeatedHalfBrickFlag)
							placementActions.Add(PlacementAction.PlaceHalfBrickRepeated(reader.ReadUInt16(), reader.ReadUInt16()));
						else if (action == PlaceDirectFramedFlag)
							placementActions.Add(PlacementAction.PlaceDirectFramed(reader.ReadUInt16(), reader.ReadInt32()));
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
						else if (action == EndOfWallsDataFlag)
							break;
						else
							placementActions.Add(PlacementAction.PlaceWall(action));
					}

					while (stream.Position < stream.Length)
					{
						action = reader.ReadByte();

						if (action == RepeatedTilePaintFlag)
							placementActions.Add(PlacementAction.PaintTileRepeated(reader.ReadByte(), reader.ReadUInt16()));
						else if (action == EndOfTilePaintDataFlag)
							break;
						else
							placementActions.Add(PlacementAction.PaintTile((byte)action));
					}

					while (stream.Position < stream.Length)
					{
						action = reader.ReadByte();

						if (action == RepeatedWallPaintFlag)
							placementActions.Add(PlacementAction.PaintWallRepeated(reader.ReadByte(), reader.ReadUInt16()));
						else
							placementActions.Add(PlacementAction.PaintWall((byte)action));
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
					if (!tile.IsActive)
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
					writer.Write(modTile.Mod.Name + "." + modTile.Name);
				}
			}
			else
			{
				foreach (KeyValuePair<ushort, ushort> moddedEntry in moddedEntryMap)
				{
					writer.Write(moddedEntry.Value);

					ModWall modWall = WallLoader.GetWall(moddedEntry.Key);
					writer.Write(modWall.Mod.Name + "." + modWall.Name);
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

			/*if (!isWalls)
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

					ushort? type = ModLoader.GetMod(parts[0])?.GetContent<ModWall>().GetType();
					if (type == null)
						throw new System.Exception($"Attempted to generate structure that depends on modded wall '{tileName}' but it was not loaded");

					entryMap[index] = type.Value;
				}
			}*/

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

					//Framing.GetTileSafely(b, a).LiquidAmount = 0;
					Framing.GetTileSafely(b, a).Slope = SlopeType.Solid;
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
	}
}