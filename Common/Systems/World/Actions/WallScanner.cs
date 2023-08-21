using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace EndlessEscapade.Common.Systems.World.Actions;

public class WallScanner : GenAction
{
    public readonly ushort[] WallTypes;

    public WallScanner(params ushort[] wallTypes) {
        WallTypes = wallTypes;
        WallLookup = new Dictionary<ushort, int>();

        for (var i = 0; i < wallTypes.Length; i++) {
            WallLookup[WallTypes[i]] = 0;
        }
    }

    public Dictionary<ushort, int> WallLookup { get; private set; }

    public override bool Apply(Point origin, int x, int y, params object[] args) {
        var tile = _tiles[x, y];
        
        if (tile.HasTile && WallLookup.ContainsKey(tile.WallType)) {
            WallLookup[tile.WallType]++;
        }

        return UnitApply(origin, x, y, args);
    }

    public WallScanner Output(Dictionary<ushort, int> resultsOutput) {
        WallLookup = resultsOutput;

        for (var i = 0; i < WallTypes.Length; i++) {
            if (!WallLookup.ContainsKey(WallTypes[i])) {
                WallLookup[WallTypes[i]] = 0;
            }
        }

        return this;
    }
}
