using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class PlaceSlopeAction : BasePlacementActionWithEntryAndStyle
    {
        public PlaceSlopeAction(ushort entry, byte slope) : base(entry, slope)
        {
        }

        public override ushort Flag => 0xFFF0;

        public override void Place(ref int i, ref int j, Structure structure,
            ref List<(Point, ushort, ushort, ushort)> deferredMultitiles)
        {
            if (WorldGen.PlaceTile(i, j, structure.EntryToTileID[EntryData], true))
                Main.tile[i, j].slope(StyleData);

            i++;
        }
    }
}