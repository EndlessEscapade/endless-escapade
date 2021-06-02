using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class PlaceWallAction : BasePlacementActionWithEntry
    {
        public PlaceWallAction(ushort entry) : base(entry)
        {
        }

        public override ushort Flag { get; }

        public override void Place(ref int i, ref int j, Structure structure,
            ref List<(Point, ushort, ushort, ushort)> deferredMultitiles)
        {
            WorldGen.PlaceWall(i, j, structure.EntryToWallID[EntryData], true);
            i++;
        }
    }
}