using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace EEMod.Systems.Structurizer.PlacementActions.Actions.Repetition
{
    public class RepeatedPlaceWallAction : BaseRepeatedPlacementActionWithEntry<PlaceWallAction>
    {
        public RepeatedPlaceWallAction(ushort repetitionCount, ushort entryData) : base(repetitionCount, entryData)
        {
        }

        public override ushort Flag => 0xFFF3;

        public override PlaceWallAction PlacementAction => new PlaceWallAction(EntryData);

        public override void Place(ref int i, ref int j, Structure structure,
            ref List<(Point, ushort, ushort, ushort)> deferredMultitiles)
        {
            for (int z = i; z < i + RepetitionCount; z++)
                WorldGen.PlaceWall(z, j, structure.EntryToWallID[EntryData], true);

            i += RepetitionCount;
        }
    }
}