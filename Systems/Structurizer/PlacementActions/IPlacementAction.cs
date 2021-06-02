using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace EEMod.Systems.Structurizer.PlacementActions
{
    public interface IPlacementAction
    {
        ushort Flag { get; }

        ushort EntryData { get; }

        byte StyleData { get; } // Used for slopes/slope type too

        byte AlternateStyleData { get; }

        void Place(ref int i, ref int j, Structure structure,
            ref List<(Point, ushort, ushort, ushort)> deferredMultitiles);
    }
}