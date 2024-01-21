using MonoMod.Cil;
using MonoMod.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.WorldBuilding;

namespace EndlessEscapade.Utilities;

// TODO: This class should not exist. World generation utilities should be built from GenAction and GenShape.
public static class WorldGenUtils
{
    public static void ExtendDownwards(int x, int y, int type) {
        while (WorldGen.InWorld(x, y) && !WorldGen.SolidTile(x, y)) {
            WorldGen.PlaceTile(x, y, type, true, true);
            WorldGen.SlopeTile(x, y);

            y++;
        }
    }
}
