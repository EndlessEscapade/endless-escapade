using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class PlaceMultitileWithStyle : BasePlacementActionWithEntryAndStyle
    {
        public PlaceMultitileWithStyle(ushort entry, byte styleData) : base(entry, styleData)
        {
        }

        public override ushort Flag => 0xFFF5;

        public override void Place(ref int i, ref int j, Structure structure,
            ref List<(Point, ushort, ushort, ushort)> deferredMultitiles)
        {
            deferredMultitiles.Add((new Point(i, j), structure.EntryToTileID[EntryData], StyleData, 0));
            i++;
        }
    }
}