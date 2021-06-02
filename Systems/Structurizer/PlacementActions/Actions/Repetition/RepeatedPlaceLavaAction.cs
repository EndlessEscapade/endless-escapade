using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace EEMod.Systems.Structurizer.PlacementActions.Actions.Repetition
{
    public class RepeatedPlaceLavaAction : BaseRepeatedPlacementActionWithLiquid
    {
        public RepeatedPlaceLavaAction(ushort repetitionCount, byte liquidData) : base(repetitionCount, liquidData)
        {
        }

        public override ushort Flag => 0xFFF7;
        
        public override void Place(ref int i, ref int j, Structure structure,
            ref List<(Point, ushort, ushort, ushort)> deferredMultitiles)
        {
            for (int z = i; z < i + RepetitionCount; z++)
            {
                Tile tile = Framing.GetTileSafely(z, j);
                tile.liquidType(1);
                tile.liquid = LiquidData;
            }

            i += RepetitionCount;
        }
    }
}