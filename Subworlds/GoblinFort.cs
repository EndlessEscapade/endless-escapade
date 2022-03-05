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
using static EEMod.EEWorld.EEWorld;
using EEMod.Tiles.Foliage.Coral.HangingCoral;
using EEMod.Tiles.Foliage.Coral.WallCoral;
using EEMod.Tiles.Foliage.Coral;
using EEMod.Tiles.Foliage;
using System;
using EEMod.Systems.Noise;
using System.Collections.Generic;
using EEMod.Autoloading;
using Terraria.WorldBuilding;
using System.Diagnostics;
using EEMod.NPCs.Goblins.Shaman;
using EEMod.NPCs.Goblins.Berserker;
using EEMod.NPCs.Goblins.Watchman;
using EEMod.Systems.Subworlds;
using EEMod.Systems;

namespace EEMod.Subworlds
{
    public class GoblinFort : Subworld
    {
        public override Point Dimensions => new Point(1600, 800);

        public override Point SpawnTile => new Point(1600 + 12, 245 + 25);

        public override int surfaceLevel => 500;

        public override string Name => "Goblin Outpost";

        internal override void WorldGeneration(int seed, GenerationProgress customProgressObject = null)
        {
            EEMod.progressMessage = "Generating Goblin Fort";

            Main.worldSurface = 400;


            //Base island generation
            #region Island terrain generation
            FillRegion(1600, 475, new Vector2(0, 325), TileID.Stone);

            for(int i = 0; i < 150; i++)
            {
                for(int j = 0; j < 50; j++)
                {
                    if(j < (((i / 21f) * (i / 21f)) + 10f))
                    {
                        WorldGen.PlaceTile((i) + 0, (50 - j) + 274, TileID.Sand);
                    }
                }
            }

            //FillRegion(500, 75, new Vector2(150, 275), TileID.Dirt);

            int elevation = 275;
            int thresh1 = WorldGen.genRand.Next(300, 350);
            int thresh2 = WorldGen.genRand.Next(650, 700);
            int slope = WorldGen.genRand.Next(15, 26);
            bool slopingFast = false;
            int initSlope = 0;

            for (int i = 150; i <= 800; i++)
            {
                //if (elevation < 200) break;

                if ((i == thresh1 || i == thresh2) && !slopingFast)
                {
                    slopingFast = true;
                    initSlope = slope;
                }

                if (i % slope == 0 && !slopingFast)
                {
                    elevation--;
                    if(WorldGen.genRand.NextBool(4)) slope++;
                }
                if (slopingFast) //
                { 
                    elevation -= (int)Math.Abs((initSlope - slope) / 3f);
                    slope -= 2;
                    
                    if(slope <= 0)
                    {
                        slopingFast = false;
                        slope = (i < thresh2 ? WorldGen.genRand.Next(9, 14) : WorldGen.genRand.Next(16, 22));
                    }
                }

                if (elevation > 275) elevation = 275;

                int rockLayer = WorldGen.genRand.Next(5, 9);
                for(int j = 275 + 75; j > elevation; j--)
                {
                    if (WorldGen.InWorld(i, j))
                    {
                        if(j - elevation < rockLayer)
                        {
                            if (i <= 160) WorldGen.PlaceTile(i, j, TileID.Sand);
                            else WorldGen.PlaceTile(i, j, TileID.Dirt);
                        }
                        else
                            WorldGen.PlaceTile(i, j, TileID.Stone);
                    }
                }
            }

            int peakElevation = elevation;

            thresh1 = WorldGen.genRand.Next(300, 350);
            thresh2 = WorldGen.genRand.Next(650, 700);
            slope = WorldGen.genRand.Next(16, 22);
            slopingFast = false;

            for (int i = 801; i <= 1600 - 150; i++)
            {
                //if (elevation < 200) break;

                Debug.WriteLine(i);

                if ((i == (1600 - thresh1) || i == (1600 - thresh2)) && !slopingFast)
                {
                    slopingFast = true;
                    slope = WorldGen.genRand.Next(12, 15);
                }

                if (i % slope == 0 && !slopingFast)
                {
                    elevation++;
                    if (WorldGen.genRand.NextBool(4)) slope++;
                }
                if (slopingFast)
                {
                    elevation += (int)Math.Abs((slope) / 2.5f);
                    slope -= 2;

                    if (slope <= 0)
                    {
                        slopingFast = false;
                        slope = (i < (1600 - thresh2) ? WorldGen.genRand.Next(9, 14) : WorldGen.genRand.Next(15, 26));
                    }
                }

                if (elevation > 275) elevation = 275;

                int rockLayer = WorldGen.genRand.Next(5, 9);
                for (int j = 275 + 75; j > elevation; j--)
                {
                    if (WorldGen.InWorld(i, j))
                    {
                        if (j - elevation < rockLayer)
                        {
                            if(i >= 1600 - 160) WorldGen.PlaceTile(i, j, TileID.Sand);
                            else WorldGen.PlaceTile(i, j, TileID.Dirt);
                        }
                        else
                            WorldGen.PlaceTile(i, j, TileID.Stone);
                    }
                }
            }

            for(int i = 150; i < 1600 - 150; i++)
            {
                for(int j = peakElevation - 1; j < peakElevation + 3; j++)
                {
                    Framing.GetTileSafely(i, j).HasTile = false;
                }
            }

            PerlinNoiseFunction perlinNoise = new PerlinNoiseFunction(2000, 2000, 10, 10, 0.2f);
            int[,] perlinNoiseFunction = perlinNoise.perlinBinary;

            for (int i = 150; i < 1600 - 150; i++)
            {
                for (int j = 100; j < 300; j++)
                {
                    if (i > 0 && i < Main.maxTilesX && j > 0 && j < Main.maxTilesY)
                    {
                        if (i - 150 < 1000 && j - 100 < 1000)
                        {
                            if (perlinNoiseFunction[i - 150 + 500, j - 100 + 200] == 1 && WorldGen.InWorld(i, j) && Framing.GetTileSafely(i, j).TileType != TileID.Sand)
                            {
                                Framing.GetTileSafely(i, j).TileType = TileID.Stone;
                            }
                        }
                    }
                }
            }

            //Base beach generation

            for (int i = 0; i < 150; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    if (j < (((i / 21f) * (i / 21f)) + 10f))
                    {
                        WorldGen.PlaceTile((1599 - i) + 0, (50 - j) + 274, TileID.Sand);
                    }
                }
            }

            FillRegionWithWater(1600, 50, new Vector2(0, 276));
            #endregion



            //Foliage generation
            #region Foliage generation
            for (int i = 150; i < 1600 - 150; i++)
            {
                for (int j = 100; j < 280; j++)
                {
                    if (Framing.GetTileSafely(i, j).TileType == TileID.Dirt &&
                        (!Framing.GetTileSafely(i - 1, j - 1).HasTile || !Framing.GetTileSafely(i - 1, j).HasTile || !Framing.GetTileSafely(i - 1, j + 1).HasTile ||
                         !Framing.GetTileSafely(i, j - 1).HasTile || !Framing.GetTileSafely(i, j + 1).HasTile ||
                         !Framing.GetTileSafely(i + 1, j - 1).HasTile || !Framing.GetTileSafely(i + 1, j).HasTile || !Framing.GetTileSafely(i + 1, j + 1).HasTile))
                    {
                        Framing.GetTileSafely(i, j).TileType = TileID.Grass;
                        //Framing.GetTileSafely(i, j).Color = PaintID.LimePaint;
                        Tile.SmoothSlope(i, j);
                    }
                }
            }

            for (int i = 150; i < 1600 - 150; i++)
            {
                for (int j = 100; j < 280; j++)
                {
                    if (!Framing.GetTileSafely(i, j).HasTile && Framing.GetTileSafely(i, j + 1).HasTile && Framing.GetTileSafely(i, j + 1).TileType == TileID.Grass && WorldGen.genRand.NextBool())
                    {
                        WorldGen.PlaceTile(i, j, TileID.Plants, style: WorldGen.genRand.Next(42));
                    }
                }
            }

            for (int i = 0; i < 1600; i++)
            {
                for (int j = 250; j < 400; j++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);

                    //if (tile.HasTile && tile.type == TileID.Sand &&
                    //    !Framing.GetTileSafely(i, j - 1).HasTile && Framing.GetTileSafely(i, j - 1).LiquidAmount == 0 && WorldGen.genRand.NextBool(2))
                    //{
                    //WorldGen.GrowPalmTree(i, j - 1);
                    //    break;
                    //}

                    if (tile.HasTile && tile.TileType == TileID.Sand &&
                        !Framing.GetTileSafely(i, j - 1).HasTile && WorldGen.genRand.NextBool(3))
                    {
                        switch (WorldGen.genRand.Next(2))
                        {
                            case 0:
                                WorldGen.PlaceTile(i, j - 1, TileID.BeachPiles);
                                break;
                            case 1:
                                if (Framing.GetTileSafely(i, j - 1).LiquidAmount > 0) WorldGen.PlaceTile(i, j - 1, TileID.Coral);
                                break;
                        }
                        break;
                    }

                    if (tile.HasTile && tile.TileType == TileID.Sand &&
                        !Framing.GetTileSafely(i, j - 1).HasTile && Framing.GetTileSafely(i, j - 1).LiquidAmount > 0 && WorldGen.genRand.NextBool(2))
                    {
                        Framing.GetTileSafely(i, j).Slope = 0;

                        switch (WorldGen.genRand.Next(3))
                        {
                            case 0:
                                int Rand = WorldGen.genRand.Next(7, 20);

                                for (int l = j - 1; l >= j - Rand; l--)
                                {
                                    if (Framing.GetTileSafely(i, l).LiquidAmount < 60) break;

                                    Framing.GetTileSafely(i, l).TileType = (ushort)ModContent.TileType<SeagrassTile>();
                                    Framing.GetTileSafely(i, l).HasTile = true;
                                }
                                break;
                            case 1:
                                int rand2 = WorldGen.genRand.Next(4, 13);

                                for (int l = j - 1; l >= j - rand2; l--)
                                {
                                    if (Main.tile[i, l].LiquidAmount < 60) break;

                                    Main.tile[i, l].TileType = TileID.Seaweed;
                                    Framing.GetTileSafely(i, l).HasTile = true;

                                    if (l == j - rand2)
                                    {
                                        Main.tile[i, l].TileFrameX = (short)(WorldGen.genRand.Next(8, 13) * 18);
                                    }
                                    else
                                    {
                                        Main.tile[i, l].TileFrameX = (short)(WorldGen.genRand.Next(1, 8) * 18);
                                    }
                                }
                                break;
                            case 2:
                                int rand3 = WorldGen.genRand.Next(4, 8);

                                for (int l = j - 1; l >= j - rand3; l--)
                                {
                                    if (Main.tile[i, l].LiquidAmount < 60) break;

                                    Main.tile[i, l].TileType = TileID.Bamboo;
                                    Framing.GetTileSafely(i, l).HasTile = true;

                                    if (l == j - 1)
                                    {
                                        Main.tile[i, l].TileFrameX = (short)(WorldGen.genRand.Next(1, 5) * 18);
                                    }
                                    else if (l == j - rand3)
                                    {
                                        Main.tile[i, l].TileFrameX = (short)(WorldGen.genRand.Next(15, 20) * 18);
                                    }
                                    else
                                    {
                                        Main.tile[i, l].TileFrameX = (short)(WorldGen.genRand.Next(5, 15) * 18);
                                    }
                                }
                                break;
                        }
                        break;
                    }
                }
            }
            #endregion



            //Structure generation
            #region Structure generation

            //Placing player boat
            BuildBoat(1600 - 90, 245);

            for (int i = 1600 - 90; i < 1600 - 90 + 45; i++)
            {
                for (int j = 250; j < 250 + 40; j++)
                {
                    if (Main.tile[i, j].WallType != WallID.None)
                    {
                        Main.tile[i, j].LiquidAmount = 0;
                    }
                }
            }



            //Placing rafts
            for(int i = 50; i < 400; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    if (Framing.GetTileSafely(i, j).HasTile) break;

                    if (!Framing.GetTileSafely(i, j).HasTile && Framing.GetTileSafely(i, j).LiquidAmount >= 64)
                    {
                        if (Main.rand.Next(80) == 0)
                        {
                            switch (WorldGen.genRand.NextBool())
                            {
                                case true:
                                    Structure.DeserializeFromBytes(ModContent.GetInstance<EEMod>().GetFileBytes("EEWorld/Structures/GoblinRaft1.lcs")).PlaceAt(i, TileCheckWater(i) - 3, false, false);
                                    break;
                                case false:
                                    Structure.DeserializeFromBytes(ModContent.GetInstance<EEMod>().GetFileBytes("EEWorld/Structures/GoblinRaft2.lcs")).PlaceAt(i, TileCheckWater(i) - 3, false, false);
                                    break;
                            }

                            i += 30;
                            break;
                        }
                    }
                }
            }



            //Determining what kind of fort

            switch(Main.rand.Next(3))
            {
                case 0: //Pitfall fort(high rise fort)
                    /*for(int i = 150; i < 650; i++)
                    {
                        if(Math.Abs())
                        {

                        }
                    }*/
                    break;
                case 1: //Heavy defense fort(watchtower perimeter with soft interior)
                    int watchtower1 = thresh1 + 15;
                    int watchtower2 = thresh2 + 15;
                    break;
                case 2: //Stepped fort(watchtowers on the interior of every step with camps in front of it

                    break;
            }

            #endregion



            EEMod.progressMessage = null;

            spawnedEnemies = false;

            Main.spawnTileX = 1600 - 90 + 12;
            Main.spawnTileY = 245 + 25;
        }

        bool spawnedEnemies = false;
        internal override void PlayerUpdate(Player player)
        {
            if(!spawnedEnemies && (Main.netMode == NetmodeID.SinglePlayer || Main.netMode == NetmodeID.Server))
            {
                player.position = new Vector2(SpawnTile.X * 16, SpawnTile.Y * 16);

                NPC.NewNPC(new Terraria.DataStructures.EntitySource_WorldGen(), 400 * 16, TileCheck(400, TileID.Grass) * 16 - 40, ModContent.NPCType<GoblinBerserker>());

                NPC.NewNPC(new Terraria.DataStructures.EntitySource_WorldGen(), 410 * 16, TileCheck(410, TileID.Grass) * 16 - 40, ModContent.NPCType<GoblinWatchman>());

                spawnedEnemies = true;
            }
        }
    }
}