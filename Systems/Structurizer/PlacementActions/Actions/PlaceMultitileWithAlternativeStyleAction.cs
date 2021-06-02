using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class PlaceMultitileWithAlternativeStyleAction : BasePlacementActionWithEntryAndStyleAlternative
    {
        public PlaceMultitileWithAlternativeStyleAction(ushort entry, byte styleData, byte alternateStyleData) : base(
            entry, styleData, alternateStyleData)
        {
        }

        public override ushort Flag => 0xFFF4;

        public override void Place(ref int i, ref int j, Structure structure,
            ref List<(Point, ushort, ushort, ushort)> deferredMultitiles)
        {
            deferredMultitiles.Add((new Point(i, j), structure.EntryToTileID[EntryData], StyleData,
                AlternateStyleData));
            i++;
        }
    }
}