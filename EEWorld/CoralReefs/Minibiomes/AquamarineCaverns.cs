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

            MakeNoiseOval(Size.X, Size.Y, new Vector2(TL.X, TL.Y), TileID.StoneSlab, true, 50);
            CreateNoise(!EnsureNoise, Position, Size, 50, 50, 0.4f);
            CreateNoise(!EnsureNoise, Position, Size, 20, 20, 0.4f);
            RemoveStoneSlabs();

            BoundClause((int i, int j) =>
                   {
                       if ((TileCheck2(i, j) != 0) && Main.rand.NextBool(10))
                       {
                           if (EESubWorlds.AquamarineZiplineLocations.Count == 0)
                           {
                               EESubWorlds.AquamarineZiplineLocations.Add(new Vector2(i, j));
                           }
                           else
                           {
                               Vector2 lastPos = EESubWorlds.AquamarineZiplineLocations[EESubWorlds.AquamarineZiplineLocations.Count - 1];
                               if ((Vector2.DistanceSquared(lastPos, new Vector2(i, j)) > 10 * 10 && Vector2.DistanceSquared(lastPos, new Vector2(i, j)) < 210 * 210) || Vector2.DistanceSquared(lastPos, new Vector2(i, j)) > 200 * 200)
                               {
                                   EESubWorlds.AquamarineZiplineLocations.Add(new Vector2(i, j));
                               }
                           }
                       }
                   });

            BoundClause((int i, int j) =>
            {
                if ((TileCheck2(i, j) != 0) && Main.rand.NextBool(10))
                {
                    if (EESubWorlds.AquamarineZiplineLocations.Count == 0)
                    {
                        EESubWorlds.AquamarineZiplineLocations.Add(new Vector2(i, j));
                    }
                    else
                    {
                        Vector2 lastPos = EESubWorlds.AquamarineZiplineLocations[EESubWorlds.AquamarineZiplineLocations.Count - 1];
                        if ((Vector2.DistanceSquared(lastPos, new Vector2(i, j)) > 10 * 10 && Vector2.DistanceSquared(lastPos, new Vector2(i, j)) < 210 * 210) || Vector2.DistanceSquared(lastPos, new Vector2(i, j)) > 200 * 200)
                        {
                            EESubWorlds.AquamarineZiplineLocations.Add(new Vector2(i, j));
                        }
                    }
                }
            });


            EESubWorlds.SpirePosition = new Vector2(TL.X + BR.X, TL.Y + BR.Y) / 2f;
        }
    }
}