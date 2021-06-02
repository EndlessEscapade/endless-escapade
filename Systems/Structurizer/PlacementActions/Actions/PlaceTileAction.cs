using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class PlaceTileAction : BasePlacementActionWithEntry
    {

        public PlaceTileAction(ushort entry) : base(entry)
        {
        }

        // Unused to save space
        public override ushort Flag { get; }

        public override void Place(ref int i, ref int j, Structure structure,
            ref List<(Point, ushort, ushort, ushort)> deferredMultitiles)
        {
            WorldGen.PlaceTile(i, j, structure.EntryToTileID[EntryData], true, true);
            i++;
        }
    }
}