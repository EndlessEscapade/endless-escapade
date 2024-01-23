using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace EndlessEscapade.Common.WorldBuilding;

[Autoload(Side = ModSide.Client)]
public sealed class Florida : ILoadable
{
    void ILoadable.Load(Mod mod) {
        var passesFieldInfo = typeof(WorldGen).GetField("_vanillaGenPasses", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        var passes = (Dictionary<string, GenPass>)passesFieldInfo.GetValue(null);

        var methodInfo = typeof(PassLegacy).GetField("_method", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        var method = (WorldGenLegacyMethod)methodInfo.GetValue(passes["Beaches"]);

        // This patch removes shallow and steep beaches on the left side of the world.
        MonoModHooks.Modify(method.Method, AddGenPassesPatch);
    }

    void ILoadable.Unload() { }
    
    private static void AddGenPassesPatch(ILContext il) {
        try {
            var c = new ILCursor(il);

            if (!c.TryGotoNext(i => i.MatchLdcI4(1))) {
                EndlessEscapade.Instance.Logger.Warn($"{nameof(Florida)} disabled: Failed to match IL instruction: {nameof(OpCodes.Ldc_I4_1)}");
                return;
            }

            if (!c.TryGotoNext(i => i.MatchStloc(1))) {
                EndlessEscapade.Instance.Logger.Warn($"{nameof(Florida)} disabled: Failed to match IL instruction: {nameof(OpCodes.Stloc_1)}");
                return;
            }

            c.Index--;

            var label = c.DefineLabel();

            c.EmitDelegate(() => false);

            c.Emit(OpCodes.Brfalse, label);
            c.Emit(OpCodes.Ret);
            c.Emit(OpCodes.Nop);

            c.MarkLabel(label);
        }
        catch (Exception) {
            MonoModHooks.DumpIL(EndlessEscapade.Instance, il);
        }
    }
}
