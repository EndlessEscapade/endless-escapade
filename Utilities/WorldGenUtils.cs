using MonoMod.Cil;
using MonoMod.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.WorldBuilding;

namespace EndlessEscapade.Utilities;

public static class WorldGenUtils
{
    public static void ExtendDownwards(int x, int y, int type) {
        while (WorldGen.InWorld(x, y) && !WorldGen.SolidTile(x, y)) {
            WorldGen.PlaceTile(x, y, type, true, true);
            WorldGen.SlopeTile(x, y);

            y++;
        }
    }

    internal static WorldGenLegacyMethod GetVanillaWorldgenPassDelegate(string passName) {
        var vanillaPasses = (Dictionary<string, GenPass>)typeof(WorldGen).GetField("_vanillaGenPasses", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
        return (WorldGenLegacyMethod)typeof(PassLegacy).GetField("_method", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(vanillaPasses[passName]);
    }
}
