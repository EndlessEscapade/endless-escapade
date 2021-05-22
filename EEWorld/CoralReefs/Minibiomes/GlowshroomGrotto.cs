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

            TilePopulate(new int[] {
                    ModContent.TileType<OrangeMushroom1x1>(),
                    ModContent.TileType<OrangeMushroom2x2>(),
                    ModContent.TileType<OrangeMushroom3x5>(),
                    ModContent.TileType<OrangeMushroom5x7>(),
                    ModContent.TileType<OrangeMushroom8x11>(), },
            new Rectangle(TL.X, TL.Y, TL.X + Size.X, TL.Y + Size.Y));

            BoundClause((int i, int j) =>
            {
                bool CorrectSpacing = TileCheck2(i, j) == (int)TileSpacing.Bottom;
                if (CorrectSpacing && Framing.GetTileSafely(i, j).type != ModContent.TileType<GlowshroomVines>() && WorldGen.genRand.Next(4) == 0)
                {
                    for (int a = 0; a < WorldGen.genRand.Next(4, 15); a++)
                    {
                        if (!Framing.GetTileSafely(i, j + a).active())
                            WorldGen.PlaceTile(i, j + a, ModContent.TileType<GlowshroomVines>());
                    }
                }
            });
        }
    }
}