using EEMod.Tiles;
using EEMod.Tiles.Foliage;
using EEMod.Tiles.Foliage.KelpForest;
using EEMod.Tiles.Walls;
using EEMod.VerletIntegration;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static EEMod.EEWorld.EEWorld;
using EEMod.Tiles.EmptyTileArrays;
using System;
using EEMod.Systems.Subworlds.EESubworlds;

namespace EEMod.EEWorld
{
    public class AquamarineCaverns : CoralReefMinibiome
    {
        public override void FoliageStep()
        {

        }

        public delegate void InOvalEvent(int i, int j);

        public void BoundClause(InOvalEvent obj)
        {
            Point TL = Bounds.TopLeft().ToPoint();
            Point BR = Bounds.BottomRight().ToPoint();
            for (int i = TL.X; i < BR.X; i++)
            {
                for (int j = TL.Y; j < BR.Y; j++)
                {
                    if (OvalCheck(Center.X, Center.Y, i, j, Size.X / 2, Size.Y / 2))
                        obj.Invoke(i, j);
                }
            }
        }

        public override void StructureStep()
        {
            Point TL = Bounds.TopLeft().ToPoint();
            Point BR = Bounds.BottomRight().ToPoint();

            //Worldgen
            MakeNoiseOval(Size.X, Size.Y, new Vector2(TL.X, TL.Y), TileID.StoneSlab, true, 50);
            CreateNoise(!EnsureNoise, Position, Size, 15, 15, 0.5f);
            CreateNoise(!EnsureNoise, Position, Size, 15, 15, 0.5f);

            BoundClause((int i, int j) =>
            {
                /*if (WorldGen.genRand.Next(100) == 0)
                {
                    WorldGen.TileRunner(i, j, Main.rand.Next(10, 20), Main.rand.Next(10, 20), ModContent.TileType<GemsandstoneTile>(), false, 0, 0, false, true);
                }*/
            });

            RemoveStoneSlabs();

            //Placing spire
            if (CoralReefs.SpirePosition == Vector2.Zero)
            {
                CoralReefs.SpirePosition = new Vector2(TL.X + BR.X, TL.Y + BR.Y) / 2f;

                Vector2 pos1 = new Vector2(CoralReefs.SpirePosition.X + 10, CoralReefs.SpirePosition.Y - 150 / 2);
                Vector2 pos2 = new Vector2(CoralReefs.SpirePosition.X + 10, CoralReefs.SpirePosition.Y + 150 / 2);

                int tile2 = 0;
                tile2 = GetGemsandType((int)pos1.Y);

                MakeExpandingChasm(pos1, pos2, tile2, 100, -2, true, new Vector2(20, 30), .5f);
                MakeExpandingChasm(pos2, pos1, tile2, 100, -2, true, new Vector2(20, 30), .5f);

                ClearRegion(46, 26, new Vector2(CoralReefs.SpirePosition.X + 10 - 24, CoralReefs.SpirePosition.Y - 26));

                MakeWavyChasm3(new Vector2(CoralReefs.SpirePosition.X - 5, CoralReefs.SpirePosition.Y - 26), new Vector2(CoralReefs.SpirePosition.X + 25, CoralReefs.SpirePosition.Y - 26), tile2, 20, -2, true, new Vector2(1, 5));
                MakeWavyChasm3(new Vector2(CoralReefs.SpirePosition.X - 5, CoralReefs.SpirePosition.Y), new Vector2(CoralReefs.SpirePosition.X + 25, CoralReefs.SpirePosition.Y), tile2, 20, -2, true, new Vector2(1, 5));
            }

            RemoveStoneSlabs();

            //Spawning crystal ziplines
            BoundClause((int i, int j) =>
            {
                if ((TileCheck2(i, j) == 3 || TileCheck2(i, j) == 4) && Main.rand.NextBool(10))
                {
                    if (CoralReefs.AquamarineZiplineLocations.Count == 0)
                    {
                        CoralReefs.AquamarineZiplineLocations.Add(new Vector2(i, j));
                    }
                    else
                    {
                        Vector2 lastPos = CoralReefs.AquamarineZiplineLocations[CoralReefs.AquamarineZiplineLocations.Count - 1];
                        if ((Vector2.DistanceSquared(lastPos, new Vector2(i, j)) > 10 * 10 && Vector2.DistanceSquared(lastPos, new Vector2(i, j)) < 210 * 210) || Vector2.DistanceSquared(lastPos, new Vector2(i, j)) > 200 * 200)
                        {
                            CoralReefs.AquamarineZiplineLocations.Add(new Vector2(i, j));
                        }
                    }
                }
            });

            //Spawning crystal bamboo (TODO: replace with bamboo geodes)
            BoundClause((int i, int j) =>
            {
                if ((TileCheck2(i, j) == 2) && Main.rand.NextBool(3))
                {
                    CoralReefs.ThinCrystalBambooLocations.Add(new Vector2(i, j));

                    Vector2 lastPos = CoralReefs.ThinCrystalBambooLocations[CoralReefs.ThinCrystalBambooLocations.Count - 1];

                    int length = Main.rand.Next(1, 6);
                    Vector2 rotVec = new Vector2(length, 0).RotatedBy(Main.rand.NextFloat((MathHelper.PiOver2 * 3) - 0.2f, (MathHelper.PiOver2 * 3) + 0.2f));
                    CoralReefs.ThinCrystalBambooLocations.Add(lastPos + rotVec);
                }
            });

            //Placing ETAs
            BoundClause((int i, int j) =>
            {
                if (TileCheck2(i, j) == 1)
                {
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            ETAHelpers.PlaceCrystal(ETAHelpers.ETAAnchor.Top, new Vector2(i, j), EmptyTileArrays.LuminantCoralCrystalHang3, "Tiles/EmptyTileArrays/AquamarineCrystalTop2x2", "Tiles/EmptyTileArrays/AquamarineCrystalTop2x2Shine");
                            break;
                        case 1:
                            ETAHelpers.PlaceCrystal(ETAHelpers.ETAAnchor.Top, new Vector2(i, j), EmptyTileArrays.LuminantCoralCrystalHang2, "Tiles/EmptyTileArrays/AquamarineCrystalTop2x4", "Tiles/EmptyTileArrays/AquamarineCrystalTop2x4Shine");
                            break;
                        case 2:
                            ETAHelpers.PlaceCrystal(ETAHelpers.ETAAnchor.Top, new Vector2(i, j), EmptyTileArrays.LuminantCoralCrystalHang1, "Tiles/EmptyTileArrays/AquamarineCrystalTop3x4", "Tiles/EmptyTileArrays/AquamarineCrystalTop3x4Shine");
                            break;
                    }
                }
                if (TileCheck2(i, j) == 2)
                {
                    switch (Main.rand.Next(7))
                    {
                        case 0:
                            ETAHelpers.PlaceCrystal(ETAHelpers.ETAAnchor.Bottom, new Vector2(i, j), EmptyTileArrays.LuminantCoralCrystalGround7, "Tiles/EmptyTileArrays/AquamarineCrystalGround1x1", "Tiles/EmptyTileArrays/AquamarineCrystalGround1x1Shine");
                            break;
                        case 1:
                            ETAHelpers.PlaceCrystal(ETAHelpers.ETAAnchor.Bottom, new Vector2(i, j), EmptyTileArrays.LuminantCoralCrystalGround3, "Tiles/EmptyTileArrays/AquamarineCrystalGround2x2", "Tiles/EmptyTileArrays/AquamarineCrystalGround2x2Shine");
                            break;
                        case 2:
                            ETAHelpers.PlaceCrystal(ETAHelpers.ETAAnchor.Bottom, new Vector2(i, j), EmptyTileArrays.LuminantCoralCrystalGround2, "Tiles/EmptyTileArrays/AquamarineCrystalGround2x3", "Tiles/EmptyTileArrays/AquamarineCrystalGround2x3Shine");
                            break;
                        case 3:
                            ETAHelpers.PlaceCrystal(ETAHelpers.ETAAnchor.Bottom, new Vector2(i, j), EmptyTileArrays.LuminantCoralCrystalGround4, "Tiles/EmptyTileArrays/AquamarineCrystalGround4x3", "Tiles/EmptyTileArrays/AquamarineCrystalGround4x3Shine");
                            break;
                        case 4:
                            ETAHelpers.PlaceCrystal(ETAHelpers.ETAAnchor.Bottom, new Vector2(i, j), EmptyTileArrays.LuminantCoralCrystalGround5, "Tiles/EmptyTileArrays/AquamarineCrystalGround4x5", "Tiles/EmptyTileArrays/AquamarineCrystalGround4x5Shine");
                            break;
                        case 5:
                            ETAHelpers.PlaceCrystal(ETAHelpers.ETAAnchor.Bottom, new Vector2(i, j), EmptyTileArrays.LuminantCoralCrystalGround6, "Tiles/EmptyTileArrays/AquamarineCrystalGround4x7", "Tiles/EmptyTileArrays/AquamarineCrystalGround4x7Shine");
                            break;
                        case 6:
                            ETAHelpers.PlaceCrystal(ETAHelpers.ETAAnchor.Bottom, new Vector2(i, j), EmptyTileArrays.LuminantCoralCrystalGround1, "Tiles/EmptyTileArrays/AquamarineCrystalGround5x7", "Tiles/EmptyTileArrays/AquamarineCrystalGround5x7Shine");
                            break;
                    }
                }
                if (TileCheck2(i, j) == 3)
                {
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            ETAHelpers.PlaceCrystal(ETAHelpers.ETAAnchor.Left, new Vector2(i, j), EmptyTileArrays.LuminantCoralSideRight2, "Tiles/EmptyTileArrays/AquamarineCrystalLeft2x1", "Tiles/EmptyTileArrays/AquamarineCrystalLeft2x1Shine");
                            break;
                        case 1:
                            ETAHelpers.PlaceCrystal(ETAHelpers.ETAAnchor.Left, new Vector2(i, j), EmptyTileArrays.LuminantCoralSideRight1, "Tiles/EmptyTileArrays/AquamarineCrystalLeft3x1", "Tiles/EmptyTileArrays/AquamarineCrystalLeft3x1Shine");
                            break;
                        case 2:
                            ETAHelpers.PlaceCrystal(ETAHelpers.ETAAnchor.Left, new Vector2(i, j), EmptyTileArrays.LuminantCoralSideRight3, "Tiles/EmptyTileArrays/AquamarineCrystalLeft5x3", "Tiles/EmptyTileArrays/AquamarineCrystalLeft5x3Shine");
                            break;
                    }
                }
                if (TileCheck2(i, j) == 4)
                {
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            ETAHelpers.PlaceCrystal(ETAHelpers.ETAAnchor.Right, new Vector2(i, j), EmptyTileArrays.LuminantCoralSideLeft2, "Tiles/EmptyTileArrays/AquamarineCrystalRight2x1", "Tiles/EmptyTileArrays/AquamarineCrystalRight2x1Shine");
                            break;
                        case 1:
                            ETAHelpers.PlaceCrystal(ETAHelpers.ETAAnchor.Right, new Vector2(i, j), EmptyTileArrays.LuminantCoralSideLeft1, "Tiles/EmptyTileArrays/AquamarineCrystalRight3x1", "Tiles/EmptyTileArrays/AquamarineCrystalRight3x1Shine");
                            break;
                        case 2:
                            ETAHelpers.PlaceCrystal(ETAHelpers.ETAAnchor.Right, new Vector2(i, j), EmptyTileArrays.LuminantCoralSideLeft3, "Tiles/EmptyTileArrays/AquamarineCrystalRight5x3", "Tiles/EmptyTileArrays/AquamarineCrystalRight5x3Shine");
                            break;
                    }
                }
            });
        }
    }
}