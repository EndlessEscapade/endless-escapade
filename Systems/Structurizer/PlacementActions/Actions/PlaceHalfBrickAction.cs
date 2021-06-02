using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace EEMod.Systems.Structurizer.PlacementActions.Actions
{
    public class PlaceHalfBrickAction : BasePlacementActionWithEntry
    {
        public PlaceHalfBrickAction(ushort entry) : base(entry)
        {
        }

        public override ushort Flag => 0xFFEE;

        public override void Place(ref int i, ref int j, Structure structure,
            ref List<(Point, ushort, ushort, ushort)> deferredMultitiles)
        {
            if (WorldGen.PlaceTile(i, j, structure.EntryToTileID[EntryData]))
                Main.tile[i, j].halfBrick(true);

            i++;
        }
    }
}