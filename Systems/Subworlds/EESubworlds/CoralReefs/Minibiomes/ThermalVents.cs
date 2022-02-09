using EEMod.Tiles;
using EEMod.Tiles.Foliage;
using EEMod.Tiles.Foliage.ThermalVents;
using EEMod.Tiles.Walls;
using EEMod.VerletIntegration;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EEMod.Systems.Subworlds.EESubworlds;
using EEMod.ID;
using static EEMod.EEWorld.EEWorld;

namespace EEMod.EEWorld
{
    public class ThermalVents : CoralReefMinibiome
    {
        public override void FoliageStep()
        {

        }

        public delegate void InOvalEvent(int i, int j);

        public override MinibiomeID id => MinibiomeID.ThermalVents;

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

            CoralReefs.MakeNoiseOval(Size.X, Size.Y, new Vector2(TL.X, TL.Y), TileID.StoneSlab, true, 25);

            RemoveStoneSlabs();

            BoundClause((int i, int j) =>
            {
                bool CorrectSpacing = TileCheck2(i, j) == (int)TileSpacing.Top;
                if (CorrectSpacing && WorldGen.genRand.NextBool(20) && Framing.GetTileSafely(i, j).TileType != ModContent.TileType<RustyPipeTile>())
                {
                    int pipeHeight = WorldGen.genRand.Next(3, 7);

                    for (int k = j - 1; k > j - pipeHeight; k--)
                    {
                        WorldGen.PlaceTile(i, k, ModContent.TileType<RustyPipeTile>());

                        if (k == j - pipeHeight + 1)
                        {
                            WorldGen.PlaceTile(i + WorldGen.genRand.Next(-1, 2), k, ModContent.TileType<RustyPipeTile>());
                        }
                    }
                }
            });

            //Placing down vents
            TilePopulate(new int[] {
                    ModContent.TileType<ThermalVent1x1>(),
                    ModContent.TileType<ThermalVent2x1>(),
                    ModContent.TileType<ThermalVent2x2>(),
                    ModContent.TileType<ThermalVent3x3>(),
                    ModContent.TileType<ThermalVent5x4>(),
                    ModContent.TileType<ThermalVent5x5>(),},
            new Rectangle(TL.X, TL.Y, TL.X + Size.X, TL.Y + Size.Y));
        }
    }
}