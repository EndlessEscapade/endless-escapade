using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace EEMod.Systems.Structurizer.PlacementActions.Actions.Repetition
{
    public class RepeatedPlaceTileAction : BaseRepeatedPlacementActionWithEntry<PlaceTileAction>
    {
        public RepeatedPlaceTileAction(ushort repetitionCount, ushort entryData) : base(repetitionCount, entryData)
        {
        }

        public override ushort Flag => 0xFFFD;

        public override PlaceTileAction PlacementAction => new PlaceTileAction(EntryData);

        public override void Place(ref int i, ref int j, Structure structure,
            ref List<(Point, ushort, ushort, ushort)> deferredMultitiles)
        {
            for (int z = i; z < i + RepetitionCount; z++)
                WorldGen.PlaceTile(z, j, structure.EntryToTileID[EntryData], true, true);

            i += RepetitionCount;
        }
    }
}