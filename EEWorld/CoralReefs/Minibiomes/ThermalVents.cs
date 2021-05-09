using EEMod.Tiles;
using EEMod.Tiles.Foliage;
using EEMod.Tiles.Foliage.ThermalVents;
using EEMod.Tiles.Walls;
using EEMod.VerletIntegration;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static EEMod.EEWorld.EEWorld;

namespace EEMod.EEWorld
{
    public class ThermalVents : CoralReefMinibiome
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
                    WorldGen.TileRunner(i, j, Main.rand.Next(10, 20), Main.rand.Next(10, 20), ModContent.TileType<ScorchedGemsandTile>(), false, 0, 0, false, true);
                }
            });

            BoundClause((int i, int j) =>
            {
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

            TilePopulate(new int[] {
                    ModContent.TileType<ThermalVent1x1>(),
                    ModContent.TileType<ThermalVent2x1>(),
                    ModContent.TileType<ThermalVent2x2>(),
                    ModContent.TileType<ThermalVent3x3>(), },
            new Rectangle(TL.X, TL.Y, TL.X + Size.X, TL.Y + Size.Y));
        }
    }
}