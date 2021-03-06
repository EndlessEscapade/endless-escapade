using EEMod.Tiles.Walls;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static EEMod.EEWorld.EEWorld;

namespace EEMod.EEWorld
{
    public class BulbousGrove : CoralReefMinibiome
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
            int tile2;
            tile2 = (ushort)GetGemsandType((int)TL.Y);

            MakeJaggedOval(Size.X, Size.Y, new Vector2(TL.X, TL.Y), TileID.StoneSlab, true, 100);

            for (int i = 0; i < 20; i++)
            {
                MakeCircle(WorldGen.genRand.Next(5, 20), new Vector2(TL.X + WorldGen.genRand.Next(Size.X), TL.Y + WorldGen.genRand.Next(Size.Y)), tile2, true);
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
                            if (Main.tile[i + k, j + l].active() && Main.tileSolid[Main.tile[i + k, j + l].type])
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
            BoundClause((int i, int j) =>
            {
                int buffer = 0;
                for (int a = 0; a < 20; a++)
                {
                    if (WorldGen.InWorld(i, j - a, 10))
                        if (Main.tile[i, j - a].active())
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