using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class PlaceHoneyAction : BasePlacementActionWithLiquid
    {
        public PlaceHoneyAction(byte liquidData) : base(liquidData)
        {
        }

        public override ushort Flag => 0xFFF9;

        public override void Place(ref int i, ref int j, Structure structure,
            ref List<(Point, ushort, ushort, ushort)> deferredMultitiles)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            tile.liquidType(2);
            tile.liquid = LiquidData;
        }
    }
}