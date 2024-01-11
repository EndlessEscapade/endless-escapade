using System;
using EndlessEscapade.Utilities;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ModLoader;

namespace EndlessEscapade.Common.Tweaks;

[Autoload(Side = ModSide.Client)]
public sealed class BeachTweaks : ILoadable
{
    void ILoadable.Load(Mod mod) {
        MonoModHooks.Modify(WorldGenUtils.GetVanillaWorldgenPassDelegate("Beaches").Method, AddGenPassesPatch);
    }

    void ILoadable.Unload() { }
    
    // Prevents shallow beach endings from generating in favor of Shipyard generation.
    private static void AddGenPassesPatch(ILContext il) {
        try {
            var c = new ILCursor(il);

            if (!c.TryGotoNext(MoveType.Before, i => i.MatchLdcI4(1), i => i.MatchStloc(1)))    {
                EndlessEscapade.Instance.Logger.Warn($"{nameof(BeachTweaks)} disabled: Failed to match IL.");
                return;
            }

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
