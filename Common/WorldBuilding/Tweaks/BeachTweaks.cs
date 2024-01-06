using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.WorldBuilding.Tweaks;

// Prevents shallow beach endings from generating in favor of Shipyard generation.
public sealed class BeachTweaks : ILoadable
{
    void ILoadable.Load(Mod mod) {
        IL_WorldGen.AddGenPasses += AddGenPassesPatch;
    }

    void ILoadable.Unload() { }

    private static void AddGenPassesPatch(ILContext il) {
        var c = new ILCursor(il);

        if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(1), i => i.MatchStloc(0))) {
            return;
        }

        c.Emit(OpCodes.Ldc_I4_0);
        c.Emit(OpCodes.Stloc_1);
    }
}
