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

        public override string Name => "Suk-Mah Outpost";

        internal override void WorldGeneration(int seed, GenerationProgress customProgressObject = null)
        {
            var rand = WorldGen.genRand;

            EEMod.progressMessage = "Generating Goblin Fort";

            Main.worldSurface = 400;



            //Base island generation

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

            FillRegion(500, 75, new Vector2(150, 275), TileID.Dirt);

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



            //Structure generation

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


            //Foliage generation

            for (int i = 150; i < 650; i++)
            {
                for (int j = 100; j < 280; j++)
                {
                    if(Framing.GetTileSafely(i, j).type == TileID.Dirt &&
                        (!Framing.GetTileSafely(i - 1, j - 1).IsActive || !Framing.GetTileSafely(i - 1, j).IsActive || !Framing.GetTileSafely(i - 1, j + 1).IsActive ||
                         !Framing.GetTileSafely(i, j - 1).IsActive || !Framing.GetTileSafely(i, j + 1).IsActive ||
                         !Framing.GetTileSafely(i + 1, j - 1).IsActive || !Framing.GetTileSafely(i + 1, j).IsActive || !Framing.GetTileSafely(i + 1, j + 1).IsActive))
                    {
                        Framing.GetTileSafely(i, j).type = TileID.Grass;
                    }
                }
            }

            for (int i = 0; i < 800; i++)
            {
                for (int j = 250; j < 400; j++)
                {
                    Tile tile = Framing.GetTileSafely(i, j);

                    //if (tile.IsActive && tile.type == TileID.Sand &&
                    //    !Framing.GetTileSafely(i, j - 1).IsActive && Framing.GetTileSafely(i, j - 1).LiquidAmount == 0 && WorldGen.genRand.NextBool(2))
                    //{
                        //WorldGen.GrowPalmTree(i, j - 1);
                    //    break;
                    //}

                    if (tile.IsActive && tile.type == TileID.Sand &&
                        !Framing.GetTileSafely(i, j - 1).IsActive && WorldGen.genRand.NextBool(3))
                    {
                        switch (Main.rand.Next(2))
                        {
                            case 0:
                                WorldGen.PlaceTile(i, j - 1, TileID.BeachPiles);
                                break;
                            case 1:
                                if(Framing.GetTileSafely(i, j - 1).LiquidAmount > 0) WorldGen.PlaceTile(i, j - 1, TileID.Coral);
                                break;
                        }
                        break;
                    }

                    if (tile.IsActive && tile.type == TileID.Sand &&
                        !Framing.GetTileSafely(i, j - 1).IsActive && Framing.GetTileSafely(i, j - 1).LiquidAmount > 0 && WorldGen.genRand.NextBool(2))
                    {
                        Main.tile[i, j].Slope = 0;

                        switch (WorldGen.genRand.Next(3))
                        {
                            case 0:
                                int Rand = WorldGen.genRand.Next(7, 20);

                                for (int l = j - 1; l >= j - Rand; l--)
                                {
                                    if (Main.tile[i, l].LiquidAmount < 60) break;

                                    Main.tile[i, l].type = (ushort)ModContent.TileType<SeagrassTile>();
                                    Main.tile[i, l].IsActive = true;
                                }
                                break;
                            case 1:
                                int rand2 = WorldGen.genRand.Next(4, 13);

                                for (int l = j - 1; l >= j - rand2; l--)
                                {
                                    if (Main.tile[i, l].LiquidAmount < 60) break;

                                    Main.tile[i, l].type = TileID.Seaweed;
                                    Main.tile[i, l].IsActive = true;

                                    if (l == j - rand2)
                                    {
                                        Main.tile[i, l].frameX = (short)(WorldGen.genRand.Next(8, 13) * 18);
                                    }
                                    else
                                    {
                                        Main.tile[i, l].frameX = (short)(WorldGen.genRand.Next(1, 8) * 18);
                                    }
                                }
                                break;
                            case 2:
                                int rand3 = WorldGen.genRand.Next(4, 8);

                                for (int l = j - 1; l >= j - rand3; l--)
                                {
                                    if (Main.tile[i, l].LiquidAmount < 60) break;

                                    Main.tile[i, l].type = TileID.Bamboo;
                                    Main.tile[i, l].IsActive = true;

                                    if (l == j - 1)
                                    {
                                        Main.tile[i, l].frameX = (short)(WorldGen.genRand.Next(1, 5) * 18);
                                    }
                                    else if (l == j - rand3)
                                    {
                                        Main.tile[i, l].frameX = (short)(WorldGen.genRand.Next(15, 20) * 18);
                                    }
                                    else
                                    {
                                        Main.tile[i, l].frameX = (short)(WorldGen.genRand.Next(5, 15) * 18);
                                    }
                                }
                                break;
                        }
                        break;
                    }
                }
            }

            EEMod.progressMessage = null;
        }

        internal override void PlayerUpdate(Player player)
        {
           
        }
    }
}