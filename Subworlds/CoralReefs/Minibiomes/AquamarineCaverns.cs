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
using EEMod.ID;
using EEMod.Systems.Subworlds.EESubworlds;
using EEMod.Tiles.Foliage.Aquamarine;
using EEMod.Tiles.Ores;
using System.Collections.Generic;
using EEMod.Subworlds.CoralReefs;

namespace EEMod.EEWorld
{
    public class AquamarineCaverns : CoralReefMinibiome
    {
        public override void FoliageStep()
        {

        }

        public delegate void InOvalEvent(int i, int j);

        public override MinibiomeID id => MinibiomeID.AquamarineCaverns;

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

        public List<Vector2> geodeLocations;
        public override void StructureStep()
        {
            geodeLocations = new List<Vector2>();

            Point TL = Bounds.TopLeft().ToPoint();
            Point BR = Bounds.BottomRight().ToPoint();

            //CoralReefs.CreateNoise(!EnsureNoise, Position, Size, 15, 15, 0.5f);
            //CoralReefs.CreateNoise(!EnsureNoise, Position, Size, 15, 15, 0.5f);

            bool hasSanctum = (CoralReefs.SpirePosition == Vector2.Zero);

            BoundClause((int i, int j) =>
            {
                if (WorldGen.genRand.Next(100) == 0)
                {
                    WorldGen.TileRunner(i, j, Main.rand.Next(10, 20), Main.rand.Next(10, 20), ModContent.TileType<GemsandstoneTile>(), false, 0, 0, false, true);
                }
            });

            RemoveStoneSlabs();

            //Spawning bamboo geodes

            int maxIterations = 0;

            Point boundTL = (TL.ToVector2() + (Vector2.One * 30)).ToPoint();
            Point boundBR = (BR.ToVector2() + (Vector2.One * -30)).ToPoint();

            Point attemptPos = new Point(WorldGen.genRand.Next(boundTL.X, boundBR.X), WorldGen.genRand.Next(boundTL.Y, boundBR.Y));

            while (geodeLocations.Count < 15 && maxIterations < 4000)
            {
                if (Vector2.DistanceSquared(attemptPos.ToVector2(), CoralReefs.SpirePosition + new Vector2(0, -13)) > 80 * 80 && NoAdjacentGeodes(attemptPos.ToVector2()) && Bounds.Contains(attemptPos))
                {
                    MakeBambooGeode(attemptPos.X, attemptPos.Y);

                    geodeLocations.Add(attemptPos.ToVector2());

                    attemptPos = (attemptPos.ToVector2() + new Vector2(WorldGen.genRand.Next(-50, 50), WorldGen.genRand.Next(-50, 50))).ToPoint();
                }
                else
                {
                    attemptPos = new Point(WorldGen.genRand.Next(boundTL.X, boundBR.X), WorldGen.genRand.Next(boundTL.Y, boundBR.Y));
                }

                maxIterations++;
            }

            for (int i = 0; i < geodeLocations.Count; i++)
            {
                Vector2 geode = geodeLocations[i];

                int junctions = 0;

                for(int j = i; j < geodeLocations.Count; j++)
                {
                    Vector2 adjGeode = geodeLocations[j];

                    if (geode == adjGeode) continue;

                    if (Vector2.Distance(geode, adjGeode) > 30 && Vector2.Distance(geode, adjGeode) < 80)
                    {
                        MakeWavyChasm3(geode, adjGeode, TileID.StoneSlab, 20, 2, true, new Vector2(5, 5));

                        junctions++;
                    }

                    if (junctions >= 2) break;
                }
            }

            Point closestPoint = Point.Zero;
            for (int i = 0; i < geodeLocations.Count; i++)
            {
                if(Vector2.Distance(geodeLocations[i], CoralReefs.SpirePosition + new Vector2(0, -13)) < Vector2.Distance(closestPoint.ToVector2(), CoralReefs.SpirePosition + new Vector2(0, -13)) && Math.Abs(CoralReefs.SpirePosition.Y - 13 - geodeLocations[i].Y) / Math.Abs(CoralReefs.SpirePosition.X - geodeLocations[i].X) <= 0.5f)
                {
                    closestPoint = geodeLocations[i].ToPoint();
                }
            }

            if(hasSanctum) MakeWavyChasm3(closestPoint.ToVector2(), CoralReefs.SpirePosition + new Vector2(0, -13), TileID.StoneSlab, 20, 2, true, new Vector2(6, 6));

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

            /*
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
            */

            //Spawning aquamarine pillars
            BoundClause((int i, int j) =>
            {
                if ((WorldGen.InWorld(i, j) && WorldGen.InWorld(i + 1, j) && Framing.GetTileSafely(i, j).TileType == ModContent.TileType<GemsandTile>() || Framing.GetTileSafely(i, j).TileType == ModContent.TileType<AquamarineTile>()) && !Framing.GetTileSafely(i, j + 1).HasTile && WorldGen.genRand.NextBool(10))
                {
                    int newJ = j + 1;

                    while (!Framing.GetTileSafely(i, newJ).HasTile)
                    {
                        WorldGen.PlaceTile(i, newJ, ModContent.TileType<AquamarinePillar>());

                        newJ++;
                    }
                }
            });

            //Spawning chimes
            BoundClause((int i, int j) =>
            {
                if ((Main.tile[i, j].TileType == ModContent.TileType<GemsandTile>() || Main.tile[i, j].TileType == ModContent.TileType<GemsandstoneTile>() || Main.tile[i, j].TileType == ModContent.TileType<AquamarineTile>()) && !Main.tile[i, j + 1].HasTile && WorldGen.genRand.NextBool(10))
                {
                    int newJ = j + 1;
                    int length = WorldGen.genRand.Next(2, 8);

                    while (!Main.tile[i, newJ].HasTile && newJ - j < length)
                    {
                        WorldGen.PlaceTile(i, newJ, ModContent.TileType<AquamarineChime>());

                        newJ++;
                    }
                }
            });

            //Spawning lamps
            BoundClause((int i, int j) =>
            {
                if(TileCheck2(i, j) == 2 && WorldGen.genRand.NextBool(10))
                {
                    if(WorldGen.genRand.NextBool())
                        WorldGen.PlaceTile(i, j - 3, ModContent.TileType<AquamarineLamp1>());
                    else
                        WorldGen.PlaceTile(i, j - 4, ModContent.TileType<AquamarineLamp2>());
                }
            });

            //Placing spire
            if (CoralReefs.SpirePosition == Vector2.Zero)
            {
                CoralReefs.SpirePosition = new Vector2(TL.X + BR.X, TL.Y + BR.Y) / 2f;

                MakeOval(80, 30, CoralReefs.SpirePosition + new Vector2(0, -13) + new Vector2(-80 / 2, -30 / 2), TileID.StoneSlab, true);

                ClearRegion(20, 20, CoralReefs.SpirePosition + new Vector2(-10, -23));

                MakeOval(30, 14, CoralReefs.SpirePosition + new Vector2(-15, -38), ModContent.TileType<AquamarineTile>(), true);
                MakeOval(30, 14, CoralReefs.SpirePosition + new Vector2(-15, -5), ModContent.TileType<AquamarineTile>(), true);
            }
        }

        public void MakeBambooGeode(int i, int j)
        {
            MakeOval(24, 24, new Vector2(i - 12, j - 12), ModContent.TileType<AquamarineTile>(), true);

            MakeOval(14, 14, new Vector2(i - 7, j - 7), TileID.StoneSlab, true);

            MakeWallCircle(24, new Vector2(i - 12, j - 12), ModContent.WallType<DarkGemsandstoneWallTile>(), true);

            RemoveStoneSlabs();

            for(int i2 = i - 7; i2 < i + 7; i2++)
            {
                for (int j2 = j - 7; j2 < j + 7; j2++)
                {
                    if ((TileCheck2(i2, j2) == 2) && WorldGen.genRand.NextBool())
                    {
                        CoralReefs.ThinCrystalBambooLocations.Add(new Vector2(i2, j2));

                        Vector2 lastPos = CoralReefs.ThinCrystalBambooLocations[CoralReefs.ThinCrystalBambooLocations.Count - 1];

                        int length = Main.rand.Next(2, 6);
                        Vector2 rotVec = new Vector2(length, 0).RotatedBy(Main.rand.NextFloat((MathHelper.PiOver2 * 3) - 0.2f, (MathHelper.PiOver2 * 3) + 0.2f));
                        CoralReefs.ThinCrystalBambooLocations.Add(lastPos + rotVec);
                    }

                    if ((TileCheck2(i2, j2) == 1) && WorldGen.genRand.NextBool())
                    {
                        CoralReefs.ThinCrystalBambooLocations.Add(new Vector2(i2, j2));

                        Vector2 lastPos = CoralReefs.ThinCrystalBambooLocations[CoralReefs.ThinCrystalBambooLocations.Count - 1];

                        int length = Main.rand.Next(2, 6);
                        Vector2 rotVec = new Vector2(length, 0).RotatedBy(Main.rand.NextFloat((MathHelper.PiOver2) - 0.2f, (MathHelper.PiOver2) + 0.2f));
                        CoralReefs.ThinCrystalBambooLocations.Add(lastPos + rotVec);
                    }
                }
            }
        }

        public bool NoAdjacentGeodes(Vector2 location)
        {
            foreach(Vector2 vec in geodeLocations)
            {
                if (Vector2.Distance(vec, location) < 30)
                    return false;
            }

            return true;
        }
    }
}