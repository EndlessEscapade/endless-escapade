using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace EEMod.Systems.Structurizer.PlacementActions
{
    public abstract class BasePlacementAction : IPlacementAction
    {
        public abstract ushort Flag { get; }

        public virtual ushort EntryData => 0;

        public virtual byte StyleData => 0;

        public virtual byte AlternateStyleData => 0;

        public abstract void Place(ref int i, ref int j, Structure structure,
            ref List<(Point, ushort, ushort, ushort)> deferredMultitiles);
    }
}