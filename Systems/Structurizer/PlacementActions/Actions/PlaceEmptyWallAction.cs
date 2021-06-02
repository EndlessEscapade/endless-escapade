using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class PlaceEmptyWallAction : BasePlacementAction
    {
        public override ushort Flag => 0xFFF2;

        public override void Place(ref int i, ref int j, Structure structure,
            ref List<(Point, ushort, ushort, ushort)> deferredMultitiles)
        {
            WorldGen.KillWall(i, j);
            i++;
        }
    }
}