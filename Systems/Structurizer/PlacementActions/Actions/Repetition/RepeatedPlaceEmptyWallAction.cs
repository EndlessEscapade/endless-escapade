using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace EEMod.Systems.Structurizer.PlacementActions.Actions.Repetition
{
    public class RepeatedPlaceEmptyWallAction : BaseRepeatedPlacementAction
    {
        public RepeatedPlaceEmptyWallAction(ushort repetitionCount) : base(repetitionCount)
        {
        }

        public override ushort Flag => 0xFFF1;

        public override void Place(ref int i, ref int j, Structure structure,
            ref List<(Point, ushort, ushort, ushort)> deferredMultitiles)
        {
            for (int z = i; z < i + RepetitionCount; z++)
                WorldGen.KillWall(z, j);

            i += RepetitionCount;
        }
    }
}