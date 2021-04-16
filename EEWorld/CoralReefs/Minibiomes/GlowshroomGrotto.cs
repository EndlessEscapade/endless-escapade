using EEMod.Tiles.Walls;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Tiles.Foliage.GlowshroomGrotto;
using static EEMod.EEWorld.EEWorld;

namespace EEMod.EEWorld
{
    public class GlowshroomGrotto : CoralReefMinibiome
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
        
        public void MakeStraightChasm(Vector2 pos1, Vector2 pos2, int width)
        {
            Vector2 Perpendicular = Vector2.Normalize((pos1 - pos2).RotatedBy(1.57f));
            for (float i = 0; i < 1; i += 1 / pos1.Length())
            {
                Vector2 lerp = Vector2.Lerp(pos1, pos2, i);
                WorldGen.KillTile((int)lerp.X, (int)lerp.Y);
                for (int j = -width; j < width; j++)
                {
                    Vector2 altP1 = lerp + Perpendicular * j;
                    WorldGen.KillTile((int)altP1.X, (int)altP1.Y);
                }
            }
        }

        public override void StructureStep()
        {
            Point TL = Bounds.TopLeft().ToPoint();
            Point BR = Bounds.BottomRight().ToPoint();
            int tile2;
            tile2 = (ushort)GetGemsandType((int)TL.Y);

            Vector2[] poses = MakeDistantLocations(20,30,Bounds);
            //FillRegion(Bounds.Width, Bounds.Height, Position.ToVector2(), TileID.Dirt);
            for (int i = 0; i < poses.Length; i++)
            {
                if (i != 0)
                {
                    MakeCircleFromCenter(WorldGen.genRand.Next(20, 30), poses[i], TileID.StoneSlab, true);
                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        MakeStraightChasm(poses[0], poses[i], WorldGen.genRand.Next(3, 5));
                    }
                    Vector2 c = FindClosest(poses[i], poses);
                    MakeStraightChasm(c, poses[i], WorldGen.genRand.Next(3, 5));
                }
                else
                    MakeCircleFromCenter(WorldGen.genRand.Next(40, 50), poses[i], TileID.StoneSlab, true);
            }
            RemoveStoneSlabs();
            BoundClause((int i, int j) =>
            {
                int noOfTiles = 0;
                for (int k = -5; k < 5; k++)
                {
                    for (int l = -5; l < 5; l++)
                    {
                        if (WorldGen.InWorld(i + k, j + l, 10))
                        {
                            if (Framing.GetTileSafely(i + k, j + l).active() && Main.tileSolid[Framing.GetTileSafely(i + k, j + l).type])
                            {
                                noOfTiles++;
                            }
                        }
                    }
                }
                if (EESubWorlds.BulbousTreePosition.Count > 0)
                {
                    for (int m = 0; m < EESubWorlds.BulbousTreePosition.Count; m++)
                    {
                        if (Vector2.DistanceSquared(new Vector2(i, j), EESubWorlds.BulbousTreePosition[m]) < 45 * 45)
                        {
                            noOfTiles += 5;
                        }
                    }
                }
                if (EESubWorlds.OrbPositions.Count > 0)
                {
                    for (int m = 0; m < EESubWorlds.OrbPositions.Count; m++)
                    {
                        if (Vector2.DistanceSquared(new Vector2(i, j), EESubWorlds.OrbPositions[m]) < 20 * 20)
                        {
                            noOfTiles += 5;
                        }
                    }
                }
                if (noOfTiles < 3)
                {
                    EESubWorlds.BulbousTreePosition.Add(new Vector2(i, j));
                }
            });

            TilePopulate(new int[] {
                    ModContent.TileType<OrangeMushroom1x1>(),
                    ModContent.TileType<OrangeMushroom2x2>(),
                    ModContent.TileType<OrangeMushroom3x5>(),
                    ModContent.TileType<OrangeMushroom5x7>(),
                    ModContent.TileType<OrangeMushroom8x11>(), },
            new Rectangle(TL.X, TL.Y, TL.X + Size.X, TL.Y + Size.Y));

            BoundClause((int i, int j) =>
            {
                int buffer = 0;
                for (int a = 0; a < 20; a++)
                {
                    if (WorldGen.InWorld(i, j - a, 10))
                        if (Framing.GetTileSafely(i, j - a).active())
                        {
                            buffer++;
                        }
                }
                if (buffer < 17 && buffer > 3)
                {
                    if (TileCheck2(i, j) == 1 && TileCheckVertical(i, j + 1, 1) - (j + 1) <= 50)
                    {
                        for (int a = 0; a < TileCheckVertical(i, j + 1, 1) - (j + 1); a++)
                        {
                            if (Main.rand.Next(2) == 1)
                            {
                                WorldGen.PlaceWall(i, j + a, ModContent.WallType<GemsandstoneWallTile>());
                            }
                        }
                    }
                }
            }
            );
        }
    }
}