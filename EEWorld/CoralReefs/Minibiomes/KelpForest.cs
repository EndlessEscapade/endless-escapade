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

namespace EEMod.EEWorld
{
    public class KelpForest : CoralReefMinibiome
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

            MakeNoiseOval(Size.X, Size.Y, new Vector2(TL.X, TL.Y), TileID.StoneSlab, true, 50);
            CreateNoise(!EnsureNoise, Position, Size, 50, 50, 0.4f);
            CreateNoise(!EnsureNoise, Position, Size, 20, 20, 0.4f);
            RemoveStoneSlabs();
            BoundClause((int i, int j) =>
            {
                if (WorldGen.genRand.Next(100) == 0)
                {
                    WorldGen.TileRunner(i, j, Main.rand.Next(10, 20), Main.rand.Next(10, 20), ModContent.TileType<KelpRockTile>(), false, 0, 0, false, true);
                }
            });

            BoundClause((int i, int j) =>
            {
                bool CorrectSpacing = TileCheck2(i, j) == (int)TileSpacing.Top;
                if (CorrectSpacing)
                {
                    for (int a = 5; a < 5 + Main.rand.Next(1, 4); a++)
                    {
                        WorldGen.PlaceTile(i, j + a, ModContent.TileType<KelpLeafTile>(), false, true);
                    }
                    for (int a = 0; a < Main.rand.Next(1, 4); a++)
                    {
                        WorldGen.PlaceTile(i, j + a, ModContent.TileType<KelpLeafTile>(), false, true);
                    }
                }
            });

            BoundClause((int i, int j) =>
            {
                bool CorrectSpacing = TileCheck2(i, j) == (int)TileSpacing.Bottom;
                if (CorrectSpacing && Framing.GetTileSafely(i, j).type != ModContent.TileType<KelpVine>() && WorldGen.genRand.Next(5) == 0)
                {
                    for (int a = 0; a < WorldGen.genRand.Next(8, 25); a++)
                    {
                        if (!Framing.GetTileSafely(i, j + a).active())
                            WorldGen.PlaceTile(i, j + a, ModContent.TileType<KelpVine>());
                    }
                }
                int buffer = 0;
                for (int a = 0; a < 14; a++)
                {
                    if (WorldGen.InWorld(i, j - a, 10))
                        if (Framing.GetTileSafely(i, j - a).active())
                        {
                            buffer++;
                        }
                }
                if (buffer < 7)
                {
                    if (TileCheck2(i, j) == 1 && TileCheckVertical(i, j + 1, 1) - (j + 1) <= 50)
                    {
                        for (int a = 0; a < 50; a++)
                        {
                            if (Main.rand.Next(4) == 1)
                            {
                                WorldGen.PlaceWall(i, j + a, ModContent.WallType<GemsandstoneWallTile>());
                            }
                        }
                    }
                }
            });

            BoundClause((int i, int j) =>
                   {
                       if (WorldGen.InWorld(i, j))
                       {
                           if (TileCheck2(i, j) != 0 && Main.rand.NextBool(8))
                           {
                               if (EESubWorlds.GiantKelpRoots.Count == 0)
                               {
                                   EESubWorlds.GiantKelpRoots.Add(new Vector2(i, j));
                               }
                               else
                               {
                                   Vector2 lastPos = EESubWorlds.GiantKelpRoots[EESubWorlds.GiantKelpRoots.Count - 1];
                                   if ((Vector2.DistanceSquared(lastPos, new Vector2(i, j)) > 10 * 10 && Vector2.DistanceSquared(lastPos, new Vector2(i, j)) < 110 * 110) || Vector2.DistanceSquared(lastPos, new Vector2(i, j)) > 200 * 200)
                                   {
                                       EESubWorlds.GiantKelpRoots.Add(new Vector2(i, j));
                                   }
                               }
                           }
                       }
                   });

            TilePopulate(new int[] {
                    ModContent.TileType<GlowHangCoral1>(),
                    ModContent.TileType<GroundGlowCoral>(),
                    ModContent.TileType<GroundGlowCoral2>(),
                    ModContent.TileType<GroundGlowCoral3>(),
                    ModContent.TileType<GroundGlowCoral4>(),
                    ModContent.TileType<KelpFlower>(),
                    ModContent.TileType<Wall4x3CoralL>(),
                    ModContent.TileType<Wall4x3CoralR>() },
            new Rectangle(TL.X, TL.Y, TL.X + Size.X, TL.Y + Size.Y));

            BoundClause((int i, int j) =>
            {
            Tile tile = Framing.GetTileSafely(i, j);
            if (TileCheck2(i, j) == 1 && Main.rand.NextBool(20))
            {
                VerletHelpers.AddStickChain(ref ModContent.GetInstance<EEMod>().verlet, new Vector2(i * 16, j * 16), Main.rand.Next(5, 15), 27);
            }

            if (tile.active() && !Framing.GetTileSafely(i, j - 1).active() && (
                tile.type == ModContent.TileType<GemsandTile>()
                || tile.type == ModContent.TileType<LightGemsandTile>()
                || tile.type == ModContent.TileType<DarkGemsandTile>()
                || tile.type == ModContent.TileType<LightGemsandstoneTile>()
                || tile.type == ModContent.TileType<GemsandstoneTile>()
                || tile.type == ModContent.TileType<DarkGemsandstoneTile>()
                || tile.type == ModContent.TileType<KelpLeafTile>()
                || tile.type == ModContent.TileType<KelpMossTile>()))
                {
                    WorldGen.PlaceTile(i, j - 1, ModContent.TileType<GreenKelpTile>());
                }
            });
        }
    }
}