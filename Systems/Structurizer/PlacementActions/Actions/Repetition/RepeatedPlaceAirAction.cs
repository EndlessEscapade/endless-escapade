using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace EEMod.Systems.Structurizer.PlacementActions.Actions.Repetition
{
    public class RepeatedPlaceAirAction : BaseRepeatedPlacementAction<PlaceAirAction>
    {
        public RepeatedPlaceAirAction(ushort repetitionCount) : base(repetitionCount)
        {
        }

        public override ushort Flag => 0xFFFF;

        public override PlaceAirAction PlacementAction => new PlaceAirAction();

        public override void Place(ref int i, ref int j, Structure structure,
            ref List<(Point, ushort, ushort, ushort)> deferredMultitiles)
        {
            for (int z = i; z < i + RepetitionCount; z++)
                WorldGen.KillTile(z, j, false, noItem: true);

            i += RepetitionCount;
        }
    }
}