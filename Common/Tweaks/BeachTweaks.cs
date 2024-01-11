using System;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Tweaks;

public sealed class BeachTweaks : ILoadable
{
    void ILoadable.Load(Mod mod) {
        IL_WorldGen.AddGenPasses += AddGenPassesPatch;
    }

    void ILoadable.Unload() { }
    
    // Prevents shallow beach endings from generating in favor of Shipyard generation.
    private static void AddGenPassesPatch(ILContext il) {
        try {
            var c = new ILCursor(il);

            if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(1), i => i.MatchStloc(0))) {
                EndlessEscapade.Instance.Logger.Warn($"{nameof(BeachTweaks)} disabled: Failed to match IL.");
                return;
            }

            c.Emit(OpCodes.Ldc_I4_0);
            c.Emit(OpCodes.Stloc_1);
        }
        catch (Exception exception) {
            MonoModHooks.DumpIL(EndlessEscapade.Instance, il);
        }
    }
}
