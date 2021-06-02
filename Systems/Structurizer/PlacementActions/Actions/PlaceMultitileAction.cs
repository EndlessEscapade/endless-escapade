using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class PlaceMultitileAction : BasePlacementActionWithEntry
    {
        public PlaceMultitileAction(ushort entry) : base(entry)
        {
        }

        public override ushort Flag => 0xFFFC;

        public override void Place(ref int i, ref int j, Structure structure,
            ref List<(Point, ushort, ushort, ushort)> deferredMultitiles)
        {
            deferredMultitiles.Add((new Point(i, j), structure.EntryToTileID[EntryData], 0, 0));
            i++;
        }
    }
}