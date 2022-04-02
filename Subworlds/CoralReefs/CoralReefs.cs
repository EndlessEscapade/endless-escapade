using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Input;
using EEMod.ID;
using EEMod.Tiles;
using EEMod.VerletIntegration;
using EEMod.Tiles.Foliage.Coral.HangingCoral;
using EEMod.Tiles.Foliage.Coral.WallCoral;
using EEMod.Tiles.Foliage.Coral;
using EEMod.Tiles.Foliage;
using System;
using EEMod.Systems.Noise;
using System.Collections.Generic;
using EEMod.Autoloading;
using Terraria.WorldBuilding;
using EEMod.Tiles.EmptyTileArrays;
using EEMod.Tiles.Ores;
using EEMod.Tiles.Walls;
using EEMod.Tiles.Furniture;
using EEMod.Tiles.Foliage.ThermalVents;
using EEMod.Tiles.Foliage.KelpForest;
using EEMod.Tiles.Foliage.Aquamarine;
using EEMod.EEWorld;
using EEMod.Tiles.Furniture.Chests;
using System.Diagnostics;
using EEMod.Tiles.Foliage.BulboBall;
using static EEMod.EEWorld.EEWorld;
using EEMod.Items.Gliders;
using EEMod.Items.Accessories;
using EEMod.Items.Weapons.Melee.Swords;
using EEMod.Items.Weapons.Ranger.Guns;
using EEMod.Items.Weapons.Mage;
using EEMod.Items.Weapons.Summon.Minions;
using EEMod.Items.Weapons.Ammo;
using EEMod.Subworlds;
using SubworldLibrary;

namespace EEMod.Subworlds.CoralReefs
{
    public class CoralReefs : Subworld
    {
        [FieldInit] public static IList<CoralReefMinibiome> Minibiomes = new List<CoralReefMinibiome>();

        [FieldInit] public static IList<Vector2> BulbousTreePosition = new List<Vector2>();

        [FieldInit] public static IList<Vector2> CoralCrystalPosition = new List<Vector2>();
        [FieldInit] public static IList<Vector2> AquamarineZiplineLocations = new List<Vector2>();
        [FieldInit] public static IList<Vector2> ThinCrystalBambooLocations = new List<Vector2>();

        [FieldInit] public static IList<Vector2> GiantKelpRoots = new List<Vector2>();
        [FieldInit] public static IList<Vector2> WebPositions = new List<Vector2>();
        [FieldInit] public static IList<Vector2> CoralReefVineLocations = new List<Vector2>();

        public static Vector2 CoralBoatPos;
        public static Vector2 SpirePosition = Vector2.Zero;

        public override int Width => 1500;
        public override int Height => 2400; 

        private int depth = 120;
        private int boatPos = 300;

        public override string Name => "CoralReefs";

        public override bool ShouldSave => true;

        public override List<GenPass> Tasks => new List<GenPass>()
        {
            new GoblinFortGeneration(progress =>
            {
                var rand = WorldGen.genRand;
                EEMod.progressMessage = "Generating Coral Reefs";

                int roomsPerLayer = 10;

                VerletHelpers.SwingableVines.Clear();

                //Placing initial blocks
                #region Initial block placement
                EEMod.progressMessage = "Initial generation";

                FillRegion(Main.maxTilesX, (Main.maxTilesY / 10) * 4, Vector2.Zero, ModContent.TileType<LightGemsandTile>());

                FillRegionNoEditWithNoise(Main.maxTilesX, Main.maxTilesY / 60, new Vector2(0, (Main.maxTilesY / 10) * 4 - (Main.maxTilesY / 60)), ModContent.TileType<GemsandTile>(), 20);
                FillRegionNoEdit(Main.maxTilesX, (Main.maxTilesY / 10) * 3, new Vector2(0, (Main.maxTilesY / 10) * 4), ModContent.TileType<GemsandTile>());

                FillRegionNoEditWithNoise(Main.maxTilesX, Main.maxTilesY / 60, new Vector2(0, (Main.maxTilesY / 10) * 7 - (Main.maxTilesY / 60)), ModContent.TileType<DarkGemsandTile>(), 20);
                FillRegionNoEdit(Main.maxTilesX, (Main.maxTilesY / 10) * 3, new Vector2(0, (Main.maxTilesY / 10) * 7), ModContent.TileType<DarkGemsandTile>());

                ClearRegion(Main.maxTilesX, (Main.maxTilesY / 20) + (Main.maxTilesY / 60) + (Main.maxTilesY / 60), Vector2.Zero);

                #endregion

                #region Surface reefs
                EEMod.progressMessage = "Making the surface";

                NoiseGenWave(new Vector2(0, 130), new Vector2(Main.maxTilesX, Main.maxTilesY / 20), new Vector2(20, 100), (ushort)ModContent.TileType<CoralsandstoneTile>(), 0.5f);
                NoiseGenWave(new Vector2(0, 110), new Vector2(Main.maxTilesX, Main.maxTilesY / 20), new Vector2(50, 50), TileID.StoneSlab, 0.6f);

                RemoveStoneSlabs();

                //Plateaus
                FillRegionEditWithNoise(Main.maxTilesX, Main.maxTilesY / 40, new Vector2(0, 120), ModContent.TileType<CoralSandTile>(), 8);
                FillRegionEditWithNoise(Main.maxTilesX, Main.maxTilesY / 120, new Vector2(0, 155), ModContent.TileType<CoralsandstoneTile>(), 8);

                //Ground
                FillRegionNoEditWithNoise(Main.maxTilesX, Main.maxTilesY / 80, new Vector2(0, 170), ModContent.TileType<CoralsandstoneTile>(), 5);
                FillRegionNoChangeWithNoise(Main.maxTilesX, Main.maxTilesY / 80, new Vector2(0, 165), ModContent.TileType<CoralSandTile>(), 8);

                FillRegionEditWithNoise(Main.maxTilesX, Main.maxTilesY / 40, new Vector2(0, 190), ModContent.TileType<LightGemsandTile>(), 10);

                for (int i = 50; i < Main.maxTilesX - 100; i++)
                {
                    if (i >= boatPos - 60 && i <= boatPos) i += 50;

                    if (WorldGen.genRand.NextBool(200))
                    {
                        int width = WorldGen.genRand.Next(40, 60);
                        int height = WorldGen.genRand.Next(20, 25);

                        MakeOvalJaggedTop(width, height, new Vector2(i, depth - 10), ModContent.TileType<CoralSandTile>());

                        MakeOval(width - 10, 10, new Vector2(i + 5, depth - 5), TileID.Dirt, true);

                        for (int k = i; k < i + 25; k++)
                        {
                            for (int l = depth - 15; l < depth + 10; l++)
                            {
                                WorldGen.SpreadGrass(k, l);
                            }
                        }

                        i += 50;
                    }
                }

                #endregion

                #region Finding suitable room positions
                EEMod.progressMessage = "Finding suitable room positions";

                int[] biomes = Helpers.FillUniformArray(roomsPerLayer * 2, 0, 3);

                Vector2[] upperRoomPositions = MakeDistantLocations(roomsPerLayer, 150, new Rectangle(200, 265, Main.maxTilesX - 400, ((Main.maxTilesY / 10) * 4) - (265 + 100)), 5000);
                Vector2[] lowerRoomPositions = MakeDistantLocations((int)(roomsPerLayer * 0.75f), 170, new Rectangle(100, (Main.maxTilesY / 10) * 4, Main.maxTilesX - 200, (Main.maxTilesY / 10) * 3), 5000);
                Vector2[] depthsRoomPositions = MakeDistantLocations(roomsPerLayer / 2, 200, new Rectangle(300, (Main.maxTilesY / 10) * 7, Main.maxTilesX - 600, ((Main.maxTilesY / 10) * 3) - 300), 5000);

                #endregion

                #region Generating chasms
                EEMod.progressMessage = "Generating chasms";

                int highestUpperRoom = 0;
                int lowestUpperRoom = 0;
                for (int i = 0; i < upperRoomPositions.Length; i++)
                {
                    if (upperRoomPositions[i].Y < upperRoomPositions[highestUpperRoom].Y) highestUpperRoom = i;
                    if (upperRoomPositions[i].Y > upperRoomPositions[lowestUpperRoom].Y) lowestUpperRoom = i;
                }

                MakeWavyChasm3(upperRoomPositions[highestUpperRoom], new Vector2(upperRoomPositions[highestUpperRoom].X + WorldGen.genRand.Next(-100, 100), 0), TileID.StoneSlab, 100, WorldGen.genRand.Next(10, 20), true, new Vector2(10, 20), WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(5, 10), true, 51, WorldGen.genRand.Next(80, 120));

                MakeWavyChasm3(upperRoomPositions[highestUpperRoom], new Vector2(upperRoomPositions[highestUpperRoom].X + rand.Next(-100, 100), 0), TileID.StoneSlab, 100, WorldGen.genRand.Next(10, 20), true, new Vector2(10, 20), WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(5, 10), true, 51, WorldGen.genRand.Next(80, 120));

                float dist = 1000000;
                int closestLowerRoom = 0;

                for (int i = 0; i < lowerRoomPositions.Length; i++)
                {
                    if (Vector2.DistanceSquared(upperRoomPositions[lowestUpperRoom], lowerRoomPositions[i]) < dist)
                    {
                        dist = Vector2.DistanceSquared(upperRoomPositions[lowestUpperRoom], lowerRoomPositions[i]);
                        closestLowerRoom = i;
                    }
                }

                MakeWavyChasm3(upperRoomPositions[lowestUpperRoom], lowerRoomPositions[closestLowerRoom], TileID.StoneSlab, 100, WorldGen.genRand.Next(7, 15), true, new Vector2(7, 15), WorldGen.genRand.Next(7, 15), WorldGen.genRand.Next(5, 10), true, 51, WorldGen.genRand.Next(80, 120));

                for (int i = 0; i < upperRoomPositions.Length; i++)
                {
                    for (int j = i; j < upperRoomPositions.Length; j++)
                    {
                        if (Vector2.DistanceSquared(upperRoomPositions[i], upperRoomPositions[j]) <= 90000)
                        {
                            MakeWavyChasm3(upperRoomPositions[i], upperRoomPositions[j], TileID.StoneSlab, 100, WorldGen.genRand.Next(7, 15), true, new Vector2(7, 15), WorldGen.genRand.Next(7, 15), WorldGen.genRand.Next(5, 10), true, 51, WorldGen.genRand.Next(80, 120));
                        }
                    }
                }

                RemoveStoneSlabs();

                EEMod.progressMessage = "Placing chasm coral";
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
                    ModContent.TileType<BulboBall>(),
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
                new Rectangle(42, depth, Main.maxTilesX - 42, Main.maxTilesY - depth - 42));

                TilePopulate(
                    new int[] { ModContent.TileType<BigTropicalTree>(),
                    ModContent.TileType<TropicalTree>(), },
                new Rectangle(42, 42, Main.maxTilesX - 42, depth), 3);
                #endregion

                #region Spawning rooms

                for (int i = 0; i < upperRoomPositions.Length; i++)
                {
                    EEMod.progressMessage = "Generating rooms pt. 1 " + (i * 100f / upperRoomPositions.Length) + "%";
                    MakeCoralRoom((int)upperRoomPositions[i].X, (int)upperRoomPositions[i].Y, 100, 50, biomes[i]);
                }

                for (int j = 0; j < lowerRoomPositions.Length; j++)
                {
                    int biome = biomes[j + (upperRoomPositions.Length - 1)];
                    if (biome > 0) biome += 2;

                    EEMod.progressMessage = "Generating rooms pt. 2 " + (j * 100f / lowerRoomPositions.Length) + "%";
                    MakeCoralRoom((int)lowerRoomPositions[j].X, (int)lowerRoomPositions[j].Y, 150, 75, biome);
                }

                /*for (int k = 0; k < depthsRoomPositions.Length; k++)
                {
                    EEMod.progressMessage = "Generating rooms pt. 3 " + (k * 100 / depthsRoomPositions.Length) + "%";
                    MakeCoralRoom((int)depthsRoomPositions[k].X, (int)depthsRoomPositions[k].Y, 150, 75, 0);
                }*/

                #endregion

                RemoveStoneSlabs();

                try
                {
                    #region Shipwrecks
                    EEMod.progressMessage = "Wrecking ships";
                    #endregion

                    FillRegionWithWater(Main.maxTilesX, Main.maxTilesY - depth, new Vector2(0, depth));

                    #region Implementing dynamic objects

                    /*
                    EEMod.progressMessage = "Adding Dynamics";

                    for (int j = 42; j < ((Main.maxTilesY / 10f) * 4f); j += 2)
                    {
                        for (int i = 42; i < Main.maxTilesX - 42; i += 2)
                        {
                            /*int noOfTiles = 0;
                            if (j > 200)
                            {
                                /*for (int m = 0; m < OrbPositions.Count; m++)
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
                                int funnyDist = 0;
                                /*for (int m = 0; m < OrbPositions.Count; m++)
                                {
                                    if (Vector2.DistanceSquared(new Vector2(i, j), OrbPositions[m]) < 200 * 200)
                                    {
                                        funnyDist++;
                                    }
                                }
                            }*/

                    /*for (int m = 0; m < BulbousTreePosition.Count; m++)
                    {
                        if (Vector2.DistanceSquared(new Vector2(i, j), BulbousTreePosition[m]) < 20 * 20)
                        {
                            ifa++;
                        }
                    }

                    if ((TileCheck2(i, j) == 3 || TileCheck2(i, j) == 4) && WorldGen.genRand.NextBool(3))
                    {
                        if (CoralReefVineLocations.Count == 0)
                        {
                            CoralReefVineLocations.Add(new Vector2(i, j));
                        }
                        else
                        {
                            Vector2 lastPos = CoralReefVineLocations[CoralReefVineLocations.Count - 1];
                            if (Vector2.DistanceSquared(lastPos, new Vector2(i, j)) > 5 * 5 &&
                                Vector2.DistanceSquared(lastPos, new Vector2(i, j)) < 55 * 55 ||
                                Vector2.DistanceSquared(lastPos, new Vector2(i, j)) > 150 * 150)
                            {
                                CoralReefVineLocations.Add(new Vector2(i, j));
                            }
                        }
                    }*/

                    #endregion

                    #region Smoothing
                    EEMod.progressMessage = "Making the world look nice";

                    for (int i = 2; i < Main.maxTilesX - 2; i++)
                    {
                        for (int j = 2; j < Main.maxTilesY - 2; j++)
                        {
                            if (WorldGen.genRand.NextBool(3))
                            {
                                Tile.SmoothSlope(i, j);
                            }
                            if (!Framing.GetTileSafely(i, j + 1).HasTile && !Framing.GetTileSafely(i, j - 1).HasTile && !Framing.GetTileSafely(i + 1, j).HasTile && !Framing.GetTileSafely(i - 1, j).HasTile)
                            {
                                WorldGen.KillTile(i, j);
                            }
                        }
                    }
                    #endregion

                    #region Removing dirt walls
                    for (int i = 2; i < Main.maxTilesX - 2; i++)
                    {
                        for (int j = 2; j < Main.maxTilesY - 2; j++)
                        {
                            if (Framing.GetTileSafely(i, j).WallType == WallID.Dirt || Framing.GetTileSafely(i, j).WallType == WallID.DirtUnsafe || Framing.GetTileSafely(i, j).WallType == WallID.DirtUnsafe1 || Framing.GetTileSafely(i, j).WallType == WallID.DirtUnsafe2 || Framing.GetTileSafely(i, j).WallType == WallID.DirtUnsafe3 || Framing.GetTileSafely(i, j).WallType == WallID.DirtUnsafe4)
                            {
                                WorldGen.KillWall(i, j);
                            }

                            WorldGen.SquareTileFrame(i, j);
                        }
                    }
                    #endregion

                    #region Placing the boat
                    EEMod.progressMessage = "Placing boat";

                    int watercheck = depth - 22;

                    //PlaceShipWalls(boatPos, watercheck, ShipWalls);
                    //PlaceShip(boatPos, watercheck, ShipTiles);
                    CoralBoatPos = new Vector2(boatPos, watercheck);

                    for (int i = 42; i < Main.maxTilesX - 84; i++)
                    {
                        if (WorldGen.genRand.NextBool(4))
                        {
                            if (TileCheck(i, ModContent.TileType<CoralSandTile>()) > depth)
                            {
                                int ballfart = TileCheck(i, ModContent.TileType<CoralSandTile>());

                                int random = WorldGen.genRand.Next(4, 15);

                                for (int j = 1; j < random; j++)
                                {
                                    if (Framing.GetTileSafely(i, ballfart - j).HasTile || Framing.GetTileSafely(i, ballfart - j).LiquidAmount < 64) break;

                                    WorldGen.PlaceTile(i, ballfart - j, ModContent.TileType<SeagrassTile>());
                                }
                            }
                        }
                    }

                    /*for (int i = 42; i < Main.maxTilesX - 84; i++)
                    {
                        if (WorldGen.genRand.NextBool(6) && (i < boatPos - 1 || i > boatPos + ShipTiles.GetLength(1) + 1))
                        {
                            if (!Framing.GetTileSafely(i, depth - 1).HasTile && !Framing.GetTileSafely(i, depth).HasTile && !Framing.GetTileSafely(i, depth + 1).HasTile)
                            {
                                switch (WorldGen.genRand.Next(2))
                                {
                                    case 0:
                                        WorldGen.PlaceTile(i, depth - 1, ModContent.TileType<LilyPadSmol>());
                                        break;

                                    case 1:
                                        WorldGen.PlaceTile(i, depth - 1, ModContent.TileType<LilyPadMedium>());
                                        break;
                                }
                            }
                        }
                    }*/

                    for (int i = 100; i < Main.maxTilesX - 200; i++)
                    {
                        for (int j = Main.maxTilesY / 10; j < Main.maxTilesY * 4 / 10; j++)
                        {
                            if (TileCheck2(i, j) == 2 && TileCheck2(i + 1, j) == 2 && WorldGen.genRand.NextBool(30))
                            {
                                int chest = WorldGen.PlaceChest(i, j - 1, (ushort)ModContent.TileType<CoralChestTile>());

                                EEMod.Instance.Logger.Debug("Chest attempting to spawn at: " + new Vector2(i, j - 1));
                                if (chest >= 0)
                                {
                                    Item item1 = new Item();

                                    int itemRand = WorldGen.genRand.Next(6);

                                    int index = 0;

                                    switch (itemRand)
                                    {
                                        case 0:
                                            item1.SetDefaults(ModContent.ItemType<MantaRayGlider>());
                                            break;
                                        case 1:
                                            item1.SetDefaults(ModContent.ItemType<CoralEarring>());
                                            break;
                                        case 2:
                                            item1.SetDefaults(ModContent.ItemType<BubbleStriker>());
                                            break;
                                        case 3:
                                            item1.SetDefaults(ModContent.ItemType<BubbleBlitzer>());
                                            break;
                                        case 4:
                                            item1.SetDefaults(ModContent.ItemType<CoralRod>());
                                            break;
                                        case 5:
                                            item1.SetDefaults(ModContent.ItemType<AquaticReinforcmentStaff>());
                                            break;
                                        default:
                                            item1.SetDefaults(ItemID.RedPotion);
                                            break;
                                    }

                                    Main.chest[chest].item[index] = item1;
                                    index++;

                                    if (itemRand == 3)
                                    {
                                        Item bullet = new Item();
                                        bullet.SetDefaults(ItemID.MusketBall);
                                        bullet.stack = WorldGen.genRand.Next(50, 100);

                                        Main.chest[chest].item[index] = bullet;
                                        index++;
                                    }

                                    if (WorldGen.genRand.NextBool())
                                    {
                                        Item item2 = new Item();
                                        item2.SetDefaults(ModContent.ItemType<CoralArrow>());
                                        item2.stack = WorldGen.genRand.Next(30, 51);

                                        Main.chest[chest].item[index] = item2;
                                        index++;
                                    }

                                    if (WorldGen.genRand.NextBool())
                                    {
                                        Item item3 = new Item();
                                        item3.SetDefaults(ItemID.Glowstick);
                                        item3.stack = WorldGen.genRand.Next(10, 21);

                                        Main.chest[chest].item[index] = item3;
                                        index++;
                                    }

                                    if (WorldGen.genRand.NextBool())
                                    {
                                        Item item4 = new Item();
                                        item4.SetDefaults(ItemID.HealingPotion);
                                        item4.stack = WorldGen.genRand.Next(5, 11);

                                        Main.chest[chest].item[index] = item4;
                                        index++;
                                    }

                                    if (WorldGen.genRand.NextBool())
                                    {
                                        Item item5 = new Item();
                                        item5.SetDefaults(ItemID.RecallPotion);
                                        item5.stack = WorldGen.genRand.Next(3, 8);

                                        Main.chest[chest].item[index] = item5;
                                        index++;
                                    }

                                    if (WorldGen.genRand.NextBool())
                                    {
                                        Item item6 = new Item();
                                        item6.SetDefaults(ItemID.GillsPotion);
                                        item6.stack = WorldGen.genRand.Next(1, 4);

                                        Main.chest[chest].item[index] = item6;
                                        index++;
                                    }

                                    Item coin = new Item();
                                    coin.SetDefaults(ItemID.GoldCoin);
                                    coin.stack = WorldGen.genRand.Next(3, 8);

                                    Main.chest[chest].item[index] = coin;
                                    index++;
                                }
                            }
                        }
                    }

                    #endregion
                }
                catch (Exception e)
                {
                    //EEMod.progressMessage = "Unsuccessful!";
                    //EEMod.progressMessage = e.ToString();
                    //SubworldManager.PreSaveAndQuit();
                    return;
                }

                //Finishing initialization stuff
                EEMod.progressMessage = "Successful!";
                // EEMod.isSaving = false;

                Main.spawnTileX = boatPos;
                Main.spawnTileY = depth - 22;

                EEMod.progressMessage = null;
            })
        };

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

        private static void CreateNoise(Vector2 startingPoint, int sizeX, int sizeY, int xPos, int yPos, bool ensureN, int width, int height, float thresh)
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
                                    tile.TileType = (ushort)GetGemsandType(j);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void MakeCoralRoom(int xPos, int yPos, int sizeX, int sizeY, int type, bool ensureNoise = false)
        {
            Vector2 TL = new Vector2(xPos - (sizeX / 2f), yPos - (sizeY / 2f));
            Vector2 BR = new Vector2(xPos + (sizeX / 2f), yPos + (sizeY / 2f));

            sizeX *= 2;
            sizeY *= 2;

            int tile2;
            tile2 = (ushort)GetGemsandType(yPos);

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
                    new Rectangle((int)TL.X, (int)TL.Y, (int)BR.X, (int)BR.Y));

                    break;

                case 0:
                    switch (Main.rand.Next(2))
                    {
                        case 0:
                            MakeJaggedOval(sizeX, sizeY, new Vector2(TL.X, TL.Y), TileID.StoneSlab, true, 100);
                            MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos + 0, yPos + 0), tile2);
                            MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos + (-sizeX / 5 - sizeX / 6), yPos + (-sizeY / 5 - sizeY / 6)), tile2);
                            MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos + (sizeX / 5 - sizeX / 6), yPos + (-sizeY / 5 - sizeY / 6)), tile2);
                            MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos + (sizeX / 5 - sizeX / 6), yPos + (sizeY / 5 - sizeY / 6)), tile2);
                            MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos + (-sizeX / 5 - sizeX / 6), yPos + (sizeY / 5 - sizeY / 6)), tile2);
                            break;

                        case 1:
                            MakeJaggedOval(sizeX, sizeY, new Vector2(TL.X, TL.Y), TileID.StoneSlab, true);
                            MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos + 0, yPos + 0), tile2);
                            MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos + (-sizeX / 5), yPos + (-sizeY / 5)), tile2);
                            MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos + (sizeX / 5), yPos + (-sizeY / 5)), tile2);
                            MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos + (sizeX / 5), yPos + (sizeY / 5)), tile2);
                            MakeOvalFlatTop(sizeX / 3, sizeY / 3, new Vector2(xPos + (-sizeX / 5), yPos + (sizeY / 5)), tile2);
                            //CreateNoise(!ensureNoise, 100, 10, 0.45f);
                            break;

                            /*case 2:
                                MakeJaggedOval(sizeX, sizeY * 2, new Vector2(TL.X, yPos - sizeY), TileID.StoneSlab, true);
                                MakeJaggedOval((int)(sizeX * 0.8f), (int)(sizeY * 1.6f), new Vector2(xPos - sizeX * 0.4f, yPos - sizeY * 0.8f), tile2, true);
                                MakeJaggedOval(sizeX / 10, sizeY / 5, new Vector2(xPos - sizeX / 20, yPos - sizeY / 10), TileID.StoneSlab, true);
                                for (int i = 0; i < 30; i++)
                                {
                                    MakeCircle(WorldGen.genRand.Next(5, 20), new Vector2(TL.X + WorldGen.genRand.Next(sizeX), yPos - sizeY + WorldGen.genRand.Next(sizeY * 2)), TileID.StoneSlab, true);
                                }
                                break;*/
                    }

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
                    new Rectangle((int)TL.X, (int)TL.Y, (int)BR.X, (int)BR.Y));

                    break;

                case (int)MinibiomeID.KelpForest: //A normally shaped room cut out with noise
                    KelpForest kelpForest = new KelpForest
                    {
                        Position = TL.ToPoint(),
                        Size = new Point(sizeX, sizeY),
                        EnsureNoise = ensureNoise
                    };
                    kelpForest.StructureStep();

                    Minibiomes.Add(kelpForest);
                    break;


                case (int)MinibiomeID.GlowshroomGrotto: //One medium-sized open room completely covered in bulbous blocks
                    GlowshroomGrotto GlowshroomGrotto = new GlowshroomGrotto
                    {
                        Position = TL.ToPoint(),
                        Size = new Point(sizeX, sizeY),
                        EnsureNoise = ensureNoise
                    };
                    GlowshroomGrotto.StructureStep();

                    Minibiomes.Add(GlowshroomGrotto);
                    break;

                case (int)MinibiomeID.ThermalVents: //A wide-open room with floating platforms that hold abandoned ashen houses with huge chasms in between
                    ThermalVents ThermalVents = new ThermalVents
                    {
                        Position = TL.ToPoint(),
                        Size = new Point(sizeX, sizeY),
                        EnsureNoise = ensureNoise
                    };
                    ThermalVents.StructureStep();

                    Minibiomes.Add(ThermalVents);
                    break;

                case (int)MinibiomeID.AquamarineCaverns: //Massive caves made with noise surrounding a central large room(where the spire is, if there's a spire)
                    AquamarineCaverns AquamarineCaverns = new AquamarineCaverns
                    {
                        Position = TL.ToPoint(),
                        Size = new Point(sizeX, sizeY),
                        EnsureNoise = ensureNoise
                    };
                    AquamarineCaverns.StructureStep();

                    Minibiomes.Add(AquamarineCaverns);
                    break;
            }
        }

        public static void MakeCrystal(int xPos, int yPos, int length, int width, int vertDir, int horDir, int type)
        {
            for (int a = 0; a < length; a++)
            {
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (!Framing.GetTileSafely(i, j).HasTile)
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
            else if (height < Main.maxTilesY * 0.7f)
                return ModContent.TileType<GemsandTile>();
            else if (height > Main.maxTilesY * 0.7f)
                return ModContent.TileType<DarkGemsandTile>();
            else
                return 0;
        }

        public static void CreateNoise(bool ensureN, Point position, Point size, int width, int height, float thresh)
        {
            perlinNoise = new PerlinNoiseFunction(2000, 2000, width, height, thresh);
            Point Center = new Point(position.X + size.X / 2, position.Y + size.Y / 2);
            int[,] perlinNoiseFunction = perlinNoise.perlinBinary;
            if (ensureN)
            {
                for (int i = position.X - width; i < position.X + size.X + width; i++)
                {
                    for (int j = position.Y - height; j < position.Y + size.Y + height; j++)
                    {
                        if (i > 0 && i < Main.maxTilesX && j > 0 && j < Main.maxTilesY)
                        {
                            if (i - (int)position.X < 1000 && j - (int)position.Y < 1000)
                            {
                                if (perlinNoiseFunction[i - position.X + width, j - position.Y + width] == 1 && OvalCheck(Center.X, Center.Y, i, j, size.X, size.Y) && WorldGen.InWorld(i, j))
                                {
                                    Tile tile = Framing.GetTileSafely(i, j);
                                    tile.TileType = (ushort)GetGemsandType(j);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void MakeNoiseOval(int width, int height, Vector2 startingPoint, int type, bool forced = false, int chance = 1)
        {
            perlinNoise = new PerlinNoiseFunction(2000, 2000, 50, 50, 0.5f, WorldGen.genRand);
            float[,] pFunction = perlinNoise.perlin2;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Point Center = new Point((int)startingPoint.X + width / 2, (int)startingPoint.Y + height / 2);
                    int W = (int)(width * .5f + pFunction[i, j] * width * .5f);
                    int H = (int)(height * .5f + pFunction[i, j] * height * .5f);
                    if (OvalCheck(Center.X, Center.Y, i + (int)startingPoint.X, j + (int)startingPoint.Y, W, H) && Main.rand.Next(chance) <= 1)
                    {
                        WorldGen.TileRunner(i + (int)startingPoint.X, j + (int)startingPoint.Y, WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(5, 10), type, true, 0f, 0f, true, true);
                    }
                }
            }
        }

        public delegate bool NoiseConditions(Vector2 point);

        public static void NoiseGen(Vector2 topLeft, Vector2 size, Vector2 dimensions, float thresh, ushort type, NoiseConditions noiseFilter = null)
        {
            perlinNoise = new PerlinNoiseFunction((int)size.X, (int)size.Y, (int)dimensions.X, (int)dimensions.Y, thresh, WorldGen.genRand);
            int[,] perlinNoiseFunction = perlinNoise.perlinBinary;
            for (int i = (int)topLeft.X; i < (int)topLeft.X + (int)size.X; i++)
            {
                for (int j = (int)topLeft.Y; j < (int)topLeft.Y + (int)size.Y; j++)
                {
                    //Tile tile = Framing.GetTileSafely(i, j);
                    if (perlinNoiseFunction[i - (int)topLeft.X, j - (int)topLeft.Y] == 1)
                    {
                        WorldGen.PlaceTile(i, j, type);
                    }
                }
            }
        }

        public static void NoiseGenWave(Vector2 topLeft, Vector2 size, Vector2 dimensions, ushort type, float thresh, NoiseConditions noiseFilter = null)
        {
            PerlinNoiseFunction perlinNoise = new PerlinNoiseFunction((int)size.X, (int)size.Y, (int)dimensions.X, (int)dimensions.Y, thresh, WorldGen.genRand);
            int[,] perlinNoiseFunction = perlinNoise.perlinBinary;
            float[] disp = PerlinArrayNoZero((int)size.X, size.Y * 0.5f, new Vector2(50, 100));
            for (int i = (int)topLeft.X; i < (int)topLeft.X + (int)size.X; i++)
            {
                for (int j = (int)topLeft.Y + (int)disp[i - (int)topLeft.X]; j < (int)topLeft.Y + (int)size.Y; j++)
                {
                    //Tile tile = Framing.GetTileSafely(i, j);
                    if (perlinNoiseFunction[i - (int)topLeft.X, j - (int)topLeft.Y] == 1)
                    {
                        WorldGen.PlaceTile(i, j, type);
                        WorldGen.PlaceTile(i, j, (ushort)GetGemsandType(j));
                    }
                }
            }
        }
    }
}