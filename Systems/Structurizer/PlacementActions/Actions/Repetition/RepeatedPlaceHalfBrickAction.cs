using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace EEMod.Systems.Structurizer.PlacementActions.Actions.Repetition
{
    public class RepeatedPlaceHalfBrickAction : BaseRepeatedPlacementActionWithEntry<PlaceHalfBrickAction>
    {
        public RepeatedPlaceHalfBrickAction(ushort repetitionCount, ushort entryData) : base(repetitionCount, entryData)
        {
        }

        public override ushort Flag => 0xFFED;

        public override PlaceHalfBrickAction PlacementAction => new PlaceHalfBrickAction(EntryData);

        public override void Place(ref int i, ref int j, Structure structure,
            ref List<(Point, ushort, ushort, ushort)> deferredMultitiles)
        {
            for (int z = i; z < i + RepetitionCount; z++)
                if (WorldGen.PlaceTile(z, j, structure.EntryToTileID[EntryData], true))
                    Main.tile[z, j].halfBrick(true);

            i += RepetitionCount;
        }
    }
}