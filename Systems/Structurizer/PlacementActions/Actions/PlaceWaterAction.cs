using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class PlaceWaterAction : BasePlacementActionWithLiquid
    {
        public PlaceWaterAction(byte liquidData) : base(liquidData)
        {
        }

        public override ushort Flag => 0xFFFB;

        public override void Place(ref int i, ref int j, Structure structure,
            ref List<(Point, ushort, ushort, ushort)> deferredMultitiles)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            tile.liquidType(0);
            tile.liquid = LiquidData;
        }
    }
}