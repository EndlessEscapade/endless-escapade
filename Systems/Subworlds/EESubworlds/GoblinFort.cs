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

namespace EEMod.Systems.Subworlds.EESubworlds
{
    public class GoblinFort : Subworld
    {
        public override Point Dimensions => new Point(800, 800);

        public override Point SpawnTile => new Point(200, 100);

        public override int surfaceLevel => 500;

        public override string Name => "Goblin Outpost";

        internal override void WorldGeneration(int seed, GenerationProgress customProgressObject = null)
        {
            var rand = WorldGen.genRand;

            EEMod.progressMessage = "Generating Goblin Fort";

            Main.worldSurface = 400;


            //Base island generation-

            #region Island terrain generation
            FillRegion(800, 475, new Vector2(0, 325), TileID.Stone);

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
            int thresh1 = rand.Next(200, 250);
            int thresh2 = rand.Next(300, 350);
            int slope = rand.Next(8, 13);
            bool slopingFast = false;
            int initSlope = 0;

            for (int i = 150; i <= 400; i++)
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
                    if(rand.NextBool(4)) slope++;
                }
                if (slopingFast) //
                { 
                    elevation -= (int)Math.Abs((initSlope - slope) / 3f);
                    slope -= 2;
                    
                    if(slope <= 0)
                    {
                        slopingFast = false;
                        slope = (i < thresh2 ? rand.Next(9, 14) : rand.Next(16, 22));
                    }
                }

                if (elevation > 275) elevation = 275;

                int rockLayer = rand.Next(5, 9);
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

            thresh1 = rand.Next(200, 250);
            thresh2 = rand.Next(300, 350);
            slope = rand.Next(16, 22);
            slopingFast = false;

            for (int i = 401; i <= 650; i++)
            {
                //if (elevation < 200) break;

                Debug.WriteLine(i);

                if ((i == (800 - thresh1) || i == (800 - thresh2)) && !slopingFast)
                {
                    slopingFast = true;
                    slope = rand.Next(12, 15);
                }

                if (i % slope == 0 && !slopingFast)
                {
                    elevation++;
                    if (rand.NextBool(4)) slope++;
                }
                if (slopingFast)
                {
                    elevation += (int)Math.Abs((slope) / 2.5f);
                    slope -= 2;

                    if (slope <= 0)
                    {
                        slopingFast = false;
                        slope = (i < (800 - thresh2) ? rand.Next(9, 14) : rand.Next(16, 22));
                    }
                }

                if (elevation > 275) elevation = 275;

                int rockLayer = rand.Next(5, 9);
                for (int j = 275 + 75; j > elevation; j--)
                {
                    if (WorldGen.InWorld(i, j))
                    {
                        if (j - elevation < rockLayer)
                        {
                            if(i >= 640) WorldGen.PlaceTile(i, j, TileID.Sand);
                            else WorldGen.PlaceTile(i, j, TileID.Dirt);
                        }
                        else
                            WorldGen.PlaceTile(i, j, TileID.Stone);
                    }
                }
            }

            for(int i = 150; i < 650; i++)
            {
                for(int j = peakElevation - 1; j < peakElevation + 3; j++)
                {
                    Framing.GetTileSafely(i, j).IsActive = false;
                }
            }

            PerlinNoiseFunction perlinNoise = new PerlinNoiseFunction(2000, 2000, 10, 10, 0.2f);
            int[,] perlinNoiseFunction = perlinNoise.perlinBinary;

            for (int i = 150; i < 650; i++)
            {
                for (int j = 100; j < 300; j++)
                {
                    if (i > 0 && i < Main.maxTilesX && j > 0 && j < Main.maxTilesY)
                    {
                        if (i - 150 < 1000 && j - 100 < 1000)
                        {
                            if (perlinNoiseFunction[i - 150 + 500, j - 100 + 200] == 1 && WorldGen.InWorld(i, j) && Framing.GetTileSafely(i, j).type != TileID.Sand)
                            {
                                Framing.GetTileSafely(i, j).type = TileID.Stone;
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
                        WorldGen.PlaceTile((799 - i) + 0, (50 - j) + 274, TileID.Sand);
                    }
                }
            }

            FillRegionWithWater(800, 50, new Vector2(0, 276));
            #endregion



            //Foliage generation

            #region Foliage generation
            for (int i = 150; i < 650; i++)
            {
                for (int j = 100; j < 280; j++)
                {
                    if (Framing.GetTileSafely(i, j).type == TileID.Dirt &&
                        (!Framing.GetTileSafely(i - 1, j - 1).IsActive || !Framing.GetTileSafely(i - 1, j).IsActive || !Framing.GetTileSafely(i - 1, j + 1).IsActive ||
                         !Framing.GetTileSafely(i, j - 1).IsActive || !Framing.GetTileSafely(i, j + 1).IsActive ||
                         !Framing.GetTileSafely(i + 1, j - 1).IsActive || !Framing.GetTileSafely(i + 1, j).IsActive || !Framing.GetTileSafely(i + 1, j + 1).IsActive))
                    {
                        Framing.GetTileSafely(i, j).type = TileID.Grass;
                        //Framing.GetTileSafely(i, j).Color = PaintID.LimePaint;
                        Tile.SmoothSlope(i, j);
                    }
                }
            }

            for (int i = 150; i < 650; i++)
            {
                for (int j = 100; j < 280; j++)
                {
                    if (!Framing.GetTileSafely(i, j).IsActive && Framing.GetTileSafely(i, j + 1).IsActive && Framing.GetTileSafely(i, j + 1).type == TileID.Grass && rand.NextBool())
                    {
                        WorldGen.PlaceTile(i, j, TileID.Plants, style: rand.Next(42));
                    }
                }
            }

            for (int i = 0; i < 800; i++)
            {
                for (int j = 250; j < 400; j++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);

                    //if (tile.IsActive && tile.type == TileID.Sand &&
                    //    !Framing.GetTileSafely(i, j - 1).IsActive && Framing.GetTileSafely(i, j - 1).LiquidAmount == 0 && rand.NextBool(2))
                    //{
                    //WorldGen.GrowPalmTree(i, j - 1);
                    //    break;
                    //}

                    if (tile.IsActive && tile.type == TileID.Sand &&
                        !Framing.GetTileSafely(i, j - 1).IsActive && rand.NextBool(3))
                    {
                        switch (rand.Next(2))
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

                    if (tile.IsActive && tile.type == TileID.Sand &&
                        !Framing.GetTileSafely(i, j - 1).IsActive && Framing.GetTileSafely(i, j - 1).LiquidAmount > 0 && rand.NextBool(2))
                    {
                        Main.tile[i, j].Slope = 0;

                        switch (rand.Next(3))
                        {
                            case 0:
                                int Rand = rand.Next(7, 20);

                                for (int l = j - 1; l >= j - Rand; l--)
                                {
                                    if (Main.tile[i, l].LiquidAmount < 60) break;

                                    Main.tile[i, l].type = (ushort)ModContent.TileType<SeagrassTile>();
                                    Main.tile[i, l].IsActive = true;
                                }
                                break;
                            case 1:
                                int rand2 = rand.Next(4, 13);

                                for (int l = j - 1; l >= j - rand2; l--)
                                {
                                    if (Main.tile[i, l].LiquidAmount < 60) break;

                                    Main.tile[i, l].type = TileID.Seaweed;
                                    Main.tile[i, l].IsActive = true;

                                    if (l == j - rand2)
                                    {
                                        Main.tile[i, l].frameX = (short)(rand.Next(8, 13) * 18);
                                    }
                                    else
                                    {
                                        Main.tile[i, l].frameX = (short)(rand.Next(1, 8) * 18);
                                    }
                                }
                                break;
                            case 2:
                                int rand3 = rand.Next(4, 8);

                                for (int l = j - 1; l >= j - rand3; l--)
                                {
                                    if (Main.tile[i, l].LiquidAmount < 60) break;

                                    Main.tile[i, l].type = TileID.Bamboo;
                                    Main.tile[i, l].IsActive = true;

                                    if (l == j - 1)
                                    {
                                        Main.tile[i, l].frameX = (short)(rand.Next(1, 5) * 18);
                                    }
                                    else if (l == j - rand3)
                                    {
                                        Main.tile[i, l].frameX = (short)(rand.Next(15, 20) * 18);
                                    }
                                    else
                                    {
                                        Main.tile[i, l].frameX = (short)(rand.Next(5, 15) * 18);
                                    }
                                }
                                break;
                        }
                        break;
                    }
                }
            }
            #endregion

            //


            //Structure generation

            #region Structure generation
            ClearRegion(45, 36, new Vector2(710, 250));

            Structure.DeserializeFromBytes(ModContent.GetInstance<EEMod>().GetFileBytes("EEWorld/Structures/builtboat.lcs")).PlaceAt(710, 245, false, false);

            for (int i = 710; i < 710 + 45; i++)
            {
                for (int j = 250; j < 250 + 40; j++)
                {
                    if (Main.tile[i, j].wall != WallID.None)
                    {
                        Main.tile[i, j].LiquidAmount = 0;
                    }
                }
            }

            for(int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    if (!Framing.GetTileSafely(i, j).IsActive && Framing.GetTileSafely(i, j).LiquidAmount >= 64)
                    {
                        Debug.WriteLine("lalalala");

                        if (rand.NextBool(24))
                        {
                            switch (rand.NextBool())
                            {
                                case true:
                                    Structure.DeserializeFromBytes(ModContent.GetInstance<EEMod>().GetFileBytes("EEWorld/Structures/GoblinRaft1.lcs")).PlaceAt(i, TileCheckWater(i) - 3, false, false);
                                    break;
                                case false:
                                    Structure.DeserializeFromBytes(ModContent.GetInstance<EEMod>().GetFileBytes("EEWorld/Structures/GoblinRaft2.lcs")).PlaceAt(i, TileCheckWater(i) - 3, false, false);
                                    break;
                            }

                            i += 10;
                        }
                    }
                }
            }

            #endregion



            EEMod.progressMessage = null;
        }

        internal override void PlayerUpdate(Player player)
        {

        }
    }
}