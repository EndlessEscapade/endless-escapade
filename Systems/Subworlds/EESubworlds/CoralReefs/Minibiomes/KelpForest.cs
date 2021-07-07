using EEMod.Systems.Subworlds.EESubworlds;
using EEMod.Tiles;
using EEMod.Tiles.Foliage;
using EEMod.Tiles.Foliage.KelpForest;
using EEMod.Tiles.Walls;
using EEMod.VerletIntegration;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Systems.Noise;
using EEMod.ID;
using static EEMod.EEWorld.EEWorld;

namespace EEMod.EEWorld
{
    public class KelpForest : CoralReefMinibiome
    {
        public override void FoliageStep()
        {

        }

        public delegate void InOvalEvent(int i, int j);

        public override MinibiomeID id => MinibiomeID.KelpForest;

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

            CoralReefs.MakeNoiseOval(Size.X, Size.Y, new Vector2(TL.X, TL.Y), TileID.StoneSlab, true, 50);
            CoralReefs.CreateNoise(!EnsureNoise, Position, Size, 50, 50, 0.4f);
            CoralReefs.CreateNoise(!EnsureNoise, Position, Size, 20, 20, 0.4f);
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

            CoralReefs.perlinNoise = new PerlinNoiseFunction(Bounds.Width + 1, Bounds.Height + 1, 50, 50, 0.5f);
            int[,] perlinNoiseFunction = CoralReefs.perlinNoise.perlinBinary;

            BoundClause((int i, int j) =>
            {
                if (perlinNoiseFunction[i - TL.X, j - TL.Y] == 1 && WorldGen.InWorld(i, j))
                {
                    if ((Framing.GetTileSafely(i, j).type == ModContent.TileType<KelpLeafTile>()))
                        Framing.GetTileSafely(i, j).type = (ushort)ModContent.TileType<KelpMossTile>();
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
            });

            BoundClause((int i, int j) =>
                   {
                       if (WorldGen.InWorld(i, j))
                       {
                           if (TileCheck2(i, j) != 0 && Main.rand.NextBool(8))
                           {
                               if (CoralReefs.GiantKelpRoots.Count == 0)
                               {
                                   CoralReefs.GiantKelpRoots.Add(new Vector2(i, j));
                               }
                               else
                               {
                                   Vector2 lastPos = CoralReefs.GiantKelpRoots[CoralReefs.GiantKelpRoots.Count - 1];
                                   if ((Vector2.DistanceSquared(lastPos, new Vector2(i, j)) > 10 * 10 && Vector2.DistanceSquared(lastPos, new Vector2(i, j)) < 110 * 110) || Vector2.DistanceSquared(lastPos, new Vector2(i, j)) > 200 * 200)
                                   {
                                       CoralReefs.GiantKelpRoots.Add(new Vector2(i, j));
                                   }
                               }
                           }
                       }
                   });

            BoundClause((int i, int j) =>
            {
                int websClose = 0;
                for (int m = 0; m < CoralReefs.WebPositions.Count; m++)
                {
                    if (Vector2.DistanceSquared(new Vector2(i, j), CoralReefs.WebPositions[m]) < 200 * 200)
                    {
                        websClose++;
                    }
                }
                if (websClose == 0)
                {
                    CoralReefs.WebPositions.Add(new Vector2(i, j));
                }
            });

            BoundClause((int i, int j) =>
            {
                if (TileCheck2(i, j) == 2 && Main.rand.NextBool(20))
                {
                    WorldGen.PlaceTile(i, j - 4, ModContent.TileType<KelpFlower>(), default, default, default, default);
                    ModTileEntity.PlaceEntityNet(i, j - 4, ModContent.TileEntityType<KelpFlowerTE>());
                }
            });

            TilePopulate(new int[] {
                    ModContent.TileType<GlowHangCoral1>(),
                    ModContent.TileType<GroundGlowCoral>(),
                    ModContent.TileType<GroundGlowCoral2>(),
                    ModContent.TileType<GroundGlowCoral3>(),
                    ModContent.TileType<GroundGlowCoral4>(),
                    ModContent.TileType<Wall4x3CoralL>(),
                    ModContent.TileType<Wall4x3CoralR>() },
            new Rectangle(TL.X, TL.Y, TL.X + Size.X, TL.Y + Size.Y));

            BoundClause((int i, int j) =>
            {
                Tile tile = Framing.GetTileSafely(i, j);
                if (TileCheck2(i, j) == 1 && Main.rand.NextBool(30) && (
                    tile.type == ModContent.TileType<GemsandTile>()
                    || tile.type == ModContent.TileType<LightGemsandTile>()
                    || tile.type == ModContent.TileType<DarkGemsandTile>()
                    || tile.type == ModContent.TileType<LightGemsandstoneTile>()
                    || tile.type == ModContent.TileType<GemsandstoneTile>()
                    || tile.type == ModContent.TileType<DarkGemsandstoneTile>()
                    || tile.type == ModContent.TileType<KelpLeafTile>()
                    || tile.type == ModContent.TileType<KelpMossTile>()))
                {
                    VerletHelpers.AddStickChainNoAdd(ref ModContent.GetInstance<EEMod>().verlet, new Vector2(i * 16, j * 16), Main.rand.Next(5, 10), 27);
                }
            });

            BoundClause((int i, int j) =>
            {
                Tile tile = Framing.GetTileSafely(i, j);
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