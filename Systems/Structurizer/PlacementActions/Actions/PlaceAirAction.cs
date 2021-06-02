using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class PlaceAirAction : BasePlacementAction
    {
        public override ushort Flag => 0xFFFE;

        public override void Place(ref int i, ref int j, Structure structure,
            ref List<(Point, ushort, ushort, ushort)> deferredMultitiles)
        {
            WorldGen.KillTile(i, j, false, noItem: true);
            i++;
        }
    }
}